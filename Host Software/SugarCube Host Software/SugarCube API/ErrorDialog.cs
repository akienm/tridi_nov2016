/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// Provides a neatly formatted dialog that we can use to display Fatal, as in the whole thing goes boom!
	/// errors to the user in a way that allows them to easily understand the problem and copy the details down
	/// for the dev team
	/// </summary>
	public partial class ErrorDialog : Form {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(ErrorDialog));
		
		/// <summary>
		/// A flag to show if we are displaying the user-friendly error message or the stack trace
		/// </summary>
		bool _stackMode = false;
		/// <summary>
		/// The user friendly error message we display
		/// </summary>
		string _userMessage;
		/// <summary>
		/// The stack trace
		/// </summary>
		string _stackTrace;

		/// <summary>
		/// Constructor, build the dialog's text (without a stack trace)
		/// </summary>
		/// <param name="applicationName">The name of the application generating dialog</param>
		/// <param name="errorMessage">A nice user friendly error message</param>
		/// <param name="nextStepsMessage">A string explaining what the user must do next, or what will happend next</param>
		public ErrorDialog(string applicationName, string errorMessage, string nextStepsMessage) :
		this(applicationName, errorMessage, nextStepsMessage, "NO STACK TRACE")	{}

		/// <summary>
		/// Constructor, build the dialog's text
		/// </summary>
		/// <param name="applicationName">The name of the application generating dialog</param>
		/// <param name="errorMessage">A nice user friendly error message</param>
		/// <param name="nextStepsMessage">A string explaining what the user must do next, or what will happend next</param>
		/// <param name="stackTrace">The stack trace</param>
		public ErrorDialog(string applicationName, string errorMessage, string nextStepsMessage, string stackTrace) {
			InitializeComponent();
			_userMessage = String.Format(lMessage.Text, applicationName, errorMessage, nextStepsMessage);
			_stackTrace = stackTrace;
			if (log.IsDebugEnabled) {
			    log.Debug("Creating new error dialog with userMessage of " + _userMessage + " and stackTrace of " + _stackTrace);
			}
			
			lMessage.Text = _userMessage;
		}

		/// <summary>
		/// Close the dialog when the user click OK
		/// </summary>
		/// <param name="sender">The control calling the event handler</param>
		/// <param name="e">The click event parameters</param>
		void BtnOKClick(object sender, EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Toggle between the user friendly error message and the stack trace
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void LMessageDoubleClick(object sender, EventArgs e) {
			if (_stackMode) {
				lMessage.Text = _userMessage;
			} else {
				lMessage.Text = _stackTrace;
			}

			_stackMode = !_stackMode;
		}
	}
}
