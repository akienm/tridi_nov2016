/* Mkulu 
 * Copyright (c) 201x Chad Ullman
 * 
 * This software is provided 'as-is', without any express or implied warranty. In
 * no event will the authors be held liable for any damages arising from the use
 * of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose, including
 * commercial applications, and to alter it and redistribute it freely, subject to
 * the following restrictions:
 * 
 *    1. The origin of this software must not be misrepresented; you must not claim
 *       that you wrote the original software. If you use this software in a product,
 *       an acknowledgment in the product documentation would be appreciated but is
 *       not required.
 * 
 *    2. Altered source versions must be plainly marked as such, and must not be
 *       misrepresented as being the original software.
 * 
 *    3. This notice may not be removed or altered from any source distribution.
 */
namespace Me.ThreeDWares.SugarCube
{
	partial class SplashScreen
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
			this.pbSplash = new System.Windows.Forms.PictureBox();
			this.pbLoader = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbSplash)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbLoader)).BeginInit();
			this.SuspendLayout();
			// 
			// pbSplash
			// 
			this.pbSplash.Image = ((System.Drawing.Image)(resources.GetObject("pbSplash.Image")));
			this.pbSplash.InitialImage = null;
			this.pbSplash.Location = new System.Drawing.Point(1, 1);
			this.pbSplash.Margin = new System.Windows.Forms.Padding(0);
			this.pbSplash.Name = "pbSplash";
			this.pbSplash.Size = new System.Drawing.Size(450, 270);
			this.pbSplash.TabIndex = 0;
			this.pbSplash.TabStop = false;
			// 
			// pbLoader
			// 
			this.pbLoader.BackColor = System.Drawing.Color.Transparent;
			this.pbLoader.Image = ((System.Drawing.Image)(resources.GetObject("pbLoader.Image")));
			this.pbLoader.Location = new System.Drawing.Point(188, 136);
			this.pbLoader.Margin = new System.Windows.Forms.Padding(0);
			this.pbLoader.Name = "pbLoader";
			this.pbLoader.Size = new System.Drawing.Size(66, 66);
			this.pbLoader.TabIndex = 1;
			this.pbLoader.TabStop = false;
			// 
			// SplashScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(110)))));
			this.ClientSize = new System.Drawing.Size(451, 270);
			this.Controls.Add(this.pbLoader);
			this.Controls.Add(this.pbSplash);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashScreen";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SplashScreen";
			this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(110)))));
			((System.ComponentModel.ISupportInitialize)(this.pbSplash)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbLoader)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.PictureBox pbLoader;
		private System.Windows.Forms.PictureBox pbSplash;
	}
}
