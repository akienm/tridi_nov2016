/* SugarCube Host Software - SugarCube Uploader
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

using log4net;
using log4net.Config;

namespace Me.ThreeDWares.SugarCube {
	public sealed class NotificationIcon: IShowUploaderWindow {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(NotificationIcon));
		
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		private static UploadForm uploaderForm;
		
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

		#region Initialize icon and menu
		public NotificationIcon() {
			if (log.IsInfoEnabled) {
			    log.Info("Creating new NotificationIcon");
			}
			
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());

			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			notifyIcon.Icon = (Icon)resources.GetObject("$this.Icon");
			notifyIcon.ContextMenu = notificationMenu;
			notifyIcon.DoubleClick += new System.EventHandler(menuShowStatusClick);
			uploaderForm = new UploadForm(notifyIcon);
			uploaderForm.Closed += UploadFormClosed;
		}

		private MenuItem[] InitializeMenu() {
			if (log.IsInfoEnabled) {
			    log.Info("Initializing notification icon menu");
			}
			
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("Show Upload Status", menuShowStatusClick),
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		#endregion

		#region Main - Program entry point
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

		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		[STAThread]
		public static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, "SugarCube_Uploader", out isFirstInstance)) {
				if (isFirstInstance) {
					// Check if we're going to be using logging
					string loggingConfigFile = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3DWares"), "uploader.lc.xml");
					if (File.Exists(loggingConfigFile)) {
						XmlConfigurator.Configure(new FileInfo(loggingConfigFile));
					}

					if (log.IsInfoEnabled) {
					    log.Info("Starting Uploader, no other instances of this application are running");
					}
					
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
					// The following three lines configure the uploader to receive ShowUploaderWindow messages from our other apps
					ServiceHost host = new ServiceHost(typeof(NotificationIcon), new Uri[]{new Uri("net.pipe://localhost")});
        			host.AddServiceEndpoint(typeof(IShowUploaderWindow), new NetNamedPipeBinding(), "PipeShowUploader");
					host.Open();      
					if (log.IsDebugEnabled) {
					    log.Debug("Configured uploader to listen on the pipe for show uploader requests");
					}
										
					Application.Run();
					// Time to close the IPC endpoint and dispose of the pipe
					host.Close();
					notificationIcon.notifyIcon.Dispose();
				} else {
					// The application is already running, so let's find its process id
					Process me = Process.GetCurrentProcess();
					foreach (Process process in Process.GetProcessesByName (me.ProcessName)) {
						if (process.Id != me.Id) {
							// found it, let's switch over and then exit
							Activate(process.MainWindowHandle);
							break;
						}
					}
				}
			} // releases the Mutex
		}
		#endregion

		#region Event Handlers
		private void UploadFormClosed(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("Exiting application");
			}
			
			Application.Exit();
		}

		private void menuShowStatusClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("User clicked on Show Status menu item");
			}
			
			if (uploaderForm.Visible) {
				if (log.IsDebugEnabled) {
				    log.Debug("Uploader window already visible");
				}
				return;
			}
			if (log.IsDebugEnabled) {
			    log.Debug("Showing uploader window");
			}
			
			uploaderForm.Show();
		}

		private void menuAboutClick(object sender, EventArgs e) {
			MessageBox.Show("SugarCube Upload Manager");
		}

		private void menuExitClick(object sender, EventArgs e) {
			if (log.IsInfoEnabled) {
			    log.Info("User selected exit menu item");
			}
			
			bool uploaderFormWasShowing = false;
			// If hte uplaoder form is visible, make sure we hide it so that it doesn't obscure our exit dialog
			if (uploaderForm.Visible) {
				uploaderFormWasShowing = true;
				uploaderForm.Hide();
			}
			
			DialogResult res = MessageBox.Show("If you exit the uploader any currently running uploads will be paused and will resume the next time the uploader is started.\n\nAre you sure you want to exit?",
			                                   "SugarCube Scan Uploader", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (res == DialogResult.Yes) {
				if (log.IsDebugEnabled) {
				    log.Debug("User chose to exit");
				}
				
				if (uploaderForm.worker.IsBusy) {
					if (log.IsDebugEnabled) {
					    log.Debug("Worker is busy, sending cancellation request");
					}
					
					uploaderForm.worker.CancelAsync();
				}
				
				while (uploaderForm.worker.IsBusy) {
					if (log.IsDebugEnabled) {
					    log.Debug("Worker reports busy");
					}
					
					Thread.Sleep(100);
				}
				
				if (log.IsDebugEnabled) {
				    log.Debug("Worker is done, closing upload form");
				}
				
				uploaderForm.Close();
				
				if (log.IsDebugEnabled) {
				    log.Debug("Uploader form closed, now exiting");
				}
				
				Application.Exit();
			} else { //user chose not to exit
				if (uploaderFormWasShowing) {
					uploaderForm.Show();
				}
			}
		}
		
		public void ShowUploaderWindow() {
			if (log.IsInfoEnabled) {
			    log.Info("Received a call on the pipe to show uplaoder window");
			}
			
			menuShowStatusClick(null, null);
		}
		#endregion
	}
}
