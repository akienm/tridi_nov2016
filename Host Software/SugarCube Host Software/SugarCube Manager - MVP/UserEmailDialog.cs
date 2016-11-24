/* SugarCube Host Software - Manager MVP
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Drawing;
using System.Windows.Forms;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	public partial class UserEmailDialog : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(UserEmailDialog));
		
		private Color _buttonBackBlue = Color.FromArgb(202, 224, 246);
		private Color _darkTextBlue = Color.FromArgb(96, 134, 181);
		public string EMail = "";
		private CubeConfig _config;

		public UserEmailDialog(CubeConfig config) {
			if (log.IsInfoEnabled) {
			    log.Info("initializing form");
			}
			
			InitializeComponent();
			lError.Text = "";
			_config = config;
			btnCancel.BackColor = _buttonBackBlue;
			btnOK.BackColor = _buttonBackBlue;

			btnCancel.ForeColor = _darkTextBlue;
			btnOK.ForeColor = _darkTextBlue;
		}

		void BtnOKClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Processing emails");
			}
			
			if (String.IsNullOrWhiteSpace(tbEmail1.Text) || String.IsNullOrWhiteSpace(tbEmail2.Text)) {
				if (log.IsInfoEnabled) {
				    log.InfoFormat("one or more of the email addresses was empty [{0}] [{1}]", tbEmail1.Text, tbEmail2.Text);
				}
				lError.Text = "eMail address can not be empty";
				return;
			}

			if (tbEmail1.Text.Trim() != tbEmail2.Text.Trim()) {
				if (log.IsInfoEnabled) {
				    log.InfoFormat("The emails do not match [{0}] [{1}]", tbEmail1.Text, tbEmail2.Text);
				}
				lError.Text = "eMail addresses do not match";
				return;
			}
			if (!_config.IsValidEmail(tbEmail1.Text.Trim())) {
				if (log.IsInfoEnabled) {
				    log.Info(tbEmail1.Text.Trim() + " is not a valid email address");
				}
				
				lError.Text = tbEmail1.Text.Trim() + " is not a valid email address";
				return;
			}

			EMail = tbEmail1.Text.Trim();

			if (log.IsInfoEnabled) {
			    log.Info("Email was set to " + EMail);
			}
			
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		void BtnCancelClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("User cancelled");
			}
			
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		
		void UserEmailDialogLoad(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
			this.Show();
			this.WindowState = FormWindowState.Normal;
			tbEmail1.Focus();
		}
	}
}
