/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// A simple wrappere call for parsing messages returned from the sugar cube.
	///
	/// Note: This class assumes that the cube has verbose set to off
	/// </summary>
	public class SugarCubeMessage {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(SugarCubeMessage));
		
		/// <summary>
		/// The numeric return code of the message, anything != 0 is a problem
		/// </summary>
		private int _returnCode = -1;
		/// <summary>
		/// The human readable portion of the response
		/// </summary>
		private string _message = "";
		/// <summary>
		/// If there is any additional data in the response, that is stashed here
		/// </summary>
		private string _data = "";

		/// <summary>
		/// Take a raw message and parse it out.
		/// If there's a problem parsing the message, the return code will be -1, the error will be in _message
		/// and the raw input in _data
		/// </summary>
		/// <param name="rawInput">The raw string data from the sugarcube</param>
		public SugarCubeMessage(string rawInput) {
			if (log.IsInfoEnabled) {
			    log.Info("Parsing message from cube: " + rawInput);
			}
			
			try {
				string[] buffer = rawInput.Split(new Char[] {':'});

				if (buffer.Length < 2) {
					if (log.IsErrorEnabled) {
					    log.Error("cube message is not formatted as expected");
					}
					
					throw new Exception("SugarCube message not in the expected format");
				}

				try {
					_returnCode = Convert.ToInt16(buffer[0]);
				} catch {
					if (log.IsErrorEnabled) {
					    log.Error("The first parameter in the SugarCube message is not an integer: " + buffer[0]);
					}
					
					throw new Exception("The first parameter in the SugarCube message must be an integer");
				}

				_message = buffer[1].Trim();

				if (buffer.Length == 3) {
					_data = buffer[2].Trim();
				}
			} catch (Exception ex) {
				_message = "Unable to parse SugarCubeMessage, " + ex.Message;
				_data = rawInput;
			}
		}

		/// <summary>
		/// Accessor for <see cref="_returnCode"/>
		/// </summary>
		public int ReturnCode {
			get {
				return _returnCode;
			}
		}

		/// Accessor for <see cref="_message"/>
		public string Message {
			get {
				return _message;
			}
		}

		/// Accessor for <see cref="_data"/>
		public string Data {
			get {
				return _data;
			}
		}
	}
}
