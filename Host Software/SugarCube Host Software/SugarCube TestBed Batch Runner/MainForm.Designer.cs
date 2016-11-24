/* SugarCube Host Software - Test Bed Batch Runner
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
			this.camControl = new Camera_NET.CameraControl();
			this.buttonLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.udIterations = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.udDelay = new System.Windows.Forms.NumericUpDown();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbLog = new System.Windows.Forms.RichTextBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.buttonLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udIterations)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDelay)).BeginInit();
			this.SuspendLayout();
			// 
			// camControl
			// 
			this.camControl.BackColor = System.Drawing.Color.Black;
			this.camControl.DirectShowLogFilepath = "";
			this.camControl.Location = new System.Drawing.Point(3, 12);
			this.camControl.Name = "camControl";
			this.camControl.Size = new System.Drawing.Size(234, 154);
			this.camControl.TabIndex = 0;
			// 
			// buttonLayout
			// 
			this.buttonLayout.Controls.Add(this.label1);
			this.buttonLayout.Controls.Add(this.udIterations);
			this.buttonLayout.Controls.Add(this.label2);
			this.buttonLayout.Controls.Add(this.udDelay);
			this.buttonLayout.Controls.Add(this.btnStart);
			this.buttonLayout.Controls.Add(this.btnCancel);
			this.buttonLayout.Location = new System.Drawing.Point(243, 12);
			this.buttonLayout.Name = "buttonLayout";
			this.buttonLayout.Size = new System.Drawing.Size(634, 84);
			this.buttonLayout.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Number of Iterations:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// udIterations
			// 
			this.buttonLayout.SetFlowBreak(this.udIterations, true);
			this.udIterations.Location = new System.Drawing.Point(257, 3);
			this.udIterations.Maximum = new decimal(new int[] {
									9999,
									0,
									0,
									0});
			this.udIterations.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.udIterations.Name = "udIterations";
			this.udIterations.Size = new System.Drawing.Size(61, 20);
			this.udIterations.TabIndex = 1;
			this.udIterations.ThousandsSeparator = true;
			this.udIterations.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(248, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Delay Between Iterations (minutes):";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// udDelay
			// 
			this.buttonLayout.SetFlowBreak(this.udDelay, true);
			this.udDelay.Location = new System.Drawing.Point(257, 29);
			this.udDelay.Maximum = new decimal(new int[] {
									60,
									0,
									0,
									0});
			this.udDelay.Name = "udDelay";
			this.udDelay.Size = new System.Drawing.Size(61, 20);
			this.udDelay.TabIndex = 3;
			// 
			// btnStart
			// 
			this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStart.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnStart.Location = new System.Drawing.Point(3, 55);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = false;
			this.btnStart.Click += new System.EventHandler(this.BtnStartClick);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(224)))), ((int)(((byte)(246)))));
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.btnCancel.Location = new System.Drawing.Point(84, 55);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
			// 
			// tbLog
			// 
			this.tbLog.BackColor = System.Drawing.Color.White;
			this.tbLog.Location = new System.Drawing.Point(3, 172);
			this.tbLog.Name = "tbLog";
			this.tbLog.ReadOnly = true;
			this.tbLog.Size = new System.Drawing.Size(874, 517);
			this.tbLog.TabIndex = 2;
			this.tbLog.Text = "";
			// 
			// progressBar
			// 
			this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.progressBar.Location = new System.Drawing.Point(243, 113);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(634, 23);
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 3;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(889, 691);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.tbLog);
			this.Controls.Add(this.buttonLayout);
			this.Controls.Add(this.camControl);
			this.Name = "MainForm";
			this.Text = "SugarCube TestBed Batch Runner [build {0}]";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.buttonLayout.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udIterations)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDelay)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.RichTextBox tbLog;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.NumericUpDown udDelay;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown udIterations;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FlowLayoutPanel buttonLayout;
		private Camera_NET.CameraControl camControl;
	}
}
