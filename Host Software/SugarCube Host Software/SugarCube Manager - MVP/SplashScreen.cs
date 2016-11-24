/* SugarCube Host Software - Manager MVP
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// Description of SplashScreen.
	/// </summary>
	public partial class SplashScreen : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(SplashScreen));

		public SplashScreen() {
			if (log.IsInfoEnabled) {
				log.Info("Initializing splash screen form");
			}

			InitializeComponent();
			pbSplash.Controls.Add(pbLoader);
			//pbLoader.Location = new Point(0, 0);
			pbLoader.BackColor = Color.Transparent;

			writeVersion("v " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
		}
		
		
      private void writeVersion(string text) {
			Font vFont = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			Graphics surface = Graphics.FromImage(pbSplash.Image);
			Size textSize = TextRenderer.MeasureText(text, vFont);

			// Create drawing rectangle for version, leaving a 3x3 margin
         // numbers at the end are to take the actual position of the visible splash body in to account (there is a large invisible margin)
         int x = pbSplash.Image.Width - textSize.Width - 10;
         int y = pbSplash.Image.Height - textSize.Height - 39;
         
         if (log.IsDebugEnabled) {
             log.Debug("Writing version string to image");
         }
         
         // Draw version to the splash screen
         TextRenderer.DrawText(surface, text, vFont, new Point(x, y), Color.Black);
      }		

	}
}
