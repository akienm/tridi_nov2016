/* SugarCube Host Software - Test Bed
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
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			bool abort = false;


			// Check if we've already started the Test Bed
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex appMutex = new Mutex(true, "SugarCube_TestBed", out isFirstInstance)) {
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

				// Make sure not other software that communicates with the cube is running, if it is, abort, else start the app
				bool noOtherCubeUserIsRunning;
				using (Mutex cubeMutex = new Mutex(true, "SugarCube_Interactor", out noOtherCubeUserIsRunning)) {
					// Check if we're going to be using logging
					string loggingConfigFile = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3DWares"), "testbed.lc.xml");
					if (File.Exists(loggingConfigFile)) {
						XmlConfigurator.Configure(new FileInfo(loggingConfigFile));
					}
					
					if (!noOtherCubeUserIsRunning) {
						if (log.IsErrorEnabled) {
						    log.Error("The Test Bed can NOT be run while the batch runner or manager are open");
						}
						
						MessageBox.Show("The Test Bed can NOT be run while the batch runner or manager are open", "UNABLE TO START TEST BED",
											 MessageBoxButtons.OK, MessageBoxIcon.Error);
						abort = true;
					}


					if (abort) {
						return;
					}

					if (!SecurityManager.PromptForAndCheckPassword()) {
						if (log.IsWarnEnabled) {
						    log.Warn("User entered invalid password, launch aborted");
						}
						
						
						MessageBox.Show("Invalid password, you do not have permission to run this application.");
						return;
					}

					try {
						Application.Run(new MainForm());
					} catch (Exception ex) {
						if (log.IsFatalEnabled) {
						    log.Fatal("Fatal Error causing application to abort", ex);
						}
						
						ErrorDialog dlg = new ErrorDialog("Test Bed", "Something has gone very wrong.\nSo Long and Thanks for All the Fish!\n" + ex.Message, "The TestBed will now go poof!", ex.StackTrace);
						dlg.ShowDialog();
					}
				} // release cubeMutex
			} //release appMutex
		}
		
		/// <summary>
		/// Event handler for any uncaught exceptions.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			Exception ex = (Exception)e.ExceptionObject;
			if (ex.Message.ToLower().Contains("safe handle has been closed")) {
				// this means that the underlying COM stgream used for communicating with the Arduino has hiccuped.  We just need to trap
				// and log this here, we will deal with the actual re-opening of the port in the cube library
				if (log.IsErrorEnabled) {
				    log.Error("COM port closed on us unexpectedly", ex);
				}
				SugarCubeFlags.ComPortErrorSet = true;
			} else {
				MessageBox.Show("Please contact 3DWares with the following information:\n\n" + ex.Message + ex.StackTrace,
									 "Uncaught Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				Application.Exit();
			}
		}
	}
	
}
