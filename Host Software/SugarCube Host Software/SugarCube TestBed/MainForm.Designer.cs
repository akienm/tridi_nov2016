/* SugarCube Host Software - Test Bed
 * Copyright (c) 2014-2015 Chad Ullman
 */

namespace Me.ThreeDWares.SugarCube
{
   partial class MainForm
   {
      /// <summary>
      /// Designer variable used to keep track of non-visual components.
      /// </summary>
      private System.ComponentModel.IContainer components = null;
      
      /// <summary>
      /// Disposes resources used by the form.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing) {
            if (components != null) {
               components.Dispose();
            }
         }
         base.Dispose(disposing);
      }
      
      /// <summary>
      /// This method is required for Windows Forms designer support.
      /// Do not change the method contents inside the source code editor. The Forms designer might
      /// not be able to load this method if it was changed manually.
      /// </summary>
      private void InitializeComponent()
      {
      	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      	System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      	this.btnRunShootString = new System.Windows.Forms.Button();
      	this.cbCameraList = new System.Windows.Forms.ComboBox();
      	this.cbResolutionList = new System.Windows.Forms.ComboBox();
      	this.btnSettings = new System.Windows.Forms.Button();
      	this.btnSnapshot = new System.Windows.Forms.Button();
      	this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
      	this.tlp2 = new System.Windows.Forms.TableLayoutPanel();
      	this.tlp3 = new System.Windows.Forms.TableLayoutPanel();
      	this.label1 = new System.Windows.Forms.Label();
      	this.label2 = new System.Windows.Forms.Label();
      	this.tlp4 = new System.Windows.Forms.TableLayoutPanel();
      	this.gbSugarCubeLog = new System.Windows.Forms.GroupBox();
      	this.tbSugarCubeLog = new System.Windows.Forms.TextBox();
      	this.gbTesterLog = new System.Windows.Forms.GroupBox();
      	this.tbTesterLog = new System.Windows.Forms.TextBox();
      	this.camControl = new Camera_NET.CameraControl();
      	this.tcCubeCommander = new System.Windows.Forms.TabControl();
      	this.tabPage2 = new System.Windows.Forms.TabPage();
      	this.flpShootStrings = new System.Windows.Forms.FlowLayoutPanel();
      	this.label3 = new System.Windows.Forms.Label();
      	this.tbShootString = new System.Windows.Forms.TextBox();
      	this.btnCancelShootString = new System.Windows.Forms.Button();
      	this.btnOpenShootFolder = new System.Windows.Forms.Button();
      	this.btnGenerateXMLManifest = new System.Windows.Forms.Button();
      	this.btnCreateDummyForms = new System.Windows.Forms.Button();
      	this.btnSetModellingOpts = new System.Windows.Forms.Button();
      	this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      	this.label6 = new System.Windows.Forms.Label();
      	this.label7 = new System.Windows.Forms.Label();
      	this.label8 = new System.Windows.Forms.Label();
      	this.label9 = new System.Windows.Forms.Label();
      	this.label10 = new System.Windows.Forms.Label();
      	this.label11 = new System.Windows.Forms.Label();
      	this.label12 = new System.Windows.Forms.Label();
      	this.label13 = new System.Windows.Forms.Label();
      	this.lSourceFolder = new System.Windows.Forms.Label();
      	this.tbScanIDPrefix = new System.Windows.Forms.TextBox();
      	this.tbHost = new System.Windows.Forms.TextBox();
      	this.tbUser = new System.Windows.Forms.TextBox();
      	this.tbPass = new System.Windows.Forms.TextBox();
      	this.tbUploadBase = new System.Windows.Forms.TextBox();
      	this.tbTriggerURL = new System.Windows.Forms.TextBox();
      	this.label14 = new System.Windows.Forms.Label();
      	this.tbEmail = new System.Windows.Forms.TextBox();
      	this.panel1 = new System.Windows.Forms.Panel();
      	this.btnSendToUploadManager = new System.Windows.Forms.Button();
      	this.btnUploadScan = new System.Windows.Forms.Button();
      	this.pbUploadStatus = new System.Windows.Forms.ProgressBar();
      	this.tabPage1 = new System.Windows.Forms.TabPage();
      	this.flpSingleCommands = new System.Windows.Forms.FlowLayoutPanel();
      	this.label4 = new System.Windows.Forms.Label();
      	this.tbSingleCommand = new System.Windows.Forms.TextBox();
      	this.btnGo = new System.Windows.Forms.Button();
      	this.cbForceCOMPort = new System.Windows.Forms.CheckBox();
      	this.cbCOMPortList = new System.Windows.Forms.ComboBox();
      	this.tlImageProcessingOptions = new System.Windows.Forms.TableLayoutPanel();
      	this.tbQuality = new System.Windows.Forms.TrackBar();
      	this.lContrastValue = new System.Windows.Forms.Label();
      	this.tbContrast = new System.Windows.Forms.TrackBar();
      	this.lMidPoint = new System.Windows.Forms.Label();
      	this.tbMidpoint = new System.Windows.Forms.TrackBar();
      	this.cbUseContrastEnhancement = new System.Windows.Forms.CheckBox();
      	this.cbUseMedianStacking = new System.Windows.Forms.CheckBox();
      	this.tbSensitivity = new System.Windows.Forms.TrackBar();
      	this.lSensitivity = new System.Windows.Forms.Label();
      	this.cbUseMotionDetection = new System.Windows.Forms.CheckBox();
      	this.label17 = new System.Windows.Forms.Label();
      	this.lJPEGQuality = new System.Windows.Forms.Label();
      	this.cbSetEXIF = new System.Windows.Forms.CheckBox();
      	this.btnExportConfig = new System.Windows.Forms.Button();
      	this.btnImportConfig = new System.Windows.Forms.Button();
      	this.tabPage3 = new System.Windows.Forms.TabPage();
      	this.btnZRefresh = new System.Windows.Forms.Button();
      	this.btnDeleteZKey = new System.Windows.Forms.Button();
      	this.btnAddZKey = new System.Windows.Forms.Button();
      	this.label15 = new System.Windows.Forms.Label();
      	this.btnUpdateZString = new System.Windows.Forms.Button();
      	this.zStringGrid = new System.Windows.Forms.DataGridView();
      	this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      	this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
      	this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      	this.lZString = new System.Windows.Forms.Label();
      	this.label5 = new System.Windows.Forms.Label();
      	this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
      	this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
      	this.tlp1.SuspendLayout();
      	this.tlp2.SuspendLayout();
      	this.tlp3.SuspendLayout();
      	this.tlp4.SuspendLayout();
      	this.gbSugarCubeLog.SuspendLayout();
      	this.gbTesterLog.SuspendLayout();
      	this.tcCubeCommander.SuspendLayout();
      	this.tabPage2.SuspendLayout();
      	this.flpShootStrings.SuspendLayout();
      	this.tableLayoutPanel1.SuspendLayout();
      	this.panel1.SuspendLayout();
      	this.tabPage1.SuspendLayout();
      	this.flpSingleCommands.SuspendLayout();
      	this.tlImageProcessingOptions.SuspendLayout();
      	((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
      	((System.ComponentModel.ISupportInitialize)(this.tbContrast)).BeginInit();
      	((System.ComponentModel.ISupportInitialize)(this.tbMidpoint)).BeginInit();
      	((System.ComponentModel.ISupportInitialize)(this.tbSensitivity)).BeginInit();
      	this.tabPage3.SuspendLayout();
      	((System.ComponentModel.ISupportInitialize)(this.zStringGrid)).BeginInit();
      	this.SuspendLayout();
      	// 
      	// btnRunShootString
      	// 
      	this.btnRunShootString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnRunShootString.Location = new System.Drawing.Point(3, 219);
      	this.btnRunShootString.Name = "btnRunShootString";
      	this.btnRunShootString.Size = new System.Drawing.Size(376, 23);
      	this.btnRunShootString.TabIndex = 0;
      	this.btnRunShootString.Text = "Run Shoot String";
      	this.btnRunShootString.UseVisualStyleBackColor = true;
      	this.btnRunShootString.Click += new System.EventHandler(this.BtnRunShootStringClick);
      	// 
      	// cbCameraList
      	// 
      	this.cbCameraList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.cbCameraList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      	this.cbCameraList.FormattingEnabled = true;
      	this.cbCameraList.Location = new System.Drawing.Point(65, 3);
      	this.cbCameraList.Name = "cbCameraList";
      	this.cbCameraList.Size = new System.Drawing.Size(231, 21);
      	this.cbCameraList.TabIndex = 0;
      	this.cbCameraList.SelectedIndexChanged += new System.EventHandler(this.CbCameraListSelectedIndexChanged);
      	// 
      	// cbResolutionList
      	// 
      	this.cbResolutionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.cbResolutionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      	this.cbResolutionList.FormattingEnabled = true;
      	this.cbResolutionList.Location = new System.Drawing.Point(65, 31);
      	this.cbResolutionList.Name = "cbResolutionList";
      	this.cbResolutionList.Size = new System.Drawing.Size(231, 21);
      	this.cbResolutionList.TabIndex = 4;
      	this.cbResolutionList.SelectedIndexChanged += new System.EventHandler(this.CbResolutionListSelectedIndexChanged);
      	// 
      	// btnSettings
      	// 
      	this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnSettings.Location = new System.Drawing.Point(302, 3);
      	this.btnSettings.Name = "btnSettings";
      	this.btnSettings.Size = new System.Drawing.Size(301, 21);
      	this.btnSettings.TabIndex = 3;
      	this.btnSettings.Text = "Adjust Camera Settings";
      	this.btnSettings.UseVisualStyleBackColor = true;
      	this.btnSettings.Click += new System.EventHandler(this.BtnSettingsClick);
      	// 
      	// btnSnapshot
      	// 
      	this.btnSnapshot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnSnapshot.Location = new System.Drawing.Point(302, 31);
      	this.btnSnapshot.Name = "btnSnapshot";
      	this.btnSnapshot.Size = new System.Drawing.Size(301, 22);
      	this.btnSnapshot.TabIndex = 4;
      	this.btnSnapshot.Text = "Take a Snapshot";
      	this.btnSnapshot.UseVisualStyleBackColor = true;
      	this.btnSnapshot.Click += new System.EventHandler(this.BtnSnapshotClick);
      	// 
      	// tlp1
      	// 
      	this.tlp1.ColumnCount = 2;
      	this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.21825F));
      	this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.78175F));
      	this.tlp1.Controls.Add(this.tlp2, 0, 0);
      	this.tlp1.Controls.Add(this.tcCubeCommander, 1, 0);
      	this.tlp1.Location = new System.Drawing.Point(0, 0);
      	this.tlp1.Name = "tlp1";
      	this.tlp1.RowCount = 1;
      	this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp1.Size = new System.Drawing.Size(1008, 730);
      	this.tlp1.TabIndex = 1;
      	// 
      	// tlp2
      	// 
      	this.tlp2.ColumnCount = 1;
      	this.tlp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      	this.tlp2.Controls.Add(this.tlp3, 0, 1);
      	this.tlp2.Controls.Add(this.tlp4, 0, 2);
      	this.tlp2.Controls.Add(this.camControl, 0, 0);
      	this.tlp2.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tlp2.Location = new System.Drawing.Point(0, 0);
      	this.tlp2.Margin = new System.Windows.Forms.Padding(0);
      	this.tlp2.Name = "tlp2";
      	this.tlp2.RowCount = 3;
      	this.tlp2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
      	this.tlp2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp2.Size = new System.Drawing.Size(606, 730);
      	this.tlp2.TabIndex = 0;
      	// 
      	// tlp3
      	// 
      	this.tlp3.ColumnCount = 3;
      	this.tlp3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.77922F));
      	this.tlp3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.22078F));
      	this.tlp3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 306F));
      	this.tlp3.Controls.Add(this.btnSnapshot, 2, 1);
      	this.tlp3.Controls.Add(this.btnSettings, 2, 0);
      	this.tlp3.Controls.Add(this.cbResolutionList, 1, 1);
      	this.tlp3.Controls.Add(this.cbCameraList, 1, 0);
      	this.tlp3.Controls.Add(this.label1, 0, 0);
      	this.tlp3.Controls.Add(this.label2, 0, 1);
      	this.tlp3.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tlp3.Location = new System.Drawing.Point(0, 337);
      	this.tlp3.Margin = new System.Windows.Forms.Padding(0);
      	this.tlp3.Name = "tlp3";
      	this.tlp3.RowCount = 2;
      	this.tlp3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp3.Size = new System.Drawing.Size(606, 56);
      	this.tlp3.TabIndex = 0;
      	// 
      	// label1
      	// 
      	this.label1.Location = new System.Drawing.Point(0, 0);
      	this.label1.Margin = new System.Windows.Forms.Padding(0);
      	this.label1.Name = "label1";
      	this.label1.Size = new System.Drawing.Size(62, 23);
      	this.label1.TabIndex = 5;
      	this.label1.Text = "Select Camera:";
      	this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// label2
      	// 
      	this.label2.Location = new System.Drawing.Point(0, 28);
      	this.label2.Margin = new System.Windows.Forms.Padding(0);
      	this.label2.Name = "label2";
      	this.label2.Size = new System.Drawing.Size(62, 23);
      	this.label2.TabIndex = 6;
      	this.label2.Text = "Resolution:";
      	this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// tlp4
      	// 
      	this.tlp4.ColumnCount = 2;
      	this.tlp4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp4.Controls.Add(this.gbSugarCubeLog, 0, 0);
      	this.tlp4.Controls.Add(this.gbTesterLog, 0, 0);
      	this.tlp4.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tlp4.Location = new System.Drawing.Point(0, 393);
      	this.tlp4.Margin = new System.Windows.Forms.Padding(0);
      	this.tlp4.Name = "tlp4";
      	this.tlp4.RowCount = 1;
      	this.tlp4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      	this.tlp4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      	this.tlp4.Size = new System.Drawing.Size(606, 337);
      	this.tlp4.TabIndex = 1;
      	// 
      	// gbSugarCubeLog
      	// 
      	this.gbSugarCubeLog.Controls.Add(this.tbSugarCubeLog);
      	this.gbSugarCubeLog.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.gbSugarCubeLog.Location = new System.Drawing.Point(0, 317);
      	this.gbSugarCubeLog.Margin = new System.Windows.Forms.Padding(0);
      	this.gbSugarCubeLog.Name = "gbSugarCubeLog";
      	this.gbSugarCubeLog.Size = new System.Drawing.Size(303, 20);
      	this.gbSugarCubeLog.TabIndex = 13;
      	this.gbSugarCubeLog.TabStop = false;
      	this.gbSugarCubeLog.Text = "SugarCube Messages:";
      	this.gbSugarCubeLog.Visible = false;
      	// 
      	// tbSugarCubeLog
      	// 
      	this.tbSugarCubeLog.BackColor = System.Drawing.Color.Black;
      	this.tbSugarCubeLog.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbSugarCubeLog.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.tbSugarCubeLog.ForeColor = System.Drawing.Color.Cyan;
      	this.tbSugarCubeLog.Location = new System.Drawing.Point(3, 16);
      	this.tbSugarCubeLog.Multiline = true;
      	this.tbSugarCubeLog.Name = "tbSugarCubeLog";
      	this.tbSugarCubeLog.ReadOnly = true;
      	this.tbSugarCubeLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      	this.tbSugarCubeLog.Size = new System.Drawing.Size(297, 1);
      	this.tbSugarCubeLog.TabIndex = 1;
      	// 
      	// gbTesterLog
      	// 
      	this.tlp4.SetColumnSpan(this.gbTesterLog, 2);
      	this.gbTesterLog.Controls.Add(this.tbTesterLog);
      	this.gbTesterLog.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.gbTesterLog.Location = new System.Drawing.Point(0, 0);
      	this.gbTesterLog.Margin = new System.Windows.Forms.Padding(0);
      	this.gbTesterLog.Name = "gbTesterLog";
      	this.gbTesterLog.Size = new System.Drawing.Size(606, 317);
      	this.gbTesterLog.TabIndex = 12;
      	this.gbTesterLog.TabStop = false;
      	this.gbTesterLog.Text = "Tester Messages:";
      	// 
      	// tbTesterLog
      	// 
      	this.tbTesterLog.BackColor = System.Drawing.Color.Black;
      	this.tbTesterLog.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbTesterLog.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.tbTesterLog.ForeColor = System.Drawing.Color.Lime;
      	this.tbTesterLog.Location = new System.Drawing.Point(3, 16);
      	this.tbTesterLog.Multiline = true;
      	this.tbTesterLog.Name = "tbTesterLog";
      	this.tbTesterLog.ReadOnly = true;
      	this.tbTesterLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      	this.tbTesterLog.Size = new System.Drawing.Size(600, 298);
      	this.tbTesterLog.TabIndex = 2;
      	this.tbTesterLog.WordWrap = false;
      	// 
      	// camControl
      	// 
      	this.camControl.BackColor = System.Drawing.Color.White;
      	this.camControl.DirectShowLogFilepath = "";
      	this.camControl.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.camControl.Location = new System.Drawing.Point(3, 3);
      	this.camControl.Name = "camControl";
      	this.camControl.Size = new System.Drawing.Size(600, 331);
      	this.camControl.TabIndex = 0;
      	// 
      	// tcCubeCommander
      	// 
      	this.tcCubeCommander.Controls.Add(this.tabPage2);
      	this.tcCubeCommander.Controls.Add(this.tabPage1);
      	this.tcCubeCommander.Controls.Add(this.tabPage3);
      	this.tcCubeCommander.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tcCubeCommander.Location = new System.Drawing.Point(606, 0);
      	this.tcCubeCommander.Margin = new System.Windows.Forms.Padding(0);
      	this.tcCubeCommander.Name = "tcCubeCommander";
      	this.tcCubeCommander.SelectedIndex = 0;
      	this.tcCubeCommander.Size = new System.Drawing.Size(402, 730);
      	this.tcCubeCommander.TabIndex = 0;
      	// 
      	// tabPage2
      	// 
      	this.tabPage2.AutoScroll = true;
      	this.tabPage2.Controls.Add(this.flpShootStrings);
      	this.tabPage2.Location = new System.Drawing.Point(4, 22);
      	this.tabPage2.Name = "tabPage2";
      	this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      	this.tabPage2.Size = new System.Drawing.Size(394, 704);
      	this.tabPage2.TabIndex = 1;
      	this.tabPage2.Text = "Process Shoot Strings";
      	this.tabPage2.UseVisualStyleBackColor = true;
      	// 
      	// flpShootStrings
      	// 
      	this.flpShootStrings.AutoScroll = true;
      	this.flpShootStrings.Controls.Add(this.label3);
      	this.flpShootStrings.Controls.Add(this.tbShootString);
      	this.flpShootStrings.Controls.Add(this.btnRunShootString);
      	this.flpShootStrings.Controls.Add(this.btnCancelShootString);
      	this.flpShootStrings.Controls.Add(this.btnOpenShootFolder);
      	this.flpShootStrings.Controls.Add(this.btnGenerateXMLManifest);
      	this.flpShootStrings.Controls.Add(this.btnCreateDummyForms);
      	this.flpShootStrings.Controls.Add(this.btnSetModellingOpts);
      	this.flpShootStrings.Controls.Add(this.tableLayoutPanel1);
      	this.flpShootStrings.Controls.Add(this.panel1);
      	this.flpShootStrings.Controls.Add(this.pbUploadStatus);
      	this.flpShootStrings.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.flpShootStrings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      	this.flpShootStrings.Location = new System.Drawing.Point(3, 3);
      	this.flpShootStrings.Margin = new System.Windows.Forms.Padding(0);
      	this.flpShootStrings.Name = "flpShootStrings";
      	this.flpShootStrings.Size = new System.Drawing.Size(388, 698);
      	this.flpShootStrings.TabIndex = 0;
      	// 
      	// label3
      	// 
      	this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      	this.label3.Location = new System.Drawing.Point(3, 0);
      	this.label3.Name = "label3";
      	this.label3.Size = new System.Drawing.Size(376, 45);
      	this.label3.TabIndex = 0;
      	this.label3.Text = "Enter a shoot string to process\r\nNote: commands MUST be seperated by whitespace (" +
      	"tabs, spaces or newlines)";
      	this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// tbShootString
      	// 
      	this.tbShootString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      	this.tbShootString.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.tbShootString.Location = new System.Drawing.Point(3, 48);
      	this.tbShootString.Multiline = true;
      	this.tbShootString.Name = "tbShootString";
      	this.tbShootString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      	this.tbShootString.Size = new System.Drawing.Size(376, 165);
      	this.tbShootString.TabIndex = 1;
      	this.tbShootString.WordWrap = false;
      	this.tbShootString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbShootStringKeyDown);
      	// 
      	// btnCancelShootString
      	// 
      	this.btnCancelShootString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnCancelShootString.Location = new System.Drawing.Point(3, 248);
      	this.btnCancelShootString.Name = "btnCancelShootString";
      	this.btnCancelShootString.Size = new System.Drawing.Size(376, 23);
      	this.btnCancelShootString.TabIndex = 3;
      	this.btnCancelShootString.Text = "Cancel Shoot String";
      	this.btnCancelShootString.UseVisualStyleBackColor = true;
      	this.btnCancelShootString.Click += new System.EventHandler(this.CancelShootString);
      	// 
      	// btnOpenShootFolder
      	// 
      	this.btnOpenShootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnOpenShootFolder.Location = new System.Drawing.Point(3, 277);
      	this.btnOpenShootFolder.Name = "btnOpenShootFolder";
      	this.btnOpenShootFolder.Size = new System.Drawing.Size(376, 23);
      	this.btnOpenShootFolder.TabIndex = 2;
      	this.btnOpenShootFolder.Text = "Open Shoot Folder";
      	this.btnOpenShootFolder.UseVisualStyleBackColor = true;
      	this.btnOpenShootFolder.Click += new System.EventHandler(this.BtnOpenShootFolderClick);
      	// 
      	// btnGenerateXMLManifest
      	// 
      	this.btnGenerateXMLManifest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnGenerateXMLManifest.Location = new System.Drawing.Point(3, 306);
      	this.btnGenerateXMLManifest.Name = "btnGenerateXMLManifest";
      	this.btnGenerateXMLManifest.Size = new System.Drawing.Size(376, 23);
      	this.btnGenerateXMLManifest.TabIndex = 4;
      	this.btnGenerateXMLManifest.Text = "Generate XML Manifest";
      	this.btnGenerateXMLManifest.UseVisualStyleBackColor = true;
      	this.btnGenerateXMLManifest.Click += new System.EventHandler(this.BtnGenerateXMLManifestClick);
      	// 
      	// btnCreateDummyForms
      	// 
      	this.btnCreateDummyForms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnCreateDummyForms.Location = new System.Drawing.Point(3, 335);
      	this.btnCreateDummyForms.Name = "btnCreateDummyForms";
      	this.btnCreateDummyForms.Size = new System.Drawing.Size(376, 23);
      	this.btnCreateDummyForms.TabIndex = 5;
      	this.btnCreateDummyForms.Text = "Create Dummy PCL Order Forms";
      	this.btnCreateDummyForms.UseVisualStyleBackColor = true;
      	this.btnCreateDummyForms.Click += new System.EventHandler(this.BtnCreateDummyFormsClick);
      	// 
      	// btnSetModellingOpts
      	// 
      	this.btnSetModellingOpts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnSetModellingOpts.Location = new System.Drawing.Point(3, 364);
      	this.btnSetModellingOpts.Name = "btnSetModellingOpts";
      	this.btnSetModellingOpts.Size = new System.Drawing.Size(376, 23);
      	this.btnSetModellingOpts.TabIndex = 9;
      	this.btnSetModellingOpts.Text = "Set Modelling Options";
      	this.btnSetModellingOpts.UseVisualStyleBackColor = true;
      	this.btnSetModellingOpts.Click += new System.EventHandler(this.BtnSetModellingOptsClick);
      	// 
      	// tableLayoutPanel1
      	// 
      	this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      	this.tableLayoutPanel1.ColumnCount = 2;
      	this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      	this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 298F));
      	this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
      	this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
      	this.tableLayoutPanel1.Controls.Add(this.label8, 0, 3);
      	this.tableLayoutPanel1.Controls.Add(this.label9, 0, 4);
      	this.tableLayoutPanel1.Controls.Add(this.label10, 0, 5);
      	this.tableLayoutPanel1.Controls.Add(this.label11, 0, 6);
      	this.tableLayoutPanel1.Controls.Add(this.label12, 0, 7);
      	this.tableLayoutPanel1.Controls.Add(this.label13, 0, 8);
      	this.tableLayoutPanel1.Controls.Add(this.lSourceFolder, 1, 1);
      	this.tableLayoutPanel1.Controls.Add(this.tbScanIDPrefix, 1, 3);
      	this.tableLayoutPanel1.Controls.Add(this.tbHost, 1, 4);
      	this.tableLayoutPanel1.Controls.Add(this.tbUser, 1, 5);
      	this.tableLayoutPanel1.Controls.Add(this.tbPass, 1, 6);
      	this.tableLayoutPanel1.Controls.Add(this.tbUploadBase, 1, 7);
      	this.tableLayoutPanel1.Controls.Add(this.tbTriggerURL, 1, 8);
      	this.tableLayoutPanel1.Controls.Add(this.label14, 0, 2);
      	this.tableLayoutPanel1.Controls.Add(this.tbEmail, 1, 2);
      	this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 393);
      	this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      	this.tableLayoutPanel1.RowCount = 9;
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tableLayoutPanel1.Size = new System.Drawing.Size(376, 234);
      	this.tableLayoutPanel1.TabIndex = 7;
      	// 
      	// label6
      	// 
      	this.tableLayoutPanel1.SetColumnSpan(this.label6, 2);
      	this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.label6.Location = new System.Drawing.Point(3, 0);
      	this.label6.Name = "label6";
      	this.label6.Size = new System.Drawing.Size(370, 23);
      	this.label6.TabIndex = 0;
      	this.label6.Text = "Order and Upload Options";
      	// 
      	// label7
      	// 
      	this.label7.Location = new System.Drawing.Point(3, 23);
      	this.label7.Name = "label7";
      	this.label7.Size = new System.Drawing.Size(72, 23);
      	this.label7.TabIndex = 1;
      	this.label7.Text = "Source Folder:";
      	this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// label8
      	// 
      	this.label8.Location = new System.Drawing.Point(3, 72);
      	this.label8.Name = "label8";
      	this.label8.Size = new System.Drawing.Size(72, 23);
      	this.label8.TabIndex = 2;
      	this.label8.Text = "ScanID Prefix:";
      	this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// label9
      	// 
      	this.label9.Location = new System.Drawing.Point(3, 98);
      	this.label9.Name = "label9";
      	this.label9.Size = new System.Drawing.Size(72, 23);
      	this.label9.TabIndex = 3;
      	this.label9.Text = "Host:";
      	this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// label10
      	// 
      	this.label10.Location = new System.Drawing.Point(3, 124);
      	this.label10.Name = "label10";
      	this.label10.Size = new System.Drawing.Size(72, 23);
      	this.label10.TabIndex = 4;
      	this.label10.Text = "User:";
      	this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// label11
      	// 
      	this.label11.Location = new System.Drawing.Point(3, 150);
      	this.label11.Name = "label11";
      	this.label11.Size = new System.Drawing.Size(72, 23);
      	this.label11.TabIndex = 5;
      	this.label11.Text = "Pass:";
      	this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// label12
      	// 
      	this.label12.Location = new System.Drawing.Point(3, 176);
      	this.label12.Name = "label12";
      	this.label12.Size = new System.Drawing.Size(72, 23);
      	this.label12.TabIndex = 6;
      	this.label12.Text = "Upload Base:";
      	this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// label13
      	// 
      	this.label13.Location = new System.Drawing.Point(3, 202);
      	this.label13.Name = "label13";
      	this.label13.Size = new System.Drawing.Size(72, 23);
      	this.label13.TabIndex = 7;
      	this.label13.Text = "Trigger URL:";
      	this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// lSourceFolder
      	// 
      	this.lSourceFolder.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.lSourceFolder.Location = new System.Drawing.Point(81, 23);
      	this.lSourceFolder.Name = "lSourceFolder";
      	this.lSourceFolder.Size = new System.Drawing.Size(292, 23);
      	this.lSourceFolder.TabIndex = 8;
      	this.lSourceFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// tbScanIDPrefix
      	// 
      	this.tbScanIDPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbScanIDPrefix.Location = new System.Drawing.Point(81, 75);
      	this.tbScanIDPrefix.Name = "tbScanIDPrefix";
      	this.tbScanIDPrefix.Size = new System.Drawing.Size(292, 20);
      	this.tbScanIDPrefix.TabIndex = 9;
      	this.tbScanIDPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbScanIDPrefixKeyDown);
      	this.tbScanIDPrefix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbScanIDPrefixKeyPress);
      	// 
      	// tbHost
      	// 
      	this.tbHost.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbHost.Location = new System.Drawing.Point(81, 101);
      	this.tbHost.Name = "tbHost";
      	this.tbHost.Size = new System.Drawing.Size(292, 20);
      	this.tbHost.TabIndex = 10;
      	// 
      	// tbUser
      	// 
      	this.tbUser.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbUser.Location = new System.Drawing.Point(81, 127);
      	this.tbUser.Name = "tbUser";
      	this.tbUser.Size = new System.Drawing.Size(292, 20);
      	this.tbUser.TabIndex = 11;
      	// 
      	// tbPass
      	// 
      	this.tbPass.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbPass.Location = new System.Drawing.Point(81, 153);
      	this.tbPass.Name = "tbPass";
      	this.tbPass.Size = new System.Drawing.Size(292, 20);
      	this.tbPass.TabIndex = 12;
      	// 
      	// tbUploadBase
      	// 
      	this.tbUploadBase.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbUploadBase.Location = new System.Drawing.Point(81, 179);
      	this.tbUploadBase.Name = "tbUploadBase";
      	this.tbUploadBase.Size = new System.Drawing.Size(292, 20);
      	this.tbUploadBase.TabIndex = 13;
      	// 
      	// tbTriggerURL
      	// 
      	this.tbTriggerURL.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbTriggerURL.Location = new System.Drawing.Point(81, 205);
      	this.tbTriggerURL.Name = "tbTriggerURL";
      	this.tbTriggerURL.Size = new System.Drawing.Size(292, 20);
      	this.tbTriggerURL.TabIndex = 14;
      	// 
      	// label14
      	// 
      	this.label14.Location = new System.Drawing.Point(3, 46);
      	this.label14.Name = "label14";
      	this.label14.Size = new System.Drawing.Size(72, 23);
      	this.label14.TabIndex = 15;
      	this.label14.Text = "Your Email:";
      	this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      	// 
      	// tbEmail
      	// 
      	this.tbEmail.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.tbEmail.Location = new System.Drawing.Point(81, 49);
      	this.tbEmail.Name = "tbEmail";
      	this.tbEmail.Size = new System.Drawing.Size(292, 20);
      	this.tbEmail.TabIndex = 16;
      	// 
      	// panel1
      	// 
      	this.panel1.Controls.Add(this.btnSendToUploadManager);
      	this.panel1.Controls.Add(this.btnUploadScan);
      	this.panel1.Location = new System.Drawing.Point(0, 630);
      	this.panel1.Margin = new System.Windows.Forms.Padding(0);
      	this.panel1.Name = "panel1";
      	this.panel1.Size = new System.Drawing.Size(376, 23);
      	this.panel1.TabIndex = 10;
      	// 
      	// btnSendToUploadManager
      	// 
      	this.btnSendToUploadManager.Location = new System.Drawing.Point(201, 0);
      	this.btnSendToUploadManager.Name = "btnSendToUploadManager";
      	this.btnSendToUploadManager.Size = new System.Drawing.Size(173, 23);
      	this.btnSendToUploadManager.TabIndex = 8;
      	this.btnSendToUploadManager.Text = "Send Scan To Upload Manager";
      	this.btnSendToUploadManager.UseVisualStyleBackColor = true;
      	this.btnSendToUploadManager.Click += new System.EventHandler(this.BtnSendToUploadManagerClick);
      	// 
      	// btnUploadScan
      	// 
      	this.btnUploadScan.Location = new System.Drawing.Point(7, 0);
      	this.btnUploadScan.Name = "btnUploadScan";
      	this.btnUploadScan.Size = new System.Drawing.Size(179, 23);
      	this.btnUploadScan.TabIndex = 7;
      	this.btnUploadScan.Text = "Upload Scan Now";
      	this.btnUploadScan.UseVisualStyleBackColor = true;
      	this.btnUploadScan.Click += new System.EventHandler(this.BtnUploadScanClick);
      	// 
      	// pbUploadStatus
      	// 
      	this.pbUploadStatus.Location = new System.Drawing.Point(3, 656);
      	this.pbUploadStatus.MarqueeAnimationSpeed = 10;
      	this.pbUploadStatus.Name = "pbUploadStatus";
      	this.pbUploadStatus.Size = new System.Drawing.Size(376, 23);
      	this.pbUploadStatus.TabIndex = 8;
      	// 
      	// tabPage1
      	// 
      	this.tabPage1.AutoScroll = true;
      	this.tabPage1.Controls.Add(this.flpSingleCommands);
      	this.tabPage1.Location = new System.Drawing.Point(4, 22);
      	this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
      	this.tabPage1.Name = "tabPage1";
      	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      	this.tabPage1.Size = new System.Drawing.Size(394, 704);
      	this.tabPage1.TabIndex = 0;
      	this.tabPage1.Text = "Options and Settings";
      	this.tabPage1.UseVisualStyleBackColor = true;
      	// 
      	// flpSingleCommands
      	// 
      	this.flpSingleCommands.Controls.Add(this.label4);
      	this.flpSingleCommands.Controls.Add(this.tbSingleCommand);
      	this.flpSingleCommands.Controls.Add(this.btnGo);
      	this.flpSingleCommands.Controls.Add(this.cbForceCOMPort);
      	this.flpSingleCommands.Controls.Add(this.cbCOMPortList);
      	this.flpSingleCommands.Controls.Add(this.tlImageProcessingOptions);
      	this.flpSingleCommands.Controls.Add(this.btnExportConfig);
      	this.flpSingleCommands.Controls.Add(this.btnImportConfig);
      	this.flpSingleCommands.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.flpSingleCommands.Location = new System.Drawing.Point(3, 3);
      	this.flpSingleCommands.Margin = new System.Windows.Forms.Padding(0);
      	this.flpSingleCommands.Name = "flpSingleCommands";
      	this.flpSingleCommands.Size = new System.Drawing.Size(388, 698);
      	this.flpSingleCommands.TabIndex = 6;
      	// 
      	// label4
      	// 
      	this.label4.Location = new System.Drawing.Point(3, 0);
      	this.label4.Name = "label4";
      	this.label4.Size = new System.Drawing.Size(381, 23);
      	this.label4.TabIndex = 0;
      	this.label4.Text = "Enter a SugarCube Command:";
      	this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// tbSingleCommand
      	// 
      	this.tbSingleCommand.Location = new System.Drawing.Point(3, 26);
      	this.tbSingleCommand.Name = "tbSingleCommand";
      	this.tbSingleCommand.Size = new System.Drawing.Size(381, 20);
      	this.tbSingleCommand.TabIndex = 11;
      	this.tbSingleCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TbSingleCommandKeyUp);
      	// 
      	// btnGo
      	// 
      	this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.btnGo.Location = new System.Drawing.Point(3, 52);
      	this.btnGo.Name = "btnGo";
      	this.btnGo.Size = new System.Drawing.Size(379, 23);
      	this.btnGo.TabIndex = 2;
      	this.btnGo.Text = "Post Command to Cube";
      	this.btnGo.UseVisualStyleBackColor = true;
      	this.btnGo.Click += new System.EventHandler(this.BtnGoClick);
      	// 
      	// cbForceCOMPort
      	// 
      	this.cbForceCOMPort.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      	this.cbForceCOMPort.Location = new System.Drawing.Point(3, 81);
      	this.cbForceCOMPort.Name = "cbForceCOMPort";
      	this.cbForceCOMPort.Size = new System.Drawing.Size(110, 24);
      	this.cbForceCOMPort.TabIndex = 5;
      	this.cbForceCOMPort.Text = "Force COM Port:";
      	this.cbForceCOMPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      	this.cbForceCOMPort.UseVisualStyleBackColor = true;
      	this.cbForceCOMPort.CheckedChanged += new System.EventHandler(this.CbCOMPortCheckedChanged);
      	// 
      	// cbCOMPortList
      	// 
      	this.cbCOMPortList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      	this.cbCOMPortList.FormattingEnabled = true;
      	this.cbCOMPortList.Location = new System.Drawing.Point(119, 81);
      	this.cbCOMPortList.Name = "cbCOMPortList";
      	this.cbCOMPortList.Size = new System.Drawing.Size(263, 21);
      	this.cbCOMPortList.TabIndex = 6;
      	// 
      	// tlImageProcessingOptions
      	// 
      	this.tlImageProcessingOptions.ColumnCount = 2;
      	this.tlImageProcessingOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.52243F));
      	this.tlImageProcessingOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.47757F));
      	this.tlImageProcessingOptions.Controls.Add(this.tbQuality, 1, 1);
      	this.tlImageProcessingOptions.Controls.Add(this.lContrastValue, 0, 8);
      	this.tlImageProcessingOptions.Controls.Add(this.tbContrast, 1, 8);
      	this.tlImageProcessingOptions.Controls.Add(this.lMidPoint, 0, 7);
      	this.tlImageProcessingOptions.Controls.Add(this.tbMidpoint, 1, 7);
      	this.tlImageProcessingOptions.Controls.Add(this.cbUseContrastEnhancement, 0, 5);
      	this.tlImageProcessingOptions.Controls.Add(this.cbUseMedianStacking, 0, 4);
      	this.tlImageProcessingOptions.Controls.Add(this.tbSensitivity, 1, 3);
      	this.tlImageProcessingOptions.Controls.Add(this.lSensitivity, 0, 3);
      	this.tlImageProcessingOptions.Controls.Add(this.cbUseMotionDetection, 0, 2);
      	this.tlImageProcessingOptions.Controls.Add(this.label17, 0, 0);
      	this.tlImageProcessingOptions.Controls.Add(this.lJPEGQuality, 0, 1);
      	this.tlImageProcessingOptions.Controls.Add(this.cbSetEXIF, 0, 9);
      	this.tlImageProcessingOptions.Location = new System.Drawing.Point(3, 111);
      	this.tlImageProcessingOptions.Name = "tlImageProcessingOptions";
      	this.tlImageProcessingOptions.RowCount = 10;
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
      	this.tlImageProcessingOptions.Size = new System.Drawing.Size(379, 348);
      	this.tlImageProcessingOptions.TabIndex = 12;
      	// 
      	// tbQuality
      	// 
      	this.tbQuality.BackColor = System.Drawing.Color.White;
      	this.tbQuality.LargeChange = 10;
      	this.tbQuality.Location = new System.Drawing.Point(149, 26);
      	this.tbQuality.Maximum = 100;
      	this.tbQuality.Minimum = 1;
      	this.tbQuality.Name = "tbQuality";
      	this.tbQuality.Size = new System.Drawing.Size(227, 45);
      	this.tbQuality.TabIndex = 1;
      	this.tbQuality.TickFrequency = 10;
      	this.tbQuality.Value = 100;
      	this.tbQuality.ValueChanged += new System.EventHandler(this.TbQualityValueChanged);
      	// 
      	// lContrastValue
      	// 
      	this.lContrastValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      	this.lContrastValue.Location = new System.Drawing.Point(3, 281);
      	this.lContrastValue.Name = "lContrastValue";
      	this.lContrastValue.Size = new System.Drawing.Size(140, 20);
      	this.lContrastValue.TabIndex = 3;
      	this.lContrastValue.Text = "Contrast Value [2]";
      	this.lContrastValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// tbContrast
      	// 
      	this.tbContrast.BackColor = System.Drawing.Color.White;
      	this.tbContrast.LargeChange = 1;
      	this.tbContrast.Location = new System.Drawing.Point(149, 269);
      	this.tbContrast.Minimum = 1;
      	this.tbContrast.Name = "tbContrast";
      	this.tbContrast.Size = new System.Drawing.Size(227, 45);
      	this.tbContrast.TabIndex = 5;
      	this.tbContrast.Value = 2;
      	this.tbContrast.ValueChanged += new System.EventHandler(this.TbContrastValueChanged);
      	// 
      	// lMidPoint
      	// 
      	this.lMidPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      	this.lMidPoint.Location = new System.Drawing.Point(3, 230);
      	this.lMidPoint.Name = "lMidPoint";
      	this.lMidPoint.Size = new System.Drawing.Size(140, 20);
      	this.lMidPoint.TabIndex = 2;
      	this.lMidPoint.Text = "Contrast Midpoint [50%]";
      	this.lMidPoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// tbMidpoint
      	// 
      	this.tbMidpoint.BackColor = System.Drawing.Color.White;
      	this.tbMidpoint.LargeChange = 10;
      	this.tbMidpoint.Location = new System.Drawing.Point(149, 218);
      	this.tbMidpoint.Maximum = 100;
      	this.tbMidpoint.Minimum = 1;
      	this.tbMidpoint.Name = "tbMidpoint";
      	this.tbMidpoint.Size = new System.Drawing.Size(227, 45);
      	this.tbMidpoint.TabIndex = 4;
      	this.tbMidpoint.TickFrequency = 10;
      	this.tbMidpoint.Value = 50;
      	this.tbMidpoint.ValueChanged += new System.EventHandler(this.TbMidpointValueChanged);
      	// 
      	// cbUseContrastEnhancement
      	// 
      	this.cbUseContrastEnhancement.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      	this.tlImageProcessingOptions.SetColumnSpan(this.cbUseContrastEnhancement, 2);
      	this.cbUseContrastEnhancement.Location = new System.Drawing.Point(3, 188);
      	this.cbUseContrastEnhancement.Name = "cbUseContrastEnhancement";
      	this.cbUseContrastEnhancement.Size = new System.Drawing.Size(211, 24);
      	this.cbUseContrastEnhancement.TabIndex = 1;
      	this.cbUseContrastEnhancement.Text = "Use Sigmoidal Contrast Enhancement";
      	this.cbUseContrastEnhancement.UseVisualStyleBackColor = true;
      	this.cbUseContrastEnhancement.CheckedChanged += new System.EventHandler(this.CbUseContrastEnhancementCheckedChanged);
      	// 
      	// cbUseMedianStacking
      	// 
      	this.cbUseMedianStacking.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      	this.cbUseMedianStacking.Checked = true;
      	this.cbUseMedianStacking.CheckState = System.Windows.Forms.CheckState.Checked;
      	this.tlImageProcessingOptions.SetColumnSpan(this.cbUseMedianStacking, 2);
      	this.cbUseMedianStacking.Location = new System.Drawing.Point(3, 158);
      	this.cbUseMedianStacking.Name = "cbUseMedianStacking";
      	this.cbUseMedianStacking.Size = new System.Drawing.Size(140, 24);
      	this.cbUseMedianStacking.TabIndex = 0;
      	this.cbUseMedianStacking.Text = "Use Median Stacking";
      	this.cbUseMedianStacking.UseVisualStyleBackColor = true;
      	// 
      	// tbSensitivity
      	// 
      	this.tbSensitivity.BackColor = System.Drawing.Color.White;
      	this.tbSensitivity.LargeChange = 1;
      	this.tbSensitivity.Location = new System.Drawing.Point(149, 107);
      	this.tbSensitivity.Name = "tbSensitivity";
      	this.tbSensitivity.Size = new System.Drawing.Size(227, 45);
      	this.tbSensitivity.TabIndex = 8;
      	this.tbSensitivity.Value = 1;
      	this.tbSensitivity.ValueChanged += new System.EventHandler(this.TbSensitivityValueChanged);
      	// 
      	// lSensitivity
      	// 
      	this.lSensitivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      	this.lSensitivity.Location = new System.Drawing.Point(3, 115);
      	this.lSensitivity.Name = "lSensitivity";
      	this.lSensitivity.Size = new System.Drawing.Size(140, 28);
      	this.lSensitivity.TabIndex = 7;
      	this.lSensitivity.Text = "Motion Sensitivity [1]\r\n(larger = less sensitive)";
      	this.lSensitivity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// cbUseMotionDetection
      	// 
      	this.cbUseMotionDetection.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      	this.cbUseMotionDetection.Checked = true;
      	this.cbUseMotionDetection.CheckState = System.Windows.Forms.CheckState.Checked;
      	this.tlImageProcessingOptions.SetColumnSpan(this.cbUseMotionDetection, 2);
      	this.cbUseMotionDetection.Location = new System.Drawing.Point(3, 77);
      	this.cbUseMotionDetection.Name = "cbUseMotionDetection";
      	this.cbUseMotionDetection.Size = new System.Drawing.Size(148, 24);
      	this.cbUseMotionDetection.TabIndex = 10;
      	this.cbUseMotionDetection.Text = "Use Motion Detection";
      	this.cbUseMotionDetection.UseVisualStyleBackColor = true;
      	// 
      	// label17
      	// 
      	this.tlImageProcessingOptions.SetColumnSpan(this.label17, 2);
      	this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
      	this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.label17.Location = new System.Drawing.Point(3, 0);
      	this.label17.Name = "label17";
      	this.label17.Size = new System.Drawing.Size(373, 23);
      	this.label17.TabIndex = 11;
      	this.label17.Text = "Image Processing Options";
      	this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// lJPEGQuality
      	// 
      	this.lJPEGQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      	this.lJPEGQuality.Location = new System.Drawing.Point(3, 37);
      	this.lJPEGQuality.Name = "lJPEGQuality";
      	this.lJPEGQuality.Size = new System.Drawing.Size(140, 23);
      	this.lJPEGQuality.TabIndex = 0;
      	this.lJPEGQuality.Text = "JPEG Quality [100]";
      	this.lJPEGQuality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      	// 
      	// cbSetEXIF
      	// 
      	this.cbSetEXIF.AutoSize = true;
      	this.cbSetEXIF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      	this.cbSetEXIF.Checked = true;
      	this.cbSetEXIF.CheckState = System.Windows.Forms.CheckState.Checked;
      	this.tlImageProcessingOptions.SetColumnSpan(this.cbSetEXIF, 2);
      	this.cbSetEXIF.Location = new System.Drawing.Point(3, 320);
      	this.cbSetEXIF.Name = "cbSetEXIF";
      	this.cbSetEXIF.Size = new System.Drawing.Size(159, 17);
      	this.cbSetEXIF.TabIndex = 12;
      	this.cbSetEXIF.Text = "Automatically Set EXIF Data";
      	this.cbSetEXIF.UseVisualStyleBackColor = true;
      	// 
      	// btnExportConfig
      	// 
      	this.btnExportConfig.Location = new System.Drawing.Point(3, 465);
      	this.btnExportConfig.Name = "btnExportConfig";
      	this.btnExportConfig.Size = new System.Drawing.Size(75, 23);
      	this.btnExportConfig.TabIndex = 14;
      	this.btnExportConfig.Text = "Export Config";
      	this.btnExportConfig.UseVisualStyleBackColor = true;
      	this.btnExportConfig.Click += new System.EventHandler(this.BtnExportConfigClick);
      	// 
      	// btnImportConfig
      	// 
      	this.btnImportConfig.Location = new System.Drawing.Point(84, 465);
      	this.btnImportConfig.Name = "btnImportConfig";
      	this.btnImportConfig.Size = new System.Drawing.Size(75, 23);
      	this.btnImportConfig.TabIndex = 13;
      	this.btnImportConfig.Text = "Import Config";
      	this.btnImportConfig.UseVisualStyleBackColor = true;
      	this.btnImportConfig.Click += new System.EventHandler(this.BtnImportConfigClick);
      	// 
      	// tabPage3
      	// 
      	this.tabPage3.AutoScroll = true;
      	this.tabPage3.Controls.Add(this.btnZRefresh);
      	this.tabPage3.Controls.Add(this.btnDeleteZKey);
      	this.tabPage3.Controls.Add(this.btnAddZKey);
      	this.tabPage3.Controls.Add(this.label15);
      	this.tabPage3.Controls.Add(this.btnUpdateZString);
      	this.tabPage3.Controls.Add(this.zStringGrid);
      	this.tabPage3.Controls.Add(this.lZString);
      	this.tabPage3.Controls.Add(this.label5);
      	this.tabPage3.Location = new System.Drawing.Point(4, 22);
      	this.tabPage3.Name = "tabPage3";
      	this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
      	this.tabPage3.Size = new System.Drawing.Size(394, 704);
      	this.tabPage3.TabIndex = 2;
      	this.tabPage3.Text = "Z String";
      	this.tabPage3.UseVisualStyleBackColor = true;
      	// 
      	// btnZRefresh
      	// 
      	this.btnZRefresh.Location = new System.Drawing.Point(259, 438);
      	this.btnZRefresh.Name = "btnZRefresh";
      	this.btnZRefresh.Size = new System.Drawing.Size(120, 23);
      	this.btnZRefresh.TabIndex = 8;
      	this.btnZRefresh.Text = "Refresh from Cube";
      	this.btnZRefresh.UseVisualStyleBackColor = true;
      	this.btnZRefresh.Click += new System.EventHandler(this.BtnZRefreshClick);
      	// 
      	// btnDeleteZKey
      	// 
      	this.btnDeleteZKey.ForeColor = System.Drawing.Color.Red;
      	this.btnDeleteZKey.Location = new System.Drawing.Point(133, 411);
      	this.btnDeleteZKey.Name = "btnDeleteZKey";
      	this.btnDeleteZKey.Size = new System.Drawing.Size(120, 23);
      	this.btnDeleteZKey.TabIndex = 7;
      	this.btnDeleteZKey.Text = "Delete Selected Key";
      	this.btnDeleteZKey.UseVisualStyleBackColor = true;
      	this.btnDeleteZKey.Click += new System.EventHandler(this.BtnDeleteZKeyClick);
      	// 
      	// btnAddZKey
      	// 
      	this.btnAddZKey.Location = new System.Drawing.Point(7, 411);
      	this.btnAddZKey.Name = "btnAddZKey";
      	this.btnAddZKey.Size = new System.Drawing.Size(120, 23);
      	this.btnAddZKey.TabIndex = 6;
      	this.btnAddZKey.Text = "Add New Key";
      	this.btnAddZKey.UseVisualStyleBackColor = true;
      	this.btnAddZKey.Click += new System.EventHandler(this.BtnAddZKeyClick);
      	// 
      	// label15
      	// 
      	this.label15.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.label15.Location = new System.Drawing.Point(0, 59);
      	this.label15.Name = "label15";
      	this.label15.Size = new System.Drawing.Size(386, 125);
      	this.label15.TabIndex = 5;
      	this.label15.Text = resources.GetString("label15.Text");
      	// 
      	// btnUpdateZString
      	// 
      	this.btnUpdateZString.Location = new System.Drawing.Point(259, 411);
      	this.btnUpdateZString.Name = "btnUpdateZString";
      	this.btnUpdateZString.Size = new System.Drawing.Size(120, 23);
      	this.btnUpdateZString.TabIndex = 4;
      	this.btnUpdateZString.Text = "Update Z String";
      	this.btnUpdateZString.UseVisualStyleBackColor = true;
      	this.btnUpdateZString.Click += new System.EventHandler(this.BtnUpdateZStringClick);
      	// 
      	// zStringGrid
      	// 
      	this.zStringGrid.AllowUserToAddRows = false;
      	this.zStringGrid.AllowUserToDeleteRows = false;
      	this.zStringGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      	this.zStringGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      	this.zStringGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      	this.zStringGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
      	      	      	this.colID,
      	      	      	this.colDescription,
      	      	      	this.colValue});
      	this.zStringGrid.Location = new System.Drawing.Point(4, 187);
      	this.zStringGrid.Name = "zStringGrid";
      	this.zStringGrid.Size = new System.Drawing.Size(394, 218);
      	this.zStringGrid.TabIndex = 3;
      	// 
      	// colID
      	// 
      	dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
      	this.colID.DefaultCellStyle = dataGridViewCellStyle2;
      	this.colID.HeaderText = "Key";
      	this.colID.Name = "colID";
      	this.colID.ReadOnly = true;
      	this.colID.Width = 50;
      	// 
      	// colDescription
      	// 
      	this.colDescription.HeaderText = "Description";
      	this.colDescription.Name = "colDescription";
      	this.colDescription.Width = 85;
      	// 
      	// colValue
      	// 
      	this.colValue.HeaderText = "Value";
      	this.colValue.Name = "colValue";
      	this.colValue.Width = 59;
      	// 
      	// lZString
      	// 
      	this.lZString.AutoSize = true;
      	this.lZString.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.lZString.Location = new System.Drawing.Point(3, 26);
      	this.lZString.Name = "lZString";
      	this.lZString.Size = new System.Drawing.Size(61, 11);
      	this.lZString.TabIndex = 1;
      	this.lZString.Text = "lZString";
      	// 
      	// label5
      	// 
      	this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      	this.label5.Location = new System.Drawing.Point(3, 3);
      	this.label5.Name = "label5";
      	this.label5.Size = new System.Drawing.Size(100, 23);
      	this.label5.TabIndex = 0;
      	this.label5.Text = "Current Z String Values";
      	// 
      	// importFileDialog
      	// 
      	this.importFileDialog.AddExtension = false;
      	this.importFileDialog.Title = "Select Config File to Import";
      	// 
      	// exportFileDialog
      	// 
      	this.exportFileDialog.FileName = "SugarCube.cfg";
      	this.exportFileDialog.Title = "Select Location for Exported Config File";
      	// 
      	// MainForm
      	// 
      	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      	this.AutoScroll = true;
      	this.ClientSize = new System.Drawing.Size(1008, 730);
      	this.Controls.Add(this.tlp1);
      	this.Name = "MainForm";
      	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      	this.Text = "SugarCube Tester [build {0}]";
      	this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
      	this.Load += new System.EventHandler(this.MainFormLoad);
      	this.tlp1.ResumeLayout(false);
      	this.tlp2.ResumeLayout(false);
      	this.tlp3.ResumeLayout(false);
      	this.tlp4.ResumeLayout(false);
      	this.gbSugarCubeLog.ResumeLayout(false);
      	this.gbSugarCubeLog.PerformLayout();
      	this.gbTesterLog.ResumeLayout(false);
      	this.gbTesterLog.PerformLayout();
      	this.tcCubeCommander.ResumeLayout(false);
      	this.tabPage2.ResumeLayout(false);
      	this.flpShootStrings.ResumeLayout(false);
      	this.flpShootStrings.PerformLayout();
      	this.tableLayoutPanel1.ResumeLayout(false);
      	this.tableLayoutPanel1.PerformLayout();
      	this.panel1.ResumeLayout(false);
      	this.tabPage1.ResumeLayout(false);
      	this.flpSingleCommands.ResumeLayout(false);
      	this.flpSingleCommands.PerformLayout();
      	this.tlImageProcessingOptions.ResumeLayout(false);
      	this.tlImageProcessingOptions.PerformLayout();
      	((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
      	((System.ComponentModel.ISupportInitialize)(this.tbContrast)).EndInit();
      	((System.ComponentModel.ISupportInitialize)(this.tbMidpoint)).EndInit();
      	((System.ComponentModel.ISupportInitialize)(this.tbSensitivity)).EndInit();
      	this.tabPage3.ResumeLayout(false);
      	this.tabPage3.PerformLayout();
      	((System.ComponentModel.ISupportInitialize)(this.zStringGrid)).EndInit();
      	this.ResumeLayout(false);
      }
      private System.Windows.Forms.Button btnSendToUploadManager;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.SaveFileDialog exportFileDialog;
      private System.Windows.Forms.OpenFileDialog importFileDialog;
      private System.Windows.Forms.Button btnImportConfig;
      private System.Windows.Forms.Button btnExportConfig;
      private System.Windows.Forms.Button btnZRefresh;
      private System.Windows.Forms.Button btnAddZKey;
      private System.Windows.Forms.Button btnDeleteZKey;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.CheckBox cbSetEXIF;
      private System.Windows.Forms.Button btnUpdateZString;
      private System.Windows.Forms.DataGridViewTextBoxColumn colID;
      private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
      private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
      private System.Windows.Forms.DataGridView zStringGrid;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label lZString;
      private System.Windows.Forms.TabPage tabPage3;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.TrackBar tbContrast;
      private System.Windows.Forms.TrackBar tbMidpoint;
      private System.Windows.Forms.Label lContrastValue;
      private System.Windows.Forms.Label lMidPoint;
      private System.Windows.Forms.CheckBox cbUseContrastEnhancement;
      private System.Windows.Forms.CheckBox cbUseMedianStacking;
      private System.Windows.Forms.TableLayoutPanel tlImageProcessingOptions;
      private System.Windows.Forms.Button btnSetModellingOpts;
      private System.Windows.Forms.TextBox tbEmail;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.ProgressBar pbUploadStatus;
      private System.Windows.Forms.TextBox tbTriggerURL;
      private System.Windows.Forms.TextBox tbUploadBase;
      private System.Windows.Forms.TextBox tbPass;
      private System.Windows.Forms.TextBox tbUser;
      private System.Windows.Forms.TextBox tbHost;
      private System.Windows.Forms.TextBox tbScanIDPrefix;
      private System.Windows.Forms.Label lSourceFolder;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.TextBox tbSingleCommand;
      private System.Windows.Forms.CheckBox cbUseMotionDetection;
      private System.Windows.Forms.TrackBar tbSensitivity;
      private System.Windows.Forms.Label lSensitivity;
      private System.Windows.Forms.ComboBox cbCOMPortList;
      private System.Windows.Forms.CheckBox cbForceCOMPort;
      private System.Windows.Forms.TrackBar tbQuality;
      private System.Windows.Forms.Label lJPEGQuality;
      private System.Windows.Forms.Button btnUploadScan;
      private System.Windows.Forms.Button btnCreateDummyForms;
      private System.Windows.Forms.Button btnGenerateXMLManifest;
      private System.Windows.Forms.Button btnCancelShootString;
      private System.Windows.Forms.Button btnOpenShootFolder;
      private System.Windows.Forms.TextBox tbShootString;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.FlowLayoutPanel flpSingleCommands;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.FlowLayoutPanel flpShootStrings;
      private System.Windows.Forms.TabPage tabPage2;
      private System.Windows.Forms.TabPage tabPage1;
      private System.Windows.Forms.TabControl tcCubeCommander;
      private System.Windows.Forms.TableLayoutPanel tlp4;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TableLayoutPanel tlp3;
      private System.Windows.Forms.TableLayoutPanel tlp2;
      private System.Windows.Forms.TableLayoutPanel tlp1;
      private System.Windows.Forms.TextBox tbTesterLog;
      private System.Windows.Forms.GroupBox gbTesterLog;
      private System.Windows.Forms.GroupBox gbSugarCubeLog;
      private System.Windows.Forms.Button btnSnapshot;
      private System.Windows.Forms.Button btnRunShootString;
      private System.Windows.Forms.Button btnGo;
      private System.Windows.Forms.ComboBox cbResolutionList;
      private System.Windows.Forms.Button btnSettings;
      private System.Windows.Forms.ComboBox cbCameraList;
      private System.Windows.Forms.TextBox tbSugarCubeLog;
      private Camera_NET.CameraControl camControl;
   }
}
