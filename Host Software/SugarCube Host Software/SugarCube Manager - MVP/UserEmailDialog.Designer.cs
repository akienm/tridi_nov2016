/* SugarCube Host Software - Manager MVP
 * Copyright (c) 2014-2015 Chad Ullman
 */

namespace Me.ThreeDWares.SugarCube
{
	partial class UserEmailDialog
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
			this.lPass = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbEmail1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbEmail2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lError = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lPass
			// 
			this.lPass.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.lPass.Location = new System.Drawing.Point(4, 40);
			this.lPass.Name = "lPass";
			this.lPass.Size = new System.Drawing.Size(100, 23);
			this.lPass.TabIndex = 0;
			this.lPass.Text = "email address:";
			this.lPass.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.Location = new System.Drawing.Point(100, 94);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(219, 94);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
			// 
			// tbEmail1
			// 
			this.tbEmail1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbEmail1.Location = new System.Drawing.Point(100, 43);
			this.tbEmail1.Name = "tbEmail1";
			this.tbEmail1.Size = new System.Drawing.Size(269, 22);
			this.tbEmail1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.label1.Location = new System.Drawing.Point(0, 1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(378, 39);
			this.label1.TabIndex = 4;
			this.label1.Text = "Please enter your email address.  This will be used as your unique key when proce" +
			"ssing your scans.";
			// 
			// tbEmail2
			// 
			this.tbEmail2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbEmail2.Location = new System.Drawing.Point(100, 66);
			this.tbEmail2.Name = "tbEmail2";
			this.tbEmail2.Size = new System.Drawing.Size(269, 22);
			this.tbEmail2.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label2.Location = new System.Drawing.Point(4, 66);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 5;
			this.label2.Text = "re-enter email:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// lError
			// 
			this.lError.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lError.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lError.ForeColor = System.Drawing.Color.Red;
			this.lError.Location = new System.Drawing.Point(0, 123);
			this.lError.Name = "lError";
			this.lError.Size = new System.Drawing.Size(381, 23);
			this.lError.TabIndex = 7;
			this.lError.Text = "This is an error message right here folks!";
			this.lError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// UserEmailDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(381, 146);
			this.ControlBox = false;
			this.Controls.Add(this.lError);
			this.Controls.Add(this.tbEmail2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tbEmail1);
			this.Controls.Add(this.lPass);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "UserEmailDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Enter Your Email Address";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.UserEmailDialogLoad);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label lError;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbEmail2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox tbEmail1;
		private System.Windows.Forms.Label lPass;
	}
}
