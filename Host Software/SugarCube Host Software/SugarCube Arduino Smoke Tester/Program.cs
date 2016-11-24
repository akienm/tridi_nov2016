/* SugarCube Host Software - SugarCube Arduino Smoke Tester
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;

namespace Me.ThreeDWares.SugarCube {
	class Program {
		private static SerialPort _port = null;
		private const string FORMAT = "{0} | {1, -7} | {2, -20} | {3}";


		public static int Main(string[] args) {
			Console.WriteLine("SugarCube Arduino Smoke Tester");
			Console.WriteLine("==============================\n");
			Console.Write("Here are the available COM ports: ");
			ListCOMPorts();
			Console.WriteLine("Which one is the Arduino on?");
			Console.Write("> ");
			string port = Console.ReadLine();
			if (!InitializePort(port)) {
				Console.WriteLine("Unable to initialize COM port " + port + ".\nPlease check and try again");
				ClosePort();
				Pause();
				return 1;
			}
			Console.WriteLine("Running tests on port " + port + " at " + DateTime.Now.ToString());
			Console.WriteLine(FORMAT, " ", "COMMAND", "EXPECTED", "ACTUAL");
			RunTests();
			Console.WriteLine("\nTesting Complete");
			ClosePort();
			Pause();
			return 0;
		}

		private static void Pause() {
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);

		}

		private static void ListCOMPorts() {
			string[] ports = SerialPort.GetPortNames();
			foreach(string portname in ports) {
				if (!portname.StartsWith("com", StringComparison.OrdinalIgnoreCase)) {
					continue;
				}
				Console.Write(portname + " ");
			}
			Console.WriteLine();
		}

		private static bool InitializePort(string thePort) {
			try {
				_port = new SerialPort(thePort);
				_port.BaudRate = 9600;
				_port.Parity = Parity.None;
				_port.StopBits = StopBits.One;
				_port.DataBits = 8;
				_port.Open();
				_port.ReadTimeout = 15000;
			} catch {
				return false;
			}
			return true;
		}

		private static void ClosePort() {
			if (_port != null) {
				try {
					_port.Close();
				} catch (Exception ex) {
					Console.WriteLine("Error while closing COM port: " + ex.Message);
				}
			}
		}

		private static void RunSingleTest(string test, string expected) {
			string actual;
			try {
				_port.DiscardInBuffer();
				_port.WriteLine(test);
				Thread.Sleep(500);
				actual = _port.ReadLine().TrimEnd('\r');
				actual = Regex.Replace(actual, @"\p{Cc}", a=>string.Format("\\{0:X2}", (byte)a.Value[0]));
			} catch (Exception ex) {
				actual = "ERROR: " + ex.Message;
			}
			string result = (actual.StartsWith(expected)) ? " ": "!";
			Console.WriteLine(FORMAT, result, test, expected, actual);

		}

		private static void RunTests() {
			if (!File.Exists("Arduino_Smoke_Tests.cfg")) {
				Console.WriteLine("Could not find the test config file Arduino_Smoke_Tests.cfg!");
				return;
			}

			string[] tests = File.ReadAllText("Arduino_Smoke_Tests.cfg").Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string test in tests) {
				string[] testData = test.Split(new char[] {'|'});
				RunSingleTest(testData[0], testData[1]);
			}
		}
	}
}