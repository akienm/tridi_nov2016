/* SugarCube Host Software - Security Token Wizard
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

			if (!SecurityManager.PromptForAndCheckPassword()) {
				MessageBox.Show("Inavlid password, you can't come in!");
				return;
			}

			Application.Run(new MainForm());
		}

	}
}
