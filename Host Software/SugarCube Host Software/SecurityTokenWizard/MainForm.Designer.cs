/* SugarCube Host Software - Security Token Wizard
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbToken = new System.Windows.Forms.TextBox();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnSet = new System.Windows.Forms.Button();
			this.btnGO = new System.Windows.Forms.Button();
			this.lStatus = new System.Windows.Forms.Label();
			this.btnClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label1.Location = new System.Drawing.Point(1, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "New Password:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(203)))), ((int)(((byte)(240)))));
			this.label2.Location = new System.Drawing.Point(1, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Security Token:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbPassword
			// 
			this.tbPassword.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbPassword.Location = new System.Drawing.Point(107, 9);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.Size = new System.Drawing.Size(368, 22);
			this.tbPassword.TabIndex = 2;
			// 
			// tbToken
			// 
			this.tbToken.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbToken.Location = new System.Drawing.Point(107, 46);
			this.tbToken.Name = "tbToken";
			this.tbToken.ReadOnly = true;
			this.tbToken.Size = new System.Drawing.Size(408, 22);
			this.tbToken.TabIndex = 3;
			// 
			// btnCopy
			// 
			this.btnCopy.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCopy.Location = new System.Drawing.Point(107, 72);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(125, 26);
			this.btnCopy.TabIndex = 4;
			this.btnCopy.Text = "Copy to Clipboard";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.BtnCopyClick);
			// 
			// btnSet
			// 
			this.btnSet.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSet.Location = new System.Drawing.Point(246, 72);
			this.btnSet.Name = "btnSet";
			this.btnSet.Size = new System.Drawing.Size(125, 26);
			this.btnSet.TabIndex = 5;
			this.btnSet.Text = "Set in Registry";
			this.btnSet.UseVisualStyleBackColor = true;
			this.btnSet.Click += new System.EventHandler(this.BtnSetClick);
			// 
			// btnGO
			// 
			this.btnGO.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGO.Location = new System.Drawing.Point(481, 6);
			this.btnGO.Name = "btnGO";
			this.btnGO.Size = new System.Drawing.Size(38, 23);
			this.btnGO.TabIndex = 6;
			this.btnGO.Text = "GO";
			this.btnGO.UseVisualStyleBackColor = true;
			this.btnGO.Click += new System.EventHandler(this.BtnGOClick);
			// 
			// lStatus
			// 
			this.lStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lStatus.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(134)))), ((int)(((byte)(181)))));
			this.lStatus.Location = new System.Drawing.Point(0, 107);
			this.lStatus.Name = "lStatus";
			this.lStatus.Size = new System.Drawing.Size(531, 23);
			this.lStatus.TabIndex = 7;
			this.lStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClear
			// 
			this.btnClear.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClear.Location = new System.Drawing.Point(386, 72);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(125, 26);
			this.btnClear.TabIndex = 8;
			this.btnClear.Text = "Clear Security Token";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.BtnClearClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(531, 130);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.lStatus);
			this.Controls.Add(this.btnGO);
			this.Controls.Add(this.btnSet);
			this.Controls.Add(this.btnCopy);
			this.Controls.Add(this.tbToken);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SugarCube Security Token Wizard";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label lStatus;
		private System.Windows.Forms.Button btnGO;
		private System.Windows.Forms.Button btnSet;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.TextBox tbToken;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}
