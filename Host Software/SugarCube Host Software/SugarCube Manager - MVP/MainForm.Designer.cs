/* SugarCube Host Software - Manager MVP
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
			this.tlMainFrame = new System.Windows.Forms.TableLayoutPanel();
			this.tlLeftFrame = new System.Windows.Forms.TableLayoutPanel();
			this.btnCompleteStep1 = new System.Windows.Forms.Button();
			this.panel7 = new System.Windows.Forms.Panel();
			this.panel6 = new System.Windows.Forms.Panel();
			this.panel5 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.btnScanLeftEar = new System.Windows.Forms.Button();
			this.btnScanRightEar = new System.Windows.Forms.Button();
			this.tbTo = new System.Windows.Forms.TextBox();
			this.tbFrom = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnUploadOrder = new System.Windows.Forms.Button();
			this.btnShowUploadQueue = new System.Windows.Forms.Button();
			this.btnNewOrder = new System.Windows.Forms.Button();
			this.btnCancelOrder = new System.Windows.Forms.Button();
			this.lSkipLeftEar = new System.Windows.Forms.Label();
			this.lSkipRightEar = new System.Windows.Forms.Label();
			this.tbDetails = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lCompleteOrderForm = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.tbSubject = new System.Windows.Forms.TextBox();
			this.camControl = new Camera_NET.CameraControl();
			this.pbLeftSpin = new System.Windows.Forms.PictureBox();
			this.pbRightSpin = new System.Windows.Forms.PictureBox();
			this.tlRightFrame = new System.Windows.Forms.TableLayoutPanel();
			this.pbCubeLogo = new System.Windows.Forms.PictureBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lStatus = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pbPartnerLogo = new System.Windows.Forms.PictureBox();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.tlMainFrame.SuspendLayout();
			this.tlLeftFrame.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbLeftSpin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRightSpin)).BeginInit();
			this.tlRightFrame.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbCubeLogo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPartnerLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// tlMainFrame
			// 
			this.tlMainFrame.BackColor = System.Drawing.Color.White;
			this.tlMainFrame.ColumnCount = 2;
			this.tlMainFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 520F));
			this.tlMainFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlMainFrame.Controls.Add(this.tlLeftFrame, 0, 0);
			this.tlMainFrame.Controls.Add(this.tlRightFrame, 1, 0);
			this.tlMainFrame.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlMainFrame.Location = new System.Drawing.Point(0, 0);
			this.tlMainFrame.Name = "tlMainFrame";
			this.tlMainFrame.RowCount = 1;
			this.tlMainFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlMainFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 562F));
			this.tlMainFrame.Size = new System.Drawing.Size(1008, 562);
			this.tlMainFrame.TabIndex = 0;
			// 
			// tlLeftFrame
			// 
			this.tlLeftFrame.BackColor = System.Drawing.Color.Transparent;
			this.tlLeftFrame.ColumnCount = 4;
			this.tlLeftFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tlLeftFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
			this.tlLeftFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlLeftFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlLeftFrame.Controls.Add(this.btnCompleteStep1, 1, 9);
			this.tlLeftFrame.Controls.Add(this.panel7, 0, 0);
			this.tlLeftFrame.Controls.Add(this.panel6, 0, 22);
			this.tlLeftFrame.Controls.Add(this.panel5, 0, 19);
			this.tlLeftFrame.Controls.Add(this.panel4, 0, 16);
			this.tlLeftFrame.Controls.Add(this.panel3, 0, 13);
			this.tlLeftFrame.Controls.Add(this.panel2, 0, 10);
			this.tlLeftFrame.Controls.Add(this.label1, 0, 3);
			this.tlLeftFrame.Controls.Add(this.label2, 0, 5);
			this.tlLeftFrame.Controls.Add(this.label3, 0, 6);
			this.tlLeftFrame.Controls.Add(this.label5, 0, 11);
			this.tlLeftFrame.Controls.Add(this.label7, 0, 14);
			this.tlLeftFrame.Controls.Add(this.label9, 0, 17);
			this.tlLeftFrame.Controls.Add(this.label11, 0, 20);
			this.tlLeftFrame.Controls.Add(this.btnScanLeftEar, 1, 12);
			this.tlLeftFrame.Controls.Add(this.btnScanRightEar, 1, 15);
			this.tlLeftFrame.Controls.Add(this.tbTo, 1, 6);
			this.tlLeftFrame.Controls.Add(this.tbFrom, 1, 5);
			this.tlLeftFrame.Controls.Add(this.panel1, 0, 2);
			this.tlLeftFrame.Controls.Add(this.btnUploadOrder, 1, 21);
			this.tlLeftFrame.Controls.Add(this.btnShowUploadQueue, 3, 21);
			this.tlLeftFrame.Controls.Add(this.btnNewOrder, 1, 1);
			this.tlLeftFrame.Controls.Add(this.btnCancelOrder, 3, 1);
			this.tlLeftFrame.Controls.Add(this.lSkipLeftEar, 3, 12);
			this.tlLeftFrame.Controls.Add(this.lSkipRightEar, 3, 15);
			this.tlLeftFrame.Controls.Add(this.tbDetails, 1, 8);
			this.tlLeftFrame.Controls.Add(this.label4, 0, 8);
			this.tlLeftFrame.Controls.Add(this.lCompleteOrderForm, 1, 17);
			this.tlLeftFrame.Controls.Add(this.label6, 0, 7);
			this.tlLeftFrame.Controls.Add(this.tbSubject, 1, 7);
			this.tlLeftFrame.Controls.Add(this.camControl, 2, 1);
			this.tlLeftFrame.Controls.Add(this.pbLeftSpin, 2, 12);
			this.tlLeftFrame.Controls.Add(this.pbRightSpin, 2, 15);
			this.tlLeftFrame.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlLeftFrame.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tlLeftFrame.Location = new System.Drawing.Point(0, 0);
			this.tlLeftFrame.Margin = new System.Windows.Forms.Padding(0);
			this.tlLeftFrame.Name = "tlLeftFrame";
			this.tlLeftFrame.RowCount = 23;
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlLeftFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlLeftFrame.Size = new System.Drawing.Size(520, 562);
			this.tlLeftFrame.TabIndex = 0;
			// 
			// btnCompleteStep1
			// 
			this.btnCompleteStep1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnCompleteStep1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnCompleteStep1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCompleteStep1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCompleteStep1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnCompleteStep1.Location = new System.Drawing.Point(83, 281);
			this.btnCompleteStep1.Name = "btnCompleteStep1";
			this.btnCompleteStep1.Size = new System.Drawing.Size(154, 26);
			this.btnCompleteStep1.TabIndex = 27;
			this.btnCompleteStep1.Text = "Continue";
			this.btnCompleteStep1.UseVisualStyleBackColor = false;
			this.btnCompleteStep1.Click += new System.EventHandler(this.CompleteStep1);
			// 
			// panel7
			// 
			this.panel7.BackColor = System.Drawing.Color.Navy;
			this.tlLeftFrame.SetColumnSpan(this.panel7, 4);
			this.panel7.ForeColor = System.Drawing.Color.Navy;
			this.panel7.Location = new System.Drawing.Point(3, 3);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(514, 2);
			this.panel7.TabIndex = 26;
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(76)))), ((int)(((byte)(110)))));
			this.tlLeftFrame.SetColumnSpan(this.panel6, 4);
			this.panel6.ForeColor = System.Drawing.Color.Navy;
			this.panel6.Location = new System.Drawing.Point(3, 533);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(514, 2);
			this.panel6.TabIndex = 25;
			// 
			// panel5
			// 
			this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(76)))), ((int)(((byte)(110)))));
			this.tlLeftFrame.SetColumnSpan(this.panel5, 4);
			this.panel5.ForeColor = System.Drawing.Color.Navy;
			this.panel5.Location = new System.Drawing.Point(3, 470);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(514, 2);
			this.panel5.TabIndex = 24;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(76)))), ((int)(((byte)(110)))));
			this.tlLeftFrame.SetColumnSpan(this.panel4, 4);
			this.panel4.ForeColor = System.Drawing.Color.Navy;
			this.panel4.Location = new System.Drawing.Point(3, 439);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(514, 2);
			this.panel4.TabIndex = 23;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(76)))), ((int)(((byte)(110)))));
			this.tlLeftFrame.SetColumnSpan(this.panel3, 4);
			this.panel3.ForeColor = System.Drawing.Color.Navy;
			this.panel3.Location = new System.Drawing.Point(3, 376);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(514, 2);
			this.panel3.TabIndex = 22;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(76)))), ((int)(((byte)(110)))));
			this.tlLeftFrame.SetColumnSpan(this.panel2, 4);
			this.panel2.ForeColor = System.Drawing.Color.Navy;
			this.panel2.Location = new System.Drawing.Point(3, 313);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(514, 2);
			this.panel2.TabIndex = 21;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label1.Location = new System.Drawing.Point(3, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Step 1";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label2.Location = new System.Drawing.Point(3, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "*From:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label3.Location = new System.Drawing.Point(3, 99);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "*To:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label5.Location = new System.Drawing.Point(3, 318);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(74, 23);
			this.label5.TabIndex = 6;
			this.label5.Text = "Step 2";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label7.Location = new System.Drawing.Point(3, 381);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(74, 23);
			this.label7.TabIndex = 8;
			this.label7.Text = "Step 3";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label9.Location = new System.Drawing.Point(3, 444);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(74, 23);
			this.label9.TabIndex = 10;
			this.label9.Text = "Step 4";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label11.Location = new System.Drawing.Point(3, 475);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(74, 23);
			this.label11.TabIndex = 12;
			this.label11.Text = "Step 5";
			// 
			// btnScanLeftEar
			// 
			this.btnScanLeftEar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnScanLeftEar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnScanLeftEar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnScanLeftEar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnScanLeftEar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnScanLeftEar.Location = new System.Drawing.Point(83, 344);
			this.btnScanLeftEar.Name = "btnScanLeftEar";
			this.btnScanLeftEar.Size = new System.Drawing.Size(154, 26);
			this.btnScanLeftEar.TabIndex = 16;
			this.btnScanLeftEar.Text = "Scan Left Ear";
			this.btnScanLeftEar.UseVisualStyleBackColor = false;
			this.btnScanLeftEar.Click += new System.EventHandler(this.ScanLeftEar);
			// 
			// btnScanRightEar
			// 
			this.btnScanRightEar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnScanRightEar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnScanRightEar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnScanRightEar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnScanRightEar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnScanRightEar.Location = new System.Drawing.Point(83, 407);
			this.btnScanRightEar.Name = "btnScanRightEar";
			this.btnScanRightEar.Size = new System.Drawing.Size(154, 26);
			this.btnScanRightEar.TabIndex = 17;
			this.btnScanRightEar.Text = "Scan Right Ear";
			this.btnScanRightEar.UseVisualStyleBackColor = false;
			this.btnScanRightEar.Click += new System.EventHandler(this.ScanRightEar);
			// 
			// tbTo
			// 
			this.tbTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tlLeftFrame.SetColumnSpan(this.tbTo, 3);
			this.tbTo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbTo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbTo.Location = new System.Drawing.Point(83, 102);
			this.tbTo.Name = "tbTo";
			this.tbTo.Size = new System.Drawing.Size(434, 22);
			this.tbTo.TabIndex = 2;
			// 
			// tbFrom
			// 
			this.tbFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tlLeftFrame.SetColumnSpan(this.tbFrom, 3);
			this.tbFrom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbFrom.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbFrom.Location = new System.Drawing.Point(83, 74);
			this.tbFrom.Name = "tbFrom";
			this.tbFrom.Size = new System.Drawing.Size(434, 22);
			this.tbFrom.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(76)))), ((int)(((byte)(110)))));
			this.tlLeftFrame.SetColumnSpan(this.panel1, 4);
			this.panel1.ForeColor = System.Drawing.Color.Navy;
			this.panel1.Location = new System.Drawing.Point(3, 43);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(514, 2);
			this.panel1.TabIndex = 20;
			// 
			// btnUploadOrder
			// 
			this.btnUploadOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnUploadOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnUploadOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnUploadOrder.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUploadOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnUploadOrder.Location = new System.Drawing.Point(83, 501);
			this.btnUploadOrder.Name = "btnUploadOrder";
			this.btnUploadOrder.Size = new System.Drawing.Size(154, 26);
			this.btnUploadOrder.TabIndex = 19;
			this.btnUploadOrder.Text = "Upload Order";
			this.btnUploadOrder.UseVisualStyleBackColor = false;
			this.btnUploadOrder.Click += new System.EventHandler(this.UploadOrderButtonClick);
			// 
			// btnShowUploadQueue
			// 
			this.btnShowUploadQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnShowUploadQueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnShowUploadQueue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnShowUploadQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnShowUploadQueue.Location = new System.Drawing.Point(278, 501);
			this.btnShowUploadQueue.Name = "btnShowUploadQueue";
			this.btnShowUploadQueue.Size = new System.Drawing.Size(154, 26);
			this.btnShowUploadQueue.TabIndex = 29;
			this.btnShowUploadQueue.Text = "Show Upload Manager";
			this.btnShowUploadQueue.UseVisualStyleBackColor = false;
			this.btnShowUploadQueue.Click += new System.EventHandler(this.ShowUploadQueueButtonClick);
			// 
			// btnNewOrder
			// 
			this.btnNewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnNewOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnNewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNewOrder.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnNewOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnNewOrder.Location = new System.Drawing.Point(83, 11);
			this.btnNewOrder.Name = "btnNewOrder";
			this.btnNewOrder.Size = new System.Drawing.Size(154, 26);
			this.btnNewOrder.TabIndex = 0;
			this.btnNewOrder.Text = "Start a New Order";
			this.btnNewOrder.UseVisualStyleBackColor = false;
			this.btnNewOrder.Click += new System.EventHandler(this.StartANewOrder);
			// 
			// btnCancelOrder
			// 
			this.btnCancelOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnCancelOrder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCancelOrder.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnCancelOrder.Location = new System.Drawing.Point(278, 11);
			this.btnCancelOrder.Name = "btnCancelOrder";
			this.btnCancelOrder.Size = new System.Drawing.Size(154, 26);
			this.btnCancelOrder.TabIndex = 0;
			this.btnCancelOrder.Text = "Cancel Order";
			this.btnCancelOrder.UseVisualStyleBackColor = false;
			this.btnCancelOrder.Click += new System.EventHandler(this.CancelOrder);
			// 
			// lSkipLeftEar
			// 
			this.lSkipLeftEar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lSkipLeftEar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.lSkipLeftEar.Location = new System.Drawing.Point(278, 341);
			this.lSkipLeftEar.Name = "lSkipLeftEar";
			this.lSkipLeftEar.Size = new System.Drawing.Size(100, 23);
			this.lSkipLeftEar.TabIndex = 7;
			this.lSkipLeftEar.Text = "Skip Left Ear";
			this.lSkipLeftEar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.lSkipLeftEar.Click += new System.EventHandler(this.SkipLeftEar);
			this.lSkipLeftEar.MouseLeave += new System.EventHandler(this.MakeSkipLabelFontNormal);
			this.lSkipLeftEar.MouseHover += new System.EventHandler(this.MakeSkipLabelFontBold);
			// 
			// lSkipRightEar
			// 
			this.lSkipRightEar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lSkipRightEar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.lSkipRightEar.Location = new System.Drawing.Point(278, 404);
			this.lSkipRightEar.Name = "lSkipRightEar";
			this.lSkipRightEar.Size = new System.Drawing.Size(100, 23);
			this.lSkipRightEar.TabIndex = 9;
			this.lSkipRightEar.Text = "Skip Right Ear";
			this.lSkipRightEar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.lSkipRightEar.Click += new System.EventHandler(this.SkipRightEar);
			this.lSkipRightEar.MouseLeave += new System.EventHandler(this.MakeSkipLabelFontNormal);
			this.lSkipRightEar.MouseHover += new System.EventHandler(this.MakeSkipLabelFontBold);
			// 
			// tbDetails
			// 
			this.tbDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tlLeftFrame.SetColumnSpan(this.tbDetails, 3);
			this.tbDetails.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbDetails.Location = new System.Drawing.Point(83, 158);
			this.tbDetails.Multiline = true;
			this.tbDetails.Name = "tbDetails";
			this.tbDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbDetails.Size = new System.Drawing.Size(434, 117);
			this.tbDetails.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label4.Location = new System.Drawing.Point(3, 155);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(74, 23);
			this.label4.TabIndex = 5;
			this.label4.Text = "Details:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lCompleteOrderForm
			// 
			this.lCompleteOrderForm.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lCompleteOrderForm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.lCompleteOrderForm.Location = new System.Drawing.Point(83, 444);
			this.lCompleteOrderForm.Name = "lCompleteOrderForm";
			this.lCompleteOrderForm.Size = new System.Drawing.Size(154, 23);
			this.lCompleteOrderForm.TabIndex = 9;
			this.lCompleteOrderForm.Text = "Complete Order Form";
			this.lCompleteOrderForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label6.Location = new System.Drawing.Point(3, 127);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(74, 20);
			this.label6.TabIndex = 4;
			this.label6.Text = "Subect:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbSubject
			// 
			this.tbSubject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.tlLeftFrame.SetColumnSpan(this.tbSubject, 3);
			this.tbSubject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbSubject.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbSubject.Location = new System.Drawing.Point(83, 130);
			this.tbSubject.Name = "tbSubject";
			this.tbSubject.Size = new System.Drawing.Size(434, 22);
			this.tbSubject.TabIndex = 3;
			// 
			// camControl
			// 
			this.camControl.DirectShowLogFilepath = "";
			this.camControl.Location = new System.Drawing.Point(243, 12);
			this.camControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.camControl.Name = "camControl";
			this.camControl.Size = new System.Drawing.Size(14, 10);
			this.camControl.TabIndex = 30;
			this.camControl.Visible = false;
			// 
			// pbLeftSpin
			// 
			this.pbLeftSpin.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.pbLeftSpin.Location = new System.Drawing.Point(248, 348);
			this.pbLeftSpin.Margin = new System.Windows.Forms.Padding(0);
			this.pbLeftSpin.Name = "pbLeftSpin";
			this.pbLeftSpin.Size = new System.Drawing.Size(18, 18);
			this.pbLeftSpin.TabIndex = 31;
			this.pbLeftSpin.TabStop = false;
			// 
			// pbRightSpin
			// 
			this.pbRightSpin.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.pbRightSpin.Location = new System.Drawing.Point(248, 411);
			this.pbRightSpin.Margin = new System.Windows.Forms.Padding(0);
			this.pbRightSpin.Name = "pbRightSpin";
			this.pbRightSpin.Size = new System.Drawing.Size(18, 18);
			this.pbRightSpin.TabIndex = 32;
			this.pbRightSpin.TabStop = false;
			// 
			// tlRightFrame
			// 
			this.tlRightFrame.BackColor = System.Drawing.Color.Transparent;
			this.tlRightFrame.ColumnCount = 1;
			this.tlRightFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlRightFrame.Controls.Add(this.pbCubeLogo, 0, 0);
			this.tlRightFrame.Controls.Add(this.progressBar, 0, 3);
			this.tlRightFrame.Controls.Add(this.lStatus, 0, 1);
			this.tlRightFrame.Controls.Add(this.pictureBox1, 0, 2);
			this.tlRightFrame.Controls.Add(this.pbPartnerLogo, 0, 4);
			this.tlRightFrame.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlRightFrame.Location = new System.Drawing.Point(520, 0);
			this.tlRightFrame.Margin = new System.Windows.Forms.Padding(0);
			this.tlRightFrame.Name = "tlRightFrame";
			this.tlRightFrame.RowCount = 5;
			this.tlRightFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
			this.tlRightFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tlRightFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlRightFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tlRightFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tlRightFrame.Size = new System.Drawing.Size(488, 562);
			this.tlRightFrame.TabIndex = 1;
			// 
			// pbCubeLogo
			// 
			this.pbCubeLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.pbCubeLogo.BackColor = System.Drawing.Color.Transparent;
			this.pbCubeLogo.Image = global::Me.ThreeDWares.SugarCube.UIIMages.SugarCubeLogo;
			this.pbCubeLogo.InitialImage = null;
			this.pbCubeLogo.Location = new System.Drawing.Point(0, 0);
			this.pbCubeLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pbCubeLogo.Name = "pbCubeLogo";
			this.pbCubeLogo.Size = new System.Drawing.Size(488, 140);
			this.pbCubeLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbCubeLogo.TabIndex = 0;
			this.pbCubeLogo.TabStop = false;
			// 
			// progressBar
			// 
			this.progressBar.BackColor = System.Drawing.Color.White;
			this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.progressBar.Location = new System.Drawing.Point(3, 385);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(482, 54);
			this.progressBar.Step = 1;
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 3;
			// 
			// lStatus
			// 
			this.lStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.lStatus.Location = new System.Drawing.Point(3, 140);
			this.lStatus.Name = "lStatus";
			this.lStatus.Size = new System.Drawing.Size(473, 23);
			this.lStatus.TabIndex = 2;
			this.lStatus.Text = "Status";
			this.lStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new System.Drawing.Point(0, 165);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(488, 217);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// pbPartnerLogo
			// 
			this.pbPartnerLogo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.pbPartnerLogo.BackColor = System.Drawing.Color.Transparent;
			this.pbPartnerLogo.InitialImage = null;
			this.pbPartnerLogo.Location = new System.Drawing.Point(0, 442);
			this.pbPartnerLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pbPartnerLogo.Name = "pbPartnerLogo";
			this.pbPartnerLogo.Size = new System.Drawing.Size(488, 120);
			this.pbPartnerLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbPartnerLogo.TabIndex = 1;
			this.pbPartnerLogo.TabStop = false;
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(1008, 562);
			this.Controls.Add(this.tlMainFrame);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(1024, 600);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SugarCube Manager [build {0}]";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
			this.tlMainFrame.ResumeLayout(false);
			this.tlLeftFrame.ResumeLayout(false);
			this.tlLeftFrame.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbLeftSpin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRightSpin)).EndInit();
			this.tlRightFrame.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbCubeLogo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPartnerLogo)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.PictureBox pbRightSpin;
		private System.Windows.Forms.PictureBox pbLeftSpin;
		private Camera_NET.CameraControl camControl;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnShowUploadQueue;
		private System.Windows.Forms.TextBox tbSubject;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lStatus;
		private System.Windows.Forms.Label lCompleteOrderForm;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button btnCompleteStep1;
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.PictureBox pbPartnerLogo;
		private System.Windows.Forms.PictureBox pbCubeLogo;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.TableLayoutPanel tlRightFrame;
		private System.Windows.Forms.TextBox tbFrom;
		private System.Windows.Forms.TextBox tbTo;
		private System.Windows.Forms.TextBox tbDetails;
		private System.Windows.Forms.Button btnUploadOrder;
		private System.Windows.Forms.Button btnScanRightEar;
		private System.Windows.Forms.Button btnScanLeftEar;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lSkipRightEar;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lSkipLeftEar;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancelOrder;
		private System.Windows.Forms.Button btnNewOrder;
		private System.Windows.Forms.TableLayoutPanel tlLeftFrame;
		private System.Windows.Forms.TableLayoutPanel tlMainFrame;
	}
}
