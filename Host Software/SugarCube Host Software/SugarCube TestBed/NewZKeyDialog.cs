/* SugarCube Host Software - Test Bed
 * Copyright (c) 2014-2015 Chad Ullman
 */
 
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Me.ThreeDWares.SugarCube {
	public partial class NewZKeyDialog : Form {
		public string NewValue;
	
		public NewZKeyDialog() {
			InitializeComponent();
		}
		
		void BtnOKClick(object sender, EventArgs e) {
			string errors = "";
			if (String.IsNullOrWhiteSpace(tbKey.Text)) {
				errors = errors + "- The key can not be empty\n";
			}
			if (Regex.IsMatch(tbKey.Text, "[^A-Z]")) {
				errors = errors + "- The key can only contain uppercase Latin 1 characters from A to Z\n";
			}
			if (!String.IsNullOrWhiteSpace(tbKey.Text) && (tbKey.Text.Length < 2 || tbKey.Text.Length > 5)) {
				errors = errors + "- The key must be between two and five characters long\n";
			}
			if (errors != "") {
				MessageBox.Show("There was a problem with the Z key you entered:\n" + errors, "Invalid Z Key",
									 MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			NewValue = tbKey.Text;
			this.Close();
		}
	}
}
