/* SugarCube Host Software - SugarCube Uploader
 * Copyright (c) 2014-2015 Chad Ullman
 */

namespace Me.ThreeDWares.SugarCube
{
	partial class UploadForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadForm));
			this.tlMainLayout = new System.Windows.Forms.TableLayoutPanel();
			this.lHeader = new System.Windows.Forms.Label();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.tlButtonLayout = new System.Windows.Forms.TableLayoutPanel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbLog = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.queueGrid = new System.Windows.Forms.DataGridView();
			this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.created = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.started = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.uploaded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.triggered = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.uploaddir = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.tlMainLayout.SuspendLayout();
			this.tlButtonLayout.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.queueGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// tlMainLayout
			// 
			this.tlMainLayout.ColumnCount = 1;
			this.tlMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlMainLayout.Controls.Add(this.lHeader, 0, 0);
			this.tlMainLayout.Controls.Add(this.pbProgress, 0, 1);
			this.tlMainLayout.Controls.Add(this.tlButtonLayout, 0, 4);
			this.tlMainLayout.Controls.Add(this.tbLog, 0, 3);
			this.tlMainLayout.Controls.Add(this.groupBox1, 0, 2);
			this.tlMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlMainLayout.Location = new System.Drawing.Point(0, 0);
			this.tlMainLayout.Name = "tlMainLayout";
			this.tlMainLayout.RowCount = 5;
			this.tlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 185F));
			this.tlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tlMainLayout.Size = new System.Drawing.Size(964, 642);
			this.tlMainLayout.TabIndex = 0;
			// 
			// lHeader
			// 
			this.lHeader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lHeader.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.lHeader.Location = new System.Drawing.Point(3, 0);
			this.lHeader.Name = "lHeader";
			this.lHeader.Size = new System.Drawing.Size(958, 30);
			this.lHeader.TabIndex = 0;
			this.lHeader.Text = "Upload Manager";
			this.lHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pbProgress
			// 
			this.pbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbProgress.Location = new System.Drawing.Point(3, 33);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(958, 24);
			this.pbProgress.TabIndex = 1;
			// 
			// tlButtonLayout
			// 
			this.tlButtonLayout.ColumnCount = 2;
			this.tlButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlButtonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlButtonLayout.Controls.Add(this.btnOK, 0, 0);
			this.tlButtonLayout.Controls.Add(this.btnCancel, 1, 0);
			this.tlButtonLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlButtonLayout.Location = new System.Drawing.Point(3, 595);
			this.tlButtonLayout.Name = "tlButtonLayout";
			this.tlButtonLayout.RowCount = 1;
			this.tlButtonLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlButtonLayout.Size = new System.Drawing.Size(958, 44);
			this.tlButtonLayout.TabIndex = 3;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnOK.Location = new System.Drawing.Point(149, 7);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(180, 30);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "Minimize To Tray";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnCancel.Location = new System.Drawing.Point(628, 7);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(180, 30);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Exit Upload Manager";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
			// 
			// tbLog
			// 
			this.tbLog.BackColor = System.Drawing.Color.White;
			this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbLog.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbLog.ForeColor = System.Drawing.Color.Navy;
			this.tbLog.Location = new System.Drawing.Point(3, 248);
			this.tbLog.Multiline = true;
			this.tbLog.Name = "tbLog";
			this.tbLog.ReadOnly = true;
			this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbLog.Size = new System.Drawing.Size(958, 341);
			this.tbLog.TabIndex = 4;
			this.tbLog.Text = "Log Messages";
			this.tbLog.WordWrap = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.queueGrid);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.groupBox1.Location = new System.Drawing.Point(3, 63);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(958, 179);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Upload Queue";
			// 
			// queueGrid
			// 
			this.queueGrid.AllowUserToAddRows = false;
			this.queueGrid.AllowUserToDeleteRows = false;
			this.queueGrid.AutoGenerateColumns = false;
			this.queueGrid.BackgroundColor = System.Drawing.Color.White;
			this.queueGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.queueGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.id,
									this.created,
									this.started,
									this.uploaded,
									this.triggered,
									this.uploaddir});
			this.queueGrid.DataSource = this.bindingSource;
			this.queueGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.queueGrid.Location = new System.Drawing.Point(3, 16);
			this.queueGrid.Name = "queueGrid";
			this.queueGrid.ReadOnly = true;
			this.queueGrid.Size = new System.Drawing.Size(952, 160);
			this.queueGrid.TabIndex = 0;
			this.queueGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.QueueGridCellFormatting);
			// 
			// id
			// 
			this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.id.HeaderText = "Scan ID";
			this.id.Name = "id";
			this.id.ReadOnly = true;
			this.id.Width = 62;
			// 
			// created
			// 
			this.created.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.created.HeaderText = "Scanned On";
			this.created.Name = "created";
			this.created.ReadOnly = true;
			this.created.Width = 80;
			// 
			// started
			// 
			this.started.HeaderText = "Upload Started";
			this.started.Name = "started";
			this.started.ReadOnly = true;
			// 
			// uploaded
			// 
			this.uploaded.HeaderText = "Upload Completed";
			this.uploaded.Name = "uploaded";
			this.uploaded.ReadOnly = true;
			// 
			// triggered
			// 
			this.triggered.HeaderText = "Processing Triggered";
			this.triggered.Name = "triggered";
			this.triggered.ReadOnly = true;
			// 
			// uploaddir
			// 
			this.uploaddir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.uploaddir.HeaderText = "Notes";
			this.uploaddir.Name = "uploaddir";
			this.uploaddir.ReadOnly = true;
			this.uploaddir.Width = 58;
			// 
			// UploadForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(964, 642);
			this.ControlBox = false;
			this.Controls.Add(this.tlMainLayout);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UploadForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SugarCube Upload Manager [build {0}]";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.UploadFormLoad);
			this.tlMainLayout.ResumeLayout(false);
			this.tlMainLayout.PerformLayout();
			this.tlButtonLayout.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.queueGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.DataGridViewTextBoxColumn uploaddir;
		private System.Windows.Forms.DataGridViewCheckBoxColumn triggered;
		private System.Windows.Forms.DataGridViewCheckBoxColumn uploaded;
		private System.Windows.Forms.DataGridViewCheckBoxColumn started;
		private System.Windows.Forms.DataGridViewTextBoxColumn created;
		private System.Windows.Forms.DataGridViewTextBoxColumn id;
		private System.Windows.Forms.DataGridView queueGrid;
		private System.Windows.Forms.BindingSource bindingSource;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox tbLog;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TableLayoutPanel tlButtonLayout;
		private System.Windows.Forms.ProgressBar pbProgress;
		private System.Windows.Forms.Label lHeader;
		private System.Windows.Forms.TableLayoutPanel tlMainLayout;
	}
}
