/* SugarCube Host Software - Test Bed Batch Runner
 * Copyright (c) 2014-2015 Chad Ullman
 */


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Camera_NET;
using DirectShowLib;
using log4net;

namespace Me.ThreeDWares.SugarCube {
	public partial class MainForm : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
		
		#region Class Data
		private CubeConfig _config = new CubeConfig();
		private ModelingOptions _modelOpts = new ModelingOptions();
		private SugarCube _cube = new SugarCube();
		private Scan _scan;
		private UploadManager _uploader;
		private ManualResetEvent _mainSynchronizer = new ManualResetEvent(false);
		private ManualResetEvent _picSynchronizer = new ManualResetEvent(false);

		private string _35mmFocalEquivalent = "";
		
		private Font _dateFont = new Font("Arial", 8F, FontStyle.Regular);
		private Font _logFont = new Font("Arial", 10F, FontStyle.Regular);

		private BackgroundWorker _mainWorker = new BackgroundWorker();
		
		
		public const int DEBUG = -300;		
		public const int INFO  = -301;		
		public const int ERROR = -302;		
		#endregion

		#region Initialization and Shutdown
		public MainForm() {
			InitializeComponent();
			if (log.IsInfoEnabled) {
			    log.Info("Initializing MainForm");
			}
			
			this.Text = string.Format(this.Text, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
			_modelOpts.doImageCropping = _config.DoImageCropping.ToString();
			_modelOpts.imageCroppingRectangle = _config.ImageCropRectangle;
			_modelOpts.doModelScaling = _config.DoModelScaling.ToString();
			_modelOpts.doModelCropping = _config.DoModelCropping.ToString();
			_modelOpts.boundingBox = _config.BoundingBox;
			_modelOpts.meshlevel = _config.MeshLevel.ToString();
			_modelOpts.template3DP = _config.Template3DP;
			// Do this now so we can get the local config folder and thus initialize the DB for scans
			_uploader= new UploadManager(_config.DataPath);
			// Set up the background worker
			_mainWorker.WorkerReportsProgress = true;
			_mainWorker.WorkerSupportsCancellation = true;
			_mainWorker.DoWork += MainWorker_DoWork;
			_mainWorker.ProgressChanged += MainWorker_ProgressChanged;
			_mainWorker.RunWorkerCompleted += MainWorker_WorkCompleted;
		}

		private void SetCameraControlSourceAndResolution (string camName, string res) {
			LogDebug(String.Format("Setting camera to {0} at res {1}", camName, res));
			CameraChoice chooser = new CameraChoice();
			chooser.UpdateDeviceList();
			DsDevice chosenCam = chooser.GetCameraByName(camName);
			ResolutionList resolutions = Camera.GetResolutionList(chosenCam.Mon);
			Camera_NET.Resolution chosenRes = resolutions.Find(x => x.ToString() == res);
			camControl.SetCamera(chosenCam.Mon, chosenRes);
		}

		void MainFormLoad(object sender, EventArgs e) {
			try {
				SetCameraControlSourceAndResolution(_config.Camera, _config.Resolution);
			} catch (Exception ex) {
				LogError("Error during camera initialization: " + ex.Message);
				return;
			}

			try {
				_cube.Initialize();
				// Set the status LED to bright blue
				_cube.PostCommand("LO");
				_cube.PostCommand("LB200");
			} catch (Exception ex) {
				LogError("Error during sugarcube initialization: " + ex.Message);
				return;
			}
			string le = "38";
			try {
				le = _cube.CubeParameters["LE"];
			} catch {
				LogInfo("35mm Focal Length Equivalent was NOT found in the Z string, defaulting to 38");
			}
			if (String.IsNullOrWhiteSpace(le)) {
				LogInfo("35mm Focal Length Equivalent was NOT set in the Z string, defaulting to 38");
			} else {
				LogInfo("Setting 35mm Focal Length Equivalent to " + le);
				_35mmFocalEquivalent = le;
			}
		}

		void MainFormFormClosed(object sender, FormClosedEventArgs e) {
			if (camControl.Camera != null) {
				try {
					camControl.CloseCamera();
				} catch (Exception ex) {
					LogError("Problem closing camera: " + ex.Message);
					LogError(ex.StackTrace);
				}
			}
			// Set the status led to very light blue
			if (_cube.Initialized) {
				try {
					_cube.PostCommand("LO");
					_cube.PostCommand("LB1");
					_cube.PostCommand("LG1");
					_cube.Close();
				} catch (Exception ex) {
					LogError("Problem closing cube: " + ex.Message);
					LogError(ex.StackTrace);
				}
			}
		}
		#endregion

		#region Logging
		private void LogDebug (string message) {
			if (log.IsDebugEnabled) {
			    log.Debug(message);
			}
			
			WriteLog(message, Color.Black);
		}

		private void LogInfo (string message) {
			if (log.IsInfoEnabled) {
			    log.Info(message);
			}
			
			WriteLog(message, Color.Navy);
		}

		private void LogError (string message) {
			if (log.IsErrorEnabled) {
			    log.Error(message);
			}
			
			WriteLog(message, Color.Red);
		}

		private void WriteLog(string message, Color logColor) {
			tbLog.DeselectAll();
			tbLog.SelectionColor = Color.Gray;
			tbLog.SelectionFont = _dateFont;
			tbLog.AppendText(DateTime.Now.ToString("HH:mm:ss.ffff") + " ");
			tbLog.SelectionColor = logColor;
			tbLog.SelectionFont = _logFont;
			tbLog.AppendText(message + Environment.NewLine);
			tbLog.SelectionStart = tbLog.Text.Length;
			tbLog.ScrollToCaret();
			tbLog.Refresh();
		}
		#endregion
		
		#region BackGround Worker Event Handlers
		private Scan InitializeScan(string scanIDPrefix, string scanSubject) {
			Scan theScan = new Scan();
			theScan.metadata.scanID = scanIDPrefix + DateTime.Now.ToString("yyyyMMddHHmm");
			theScan.LocalScanFolder = Path.Combine(_config.DataPath, theScan.metadata.scanID);
			if (!Directory.Exists(theScan.LocalScanFolder)) {
				Directory.CreateDirectory(theScan.LocalScanFolder);
			}

			theScan.orderDetails.from = _config.UserEmail;
			theScan.orderDetails.to = _config.UserEmail;
			theScan.orderDetails.subject = scanSubject;
			theScan.orderDetails.body = "";
			theScan.orderDetails.referenceID = theScan.metadata.scanID;

			theScan.imageSet.id = scanSubject;

			theScan.diagnostics.shootingString = _config.ShootString.Replace(Environment.NewLine, " ");
			theScan.diagnostics.cameraID = _config.Camera;
			theScan.diagnostics.imageResolution = _config.Resolution;
			theScan.diagnostics.jpegQuality = _config.JpegQuality.ToString();
			theScan.diagnostics.COMport = _cube.PortName;
			theScan.diagnostics.forceCOMPort = "false";
			theScan.diagnostics.motion_detection = (_config.UseMotionDetection) ? "on" : "off";
			theScan.diagnostics.motion_sensitivity = _config.MotionSensitivity.ToString();

			theScan.modelingOptions = _modelOpts;
			
			theScan.zData = _cube.CubeParameters;
			return theScan;
		}		
		
		private void MainWorker_DoWork(object sender, DoWorkEventArgs e) {
			BackgroundWorker worker = sender as BackgroundWorker;
			for (int i = 1; i <= udIterations.Value; i++) {
				string batchSubject = String.Format("Batch Scan {0} of {1}", i, udIterations.Value);
				worker.ReportProgress(DEBUG, batchSubject);
				if (worker.CancellationPending) {
					e.Cancel = true;
					return;
				}
				_scan = InitializeScan(_config.ShootPrefix, batchSubject);

				if (worker.CancellationPending) {
					e.Cancel = true;
					return;
				}
				try {
					_cube.ProcessShootString(_config.ShootString, worker, e, _picSynchronizer);
				} catch (Exception ex) {
					worker.ReportProgress(ERROR, "Unable to run scan: " + ex.Message);
					continue;
				}

				if (worker.CancellationPending) {
					e.Cancel = true;
					return;
				}

				if (_config.DoSetEXIF) {
					try {
						worker.ReportProgress(INFO, "Setting EXIF data");
						CubeImageUtils.SetExifInfo(_scan.LocalScanFolder, _35mmFocalEquivalent);
					} catch (Exception ex) {
						worker.ReportProgress(ERROR, "Unable to set Exif: " + ex.Message);
					}
				}

				if (worker.CancellationPending) {
					e.Cancel = true;
					return;
				}
				
				string host = _config.SftpHost;
				string user = _config.SftpUser;
				string pass = _config.SftpPassword;
				string uploadBase = _config.SftpUploadBase;
				string triggerURL = _config.TriggerURL;
	
				try {
					worker.ReportProgress(UploadManager.STATUS_FLAG, "Uploading Scan");
					_scan.metadata.uploadStartTime = DateTime.Now.ToString(Scan.DATE_FORMAT);
					_scan.WriteManifest();
					UploadEar(worker, _scan.LocalScanFolder, host, user, pass, uploadBase, _scan.metadata.scanID, triggerURL);
				} catch (Exception ex) {
					worker.ReportProgress(ERROR, "Unable to upload scan: " + ex.Message);
				}
				
				worker.ReportProgress(DEBUG, "Scan upload complete");
				
				if (i == udIterations.Value) {
					continue; // skip the wait time
				}
				
				worker.ReportProgress(DEBUG, String.Format("Waiting {0} minute{1} to start next scan", udDelay.Value, udDelay.Value > 1 ? "s" : ""));
				worker.ReportProgress(UploadManager.SWITCH_TO_CONTINUOUS, 0);
				ulong start = Convert.ToUInt64(DateTime.Now.Ticks);
				Debug.WriteLine("start = " + start);
				ulong max = Convert.ToUInt64(udDelay.Value) * 60 * 10000000; // convert the time from minutes to ticks, which is a hundred nanoseconds
				Debug.WriteLine("max = " + max);
				ulong delta = Convert.ToUInt64(DateTime.Now.Ticks) - start;
				Debug.WriteLine("first delta = " + delta);
				while (delta <= max) {
					Thread.Sleep(5000);
					if (worker.CancellationPending) {
						e.Cancel = true;
						return;
					}
					int prog = Convert.ToInt16(Math.Round(((double)delta/(double)max)*100));
					Debug.WriteLine("prog = " + prog);
					//worker.ReportProgress(DEBUG, prog.ToString());
					worker.ReportProgress(prog);
					delta = Convert.ToUInt64(DateTime.Now.Ticks) - start;
					Debug.WriteLine("delta = " + delta);
				}
			}
			
			worker.ReportProgress(UploadManager.SWITCH_TO_CONTINUOUS, null);
			worker.ReportProgress(DEBUG, "Batch Run Complete");
		}

		private void MainWorker_WorkCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Error != null) {
				LogError("An error occured while running the batch: " + e.Error.Message);
			}
			
			if (e.Cancelled) {
				LogDebug("Batch cancelled by user request");
			}
			
			btnStart.Enabled = true;
			btnCancel.Enabled = false;
			udIterations.Enabled = true;
			udDelay.Enabled = true;
		}

		//TODO: This is ughly, make it prettier
		private void MainWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			if (e.ProgressPercentage == SugarCube.REPORT_STATUS_FLAG ||
			   	e.ProgressPercentage == UploadManager.STATUS_FLAG ||
			   	e.ProgressPercentage == UploadManager.INFO_FLAG ||
			   	e.ProgressPercentage == INFO) {
				LogInfo((string)e.UserState);
			} else if (e.ProgressPercentage == ERROR) {
				LogError((string)e.UserState);
			} else if (e.ProgressPercentage == DEBUG) {
				LogDebug((string)e.UserState);
			} else if (e.ProgressPercentage == SugarCube.TAKE_SNAPSHOT_FLAG) {
				TakeSnapshot(_scan.LocalScanFolder);
				_picSynchronizer.Set();
			} else if (e.ProgressPercentage == UploadManager.SWITCH_TO_CONTINUOUS) {
				progressBar.Value = 0;
				progressBar.Style = ProgressBarStyle.Continuous;
			} else if (e.ProgressPercentage == UploadManager.SWITCH_TO_MARQUEE) {
				progressBar.Value = 100;
				progressBar.Style = ProgressBarStyle.Marquee;
			} else if (e.ProgressPercentage == UploadManager.STATUS_FLAG) {
				LogInfo((string)e.UserState);
			} else if (e.ProgressPercentage == UploadManager.INFO_FLAG) {
				LogInfo((string)e.UserState);
			} else { // We should be gettign an upload precentage from the upload manager
				progressBar.Value = e.ProgressPercentage;
			}			
		}

		private string TakeSnapshot(string savePath) {
			string fileName = "snapshot-" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".jpg";
			Debug.WriteLine("savePath = {0}, fileName = {1}", savePath, fileName);
			if (!Directory.Exists(savePath)) {
				Directory.CreateDirectory(savePath);
			}
			string filePath = Path.Combine(savePath, fileName );
			Debug.WriteLine("filePath = " + filePath);
			LogInfo("Saving snapshot to " + filePath);
			CubeImageUtils.TakeSnapshot(filePath, camControl, _config.JpegQuality,
												 _config.UseMotionDetection, _config.MotionSensitivity,
												 _config.UseMedianStacking,
												 _config.UseContrastEnhancement, _config.ContrastValue, _config.ContrastMidpoint);

			_scan.imageSet.images.Add(new ScanImage(_cube.Elevation.ToString(), _cube.Rotation.ToString(), fileName ));
			return filePath;
		}


		// Helper function for uploading scan
		private void UploadEar(BackgroundWorker worker, string localScanFolder, string host, string user, string pass, string uploadBase, string scanID, string triggerURL) {
			worker.ReportProgress(UploadManager.SWITCH_TO_CONTINUOUS);
			string uploadedPath = _uploader.UploadScan(worker, localScanFolder, host, user, pass, uploadBase, scanID);
			if (uploadedPath.StartsWith(":ERROR:")) {
				throw new Exception(uploadedPath.Remove(0, 7));
			}
			worker.ReportProgress(UploadManager.STATUS_FLAG, "Triggering Remote Processing of Scan");
			_uploader.TriggerScanProcessing(worker, triggerURL, uploadedPath);
		}
		#endregion

		#region UI Interactions
		void BtnStartClick(object sender, EventArgs e) {
			btnStart.Enabled = false;
			udIterations.Enabled = false;
			udDelay.Enabled = false;
			btnCancel.Enabled = true;
			_mainWorker.RunWorkerAsync();
		}

		void BtnCancelClick(object sender, EventArgs e) {
			_mainWorker.CancelAsync();
		}
		#endregion
	}
}
