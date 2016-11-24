/* SugarCube Host Software - Manager MVP
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
		private const string LEFT_EAR = "LeftEar";
		private const string RIGHT_EAR = "RightEar";
		private const bool ENABLED = true;
		private const bool DISABLED = false;
		private const string CONFIG_PASS = "SugarCube4u!"; //TODO: Make sure this gets encrypted

		private Color _buttonBackBlue = Color.FromArgb(202, 224, 246);
		private Color _darkTextBlue = Color.FromArgb(96, 134, 181);
		private Color _lightTextBlue = Color.FromArgb(165,203,240);

		private Font _skipLabelFontBold;
		private Font _skipLabelFontNormal;
		private Font _skipLabelFontDisabled;

		private int _currentStep;
		private string _currentEar;

		private bool _skipOrderForm = false;

		private StringBuilder _log = new StringBuilder();

		private EmailToCollection _emailsTo;

		private CubeConfig _config = new CubeConfig();
		private Scan _leftScan = null;
		private Scan _rightScan = null;
		private ModelingOptions _modelOpts = new ModelingOptions();
		private SugarCube _cube = new SugarCube();
		private UploadManager _uploader;
		private ManualResetEvent _synchronizer = new ManualResetEvent(false);
		private string _35mmFocalEquivalent = "38"; // The default FLE is 38 if it's not set in the Z data
		#endregion

		#region Initialization and Shutdown
		public MainForm() {
			if (log.IsInfoEnabled) {
				log.Info("initializing main form");
			}

			InitializeComponent();
			this.Text = string.Format(this.Text, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
			_skipLabelFontNormal = lSkipLeftEar.Font;
			_skipLabelFontBold = new Font(_skipLabelFontNormal, FontStyle.Bold | FontStyle.Underline);
			_skipLabelFontDisabled = new Font(lSkipLeftEar.Font, FontStyle.Regular);
			_modelOpts.doImageCropping = _config.DoImageCropping.ToString();
			_modelOpts.imageCroppingRectangle = _config.ImageCropRectangle;
			_modelOpts.doModelScaling = _config.DoModelScaling.ToString();
			_modelOpts.doModelCropping = _config.DoModelCropping.ToString();
			_modelOpts.boundingBox = _config.BoundingBox;
			_modelOpts.meshlevel = _config.MeshLevel.ToString();
			_modelOpts.template3DP = _config.Template3DP;
			//Do this now so we can get the local config folder and thus initialize the DB for scans
			_uploader= new UploadManager(_config.DataPath);
			_emailsTo = new EmailToCollection(_config.DataPath);
			tbTo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			tbTo.AutoCompleteSource = AutoCompleteSource.CustomSource;
			tbTo.AutoCompleteCustomSource = _emailsTo.data;
		}

		private void SetCameraControlSourceAndResolution (string camName, string res) {
			if (log.IsInfoEnabled) {
				log.InfoFormat("Attempting to set camera to {0} at res {1}", camName, res);
			}

			CameraChoice chooser = null;
			DsDevice chosenCam = null;
			ResolutionList resolutions = null;
			Camera_NET.Resolution chosenRes = null;

			try {
				chooser = new CameraChoice();
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to initialize the CameraChoice object", ex);
				}

				throw new Exception("Unable to initialize the CameraChoice object");
			}

			try {
				chooser.UpdateDeviceList();
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to update the device list", ex);
				}

				throw new Exception("Unable to update the device list - " + ex.Message);
			}

			try {
				chosenCam = chooser.GetCameraByName(camName);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to get the chosen camera by name:" + camName, ex);
				}

				throw new Exception("Unable to get the chosen camera by name (" + camName + ") - " + ex.Message);
			}

			if (chosenCam == null) {
				if (log.IsErrorEnabled) {
					log.Error("The chosen camera, " + camName + " does not exist on this system");
				}

				throw new Exception("The chosen camera, " + camName + " does not exist on this system");
			}

			try {
				resolutions = Camera.GetResolutionList(chosenCam.Mon);
			}	catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to get the list of available resolutions from the camera", ex);
				}

				throw new Exception("Unable to get the list of available resolutions from the camera - " + ex.Message);
			}

			try {
				chosenRes = resolutions.Find(x => x.ToString() == res);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to find the chosen resolution (" + res + ") or a default for the camera", ex);
				}

				throw new Exception("Unable to find the chosen resolution (" + res + ") or a default for the camera - " + ex.Message);
			}

			if (chosenRes == null) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to find " + res + " in the list of available camera resolutions, initializing camera with default resolution");
				}
			}

			try {
				camControl.SetCamera(chosenCam.Mon, chosenRes);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to set the camera control's camera to " + camName + " at " + res, ex);
				}

				throw new Exception("Unable to set the camera control's camera to " + camName + " at " + res + " - " + ex.Message);
			}
		}

		void MainFormLoad(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Running onLoad event handler");
			}

			if (_config.ConfigFileCreated) { //we need to initialize the sugar cube config from our default set of parameters
				if (log.IsInfoEnabled) {
					log.Info("Importing default config file");
				}

				_config.ImportConfigFile("default.cfg");
			}

			if (_config.ShootPrefix == "") { //prefix is not yet set or we just initialized the config
				if (log.IsInfoEnabled) {
					log.Info("Shoot prefix not set, prompting for email");
				}

				UserEmailDialog dlg = new UserEmailDialog(_config);
				if (dlg.ShowDialog() == DialogResult.OK) {
					_config.UserEmail = dlg.EMail;
					_config.ShootPrefix = _config.EmailToPrefix(dlg.EMail);
					_config.SftpUploadBase = _config.SftpUploadBase + _config.UserEmail;
				} else {
					if (log.IsFatalEnabled) {
						log.Fatal("User chose not to give email address, aborting");
					}

					ShowFatalError("You will not be able to run any scans until you set your email address");
					_config.RemoveConfigFile();
					this.Close();
					return;
				}
			}

			StringCollection args = new StringCollection();
			args.AddRange(Environment.GetCommandLineArgs());
			bool testMode = args.Contains("--force-test-mode");
			if (testMode) {
				if (log.IsInfoEnabled) {
					log.Info("TEST MODE ENABLED");
				}

				this.Text = this.Text + " - TEST MODE";
				SetStep0();
				return;
			}

			// Initialize the cube first.  Then, we can check if there is a camera and resolution set in the
			// Z string that we should default to (overriding anything set in the config file)
			try {
				if (log.IsInfoEnabled) {
					log.Info("Initializing cube");
				}
				_cube.Initialize();
				// Set the status LED to bright blue
				if (log.IsDebugEnabled) {
					log.Debug("setting led to bright blue");
				}

				_cube.PostCommand("LO");
				_cube.PostCommand("LB200");
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Error during sugarcube initialization", ex);
				}
				ShowFatalError("Unable to initialize the SugarCube", ex.Message + "\n" + ex.StackTrace);
				this.Close();
				return;
			}

			if (log.IsInfoEnabled) {
				log.Info("preparing to read LE from cube parameters");
			}

			string le = "38";
			try {
				le = _cube.CubeParameters["LE"];
			} catch {
				if (log.IsWarnEnabled) {
					log.Warn("35mm Focal Length Equivalent was NOT found in the Z string, defaulting to 38");
				}
			}
			if (String.IsNullOrWhiteSpace(le)) {
				if (log.IsWarnEnabled) {
					log.Warn("35mm Focal Length Equivalent was NOT set in the Z string, defaulting to 38");
				}
			} else {
				_35mmFocalEquivalent = le;
			}

			if (log.IsInfoEnabled) {
				log.Info("preparing to set camera control and resultion");
			}

			string cameraToUse = "";

			if (log.IsInfoEnabled) {
				log.Info("preparing to read CN from cube parameters, camera name in config is " + _config.Camera);
			}

			try {
				cameraToUse = _cube.CubeParameters["CN"];
				if (String.IsNullOrWhiteSpace(cameraToUse)) {
					if (log.IsWarnEnabled) {
						log.Warn("Camera Name was not set in the Z string, defaulting to camera name in config file");
					}
					cameraToUse = _config.Camera;
				}
			} catch {
				if (log.IsWarnEnabled) {
					log.Warn("Camera Name was not found in the Z string, defaulting to camera name in config file");
				}
				cameraToUse = _config.Camera;
			}
			if (log.IsInfoEnabled) {
				log.Info("Camera name set to " + cameraToUse);
			}

			if (log.IsInfoEnabled) {
				log.Info("preparing to set camera control and resultion");
			}

			try {
				SetCameraControlSourceAndResolution(cameraToUse, _config.Resolution);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Error during camera initialization", ex);
				}

				ShowFatalError("Unable to initialize the SugarCube camera", ex.Message + "\n" + ex.StackTrace);
				this.Close();
				return;
			}

			// Check if we have an order form to complete or not, and update the UI if necessary
			bool orderFormExists = false;

			if (File.Exists("OrderForm.pdf")) {
				FileInfo orderTemplateInfo = new FileInfo("OrderForm.pdf");
				if (orderTemplateInfo.Length > 0) {
					if (log.IsInfoEnabled) {
						log.Info("Order form template is not empty or missing, UI will reflect this");
					}
					orderFormExists = true;
				} else {
					if (log.IsInfoEnabled) {
						log.Info("Order form template is empty, adjusting UI");
					}
				}
			} else {
				if (log.IsWarnEnabled) {
					log.Warn("Order form template is missing, someone's been mucking about in the program files folder");
				}
			}

			if (!orderFormExists) {
				_skipOrderForm = true;
				label9.Visible = false;
				lCompleteOrderForm.Visible = false;
				panel5.Visible = false;
				label11.Text = "Step 4";
			}

			// Find the brand logo and update the UI
			if (File.Exists("BrandLogo.bmp")) {
				if (log.IsDebugEnabled) {
					log.Debug("Found brand logo, updating UI");
				}

				pbPartnerLogo.Image = new Bitmap("BrandLogo.bmp");
			}

			if (log.IsInfoEnabled) {
				log.Info("Attempting to start the uploader");
			}

			Process.Start("SugarCube Uploader.exe");

			this.WindowState = FormWindowState.Minimized;
			this.Show();
			this.WindowState = FormWindowState.Normal;

			SetStep0();
		}

		void MainFormFormClosed(object sender, FormClosedEventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Closing down");
			}

			if (camControl.Camera != null) {
				if (log.IsInfoEnabled) {
					log.Info("Going to close down camera");
				}

				try {
					camControl.CloseCamera();
				} catch (Exception ex) {
					if (log.IsErrorEnabled) {
						log.Error("Problem closing camera", ex);
					}
				}
			}
			// Set the status led to very light blue
			if (_cube.Initialized) {
				if (log.IsInfoEnabled) {
					log.Info("Setting cube status LED to light blue and closing connection");
				}

				try {
					_cube.PostCommand("LO");
					_cube.PostCommand("LB1");
					_cube.PostCommand("LG1");
					_cube.Close();
				} catch (Exception ex) {
					if (log.IsErrorEnabled) {
						log.Error("Problem while closing cube", ex);
					}
				}
			}
		}
		#endregion

		#region Error Messaging
		private void ShowError(string message) {
			using (ErrorDialog dlg = new ErrorDialog("SugarCube Manager", message, "Click OK to continue")) {
				dlg.ShowDialog();
			}
		}
		private void ShowError(string message, string stackTrace) {
			using (ErrorDialog dlg = new ErrorDialog("SugarCube Manager", message, "Click OK to continue", stackTrace)) {
				dlg.ShowDialog();
			}
		}
		private void ShowFatalError(string message) {
			using (ErrorDialog dlg = new ErrorDialog("SugarCube Manager", message, "The SugarCube Manager will now close")) {
				dlg.ShowDialog();
			}
		}
		private void ShowFatalError(string message, string stackTrace) {
			using (ErrorDialog dlg = new ErrorDialog("SugarCube Manager", message, "The SugarCube Manager will now close", stackTrace)) {
				dlg.ShowDialog();
			}
		}

		#endregion

		#region UI Manipulation
		void MakeSkipLabelFontBold(object sender, EventArgs e) {
			if (log.IsDebugEnabled) {
				log.Debug("MakeSkipLabelFontBold");
			}

			Label theLabel = (Label)sender;
			bool enabled = (bool)theLabel.Tag;
			if (!enabled) {
				return;
			}
			theLabel.Font = _skipLabelFontBold;
		}

		void MakeSkipLabelFontNormal(object sender, EventArgs e) {
			if (log.IsDebugEnabled) {
				log.Debug("MakeSkipLabelFontNormal");
			}

			Label theLabel = (Label)sender;
			theLabel.ForeColor = _lightTextBlue;
			bool enabled = (bool)theLabel.Tag;
			if (!enabled) {
				return;
			}
			theLabel.Font = _skipLabelFontNormal;
		}

		void MakeSkipLabelFontDisabled(Label theLabel) {
			if (log.IsDebugEnabled) {
				log.Debug("MakeSkipLabelFontDisabled");
			}

			theLabel.Font = _skipLabelFontDisabled;
			theLabel.ForeColor = Color.Silver;
		}

		private void SetButtonState(Button theButton, bool enabled) {
			if (log.IsDebugEnabled) {
				log.Debug("SetButtonState");
			}

			if (enabled) {
				theButton.Enabled = true;
				theButton.BackColor = _buttonBackBlue;
				theButton.ForeColor = _darkTextBlue;
			} else {
				theButton.Enabled = false;
				theButton.BackColor = Color.WhiteSmoke;
				theButton.ForeColor = Color.Silver;
			}

		}
		// Ready to start a new order
		private void SetStep0() {
			if (log.IsDebugEnabled) {
				log.Debug("Setting Step 0");
			}

			_currentStep = 0;
			_currentEar = "";
			_leftScan = null;
			_rightScan = null;
			SetButtonState(btnNewOrder, ENABLED);
			SetButtonState(btnCancelOrder, DISABLED);

			tbFrom.Enabled = false;
			tbFrom.Text = "";
			tbTo.Enabled = false;
			tbTo.Text = "";
			tbSubject.Enabled = false;
			tbSubject.Text = "";
			tbDetails.Enabled = false;
			tbDetails.Text = "";
			SetButtonState(btnCompleteStep1, DISABLED);

			SetButtonState(btnScanLeftEar, DISABLED);
			lSkipLeftEar.Tag = false;
			lSkipLeftEar.Click -= SkipLeftEar;
			MakeSkipLabelFontDisabled(lSkipLeftEar);

			SetButtonState(btnScanRightEar, DISABLED);
			lSkipRightEar.Tag = false;
			lSkipRightEar.Click -= SkipRightEar;
			MakeSkipLabelFontDisabled(lSkipRightEar);

			SetButtonState(btnUploadOrder, DISABLED);
			SetButtonState(btnShowUploadQueue, ENABLED);

			//camControl.Visible = false;
			//this.tlRightFrame.SetRowSpan(this.camControl, 1);
			this.tlRightFrame.SetRowSpan(this.pictureBox1, 1);
			pictureBox1.Image = null;
			this.tlRightFrame.RowStyles[1] = new RowStyle(SizeType.Percent, 50F);
			progressBar.Visible = false;
			lStatus.Text = "";

			pbLeftSpin.Image = null;
			pbRightSpin.Image = null;
			this.Cursor = Cursors.Default;

		}

		// New order started, entering email details
		private void SetStep1() {
			if (log.IsDebugEnabled) {
				log.Debug("Setting Step 1");
			}

			_currentStep = 1;
			_currentEar = "";
			SetButtonState(btnNewOrder, DISABLED);
			SetButtonState(btnCancelOrder, ENABLED);

			tbFrom.Enabled = true;
			tbTo.Enabled = true;
			tbSubject.Enabled = true;
			tbDetails.Enabled = true;
			tbFrom.Focus();
			SetButtonState(btnCompleteStep1, ENABLED);

			tbFrom.Text = _config.UserEmail;
			tbTo.Text = _emailsTo.MostRecentlyUsedEmail;
		}

		// Scan Left Ear
		private void SetStep2() {
			if (log.IsDebugEnabled) {
				log.Debug("Setting Step 2");
			}

			_currentStep = 2;
			_currentEar = LEFT_EAR;

			tbFrom.Enabled = false;
			tbTo.Enabled = false;
			_config.UserEmail = tbFrom.Text;
			_emailsTo.AddEmail(tbTo.Text);
			tbSubject.Enabled = false;
			tbDetails.Enabled = false;
			SetButtonState(btnCompleteStep1, DISABLED);

			SetButtonState(btnScanLeftEar, ENABLED);
			lSkipLeftEar.Tag = true;
			lSkipLeftEar.Click += SkipLeftEar;
			MakeSkipLabelFontNormal(lSkipLeftEar, null);

			//this.tlRightFrame.RowStyles[1] = new RowStyle(SizeType.Absolute, 0F);
			//camControl.Visible = true;
			//this.tlRightFrame.SetRowSpan(this.camControl, 2);
			//this.tlRightFrame.SetRowSpan(this.pictureBox1, 2);
			pictureBox1.Image = null;
			lStatus.Text = "Please insert the Left Ear impression in to the Cube";

			pbLeftSpin.Image = null;
			pbRightSpin.Image = null;

			worker.DoWork -= RunScan_DoWork;
			worker.DoWork -= UploadScan_DoWork;
			worker.DoWork += RunScan_DoWork;

			worker.ProgressChanged -= RunScan_ProgressChanged;
			worker.ProgressChanged -= UploadScan_ProgressChanged;
			worker.ProgressChanged += RunScan_ProgressChanged;

			worker.RunWorkerCompleted -= RunScan_WorkCompleted;
			worker.RunWorkerCompleted -= UploadScan_WorkCompleted;
			worker.RunWorkerCompleted += RunScan_WorkCompleted;
		}

		// Scan Right Ear
		private void SetStep3() {
			if (log.IsDebugEnabled) {
				log.Debug("Setting Step 3");
			}

			_rightScan = null;
			_currentStep = 3;
			_currentEar = RIGHT_EAR;

			SetButtonState(btnScanLeftEar, DISABLED);
			lSkipLeftEar.Tag = false;
			lSkipLeftEar.Click -= SkipLeftEar;
			MakeSkipLabelFontDisabled(lSkipLeftEar);

			SetButtonState(btnScanRightEar, ENABLED);
			lSkipRightEar.Tag = true;
			lSkipRightEar.Click += SkipRightEar;
			MakeSkipLabelFontNormal(lSkipRightEar, null);

			pictureBox1.Image = null;
			lStatus.Text = "Please insert the Right Ear impression in to the Cube";
			pbLeftSpin.Image = null;
			pbRightSpin.Image = null;
			this.Cursor = Cursors.Default;
		}

		// Complete Order Form
		private void SetStep4() {
			if (log.IsDebugEnabled) {
				log.Debug("Setting Step 4");
			}

			_currentStep = 4;
			_currentEar = "";

			SetButtonState(btnScanRightEar, DISABLED);
			lSkipRightEar.Tag = false;
			lSkipRightEar.Click -= SkipRightEar;
			MakeSkipLabelFontDisabled(lSkipRightEar);

			this.tlRightFrame.RowStyles[1] = new RowStyle(SizeType.Percent, 50F);
			//camControl.Visible = false;
			//this.tlRightFrame.SetRowSpan(this.camControl, 1);
			this.tlRightFrame.SetRowSpan(pictureBox1, 1);
			pictureBox1.Image = null;
			lStatus.Text = "Completing Order Form";
			pbLeftSpin.Image = null;
			pbRightSpin.Image = null;
			this.Cursor = Cursors.Default;
		}

		// Upload Data
		private void SetStep5() {
			if (log.IsDebugEnabled) {
				log.Debug("Setting Step 5");
			}

			_currentStep = 5;
			_currentEar = "";
			SetButtonState(btnUploadOrder, ENABLED);

			lStatus.Text = "Preparing to Upload Order";

			progressBar.Visible = true;

			worker.DoWork -= RunScan_DoWork;
			worker.DoWork -= UploadScan_DoWork;
			worker.DoWork += UploadScan_DoWork;

			worker.ProgressChanged -= RunScan_ProgressChanged;
			worker.ProgressChanged -= UploadScan_ProgressChanged;
			worker.ProgressChanged += UploadScan_ProgressChanged;

			worker.RunWorkerCompleted -= RunScan_WorkCompleted;
			worker.RunWorkerCompleted -= UploadScan_WorkCompleted;
			worker.RunWorkerCompleted += UploadScan_WorkCompleted;
		}
		#endregion

		#region UI Events
		void MainFormKeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.A && e.Alt && e.Control) { //Ctrl + Alt + A means open the config manager
				if (log.IsInfoEnabled) {
					log.Info("Access to config manager requested");
				}

				if (SecurityManager.PromptForAndCheckPassword()) { // Only asks for password if the SecurityToken is set
					if (log.IsInfoEnabled) {
						log.Info("config manager launching");
					}
					ConfigManager manager = new ConfigManager(ref _config);
					manager.ShowDialog();
					manager.Dispose();
				} else {
					ShowError("Invalid configuration manager password.");
					if (log.IsWarnEnabled) {
						log.Warn("Invalid config manager password entered");
					}
				}
			}
		}


		void StartANewOrder(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Starting a new order");
			}

			SetStep1();
		}

		void CancelOrder(object sender, EventArgs e) {
			DialogResult res = MessageBox.Show("Are you sure you want to cancel this scan?  All the images that have been captured up until this point, along with any order forms you have been completed will be discarded.",
														  "Cancellation Confirmation Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (res == DialogResult.Yes) {
				worker.CancelAsync();
				if (log.IsInfoEnabled) {
					log.Info("User cancelled order");
				}

				SetStep0();
			}
		}

		void CompleteStep1(object sender, EventArgs e) {
			if (String.IsNullOrWhiteSpace(tbFrom.Text) || String.IsNullOrWhiteSpace(tbTo.Text)) {
				ShowError("The From and To fields are required");
				return;
			}

			SetStep2();
		}

		void SkipLeftEar(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("User skipped left ear");
			}

			SetStep3();
		}

		void ScanLeftEar(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Scanning left ear");
			}

			lSkipLeftEar.Tag = false;
			lSkipLeftEar.Click -= SkipLeftEar;
			MakeSkipLabelFontDisabled(lSkipLeftEar);
			SetButtonState(btnScanLeftEar, DISABLED);
			lStatus.Text = "Scanning Left Ear";
			pbLeftSpin.Image = UIIMages.loader;
			this.Cursor = Cursors.WaitCursor;
			_leftScan = new Scan();
			InitializeScan(ref _leftScan, _config.ShootPrefix, "Left Ear");
			worker.RunWorkerAsync(LEFT_EAR);
		}

		void SkipRightEar(object sender, EventArgs e) {
			if (_leftScan == null) {
				DialogResult res = MessageBox.Show("You did not scan the left ear.  Are you sure you want to skip scanning the right ear as well.  If you select yes, the scan will be cancelled.  If you select no, you will be able to scan the right ear.",
															  "No Ears Scanned", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (res == DialogResult.Yes) {
					if (log.IsInfoEnabled) {
						log.Info("User skipped both ears, cancelling scan");
					}
					SetStep0();
				}
			} else {
				if (log.IsInfoEnabled) {
					log.Info("User skipped right ear");
				}

				SetStep4();
				CompleteOrderForm();
			}
		}

		void ScanRightEar(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Scanning right ear");
			}

			lSkipRightEar.Tag = false;
			lSkipRightEar.Click -= SkipRightEar;
			MakeSkipLabelFontDisabled(lSkipRightEar);
			SetButtonState(btnScanRightEar, DISABLED);
			lStatus.Text = "Scanning Right Ear";
			this.Cursor = Cursors.WaitCursor;
			pbRightSpin.Image = UIIMages.loader;
			_rightScan = new Scan();
			InitializeScan(ref _rightScan, _config.ShootPrefix, "Right Ear");
			worker.RunWorkerAsync(RIGHT_EAR);
		}

		private void EncryptOrderForm(string orderForm) {
			if (log.IsInfoEnabled) {
				log.Info("Encrypting order form");
			}

			Process p = new Process();
			p.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "hcrypt.exe");
			p.StartInfo.Arguments = String.Format("--encrypt --file=\"{0}\" --pass=\"{1}\"", orderForm, _config.HCryptPassword);
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.Start();
			string errors = p.StandardOutput.ReadToEnd();
			if (!String.IsNullOrWhiteSpace(errors)) {
				if (log.IsErrorEnabled) {
					log.Error("Error encyrpting order file: " + errors);
				}

			}
			p.WaitForExit();
			p.Close();
		}

		void CompleteOrderForm() {
			if (log.IsInfoEnabled) {
				log.Info("Completing order form");
			}

			if (_skipOrderForm) {
				SetStep5();
				return;
			}

			//Sanity check, this should never happen but it just may
			if (!File.Exists("OrderForm.pdf")) {
				if (log.IsInfoEnabled) {
					log.Info("Order form template is suddenly missing, handling gracefully");
				}

				MessageBox.Show("The order form, OrderForm.pdf, has been deleted from the application installation folder.  You will need to re-install the software to corrent this or your supplier may not be able to complete your order", "Order Form Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				_skipOrderForm = true;
				SetStep5();
				return;
			}

			//First copy the order form into one of the scan folders
			string scanToUse = (_leftScan == null) ? RIGHT_EAR : LEFT_EAR;
			string orderFileName;
			if (scanToUse == LEFT_EAR) {
				orderFileName= String.Format("{0}\\ORDER_{1}.pdf", _leftScan.LocalScanFolder, _leftScan.metadata.scanID);
			} else {
				orderFileName= String.Format("{0}\\ORDER_{1}.pdf", _rightScan.LocalScanFolder, _rightScan.metadata.scanID);
			}
			if (log.IsDebugEnabled) {
				log.Debug("order file name = " + orderFileName);
			}

			File.Copy("OrderForm.pdf", orderFileName);

			// Allow the user to edit the order form
			if (log.IsDebugEnabled) {
				log.Debug("Opening order form for editing");
			}

			Process p = new Process();
			p.StartInfo.FileName = orderFileName;
			p.Start();
			p.WaitForExit();
			p.Close();

			//Give the system a second to unlock the order file
			Thread.Sleep(250);

			//Encrypt the order in the first folder
			EncryptOrderForm(orderFileName);

			//Add the encrypted order files to the list of attachments
			if (scanToUse == LEFT_EAR) {
				_leftScan.orderDetails.attachments.Add(Path.GetFileName(orderFileName + ".data.hcrypt"));
				_leftScan.orderDetails.attachments.Add(Path.GetFileName(orderFileName + ".head.hcrypt"));
			} else {
				_rightScan.orderDetails.attachments.Add(orderFileName + ".data.hcrypt");
				_rightScan.orderDetails.attachments.Add(orderFileName + ".head.hcrypt");
			}

			//Copy the order file to the other scan folder and encrypt it, if necessary
			string orderCopyName = "";
			if (scanToUse == LEFT_EAR && !(_rightScan == null)) {
				orderCopyName = String.Format("{0}\\ORDER_{1}.pdf", _rightScan.LocalScanFolder, _rightScan.metadata.scanID);
				File.Copy(orderFileName, orderCopyName);
				EncryptOrderForm(orderCopyName);
				_rightScan.orderDetails.attachments.Add(orderFileName + ".data.hcrypt");
				_rightScan.orderDetails.attachments.Add(orderFileName + ".head.hcrypt");
			}

			//Delete the unencrypted order file in the first folder
			File.Delete(orderFileName);

			//Delete the unencrypted order file in the second folder, if necessary
			if (!String.IsNullOrWhiteSpace(orderCopyName) && File.Exists(orderCopyName)) {
				File.Delete(orderCopyName);
			}

			SetStep5();
		}

		void UploadOrder(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Uploading order");
			}

			SetButtonState(btnUploadOrder, DISABLED);
			worker.RunWorkerAsync();
		}

		// Helper for both BtnQueueUploadClick and our background uploader if something
		// goes wrong and we need to add the scans to the queue
		void AddScansToUploadQueue() {
			if (log.IsInfoEnabled) {
				log.Info("Adding scan(s) to upload queue");
			}

			// Add the left ear, if it's there
			if (_leftScan != null) {
				if (log.IsDebugEnabled) {
					log.Debug("Adding left ear scan");
				}

				_leftScan.metadata.uploadStartTime = DateTime.Now.ToString(Scan.DATE_FORMAT);
				_leftScan.WriteManifest();
				_uploader.AddScanToQueue(_leftScan.metadata.scanID + "-" + LEFT_EAR);
			}
			// and now the right one
			if (_rightScan != null) {
				if (log.IsDebugEnabled) {
					log.Debug("Adding right ear scan");
				}

				_rightScan.metadata.uploadStartTime = DateTime.Now.ToString(Scan.DATE_FORMAT);
				_rightScan.WriteManifest();
				_uploader.AddScanToQueue(_rightScan.metadata.scanID + "-" + RIGHT_EAR);
			}

		}

		void UploadOrderButtonClick(object sender, EventArgs e) {
			_config.UserEmail = tbFrom.Text;
			this.Cursor = Cursors.WaitCursor;
			SetButtonState(btnUploadOrder, DISABLED);
			AddScansToUploadQueue();
			SetButtonState(btnUploadOrder, ENABLED);
			string message = "The order has been added to the upload queue.\nYou can click the Show Upload Queue button at any time to see the state of the upload process.";
			if (!_uploader.CheckForInternetConnection()) {
				message = "The order has been added to the upload queue.\nThere is currently no internet connection to the 3DWares servers on this machine.  Once connectivity is established, the upload will proceed.\nYou can click the Show Upload Queue button at any time to see the state of the upload process.";
			}
			this.Cursor = Cursors.Default;
			MessageBox.Show(message,  "SugarCube Manager");
			SetStep0();

		}

		void ShowUploadQueueButtonClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Attempting to show uploader window");
			}

			int res = _uploader.ShowUploaderWindow();
			if (res == -1) { //start the uploader
				if (log.IsDebugEnabled) {
					log.Debug("Uploader does not seem to be running, attempting to start the uploader");
				}
				Process.Start("SugarCube Uploader.exe");
				Thread.Sleep(500);
				_uploader.ShowUploaderWindow();
			}
		}
		#endregion

		#region BackGround Worker Event Handlers and Supporting Methods
		private void InitializeScan(ref Scan theScan, string scanIDPrefix, string imageSetID) {
			if (log.IsInfoEnabled) {
				log.Info("Initializing scan");
			}

			theScan.metadata.scanID = scanIDPrefix + DateTime.Now.ToString("yyyyMMddHHmm");
			if (log.IsDebugEnabled) {
				log.Debug("ScanID set to " + theScan.metadata.scanID);
			}

			theScan.LocalScanFolder = Path.Combine(_config.DataPath, theScan.metadata.scanID + "-" + _currentEar);
			if (log.IsDebugEnabled) {
				log.Debug("Local scan folder = " + theScan.LocalScanFolder);
			}

			if (!Directory.Exists(theScan.LocalScanFolder)) {
				Directory.CreateDirectory(theScan.LocalScanFolder);
			}

			theScan.orderDetails.from = tbFrom.Text;
			theScan.orderDetails.to = tbTo.Text;
			theScan.orderDetails.subject = tbSubject.Text;
			theScan.orderDetails.body = tbDetails.Text;
			theScan.orderDetails.referenceID = theScan.metadata.scanID;

			theScan.imageSet.id = imageSetID;

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
		}

		private void TakeSnapshot(string savePath) {
			if (log.IsInfoEnabled) {
				log.Info("Taking snapshot to be saved to " + savePath);
			}

			string fileName = "snapshot-" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".jpg";
			if (log.IsDebugEnabled) {
				log.Debug("Snapshot file name = " + fileName);
			}

			if (!Directory.Exists(savePath)) {
				if (log.IsDebugEnabled) {
					log.Debug("Need to create savepath");
				}

				Directory.CreateDirectory(savePath);
			}
			string filePath = Path.Combine(savePath, fileName );
			try {
				CubeImageUtils.TakeSnapshot(filePath, camControl, _config.JpegQuality,
													 _config.UseMotionDetection, _config.MotionSensitivity,
													 _config.UseMedianStacking,
													 _config.UseContrastEnhancement, _config.ContrastValue, _config.ContrastMidpoint);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Unable to take snapshot", ex);
				}

				ShowError("An error occured while trying to capture the current scanner image. The scan may need to be restarted.", ex.StackTrace);
				return;
			}

			if (_currentEar == LEFT_EAR) {
				if (log.IsDebugEnabled) {
					log.Debug("adding image to left ear set");
				}

				_leftScan.imageSet.images.Add(new ScanImage(_cube.Elevation.ToString(), _cube.Rotation.ToString(), fileName ));
			} else {
				if (log.IsDebugEnabled) {
					log.Debug("adding image to right ear set");
				}

				_rightScan.imageSet.images.Add(new ScanImage(_cube.Elevation.ToString(), _cube.Rotation.ToString(), fileName ));
			}
			if (log.IsDebugEnabled) {
				log.Debug("Loading snapshot in to picture box");
			}

			pictureBox1.Load(filePath);
		}

		private void RunScan_DoWork(object sender, DoWorkEventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Preparing to run scan in background thread");
			}

			BackgroundWorker worker = sender as BackgroundWorker;
			string shootString = _config.ShootString;
			_cube.ProcessShootString(shootString, worker, e, _synchronizer);
			e.Result = e.Argument;
		}

		private void RunScan_WorkCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (log.IsInfoEnabled) {
				log.Info("Running of scan is complete");
			}

			if (e.Error != null) {
				ShowError("An error occured while running the scan.  Please check the logs for more details");
				if (log.IsErrorEnabled) {
					log.Error("An error occured while scanning", e.Error);
				}

			}

			if (e.Cancelled) {
				if (log.IsInfoEnabled) {
					log.Info("Scan cancelled by user");
				}

				SetStep0();
				return;
			}

			string ear = e.Result as string;
			if (ear == LEFT_EAR) {
				if (_config.DoSetEXIF) {
					try {
						CubeImageUtils.SetExifInfo(_leftScan.LocalScanFolder, _35mmFocalEquivalent);
					} catch (Exception ex) {
						if (log.IsErrorEnabled) {
							log.Error("error setting EXIF for left ear",  ex);
						}
					}
				}
				SetStep3();
			} else {
				if (_config.DoSetEXIF) {
					try {
						CubeImageUtils.SetExifInfo(_rightScan.LocalScanFolder, _35mmFocalEquivalent);
					} catch (Exception ex) {
						if (log.IsErrorEnabled) {
							log.Error("error setting EXIF for right ear",  ex);
						}
					}
				}
				SetStep4();
				CompleteOrderForm();
			}

		}

		private void RunScan_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			if (e.ProgressPercentage == SugarCube.REPORT_STATUS_FLAG) {
				if (log.IsInfoEnabled) {
					log.Info("scan status: " + (string)e.UserState);
				}

			} else if (e.ProgressPercentage == SugarCube.TAKE_SNAPSHOT_FLAG) {
				string localScanFolder = (_currentEar == LEFT_EAR) ? _leftScan.LocalScanFolder : _rightScan.LocalScanFolder;
				TakeSnapshot(localScanFolder);
				_synchronizer.Set();
			}
		}

		// Helper function for UploadScan_DoWork
		private void UploadEar(BackgroundWorker worker, string localScanFolder, string host, string user, string pass, string uploadBase, string scanID, string triggerURL) {
			if (log.IsInfoEnabled) {
				log.Info("Uploading ear");
			}

			worker.ReportProgress(UploadManager.SWITCH_TO_CONTINUOUS);
			string uploadedPath = _uploader.UploadScan(worker, localScanFolder, host, user, pass, uploadBase, scanID);
			if (uploadedPath.StartsWith(":ERROR:")) {
				throw new Exception(uploadedPath.Remove(0, 7));
			}
			worker.ReportProgress(UploadManager.STATUS_FLAG, "Triggering Remote Processing of Scan");
			_uploader.TriggerScanProcessing(worker, triggerURL, uploadedPath);
		}

		// UploadEar may thow an exception.  If it does, we queue all scans for later upload,
		// save the error message and throw it again to be picked up by WorkCompleted
		private void UploadScan_DoWork(object sender, DoWorkEventArgs e) {
			BackgroundWorker worker = sender as BackgroundWorker;
			string host = _config.SftpHost;
			string user = _config.SftpUser;
			string pass = _config.SftpPassword;
			string uploadBase = _config.SftpUploadBase;
			string triggerURL = _config.TriggerURL;

			try {
				// Upload the left ear, if it's there
				if (_leftScan != null) {
					worker.ReportProgress(UploadManager.STATUS_FLAG, "Uploading Left Ear");
					_leftScan.metadata.uploadStartTime = DateTime.Now.ToString(Scan.DATE_FORMAT);
					_leftScan.WriteManifest();
					UploadEar(worker, _leftScan.LocalScanFolder, host, user, pass, uploadBase, _leftScan.metadata.scanID, triggerURL);
				}
				// and now the right one
				if (_rightScan != null) {
					worker.ReportProgress(UploadManager.STATUS_FLAG, "Uploading Right Ear");
					_rightScan.metadata.uploadStartTime = DateTime.Now.ToString(Scan.DATE_FORMAT);
					_rightScan.WriteManifest();
					UploadEar(worker, _rightScan.LocalScanFolder, host, user, pass, uploadBase, _rightScan.metadata.scanID, triggerURL);
				}
			} catch (Exception ex) {
				AddScansToUploadQueue();
				throw new Exception(ex.Message);
			}
		}

		private void UploadScan_WorkCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Error != null) {
				ShowError("A problem occured while uploading the scan to the 3DWares cloud.  " +
							 "The scan has been added to the background upload queue.  " +
							 "You may need to update your configuration before the upload will be able to complete succesfully. " +
							 "The error was:\n\n" + e.Error.Message);
				if (log.IsErrorEnabled) {
					log.Error("Error uploading scan", e.Error);
				}

				SetStep0();
				return;
			}

			if (e.Cancelled) {
				SetStep0();
				return;
			}

			MessageBox.Show("Your scan has been uploaded to the 3DWares cloud.  You will receive emails once your scans have completed processing.", "SugarCube Manager");
			SetStep0();
		}

		private void UploadScan_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			if (e.ProgressPercentage == UploadManager.SWITCH_TO_CONTINUOUS) {
				progressBar.Value = 0;
				progressBar.Style = ProgressBarStyle.Continuous;
			} else if (e.ProgressPercentage == UploadManager.SWITCH_TO_MARQUEE) {
				progressBar.Value = 100;
				progressBar.Style = ProgressBarStyle.Marquee;
			} else if (e.ProgressPercentage == UploadManager.STATUS_FLAG) {
				lStatus.Text = (string)e.UserState;
			} else if (e.ProgressPercentage == UploadManager.INFO_FLAG) {
				if (log.IsInfoEnabled) {
					log.Info("upload progress: " + (string)e.UserState);
				}
			} else {
				progressBar.Value = e.ProgressPercentage;
			}
		}

		#endregion





	}
}

