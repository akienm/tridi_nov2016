/* SugarCube Host Software - Log Reader
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Me.ThreeDWares.SugarCube {
	//Note: We must use a class with properties for this to work with the DataGridView properly
	public class LogEvent {
		private string _timestamp;
		public string _thread;
		public string _level;
		public string _module;
		public string _message;
		
		public LogEvent(string timestamp, string thread, string level, string module, string message) {
			_timestamp = timestamp;
			_thread = thread;
			_level = level;
			_module = module;
			_message = message;
		}

		public string Timestamp {
			get {
				return _timestamp;
			}
		}

		public string Thread {
			get {
				return _thread;
			}
		}

		public string Level {
			get {
				return _level;
			}
		}
		public string Module {
			get {
				return _module;
			}
		}
		public string Message {
			get {
				return _message;
			}
		}
	}


	public partial class ReaderForm : Form {
		private StringCollection _modules = new StringCollection();
		private List<LogEvent> _events = new List<LogEvent>();
		private BindingSource _binder = new BindingSource();
		private bool _ignoreUpdates = false;

		
		public ReaderForm() {
			InitializeComponent();
		}

		private void SetupGridView() {
			gridView.AutoGenerateColumns = false;
			gridView.AutoSize = true;
			gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			gridView.DataSource = _binder;

			DataGridViewColumn column = new DataGridViewTextBoxColumn();
			column.DataPropertyName = "Timestamp";
			column.Name = "Timestamp";
			column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			gridView.Columns.Add(column);

			column = new DataGridViewTextBoxColumn();
			column.DataPropertyName = "Thread";
			column.Name = "Thread";
			column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			gridView.Columns.Add(column);

			column = new DataGridViewTextBoxColumn();
			column.DataPropertyName = "Level";
			column.Name = "Level";
			column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			gridView.Columns.Add(column);

			column = new DataGridViewTextBoxColumn();
			column.DataPropertyName = "Module";
			column.Name = "Module";
			column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			gridView.Columns.Add(column);

			column = new DataGridViewTextBoxColumn();
			column.DataPropertyName = "Message";
			column.Name = "Message";
			//column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			gridView.Columns.Add(column);
		}

		private void ColorGridViewRow(DataGridViewRow row) {
			switch (row.Cells[2].Value.ToString()) {
			case "DEBUG":
				row.DefaultCellStyle.ForeColor = Color.DarkGray;
				break;
			case "WARN":
				row.DefaultCellStyle.ForeColor = Color.DarkBlue;
				break;
			case "ERROR":
				row.DefaultCellStyle.ForeColor = Color.Red;
				break;
			case "FATAL":
				row.DefaultCellStyle.ForeColor = Color.White;
				row.DefaultCellStyle.BackColor = Color.Red;
				break;
			default:
				break;
			}
		}

		void ReaderFormLoad(object sender, EventArgs e) {
			_ignoreUpdates = true;
			cbLogLevel.SelectedIndex = 0;
			cbModuleChooser.SelectedIndex = 0;
			_ignoreUpdates = false;
			SetupGridView();
		}

		private void AddEventsToBinder(string moduleFilter, string levelFilter) {
			// check if we need to update the filter string for selecting All modules or leves
			moduleFilter = (moduleFilter == "All") ? ".*" : moduleFilter;
			levelFilter = (levelFilter == "All") ? ".*" : levelFilter;

			foreach (LogEvent entry in _events) {
				if (Regex.IsMatch(entry.Module, moduleFilter) && Regex.IsMatch(entry.Level, levelFilter)) {
					_binder.Add(entry);
				}
			}
		}

		private void ProcessEvent(string line) {
			Match parsedString = Regex.Match(line, @"(\d{4}-\d{2}-\d{2}\s+\d+:\d+:\d+,\d+)\s+\[(\d+)\]\s+([A-Z]+)\s+Me\.ThreeDWares\.SugarCube\.([A-Za-z\.]+) - (.+)", RegexOptions.Singleline);
			if (!_modules.Contains(parsedString.Groups[4].Value)) {
				_modules.Add(parsedString.Groups[4].Value);
			}
			_events.Add(new LogEvent(parsedString.Groups[1].Value, parsedString.Groups[2].Value, parsedString.Groups[3].Value,
											 parsedString.Groups[4].Value, parsedString.Groups[5].Value));

		}

		private void ReadLogFile(string fileName) {
			string[] lines = File.ReadAllLines(fileName);

			string line;
			for (int i = 0; i < lines.Length; i++) {
				line = lines[i];
				while (i < lines.Length - 1 && !Regex.IsMatch(lines[i+1], @"^\d{4}-\d{2}-\d{2}")) { //handle multi line messages
					i++;
					line = line + Environment.NewLine + lines[i];
				}
				ProcessEvent(line);
			}
		}

		void BtnOpenClick(object sender, EventArgs e) {
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				_binder.Clear();
				_events.Clear();
				_modules.Clear();
				ReadLogFile(openFileDialog.FileName);
				ArrayList.Adapter(_modules).Sort();
				cbModuleChooser.Items.Clear();
				cbModuleChooser.Items.Add("All");
				foreach (string module in _modules) {
					cbModuleChooser.Items.Add(module);
				}
				string fileCreated = File.GetCreationTime(openFileDialog.FileName).ToString("yyyy-MM-dd HH:mm:ss");
				this.Text = "Log Reader - '" + openFileDialog.FileName + "' created " + fileCreated;
				_ignoreUpdates = true;
				cbModuleChooser.SelectedIndex = 0;
				cbLogLevel.SelectedIndex = 0;
				_ignoreUpdates = false;
				AddEventsToBinder("All", "All");
			}
		}


		void GridViewCellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
			if (_ignoreUpdates) {
				return;
			}
			
			if (gridView.Rows[0].Cells[0].Value != null) {
				ColorGridViewRow(gridView.Rows[e.RowIndex]);
			}
		}

		void FilterChanged(object sender, EventArgs e) {
			if (_ignoreUpdates) {
				return;
			}

			if (_events.Count > 0) {
				_binder.Clear();
				AddEventsToBinder(cbModuleChooser.SelectedItem.ToString(), cbLogLevel.SelectedItem.ToString());
			}

		}
	}
}
