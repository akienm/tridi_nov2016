/* SugarCube Host Software - Manager MVP
 * Copyright (c) 2014-2015 Chad Ullman
 */
 
namespace Me.ThreeDWares.SugarCube
{
	partial class ConfigManager
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigManager));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tbResolution = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.tbCamName = new System.Windows.Forms.TextBox();
			this.tbCropRectangle = new System.Windows.Forms.TextBox();
			this.tbTemplate = new System.Windows.Forms.TextBox();
			this.tbScanIDPrefix = new System.Windows.Forms.TextBox();
			this.tbUser = new System.Windows.Forms.TextBox();
			this.tbPass = new System.Windows.Forms.TextBox();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.tbUploadBase = new System.Windows.Forms.TextBox();
			this.tbTriggerURL = new System.Windows.Forms.TextBox();
			this.tbShootString = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.udQuality = new System.Windows.Forms.NumericUpDown();
			this.udSensitivity = new System.Windows.Forms.NumericUpDown();
			this.udMeshLevel = new System.Windows.Forms.NumericUpDown();
			this.cbMotionDetection = new System.Windows.Forms.CheckBox();
			this.label18 = new System.Windows.Forms.Label();
			this.tbHcryptPass = new System.Windows.Forms.TextBox();
			this.cbDoImageCropping = new System.Windows.Forms.CheckBox();
			this.label26 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.cbMedianStacking = new System.Windows.Forms.CheckBox();
			this.cbContrastEnhancement = new System.Windows.Forms.CheckBox();
			this.udMidpoint = new System.Windows.Forms.NumericUpDown();
			this.udContrastValue = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.tbBoundBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbDoModelCropping = new System.Windows.Forms.CheckBox();
			this.cbDoModelScaling = new System.Windows.Forms.CheckBox();
			this.label27 = new System.Windows.Forms.Label();
			this.cbSetEXIFData = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udQuality)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udSensitivity)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udMeshLevel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udMidpoint)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udContrastValue)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoScroll = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 693F));
			this.tableLayoutPanel1.Controls.Add(this.tbResolution, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label15, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.label13, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.label11, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.label9, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label16, 0, 13);
			this.tableLayoutPanel1.Controls.Add(this.label14, 0, 14);
			this.tableLayoutPanel1.Controls.Add(this.label12, 0, 15);
			this.tableLayoutPanel1.Controls.Add(this.label10, 0, 16);
			this.tableLayoutPanel1.Controls.Add(this.label8, 0, 17);
			this.tableLayoutPanel1.Controls.Add(this.label25, 0, 21);
			this.tableLayoutPanel1.Controls.Add(this.label24, 0, 22);
			this.tableLayoutPanel1.Controls.Add(this.label23, 0, 23);
			this.tableLayoutPanel1.Controls.Add(this.label22, 0, 24);
			this.tableLayoutPanel1.Controls.Add(this.label21, 0, 25);
			this.tableLayoutPanel1.Controls.Add(this.label20, 0, 26);
			this.tableLayoutPanel1.Controls.Add(this.label19, 0, 28);
			this.tableLayoutPanel1.Controls.Add(this.label17, 0, 29);
			this.tableLayoutPanel1.Controls.Add(this.tbCamName, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.tbCropRectangle, 1, 15);
			this.tableLayoutPanel1.Controls.Add(this.tbTemplate, 1, 17);
			this.tableLayoutPanel1.Controls.Add(this.tbScanIDPrefix, 1, 22);
			this.tableLayoutPanel1.Controls.Add(this.tbUser, 1, 23);
			this.tableLayoutPanel1.Controls.Add(this.tbPass, 1, 24);
			this.tableLayoutPanel1.Controls.Add(this.tbHost, 1, 25);
			this.tableLayoutPanel1.Controls.Add(this.tbUploadBase, 1, 26);
			this.tableLayoutPanel1.Controls.Add(this.tbTriggerURL, 1, 28);
			this.tableLayoutPanel1.Controls.Add(this.tbShootString, 0, 30);
			this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 31);
			this.tableLayoutPanel1.Controls.Add(this.udQuality, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.udSensitivity, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.udMeshLevel, 1, 16);
			this.tableLayoutPanel1.Controls.Add(this.cbMotionDetection, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.label18, 0, 27);
			this.tableLayoutPanel1.Controls.Add(this.tbHcryptPass, 1, 27);
			this.tableLayoutPanel1.Controls.Add(this.cbDoImageCropping, 1, 14);
			this.tableLayoutPanel1.Controls.Add(this.label26, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.label29, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.label30, 0, 10);
			this.tableLayoutPanel1.Controls.Add(this.label31, 0, 11);
			this.tableLayoutPanel1.Controls.Add(this.cbMedianStacking, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.cbContrastEnhancement, 1, 9);
			this.tableLayoutPanel1.Controls.Add(this.udMidpoint, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.udContrastValue, 1, 11);
			this.tableLayoutPanel1.Controls.Add(this.label6, 0, 20);
			this.tableLayoutPanel1.Controls.Add(this.tbBoundBox, 1, 20);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 18);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 19);
			this.tableLayoutPanel1.Controls.Add(this.cbDoModelCropping, 1, 19);
			this.tableLayoutPanel1.Controls.Add(this.cbDoModelScaling, 1, 18);
			this.tableLayoutPanel1.Controls.Add(this.label27, 0, 12);
			this.tableLayoutPanel1.Controls.Add(this.cbSetEXIFData, 1, 12);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 33;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(890, 647);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// tbResolution
			// 
			this.tbResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbResolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbResolution.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbResolution.Location = new System.Drawing.Point(183, 84);
			this.tbResolution.Name = "tbResolution";
			this.tbResolution.Size = new System.Drawing.Size(687, 20);
			this.tbResolution.TabIndex = 27;
			// 
			// label15
			// 
			this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label15.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label15.Location = new System.Drawing.Point(3, 189);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(174, 27);
			this.label15.TabIndex = 14;
			this.label15.Text = "detection sensitivity";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label13
			// 
			this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label13.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label13.Location = new System.Drawing.Point(3, 162);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(174, 27);
			this.label13.TabIndex = 12;
			this.label13.Text = "use motion detection";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label11.Location = new System.Drawing.Point(3, 135);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(174, 27);
			this.label11.TabIndex = 10;
			this.label11.Text = "quality";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label9.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label9.Location = new System.Drawing.Point(3, 108);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(174, 27);
			this.label9.TabIndex = 8;
			this.label9.Text = "SHOOTING";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label7.Location = new System.Drawing.Point(3, 81);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(174, 27);
			this.label7.TabIndex = 6;
			this.label7.Text = "resolution";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
			this.label1.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(867, 27);
			this.label1.TabIndex = 0;
			this.label1.Text = "This is the configuration manager.  If you don\'t know what you\'re doing, you shou" +
			"ldn\'t be here!\r\n";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label2.Location = new System.Drawing.Point(3, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(174, 27);
			this.label2.TabIndex = 1;
			this.label2.Text = "CAMERA";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label3.Location = new System.Drawing.Point(3, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(174, 27);
			this.label3.TabIndex = 2;
			this.label3.Text = "camera name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label16
			// 
			this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label16.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label16.Location = new System.Drawing.Point(3, 351);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(174, 27);
			this.label16.TabIndex = 15;
			this.label16.Text = "MODELING OPTIONS";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label14.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label14.Location = new System.Drawing.Point(3, 378);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(174, 27);
			this.label14.TabIndex = 13;
			this.label14.Text = "do image cropping";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label12.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label12.Location = new System.Drawing.Point(3, 405);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(174, 27);
			this.label12.TabIndex = 11;
			this.label12.Text = "image crop rectangle";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label10.Location = new System.Drawing.Point(3, 432);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(174, 27);
			this.label10.TabIndex = 9;
			this.label10.Text = "mesh level";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label8.Location = new System.Drawing.Point(3, 459);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(174, 27);
			this.label8.TabIndex = 7;
			this.label8.Text = "3dp template";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label25
			// 
			this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label25.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label25.Location = new System.Drawing.Point(3, 567);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(174, 27);
			this.label25.TabIndex = 24;
			this.label25.Text = "SFTP";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label24
			// 
			this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label24.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label24.Location = new System.Drawing.Point(3, 594);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(174, 27);
			this.label24.TabIndex = 23;
			this.label24.Text = "scan ID prefix";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label23
			// 
			this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label23.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label23.Location = new System.Drawing.Point(3, 621);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(174, 27);
			this.label23.TabIndex = 22;
			this.label23.Text = "SFTP user";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label22.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label22.Location = new System.Drawing.Point(3, 648);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(174, 27);
			this.label22.TabIndex = 21;
			this.label22.Text = "SFTP pass";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label21
			// 
			this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label21.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label21.Location = new System.Drawing.Point(3, 675);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(174, 27);
			this.label21.TabIndex = 20;
			this.label21.Text = "SFTP host";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label20
			// 
			this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label20.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label20.Location = new System.Drawing.Point(3, 702);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(174, 27);
			this.label20.TabIndex = 19;
			this.label20.Text = "upload base";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label19
			// 
			this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label19.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label19.Location = new System.Drawing.Point(3, 756);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(174, 27);
			this.label19.TabIndex = 18;
			this.label19.Text = "trigger URL";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label17.Location = new System.Drawing.Point(3, 783);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(167, 27);
			this.label17.TabIndex = 16;
			this.label17.Text = "SHOOT STRING";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbCamName
			// 
			this.tbCamName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCamName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbCamName.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbCamName.Location = new System.Drawing.Point(183, 57);
			this.tbCamName.Name = "tbCamName";
			this.tbCamName.Size = new System.Drawing.Size(687, 20);
			this.tbCamName.TabIndex = 25;
			// 
			// tbCropRectangle
			// 
			this.tbCropRectangle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCropRectangle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbCropRectangle.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbCropRectangle.Location = new System.Drawing.Point(183, 408);
			this.tbCropRectangle.Name = "tbCropRectangle";
			this.tbCropRectangle.Size = new System.Drawing.Size(687, 20);
			this.tbCropRectangle.TabIndex = 29;
			// 
			// tbTemplate
			// 
			this.tbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbTemplate.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbTemplate.Location = new System.Drawing.Point(183, 462);
			this.tbTemplate.Name = "tbTemplate";
			this.tbTemplate.Size = new System.Drawing.Size(687, 20);
			this.tbTemplate.TabIndex = 30;
			// 
			// tbScanIDPrefix
			// 
			this.tbScanIDPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbScanIDPrefix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbScanIDPrefix.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbScanIDPrefix.Location = new System.Drawing.Point(183, 597);
			this.tbScanIDPrefix.Name = "tbScanIDPrefix";
			this.tbScanIDPrefix.Size = new System.Drawing.Size(687, 20);
			this.tbScanIDPrefix.TabIndex = 31;
			this.tbScanIDPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbScanIDPrefixKeyDown);
			this.tbScanIDPrefix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbScanIDPrefixKeyPress);
			// 
			// tbUser
			// 
			this.tbUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbUser.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbUser.Location = new System.Drawing.Point(183, 624);
			this.tbUser.Name = "tbUser";
			this.tbUser.Size = new System.Drawing.Size(687, 20);
			this.tbUser.TabIndex = 28;
			// 
			// tbPass
			// 
			this.tbPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbPass.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbPass.Location = new System.Drawing.Point(183, 651);
			this.tbPass.Name = "tbPass";
			this.tbPass.Size = new System.Drawing.Size(687, 20);
			this.tbPass.TabIndex = 26;
			// 
			// tbHost
			// 
			this.tbHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbHost.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbHost.Location = new System.Drawing.Point(183, 678);
			this.tbHost.Name = "tbHost";
			this.tbHost.Size = new System.Drawing.Size(687, 20);
			this.tbHost.TabIndex = 33;
			// 
			// tbUploadBase
			// 
			this.tbUploadBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUploadBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbUploadBase.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbUploadBase.Location = new System.Drawing.Point(183, 705);
			this.tbUploadBase.Name = "tbUploadBase";
			this.tbUploadBase.Size = new System.Drawing.Size(687, 20);
			this.tbUploadBase.TabIndex = 34;
			// 
			// tbTriggerURL
			// 
			this.tbTriggerURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTriggerURL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbTriggerURL.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbTriggerURL.Location = new System.Drawing.Point(183, 759);
			this.tbTriggerURL.Name = "tbTriggerURL";
			this.tbTriggerURL.Size = new System.Drawing.Size(687, 20);
			this.tbTriggerURL.TabIndex = 35;
			// 
			// tbShootString
			// 
			this.tbShootString.AcceptsReturn = true;
			this.tbShootString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbShootString.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tableLayoutPanel1.SetColumnSpan(this.tbShootString, 2);
			this.tbShootString.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbShootString.Location = new System.Drawing.Point(3, 813);
			this.tbShootString.Multiline = true;
			this.tbShootString.Name = "tbShootString";
			this.tbShootString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbShootString.Size = new System.Drawing.Size(867, 294);
			this.tbShootString.TabIndex = 36;
			this.tbShootString.WordWrap = false;
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnOK.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnOK.Location = new System.Drawing.Point(183, 1113);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(103, 27);
			this.btnOK.TabIndex = 38;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
			// 
			// udQuality
			// 
			this.udQuality.Location = new System.Drawing.Point(183, 138);
			this.udQuality.Name = "udQuality";
			this.udQuality.Size = new System.Drawing.Size(120, 20);
			this.udQuality.TabIndex = 41;
			// 
			// udSensitivity
			// 
			this.udSensitivity.Location = new System.Drawing.Point(183, 192);
			this.udSensitivity.Maximum = new decimal(new int[] {
									10,
									0,
									0,
									0});
			this.udSensitivity.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.udSensitivity.Name = "udSensitivity";
			this.udSensitivity.Size = new System.Drawing.Size(120, 20);
			this.udSensitivity.TabIndex = 42;
			this.udSensitivity.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// udMeshLevel
			// 
			this.udMeshLevel.Location = new System.Drawing.Point(183, 435);
			this.udMeshLevel.Maximum = new decimal(new int[] {
									9,
									0,
									0,
									0});
			this.udMeshLevel.Minimum = new decimal(new int[] {
									7,
									0,
									0,
									0});
			this.udMeshLevel.Name = "udMeshLevel";
			this.udMeshLevel.Size = new System.Drawing.Size(120, 20);
			this.udMeshLevel.TabIndex = 46;
			this.udMeshLevel.Value = new decimal(new int[] {
									7,
									0,
									0,
									0});
			// 
			// cbMotionDetection
			// 
			this.cbMotionDetection.Location = new System.Drawing.Point(183, 165);
			this.cbMotionDetection.Name = "cbMotionDetection";
			this.cbMotionDetection.Size = new System.Drawing.Size(104, 21);
			this.cbMotionDetection.TabIndex = 49;
			this.cbMotionDetection.UseVisualStyleBackColor = true;
			this.cbMotionDetection.CheckedChanged += new System.EventHandler(this.CbMotionDetectionCheckedChanged);
			// 
			// label18
			// 
			this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label18.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label18.Location = new System.Drawing.Point(3, 729);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(174, 27);
			this.label18.TabIndex = 52;
			this.label18.Text = "hcrypt password";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbHcryptPass
			// 
			this.tbHcryptPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHcryptPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbHcryptPass.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbHcryptPass.Location = new System.Drawing.Point(183, 732);
			this.tbHcryptPass.Name = "tbHcryptPass";
			this.tbHcryptPass.Size = new System.Drawing.Size(687, 20);
			this.tbHcryptPass.TabIndex = 53;
			// 
			// cbDoImageCropping
			// 
			this.cbDoImageCropping.Location = new System.Drawing.Point(183, 381);
			this.cbDoImageCropping.Name = "cbDoImageCropping";
			this.cbDoImageCropping.Size = new System.Drawing.Size(104, 21);
			this.cbDoImageCropping.TabIndex = 51;
			this.cbDoImageCropping.UseVisualStyleBackColor = true;
			this.cbDoImageCropping.CheckedChanged += new System.EventHandler(this.CbDoImageCroppingCheckedChanged);
			// 
			// label26
			// 
			this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label26.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label26.Location = new System.Drawing.Point(3, 216);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(174, 27);
			this.label26.TabIndex = 54;
			this.label26.Text = "use median stacking";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label29
			// 
			this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label29.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label29.Location = new System.Drawing.Point(3, 243);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(174, 27);
			this.label29.TabIndex = 54;
			this.label29.Text = "use contrast enhancement";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label30
			// 
			this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label30.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label30.Location = new System.Drawing.Point(3, 270);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(174, 27);
			this.label30.TabIndex = 54;
			this.label30.Text = "contrast midpoint";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label31
			// 
			this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label31.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label31.Location = new System.Drawing.Point(3, 297);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(174, 27);
			this.label31.TabIndex = 54;
			this.label31.Text = "contrast value";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbMedianStacking
			// 
			this.cbMedianStacking.Location = new System.Drawing.Point(183, 219);
			this.cbMedianStacking.Name = "cbMedianStacking";
			this.cbMedianStacking.Size = new System.Drawing.Size(104, 21);
			this.cbMedianStacking.TabIndex = 49;
			this.cbMedianStacking.UseVisualStyleBackColor = true;
			// 
			// cbContrastEnhancement
			// 
			this.cbContrastEnhancement.Location = new System.Drawing.Point(183, 246);
			this.cbContrastEnhancement.Name = "cbContrastEnhancement";
			this.cbContrastEnhancement.Size = new System.Drawing.Size(104, 21);
			this.cbContrastEnhancement.TabIndex = 49;
			this.cbContrastEnhancement.UseVisualStyleBackColor = true;
			this.cbContrastEnhancement.CheckedChanged += new System.EventHandler(this.CbContrastEnhancementCheckedChanged);
			// 
			// udMidpoint
			// 
			this.udMidpoint.Location = new System.Drawing.Point(183, 273);
			this.udMidpoint.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.udMidpoint.Name = "udMidpoint";
			this.udMidpoint.Size = new System.Drawing.Size(120, 20);
			this.udMidpoint.TabIndex = 42;
			this.udMidpoint.Value = new decimal(new int[] {
									50,
									0,
									0,
									0});
			// 
			// udContrastValue
			// 
			this.udContrastValue.Location = new System.Drawing.Point(183, 300);
			this.udContrastValue.Maximum = new decimal(new int[] {
									10,
									0,
									0,
									0});
			this.udContrastValue.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.udContrastValue.Name = "udContrastValue";
			this.udContrastValue.Size = new System.Drawing.Size(120, 20);
			this.udContrastValue.TabIndex = 42;
			this.udContrastValue.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label6.Location = new System.Drawing.Point(3, 540);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(174, 27);
			this.label6.TabIndex = 5;
			this.label6.Text = "bounding box";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbBoundBox
			// 
			this.tbBoundBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbBoundBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tbBoundBox.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbBoundBox.Location = new System.Drawing.Point(183, 543);
			this.tbBoundBox.Name = "tbBoundBox";
			this.tbBoundBox.Size = new System.Drawing.Size(687, 20);
			this.tbBoundBox.TabIndex = 32;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label5.Location = new System.Drawing.Point(3, 486);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(174, 27);
			this.label5.TabIndex = 4;
			this.label5.Text = "do model scaling";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label4.Location = new System.Drawing.Point(3, 513);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(174, 27);
			this.label4.TabIndex = 3;
			this.label4.Text = "do model cropping";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbDoModelCropping
			// 
			this.cbDoModelCropping.Location = new System.Drawing.Point(183, 516);
			this.cbDoModelCropping.Name = "cbDoModelCropping";
			this.cbDoModelCropping.Size = new System.Drawing.Size(104, 21);
			this.cbDoModelCropping.TabIndex = 48;
			this.cbDoModelCropping.UseVisualStyleBackColor = true;
			this.cbDoModelCropping.CheckedChanged += new System.EventHandler(this.CbDoModelCroppingCheckedChanged);
			// 
			// cbDoModelScaling
			// 
			this.cbDoModelScaling.Location = new System.Drawing.Point(183, 489);
			this.cbDoModelScaling.Name = "cbDoModelScaling";
			this.cbDoModelScaling.Size = new System.Drawing.Size(104, 21);
			this.cbDoModelScaling.TabIndex = 50;
			this.cbDoModelScaling.UseVisualStyleBackColor = true;
			this.cbDoModelScaling.CheckedChanged += new System.EventHandler(this.CbDoModelScalingCheckedChanged);
			// 
			// label27
			// 
			this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label27.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label27.Location = new System.Drawing.Point(3, 326);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(174, 23);
			this.label27.TabIndex = 55;
			this.label27.Text = "set EXIF data";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbSetEXIFData
			// 
			this.cbSetEXIFData.Location = new System.Drawing.Point(183, 327);
			this.cbSetEXIFData.Name = "cbSetEXIFData";
			this.cbSetEXIFData.Size = new System.Drawing.Size(104, 21);
			this.cbSetEXIFData.TabIndex = 56;
			this.cbSetEXIFData.UseVisualStyleBackColor = true;
			// 
			// ConfigManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(890, 647);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ConfigManager";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configuration Manager";
			this.Load += new System.EventHandler(this.ConfigManagerLoad);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udQuality)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udSensitivity)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udMeshLevel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udMidpoint)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udContrastValue)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.CheckBox cbSetEXIFData;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.NumericUpDown udContrastValue;
		private System.Windows.Forms.NumericUpDown udMidpoint;
		private System.Windows.Forms.CheckBox cbContrastEnhancement;
		private System.Windows.Forms.CheckBox cbMedianStacking;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TextBox tbHcryptPass;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.CheckBox cbDoImageCropping;
		private System.Windows.Forms.CheckBox cbDoModelScaling;
		private System.Windows.Forms.CheckBox cbMotionDetection;
		private System.Windows.Forms.CheckBox cbDoModelCropping;
		private System.Windows.Forms.NumericUpDown udMeshLevel;
		private System.Windows.Forms.NumericUpDown udSensitivity;
		private System.Windows.Forms.NumericUpDown udQuality;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox tbShootString;
		private System.Windows.Forms.TextBox tbTriggerURL;
		private System.Windows.Forms.TextBox tbUploadBase;
		private System.Windows.Forms.TextBox tbHost;
		private System.Windows.Forms.TextBox tbPass;
		private System.Windows.Forms.TextBox tbUser;
		private System.Windows.Forms.TextBox tbScanIDPrefix;
		private System.Windows.Forms.TextBox tbBoundBox;
		private System.Windows.Forms.TextBox tbTemplate;
		private System.Windows.Forms.TextBox tbCropRectangle;
		private System.Windows.Forms.TextBox tbCamName;
		private System.Windows.Forms.TextBox tbResolution;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
