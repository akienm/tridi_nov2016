/* SugarCube Host Software - Order Form Decrypter
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using org.mkulu.config;

namespace Me.ThreeDWares.SugarCube {
	public partial class MainForm : Form {
		private const bool ENABLED = true;
		private const bool DISABLED = false;

		/// <summary>
		/// The private key that is used to encrypt/decrypt config data
		/// </summary>
		private const string KEY = @"3JVr4Y^Nw!@kMdEgbFT#aaFZ4BM45F2&R6gbbNn8*";

		private string _dataPath;
		private IniFileManager _securityConfig;

		public MainForm() {
			InitializeComponent();
			_dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3DWares");
			if (!Directory.Exists(_dataPath)) {
				Directory.CreateDirectory(_dataPath);
			}
			_securityConfig = new IniFileManager(Path.Combine(_dataPath, "OrderSec.cfg"), KEY, true);
		}

		void MainFormLoad(object sender, EventArgs e) {
			tbPassword.Text = _securityConfig.getEncryptedString("SECURITY", "hcryptpass", "");
		}


		void BtnChooseFileClick(object sender, EventArgs e) {
			openFileDialog.Filter = "Encrypted Orders|*.head.hcrypt";
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				tbOrderForm.Text = openFileDialog.FileName;
			}
		}

		void BtnImportPasswordClick(object sender, EventArgs e) {
			openFileDialog.Filter = "Security Config Files|*.cfg";
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				setUIState(DISABLED);
				string newSecurityFile = openFileDialog.FileName;
				try {
					IniFileManager test = new IniFileManager(newSecurityFile, KEY, true);
					string newPassword = test.getEncryptedString("SECURITY", "hcryptpass", "@@@***$$$%%%12345");
					if (newPassword == "@@@***$$$%%%12345") {
						ShowError("The file '" + newSecurityFile + "' does not contain a valid encryption password.");
					}
					_securityConfig.writeEncryptedString("SECURITY", "hcryptpass", newPassword);
					tbPassword.Text = newPassword;
				} catch {
					ShowError("The file '" + newSecurityFile + "' does not appear to be a valid security config file");
				} finally {
					setUIState(ENABLED);
				}
			}
		}

		private void setUIState(bool state) {
			tbOrderForm.Enabled = state;
			tbPassword.Enabled = state;
			btnChooseFile.Enabled = state;
			btnImportPassword.Enabled = state;
			btnDecrypt.Enabled = state;
			btnExit.Enabled = state;
		}

		private void ShowError(string message) {
			MessageBox.Show(message, "Unable to Decrypt Order File", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		void BtnDecryptClick(object sender, EventArgs e) {
			setUIState(DISABLED);

			if (String.IsNullOrWhiteSpace(tbOrderForm.Text)) {
				ShowError("You must select an order form to decrypt.");
				setUIState(ENABLED);
				return;
			}

			if (!File.Exists(tbOrderForm.Text)) {
				ShowError("The specified order form '" + tbOrderForm.Text + "' can not be found.");
				setUIState(ENABLED);
				return;
			}

			if (!tbOrderForm.Text.Contains(".head.hcrypt")) {
				ShowError("The specified order form '" + tbOrderForm.Text + "' is not a valid encrypted order form.");
				setUIState(ENABLED);
				return;
			}

			if (!File.Exists(tbOrderForm.Text.Replace(".head.", ".data."))) {
				string dataFileName = Path.GetFileName(tbOrderForm.Text.Replace(".head.", ".data."));
				ShowError("The data portion of the order form is missing.  You need to find the file '" + dataFileName + "' and copy it in to the same location as your order form header.");
				setUIState(ENABLED);
				return;
			}

			if (String.IsNullOrWhiteSpace(tbPassword.Text)) {
				ShowError("You must provide the password to decrypt the order form.");
				setUIState(ENABLED);
				return;
			}

			_securityConfig.writeEncryptedString("SECURITY", "hcryptpass", tbPassword.Text);

			if (DecryptOrderForm()) {
				DialogResult res = MessageBox.Show("Do you want to delete the encrypted order form?", "Decryption Succesful",
				                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.Yes) {
					File.Delete(tbOrderForm.Text);
					File.Delete(tbOrderForm.Text.Replace(".head.", ".data."));
					tbOrderForm.Text = "";
				}
			}
			setUIState(ENABLED);
		}

		private bool DecryptOrderForm() {
			bool result = true;
			Process p = new Process();
			p.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "hcrypt.exe");
			p.StartInfo.Arguments = String.Format("--decrypt --file=\"{0}\" --pass=\"{1}\"", tbOrderForm.Text, tbPassword.Text);
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.Start();
			string errors = p.StandardOutput.ReadToEnd();
			if (!String.IsNullOrWhiteSpace(errors)) {
				ShowError("There was an error while decrypting the order file.\n\n" + errors);
				result = false;
			}
			p.WaitForExit();
			p.Close();
			return result;
		}
		
		void BtnExitClick(object sender, EventArgs e) {
			this.Close();
		}


	}
}
