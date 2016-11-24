/* SugarCube Host Software - Custom Installer Builder
 * Copyright (c) 2015 Chad Ullman
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
			this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbOrderForm = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.tbPass1 = new System.Windows.Forms.TextBox();
			this.tbPass2 = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.tbImage = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.pbImagePreview = new System.Windows.Forms.PictureBox();
			this.btnSelectImage = new System.Windows.Forms.Button();
			this.btnSelectOrderForm = new System.Windows.Forms.Button();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbInstallerPath = new System.Windows.Forms.TextBox();
			this.tbSaveLocation = new System.Windows.Forms.TextBox();
			this.btnSelectInstaller = new System.Windows.Forms.Button();
			this.btnSetSaveLocation = new System.Windows.Forms.Button();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.dlgSelectSaveFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.mainLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbImagePreview)).BeginInit();
			this.SuspendLayout();
			// 
			// mainLayout
			// 
			this.mainLayout.ColumnCount = 4;
			this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.57143F));
			this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.42857F));
			this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 353F));
			this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.mainLayout.Controls.Add(this.label1, 0, 0);
			this.mainLayout.Controls.Add(this.label3, 0, 3);
			this.mainLayout.Controls.Add(this.label2, 0, 2);
			this.mainLayout.Controls.Add(this.tbOrderForm, 1, 3);
			this.mainLayout.Controls.Add(this.label4, 0, 5);
			this.mainLayout.Controls.Add(this.label5, 0, 6);
			this.mainLayout.Controls.Add(this.label6, 0, 7);
			this.mainLayout.Controls.Add(this.tbPass1, 1, 6);
			this.mainLayout.Controls.Add(this.tbPass2, 1, 7);
			this.mainLayout.Controls.Add(this.label7, 0, 9);
			this.mainLayout.Controls.Add(this.label8, 0, 10);
			this.mainLayout.Controls.Add(this.tbImage, 1, 10);
			this.mainLayout.Controls.Add(this.label9, 0, 11);
			this.mainLayout.Controls.Add(this.pbImagePreview, 1, 11);
			this.mainLayout.Controls.Add(this.btnSelectImage, 3, 10);
			this.mainLayout.Controls.Add(this.btnSelectOrderForm, 3, 3);
			this.mainLayout.Controls.Add(this.btnGenerate, 1, 15);
			this.mainLayout.Controls.Add(this.label10, 0, 13);
			this.mainLayout.Controls.Add(this.label11, 0, 14);
			this.mainLayout.Controls.Add(this.label12, 0, 12);
			this.mainLayout.Controls.Add(this.btnCancel, 2, 15);
			this.mainLayout.Controls.Add(this.tbInstallerPath, 1, 13);
			this.mainLayout.Controls.Add(this.tbSaveLocation, 1, 14);
			this.mainLayout.Controls.Add(this.btnSelectInstaller, 3, 13);
			this.mainLayout.Controls.Add(this.btnSetSaveLocation, 3, 14);
			this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainLayout.Location = new System.Drawing.Point(0, 0);
			this.mainLayout.Margin = new System.Windows.Forms.Padding(0);
			this.mainLayout.Name = "mainLayout";
			this.mainLayout.RowCount = 16;
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainLayout.Size = new System.Drawing.Size(748, 563);
			this.mainLayout.TabIndex = 0;
			// 
			// label1
			// 
			this.mainLayout.SetColumnSpan(this.label1, 4);
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(742, 62);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(3, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(129, 29);
			this.label3.TabIndex = 2;
			this.label3.Text = "Location of order form:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.mainLayout.SetColumnSpan(this.label2, 4);
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 62);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(742, 42);
			this.label2.TabIndex = 1;
			this.label2.Text = "You can add an order form for your customers to complete as part of each scan.  T" +
			"he order form must be a fillable PDF.  If you add an order form, you must set a " +
			"password as well.";
			// 
			// tbOrderForm
			// 
			this.tbOrderForm.BackColor = System.Drawing.Color.WhiteSmoke;
			this.mainLayout.SetColumnSpan(this.tbOrderForm, 2);
			this.tbOrderForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbOrderForm.Location = new System.Drawing.Point(138, 107);
			this.tbOrderForm.Name = "tbOrderForm";
			this.tbOrderForm.ReadOnly = true;
			this.tbOrderForm.Size = new System.Drawing.Size(546, 20);
			this.tbOrderForm.TabIndex = 1;
			this.tbOrderForm.TextChanged += new System.EventHandler(this.TbOrderFormTextChanged);
			// 
			// label4
			// 
			this.mainLayout.SetColumnSpan(this.label4, 4);
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(3, 133);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(742, 40);
			this.label4.TabIndex = 3;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// label5
			// 
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(3, 173);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(129, 26);
			this.label5.TabIndex = 6;
			this.label5.Text = "Encryption Password:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(3, 199);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(129, 26);
			this.label6.TabIndex = 7;
			this.label6.Text = "Re-enter Password:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbPass1
			// 
			this.mainLayout.SetColumnSpan(this.tbPass1, 2);
			this.tbPass1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbPass1.Enabled = false;
			this.tbPass1.Location = new System.Drawing.Point(138, 176);
			this.tbPass1.Name = "tbPass1";
			this.tbPass1.Size = new System.Drawing.Size(546, 20);
			this.tbPass1.TabIndex = 3;
			// 
			// tbPass2
			// 
			this.mainLayout.SetColumnSpan(this.tbPass2, 2);
			this.tbPass2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbPass2.Enabled = false;
			this.tbPass2.Location = new System.Drawing.Point(138, 202);
			this.tbPass2.Name = "tbPass2";
			this.tbPass2.Size = new System.Drawing.Size(546, 20);
			this.tbPass2.TabIndex = 4;
			// 
			// label7
			// 
			this.mainLayout.SetColumnSpan(this.label7, 4);
			this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(3, 225);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(742, 46);
			this.label7.TabIndex = 10;
			this.label7.Text = "You can add your own cutom branded image to the SugarCube Manager UI.  This image" +
			" must be exactly 488 pixels wide and 120 pixels high.  It will appear in the low" +
			"er right quadrant of the Manager UI.";
			// 
			// label8
			// 
			this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(3, 271);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(129, 29);
			this.label8.TabIndex = 11;
			this.label8.Text = "Select Image:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbImage
			// 
			this.tbImage.BackColor = System.Drawing.Color.WhiteSmoke;
			this.mainLayout.SetColumnSpan(this.tbImage, 2);
			this.tbImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbImage.Location = new System.Drawing.Point(138, 274);
			this.tbImage.Name = "tbImage";
			this.tbImage.ReadOnly = true;
			this.tbImage.Size = new System.Drawing.Size(546, 20);
			this.tbImage.TabIndex = 5;
			// 
			// label9
			// 
			this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(3, 300);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(129, 126);
			this.label9.TabIndex = 14;
			this.label9.Text = "Image Preview:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pbImagePreview
			// 
			this.pbImagePreview.BackColor = System.Drawing.Color.White;
			this.mainLayout.SetColumnSpan(this.pbImagePreview, 3);
			this.pbImagePreview.InitialImage = global::Me.ThreeDWares.SugarCube.InstallationComponents.BlankBrandImage;
			this.pbImagePreview.Location = new System.Drawing.Point(138, 303);
			this.pbImagePreview.Name = "pbImagePreview";
			this.pbImagePreview.Size = new System.Drawing.Size(488, 120);
			this.pbImagePreview.TabIndex = 15;
			this.pbImagePreview.TabStop = false;
			// 
			// btnSelectImage
			// 
			this.btnSelectImage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSelectImage.Location = new System.Drawing.Point(690, 274);
			this.btnSelectImage.Name = "btnSelectImage";
			this.btnSelectImage.Size = new System.Drawing.Size(29, 23);
			this.btnSelectImage.TabIndex = 6;
			this.btnSelectImage.Text = "...";
			this.btnSelectImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSelectImage.UseVisualStyleBackColor = true;
			this.btnSelectImage.Click += new System.EventHandler(this.BtnSelectImageClick);
			// 
			// btnSelectOrderForm
			// 
			this.btnSelectOrderForm.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSelectOrderForm.Location = new System.Drawing.Point(690, 107);
			this.btnSelectOrderForm.Name = "btnSelectOrderForm";
			this.btnSelectOrderForm.Size = new System.Drawing.Size(29, 23);
			this.btnSelectOrderForm.TabIndex = 2;
			this.btnSelectOrderForm.Text = "...";
			this.btnSelectOrderForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSelectOrderForm.UseVisualStyleBackColor = true;
			this.btnSelectOrderForm.Click += new System.EventHandler(this.BtnSelectOrderFormClick);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGenerate.Location = new System.Drawing.Point(138, 533);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(179, 23);
			this.btnGenerate.TabIndex = 7;
			this.btnGenerate.Text = "Generate Custom Installer";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.BtnGenerateClick);
			// 
			// label10
			// 
			this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(3, 472);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(129, 29);
			this.label10.TabIndex = 16;
			this.label10.Text = "Core Installation Package:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(3, 501);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(129, 29);
			this.label11.TabIndex = 17;
			this.label11.Text = "Save Location:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.mainLayout.SetColumnSpan(this.label12, 4);
			this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(3, 426);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(742, 46);
			this.label12.TabIndex = 18;
			this.label12.Text = "Finally, specify the location of the core SugarCube Manager installation package " +
			"and the folder on disk where you want to save the generated installer to";
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(337, 533);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(134, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel/Exit";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
			// 
			// tbInstallerPath
			// 
			this.tbInstallerPath.BackColor = System.Drawing.Color.WhiteSmoke;
			this.mainLayout.SetColumnSpan(this.tbInstallerPath, 2);
			this.tbInstallerPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbInstallerPath.Location = new System.Drawing.Point(138, 475);
			this.tbInstallerPath.Name = "tbInstallerPath";
			this.tbInstallerPath.ReadOnly = true;
			this.tbInstallerPath.Size = new System.Drawing.Size(546, 20);
			this.tbInstallerPath.TabIndex = 19;
			// 
			// tbSaveLocation
			// 
			this.tbSaveLocation.BackColor = System.Drawing.Color.WhiteSmoke;
			this.mainLayout.SetColumnSpan(this.tbSaveLocation, 2);
			this.tbSaveLocation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbSaveLocation.Location = new System.Drawing.Point(138, 504);
			this.tbSaveLocation.Name = "tbSaveLocation";
			this.tbSaveLocation.ReadOnly = true;
			this.tbSaveLocation.Size = new System.Drawing.Size(546, 20);
			this.tbSaveLocation.TabIndex = 20;
			// 
			// btnSelectInstaller
			// 
			this.btnSelectInstaller.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSelectInstaller.Location = new System.Drawing.Point(690, 475);
			this.btnSelectInstaller.Name = "btnSelectInstaller";
			this.btnSelectInstaller.Size = new System.Drawing.Size(29, 23);
			this.btnSelectInstaller.TabIndex = 21;
			this.btnSelectInstaller.Text = "...";
			this.btnSelectInstaller.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSelectInstaller.UseVisualStyleBackColor = true;
			this.btnSelectInstaller.Click += new System.EventHandler(this.BtnSelectInstallerClick);
			// 
			// btnSetSaveLocation
			// 
			this.btnSetSaveLocation.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetSaveLocation.Location = new System.Drawing.Point(690, 504);
			this.btnSetSaveLocation.Name = "btnSetSaveLocation";
			this.btnSetSaveLocation.Size = new System.Drawing.Size(29, 23);
			this.btnSetSaveLocation.TabIndex = 22;
			this.btnSetSaveLocation.Text = "...";
			this.btnSetSaveLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSetSaveLocation.UseVisualStyleBackColor = true;
			this.btnSetSaveLocation.Click += new System.EventHandler(this.BtnSetSaveLocationClick);
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.FileName = "openFileDialog1";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(748, 563);
			this.Controls.Add(this.mainLayout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SugarCube Manager Custom Installer Builder";
			this.mainLayout.ResumeLayout(false);
			this.mainLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbImagePreview)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnSetSaveLocation;
		private System.Windows.Forms.Button btnSelectInstaller;
		private System.Windows.Forms.TextBox tbSaveLocation;
		private System.Windows.Forms.TextBox tbInstallerPath;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.FolderBrowserDialog dlgSelectSaveFolder;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.PictureBox pbImagePreview;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox tbImage;
		private System.Windows.Forms.Button btnSelectImage;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox tbPass2;
		private System.Windows.Forms.TextBox tbPass1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSelectOrderForm;
		private System.Windows.Forms.TextBox tbOrderForm;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel mainLayout;
	}
}
