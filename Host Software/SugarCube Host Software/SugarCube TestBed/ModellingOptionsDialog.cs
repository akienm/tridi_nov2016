/* SugarCube Host Software - Test Bed
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// Description of ModellingOptionsDialog.
	/// </summary>
	public partial class ModellingOptionsDialog : Form {
		public ModelingOptions settings;

		public ModellingOptionsDialog(ModelingOptions opts) {
			InitializeComponent();
			cbDoImageCropping.Checked = Convert.ToBoolean(opts.doImageCropping);
			tbCropRect.Text = opts.imageCroppingRectangle;
			cbDoModelCropping.Checked = Convert.ToBoolean(opts.doModelCropping);
			cbDoModelScalling.Checked = Convert.ToBoolean(opts.doModelScaling);
			numMeshLevel.Value = Convert.ToInt16(opts.meshlevel);
			tbBoundBox.Text = opts.boundingBox;
			tbTemplate3dp.Text = opts.template3DP;
			CbDoImageCroppingCheckedChanged(this, null);
			CbDoModelScallingCheckedChanged(this, null);
		}

		void BtnOKClick(object sender, EventArgs e) {
			settings = new ModelingOptions();
			settings.doImageCropping = cbDoImageCropping.Checked.ToString();
			settings.imageCroppingRectangle = tbCropRect.Text;
			settings.doModelCropping = cbDoModelCropping.Checked.ToString();
			settings.doModelScaling = cbDoModelScalling.Checked.ToString();
			settings.meshlevel = numMeshLevel.Value.ToString();
			settings.boundingBox = tbBoundBox.Text;
			settings.template3DP = tbTemplate3dp.Text;
			Close();
		}

		void CbDoImageCroppingCheckedChanged(object sender, EventArgs e) {
			tbCropRect.Enabled = cbDoImageCropping.Checked;
		}

		void CbDoModelScallingCheckedChanged(object sender, EventArgs e) {
			if (!cbDoModelScalling.Checked) {
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
	}
}
