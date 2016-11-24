/* SugarCube Host Software - Order Form Decrypter
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Windows.Forms;

namespace Me.ThreeDWares.SugarCube {
	internal sealed class Program {
		[STAThread]
		private static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

	}
}
