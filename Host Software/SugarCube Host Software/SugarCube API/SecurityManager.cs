/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Windows.Forms;

using log4net;
using Microsoft.Win32;

namespace Me.ThreeDWares.SugarCube {

	/// <summary>
	/// This class manages all aspects of working with the sugarcube security token
	/// </summary>
	public static class SecurityManager {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(SecurityManager));
				
		/// <summary>
		/// A salt used when encrypting the security token
		/// </summary>
		private const string SALT = "SugarCube4u";
		/// <summary>
		/// The registry location where the security token is stored, currently HKEY_LOCAL_MACHINE\SOFTWARE\3DWares\SugarCube
		/// </summary>
		private const string SUGARCUBE_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\3DWares\SugarCube";
		/// <summary>
		/// The name of the key for the token, imaginatively it is 'SecurityToken'
		/// </summary>
		private const string SECURITY_TOKEN = "SecurityToken";

		/// <summary>
		/// Given a password, return the SHA1 hash of the salted password
		/// </summary>
		/// <param name="password">A password to hash</param>
		/// <returns>The hashed password, as a strign of HEX</returns>
		public static string HashPassword(string password) {
			if (log.IsInfoEnabled) {
			    log.Info("Hashin password");
			}
			
			SHA1 sha = System.Security.Cryptography.SHA1.Create();
			byte[] inputBytes = Encoding.ASCII.GetBytes(SALT + password);
			byte[] hash = sha.ComputeHash(inputBytes);

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++) {
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Checks if the current user has the administrator role, which is necessary to actually set a new token
		/// in the registry
		/// </summary>
		/// <returns>True if the user has hte admin role, false if they do not</returns>
		public static bool IsAdministrator() {
			bool isAdmin = (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
					 .IsInRole(WindowsBuiltInRole.Administrator);
			if (log.IsInfoEnabled) {
			    log.Info("Checking is user is admin: " + isAdmin);
			}
			return isAdmin;
		}

		/// <summary>
		/// Checks if the given password, once hashed, matches the expected password
		/// </summary>
		/// <param name="password">A clear text string to check against expected</param>
		/// <param name="expected">An already hashed string</param>
		/// <returns></returns>
		private static bool ValidatePassword(string password, string expected) {
			bool validated = (HashPassword(password) == expected);
			if (log.IsInfoEnabled) {
			    log.Info("Validating password against expected: " + validated);
			}
			return validated;
		}

		/// <summary>
		/// Retrieves the security token from the registry
		/// </summary>
		/// <returns>An empty string if the token doesn't exist, else the value of the token</returns>
		public static string GetSecurityToken() {
			if (log.IsInfoEnabled) {
			    log.Info("Getting security token from registry");
			}
			
			string result = (string)Registry.GetValue(SUGARCUBE_KEY, SECURITY_TOKEN, "");
			if (result == null) {
				result = "";
			}
			if (log.IsDebugEnabled) {
			    log.Debug("Security token is " + result);
			}
			
			return result;
		}

		/// <summary>
		/// Writes a new security token to the registry, creating the registry path and key if necessary
		/// </summary>
		/// <param name="newToken">The new security token to write</param>
		public static void SetSecurityToken(string newToken) {
			if (log.IsInfoEnabled) {
			    log.Info("Writing new security token to the registry");
			}
			
			RegistryKey software = Registry.LocalMachine.OpenSubKey("SOFTWARE", RegistryKeyPermissionCheck.ReadWriteSubTree);
			RegistryKey threeDWares = software.CreateSubKey("3DWares");
			RegistryKey cube = threeDWares.CreateSubKey("SugarCube");
			cube.SetValue(SECURITY_TOKEN, newToken);
		}

		/// <summary>
		/// Clears the security token from the registry
		/// </summary>
		public static void ClearSecurityToken() {
			if (log.IsInfoEnabled) {
			    log.Info("Clearing security token from the registry");
			}
			
			RegistryKey software = Registry.LocalMachine.OpenSubKey("SOFTWARE", RegistryKeyPermissionCheck.ReadWriteSubTree);
			RegistryKey threeDWares = software.CreateSubKey("3DWares");
			RegistryKey cube = threeDWares.CreateSubKey("SugarCube");
			cube.DeleteValue(SECURITY_TOKEN);
		}

		/// <summary>
		/// Checks to see if the security token is set, using <see cref="GetSecurityToken"/>.  If it is, prompts
		/// the user for the password and then compares that to the security token using <see cref="ValidatePassword"/> 
		/// </summary>
		/// <returns>True if there is no security token set or the user entered the correct password, false if the user
		/// entered the wrong password
		/// </returns>
		public static bool PromptForAndCheckPassword() {
			if (log.IsInfoEnabled) {
			    log.Info("Checking if security token is set");
			}
			
			string securityToken = GetSecurityToken();
			if (securityToken == "") { //The SecurityToken key is not set, so no need to prompt for a password
				if (log.IsInfoEnabled) {
				    log.Info("Security token not set");
				}
				
				return true;
			}
			bool result = false;
			PasswordDialog dlg = new PasswordDialog();
			dlg.StartPosition = FormStartPosition.CenterScreen;
			if (log.IsInfoEnabled) {
			    log.Info("Prompting for password");
			}
			
			if (dlg.ShowDialog() == DialogResult.OK) {
				if (ValidatePassword(dlg.Password, securityToken)) {
					result = true;
				}
			}
			dlg.Dispose();
			if (log.IsInfoEnabled) {
			    log.Info("Result of PromptAndCheckForPassword = " + result);
			}
			
			return result;
		}
	}
}