/* SugarCube Host Software - Security Token Wizard
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form {
		private Color _buttonBackBlue = Color.FromArgb(202, 224, 246);
		private Color _darkTextBlue = Color.FromArgb(96, 134, 181);

		public MainForm() {
			InitializeComponent();
			btnGO.BackColor = _buttonBackBlue;
			btnCopy.BackColor = _buttonBackBlue;
			btnSet.BackColor = _buttonBackBlue;
			btnClear.BackColor = _buttonBackBlue;

			btnGO.ForeColor = _darkTextBlue;
			btnCopy.ForeColor = _darkTextBlue;
			btnSet.ForeColor = _darkTextBlue;
			btnClear.ForeColor = _darkTextBlue;

			this.tbToken.Text = SecurityManager.GetSecurityToken();
		}

		void BtnGOClick(object sender, EventArgs e) {
			if (String.IsNullOrWhiteSpace(tbPassword.Text)) {
				lStatus.Text = "Password can not be empty";
				return;
			}
			if (tbPassword.Text.Length < 8) {
				lStatus.Text = "Password must be eight or more characters";
				return;
			}
			this.tbToken.Text = SecurityManager.HashPassword(tbPassword.Text);
			lStatus.Text = "New security token has been generated";
		}

		void BtnCopyClick(object sender, EventArgs e) {
			if (String.IsNullOrWhiteSpace(tbToken.Text)) {
				lStatus.Text = "No Token to Copy";
				return;
			}
			Clipboard.SetText(tbToken.Text);
			lStatus.Text = "Copied Security Token to Clipboard";

		}

		void BtnSetClick(object sender, EventArgs e) {
			if (!SecurityManager.IsAdministrator()) {
				MessageBox.Show("You must run this tool as an administrator in order to set the registry");
				return;
			}

			if (String.IsNullOrWhiteSpace(tbToken.Text)) {
				lStatus.Text = "No Token to Set";
				return;
			}
			SecurityManager.SetSecurityToken(tbToken.Text);
			lStatus.Text = "Set Security Token in the Registry";
		}


		void BtnClearClick(object sender, EventArgs e) {
			if (!SecurityManager.IsAdministrator()) {
				MessageBox.Show("You must run this tool as an administrator in order to set the registry");
				return;
			}

			SecurityManager.ClearSecurityToken();
			lStatus.Text = "Cleared Security Token in the Registry";
			tbPassword.Text = "";
			tbToken.Text = "";

		}
	}
}
