/* SugarCube Host Software - SugarCube Uploader
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using log4net;
using System.Data.SQLite;

namespace Me.ThreeDWares.SugarCube {
	public partial class UploadForm : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(UploadForm));

		public BackgroundWorker worker = new BackgroundWorker();
		private CubeConfig _config = new CubeConfig();
		private UploadManager _uploader;
		private NotifyIcon _icon;
		private List<UploadQueueEntry> _scans;

		public UploadForm(NotifyIcon theIcon) {
			if (log.IsInfoEnabled) {
				log.Info("Creating upload form");
			}

			InitializeComponent();
			_icon = theIcon;
			_icon.Text = "SugarCube Upload Manager";

			this.Text = string.Format(this.Text, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

			tbLog.Text = "";

			_uploader = new UploadManager(_config.DataPath);
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.ProgressChanged += Uploader_ProgressChanged;
			worker.RunWorkerCompleted += Uploader_WorkCompleted;
			worker.DoWork += Uploader_DoWork;

			timer.Tick += new EventHandler(CheckForNewScans);
			timer.Interval = 10 * 1000; //every 10 seconds
			//CheckForNewScans(null, null);
			timer.Enabled = true;
		}

		void UploadFormLoad(object sender, EventArgs e) {
			queueGrid.Columns[0].DataPropertyName = "Id";
			queueGrid.Columns[1].DataPropertyName = "Created";
			queueGrid.Columns[2].DataPropertyName = "Started";
			queueGrid.Columns[3].DataPropertyName = "Uploaded";
			queueGrid.Columns[4].DataPropertyName = "Triggered";
			queueGrid.Columns[5].DataPropertyName = "UploadDir";
			UpdateQueueGrid();
		}

		private void UpdateQueueGrid() {
			_scans = _uploader.GetAllUploadQueueItems();
			bindingSource.DataSource = _scans;
			if (queueGrid.RowCount > 0) {
				queueGrid.FirstDisplayedScrollingRowIndex = queueGrid.RowCount-1;
			}
		}

		void CheckForNewScans(object sender, EventArgs e) {
			UpdateQueueGrid();

			if (log.IsInfoEnabled) {
				log.Info("Checking internet connection");
			}

			if (!_uploader.CheckForInternetConnection()) {
				if (log.IsInfoEnabled) {
					log.Info("No internet connection found");
				}
				SetUI("No Internet Connection", "No Internet Connection currently available, scans will be queued until the system is online again", 0);
				return;
			}


			if (log.IsInfoEnabled) {
				log.Info("Checking for new scans");
			}

			if (worker.IsBusy) { //don't do anything if we're already processing the queue
				if (log.IsDebugEnabled) {
					log.Debug("Worker is busy");
				}

				return;
			}

			if (log.IsDebugEnabled) {
				log.Debug("Running background worker");
			}

			worker.RunWorkerAsync();
		}

		void BtnCancelClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("User selected cancel button");
			}

			DialogResult res = MessageBox.Show("If you exit the upload manager any currently running uploads will be stopped and will restart the next time the upload manager is started.\n\nAre you sure you want to exit?",
														  "SugarCube Uploade Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (res == DialogResult.Yes) {
				if (log.IsDebugEnabled) {
					log.Debug("user confirmed exit, sending canellation message to worker");
				}

				worker.CancelAsync();
				SetUI("Stopping Upload", "", 0);
				btnOK.Enabled = false;
				btnCancel.Enabled = false;
				while (worker.IsBusy) {
					Thread.Sleep(100);
				}
				if (log.IsDebugEnabled) {
					log.Debug("Worker is done, exiting");
				}

				this.Close();
			}
		}

		void BtnOKClick(object sender, EventArgs e) {
			this.Hide();
		}


		private void Uploader_DoWork(object sender, DoWorkEventArgs e) {
			BackgroundWorker worker = sender as BackgroundWorker;
			string host = _config.SftpHost;
			string user = _config.SftpUser;
			string pass = _config.SftpPassword;
			string uploadBase = _config.SftpUploadBase;
			string triggerURL = _config.TriggerURL;

			_uploader.ProcessQueuedScans(worker, host, user, pass, uploadBase, triggerURL);
		}

		private void Uploader_WorkCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Error != null) {
				if (e.Error is InternetConnectionException) {
					if (log.IsInfoEnabled) {
						log.Info("No internet connection found");
					}
					SetUI("No Internet Connection", "No Internet Connection currently available, scans will be queued until the system is online again", 0);
					return;
				}

				SetUI("Unable To Upload Scan", e.Error.Message, 0);
				return;
			}

			if (e.Cancelled) {
				SetUI("Upload Cancelled", "", 0);
				return;
			}

			SetUI("All Currently Queued Scans Uploaded", "", 0);
		}

		private void Uploader_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			if (log.IsDebugEnabled) {
				log.DebugFormat("Uploader progress changed [{0}] {1}", e.ProgressPercentage, (string)e.UserState);
			}

			if (e.ProgressPercentage == UploadManager.DEBUG_FLAG) { // do nothing with debug messages
				return;
			}
			if (e.ProgressPercentage == UploadManager.SWITCH_TO_CONTINUOUS) {
				pbProgress.Value = 0;
				pbProgress.Style = ProgressBarStyle.Continuous;
			} else if (e.ProgressPercentage == UploadManager.SWITCH_TO_MARQUEE) {
				pbProgress.Value = 100;
				pbProgress.Style = ProgressBarStyle.Marquee;
			} else if (e.ProgressPercentage == UploadManager.STATUS_FLAG ||
						  e.ProgressPercentage == UploadManager.INFO_FLAG) {
				WriteLog((string)e.UserState);
			} else if (e.ProgressPercentage == UploadManager.ERROR_FLAG) {
				WriteLog("ERROR: " + (string)e.UserState);
			} else if (e.ProgressPercentage == UploadManager.QUEUE_FLAG) {
				SetUI((string)e.UserState, "", 0);
			} else {
				pbProgress.Value = e.ProgressPercentage;
			}
		}


		private void SetUI(string headerText, string actionText, int progress) {
			if (log.IsDebugEnabled) {
				log.DebugFormat("In SetUI [{0}] ({1}) {2}", progress, headerText, actionText);
			}

			lHeader.Text = headerText;
			// there is a 64 character limit on the tool tip length for a notification icon
			_icon.Text = headerText.Length > 64 ? headerText.Substring(0, 64) : headerText;
			if (!String.IsNullOrWhiteSpace(actionText))  {
				WriteLog(actionText);
			}
			pbProgress.Value = progress;
		}

		private void WriteLog(string message) {
			if (log.IsDebugEnabled) {
				log.Debug("In WriteLog: " + message);
			}

			tbLog.AppendText(String.Format("{0}> {1}\n", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), message));
		}



		void QueueGridCellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
			if (e.ColumnIndex == 5) { // handle the Notes column
				string note = (string)e.Value;
				if (note != null && note.StartsWith("ERROR ")) {
					note = note.Substring(6);
				} else {
					note = "";
				}
				e.Value = note;
				e.FormattingApplied = true;
			}
		}
	}
}