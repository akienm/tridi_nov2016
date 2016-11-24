/* SugarCube Host Software - Manager MVP
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using log4net;
using org.mkulu.config;

namespace Me.ThreeDWares.SugarCube {
	public partial class ConfigManager : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(ConfigManager));
		
		private CubeConfig _config;

		public ConfigManager(ref CubeConfig config) {
			if (log.IsInfoEnabled) {
			    log.Info("Initializing object");
			}
			
			InitializeComponent();
			_config = config;
		}

		void ConfigManagerLoad(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Running onLoad, reading config");
			}
			
			tbCamName.Text = _config.Camera;
			tbResolution.Text = _config.Resolution;

			tbShootString.Text = _config.ShootString;
			udQuality.Value = _config.JpegQuality;
			cbMotionDetection.Checked = _config.UseMotionDetection;
			udSensitivity.Value = _config.MotionSensitivity;
			cbMedianStacking.Checked = _config.UseMedianStacking;
			cbContrastEnhancement.Checked = _config.UseContrastEnhancement;
			udMidpoint.Value = _config.ContrastMidpoint;
			udContrastValue.Value = _config.ContrastValue;
			cbSetEXIFData.Checked = _config.DoSetEXIF;

			tbHost.Text = _config.SftpHost;
			tbUser.Text = _config.SftpUser;
			tbPass.Text = _config.SftpPassword;
			tbScanIDPrefix.Text = _config.ShootPrefix;
			tbUploadBase.Text = _config.SftpUploadBase;
			tbTriggerURL.Text = _config.TriggerURL;

			tbHcryptPass.Text = _config.HCryptPassword;

			cbDoImageCropping.Checked = _config.DoImageCropping;
			tbCropRectangle.Text = _config.ImageCropRectangle;
			cbDoModelScaling.Checked = _config.DoModelScaling;
			cbDoModelCropping.Checked = _config.DoModelCropping;
			tbBoundBox.Text = _config.BoundingBox;
			udMeshLevel.Value = _config.MeshLevel;
			tbTemplate.Text = _config.Template3DP;

			CbMotionDetectionCheckedChanged(this, null);
			CbContrastEnhancementCheckedChanged(this, null);
			CbDoImageCroppingCheckedChanged(this, null);
			CbDoModelScalingCheckedChanged(this, null);
		}

		void BtnOKClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Saving config to disk");
			}
			
			_config.Camera = tbCamName.Text;
			_config.Resolution = tbResolution.Text;

			_config.ShootString =  tbShootString.Text;
			_config.UseMotionDetection = cbMotionDetection.Checked;
			_config.MotionSensitivity = Convert.ToInt16(udSensitivity.Value);
			_config.JpegQuality = Convert.ToInt16(udQuality.Value);
			_config.UseMedianStacking = cbMedianStacking.Checked;
			_config.UseContrastEnhancement = cbContrastEnhancement.Checked;
			_config.ContrastMidpoint = Convert.ToInt16(udMidpoint.Value);
			_config.ContrastValue = Convert.ToInt16(udContrastValue.Value);
			_config.DoSetEXIF = cbSetEXIFData.Checked;

			_config.SftpHost = tbHost.Text;
			_config.SftpUser = tbUser.Text;
			_config.SftpPassword = tbPass.Text;
			_config.ShootPrefix = tbScanIDPrefix.Text;
			_config.SftpUploadBase = tbUploadBase.Text;
			_config.TriggerURL = tbTriggerURL.Text;

			_config.HCryptPassword = tbHcryptPass.Text;

			_config.DoImageCropping = cbDoImageCropping.Checked;
			_config.ImageCropRectangle = tbCropRectangle.Text;
			_config.DoModelScaling = cbDoModelScaling.Checked;
			_config.DoModelCropping = cbDoModelCropping.Checked;
			_config.BoundingBox = tbBoundBox.Text;
			_config.MeshLevel = Convert.ToInt16(udMeshLevel.Value);
			_config.Template3DP = tbTemplate.Text;
			this.Close();
		}



		void CbMotionDetectionCheckedChanged(object sender, EventArgs e) {
			udSensitivity.Enabled = cbMotionDetection.Checked;

		}

		void CbContrastEnhancementCheckedChanged(object sender, EventArgs e) {
			udMidpoint.Enabled = cbContrastEnhancement.Checked;
			udContrastValue.Enabled = cbContrastEnhancement.Checked;
		}

		void CbDoImageCroppingCheckedChanged(object sender, EventArgs e) {
			tbCropRectangle.Enabled = cbDoImageCropping.Checked;
		}

		void CbDoModelScalingCheckedChanged(object sender, EventArgs e) {
			if (!cbDoModelScaling.Checked) {
				cbDoModelCropping.Checked = false;
				cbDoModelCropping.Enabled = false;
				tbBoundBox.Enabled = false;
			} else {
				cbDoModelCropping.Enabled = true;
				CbDoModelCroppingCheckedChanged(this, null);
			}
		}

		void CbDoModelCroppingCheckedChanged(object sender, EventArgs e) {
			tbBoundBox.Enabled = cbDoModelCropping.Checked;
		}
		
		// Works in conjunction with TbScanIDPrefixKeyPress to make sure the user is not entering an invalid Shoot String ID
		void TbScanIDPrefixKeyDown(object sender, KeyEventArgs e) {
			_config.CheckKeyIsValidForScanID(e);
		}
		
		// If the key is not allowed, we set handled to be true which means that the keypress never makes it all the way to the control.
		// We also pop up a warning dialog
		void TbScanIDPrefixKeyPress(object sender, KeyPressEventArgs e) {
			e.Handled = !_config.IsValidKeyForScanID;
			if (e.Handled) {
				MessageBox.Show("Scan ID Prefix can only contain the characters A-Z, a-z, 0-9 and -", "Configuration Manager Error",
									 MessageBoxButtons.OK, MessageBoxIcon.Error);
			}			
		}



	}
}
