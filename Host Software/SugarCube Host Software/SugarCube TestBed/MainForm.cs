/* SugarCube Host Software - Test Bed
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using Camera_NET;
using DirectShowLib;
using ImageMagick;
using log4net;
using org.mkulu.config;
using XnaFan.ImageComparison;

namespace Me.ThreeDWares.SugarCube {

	public partial class MainForm : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
		
		#region Class Data, Initialization and Teardown
		private CameraChoice _cameraChoice = new CameraChoice();
		private BackgroundWorker _worker = new BackgroundWorker();

		private Scan _scan;
		private SugarCube _cube = new SugarCube();
		private UploadManager _uploader;
		private ModelingOptions _modelOpts = new ModelingOptions();
		private CubeConfig _config;

		private bool _temporarilyDisableMotionDetection = false;

		/// <summary>
		/// Synchronization event to be used to synchronize between sending commands to the SugarCube and getting the response
		/// </summary>
		private ManualResetEvent _synchronizer = new ManualResetEvent(false);

		public MainForm() {
			InitializeComponent();
			if (log.IsInfoEnabled) {
			    log.Info("Initializing mainform");
			}
			
			this.Text = string.Format(this.Text, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
			_worker.WorkerReportsProgress = true;
			_worker.WorkerSupportsCancellation = true;
			StringCollection args = new StringCollection();
			args.AddRange(Environment.GetCommandLineArgs());
			if (args.Contains("--standalone")) {
				string configPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "AppData");
				_config = new CubeConfig(configPath);
			} else {
				_config = new CubeConfig();
			}
			_uploader = new UploadManager(_config.DataPath);
			// if we're logging, might as well log some of the DirectShow info as well
			if (Directory.Exists(Path.Combine(_config.DataPath, "logs"))) {
				camControl.DirectShowLogFilepath = Path.Combine(_config.DataPath, "logs\\DirectShow.log");
			}
		}

		void MainFormLoad(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Running mainform onload");
			}
			
			if (_config.ConfigFileCreated) { //we need to initialize the sugar cube config from our default set of parameters
				if (log.IsInfoEnabled) {
				    log.Info("Initializing config file from default.cfg");
				}
				
				_config.ImportConfigFile("default.cfg");
			}

			// Try and start the Upload Manager
			try {
				if (log.IsDebugEnabled) {
				    log.Debug("Trying to start the upload manager");
				}
				
				Process.Start("SugarCube Uploader.exe");
			} catch (FileNotFoundException fnf) {
				if (log.IsErrorEnabled) {
				    log.Error("SugarCube Uploader.exe is missing");
				}
				
				LogWriteLn("Unable to start the upload manager, SugarCube Uploader.exe is missing");
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to start upload manager", ex);
				}
				
				LogWriteLn("Unable to start the upload manager: " + ex.Message);
			}

			// Get the list of all ports into cbCOMPortList
			string[] serialPorts = SerialPort.GetPortNames();
			if (serialPorts.Length > 0) {
				cbCOMPortList.Items.AddRange(serialPorts);
				cbCOMPortList.SelectedIndex = 0;
				if (log.IsDebugEnabled) {
				    log.Debug("Found " + serialPorts.Length + " serial ports");
				}
				
			} else {
				if (log.IsErrorEnabled) {
				    log.Error("No serial ports detected on computer");
				}
				
				LogWriteLn("Oops, there are no serial ports avaialbe on this computer!");
			}

			// Fill camera list combobox with available cameras
			FillCameraList();

			// Select the first one
			if (cbCameraList.Items.Count > 0) {
				if (log.IsDebugEnabled) {
				    log.Debug("Initializing camera control, selecting first available camera");
				}
				
				LogWriteLn("Initializing camera control, selecting first available camera");
				cbCameraList.SelectedIndex = 0;
			}

			// Fill the resolutions combo box
			FillResolutionList();

			// Set the options from the config file
			SetOptionsFromConfig();

			// Try and initialize the cube
			try {
				_cube.Initialize();
				LogWriteLn("Cube initialized, the cube is on port " + _cube.PortName);
				if (log.IsDebugEnabled) {
				    log.Debug("Cube initialized, the cube is on port " + _cube.PortName);
				}
				
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Failed to initialize cube", ex);
				}
				
				LogWriteLn("Unable to automatically initialize SugarCube: " + ex.Message);
				return;
			}

			// Read the Z string
			ReadZString(_cube.CubeParameters);

		}

		private void SetOptionsFromConfig() {
			try {
				LogWriteLn("Processing Configuration File:");
				if (log.IsInfoEnabled) {
				    log.Info("Processing config file");
				}
				
				int idx = cbCameraList.Items.IndexOf(_config.Camera);
				if (idx > -1) {
					if (log.IsDebugEnabled) {
					    log.Debug("Setting camera list index to " + idx);
					}
					
					cbCameraList.SelectedIndex = idx;
				} else {
					if (log.IsErrorEnabled) {
					    log.Error("unable to set camera to " + _config.Camera);
					}
					
					LogWriteLn("Error processing config, unable to set camera to " + _config.Camera);
				}
				idx = cbResolutionList.Items.IndexOf(_config.Resolution);
				if (idx > -1) {
					if (log.IsDebugEnabled) {
					    log.Debug("Setting resolution list index to " + idx);
					}
					
					cbResolutionList.SelectedIndex = idx;
				} else {
					if (log.IsErrorEnabled) {
					    log.Error("unable to set resolution to " + _config.Resolution);
					}
					
					LogWriteLn("Error processing config, unable to set resolution to " + _config.Resolution);
				}

				tbShootString.Text = _config.ShootString;
				tbQuality.Value = _config.JpegQuality;
				cbUseMotionDetection.Checked = _config.UseMotionDetection;
				tbSensitivity.Value = _config.MotionSensitivity;
				cbUseMedianStacking.Checked = _config.UseMedianStacking;
				cbUseContrastEnhancement.Checked = _config.UseContrastEnhancement;
				tbMidpoint.Value = _config.ContrastMidpoint;
				tbContrast.Value = _config.ContrastValue;
				cbSetEXIF.Checked = _config.DoSetEXIF;

				tbHost.Text = _config.SftpHost;
				tbUser.Text = _config.SftpUser;
				tbPass.Text = _config.SftpPassword;
				tbEmail.Text = _config.UserEmail;
				tbScanIDPrefix.Text = _config.ShootPrefix;
				tbUploadBase.Text = _config.SftpUploadBase;
				tbTriggerURL.Text = _config.TriggerURL;

				_modelOpts.doImageCropping = _config.DoImageCropping.ToString();
				_modelOpts.imageCroppingRectangle = _config.ImageCropRectangle;
				_modelOpts.doModelScaling = _config.DoModelScaling.ToString();
				_modelOpts.doModelCropping = _config.DoModelCropping.ToString();
				_modelOpts.boundingBox = _config.BoundingBox;
				_modelOpts.meshlevel = _config.MeshLevel.ToString();
				_modelOpts.template3DP = _config.Template3DP;
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Failed to load config from disk", ex);
				}
				
				using (ErrorDialog dlg = new ErrorDialog("Test Bed", "Unable to load the configuration from disk: " + ex.Message, "Click OK to continue", ex.StackTrace)) {
					dlg.ShowDialog();
				}
			}
		}

		private void ReadZString (StringDictionary zData) {
			if (log.IsInfoEnabled) {
			    log.Info("Reading Z string data");
			}
			
			lZString.Text = _cube.LastRead.Split(':')[2].Trim();
			zStringGrid.Rows.Clear();
			StringDictionary zDescriptions = _config.ZKeyDescriptions;
			foreach (string key in zData.Keys) {
				//Note: String dictionaries automatically lowercase all keys, so we reverse that here
				string description = zDescriptions.ContainsKey(key) ? zDescriptions[key] : "";
				zStringGrid.Rows.Add(key.ToUpper(), description, zData[key]);
			}
		}

		void MainFormFormClosed(object sender, FormClosedEventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Preparing to close down");
			}
			
			camControl.CloseCamera();
			_cube.Close();

			// Save the user's selections to file
			try {
				_config.Camera = cbCameraList.SelectedItem.ToString();
				_config.Resolution = cbResolutionList.SelectedItem.ToString();

				_config.ShootString = tbShootString.Text;
				_config.UseMotionDetection = cbUseMotionDetection.Checked;
				_config.MotionSensitivity = tbSensitivity.Value;
				_config.JpegQuality = tbQuality.Value;
				_config.UseMedianStacking = cbUseMedianStacking.Checked;
				_config.UseContrastEnhancement = cbUseContrastEnhancement.Checked;
				_config.ContrastMidpoint = tbMidpoint.Value;
				_config.ContrastValue = tbContrast.Value;

				_config.SftpHost = tbHost.Text;
				_config.SftpUser = tbUser.Text;
				_config.SftpPassword = tbPass.Text;
				_config.ShootPrefix = tbScanIDPrefix.Text;
				_config.UserEmail = tbEmail.Text;
				_config.SftpUploadBase = tbUploadBase.Text;
				_config.TriggerURL = tbTriggerURL.Text;
				_config.DoSetEXIF = cbSetEXIF.Checked;

				_config.DoImageCropping = Convert.ToBoolean(_modelOpts.doImageCropping);
				_config.ImageCropRectangle = _modelOpts.imageCroppingRectangle;
				_config.DoModelScaling = Convert.ToBoolean(_modelOpts.doModelScaling);
				_config.DoModelCropping = Convert.ToBoolean( _modelOpts.doModelCropping);
				_config.BoundingBox = _modelOpts.boundingBox;
				_config.MeshLevel = Convert.ToInt16(_modelOpts.meshlevel);
				_config.Template3DP = _modelOpts.template3DP;
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Failed to save config to disc", ex);
				}
				
				using (ErrorDialog dlg = new ErrorDialog("Test Bed", "Unable to save configurationt to disk: " + ex.Message, "Click OK to continue", ex.StackTrace)) {
					dlg.ShowDialog();
				}
			}
		}

		#endregion

		#region Async event handlers for shoot string and upload processing
		private void ShootCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Error != null) {
				LogWriteLn("An error occured while trying to process the shoot string: " + e.Error.Message);
			}

			if (e.Cancelled) {
				LogWriteLn("Scan cancelled by user");
			}

			if (cbSetEXIF.Checked) {
				string result = "";
				LogWriteLn("Setting Exif data for images, checking Z string for LE value");
				string le = "38";
				try {
					le = _cube.CubeParameters["LE"];
				} catch {
					LogWriteLn("35mm Focal Length Equivalent was NOT found in the Z string, defaulting to 38\n");
				}
				if (String.IsNullOrWhiteSpace(le)) {
					LogWriteLn("35mm Focal Length Equivalent was NOT set in the Z string, defaulting to 38\n");
				}
				try {
					result = result + CubeImageUtils.SetExifInfo(_scan.LocalScanFolder, le);
				} catch (Exception ex) {
					result = ex.Message;
				}
				LogWriteLn(result);
			} else {
				LogWriteLn("Skipping setting EXIF data");
			}

			ToggleUIControls();
		}

		private void ShootReportsProgress(object sender, ProgressChangedEventArgs e) {
			if (e.ProgressPercentage == SugarCube.REPORT_STATUS_FLAG) {
				LogWrite((string)e.UserState);
			} else if (e.ProgressPercentage == SugarCube.TAKE_SNAPSHOT_FLAG) {
				LogWriteLn(" snapshot saved to " + TakeSnapshot(_scan.LocalScanFolder));
				_synchronizer.Set();
			} else if (e.ProgressPercentage == SugarCube.DISABLE_MOTION_FLAG) {
				_temporarilyDisableMotionDetection = true;
			}
		}

		private void ShootDoWork(object sender, DoWorkEventArgs e) {
			BackgroundWorker worker = sender as BackgroundWorker;
			string shootString = (string)e.Argument;
			_cube.ProcessShootString(shootString, worker, e, _synchronizer);
		}

		private void UploadCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Error != null) {
				LogWriteLn("An error occured while trying to upload the shoot data: " + e.Error.Message);
			}
			pbUploadStatus.Value = 0;
			pbUploadStatus.Style = ProgressBarStyle.Continuous;

			ToggleUIControls();
			btnCancelShootString.Enabled = true;

		}

		private void UploadReportsProgress(object sender, ProgressChangedEventArgs e) {
			if (e.ProgressPercentage == UploadManager.INFO_FLAG) {
				LogWriteLn((string)e.UserState);
			} else if (e.ProgressPercentage == UploadManager.DEBUG_FLAG) {
				LogWriteLn("DEBUG: " + (string)e.UserState);
			} else if (e.ProgressPercentage == UploadManager.SWITCH_TO_MARQUEE) {
				pbUploadStatus.Value = 100;
				pbUploadStatus.Style = ProgressBarStyle.Marquee;
			} else {
				pbUploadStatus.Value = e.ProgressPercentage;
			}
		}

		private void UploadDoWork(object sender, DoWorkEventArgs e) {
			BackgroundWorker worker = sender as BackgroundWorker;
			string uploadedPath = _uploader.UploadScan(worker, _scan.LocalScanFolder, tbHost.Text, tbUser.Text, tbPass.Text, tbUploadBase.Text, _scan.metadata.scanID);
			if (uploadedPath.StartsWith(":ERROR:")) {
				worker.ReportProgress(UploadManager.INFO_FLAG, "Error in upload, skipping scan processing trigger");
				return;
			}
			worker.ReportProgress(UploadManager.SWITCH_TO_MARQUEE);
			_uploader.TriggerScanProcessing(worker, tbTriggerURL.Text, uploadedPath);
		}

		#endregion

		#region Camera Control Stuff
		private void FillCameraList() {
			if (log.IsInfoEnabled) {
			    log.Info("Preparing to get list of all cameras");
			}
			
			cbCameraList.Items.Clear();

			try {
				_cameraChoice.UpdateDeviceList();

				foreach (var camera_device in _cameraChoice.Devices) {
					cbCameraList.Items.Add(camera_device.Name);
				}
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Failed to get camera list" , ex);
				}
				
				LogWriteLn("Unable to get the list of attached web cams: " + ex.Message);
			}
		}

		private void FillResolutionList() {
			if (log.IsInfoEnabled) {
			    log.Info("preparing to get list of available resolutions for camera");
			}
			
			cbResolutionList.Items.Clear();

			if (!camControl.CameraCreated) {
				if (log.IsErrorEnabled) {
				    log.Error("no camera selected that we can query the resolution list from");
				}
				
				LogWriteLn("There is currently no camera selected that we can query the resolution list from, something's not right!");
				return;
			}

			try {
				ResolutionList resolutions = Camera.GetResolutionList(camControl.Moniker);

				if (resolutions == null)
					return;

				int index_to_select = -1;

				for (int index = 0; index < resolutions.Count; index++) {
					cbResolutionList.Items.Add(resolutions[index].ToString());

					if (resolutions[index].CompareTo(camControl.Resolution) == 0) {
						index_to_select = index;
					}
				}

				// select current resolution
				if (index_to_select >= 0) {
					cbResolutionList.SelectedIndex = index_to_select;
				}
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Failed to get lis tof resultions", ex);
				}
				
				LogWriteLn("There was a problem while trying to set the resolution list for the camera: " + ex.Message);
			}
		}

		void CbCameraListSelectedIndexChanged(object sender, EventArgs e) {
			if (cbCameraList.SelectedIndex < 0) {
				camControl.CloseCamera();
			} else {
				try {
					DsDevice chosenCam = _cameraChoice.Devices[cbCameraList.SelectedIndex];
					if (log.IsInfoEnabled) {
					    log.Info("Changing camera to " + chosenCam.Name);
					}
					
					LogWriteLn("Setting camera to " + chosenCam.Name + Environment.NewLine);
					camControl.SetCamera(chosenCam.Mon, null);
				} catch (Exception ex) {
					if (log.IsErrorEnabled) {
					    log.Error("Failed to change camera", ex);
					}
					
					LogWriteLn("Unable to open selected camera: " + ex.Message);
				}
			}

			FillResolutionList();
		}

		// The trick here is that the index of the selected resolution in the combo box cbResolutionList
		// will match the index in the ResoltionsList returned by Camera.GetResolutionList
		void CbResolutionListSelectedIndexChanged(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Changing camera resolution");
			}
			
			if (!camControl.CameraCreated)
				return;

			int resolutionIndex = cbResolutionList.SelectedIndex;
			if (resolutionIndex < 0) {
				return;
			}
			try {
				ResolutionList resolutions = Camera.GetResolutionList(camControl.Moniker);

				if ( resolutions == null )
					return;

				if ( resolutionIndex >= resolutions.Count )
					return; // throw

				if (resolutions[resolutionIndex].CompareTo(camControl.Resolution) == 0) {
					// this resolution is already selected
					return;
				}

				// Recreate camera
				if (log.IsInfoEnabled) {
				    log.Info("Setting camera resolution to " + resolutions[resolutionIndex].ToString());
				}
				
				LogWriteLn("Setting camera resolution to " + resolutions[resolutionIndex].ToString() + Environment.NewLine);
				camControl.SetCamera(camControl.Moniker, resolutions[resolutionIndex]);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to change resolution", ex);
				}
				
				LogWriteLn("Unable to set selected resolution: " + ex.Message);
			}
		}


		private string TakeSnapshot() {
			if (log.IsInfoEnabled) {
			    log.Info("TakeSnapshot called using %TEMP% as path");
			}
			
			return TakeSnapshot(Path.GetTempPath());
		}

		private string TakeSnapshot(string savePath) {
			if (log.IsInfoEnabled) {
			    log.Info("TakeSnapshot called using savePath = " + savePath);
			}
			
			string fileName = "snapshot-" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".jpg";
			if (log.IsDebugEnabled) {
			    log.Debug("Snapshot file name is " + fileName);
			}
			
			if (!Directory.Exists(savePath)) {
				if (log.IsDebugEnabled) {
				    log.Debug("savepath does not exist, creating");
				}
				
				Directory.CreateDirectory(savePath);
			}
			string filePath = Path.Combine(savePath, fileName );
			bool useMotionDetection = cbUseMotionDetection.Checked && !_temporarilyDisableMotionDetection;
			if (log.IsDebugEnabled) {
			    log.Debug("use motion detection = " + useMotionDetection);
			}
			
			CubeImageUtils.TakeSnapshot(filePath, camControl, tbQuality.Value,
												 useMotionDetection, tbSensitivity.Value,
												 cbUseMedianStacking.Checked,
												 cbUseContrastEnhancement.Checked, tbContrast.Value, tbMidpoint.Value);

			_scan.imageSet.images.Add(new ScanImage(_cube.Elevation.ToString(), _cube.Rotation.ToString(), fileName));
			_temporarilyDisableMotionDetection = false;
			return filePath;
		}
		#endregion

		#region Logging
		private void LogWriteLn(string message) {
			tbTesterLog.AppendText(message + Environment.NewLine);
		}

		private void LogWrite(string message) {
			tbTesterLog.AppendText(message);
		}

		private void InitializeScan(string scanIDPrefix, string imageSetID) {
			if (log.IsInfoEnabled) {
			    log.Info("Initializing new scan");
			}
			
			_scan = new Scan();
			_scan.metadata.scanID = scanIDPrefix + DateTime.Now.ToString("yyyyMMddHHmm");

			_scan.orderDetails.from = tbEmail.Text;
			_scan.orderDetails.to = tbEmail.Text;
			_scan.orderDetails.referenceID = _scan.metadata.scanID;
			_scan.orderDetails.subject = "SugarCube Tester Generated Scan";

			_scan.imageSet.id = imageSetID;

			_scan.diagnostics.cameraID = cbCameraList.SelectedItem.ToString();
			_scan.diagnostics.imageResolution = cbResolutionList.SelectedItem.ToString();
			_scan.diagnostics.jpegQuality = tbQuality.Value.ToString();
			_scan.diagnostics.COMport = _cube.PortName;
			_scan.diagnostics.forceCOMPort = (cbForceCOMPort.Checked) ? "true" : "false";
			_scan.diagnostics.motion_detection = (cbUseMotionDetection.Checked) ? "on" : "off";
			_scan.diagnostics.motion_sensitivity = tbSensitivity.Value.ToString();

			_scan.zData = _cube.CubeParameters;
		}

		private void ShowError(string message) {
			MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		#endregion

		#region UI Events and Interactions
		void BtnSettingsClick(object sender, EventArgs e) {
			if (camControl.CameraCreated) {
				Camera.DisplayPropertyPage_Device(camControl.Moniker, this.Handle);
			}
		}

		void TbSingleCommandKeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				BtnGoClick(this, null);
			}
		}

		void BtnGoClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("perparing to send single command to cube");
			}
			
			if (!_cube.Initialized) {
				if (log.IsDebugEnabled) {
					log.Debug("initializing cube");
				}
				
				try {
					if (cbForceCOMPort.Checked == true) {
						_cube.Initialize(cbCOMPortList.Text);
					} else {
						_cube.Initialize();
					}
				} catch (Exception ex) {
					if (log.IsErrorEnabled) {
					    log.Error("failed to initialize cube", ex);
					}
					
					LogWriteLn("Unable to initialize cube: " + ex.Message);
					return;
				}
				// Read the Z string
				ReadZString(_cube.CubeParameters);
			}

			if (log.IsDebugEnabled) {
			    log.Debug("Sending command " + tbSingleCommand.Text + " to the SugarCube");
			}
			
			LogWriteLn("Sending command " + tbSingleCommand.Text + " to the SugarCube");
			string res = _cube.PostCommand(tbSingleCommand.Text);
			if (log.IsDebugEnabled) {
			    log.Debug("cube response was: " + res);
			}
			
			LogWriteLn("<-- " + res);
		}

		void BtnSnapshotClick(object sender, EventArgs e) {
			LogWriteLn("Saved snapshot to " + TakeSnapshot());
		}

		private void ToggleUIControls() {
			// Camera Settings
			cbCameraList.Enabled = !cbCameraList.Enabled;
			cbResolutionList.Enabled = !cbResolutionList.Enabled;
			btnSettings.Enabled = !btnSettings.Enabled;
			btnSnapshot.Enabled = !btnSnapshot.Enabled;

			//Options and Settings Tab
			tbSingleCommand.Enabled = !tbSingleCommand.Enabled;
			btnGo.Enabled = !btnGo.Enabled;
			tbQuality.Enabled = !tbQuality.Enabled;
			cbForceCOMPort.Enabled = !cbForceCOMPort.Enabled;
			cbCOMPortList.Enabled = (cbForceCOMPort.Enabled && cbForceCOMPort.Checked);
			cbUseMotionDetection.Enabled = !cbUseMotionDetection.Enabled;
			tbSensitivity.Enabled = !tbSensitivity.Enabled;
			btnSetModellingOpts.Enabled = !btnSetModellingOpts.Enabled;

			//Shoot String Tab
			tbShootString.Enabled = !tbShootString.Enabled;
			btnRunShootString.Enabled = !btnRunShootString.Enabled;
			btnOpenShootFolder.Enabled = !btnOpenShootFolder.Enabled;
			btnGenerateXMLManifest.Enabled = !btnGenerateXMLManifest.Enabled;
			btnCreateDummyForms.Enabled = !btnCreateDummyForms.Enabled;
			btnUploadScan.Enabled = !btnUploadScan.Enabled;
			tbEmail.Enabled = !tbEmail.Enabled;
			tbHost.Enabled = !tbHost.Enabled;
			tbUser.Enabled = !tbUser.Enabled;
			tbPass.Enabled = !tbPass.Enabled;
			tbScanIDPrefix.Enabled = !tbScanIDPrefix.Enabled;
			tbUploadBase.Enabled = !tbUploadBase.Enabled;
			tbTriggerURL.Enabled = !tbTriggerURL.Enabled;
		}

		void BtnRunShootStringClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Preparing to run shoot string");
			}
			
			btnRunShootString.Enabled = false;
			if (String.IsNullOrWhiteSpace(tbShootString.Text)) {
				if (log.IsWarnEnabled) {
				    log.Warn("No shoot string specified");
				}
				
				LogWriteLn("You actually have to provide a shooting string to run you know.");
				btnRunShootString.Enabled = true;
				return;
			}

			if (tbScanIDPrefix.Text == "<prefix>" || String.IsNullOrWhiteSpace(tbScanIDPrefix.Text)) {
				if (log.IsWarnEnabled) {
				    log.Warn("Scan prefix is not set");
				}
				
				LogWriteLn("The scan ID prefix must be set before you can run a shoot string");
				btnRunShootString.Enabled = true;
				return;
			}

			if (!_cube.Initialized) {
				if (log.IsDebugEnabled) {
				    log.Debug("Need to initialize cube");
				}
				
				try {
					if (cbForceCOMPort.Checked == true) {
						_cube.Initialize(cbCOMPortList.Text);
					} else {
						_cube.Initialize();
					}
				} catch (Exception ex) {
					if (log.IsErrorEnabled) {
					    log.Error("Failed to initialize cube", ex);
					}
					
					LogWriteLn("Unable to initialize cube: " + ex.Message);
					btnRunShootString.Enabled = true;
					return;
				}
				ReadZString(_cube.CubeParameters);
			}
			btnRunShootString.Enabled = true;
			ToggleUIControls();

			InitializeScan(tbScanIDPrefix.Text, "TesterScan");
			_scan.LocalScanFolder = Path.Combine(_config.DataPath, _scan.metadata.scanID);

			lSourceFolder.Text = _scan.LocalScanFolder;
			_scan.diagnostics.shootingString = tbShootString.Text.Replace(Environment.NewLine, " ");

			// remove all existing event handlers from our worker
			_worker.DoWork -= UploadDoWork;
			_worker.ProgressChanged -= UploadReportsProgress;
			_worker.RunWorkerCompleted -= UploadCompleted;
			_worker.DoWork -= ShootDoWork;
			_worker.ProgressChanged -= ShootReportsProgress;
			_worker.RunWorkerCompleted -= ShootCompleted;

			// and add just our shoot event handlers
			_worker.DoWork += ShootDoWork;
			_worker.ProgressChanged += ShootReportsProgress;
			_worker.RunWorkerCompleted += ShootCompleted;

			_worker.RunWorkerAsync(tbShootString.Text);
		}

		void BtnOpenShootFolderClick(object sender, EventArgs e) {
			if (!Directory.Exists(_scan.LocalScanFolder)) {
				MessageBox.Show("No images were captured in this shoot, so the shoot folder was never created");
				return;
			}
			System.Diagnostics.Process.Start(_scan.LocalScanFolder);
		}

		void BtnGenerateXMLManifestClick(object sender, EventArgs e) {
			if (!Directory.Exists(_scan.LocalScanFolder)) {
				Directory.CreateDirectory(_scan.LocalScanFolder);
			}

			_scan.modelingOptions = _modelOpts;
			LogWriteLn("Creating XML manifest file under shoot folder" + _scan.LocalScanFolder);
			try {
				_scan.WriteManifest();
			} catch (Exception ex) {
				LogWriteLn("Failed to generate manifest: " + ex.Message);
			}
		}

		void BtnCreateDummyFormsClick(object sender, EventArgs e) {
			if (!Directory.Exists(_scan.LocalScanFolder)) {
				Directory.CreateDirectory(_scan.LocalScanFolder);
			}
			string pdfBase = Path.Combine(_scan.LocalScanFolder, "ORDER_tester_010101.pdf.hcrypt.");
			File.WriteAllText(pdfBase + "data", "");
			_scan.orderDetails.attachments.Add("ORDER_tester_010101.pdf.hcrypt." + "data");
			File.WriteAllText(pdfBase + "head", "");
			_scan.orderDetails.attachments.Add("ORDER_tester_010101.pdf.hcrypt." + "head");
			LogWriteLn("Created dummy form head and data in " + _scan.LocalScanFolder);
		}

		void BtnUploadScanClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Preparing to call background worker to upload scan");
			}
			
			ToggleUIControls();
			btnCancelShootString.Enabled = false;
			_scan.modelingOptions = _modelOpts;
			_scan.metadata.uploadStartTime = DateTime.Now.ToString(Scan.DATE_FORMAT);
			_scan.WriteManifest();

			// remove all the (potential) event handlers from our worker
			_worker.DoWork -= UploadDoWork;
			_worker.ProgressChanged -= UploadReportsProgress;
			_worker.RunWorkerCompleted -= UploadCompleted;
			_worker.DoWork -= ShootDoWork;
			_worker.ProgressChanged -= ShootReportsProgress;
			_worker.RunWorkerCompleted -= ShootCompleted;

			// and add just our upload event handlers
			_worker.DoWork += UploadDoWork;
			_worker.ProgressChanged += UploadReportsProgress;
			_worker.RunWorkerCompleted += UploadCompleted;

			_worker.RunWorkerAsync();
		}

		void BtnSendToUploadManagerClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Adding new scan to upload manager queue");
			}
			
			ToggleUIControls();
			_scan.modelingOptions = _modelOpts;
			_scan.metadata.uploadStartTime = DateTime.Now.ToString(Scan.DATE_FORMAT);
			_scan.WriteManifest();
			_uploader.AddScanToQueue(_scan.metadata.scanID);
			string message = "The order has been added to the upload queue and will be processed by the upload manager.";
			if (!_uploader.CheckForInternetConnection()) {
				message = "There is currently no internet connection on this machine.  The order has been added to the upload queue and will be processed by the upload manager once internet connetivity is restored.";
			}
			LogWriteLn(message);
			ToggleUIControls();

		}


		void CbCOMPortCheckedChanged(object sender, EventArgs e) {
			cbCOMPortList.Enabled = cbForceCOMPort.Checked;
			// If this changed, assume our SugarCube needs to be re-initialized
			_cube.Initialized = false;
		}

		void TbQualityValueChanged(object sender, EventArgs e) {
			lJPEGQuality.Text = "JPEG Quality [" + tbQuality.Value + "]";
		}

		void TbSensitivityValueChanged(object sender, EventArgs e) {
			if (tbSensitivity.Value == 0) {
				DialogResult res = MessageBox.Show("Are you really sure you want to set motion sensitivity all the way to zero and not to one?\n\nAt this level the chances are good you will never take a picture due to the simple white noise caused by somic rays impacting on the CCD or something!",
															  "DANGER WILL ROBINSON, DANGER",
															  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (res == DialogResult.No) {
					tbSensitivity.Value = 1;
				}
			}
			lSensitivity.Text = "Motion Sensitivity [" + tbSensitivity.Value + "]\n(larger = less sensitive)";
		}

		void CbUseContrastEnhancementCheckedChanged(object sender, EventArgs e) {
			tbContrast.Enabled = cbUseContrastEnhancement.Checked;
			tbMidpoint.Enabled = cbUseContrastEnhancement.Checked;
		}

		void TbMidpointValueChanged(object sender, EventArgs e) {
			lMidPoint.Text = "Contrast Midpoint [" + tbMidpoint.Value + "%]";
		}

		void TbContrastValueChanged(object sender, EventArgs e) {
			lContrastValue.Text = "Contrast Value [" + tbContrast.Value + "]";
		}

		void CancelShootString(object sender, EventArgs e) {
			_worker.CancelAsync();
		}

		void BtnSetModellingOptsClick(object sender, EventArgs e) {
			ModellingOptionsDialog dlg = new ModellingOptionsDialog(_modelOpts);
			dlg.ShowDialog();
			_modelOpts = dlg.settings;
			dlg.Dispose();
		}

		void BtnAddZKeyClick(object sender, EventArgs e) {
			NewZKeyDialog dlg = new NewZKeyDialog();
			dlg.ShowDialog();
			zStringGrid.Rows.Add(dlg.NewValue, "", "");
		}

		void BtnDeleteZKeyClick(object sender, EventArgs e) {
			if (zStringGrid.SelectedRows.Count > 0) {
				DialogResult res = MessageBox.Show("Are you sure you want to delete this key?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.Yes) {
					zStringGrid.Rows.RemoveAt(zStringGrid.SelectedRows[0].Index);
				}
			}
		}

		void BtnUpdateZStringClick(object sender, EventArgs e) {
			string newZstring = "";
			StringDictionary newZKeyDescriptions = new StringDictionary();
			foreach(DataGridViewRow row in zStringGrid.Rows) {
				newZstring = newZstring + String.Format("{0}={1};", row.Cells[0].Value, row.Cells[2].Value);
				newZKeyDescriptions.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
			}

			DialogResult res = MessageBox.Show("Do you want to set the Z string as shown below?\n\n" + newZstring, "Set Z String", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (res == DialogResult.Yes) {
				_config.ZKeyDescriptions = newZKeyDescriptions;
				ReadZString(_cube.SetCubeParameters(newZstring));
			}
		}

		void BtnZRefreshClick(object sender, EventArgs e) {
			DialogResult res = MessageBox.Show("If you have any unsaved changes they will be lost. Do you want to proceed with the refresh?", "Confirmation Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (res == DialogResult.Yes) {
				ReadZString(_cube.CubeParameters);
			}
		}

		// Even though the ShortcutsEnabled property of this text box is true, we still need this to make ^A work properly.
		// Here's this little gem from http://msdn.microsoft.com/en-us/library/system.windows.forms.textboxbase.shortcutsenabled%28v=vs.100%29.aspx
		// 'Note: The TextBox control does not support the CTRL+A shortcut key when the Multiline property value is true.'
		void TbShootStringKeyDown(object sender, KeyEventArgs e) {
			if (e.Control && (e.KeyCode == Keys.A)) {
				if (sender != null)
					((TextBox)sender).SelectAll();
				e.Handled = true;
			}
		}

		void BtnExportConfigClick(object sender, EventArgs e) {
			if (exportFileDialog.ShowDialog() == DialogResult.OK) {
				_config.ExportConfigFile(exportFileDialog.FileName);
			}
		}

		void BtnImportConfigClick(object sender, EventArgs e) {
			if (importFileDialog.ShowDialog() == DialogResult.OK) {
				_config.ImportConfigFile(importFileDialog.FileName);
				SetOptionsFromConfig();
			}
		}

		// Works in conjunction with TbScanIDPrefixKeyPress to make sure the user is not entering an invalid Shoot String ID
		void TbScanIDPrefixKeyDown(object sender, KeyEventArgs e) {
			_config.CheckKeyIsValidForScanID(e);
		}

		// If the key is not allowed, we set handled to be true which means that the keypress never makes it all the way to the control.
		// We also pop up a warning dialog
		void TbScanIDPrefixKeyPress(object sender, KeyPressEventArgs e) {
			e.Handled = !_config.IsValidKeyForScanID;
			if (e.Handled) {
				MessageBox.Show("Scan ID Prefix can only contain the characters A-Z, a-z, 0-9 and -", "Configuration Error",
									 MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion


	}
}


/* Notes:
 * http://www.codeproject.com/Articles/9299/Comparing-Images-using-GDI
 */