/* SugarCube Host Software - Test Bed
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.IO;
using System.Windows.Forms;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	public class EmailToCollection  {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(EmailToCollection));

		private string _dataFilePath;
		public AutoCompleteStringCollection data = new AutoCompleteStringCollection();

		public EmailToCollection(string dataPath) {
			if (log.IsInfoEnabled) {
				log.Info("Initializing");
			}

			_dataFilePath = Path.Combine(dataPath, "email_list.txt");
			if (log.IsDebugEnabled) {
				log.Debug("data file path is " + _dataFilePath);
			}

			LoadEmailsFromDisk();
		}

		private void LoadEmailsFromDisk() {
			if (log.IsInfoEnabled) {
				log.Info("loading emails from disk");
			}

			if (!File.Exists(_dataFilePath)) {
				if (log.IsInfoEnabled) {
					log.Info("data file does not exist, returning");
				}

				return;
			}
			string[] emails = File.ReadAllLines(_dataFilePath);
			data.Clear();
			data.AddRange(emails);
			if (log.IsDebugEnabled) {
				log.Debug("Added " + data.Count + " emails");
			}

		}

		private void SaveEmailsToDisk() {
			if (log.IsInfoEnabled) {
				log.Info("Saving emails to disk");
			}

			string[] emails = new string[data.Count];
			for (int i = 0; i < data.Count; i++) {
				emails[i] = data[i];
			}
			File.Delete(_dataFilePath);
			File.WriteAllLines(_dataFilePath, emails);
		}

		public string MostRecentlyUsedEmail {
			get {
				string val = (data.Count == 0) ? "" : data[0];
				if (log.IsInfoEnabled) {
					log.Info("Most recently used email is " + val);
				}
				return val;
			}
		}


		// Add an email to our list.  Insert it at position 1 in the list.
		// if it's already there, move it to position 1
		public void AddEmail(string email) {
			if (log.IsInfoEnabled) {
				log.Info("Adding email " + email + " to list");
			}

			if (data.Contains(email)) {
				if (log.IsDebugEnabled) {
					log.Debug("email was already in list, moving to position 0");
				}

				data.Remove(email);
			}
			data.Insert(0, email);
			SaveEmailsToDisk();
		}
	}
}
