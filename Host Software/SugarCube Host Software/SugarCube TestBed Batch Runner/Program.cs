/* SugarCube Host Software - Test Bed Batch Runner
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using log4net;
using log4net.Config;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool IsIconic(IntPtr hWnd);

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		private const int SH_SHOW = 5;
		private const int SH_RESTORE = 9;

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// Bring the window specified by hwnd to the foreground.  If necessary, first restore the window
		/// from the task bar
		/// </summary>
		/// <param name="hwnd">The handle of the window to activate</param>
		private static void Activate(IntPtr hwnd) {
			if (IsIconic(hwnd))
				ShowWindow(hwnd, SH_RESTORE);
			else
				ShowWindow(hwnd, SH_SHOW);

			SetForegroundWindow(hwnd);
		}

		[STAThread]
		private static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			bool abort = false;
			// Check if we've already started the Batch Runner
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex appMutex = new Mutex(true, "SugarCube_TestBed_Batch", out isFirstInstance)) {
				if (!isFirstInstance) {
					// The application is already running, so let's find its process id
					Process me = Process.GetCurrentProcess();
					foreach (Process process in Process.GetProcessesByName (me.ProcessName)) {
						if (process.Id != me.Id) {
							// found it, let's switch over and then exit
							Activate(process.MainWindowHandle);
							abort = true;
							break;
						}
					}
				}

				if (abort) {
					return;
				}

				// Check if we're going to be using logging
				string loggingConfigFile = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3DWares"), "tbbatch.lc.xml");
				if (File.Exists(loggingConfigFile)) {
					XmlConfigurator.Configure(new FileInfo(loggingConfigFile));
				}
					
				if (!SecurityManager.PromptForAndCheckPassword()) {
					MessageBox.Show("Invalid password, you do not have permission to run this application.");
					return;
				}

				bool noOtherCubeUserIsRunning;
				// Make sure not other software that communicates with the cube is running, if it is, abort
				using (Mutex cubeMutex = new Mutex(true, "SugarCube_Interactor", out noOtherCubeUserIsRunning)) {
					if (!noOtherCubeUserIsRunning) {
						if (log.IsErrorEnabled) {
						    log.Error("Test Bed Batch Runner can not start while either the test bed or manager are already running");
						}
						
						MessageBox.Show("The Test Bed Batch Runenr can NOT be run while the test bed or manager are open", "UNABLE TO START TEST BED BATCH RUNNER",
											 MessageBoxButtons.OK, MessageBoxIcon.Error);
						abort = true;
					}


					if (abort) {
						return;
					}

					Application.Run(new MainForm());
				} // release the cubeMutex
			} // release the appMutex

		}
	}


}
