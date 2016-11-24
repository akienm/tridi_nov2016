/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// This class provides us a globally accesible set of flags we can use to communicate status of the
	/// underlying COM port from our global uncaught error handler back to the SugarCube code
	/// </summary>
	public static class SugarCubeFlags {
		/// <summary>
		/// True if there has been an error on the COM port (usually a 'safe handle has been closed') error.
		/// False otherwise
		/// </summary>
		public static bool ComPortErrorSet = false;
	}
	
	/// <summary>
	/// This class handles all communications with the actual SugarCube hardware itself.
	/// </summary>
	/// <remarks>
	/// This class requires you to use a background worker to actually process the shoot strings
	/// </remarks>
	public class SugarCube {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(SugarCube));
		
		#region Class Data
		/// <summary>
		/// The actual serial port we are communicating on
		/// </summary>
		private SerialPort _port = null;

		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the
		/// event data contains a status message
		/// </summary>
		public const int REPORT_STATUS_FLAG = -101;
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the main
		/// thread should capture an image from the cam stream
		/// </summary>
		public const int TAKE_SNAPSHOT_FLAG = -102;
		/// <summary>
		/// FLAG used when reporting status via a background worker to indicate that the main
		/// thread should temporarily disable motion detection for the next image
		/// </summary>
		public const int DISABLE_MOTION_FLAG = -103;

		/// <summary>
		/// Has the cube port been initialized?
		/// </summary>
		public bool Initialized = false;
		/// <summary>
		/// The current rotation of the cube
		/// </summary>
		public double Rotation = 0;
		/// <summary>
		/// The current elevation of the cube
		/// </summary>
		public double Elevation = 0;
		/// <summary>
		/// The name of the port we are communicating on
		/// </summary>
		private string _portName;
		/// <summary>
		/// Stores the last raw string that we received from the cube
		/// </summary>
		private string _lastRead = "";
		#endregion

		#region Port Initialization and Shutdown
		/// <summary>
		/// Attempts to locate the cube by iterating through all COM* ports on the system, sending out the string
		/// "c\n" and looking for a valid cube CheckReady response
		/// </summary>
		/// <returns>The name of the port the cube is located on, an empty string if the cube is not found</returns>
		private string LocateSugarCubePort() {
			if (log.IsInfoEnabled) {
			    log.Info("Locating cube port");
			}
			
			string[] ports = SerialPort.GetPortNames();
			string thePort = "";
			if (log.IsDebugEnabled) {
			    log.Debug("There are " + ports.Length + " ports to check");
			}
			
			foreach(string portname in ports) {
				if (!portname.StartsWith("com", StringComparison.OrdinalIgnoreCase)) {
					continue;
				}
				Debug.WriteLine("Checking " + portname, "LocateSugarCubePort");
				try {
					if (log.IsDebugEnabled) {
					    log.Debug("Checking port " + portname);
					}
					
					SerialPort port = new SerialPort(portname);
					port.BaudRate = 9600;
					port.Open();
					port.ReadTimeout = 15000; //give it 15 seconds, in case the sugarcube is booting
					port.Write("c");
					System.Threading.Thread.Sleep(500);
					string rawMessage = port.ReadLine();
					if (log.IsDebugEnabled) {
					    log.Debug("raw response from port: " + rawMessage);
					}
					
					SugarCubeMessage response = new SugarCubeMessage(rawMessage);
					if (response.Message.Contains("CheckReady")) { //at this time, don't care about the CheckReady status
						if (log.IsDebugEnabled) {
						    log.Debug("Found cube on port " + portname);
						}
						
						thePort = portname;
						// see http://stackoverflow.com/questions/7113909/objectdisposedexecption-after-closing-a-net-serialport for why we use close, dispose and set to null
						port.Close();
						port.Dispose();
						port = null;
						break;
					}
					port.Close();
				} catch (Exception ex) {
					if (log.IsDebugEnabled) {
					    log.Debug("Not this one: " + ex.Message);
					}
					
				}
			}
			if (thePort == "") {
				if (log.IsErrorEnabled) {
				    log.Error("SugarCube not found!");
				}
				
				throw new Exception("SugarCube could not be located on any available COM port");
			}
			return thePort;
		}

		/// <summary>
		/// Sets up the port defined in _portName for use by the API.  The COM port settings used for the
		/// cube are:
		/// <list type="bullet">
		/// <item><term>BaudRate</term> <description>9600</description></item>
		/// <item><term>Parity</term> <description>Parity.None</description></item>
		/// <item><term>StopBits</term> <description>StopBits.One</description></item>
		/// <item><term>DataBits</term> <description>8</description></item>
		/// <item><term>ReadTimeout</term> <description>15000</description></item>
		/// </list>
		///
		/// NOTE: The function does not checking to see if the port exists or can be opened.
		///
		/// As a side effect, this method sets Initialized to true
		/// </summary>
		private void SetUpPort() {
			if (log.IsInfoEnabled) {
			    log.Info("Setting up port " + _portName + " for the cube");
			}
			
			_port = new SerialPort(_portName);
			_port.BaudRate = 9600;
			_port.Parity = Parity.None;
			_port.StopBits = StopBits.One;
			_port.DataBits = 8;
			_port.ReadTimeout = 15000;
			_port.NewLine = "\n";
			Initialized = true;
			_port.Open();
		}

		/// <summary>
		/// Initialize the cube by finding the port, using <see cref="LocateSugarCubePort"/> to get the port name.
		/// </summary>
		public void Initialize() {
			SugarCubeFlags.ComPortErrorSet = false;
			if (log.IsInfoEnabled) {
			    log.Info("Initializing the cube by locating the correct port");
			}
			
			Close();
			try {
				_portName = LocateSugarCubePort();
				SetUpPort();
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to initialize cube: " + ex.Message);
				}
				throw new Exception("Unable to initialize SugarCube: " + ex.Message);
			}
		}

		/// <summary>
		/// Initialize the cube using the port name passed in.
		/// </summary>
		/// <param name="port">The name of the com port to use</param>
		public void Initialize(string port) {
			SugarCubeFlags.ComPortErrorSet = false;
			if (log.IsInfoEnabled) {
			    log.Info("Initializing the cube using port " + port);
			}
			Close();
			if (Array.IndexOf(SerialPort.GetPortNames(), port) < 0) {
				if (log.IsErrorEnabled) {
				    log.Error("The specified port does not exist: " + port);
				}
				
				throw new Exception("Unable to initialize SugarCube, port " + port + " does not exist");
			}
			_portName = port;
			try {
				SetUpPort();
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to initialize cube: " + ex.Message);
				}
				throw new Exception("Unable to initialize SugarCube: " + ex.Message);
			}
		}

		/// <summary>
		/// Close the port, if it's open
		/// </summary>
		public void Close() {
			if (log.IsInfoEnabled) {
			    log.Info("Attempting to close port");
			}
			
			if (_port != null) {
				if (log.IsDebugEnabled) {
				    log.Debug("Port object existed");
				}
				
				// see http://stackoverflow.com/questions/7113909/objectdisposedexecption-after-closing-a-net-serialport for why we are using close, dispose and setting to null
				if (_port.IsOpen) {
					_port.Close();
				}
				_port.Dispose();
				_port = null;
				Initialized = false;
			}
		}

		/// <summary>
		/// Get name of the port we are communicating on
		/// </summary>
		public string PortName {
			get {
				return _portName;
			}
		}

		#endregion

		#region Communicating with the cube
		/// <summary>
		/// Sends a command to the cube and reads the response, as a <see cref="SugarCubeMessage"/> using
		/// <see cref="ReadMessage"/>/
		/// </summary>
		/// <param name="command">A string to send to the cube</param>
		/// <returns>A string consisting of the return code and message from the cube</returns>
		public string PostCommand(string command) {
			if (log.IsInfoEnabled) {
			    log.Info("Posting command to cube: " + command);
			}
			
			if (SugarCubeFlags.ComPortErrorSet || _port == null || !_port.IsOpen) {
				if (log.IsInfoEnabled) {
				    log.Info("Needed to reopen COM port before posting command");
				}
				
				Initialize(_portName);
			}
			_port.WriteLine(command);
			SugarCubeMessage message = ReadMessage();
			string result = message.ReturnCode + ": " + message.Message;
			if (!String.IsNullOrWhiteSpace(message.Data)) {
				result = result + " - " + message.Data;
			}
			return result;
		}

		/// <summary>
		/// Read a message from the cube.
		/// </summary>
		/// <returns>The read message, as a <see cref="SugarCubeMessage"/>.  If an error occured duringt the read, the error details
		/// encapsulated in a SugarCubeMessage that has a return code of -1</returns>
		private SugarCubeMessage ReadMessage() {
			if (log.IsInfoEnabled) {
			    log.Info("Reading message from cube");
			}
			
			//_port.DiscardInBuffer();
			string message = "";
			try {
				if (SugarCubeFlags.ComPortErrorSet || _port == null || !_port.IsOpen) {
					if (log.IsInfoEnabled) {
					    log.Info("Needed to reopen port before reading message");
					}
					
					Initialize(_portName);
				}
				message = _port.ReadLine();
				_lastRead = message;
				if (log.IsDebugEnabled) {
				    log.Debug("message read was: " + message);
				}
				
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("error reading message from cube: " + ex.Message);
				}
				message = "-1:Error Reading Cube Port:" + ex.Message;
			}
			return new SugarCubeMessage(message);
		}

		/// <summary>
		/// Clear any data currently queued up on the port
		/// </summary>
		private void ClearPort() {
			if (log.IsInfoEnabled) {
			    log.Info("Clearing port's in buffer");
			}
			
			_port.DiscardInBuffer();
		}
		#endregion

		#region Z String Processing
		/// <summary>
		/// Take a string of cube parameters from EEPROM and parse it out in to a StringDictionary
		/// The key/value pairs are seperated by semi-colons, the keys are seperated from the values
		/// by an equals sign
		/// </summary>
		/// <param name="parameters">The string containing the paramters to parse</param>
		/// <returns>A string dictionary of the key/value pairs</returns>
		private StringDictionary ParseParametersString(string parameters) {
			if (log.IsInfoEnabled) {
			    log.Info("Parsing Z parameterr string: " + parameters);
			}
			
			StringDictionary result = new StringDictionary();
			string[] pairs = parameters.Split(';');
			if (pairs.Length == 0) { // we have no stored parameters
				if (log.IsInfoEnabled) {
				    log.Info("No stored parameters found");
				}
				
				return result;
			}
			if (log.IsDebugEnabled) {
			    log.Debug("Found " + pairs.Length + " parameters");
			}
			
			foreach (string pair in pairs) {
				string[] details = pair.Split('=');
				if (details.Length != 2) { //the stored parameter is not in the expected format
					if (log.IsDebugEnabled) {
					    log.Debug("Could not parse parameter " + pair);
					}
					
					continue;
				}
				result.Add(details[0], details[1]);
			}
			return result;
		}

		/// <summary>
		/// Get only parameter, gets the values stored in EEPROM.  Will throw an exception if called before the
		/// port is initialized
		/// </summary>
		public StringDictionary CubeParameters {
			get {
				if (log.IsInfoEnabled) {
				    log.Info("Getting stored Z parameters");
				}
				
				if (!Initialized) {
					if (log.IsErrorEnabled) {
					    log.Error("GET CubeParameters called before cube is initialized");
					}
					throw new Exception("SugarCube has not yet been initialized");
				}

				if (SugarCubeFlags.ComPortErrorSet || _port == null || !_port.IsOpen) {
					if (log.IsInfoEnabled) {
					    log.Info("Neeed to reopen port before reading z values");
					}
					
					Initialize(_portName);
				}
				
				_port.WriteLine("z");
				SugarCubeMessage result = ReadMessage();
				if (result.ReturnCode != 0) {
					throw new Exception(String.Format("Unable to get SugarCube parameters, return code was {0}, details: {1}",
																 result.ReturnCode, result.Message));
				}
				return ParseParametersString(result.Data);
			}

		}

		/// <summary>
		/// Store a new string in to the EEPROM
		/// </summary>
		/// <param name="parameters">The new string to store in to the EEPROM</param>
		/// <returns>As a safety check, calls CubeParameters to make sure the store worked</returns>
		public StringDictionary SetCubeParameters(string parameters) {
			if (log.IsInfoEnabled) {
			    log.Info("Setting Z string to " + parameters);
			}
			
			if (!Initialized) {
				if (log.IsErrorEnabled) {
				    log.Error("SetCubeParameters called before cube is initialized");
				}
				throw new Exception("SugarCube has not yet been initialized");
			}
			// Must make absolutely sure to end the Z write with a newline 
			if (SugarCubeFlags.ComPortErrorSet || _port == null || !_port.IsOpen) {
				if (log.IsInfoEnabled) {
				    log.Info("Needed to reopen port before writing Z values");
				}
				
				Initialize(_portName);
			}
			
			_port.WriteLine("Z" + parameters + "\n");
			SugarCubeMessage result = ReadMessage();
			if (result.ReturnCode != 0) {
				throw new Exception(String.Format("Unable to set SugarCube parameters, return code was {0}, details: {1}",
															 result.ReturnCode, result.Message));
			}
			return CubeParameters;
		}


		#endregion

		#region Shoot String Processing
		/// <summary>
		/// Get the raw string that was last read from the Cube
		/// </summary>
		public string LastRead {
			get {
				return _lastRead;
			}
		}


		/// <summary>
		/// Process a simple repeat loop, by copying the set of items to be repeated into our command list the specified
		/// number of times
		/// </summary>
		/// <param name="worker">The BackgroundWorker processing our shoot string</param>
		/// <param name="idx">The current position in our command list where we encountered the repeat loop</param>
		/// <param name="arg">The argument to our repeat loop command, the number of times to repeat the loop</param>
		/// <param name="commands">A reference to our list of shoot commands to process</param>
		private void ProcessRepeatLoop(BackgroundWorker worker, int idx, string arg, ref List<string> commands) {
			if (log.IsInfoEnabled) {
			    log.Info("Processing repeat loop");
			}
			
			int repeats = 0;
			try {
				repeats = Convert.ToInt16(arg);
			} catch  {
				if (log.IsErrorEnabled) {
				    log.Error("Repeat argument error, " + arg + " must be a positive integer");
				}
				worker.ReportProgress(REPORT_STATUS_FLAG, " * " + arg + " must be a positive integer" + Environment.NewLine);
				throw new ArgumentException(arg + " must be a positive integer");
			}
			if (repeats <= 0) {
				if (log.IsErrorEnabled) {
				    log.Error("Repeat argument error, " + arg + " must be greater then 0");
				}
				worker.ReportProgress(REPORT_STATUS_FLAG, " * " + arg + " must be an integer > 0" + Environment.NewLine);
				throw new ArgumentException(arg + " must be an integer > 0");
			}
			if (repeats == 1) {
				worker.ReportProgress(REPORT_STATUS_FLAG, " - there is really no point in having a loop with a repeat count of 1, is there now" + Environment.NewLine);
				return;
			}
			repeats = repeats - 1; //to account for the first copy that's already there
			// find the location of the end parenth
			int loopEnd = commands.FindIndex(idx, delegate (string str) {
				return str == ")";
			}) - 1;
			if (loopEnd == -2) {
				if (log.IsErrorEnabled) {
				    log.Error("No loop end was foind, ignoring loop");
				}
				
				worker.ReportProgress(REPORT_STATUS_FLAG, " * no closing parentheses was found, ignoring loop" + Environment.NewLine);
				return;
			}
			List<string> loopedCommands = commands.GetRange(idx + 1, loopEnd - idx);
			for (int i = 1; i <= repeats; i++) {
				commands.InsertRange(idx + 1, loopedCommands);
			}
			if (log.IsInfoEnabled) {
				log.Info("Total repeat count is " + (repeats + 1));
			}
			
			worker.ReportProgress(REPORT_STATUS_FLAG, " - begin loop, repeating " + (repeats + 1) + " times" + Environment.NewLine);
		}

		/// <summary>
		/// A helper function for <see cref="ProcessForLoop"/>, it updates the * token in a copy of our repeated commands
		/// </summary>
		/// <param name="copiedCommands">The list of commands we have to update </param>
		/// <param name="newValue">The new numeric value to use when replacing the * token in our copied commands</param>
		/// <returns>The copied list of commands with the new numeric value inserted in to them</returns>
		private List<string> UpdateCounter(List<string> copiedCommands, int newValue) {
			if (log.IsInfoEnabled) {
			    log.Info("Call to UpdateCounter with newValue = " + newValue);
			}
			
			List<string> copy = new List<string>(copiedCommands);
			for (int i =0; i < copy.Count; i++) {
				copy[i] = copy[i].Replace("*", newValue.ToString());
			}
			return copy;
		}

		/// <summary>
		/// Processes a for loop. Our for loops are fairly rigid, you have to specify a starting integer, an ending integer and a
		/// integer increment.  The code will automatically repeat the commands enclosed in the for loop the required number of times
		/// by inserting them in to the command list. All integers must be positive and start must be less then end
		///
		/// Each time the code is repeated, any occurences of the * token will be replaced with the calculated numerical value for that
		/// repeat.
		/// </summary>
		/// <param name="worker">The BackgroundWorker processing our shoot string</param>
		/// <param name="idx">The current position in our command list where we encountered the for loop</param>
		/// <param name="arg">The argument to our for loop command, the start value, the end value and the increment, all seperated
		/// by commas</param>
		/// <param name="commands">A reference to our list of shoot commands to process</param>
		private void ProcessForLoop(BackgroundWorker worker, int idx, string arg, ref List<string> commands) {
			if (log.IsInfoEnabled) {
			    log.Info("Processing For loop");
			}
			
			string[] args = arg.Split(new char[] {','});
			if (args.Length != 3) {
				if (log.IsErrorEnabled) {
				    log.Error(arg + " is not a valid set of For loop parameters, there must be three parameters");
				}
				worker.ReportProgress(REPORT_STATUS_FLAG, " * " + arg + " is not a valid set of loop parameters" + Environment.NewLine);
				throw new ArgumentException(arg + " is not a valid set of loop parameters, there must be three parameters");
			}
			int start;
			int end;
			int increment;
			try {
				start = Convert.ToInt32(args[0]);
				end = Convert.ToInt32(args[1]);
				increment = Convert.ToInt32(args[2]);
			} catch {
				if (log.IsErrorEnabled) {
				    log.Error(arg + " is not a valid set of FOR loop parameters, all values must be positive integers");
				}
				worker.ReportProgress(REPORT_STATUS_FLAG, " * " + arg + " is not a valid set of loop parameters" + Environment.NewLine);
				throw new ArgumentException(arg + " is not a valid set of loop parameters, all values must be positive integers");
			}
			if (start > end) {
				if (log.IsErrorEnabled) {
				    log.Error("The starting FOR loop value must be smaller than the ending value");
				}
				throw new ArgumentException("The starting loop value must be smaller than the ending value");
			}
			if ((start > end) || (increment < 0)) {
				if (log.IsErrorEnabled) {
				    log.Error("The FOR loop does not support negative increments");
				}
				throw new ArgumentException("The loop does not support negative increments");
			}

			if (log.IsDebugEnabled) {
				log.DebugFormat("FOR args: start = {0}, end = {1}, increment = {2}", start, end, increment);
			}
			
			// find the location of the end parenthesis
			int loopEnd = commands.FindIndex(idx, delegate (string str) {
				return str == "}";
			}) - 1;
			if (loopEnd == -2) {
				if (log.IsErrorEnabled) {
				    log.Error("FOR oop error, no closing parenthesis was found, ignoring loop");
				}
				worker.ReportProgress(REPORT_STATUS_FLAG, " * no closing parenthesis was found, ignoring loop" + Environment.NewLine);
				return;
			}
			List<string> commandTemplate = commands.GetRange(idx + 1, loopEnd - idx);
			commands.RemoveRange(idx + 1, loopEnd - idx); // remove the template
			idx++;
			for (int i = start; i <= end; i=i+increment) {
				commands.InsertRange(idx, UpdateCounter(commandTemplate, i));
				idx = idx + commandTemplate.Count;
			}
			worker.ReportProgress(REPORT_STATUS_FLAG, String.Format(" - begin loop, iterating from {0} to {1} by {2}\n",start, end, increment));

		}

		/// <summary>
		/// Actually sends a command to the cube and waits for a response
		/// </summary>
		/// <param name="worker">The BackgroundWorker processing our shoot string</param>
		/// <param name="command">The full string command to send to the cube</param>
		private SugarCubeMessage SendCommandToCube(BackgroundWorker worker, string command) {
			if (log.IsInfoEnabled) {
			    log.Info("Preparing to send command " + command + " to cube");
			}
			
			if (SugarCubeFlags.ComPortErrorSet || _port == null || !_port.IsOpen) {
				if (log.IsInfoEnabled) {
				    log.Info("Needed to open port before sending command in sendcommand");
				}
				
				Initialize(_portName);
			}
			ClearPort();
			_port.Write(command);
			if (_port == null || !_port.IsOpen) {
				if (log.IsInfoEnabled) {
				    log.Info("Needed to open the port before reading result in sendcommand");
				}
				
				Initialize(_portName);
			}
			SugarCubeMessage cubeMessage = ReadMessage();
			if (cubeMessage.ReturnCode < 0) {
				if (log.IsErrorEnabled) {
				    log.Error("Error sending command to cube");
				}
				
				worker.ReportProgress(REPORT_STATUS_FLAG, String.Format("<** ERROR: {0} ({1})\n", cubeMessage.Message, cubeMessage.Data));
			} else {
				worker.ReportProgress(REPORT_STATUS_FLAG, String.Format("<-- {0}: {1}: {2}\n", cubeMessage.ReturnCode, cubeMessage.Message, cubeMessage.Data));
			}
			return cubeMessage;
		}

		/// <summary>
		/// Pauses the shoot string processing for waitTime number of seconds
		/// </summary>
		/// <param name="worker">The BackgroundWorker processing our shoot string</param>
		/// <param name="waitTime">The fractional seconds to pause</param>
		private void Pause(BackgroundWorker worker, string waitTime) {
			if (log.IsInfoEnabled) {
			    log.Info("Preparing to pause for " + waitTime);
			}
			
			double sleepTime = 0;
			try {
				sleepTime = Convert.ToDouble(waitTime);
			} catch  {
				if (log.IsErrorEnabled) {
				    log.Error(waitTime + " is not a valid pause time");
				}
				
				worker.ReportProgress(REPORT_STATUS_FLAG, " * " + waitTime + " is not a valid pause time" + Environment.NewLine);
				return;
			}
			worker.ReportProgress(REPORT_STATUS_FLAG, " - pausing " + waitTime + " seconds" + Environment.NewLine);
			System.Threading.Thread.Sleep(Convert.ToInt32(sleepTime * 1000)); // convert seconds to milliseconds
		}

		/// <summary>
		/// The heart of cube operation, this is where we process the shooting strings passed in to use.  This method must be
		/// run from a background worker thread.  The webcam runs in the parent thread so we need a way to communicate back to the parent
		/// thread when we want to take a picture.  We do this by using the Worker_ReportProgress event handler.  We set the
		/// percentProgress of the ReportProgress method to be one of our three _FLAG constants.  Based on that value, we can do
		/// interesting thing in the main thread.
		///
		/// There are three groups of shoot string commands we understand and can handle.  The first group are commands that are handeled by the
		/// host software, that is they never get passes to the Cube.  The next two sets both get passed to the Cube, but they can be grouped in
		/// to two categories: Commands and Queries.  Commands, which are all upper case, direct the cube to perform a specific action.  Queries,
		/// which are all lower case, request information from the cube.	All the commands are detailed in the two tables below. The first table
		/// covers commands that are handled by the host software, the second table is all the commands passed through to the cube.
		///
		/// <list type="table">
		/// <listheader>
		/// <term>Command</term>
		/// <term>Arguments</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item><term>P</term> <term>fractional seconds</term> <term>pause for x seconds</term></item>
		/// <item><term>T</term> <term></term> <term>take a picture</term></item>
		/// <item><term>(</term> <term>number of repeats</term> <term>start a repeat loop, repeat x number of times</term></item>
		/// <item><term>)</term> <term></term> <term>end of repeat loop</term></item>
		/// <item><term>{</term> <term>start number, end number, increment</term> <term>start a for loop, using * as the token for the numeric increment</term></item>
		/// <item><term>}</term> <term></term> <term>end of for loop</term></item>
		/// <item><term>~</term> <term></term> <term>for testing, disables motion detection for the next snapshot (if it is enabled)</term></item>
		/// </list>
		///
		/// <list type="table">
		/// <listheader>
		/// <term>Command</term>
		/// <term>Arguments</term>
		/// <term>Description</term>
		/// </listheader>
		/// <item><term>b</term> <term></term> <term>print the banner</term></item>
		/// <item><term>c</term> <term></term> <term>check if the cube is ready</term></item>
		/// <item><term>d</term> <term></term> <term>check door status</term></item>
		/// <item><term>E</term> <term>degrees of elevation</term> <term>elevate to x degrees from origin</term></item>
		/// <item><term>e</term> <term></term> <term>check elevation homed</term></item>
		/// <item><term>F</term> <term></term> <term>turn off the shooting lights</term></item>
		/// <item><term>H</term> <term></term> <term>home the motors</term></item>
		/// <item><term>I</term> <term>0 or 1</term> <term>turn shooting lights off (0) or on(1)</term></item>
		/// <item><term>L</term> <term>R,G,B with a number or O</term> <term>Set the LED</term></item>
		/// <item><term>N</term> <term></term> <term>turn on the shooting lights</term></item>
		/// <item><term>R</term> <term>degrees of rotation</term> <term>rotate to x degrees from origin</term></item>
		/// <item><term>r</term> <term></term> <term>check rotation homed</term></item>
		/// <item><term>S</term> <term></term> <term>reset cube</term></item>
		/// <item><term>t</term> <term></term> <term>check 12v status</term></item>
		/// <item><term>V</term> <term>0 or 1</term> <term>set verbose output off (0) or on (1)</term></item>
		/// <item><term>v</term> <term></term> <term>report the current cube version</term></item>
		/// <item><term>X</term> <term></term> <term>set cube variables</term></item>
		/// <item><term>x</term> <term></term> <term>report cube variables</term></item>
		/// <item><term>Z</term> <term>a string</term> <term>Store the string into the Arduino's EEPROM (command must be terminated by \n</term></item>
		/// <item><term>z</term> <term></term> <term>Retrieve the data stored in the Arduino's EEPROM and print it out</term></item>
		/// </list>
		/// </summary>
		/// <param name="shootString">The shoot string to process</param>
		/// <param name="worker">The BackgroundWorker processing our shoot string</param>
		/// <param name="e">The DoWorkEventArgs for the background worker, used to check if the user has cancelled the operation</param>
		/// <param name="synchronizer">A ManualResetEvent used to synchronize this thread with the parent thread when taking snapshots</param>
		public void ProcessShootString (string shootString, BackgroundWorker worker, DoWorkEventArgs e, ManualResetEvent synchronizer) {
			if (log.IsInfoEnabled) {
			    log.Info("Preparing to process shoot string");
			}
			if (log.IsDebugEnabled) {
			    log.Debug("Raw shoot string is " + shootString);
			}
			
			SugarCubeMessage result;
			Stopwatch timer = Stopwatch.StartNew();
			try {
				// Replace any \n with a space
				shootString = shootString.Replace(Environment.NewLine, " ");
				// Strip out the comments, enclosed between [ and ]
				shootString = Regex.Replace(shootString, @"\[[^\]]*\]", "");
				//Replace any stray end of comment characters, like the ones Akien tends to put in the front of the shoot string
				shootString = shootString.Replace("]", "");
				if (log.IsDebugEnabled) {
				    log.Debug("Processes shoot string is " + shootString);
				}
				List<string> commands = new List<string>();
				commands.AddRange(Regex.Split(shootString, @"\s"));
				if (log.IsDebugEnabled) {
				    log.Debug("Found " + commands.Count + " commands to process");
				}
				worker.ReportProgress(REPORT_STATUS_FLAG, "Processing " + commands.Count + " shoot string commands:" + Environment.NewLine);
				for (int idx = 0; idx < commands.Count; idx++) {
					// Check if the user chose to cancel
					if (worker.CancellationPending) {
						if (log.IsDebugEnabled) {
						    log.Debug("User cancelled shoot string processing");
						}
						SendCommandToCube(worker, "H"); //Reset the cube
						SendCommandToCube(worker, "S"); //Reset the cube
						e.Cancel = true;
						return;
					}
					if (String.IsNullOrWhiteSpace(commands[idx])) { //ignore empty strings created by using a Regex split
						continue;
					}
					string cmd = commands[idx].Substring(0,1);
					string arg = commands[idx].Length > 1 ? commands[idx].Substring(1) : "";
					if (log.IsDebugEnabled) {
						log.DebugFormat("shoot string command: idx = {0}, cmd = {1}, arg = {2}", idx, cmd, arg);
					}

					switch (cmd) {
					case "":
						//ignore empty commands
						break;
					// The commands processed by the host software
					case "P" :
						Pause(worker, arg);
						break;
					case "T" :
						// the cam control is in the parent thread so we can't interact with it directly
						// we need to use our synchronizer to synch up the two threads
						synchronizer.Reset();
						worker.ReportProgress(TAKE_SNAPSHOT_FLAG);
						synchronizer.WaitOne();
						break;
					case "(":
						ProcessRepeatLoop(worker, idx, arg, ref commands);
						break;
					case "{":
						ProcessForLoop(worker, idx, arg, ref commands);
						break;
					case ")":
					case "}":
						worker.ReportProgress(REPORT_STATUS_FLAG, " - end of loop, moving on" + Environment.NewLine);
						break;
					case "~" :
						worker.ReportProgress(DISABLE_MOTION_FLAG, " - disabling motion detection for the next snapshot" + Environment.NewLine);
						break;
					// The commands passed through to the cube
					case "b":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> printing cube banner" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "c":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> checking cube ready status" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "d":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> checking door status" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "E" :
						try {
							int tmp = Convert.ToInt16(arg);
						} catch {
							if (log.IsErrorEnabled) {
							    log.Error(arg + " is not a valid elevation");
							}
							
							worker.ReportProgress(REPORT_STATUS_FLAG, " * " + arg + " is not a valid elevation" + Environment.NewLine);
							break;
						}
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> setting SugarCube elevation to " + arg + Environment.NewLine);
						result = SendCommandToCube(worker, cmd + arg);
						Elevation = Convert.ToDouble(result.Data);
						break;
					case "e":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> checking if elevation motor is homed" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "F" :
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> turning off shoot lights in SugarCube" + Environment.NewLine);
						SendCommandToCube(worker,cmd);
						break;
					case "H":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> homing cube motors" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "I" :
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> setting shoot lights in SugarCube to " + arg + Environment.NewLine);
						SendCommandToCube(worker,cmd + arg);
						break;
					case "L" :
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> setting LEDs in SugarCube to " + arg + Environment.NewLine);
						SendCommandToCube(worker,cmd + arg);
						break;
					case "N" :
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> turning on shoot lights in SugarCube" + Environment.NewLine);
						SendCommandToCube(worker,cmd);
						break;
					case "R" :
						try {
							int tmp = Convert.ToInt16(arg);
						} catch {
							if (log.IsErrorEnabled) {
							    log.Error(arg + " is not a valid rotation");
							}
							
							worker.ReportProgress(REPORT_STATUS_FLAG, " * " + arg + " is not a valid rotation" + Environment.NewLine);
							break;
						}
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> setting SugarCube rotation to " + arg + Environment.NewLine);
						result = SendCommandToCube(worker, cmd + arg);
						Rotation = Convert.ToDouble(result.Data);
						break;
					case "r":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> checking if rotation motor is homed" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "S" :
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> resetting SugarCube" + Environment.NewLine);
						Rotation = 0;
						Elevation = 0;
						SendCommandToCube(worker,cmd);
						break;
					case "t":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> checking 12V status" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "V":
						try {
							int choice = Convert.ToInt16(arg);
						} catch {
							if (log.IsErrorEnabled) {
							    log.Error(arg + " is not a valid verbosity level");
							}
							
							worker.ReportProgress(REPORT_STATUS_FLAG, " * " + arg + " is not a valid verbosity" + Environment.NewLine);
							break;
						}
						SendCommandToCube(worker, cmd + arg);
						break;
					case "v" :
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> getting SugarCube version" + Environment.NewLine);
						SendCommandToCube(worker,cmd);
						break;
					case "X":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> setting cube parameters" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "x" :
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> getting cube parameters" + Environment.NewLine);
						SendCommandToCube(worker, cmd);
						break;
					case "Z":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> setting SugarCube EEPROM " + arg + Environment.NewLine);
						SendCommandToCube(worker,cmd + arg);
						break;
					case "z":
						worker.ReportProgress(REPORT_STATUS_FLAG, "--> getting SugarCube EEPROM " + Environment.NewLine);
						SendCommandToCube(worker,cmd);
						break;
					default :
						worker.ReportProgress(REPORT_STATUS_FLAG, " * I don't know what to do with " + cmd + arg + Environment.NewLine);
						break;
					}
				}
				timer.Stop();
				if (log.IsInfoEnabled) {
				    log.Info("Total time to process shoot string: " + timer.Elapsed);
				}
				
				worker.ReportProgress(REPORT_STATUS_FLAG, "Total elapsed time to process shoot string was " + timer.Elapsed + Environment.NewLine);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
					log.Error("Error while processing shoot string: " + ex.Message);
				}
				
				throw new Exception("Error while processing shoot string: " + ex.Message);
			}
		}
		#endregion



	}
}

#region Beta 2 Shoot String Handling, for historic reference only
/*
private void ProcessBeta2ShootString(BackgroundWorker worker, string shootString) {
/*
   * N = turn on lights, processed by Arduino
   * H = Home elevation motor, processed by Arduino
   * h = home rotation motor, processed by Arduino
   * P = pause for a user specified time, the command takes a float argument
   * D = Down elevation by ~10 degrees, processed by Arduino
   * U = Up elevation by ~10 degrees, processed by Arduino
   * T = Take a high resolution picture, -- the app needs to capture and save a high resolution photo now.
   * R = Rotate right by ~15 degrees, processed by Arduino
   * L = Rotate left by ~15 degrees, processed by Arduino
   * F = lights off, processed by Arduino
   * S = Reset system, processed by Arduino (homes and turns off lights)
   * x = end of string. Scanning is complete.
   * /
   Stopwatch timer = Stopwatch.StartNew();
   shootString = shootString.Replace(Environment.NewLine, " ");
   _scan.diagnostics.shootingString = shootString;
   shootString = Regex.Replace(shootString, @"\[[^\]]*\]", "");
   List<string> commands = new List<string>();
   commands.AddRange(Regex.Split(shootString, @"\s"));
   Debug.WriteLine("commands = " + String.Join(",", commands.ToArray()), "shoot strings");
   worker.ReportProgress(0, "Processing " + commands.Count + " shoot string commands:" + Environment.NewLine);
   for (int idx = 0; idx < commands.Count; idx++) {
       if (String.IsNullOrWhiteSpace(commands[idx])) { //ignore empty strings created by using a Regex split
           continue;
       }
       synchronizer.Reset();
       string cmd = commands[idx].Substring(0,1);
       string arg = commands[idx].Length > 1 ? commands[idx].Substring(1) : "";
       Debug.WriteLine(String.Format("idx = {0}, cmd = {1}, arg = {2}", idx, cmd, arg), "shoot strings");
       worker.ReportProgress(0, " > " + cmd + " " + arg);

       switch (cmd) {
       case "":
           //ignore empty commands
           break;
       case "P" :
           double sleepTime = 0;
           try {
               sleepTime = Convert.ToDouble(arg);
           } catch (Exception ex) {
               worker.ReportProgress(0, " * " + arg + " is not a valid pause time" + Environment.NewLine);
               break;
           }
           worker.ReportProgress(0, " - pausing " + arg + " seconds" + Environment.NewLine);
           System.Threading.Thread.Sleep(Convert.ToInt32(sleepTime * 1000)); // convert seconds to milliseconds
           break;
       case "T" :
           //the cam control is in the parent thread so we can't interact with it directly
           synchronizer.Reset();
           worker.ReportProgress(1);
           synchronizer.WaitOne();
           break;
       case "N" :
       case "F" :
       case "S" :
       case "g" :
       case "r" :
       case "b" :
       case "s" :
       case "V" :
       case "t" :
           worker.ReportProgress(0, " - passing command to SugarCube" + Environment.NewLine);
           _port.Write(cmd);
           synchronizer.Reset();
           synchronizer.WaitOne();
           break;
       case "H" :
           worker.ReportProgress(0, " - passing command to SugarCube" + Environment.NewLine);
           Elevation = 0;
           _port.Write(cmd);
           synchronizer.Reset();
           synchronizer.WaitOne();
           break;
       case "h" :
           worker.ReportProgress(0, " - passing command to SugarCube" + Environment.NewLine);
           Rotation = 0;
           _port.Write(cmd);
           synchronizer.Reset();
           synchronizer.WaitOne();
           break;
       case "D" :
           worker.ReportProgress(0, " - passing command to SugarCube" + Environment.NewLine);
           Elevation += 10;
           _port.Write(cmd);
           synchronizer.Reset();
           synchronizer.WaitOne();
           break;
       case "U" :
           Elevation -= 10;
           worker.ReportProgress(0, " - passing command to SugarCube" + Environment.NewLine);
           _port.Write(cmd);
           synchronizer.Reset();
           synchronizer.WaitOne();
           break;
       case "R" :
           Rotation += 15;
           worker.ReportProgress(0, " - passing command to SugarCube" + Environment.NewLine);
           _port.Write(cmd);
           synchronizer.Reset();
           synchronizer.WaitOne();
           break;
       case "L" :
           Rotation -= 15;
           worker.ReportProgress(0, " - passing command to SugarCube" + Environment.NewLine);
           _port.Write(cmd);
           synchronizer.Reset();
           synchronizer.WaitOne();
           break;
       case "x" :
           worker.ReportProgress(0, " - Shoot complete!" + Environment.NewLine);
           break;
       case "(":
           int repeats = 0;
           try {
               repeats = Convert.ToInt16(arg);
           } catch (Exception ex) {
               worker.ReportProgress(0, " * " + arg + " is not a valid repeat count" + Environment.NewLine);
               break;
           }
           repeats = repeats - 1; //to account for the first copy that's already there
           // find the location of the end parent
           int loopEnd = commands.FindIndex(idx, delegate (string str) {
               return str == ")";
           }) - 1;
           if (loopEnd == -1) {
               worker.ReportProgress(0, " * no closing parentheses was found, ignoring loop" + Environment.NewLine);
               break;
           }
           List<string> loopedCommands = commands.GetRange(idx + 1, loopEnd - idx);
           for (int i = 1; i <= repeats; i++) {
               commands.InsertRange(idx + 1, loopedCommands);
           }
           worker.ReportProgress(0, " - Begin loop, repeating " + (repeats + 1) + " times" + Environment.NewLine);
           break;
       case ")":
           worker.ReportProgress(0, " - End of loop, moving on" + Environment.NewLine);
           break;
       default :
           worker.ReportProgress(0, " * I don't know what to do with " + cmd + Environment.NewLine);
           break;

       }
   }
   timer.Stop();
   worker.ReportProgress(0, "Total elapsed time to process string was " + timer.Elapsed + Environment.NewLine);
}
*/
#endregion
