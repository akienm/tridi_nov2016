/* SugarCube Host Software - Test Bed
 * Copyright (c) 2014-2015 Chad Ullman
 */

namespace Me.ThreeDWares.SugarCube
{
	partial class ModellingOptionsDialog
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.tbCropRect = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbTemplate3dp = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.cbDoImageCropping = new System.Windows.Forms.CheckBox();
			this.cbDoModelCropping = new System.Windows.Forms.CheckBox();
			this.cbDoModelScalling = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbBoundBox = new System.Windows.Forms.TextBox();
			this.numMeshLevel = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMeshLevel)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.78723F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.21277F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tbCropRect, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.tbTemplate3dp, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.btnOK, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.cbDoImageCropping, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbDoModelCropping, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.cbDoModelScalling, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.tbBoundBox, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.numMeshLevel, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 9;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(611, 273);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(175, 28);
			this.label1.TabIndex = 1;
			this.label1.Text = "Image Cropping Rectangle:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbCropRect
			// 
			this.tbCropRect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbCropRect.Location = new System.Drawing.Point(184, 31);
			this.tbCropRect.Name = "tbCropRect";
			this.tbCropRect.Size = new System.Drawing.Size(424, 22);
			this.tbCropRect.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(3, 168);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(175, 28);
			this.label4.TabIndex = 9;
			this.label4.Text = "Template 3DP:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbTemplate3dp
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.tbTemplate3dp, 2);
			this.tbTemplate3dp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbTemplate3dp.Location = new System.Drawing.Point(3, 199);
			this.tbTemplate3dp.Name = "tbTemplate3dp";
			this.tbTemplate3dp.Size = new System.Drawing.Size(605, 20);
			this.tbTemplate3dp.TabIndex = 10;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.tableLayoutPanel1.SetColumnSpan(this.btnOK, 2);
			this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.Location = new System.Drawing.Point(268, 227);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 42);
			this.btnOK.TabIndex = 11;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
			// 
			// cbDoImageCropping
			// 
			this.cbDoImageCropping.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.tableLayoutPanel1.SetColumnSpan(this.cbDoImageCropping, 2);
			this.cbDoImageCropping.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbDoImageCropping.Location = new System.Drawing.Point(3, 3);
			this.cbDoImageCropping.Name = "cbDoImageCropping";
			this.cbDoImageCropping.Size = new System.Drawing.Size(192, 22);
			this.cbDoImageCropping.TabIndex = 0;
			this.cbDoImageCropping.Text = "Do Image Cropping:";
			this.cbDoImageCropping.UseVisualStyleBackColor = true;
			this.cbDoImageCropping.CheckedChanged += new System.EventHandler(this.CbDoImageCroppingCheckedChanged);
			// 
			// cbDoModelCropping
			// 
			this.cbDoModelCropping.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.tableLayoutPanel1.SetColumnSpan(this.cbDoModelCropping, 2);
			this.cbDoModelCropping.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbDoModelCropping.Location = new System.Drawing.Point(3, 87);
			this.cbDoModelCropping.Name = "cbDoModelCropping";
			this.cbDoModelCropping.Size = new System.Drawing.Size(192, 22);
			this.cbDoModelCropping.TabIndex = 3;
			this.cbDoModelCropping.Text = "Do Model Cropping:";
			this.cbDoModelCropping.UseVisualStyleBackColor = true;
			this.cbDoModelCropping.CheckedChanged += new System.EventHandler(this.CbDoModelCroppingCheckedChanged);
			// 
			// cbDoModelScalling
			// 
			this.cbDoModelScalling.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.tableLayoutPanel1.SetColumnSpan(this.cbDoModelScalling, 2);
			this.cbDoModelScalling.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbDoModelScalling.Location = new System.Drawing.Point(3, 59);
			this.cbDoModelScalling.Name = "cbDoModelScalling";
			this.cbDoModelScalling.Size = new System.Drawing.Size(192, 22);
			this.cbDoModelScalling.TabIndex = 4;
			this.cbDoModelScalling.Text = "Do Model Scaling:";
			this.cbDoModelScalling.UseVisualStyleBackColor = true;
			this.cbDoModelScalling.CheckedChanged += new System.EventHandler(this.CbDoModelScallingCheckedChanged);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(3, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(175, 28);
			this.label3.TabIndex = 7;
			this.label3.Text = "Bounding  Box:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbBoundBox
			// 
			this.tbBoundBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbBoundBox.Location = new System.Drawing.Point(184, 115);
			this.tbBoundBox.Name = "tbBoundBox";
			this.tbBoundBox.Size = new System.Drawing.Size(424, 22);
			this.tbBoundBox.TabIndex = 8;
			// 
			// numMeshLevel
			// 
			this.numMeshLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numMeshLevel.Location = new System.Drawing.Point(184, 143);
			this.numMeshLevel.Maximum = new decimal(new int[] {
									9,
									0,
									0,
									0});
			this.numMeshLevel.Minimum = new decimal(new int[] {
									7,
									0,
									0,
									0});
			this.numMeshLevel.Name = "numMeshLevel";
			this.numMeshLevel.Size = new System.Drawing.Size(38, 22);
			this.numMeshLevel.TabIndex = 6;
			this.numMeshLevel.Value = new decimal(new int[] {
									7,
									0,
									0,
									0});
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 140);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(175, 28);
			this.label2.TabIndex = 5;
			this.label2.Text = "Mesh Level:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ModellingOptionsDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(611, 273);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ModellingOptionsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Set Modelling Options";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMeshLevel)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox tbTemplate3dp;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbBoundBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numMeshLevel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox cbDoModelScalling;
		private System.Windows.Forms.CheckBox cbDoModelCropping;
		private System.Windows.Forms.TextBox tbCropRect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox cbDoImageCropping;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
