/* SugarCube Host Software - Order Form Decrypter
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
			this.label1 = new System.Windows.Forms.Label();
			this.tbOrderForm = new System.Windows.Forms.TextBox();
			this.btnChooseFile = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.btnImportPassword = new System.Windows.Forms.Button();
			this.btnDecrypt = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(-3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(143, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Encrypted Order Form:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// tbOrderForm
			// 
			this.tbOrderForm.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbOrderForm.Location = new System.Drawing.Point(146, 12);
			this.tbOrderForm.Name = "tbOrderForm";
			this.tbOrderForm.Size = new System.Drawing.Size(303, 22);
			this.tbOrderForm.TabIndex = 1;
			// 
			// btnChooseFile
			// 
			this.btnChooseFile.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnChooseFile.Location = new System.Drawing.Point(455, 12);
			this.btnChooseFile.Name = "btnChooseFile";
			this.btnChooseFile.Size = new System.Drawing.Size(148, 23);
			this.btnChooseFile.TabIndex = 2;
			this.btnChooseFile.Text = "Choose File";
			this.btnChooseFile.UseVisualStyleBackColor = true;
			this.btnChooseFile.Click += new System.EventHandler(this.BtnChooseFileClick);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(-3, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(143, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Password:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// tbPassword
			// 
			this.tbPassword.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbPassword.Location = new System.Drawing.Point(146, 54);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.Size = new System.Drawing.Size(303, 22);
			this.tbPassword.TabIndex = 4;
			// 
			// btnImportPassword
			// 
			this.btnImportPassword.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnImportPassword.Location = new System.Drawing.Point(455, 54);
			this.btnImportPassword.Name = "btnImportPassword";
			this.btnImportPassword.Size = new System.Drawing.Size(148, 23);
			this.btnImportPassword.TabIndex = 5;
			this.btnImportPassword.Text = "Import Password";
			this.btnImportPassword.UseVisualStyleBackColor = true;
			this.btnImportPassword.Click += new System.EventHandler(this.BtnImportPasswordClick);
			// 
			// btnDecrypt
			// 
			this.btnDecrypt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDecrypt.Location = new System.Drawing.Point(146, 96);
			this.btnDecrypt.Name = "btnDecrypt";
			this.btnDecrypt.Size = new System.Drawing.Size(157, 23);
			this.btnDecrypt.TabIndex = 6;
			this.btnDecrypt.Text = "Decrypt Order Form";
			this.btnDecrypt.UseVisualStyleBackColor = true;
			this.btnDecrypt.Click += new System.EventHandler(this.BtnDecryptClick);
			// 
			// btnExit
			// 
			this.btnExit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExit.Location = new System.Drawing.Point(332, 96);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(117, 23);
			this.btnExit.TabIndex = 7;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Encrypted Orders|*.head.hcrypt";
			this.openFileDialog.Title = "Select Encrypted Order Form";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(615, 136);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnDecrypt);
			this.Controls.Add(this.btnImportPassword);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnChooseFile);
			this.Controls.Add(this.tbOrderForm);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "3dWares SugarCube Order Form Decrypter";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnDecrypt;
		private System.Windows.Forms.Button btnImportPassword;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnChooseFile;
		private System.Windows.Forms.TextBox tbOrderForm;
		private System.Windows.Forms.Label label1;
	}
}
