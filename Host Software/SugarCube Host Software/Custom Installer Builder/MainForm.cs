/* SugarCube Host Software - Custom Installer Builder
 * Copyright (c) 2015 Chad Ullman
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using org.mkulu.config;

namespace Me.ThreeDWares.SugarCube {
	public partial class MainForm : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		//private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

		/// <summary>
		/// The private key that is used to encrypt/decrypt config data
		/// </summary>
		private const string KEY = @"3JVr4Y^Nw!@kMdEgbFT#aaFZ4BM45F2&R6gbbNn8*";

		public MainForm() {
			InitializeComponent();
			pbImagePreview.Image = global::Me.ThreeDWares.SugarCube.InstallationComponents.BlankBrandImage;
		}

		void BtnSelectOrderFormClick(object sender, EventArgs e) {
			dlgOpenFile.Filter = "PDF Files|*.pdf";
			dlgOpenFile.FileName = "";
			dlgOpenFile.Title = "Select Order Form";
			if (dlgOpenFile.ShowDialog() == DialogResult.OK) {
				tbOrderForm.Text = dlgOpenFile.FileName;
			}
		}

		void BtnSelectImageClick(object sender, EventArgs e) {
			dlgOpenFile.Filter = "Graphic Files|*.bmp;*.jpg;*.jpeg;*.png";
			dlgOpenFile.FileName = "";
			dlgOpenFile.Title = "Select Brand Image";
			if (dlgOpenFile.ShowDialog() == DialogResult.OK) {
				Image brandImage = new Bitmap(dlgOpenFile.FileName);
				if (brandImage.Width != 488 || brandImage.Height != 120) {
					MessageBox.Show("The image must be 488 x 120 pixels in size", "Incorrect Image Dimensions", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				pbImagePreview.Image = brandImage;
				tbImage.Text = dlgOpenFile.FileName;
			}
		}

		void TbOrderFormTextChanged(object sender, EventArgs e) {
			if (!String.IsNullOrEmpty(tbOrderForm.Text)) {
				tbPass1.Enabled = true;
				tbPass2.Enabled = true;
			}
		}
		
		void BtnSelectInstallerClick(object sender, EventArgs e)
		{
			dlgOpenFile.Filter = "Installer Images|*.msi";
			dlgOpenFile.FileName = "*.msi";
			dlgOpenFile.Title = "Select SugarCube Manager Base Installer";
			if (dlgOpenFile.ShowDialog() != DialogResult.OK) {
				return;
			}
			tbInstallerPath.Text = dlgOpenFile.FileName;
			
		}
		
		void BtnSetSaveLocationClick(object sender, EventArgs e)
		{
			dlgSelectSaveFolder.Description = "Select the folder to generate the new installer in";
			// Prompt for location to save the new custom installer
			if (dlgSelectSaveFolder.ShowDialog() != DialogResult.OK) {
				return;
			}
			tbSaveLocation.Text = dlgSelectSaveFolder.SelectedPath;
			
		}

		void BtnCancelClick(object sender, EventArgs e) {
			DialogResult result = MessageBox.Show("Are you sure you want to exit the custom installer builder?", "Cancellation Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.Yes) {
				this.Close();
			}
		}

		void ShowError(string message) {
			MessageBox.Show(message, "There Is A Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		
		void CompressInstallerFiles(string savePath) {
			Process p = new Process();
			p.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "7za.exe");
			p.StartInfo.WorkingDirectory = savePath;
			p.StartInfo.Arguments = "a -t7z scmdeploy.7z SugarCubeManager.msi vc_redist-2012.x86.exe OrderSec.cfg OrderForm.pdf BrandLogo.bmp";
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.Start();
			string errors = p.StandardOutput.ReadToEnd();
			if (errors.ToLower().Contains("error:")) {
				ShowError("Unable to compress installation files:\n   " + errors);
  			}
			p.WaitForExit();
			p.Close();			
		}

		void BtnGenerateClick(object sender, EventArgs e) {
			// Check if there is an order form specified, and make sure the file exists
			if (!String.IsNullOrEmpty(tbOrderForm.Text) && !File.Exists(tbOrderForm.Text)) {
				ShowError("The order form specified can not be found:\n" + tbOrderForm.Text);
				return;
			}

			// If there is an order form, check the password
			if (!String.IsNullOrEmpty(tbOrderForm.Text)) {
				if (tbPass1.Text != tbPass2.Text) {
					ShowError("The passwords to encrypt the order form do not match, please recheck");
					return;
				}
			}

			// Check location of manager msi
			if (String.IsNullOrEmpty(tbInstallerPath.Text) || !File.Exists(tbInstallerPath.Text)) {
				ShowError("You must provide a valid path to the SugarCube Manager base installer");
				return;
			}
			string msiPath = tbInstallerPath.Text;
			// We *assume* that our copy of the VisualC++ 2012 redistributable is in the same path as our core msi
			string vcPath = Path.Combine(Path.GetDirectoryName(msiPath), "vc_redist-2012.x86.exe");
			// Check location of Visual C++ redistributable
			if (String.IsNullOrEmpty(vcPath) || !File.Exists(vcPath)) {
				ShowError("You are missing a copy of the VisualC++ 2012 Redistributable, which should be in the same location as the SugarCube Manager base installer");
				return;
			}

			// Prompt for location to save the new custom installer
			if (String.IsNullOrEmpty(tbSaveLocation.Text)) {
				ShowError("You must choose the location to save the generated installer to");
				return;
			}
			string saveFolder = tbSaveLocation.Text;


			// Copy manager msi to save location as SugarCubeManager.msi
			File.Copy(msiPath, Path.Combine(saveFolder, "SugarCubeManager.msi"));
			// Copy VC++ redistributable to save location
			File.Copy(vcPath, Path.Combine(saveFolder, "vc_redist-2012.x86.exe"));

			// Copy order form to save location as OrderForm.pdf (or create zero-byte one)
			if (File.Exists(tbOrderForm.Text)) {
				File.Copy(tbOrderForm.Text, Path.Combine(saveFolder, "OrderForm.pdf"));
			} else {
				FileStream orderForm = File.Create(Path.Combine(saveFolder, "OrderForm.pdf"));
				orderForm.Close();
			}

			// Create OrderSec.cfg in save location
			IniFileManager securityConfig = new IniFileManager(Path.Combine(saveFolder, "OrderSec.cfg"), KEY, true);
			securityConfig.writeEncryptedString("SECURITY", "hcryptpass", tbPass1.Text);

			// Create BrandLogo.bmp in save location
			pbImagePreview.Image.Save(Path.Combine(saveFolder, "BrandLogo.bmp"), ImageFormat.Bmp);

			// Use 7Zip to zip up SugarCubeManager.msi, OrderSec.cfg, OrderForm.pdf and BrandLogo.bmp into scmdeploy.7z
			CompressInstallerFiles(saveFolder);
			
			// Create the three byte arrays for the pieces of the scmsetup.exe file
			byte[] zipHeader = global::Me.ThreeDWares.SugarCube.InstallationComponents._7zSD;
			byte[] zipConfig = global::Me.ThreeDWares.SugarCube.InstallationComponents._7ZipSelfExtractorConfig;
			byte[] scmDeploy = File.ReadAllBytes(Path.Combine(saveFolder, "scmdeploy.7z"));

			// Concatenate 7zSD, 7ZipSelfExtractorConfig and scmdeploy into setup.exe
			byte[] setupExe = new byte[ zipHeader.Length + zipConfig.Length + scmDeploy.Length ];
			System.Buffer.BlockCopy( zipHeader, 0, setupExe, 0, zipHeader.Length );
			System.Buffer.BlockCopy( zipConfig, 0, setupExe, zipHeader.Length, zipConfig.Length );
			System.Buffer.BlockCopy( scmDeploy, 0, setupExe, zipHeader.Length + zipConfig.Length, scmDeploy.Length);
			File.WriteAllBytes(Path.Combine(saveFolder, "scmsetup.exe"), setupExe);

			// Clean up leftover files
			File.Delete(Path.Combine(saveFolder, "SugarCubeManager.msi"));
			File.Delete(Path.Combine(saveFolder, "vc_redist-2012.x86.exe"));
			File.Delete(Path.Combine(saveFolder, "OrderForm.pdf"));
			File.Delete(Path.Combine(saveFolder, "BrandLogo.bmp"));
			File.Delete(Path.Combine(saveFolder, "scmdeploy.7z"));
			
			// Let the use know that we are done
			string message = "The customized SugarCube Manager installer (scmsetup.exe) is avilable at:\n" + saveFolder + "\n\nIn addition, in this folder you will find your order form encryption key, OrderSec.cfg.  This file contains your OrderForm encryption/decryption password, please keep it safe within your organization.\n\nNote: Your actual order form password is encrypted in this file";
			MessageBox.Show(message, "Generation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
			
		}

	}
}
