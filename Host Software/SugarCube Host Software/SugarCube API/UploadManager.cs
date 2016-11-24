/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web;

using log4net;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System.Data.SQLite;

namespace Me.ThreeDWares.SugarCube {

	#region Custom Exceptions
	/// <summary>
	/// An exception that is raised when we are unable to trigger the processing of an uploaded scan
	/// </summary>
	public class TriggerFailedException: Exception {
		/// <summary>
		/// Default constructor
		/// </summary>
		public TriggerFailedException() {}
		/// <summary>
		/// Constructor, allows you to specify a message
		/// </summary>
		/// <param name="message">Details on the exception</param>
		public TriggerFailedException(string message) : base(message) {}
		/// <summary>
		/// Constructor, allows you to specify a message and capture the inner exception
		/// </summary>
		/// <param name="message">Details on the exception</param>
		/// <param name="inner">The inner exception</param>
		public TriggerFailedException(string message, Exception inner)	: base(message, inner) {}
	}

	/// <summary>
	/// An exception that is raised when there is no internet connecion available
	/// </summary>
	public class InternetConnectionException: Exception {
		/// <summary>
		/// Default constructor
		/// </summary>
		public InternetConnectionException() {}
		/// <summary>
		/// Constructor, allows you to specify a message
		/// </summary>
		/// <param name="message">Details on the exception</param>
		public InternetConnectionException(string message) : base(message) {}
		/// <summary>
		/// Constructor, allows you to specify a message and capture the inner exception
		/// </summary>
		/// <param name="message">Details on the exception</param>
		/// <param name="inner">The inner exception</param>
		public InternetConnectionException(string message, Exception inner)	: base(message, inner) {}
	}

	/// <summary>
	/// An exception that is raised when there is no Manifest.xml file in the uplaod folder
	/// </summary>
	public class MissingManifestFileException: Exception {
		/// <summary>
		/// Default constructor
		/// </summary>
		public MissingManifestFileException() {}
		/// <summary>
		/// Constructor, allows you to specify a message
		/// </summary>
		/// <param name="message">Details on the exception</param>
		public MissingManifestFileException(string message) : base(message) {}
		/// <summary>
		/// Constructor, allows you to specify a message and capture the inner exception
		/// </summary>
		/// <param name="message">Details on the exception</param>
		/// <param name="inner">The inner exception</param>
		public MissingManifestFileException(string message, Exception inner)	: base(message, inner) {}
	}

	/// <summary>
	/// An exception that is raised when there is no Manifest.xml file in the uplaod folder
	/// </summary>
	public class UploadException: Exception {
		/// <summary>
		/// Default constructor
		/// </summary>
		public UploadException() {}
		/// <summary>
		/// Constructor, allows you to specify a message
		/// </summary>
		/// <param name="message">Details on the exception</param>
		public UploadException(string message) : base(message) {}
		/// <summary>
		/// Constructor, allows you to specify a message and capture the inner exception
		/// </summary>
		/// <param name="message">Details on the exception</param>
		/// <param name="inner">The inner exception</param>
		public UploadException(string message, Exception inner)	: base(message, inner) {}
	}
	#endregion

	#region UploadQueueEntry class
	/// <summary>
	/// This class represent a single entry in the upload queue.  It is a direct representation of the queue DB table
	/// </summary>
	public class UploadQueueEntry {
		/// <summary>
		/// The job ID for the scan
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// Has the upload already been started (used to check if we need to resume a previously started upload)
		/// </summary>
		public bool Started { get; set; }
		/// <summary>
		/// Has the upload completed
		/// </summary>
		public bool Uploaded { get; set; }
		/// <summary>
		/// The firectory on the server where the scan was uploaded to
		/// </summary>
		public string UploadDir { get; set; }
		/// <summary>
		/// Has the scan been triggered (i.e. have we told the server software it can begin processing the scan)
		/// </summary>
		public bool Triggered { get; set; }
		/// <summary>
		/// The date and time the item was added to the queue in yyyy/MM/dd HH:mm:ss format
		/// </summary>
		public string Created { get; set; }
		
		/// <summary>
		/// Nice simple constructor to enable us to quickly create UploadQueueEntry items.  In each case, the parameters are passed in as raw
		/// objects straight from the SQL query and are cast to type inside the constructor
		/// </summary>
		/// <param name="rawId">the Id</param>
		/// <param name="rawStarted">has the scan been started</param>
		/// <param name="rawUploaded">has the scan completed uploading</param>
		/// <param name="rawUploadDir">where the scan has been uplaoded to</param>
		/// <param name="rawTriggered">has the scan been triggered</param>
		/// <param name="rawCreated">when was the scan created</param>
		public UploadQueueEntry(object rawId, object rawStarted, object rawUploaded, object rawUploadDir, object rawTriggered, object rawCreated) {
			Id 			= Convert.ToString(rawId);
			Started 		= Convert.ToBoolean(rawStarted);
			Uploaded 	= Convert.ToBoolean(rawUploaded);
			UploadDir	= Convert.ToString(rawUploadDir);
			Triggered 	= Convert.ToBoolean(rawTriggered);
			Created 		= Convert.ToString(rawCreated);
		}
	}
	#endregion

	#region UploadManager class
	/// <summary>
	/// The UploadManager class takes care of both uploading the scan to the servers as well as triggering the
	/// scan processing. It can use both a faster, non-resumeable asynchromous upload method or a slower but resumeable
	/// upload technique. The manager also provides functionality for storing scan details to a queue for uploading
	/// in the background
	/// </summary>
	public class UploadManager {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(UploadManager));

		#region Class Data
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the
		/// event data contains debug info that should never be surfaced to a regular user
		/// </summary>
		public const int DEBUG_FLAG = -200;
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the
		/// event data contains a status message for the log
		/// </summary>
		public const int INFO_FLAG = -210;
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the
		/// event data contains an error message for the log
		/// </summary>
		public const int ERROR_FLAG = -220;
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the
		/// user visible status should be updated
		/// </summary>
		public const int STATUS_FLAG = -230;
		/// <summary>
		/// Flag used when reporting status via a background worker that the status message contains information
		/// about the number of queued scans being processed
		/// </summary>
		public const int QUEUE_FLAG = -240;
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the
		/// progress bar should switch to marquee mode
		/// </summary>
		public const int SWITCH_TO_MARQUEE = -300;
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the
		/// progress bar should switch to continuous mode
		/// </summary>
		public const int SWITCH_TO_CONTINUOUS = -310;
		/// <summary>
		/// The location, on disk, where we store app data
		/// </summary>
		private string _appDataPath;
		/// <summary>
		/// The name of the db file that holds our queue database
		/// </summary>
		private const string DB_NAME = "SugarCube_Scans.sqlite";
		/// <summary>
		/// Our IPC proxy for communicating with the uploader
		/// </summary>
		private IShowUploaderWindow pipeProxy = null;
		#endregion

		#region Initialization, Teardown and Global Utility
		/// <summary>
		/// Default constructor
		/// </summary>
		public UploadManager() {
			if (log.IsInfoEnabled) {
				log.Info("Initializing UploadManager using default constructor");
			}

			InitializeUploaderPipe();
		}

		/// <summary>
		/// Constructor that allows you to specify the app data path.  Needed when making use of the scan
		/// queuing functionality.  As a side effect, initializes our SQLiteConnection.
		/// </summary>
		/// <param name="dataPath">The path to store app data</param>
		public UploadManager(string dataPath) {
			if (log.IsInfoEnabled) {
				log.Info("Initializing UploadManager using dataPath " + dataPath);
			}
			_appDataPath = dataPath;
			InitializeUploaderPipe();
			InitializeDB();
		}

		/// <summary>
		/// Simply checks to see if the user has requested that the background worker cancel work
		/// </summary>
		/// <param name="worker">The background worker to check</param>
		/// <returns>true if we were asked to cancel, false if we should proceed</returns>
		private bool CheckForCancellation(BackgroundWorker worker) {
			if (log.IsInfoEnabled) {
				log.Info("Checking for cancellation request");
			}

			if (worker.CancellationPending) {
				if (log.IsInfoEnabled) {
					log.Info("Cancellation requested");
				}
				worker.ReportProgress(INFO_FLAG, "Canceling upload");
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Try to initialize the one end of our named pipe for communicating with the upload manager
		/// </summary>
		/// <returns>true if the initialization succeded, false otherwise</returns>
		private bool InitializeUploaderPipe() {
			if (log.IsInfoEnabled) {
				log.Info("Attempting to initialize uploader communications pipe");
			}

			try {
				ChannelFactory<IShowUploaderWindow> pipeFactory =
					new ChannelFactory<IShowUploaderWindow>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/PipeShowUploader"));
				pipeProxy =	pipeFactory.CreateChannel();
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to initialize pipe: " + ex.Message);
				}
				return false;
			}
			return true;
		}
		#endregion

		#region Scan Processing Trigger
		/// <summary>
		/// Reads the HTTP response from a web request and returns it as a string
		/// </summary>
		/// <param name="res">The http web response object to read from</param>
		/// <returns>The text of the http response</returns>
		private string ReadResponse(HttpWebResponse res) {
			if (log.IsInfoEnabled) {
				log.Info("Reading response from web stream");
			}

			StreamReader reader = new StreamReader(res.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8") );
			string response = reader.ReadToEnd();
			if (log.IsDebugEnabled) {
				log.Debug("response is " + response);
			}
			return response;
		}

		/// <summary>
		/// Triggers the actual processing of the scan on the server
		/// </summary>
		/// <param name="worker">The background worker we will be reporting status to</param>
		/// <param name="triggerURL">The URL to use to trigger the processing</param>
		/// <param name="uploadedPath">The path, on the server, where we have uploaded the new scan to</param>
		public void TriggerScanProcessing(BackgroundWorker worker, string triggerURL, string uploadedPath) {
			if (log.IsInfoEnabled) {
				log.Info("Preparing to trigger scan processing");
			}
			triggerURL = triggerURL + uploadedPath;
			if (log.IsDebugEnabled) {
				log.Debug("Trigger URL is " + triggerURL);
			}

			worker.ReportProgress(INFO_FLAG, "Calling trigger program: " + triggerURL);

			try {
				Stopwatch timer = Stopwatch.StartNew();
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(triggerURL);
				string result;
				using (HttpWebResponse res = (HttpWebResponse)req.GetResponse()) {
					result = ReadResponse(res);
				}
				timer.Stop();
				if (log.IsInfoEnabled) {
					log.InfoFormat("Result of trigger attempt was {0}, elapsed time was {1}", result, timer.Elapsed);
				}

				worker.ReportProgress(INFO_FLAG, "Upload Result: " + Environment.NewLine + result.Replace("\n", Environment.NewLine));
				worker.ReportProgress(INFO_FLAG, "Trigger time was : " + timer.Elapsed);
			} catch (WebException wex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to connect to trigger URL: " + wex.Message);
				}

				throw new TriggerFailedException("Unable to connect to trigger URL: " + wex.Message, wex);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("An error occured while calling the trigger: " + ex.Message);
				}

				throw new TriggerFailedException("An error occured while calling the trigger: " + ex.Message, ex);
			}
		}
		#endregion

		#region SFTP Uploading
		/// <summary>
		/// Helper function for <see cref="UploadScan"/>, performs a slower but resumeable upload of
		/// the files to the server
		/// </summary>
		/// <param name="worker">The background worker we will be reporting status to</param>
		/// <param name="sftp">The sftp client we are using to do the uploading</param>
		/// <param name="fileList">A list of all the files to be uploaded</param>
		private void ResumeableUploadFileSet(BackgroundWorker worker, SftpClient sftp, string[] fileList) {
			if (log.IsInfoEnabled) {
				log.Info("Beginning resumeable upload of file set");
			}

			if (fileList.Length == 0) {
				if (log.IsInfoEnabled) {
					log.Info("No files in file set to upload");
				}
				worker.ReportProgress(INFO_FLAG, "No files to upload");
				return;
			}
			int counter = 0;

			//  Start file uploads
			worker.ReportProgress(INFO_FLAG, "Beginning upload");
			if (log.IsDebugEnabled) {
				log.Debug("Beginning upload");
			}

			foreach (string localFile in fileList) {
				if (CheckForCancellation(worker)) {
					if (log.IsDebugEnabled) {
						log.Debug("Cancellation pending");
					}
					return;
				}

				counter++;
				string fileName = Path.GetFileName(localFile);
				if (log.IsInfoEnabled) {
					log.InfoFormat("uploading file {0} of {1} ({2})", counter, fileList.Length, fileName);
				}
				worker.ReportProgress(INFO_FLAG, String.Format("Uploading file {0} of {1} ({2})", counter, fileList.Length, fileName));
				bool remoteExists = sftp.Exists(fileName);
				if (log.IsDebugEnabled) {
					log.Debug("Remote file exists: " + remoteExists);
				}

				
				using (var destStream = sftp.Open(sftp.WorkingDirectory + "/" + fileName, remoteExists ? FileMode.Append : FileMode.CreateNew, FileAccess.Write)) {
					using (var localStream = File.Open(localFile, FileMode.Open, FileAccess.Read)) {
						const int BufferSize = 102400;
						var buffer = new byte[BufferSize];
						var uploadCompleted = false;
						long offset = 0;

						if (remoteExists) {
							var attr = sftp.GetAttributes(fileName);
							offset = attr.Size;
							localStream.Seek(offset, SeekOrigin.Begin);
						}

						while (!uploadCompleted) {
							var bytesRead = localStream.Read(buffer, 0, BufferSize);

							destStream.Write(buffer, 0, bytesRead);
							offset += bytesRead;
							uploadCompleted = (bytesRead < BufferSize);
							if (CheckForCancellation(worker)) {
								if (log.IsDebugEnabled) {
									log.Debug("Cancellation pending");
								}
								uploadCompleted = true;
							}
						}
					}
				}
				if (CheckForCancellation(worker)) {
					if (log.IsDebugEnabled) {
						log.Debug("Cancellation pending");
					}
					return;
				}
				double perc = (counter * 100) / fileList.Length;
				worker.ReportProgress(Convert.ToInt32(perc));
			}
		}

		/// <summary>
		/// Helper function for <see cref="UploadScan"/>, performs an asynchronous, non-resumeable upload of
		/// the files to the server
		/// </summary>
		/// <param name="worker">The background worker we will be reporting status to</param>
		/// <param name="sftp">The sftp client we are using to do the uploading</param>
		/// <param name="fileList">A list of all the files to be uploaded</param>
		/// <param name="bytesToUpload">The total size (in bytes) of the files to be uploaded</param>
		private void AsyncUploadFileSet(BackgroundWorker worker, SftpClient sftp, string[] fileList, ulong bytesToUpload) {
			if (log.IsInfoEnabled) {
				log.Info("Beginning asynchronous, non-resumable upload of file set");
			}

			if (fileList.Length == 0) {
				if (log.IsInfoEnabled) {
					log.Info("No files in file set");
				}
				worker.ReportProgress(INFO_FLAG, "No files to upload");
				return;
			}

			var uploadWaitHandles = new List<WaitHandle>();
			SftpUploadAsyncResult[] uploadResults = new SftpUploadAsyncResult[fileList.Length];

			//  Start file uploads
			worker.ReportProgress(INFO_FLAG, "Beginning upload");
			int i = 0;
			foreach (string file in fileList) {
				if (log.IsDebugEnabled) {
					log.Debug("Starting upload of file " + file);
				}

				worker.ReportProgress(INFO_FLAG, "Starting " + file);
				uploadResults[i] = sftp.BeginUploadFile(File.OpenRead(file), Path.GetFileName(file), null, null) as SftpUploadAsyncResult;
				uploadWaitHandles.Add(uploadResults[i].AsyncWaitHandle);
				i++;

				if (CheckForCancellation(worker)) {
					if (log.IsDebugEnabled) {
						log.Debug("Cancellation pending");
					}
					return;
				}
			}

			bool uploadCompleted = false;
			if (log.IsDebugEnabled) {
				log.Debug("Total size of files to upload is " + bytesToUpload);
			}

			worker.ReportProgress(INFO_FLAG, "Waiting for upload to complete, total upload size (in bytes) = " + bytesToUpload.ToString());
			while (!uploadCompleted) {
				if (worker.CancellationPending) {
					if (log.IsDebugEnabled) {
						log.Debug("Cancellation pending");
					}
					uploadCompleted = true;
					continue;
				}
				//  Assume upload completed
				uploadCompleted = true;
				ulong bytesUploaded = 0;

				foreach (SftpUploadAsyncResult result in uploadResults) {
					bytesUploaded += result.UploadedBytes;
					if (!result.IsCompleted) {
						uploadCompleted = false;
					}
				}
				double perc = (bytesUploaded * 100) / bytesToUpload;
				worker.ReportProgress(Convert.ToInt32(perc));
				Thread.Sleep(500);
			}

			if (!worker.CancellationPending) {
				worker.ReportProgress(INFO_FLAG, "Upload complete, finalizing files");
			}
			//  End file uploads
			foreach (SftpUploadAsyncResult result in uploadResults) {
				try {
					sftp.EndUploadFile(result);
				} catch (SshOperationTimeoutException tex) {
					if (log.IsWarnEnabled) {
						log.Warn("Timeout exception when attempting to call EndUploadFile on async result object");
					}
				}
			}
		}

		/// <summary>
		/// Uploads a set of scanned images and attachments to the server, always uses the faster, non-resumeable
		/// asynchromous upload method
		/// </summary>
		/// <param name="worker">The background worker we will be reporting status to</param>
		/// <param name="shootFolder">The local folder where our scan is stored</param>
		/// <param name="host">The upload host</param>
		/// <param name="user">The SFTP user</param>
		/// <param name="pass">The password for the SFTP user</param>
		/// <param name="uploadBase">The base path on the server for storing new scans</param>
		/// <param name="scanID">The ID of the scan, will be used with uploadBase to create the new shoot folder</param>
		/// <returns>The name of the remote folder the scan was uploaded to</returns>
		public string UploadScan(BackgroundWorker worker, string shootFolder, string host,
										 string user, string pass, string uploadBase, string scanID) {
			if (log.IsInfoEnabled) {
				log.Info("Upload Scan called (default asynch mode)");
			}
			return UploadScan(worker, shootFolder, host, user, pass, uploadBase, scanID, false);
		}

		/// <summary>
		/// Uploads a set of scanned images and attachments to the server.  Alows you to choose between
		/// a faster, non-resumable upload or the slower, resumeable method.
		/// </summary>
		/// <param name="worker">The background worker we will be reporting status to</param>
		/// <param name="shootFolder">The local folder where our scan is stored</param>
		/// <param name="host">The upload host</param>
		/// <param name="user">The SFTP user</param>
		/// <param name="pass">The password for the SFTP user</param>
		/// <param name="uploadBase">The base path on the server for storing new scans</param>
		/// <param name="scanID">The ID of the scan, will be used with uploadBase to create the new shoot folder</param>
		/// <param name="isResumeable">True if we want to use the slower resumeable upload method, false if we want a faster but non-resumeable asynchronous upload</param>
		/// <returns>The name of the remote folder the scan was uploaded to, or a string starting with :ERROR: if an error has occured</returns>
		public string UploadScan(BackgroundWorker worker, string shootFolder, string host,
										 string user, string pass, string uploadBase, string scanID, bool isResumeable) {
			if (log.IsInfoEnabled) {
				log.Info("UploadScan called");
			}

			if (!File.Exists(Path.Combine(shootFolder, "Manifest.xml"))) {
				if (log.IsWarnEnabled) {
					log.WarnFormat("Shoot Folder {0} does not contain Manifest.xml file", shootFolder);
				}
				throw new MissingManifestFileException("No Manifest.xml file found in " + shootFolder);
			}

			try {
				if (log.IsDebugEnabled) {
					log.DebugFormat("Creating SFTP client using values {0}:{1}@{2}", user, pass, host);
				}
				worker.ReportProgress(DEBUG_FLAG, String.Format("Creating SFTP client using values {0}:{1}@{2}", user, pass, host));
				Stopwatch timer = Stopwatch.StartNew();
				using (var sftp = new SftpClient(host, user, pass)) {
					worker.ReportProgress(DEBUG_FLAG, "SFTP Client Created");
					if (log.IsDebugEnabled) {
						log.Debug("SFTP Client Created");
					}
					sftp.Connect();
					worker.ReportProgress(DEBUG_FLAG, "SFTP Client Connected");
					if (log.IsDebugEnabled) {
						log.Debug("SFTP Client Connected");
					}
					worker.ReportProgress(DEBUG_FLAG, "Checking if uploadBase directory exists");
					if (log.IsDebugEnabled) {
						log.Debug("Checking if uploadBase directory exists");
					}
					if (!sftp.Exists(uploadBase)) {
						worker.ReportProgress(DEBUG_FLAG, "uploadBase directory does not exist, attempting to create it");
						if (log.IsDebugEnabled) {
							log.Debug("uploadBase directory does not exist, attempting to create it");
						}
						sftp.CreateDirectory(uploadBase);
					}
					worker.ReportProgress(DEBUG_FLAG, "Changing directory to " + uploadBase);
					if (log.IsDebugEnabled) {
						log.Debug("Changing directory to " + uploadBase);
					}
					sftp.ChangeDirectory(uploadBase);

					if (CheckForCancellation(worker)) {
						if (log.IsDebugEnabled) {
							log.Debug("Cancellation pending");
						}
						return "CANCELLED";
					}

					if (sftp.Exists(scanID)) {
						worker.ReportProgress(DEBUG_FLAG, "Order folder " + scanID + " already exists");
						if (log.IsDebugEnabled) {
							log.Debug("Order folder " + scanID + " already exists");
						}
					} else {
						worker.ReportProgress(DEBUG_FLAG, "Creating order folder " + scanID);
						if (log.IsDebugEnabled) {
							log.Debug("Creating order folder " + scanID);
						}
						sftp.CreateDirectory(scanID);
					}
					worker.ReportProgress(DEBUG_FLAG, "Changing directory to " + scanID);
					if (log.IsDebugEnabled) {
						log.Debug("Changing directory to " + scanID);
					}
					sftp.ChangeDirectory(scanID);
					string uploadDirectory = sftp.WorkingDirectory;
					if (log.IsDebugEnabled) {
						log.Debug("Current FTP working directory is " + uploadDirectory);
					}


					if (CheckForCancellation(worker)) {
						if (log.IsDebugEnabled) {
							log.Debug("Cancellation pending");
						}
						return "CANCELLED";
					}

					worker.ReportProgress(INFO_FLAG, "Getting the list of all files to upload");
					if (log.IsInfoEnabled) {
						log.Info("Getting list of files to uplaod");
					}

					string[] fileList = Directory.GetFiles(shootFolder, "*.*");
					int fileCount = fileList.Length;
					ulong bytesToUpload = 0;
					foreach (string file in fileList) {
						bytesToUpload += Convert.ToUInt64(new FileInfo(file).Length);
					}
					if (log.IsInfoEnabled) {
						log.InfoFormat("Found {0} files to upload ({1} bytes)", fileList.Length, bytesToUpload);
					}

					worker.ReportProgress(INFO_FLAG, String.Format("Found {0} files to upload ({1} bytes)", fileList.Length, bytesToUpload));

					if (CheckForCancellation(worker)) {
						if (log.IsDebugEnabled) {
							log.Debug("Cancellation pending");
						}
						return "CANCELLED";
					}

					if (isResumeable) {
						worker.ReportProgress(DEBUG_FLAG, "Using resumeable upload");
						ResumeableUploadFileSet(worker, sftp, fileList);
					} else {
						worker.ReportProgress(DEBUG_FLAG, "Using asynchronous upload");
						AsyncUploadFileSet(worker, sftp, fileList, bytesToUpload);
					}

					sftp.Disconnect();
					timer.Stop();
					worker.ReportProgress(100, "Upload complete in " + timer.Elapsed);
					if (log.IsInfoEnabled) {
						log.Info("Upload complete in " + timer.Elapsed);
					}

					return uploadDirectory;
				}
			} catch (SshAuthenticationException authex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to authenticate secure connection", authex);
				}
				throw authex;
			} catch (SftpPermissionDeniedException pde) {
				if (log.IsErrorEnabled) {
					log.Error("SFTP Permission exception", pde);
				}
				throw pde;
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("An unknown error occured during upload.", ex);
				}
				throw new UploadException("An unexpected error occured while attempting the upload.", ex);
			}
		}
		#endregion

		#region Upload Queue Management
		/// <summary>
		/// We don't want to lock the DB and cause problems when we have two seperate apps working with the same
		/// SQLite files, so we create a connection, e xecute our SQL and get out.  This method makes that a lot
		/// simpler
		/// </summary>
		/// <returns></returns>
		private SQLiteConnection GetDBConnection() {
			string dbFilePath = Path.Combine(_appDataPath, DB_NAME);
			if (log.IsInfoEnabled) {
				log.Info("Getting connection to DB at " + dbFilePath);
			}

			SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath + ";Version=3;");
			con.Open();
			return con;
		}

		/// <summary>
		/// Execute a non-query SQL statement
		/// </summary>
		/// <param name="sql">The sql to execute</param>
		/// <returns></returns>
		private int RunNonQuery(string sql) {
			int result;
			if (log.IsInfoEnabled) {
				log.Info("Preparing to execute non-query");
			}
			if (log.IsDebugEnabled) {
				log.Debug("sql = " + sql);
			}


			SQLiteConnection con = GetDBConnection();
			using (SQLiteCommand cmd = new SQLiteCommand(sql, con)) {
				result = cmd.ExecuteNonQuery();
			}
			con.Close();
			if (log.IsDebugEnabled) {
				log.Debug("result = " + result);
			}

			return result;
		}

		/// <summary>
		/// Initializes <see cref="_con"/>, our SQLite DB.  Checks to make sure the DB file exists and creates
		/// it if it doesn't
		/// </summary>
		private void InitializeDB () {
			string dbFilePath = Path.Combine(_appDataPath, DB_NAME);
			if (log.IsInfoEnabled) {
				log.Info("Initializing DB at " + dbFilePath);
			}

			if (!File.Exists(dbFilePath)) {
				//If the DB doesn't already exist, create it and set up the tables
				SQLiteConnection.CreateFile(dbFilePath);
				RunNonQuery("CREATE TABLE scans (id TEXT, started INT, uploaded INT, uploadDir TEXT, triggered INT, created TEXT)");
				if (log.IsInfoEnabled) {
					log.Info("Created new DB");
				}

			}
		}

		/// <summary>
		/// Adds a new scan into the scans table
		/// </summary>
		/// <param name="scanID">the ID of the scan to add</param>
		public void AddScanToQueue(string scanID) {
			if (log.IsInfoEnabled) {
				log.Info("Adding scan " + scanID + " to queue");
			}

			string sql = String.Format("INSERT INTO scans VALUES ('{0}', 0, 0, 0, '', '{1}')", scanID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
			RunNonQuery(sql);
		}

		/// <summary>
		/// Marks a scan as begun, that is uploading has started
		/// </summary>
		/// <param name="scanID">the ID of the scan to update</param>
		private void MarkScanAsBegun(string scanID) {
			if (log.IsInfoEnabled) {
				log.Info("Marking scan as begun: " + scanID);
			}

			string sql = String.Format("UPDATE SCANS SET started = 1 WHERE id = '{0}'", scanID);
			RunNonQuery(sql);
		}

		/// <summary>
		/// Marks a scan as uploaded, that is the upload has completed but the trigger URL has not been called yet
		/// </summary>
		/// <param name="scanID">the ID of the scan to update</param>
		/// <param name="uploadDirectory">The path on the server where the scan was uploaded to</param>
		private void MarkScanAsUploaded(string scanID, string uploadDirectory) {
			if (log.IsInfoEnabled) {
				log.Info("Marking scan as uploaded: " + scanID);
			}

			string sql = String.Format("UPDATE SCANS SET uploaded = 1, uploadDir = '{0}' WHERE id = '{1}'", uploadDirectory, scanID);
			RunNonQuery(sql);
		}


		/// <summary>
		/// Marks a scan as triggered, that is the trigger URL has been called
		/// </summary>
		/// <param name="scanID">the ID of the scan to update</param>
		private void MarkScanAsTriggered(string scanID) {
			if (log.IsInfoEnabled) {
				log.Info("Marking scan as triggered: " + scanID);
			}

			string sql = String.Format("UPDATE SCANS SET triggered = 1 WHERE id = '{0}'", scanID);
			RunNonQuery(sql);
		}

		/// <summary>
 		/// Gets a list of UploadQueueEntries from the upload queue.
		/// </summary>
		/// <param name="openOnly">If false, returns all queue items, if true, returns only open queue items</param>
		/// <returns>A list of all the items in the queue or all the open items in the queue</returns>
		private List<UploadQueueEntry> GetQueueItems (bool openOnly) {
			if (log.IsInfoEnabled) {
				log.Info("Getting list of queued scans, openOnly is " + openOnly.ToString());
			}

			string sql = "SELECT id, started, uploaded, uploadDir, triggered, created FROM scans";
			if (openOnly) {
				sql = sql + " WHERE uploaded = 0 OR triggered = 0";
			}
			sql = sql + " ORDER BY id";
			SQLiteConnection con = GetDBConnection();
			SQLiteCommand cmd = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = cmd.ExecuteReader();
			List<UploadQueueEntry> queueEntries = new List<UploadQueueEntry>();
			while (reader.Read()) {
				if (log.IsDebugEnabled) {
					log.DebugFormat("processing DB row {0}, {1}, {2}, {3}, {4}, {5}",
										 new object[] {reader["id"], reader["started"], reader["uploaded"], reader["uploadDir"], reader["triggered"], reader["created"]});
				}
				queueEntries.Add(new UploadQueueEntry(reader["id"], reader["started"], reader["uploaded"], reader["uploadDir"], reader["triggered"], reader["created"]));
			}
			if (log.IsDebugEnabled) {
				log.Debug("Found " + queueEntries.Count + " queued scans");
			}

			return queueEntries;
		}

		/// <summary>
		/// Gets all the items in the upload queue, open and closed
		/// </summary>
		/// <returns>A list of all the items in the queue</returns>
		public List<UploadQueueEntry> GetAllUploadQueueItems() {
			if (log.IsInfoEnabled) {
				log.Info("Getting list of all queued scans");
			}

			return GetQueueItems(false);
		}		


		/// <summary>
		/// Gets all the open items in the queue
		/// </summary>
		/// <returns></returns>
		public List<UploadQueueEntry> GetOpenUploadQueueItems () {
			if (log.IsInfoEnabled) {
				log.Info("Getting list of all queued scans");
			}

			return GetQueueItems(true);
		}

		/// <summary>
		/// Gets the list of scans from the DB that are either waiting to be uplaoded or triggered, and begins
		/// uploading them
		/// </summary>
		/// <param name="worker">A reference to the BackgroundWorker object running this thread</param>
		/// <param name="host">The host to upload scans to</param>
		/// <param name="user">The user for logging in to the SFTP host</param>
		/// <param name="pass">The password for the user</param>
		/// <param name="uploadBase">The base path to uplaod new scan to</param>
		/// <param name="triggerURL">The trigger URL to call once the upload is complete</param>
		public void ProcessQueuedScans(BackgroundWorker worker, string host, string user,
												 string pass, string uploadBase, string triggerURL) {
			if (log.IsInfoEnabled) {
				log.Info("Processing queued scans");
			}

			// Check if we have any rows to process
			string sql = "SELECT COUNT(*) FROM scans WHERE uploaded = 0 OR triggered = 0";
			SQLiteConnection con = GetDBConnection();
			SQLiteCommand cmd = new SQLiteCommand(sql, con);
			int count = Convert.ToInt16(cmd.ExecuteScalar());
			con.Close();
			if (log.IsDebugEnabled) {
				log.Debug("There are currently " + count + " scans to upload");
			}

			if (count == 0) {
				if (log.IsInfoEnabled) {
					log.Info("No queued scans to upload at this time");
				}

				worker.ReportProgress(QUEUE_FLAG, "There Are Currently No Queued Scans To Upload");
				return;
			}

			if (CheckForCancellation(worker)) {
				if (log.IsDebugEnabled) {
					log.Debug("Cancellation pending");
				}
				return;
			}

			List<UploadQueueEntry> queuedItems = GetOpenUploadQueueItems();
			int counter = 0;
			string shootFolder = "";
			foreach (UploadQueueEntry queuedItem in queuedItems) {
				counter += 1;
				if (log.IsDebugEnabled) {
					log.DebugFormat("Uploading queued scan id={0}, started={1}, uploaded={2}, uplaodDir={3}, triggered={4}, created={5}",
										 new object[] {queuedItem.Id, queuedItem.Started, queuedItem.Uploaded, queuedItem.UploadDir, queuedItem.Triggered, queuedItem.Created});
				}

				worker.ReportProgress(QUEUE_FLAG, string.Format("Processing Scan {0} of {1}", counter, queuedItems.Count));
				if (log.IsInfoEnabled) {
					log.InfoFormat("Processing queued scan {0} of {1}", counter, queuedItems.Count);
				}

				// check to see we have a working internet connection at this time
				if (!CheckForInternetConnection()) {
					// exit out with an exception which we can handle in our client software.
					throw new InternetConnectionException("There is currently no connectivity to the 3DWares servers, so uploading can not proceed at this time");
				}

				string uploadDirectory = queuedItem.UploadDir;
				try {
					// Check if we need to upload the scan first before triggering it
					if (!queuedItem.Uploaded) {
						if (log.IsDebugEnabled) {
							log.Debug("Scan is not yet uploaded");
						}

						//If a scan has already been started once before, we need to use the resumable upload method, otherwise we
						//can try to first use the faster async upload technique
						bool useResumeable = queuedItem.Started;
						MarkScanAsBegun(queuedItem.Id);
						shootFolder = Path.Combine(_appDataPath, queuedItem.Id);
						if (!Directory.Exists(shootFolder)) {
							if (log.IsWarnEnabled) {
								log.Warn("Folder for scan " + shootFolder + " not found, it may have been deleted");
							}
							worker.ReportProgress(ERROR_FLAG, "Scan " + queuedItem.Id + " can not be found on disk, it may have been deleted");
							MarkScanAsUploaded(queuedItem.Id, "ERROR, NOT FOUND");
							MarkScanAsTriggered(queuedItem.Id);
							continue;
						}

						uploadDirectory = UploadScan(worker, shootFolder, host, user, pass, uploadBase, queuedItem.Id, useResumeable);
						if (log.IsDebugEnabled) {
							log.Debug("upload directory for scan is " + uploadDirectory);
						}

						if (uploadDirectory == "CANCELLED") {
							if (log.IsDebugEnabled) {
								log.Debug("Cancellation pending");
							}
							return;
						}
						
						MarkScanAsUploaded(queuedItem.Id, uploadDirectory);
						if (CheckForCancellation(worker)) {
							if (log.IsDebugEnabled) {
								log.Debug("Cancellation pending");
							}
							return;
						}
					} // scan is now uploaded
					
					TriggerScanProcessing(worker, triggerURL, uploadDirectory);
					MarkScanAsTriggered(queuedItem.Id);
					Directory.Delete(shootFolder, true);
				} catch (MissingManifestFileException) {
					if (log.IsWarnEnabled) {
					    log.Warn("Manifest.xml file is missing from scan, removing scan from active queue and deleting shoot folder");
					}
					MarkScanAsUploaded(queuedItem.Id, "ERROR, NO MANIFEST.XML FOUND");
					MarkScanAsTriggered(queuedItem.Id);		
					Directory.Delete(shootFolder, true);
					worker.ReportProgress(ERROR_FLAG, "Scan " + queuedItem.Id + " had no Manifest.xml file, the scan has been deleted");
				} catch (SshAuthenticationException) {
					worker.ReportProgress(ERROR_FLAG, "There was an error connecting to the 3DWares cloud, we will retry the upload later.");
				} catch (SftpPermissionDeniedException) {
					worker.ReportProgress(ERROR_FLAG, "There was an error in the 3DWares cloud, we will retry the upload later.");
				} catch (UploadException uex) {
					worker.ReportProgress(ERROR_FLAG, "There was an error uploading the scan: " + uex.InnerException.Message + ". We will try again later.");
				} catch (TriggerFailedException tex) {
					worker.ReportProgress(ERROR_FLAG, "Unable to trigger scan processing in the 3DWares cloud: " + tex.InnerException.Message + ". We will try again later");
				} catch (Exception ex) {
					worker.ReportProgress(ERROR_FLAG, "An unexpected error occured during the upload: " + ex.Message + ". We will try again later");
				}
			}
			if (log.IsInfoEnabled) {
				log.Info("All queued scans processed");
			}
			worker.ReportProgress(QUEUE_FLAG, "All Currently Queued Scans Have Completed");
		}

		#endregion

		#region Utility Functions
		/// <summary>
		/// A simple test that tries to connect to our test page at http://test.soundfit.me/isup.html in order to determine if
		/// there is a working internet connection *at that moment*.
		/// </summary>
		/// <returns>True if there is a working internet connection, False if there is no</returns>
		public bool CheckForInternetConnection() {
			if (log.IsInfoEnabled) {
				log.Info("Checking for connection to soundfit/3dwares servers");
			}

			try {
				using (var client = new WebClient() {
					Proxy = null
				})
				using (var stream = client.OpenRead("http://test.soundfit.me/isup.html")) {
					if (log.IsDebugEnabled) {
						log.Debug("able to read test page");
					}
					return true;
				}
			} catch {
				if (log.IsWarnEnabled) {
					log.Warn("Unable to read test page");
				}

				return false;
			}
		}

		/// <summary>
		/// Tries to call the remote ShowUploaderWindow method on the other side of our PipeProxy, which should cause the upload manager
		/// window to appear. Traps the EndpointNotFoundException specifically so we can attempt to start the upload manager if it is not already
		/// running
		/// </summary>
		/// <returns>
		/// 1 if all is well, 0 if the proxy has not been initialized, -1 if there was a EndpointNotFoundException and -2 if any other exception
		/// occured
		/// </returns>
		public int ShowUploaderWindow() {
			if (log.IsInfoEnabled) {
				log.Info("Attempting to show uploader window");
			}

			if (pipeProxy == null) {
				if (log.IsDebugEnabled) {
					log.Debug("Pipe proxy is null");
				}

				return 0;
			}
			try {
				pipeProxy.ShowUploaderWindow();
			} catch (EndpointNotFoundException enfx) {
				if (log.IsErrorEnabled) {
					log.Error("EndpointNotFoundException: " + enfx.Message);
				}
				return -1;
			} catch (Exception ex) {
				if (log.IsDebugEnabled) {
					log.Debug("Exception: " + ex.Message);
				}

				return -2;
			}
			return 1;
		}

		#endregion

	}
	#endregion
}
