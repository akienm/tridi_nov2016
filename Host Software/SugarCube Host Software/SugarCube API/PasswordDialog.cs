/* SugarCube Host Software - Manager MVP
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// A simple password prompt dialog that is used by the <see cref="SecurityManager"/> in 
	/// <see cref="SecurityManager.PromptForAndCheckPassword"/>
	/// </summary>
	public partial class PasswordDialog : Form {
		/// <summary>
		/// The colour of our button background
		/// </summary>
		private Color _buttonBackBlue = Color.FromArgb(202, 224, 246);
		/// <summary>
		/// The colour of all the text on the form
		/// </summary>
		private Color _darkTextBlue = Color.FromArgb(96, 134, 181);
		/// <summary>
		/// This is where we stash the entered password for retrieval by the caller
		/// </summary>
		public string Password;

		/// <summary>
		/// Intialize the form and set the colours of our buttons and text
		/// </summary>
		public PasswordDialog() {
			InitializeComponent();
			btnCancel.BackColor = _buttonBackBlue;
			btnOK.BackColor = _buttonBackBlue;

			btnCancel.ForeColor = _darkTextBlue;
			btnOK.ForeColor = _darkTextBlue;

			lPass.ForeColor = _darkTextBlue;
		}

		/// <summary>
		/// When the user click OK (or hits enter), stash the value of tbPass.Text into Password and set the 
		/// dialog result to OK
		/// </summary>
		void BtnOKClick(object sender, EventArgs e) {
			Password = tbPass.Text;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// When the user click Cancel (or hits escape) set the value of dialog result to cancel
		/// </summary>
		void BtnCancelClick(object sender, EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
