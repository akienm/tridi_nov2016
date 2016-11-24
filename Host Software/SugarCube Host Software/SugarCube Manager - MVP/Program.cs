/* SugarCube Host Software - Manager MVP
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
		/// A reference to our splash screen, so we can easily close it from the subscribed event handler
		/// </summary>
		public static SplashScreen splashScreen = null;

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

		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args) {
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool abort = false;
			// Check if we've already started the Manager
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex appMutex = new Mutex(true, "SugarCube_Manager", out isFirstInstance)) {
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

				bool noOtherCubeUserIsRunning;
				// Make sure not other software that communicates with the cube is running, if it is, abort
				using (Mutex cubeMutex = new Mutex(true, "SugarCube_Interactor", out noOtherCubeUserIsRunning)) {
					// Check if we're going to be using logging
					string loggingConfigFile = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3DWares"), "manager.lc.xml");
					if (File.Exists(loggingConfigFile)) {
						XmlConfigurator.Configure(new FileInfo(loggingConfigFile));
					}

					if (!noOtherCubeUserIsRunning) {
						if (log.IsErrorEnabled) {
							log.Error("The manager can not run if the test bed or test bed batch runner are open");
						}

						MessageBox.Show("The Manager can NOT be run while the test bed or batch runner are open", "UNABLE TO START TEST BED BATCH RUNNER",
											 MessageBoxButtons.OK, MessageBoxIcon.Error);
						abort = true;
					}


					if (abort) {
						return;
					}

					//show splash
					if (log.IsInfoEnabled) {
						log.Info("Starting splash screen");
					}

					Thread splashThread = new Thread(new ThreadStart(
					delegate {
						splashScreen = new SplashScreen();
						Application.Run(splashScreen);
					}));

					splashThread.SetApartmentState(ApartmentState.STA);
					splashThread.Start();

					MainForm theForm = null;
					try {
						theForm = new MainForm();
						theForm.Load += new EventHandler(OnMainFormLoaded);
						Application.Run(theForm);
					} catch (Exception ex) {
						if (log.IsErrorEnabled) {
							log.Error("Fatal error while initializing the main form", ex);
						}

						ErrorDialog dlg = new ErrorDialog("SugarCube Manager", "A fatal error has occured while trying to initialize the application", "The SugarCube Manager will now close", ex.StackTrace);
						dlg.ShowDialog();
					}
					if (splashScreen != null) {
						CloseSplashScreen();
					}
				} // release the cubeMutex
			} //release the appMutex

		}

		static void CloseSplashScreen() {
			if (log.IsInfoEnabled) {
				log.Info("Closing splash screen");
			}

			splashScreen.Invoke(new Action(splashScreen.Close));
			splashScreen.Dispose();
			splashScreen = null;
		}

		/// <summary>
		/// Event handler to close the splash screen once the main form has completed loading
		/// </summary>
		static void OnMainFormLoaded(object sender, EventArgs e) {
			//close splash
			if (splashScreen == null) {
				return;
			}

			if (log.IsInfoEnabled) {
				log.Info("Main form has loaded, calling close splash screen");
			}

			CloseSplashScreen();
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
