/* SugarCube Host Software - SugarCube Simulator
 * Copyright (c) 2015 Chad Ullman
 *
 * A simple tool, used in conjunction with a null modem driver like com0com, to simulate having
 * a SugarCube attached when in fact we don't ;-)
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
		private static bool _exitFlag = false;

		public static int Main(string[] args) {
			Console.WriteLine("SugarCube Simulator");
			Console.WriteLine("===================\n");
			Console.Write("Here are the available COM ports: ");
			ListCOMPorts();
			Console.WriteLine("Which one will we be listening on?");
			Console.Write("> ");
			string port = Console.ReadLine();
			if (!InitializePort(port)) {
				Console.WriteLine("Unable to initialize COM port " + port + ".\nPlease check and try again");
				ClosePort();
				Console.Write("Press any key to continue . . . ");
				Console.ReadKey(true);
				return 1;
			}
			Console.WriteLine("Listening on port " + port + " at " + DateTime.Now.ToString());
			try {
				ListenOnPort();
			} finally {
				ClosePort();
				Console.WriteLine("\nSimulation Complete");
			}
			return 0;
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
			} catch (Exception ex) {
				Console.WriteLine("ERROR: " + ex.Message);
				return false;
			}
			return true;
		}

		private static void ClosePort() {
			if (_port != null) {
				try {
					_port.Close();
				} catch (Exception ex) {
					Console.WriteLine("ERROR: " + ex.Message);
				}
			}
		}

		private static void ListenOnPort() {
			_port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
			while (!_exitFlag) {
				Thread.Sleep(100);
			}
		}

		private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e) {
			SerialPort sp = (SerialPort)sender;
			string indata = sp.ReadExisting().TrimEnd('\r');
			indata = Regex.Replace(indata, @"\p{Cc}", a=>string.Format("\\{0:X2}", (byte)a.Value[0]));
			Console.WriteLine("[" + DateTime.Now.ToString("HH:mm.ff") + "]: " + indata);
			if (indata.StartsWith("c")) {
				sp.WriteLine("0: CheckReady OK");
			} else {
				sp.WriteLine("0: Simulation OK");
			}
		}


	}
}