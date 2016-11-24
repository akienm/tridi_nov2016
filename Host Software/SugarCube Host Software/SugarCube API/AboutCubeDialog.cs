/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// A simple about form
	/// </summary>
	public partial class AboutCubeDialog : Form {
		private static readonly ILog log = LogManager.GetLogger(typeof(AboutCubeDialog));

		public AboutCubeDialog() {
			if (log.IsDebugEnabled) {
			    log.Debug("Initializing about dialog");
			}
			
			InitializeComponent();
		}
	}
}
