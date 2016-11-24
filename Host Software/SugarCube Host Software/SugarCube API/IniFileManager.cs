/* Mkulu Ini File Manager
 * Copyright (c) 2010-2014 Chad Ullman
 *
 * This software is provided 'as-is', without any express or implied warranty. In
 * no event will the authors be held liable for any damages arising from the use
 * of this software.
 *
 * Permission is granted to anyone to use this software for any purpose, including
 * commercial applications, and to alter it and redistribute it freely, subject to
 * the following restrictions:
 *
 *    1. The origin of this software must not be misrepresented; you must not claim
 *       that you wrote the original software. If you use this software in a product,
 *       an acknowledgment in the product documentation would be appreciated but is
 *       not required.
 *
 *    2. Altered source versions must be plainly marked as such, and must not be
 *       misrepresented as being the original software.
 *
 *    3. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using System.Diagnostics;

namespace org.mkulu.config {
	#region IniFileManager
	/// <summary>
	/// This class provides a simple interface to config data stored in the now classic Microsoft .ini file
	/// format. Originally the class used the Win32 Get/WritePrivateProfileString functions, but that was later
	/// changed to the current implementation, where the class parses the config file itself.  Blank lines are
	/// ignored, as are any lines starting with a semi-colon (;) or REM.  The class supports encrypting the entire
	/// config file, or just certain values in the file.
	/// 
	/// Any comments in the config file will be lost once it has been processed by this class.
	/// 
	/// The config file is saved to disk every time you use one of the writeXXX functions.  While this is somewhat
	/// inefficient, realistically we are not going to be writing out thosands of config entries all the time, and in
	/// real world use the benefits of never having to worry about flushing the updates to disk outweigh any nano-second
	/// performance benefits we might get by implemeting a transaction/commit system.
	/// 
	/// The latest version of this file can always be found at http://apps.mkulu.org/mkuluutils/inifilemanager.shtml
 	/// </summary>
	public class IniFileManager {
		#region Data Members
		/// <summary>
		/// The name of the actual file on disk that holds our config info
		/// </summary>
		private string _configFileName;
		/// <summary>
		/// A byte array that holds our cryptographic key
		/// </summary>
		private byte[] _cryptoKey;
		/// <summary>
		/// The initialization vectore for our crypto functions
		/// </summary>
		private byte[] _cryptoInitializationVector;
		/// <summary>
		/// A flag to indicate if we are dealing with an entirely encrypted config file or not
		/// </summary>
		private bool _fileIsEncrypted;
		/// <summary>
		/// If we are using encryption, a flag to indicate if we have initialized our crypto engine
		/// </summary>
		private bool _encryptionInitialized = false;
		/// <summary>
		/// Our crypto engine
		/// </summary>
		private AesManaged _aes;
		/// <summary>
		/// We cache our config info into a sorted dictionary, where the keys are the section names and the values are string
		/// dictionaries of key/value pairs.  We use our custom IniCacheSorter class to sort the config sections in case-insensitive
		/// alphabetical order
		/// </summary>
		private SortedDictionary<string, StringDictionary> _cache = new SortedDictionary<string, StringDictionary>(new IniCacheSorter<string>());
		/// <summary>
		/// A simple text token that can be used to replace newlines when storing string values.
		/// </summary>
		private const string NEWLINE_TOKEN = "*NEWLINE_TOKEN*";
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance of the class, specifying the file to manage
		/// </summary>
		/// <param name="fileToManage">The name of the file to manage</param>
		public IniFileManager(string fileToManage) {
			_configFileName = fileToManage;
			_fileIsEncrypted = false;
			cacheIniFile();
		}

		/// <summary>
		/// Create a new instance of the class, specifying the file to manage and providing
		/// the crypto key to use for the entire file.  By default, initializes _aes
		/// </summary>
		/// <param name="fileToManage">The name of the file to manage</param>
		/// <param name="key">The key to use to encrypt/decrypt the file</param>
		public IniFileManager(string fileToManage, string key) {
			_configFileName = fileToManage;
			_fileIsEncrypted = true;
			_aes = new AesManaged();
			_aes.BlockSize = 128;
			_aes.KeySize = 256;
			_cryptoKey = ASCIIEncoding.ASCII.GetBytes(key);
			_cryptoInitializationVector = _cryptoKey;
			if (_cryptoInitializationVector.Length != (_aes.BlockSize / 8)) {
				Array.Resize(ref _cryptoInitializationVector, (_aes.BlockSize / 8));
			}
			if (_cryptoKey.Length != (_aes.KeySize / 8)) {
				Array.Resize(ref _cryptoKey, (_aes.KeySize / 8));
			}
			_encryptionInitialized = true;

			cacheIniFile();
		}

		/// <summary>
		/// Create a new instance of the class, specifying the file to manage and providing
		/// the crypto key to use for the encrypted entries in the file.  The file itself
		/// is not encrypted, only certain keys.  By default, initializes _aes.
		/// </summary>
		/// <param name="fileToManage">The name of the file to manage</param>
		/// <param name="key">The key to use to encrypt/decrypt the file</param>
		/// <param name="fileHasEncryptedData">True if the file contains encrypted keys, false otherwise</param>
		public IniFileManager(string fileToManage, string key, bool fileHasEncryptedData) {
			_configFileName = fileToManage;
			_fileIsEncrypted = false;
			if (fileHasEncryptedData) {
				_aes = new AesManaged();
				_aes.BlockSize = 128;
				_aes.KeySize = 256;
				_cryptoKey = ASCIIEncoding.ASCII.GetBytes(key);
				_cryptoInitializationVector = _cryptoKey;
				if (_cryptoInitializationVector.Length != (_aes.BlockSize / 8)) {
					Array.Resize(ref _cryptoInitializationVector, (_aes.BlockSize / 8));
				}
				if (_cryptoKey.Length != (_aes.KeySize / 8)) {
					Array.Resize(ref _cryptoKey, (_aes.KeySize / 8));
				}
				_encryptionInitialized = true;
			}
			cacheIniFile();
		}
		#endregion

		#region Encryption Utilities
		/// <summary>
		/// Given a string, returns the encrypted version of that string
		/// 
		/// From http://remy.supertext.ch/2011/01/simple-c-encryption-and-decryption/ 
		/// </summary>
		/// <param name="source">A string to encrypt</param>
		/// <returns>The encrypted string</returns>
		private string Encrypt(string source) {
			if (String.IsNullOrEmpty(source)) {
				return "";
			}

			ICryptoTransform encryptor = _aes.CreateEncryptor(_cryptoKey, _cryptoInitializationVector);

			try {
				byte[] sourceBytes = ASCIIEncoding.ASCII.GetBytes(source);
				byte[] encryptedSource = encryptor.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);
				return Convert.ToBase64String(encryptedSource);
			} catch (CryptographicException cex) {
				throw new Exception("Unable to encrypt string: " + cex.Message, cex);
			} catch (Exception) {
				throw;
			}
		}

		/// <summary>
		/// Given an encrypted string, returns the plain text
		/// </summary>
		/// <param name="source">An encrypted string</param>
		/// <returns>The plain text for that string</returns>
		private string Decrypt(string source) {
			if (String.IsNullOrEmpty(source)) {
				return "";
			}
			ICryptoTransform decryptor = _aes.CreateDecryptor(_cryptoKey, _cryptoInitializationVector);
			byte[] encryptedSourceBytes = Convert.FromBase64String(source);
			string plaintext = null;
			using (MemoryStream msDecrypt = new MemoryStream(encryptedSourceBytes)) {
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
					using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
						// Read the decrypted bytes from the decrypting stream
						// and place them in a string.
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
			return plaintext;
		}
		#endregion

		#region Cache Handling
		/// <summary>
		/// Loads the entire ini file in to _cache, decrypting if ncecessary
		/// </summary>
		private void cacheIniFile() {
			string configData;

			if (!File.Exists(_configFileName)) {
				return;
			}

			configData = File.ReadAllText(_configFileName);

			//It is always possible to pass in an unencrypted ini file which we want to encrypt, so we need to check for that
			int encryptionIndicator = configData.IndexOf(Environment.NewLine);
			if (encryptionIndicator == -1 && !_fileIsEncrypted) {
				throw new Exception("Config file is encrypted, but no key provided");
			}
			if (encryptionIndicator == -1 && _fileIsEncrypted) {
				configData = Decrypt(configData);
			}

			string[] lines = configData.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

			StringDictionary sectionData = null;
			string sectionName = "";
			Match res;
			foreach (string line in lines) {
				Debug.WriteLine("Processing line " + line);
				if (Regex.IsMatch(line, @"^\s*;") || Regex.IsMatch(line, @"^\s*rem\s+", RegexOptions.IgnoreCase)) { //comment line, skip
					Debug.WriteLine("  line is a comment, skipping");
					continue;
				} else if (Regex.IsMatch(line, @"^\[.*\]")) { //section header
					Debug.WriteLine("  line is a section header, processing");
					if (sectionData != null) {
						_cache.Add(sectionName, sectionData);
					}
					res = Regex.Match(line, @"\[(.*)\]");
					sectionName = res.Groups[1].Value;
					//sectionName = sectionName.ToLower();
					Debug.WriteLine("  sectionName = " + sectionName);
					sectionData = new StringDictionary();
				} else {
					Debug.WriteLine("  line may be section data");
					Match parts = Regex.Match(line, @"^([^=]*)=(.*)");
					if (parts.Groups.Count != 3 || sectionData == null) {
						Debug.WriteLine("  nope, this is in fact not section data");
						continue;
					}
					Debug.WriteLine("  " + parts.Groups[1].Value.Trim() + "=" + parts.Groups[2].Value.Trim());
					sectionData.Add(parts.Groups[1].Value.Trim(), parts.Groups[2].Value.Trim());
				}
			}
			// the last section we processed won't have been saved yet, so do that now
			_cache.Add(sectionName, sectionData);
		}

		/// <summary>
		/// Dump the entire _cache back to file, encrypting it if necessary
		/// </summary>
		private void dumpCache() {
			StringBuilder configData = new StringBuilder();

			foreach (KeyValuePair<string, StringDictionary> section in _cache) {
				configData.AppendLine("[" + section.Key + "]");
				foreach (DictionaryEntry data in section.Value) {
					configData.AppendLine(String.Format("{0}={1}", data.Key, data.Value));
				}
				configData.AppendLine();
			}

			string config = configData.ToString();

			//dump cache in to configData
			if (_fileIsEncrypted) {
				config = Encrypt(config);
			}

			File.WriteAllText(_configFileName, config);
		}
		#endregion

		#region Data Access - Read
		/// <summary>
		/// Get a value from a key in a section as a string.  Internally calls doGetString
		/// </summary>
		/// <param name="section">The section of the config file to read from</param>
		/// <param name="key">The key whose value we want to retreive</param>
		/// <param name="defaultValue">The default value to return if the key is not found</param>
		/// <returns>A string</returns>
		public string getString(string section, string key, string defaultValue) {
			return doGetString(section, key, defaultValue, false);
		}

		/// <summary>
		/// Get a value from an encrypted key in a section as a string.  Internally calls doGetString
		/// </summary>
		/// <param name="section">The section of the config file to read from</param>
		/// <param name="key">The key whose value we want to retreive</param>
		/// <param name="defaultValue">The default value to return if the key is not found</param>
		/// <returns>A string</returns>
		public string getEncryptedString(string section, string key, string defaultValue) {
			return doGetString(section, key, defaultValue, true);
		}

		/// <summary>
		/// Does the actual work of getting a string from the file and encrypting as necessary
		/// </summary>
		/// <param name="section">The section of the config file to read from</param>
		/// <param name="key">The key whose value we want to retreive</param>
		/// <param name="defaultValue">The default value to return if the key is not found</param>
		/// <param name="encrypted">True if the key is encrypted, false otherwise</param>
		/// <returns>A string</returns>
		private string doGetString(string section, string key, string defaultValue, bool encrypted) {
			if (encrypted && !_encryptionInitialized) {
				throw new Exception("You did not specify an encryption key, encryption has not been initialized");
			}

			key = key.ToLower();
			if (!_cache.ContainsKey(section)) {
				return defaultValue;
			}
			StringDictionary sectionData = (StringDictionary)_cache[section];
			if (!sectionData.ContainsKey(key)) {
				return defaultValue;
			}
			string buffer = (encrypted) ? Decrypt(sectionData[key]) : sectionData[key];
			return buffer.Replace(NEWLINE_TOKEN, Environment.NewLine);
		}

		/// <summary>
		/// Get a value from a key in a section as an integer.  Internally calls getString
		/// </summary>
		/// <param name="section">The section of the config file to read from</param>
		/// <param name="key">The key whose value we want to retreive</param>
		/// <param name="defaultValue">The default value to return if the key is not found</param>
		/// <returns>An integer</returns>
		public int getInt(string section, string key, int defaultValue) {
			string value = getString(section, key, defaultValue.ToString());
			int result;
			try {
				result = Convert.ToInt32(value);
				return result;
			} catch {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get a value from a key in a section as a boolean.  Internally calls getString
		/// </summary>
		/// <param name="section">The section of the config file to read from</param>
		/// <param name="key">The key whose value we want to retreive</param>
		/// <param name="defaultValue">The default value to return if the key is not found</param>
		/// <returns>A boolean</returns>
		public bool getBool(string section, string key, bool defaultValue) {
			string value = getString(section, key, defaultValue.ToString());
			bool result;
			if (Boolean.TryParse(value, out result)) {
				return result;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get a list of all the sections in a config file
		/// </summary>
		/// <returns>A string list of all the sections in the config file</returns>
		public List<string> getSections() {
			return _cache.Keys.ToList<string>();
		}

		/// <summary>
		/// Get a string dictionary containing all the key/value pairs in a section of the config file.  
		/// 
		/// Note: If the section contains encrypted keys, you do not get the unecrypted values.
		/// </summary>
		/// <param name="section">The name of the section to get</param>
		/// <returns>A string dictionary of key/value pairs</returns>
		public StringDictionary getSection(string section) {
			if (_cache.Keys.Contains(section)) {
				return _cache[section];
			} else {
				return null;
			}
		}
		#endregion

		#region Data Access - Write

		/// <summary>
		/// Set the string value of a key in a section as a string.  Internally calls doWriteString
		/// </summary>
		/// <param name="section">The section of the config file to write to</param>
		/// <param name="key">The key whose value we want to set</param>
		/// <param name="value">The value to set</param>
		public void writeString(string section, string key, string value) {
			doWriteString(section, key, value, false);
		}

		/// <summary>
		/// Set the encrypted string value of a key in a section as a string.  Internally calls doWriteString
		/// </summary>
		/// <param name="section">The section of the config file to write to</param>
		/// <param name="key">The key whose value we want to set</param>
		/// <param name="value">The value to set</param>
		public void writeEncryptedString(string section, string key, string value) {
			doWriteString(section, key, value, true);
		}

		/// <summary>
		/// Does the actual work of setting the string value and encrypting it if necessary
		/// </summary>
		/// <param name="section">The section of the config file to write to</param>
		/// <param name="key">The key whose value we want to set</param>
		/// <param name="value">The value to set</param>
		/// <param name="encrypted">True if the value is to be encrypted, false otherwise</param>
		private void doWriteString(string section, string key, string value, bool encrypted) {
			if (encrypted && !_encryptionInitialized) {
				throw new Exception("You did not specify an encryption key, encryption has not been initialized");
			}

			if (!_cache.ContainsKey(section)) {
				_cache.Add(section, new StringDictionary());
			}

			StringDictionary sectionData = (StringDictionary)_cache[section];
			string buffer = value.Replace(Environment.NewLine, NEWLINE_TOKEN);
			if (encrypted) {
				buffer = Encrypt(buffer);
			}
			if (!sectionData.ContainsKey(key)) {
				sectionData.Add(key, buffer);
			} else {
				sectionData[key] = buffer;
			}
			dumpCache();
		}

		/// <summary>
		/// Set the string value of a key in a section as an integer.  Internally calls writeString
		/// </summary>
		/// <param name="section">The section of the config file to write to</param>
		/// <param name="key">The key whose value we want to set</param>
		/// <param name="value">The value to set</param>
		public void writeInt(string section, string key, int value) {
			writeString(section, key, value.ToString());
		}

		/// <summary>
		/// Set the string value of a key in a section as a boolean.  Internally calls writeString
		/// </summary>
		/// <param name="section">The section of the config file to write to</param>
		/// <param name="key">The key whose value we want to set</param>
		/// <param name="value">The value to set</param>
		public void writeBool(string section, string key, bool value) {
			writeString(section, key, value.ToString());
		}
		#endregion

		#region Data Access - Delete
		/// <summary>
		/// Allows you to remove an entire section from the config file. Yes, this is dangerours, so be careful, it's irreversible!
		/// </summary>
		/// <param name="section">The section to delete</param>
		public void deleteSection(string section) {
			if (_cache.ContainsKey(section)) {
				_cache.Remove(section);
				dumpCache();
			}
		}

		/// <summary>
		/// Delete a key from a section. Yes, this is dangerours, so be careful, it's irreversible!
		/// </summary>
		/// <param name="section">The section where the key is stored</param>
		/// <param name="key">The name of the key to remove</param>
		public void deleteKey(string section, string key) {
			if (_cache.ContainsKey(section) && ((StringDictionary)_cache[section]).ContainsKey(key)) {
				((StringDictionary)_cache[section]).Remove(key);
				dumpCache();
			}
		}
		#endregion
	}
	#endregion

	#region IniCacheSorter
	/// <summary>
	/// A simple utility class to sort our config cache alphabetically, ignoring case
	/// </summary>
	public class IniCacheSorter<T> : IComparer<T> where T: IComparable<T> {

		/// <summary>
		/// The actual comparison function.  Sorts the dictionary in alphabetical order, ignoring case
		/// </summary>
		/// <param name="x">The first item to compare, of type T</param>
		/// <param name="y">The second item to compare, of type T</param>
		/// <returns>
		///      Less than zero: x is less than y.
		///                Zero: x equals y.
		///   Greater than zero: x is greater than y.
		/// </returns>
		public int Compare(T x, T y) {
			string first  = x as string;
			string second = y as string;

			return string.Compare(first, second, true); //case insensitive comparison
		}
	}
	#endregion
}
