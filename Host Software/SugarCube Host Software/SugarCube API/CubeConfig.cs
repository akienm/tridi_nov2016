/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using log4net;
using org.mkulu.config;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// This class handles all the configurable attributes of our SugarCube ecosystem
	/// </summary>
	public class CubeConfig {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(CubeConfig));
		
		/// <summary>
		/// The private key that is used to encrypt/decrypt config data
		/// </summary>
		private const string KEY = @"3JVr4Y^Nw!@kMdEgbFT#aaFZ4BM45F2&R6gbbNn8*";

		/// <summary>
		/// We store our configuration in good old fashioned ini files, managed by this class, this is the
		/// main config file
		/// </summary>
		private IniFileManager _config;
		/// <summary>
		/// This file contains our (encrypted) password that we use to encrypt the order forms before uploading
		/// them to the 3dwares servers
		/// </summary>
		private IniFileManager _orderPassword;
		/// <summary>
		/// The path to where we store our config file and also all the scans we do
		/// </summary>
		private string _dataPath;
		/// <summary>
		/// Keeps track of whether we had to create the config file when this class was initialized or not
		/// </summary>
		public bool ConfigFileCreated = false;

		/// <summary>
		/// Used in conjunction with IsValidKeyForScanID and CheckKeyIsValidForScanID to determine if a character being
		/// entered in to a Scan ID text box is a valid character
		/// </summary>
		private bool _isValidKeyForScanID;

		/// <summary>
		/// Used by IsValidEmail and DomainMapper to track if a given email address is valid or not
		/// </summary>
		private bool _isValidEmail = false;

		/// <summary>
		/// Constructor, uses the default value for _dataPath, which is %appdata%.  If it doesn't already exist,
		/// creates _dataPath and then initializes the IniFileManager, which in turn creates the config file. Sets the
		/// ConfigFileCreated token to track if the config already existed or not (Calls Setup to do all the actual work)
		/// </summary>
		public CubeConfig () {
			if (log.IsDebugEnabled) {
			    log.Debug("Initializing CubeConfig with default constuctor");
			}
			
			_dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3DWares");
			Setup();
		}

		/// <summary>
		/// Constructor, uses the value passed in for _dataPath.  If it doesn't already exist, creates _dataPath and
		/// then initializes the IniFileManager, which in turn creates the config file. Sets the ConfigFileCreated
		/// token to track if the config already existed or not  (Calls Setup to do all the actual work)
		/// </summary>
		/// <param name="dataPath"></param>
		public CubeConfig(string dataPath) {
			if (log.IsDebugEnabled) {
			    log.Debug("Initializing CubeConfig, passing in dataPath: " + dataPath);
			}
			
			_dataPath = dataPath;
			Setup();

		}

		/// <summary>
		/// Does the actual work of setting up the cube config, called from the constructors
		/// </summary>
		private void Setup() {
			if (log.IsDebugEnabled) {
			    log.Debug("In setup, _dataPath = " + _dataPath);
			}
			
			if (!Directory.Exists(_dataPath)) {
				if (log.IsInfoEnabled) {
				    log.Info("Created _dataPath at " + _dataPath);
				}
				
				Directory.CreateDirectory(_dataPath);
				ConfigFileCreated = true;
			}

			string filePath = Path.Combine(_dataPath, "SugarCube.cfg");
			if (!File.Exists(filePath)) {
				if (log.IsInfoEnabled) {
				    log.Info("SugarCube.cfg not found in _dataPath, creating");
				}
				
				ConfigFileCreated = true;
			}
			_config = new IniFileManager(filePath, KEY, true);
			
			if (!File.Exists("OrderSec.cfg")) {
				if (log.IsInfoEnabled) {
				    log.Info("Order Security file not found");
				}
			}
			_orderPassword = new IniFileManager("OrderSec.cfg", KEY, true);
		}

		/// <summary>
		/// The path on disk where app specific data should be stored
		/// </summary>
		public string DataPath {
			get {
				return _dataPath;
			}
		}

		/// <summary>
		/// Makes a copy of our SugarCube.cfg file in the file specified by savePath
		/// </summary>
		/// <param name="savePath">The file (with full path) to save the config to.  The path must exist, this function will not
		/// create the path if it doesn't already exist</param>
		public void ExportConfigFile(string savePath) {
			if (log.IsDebugEnabled) {
			    log.Debug("Trying to export config file to " + savePath);
			}
			
			try {
				File.Copy(Path.Combine(_dataPath, "SugarCube.cfg"), savePath);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to copy SugarCube.cfg to " + savePath + ": " + ex.Message);
				}
				
				throw new Exception("Unable to copy SugarCube.cfg to " + savePath + ": " + ex.Message);
			}
		}

		/// <summary>
		/// Import a saved config file
		/// </summary>
		/// <param name="configPath">The full path to the exported config file to save</param>
		public void ImportConfigFile(string configPath) {
			if (log.IsDebugEnabled) {
			    log.Debug("Attempting to import new config file from " + configPath);
			}
			
			if (!File.Exists(configPath)) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to import " + configPath + ", file does not exist");
				}
				
				throw new ArgumentException("Unable to import " + configPath + ", file does not exist");
			}
			try {
				RemoveConfigFile();
				string filePath = Path.Combine(_dataPath, "SugarCube.cfg");
				if (log.IsDebugEnabled) {
				    log.Debug("Attempting to copy file " + configPath + " to " + filePath);
				}
				
				File.Copy(configPath, filePath);
				_config = new IniFileManager(filePath, KEY, true);
			} catch (Exception ex) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to import config file " + configPath + ": " + ex.Message);
				}
				
				throw new Exception("Unable to import config file " + configPath + ": " + ex.Message);
			}
		}

		/// <summary>
		/// Delete the config file, allowing us to start anew with the default values
		/// </summary>
		public void RemoveConfigFile() {
			string filePath = Path.Combine(_dataPath, "SugarCube.cfg");
			if (log.IsDebugEnabled) {
			    log.Debug("Attempting to remove config file at " + filePath);
			}
			
			if (File.Exists(filePath)) {
				File.Delete(filePath);
			}
		}

		/// <summary>
		/// The name of the camera to use to capture images
		/// </summary>
		public string Camera {
			get {
				string val = _config.getString("CAMERA", "camera", "IZONE UVC 5M CAMERA");
				if (log.IsDebugEnabled) {
				    log.Debug("GET Camera: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET Camera: " + value);
				}
				_config.writeString("CAMERA", "camera", value);
			}
		}

		/// <summary>
		/// The camera resolutiont to capture images at
		/// </summary>
		public string Resolution {
			get {
				string val =  _config.getString("CAMERA", "resolution", "2048x1536");
				if (log.IsDebugEnabled) {
				    log.Debug("GET Resolution: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET Resolution: " + value);
				}
				_config.writeString("CAMERA", "resolution", value);
			}
		}

		/// <summary>
		/// The shooting string that should be used for the capture
		/// </summary>
		public string ShootString {
			get {
				string val =  _config.getString("SHOOTING", "shootstring", "");
				if (log.IsDebugEnabled) {
				    log.Debug("GET ShootString: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET ShootString: " + value);
				}
				_config.writeString("SHOOTING", "shootstring", value);
			}
		}

		/// <summary>
		/// The JPEG image quality to save images at (default 100)
		/// </summary>
		public int JpegQuality {
			get {
				int val =  _config.getInt("SHOOTING", "quality", 100);
				if (log.IsDebugEnabled) {
				    log.Debug("GET JpegQuality: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET JpegQuality: " + value);
				}
				_config.writeInt("SHOOTING", "quality", value);
			}
		}

		/// <summary>
		/// Should we use motion detection to wait for the camera arm to stop moving before we take a picture.
		/// Default is true
		/// </summary>
		public bool UseMotionDetection {
			get {
				bool val =  _config.getBool("SHOOTING", "useMotionDetection", true);
				if (log.IsDebugEnabled) {
				    log.Debug("GET UseMotionDetection: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET UseMotionDetection: " + value);
				}
				_config.writeBool("SHOOTING", "useMotionDetection", value);

			}
		}

		/// <summary>
		/// How sensistive to motion should we be. i is hypersensitive, 2 is very sensitive, 3 is sensitive, 4 and up are
		/// increasingly less sensitive.  Default is 1
		/// </summary>
		public int MotionSensitivity {
			get {
				int val =  _config.getInt("SHOOTING", "motionSensitivity", 1);
				if (log.IsDebugEnabled) {
				    log.Debug("GET MotionSensitivity: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET MotionSensitivity: " + value);
				}
				_config.writeInt("SHOOTING", "motionSensitivity", value);

			}
		}

		/// <summary>
		/// When taking a picture, should we use median stackign to try and reduce noise levels by actually taking five picture
		/// in rapid successiona nd then useing median stackign to merge the five images?  Default is true.
		/// </summary>
		public bool UseMedianStacking {
			get {
				bool val =  _config.getBool("SHOOTING", "useMedianStacking", false);
				if (log.IsDebugEnabled) {
				    log.Debug("GET UseMedianStacking: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET UseMedianStacking: " + value);
				}
				_config.writeBool("SHOOTING", "useMedianStacking", value);
			}
		}

		/// <summary>
		/// Should we try and enhance the contrast of the captured image, using a sigmoidal constrast enhancement?
		/// Default is false.
		///
		/// For more details on the transform, see http://www.imagemagick.org/Usage/color_mods/#sigmoidal
		/// </summary>
		public bool UseContrastEnhancement {
			get {
				bool val =  _config.getBool("SHOOTING", "useContrastEnhancement", false);
				if (log.IsDebugEnabled) {
				    log.Debug("GET UseConstrastEnhancement: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET UseContrastEnhancement: " + value);
				}
				_config.writeBool("SHOOTING", "useContrastEnhancement", value);
			}
		}

		/// <summary>
		/// If we are using contrast enhancement, select the midpoint of the transform function.
		/// Default is 50
		/// </summary>
		public int ContrastMidpoint {
			get {
				int val =  _config.getInt("SHOOTING", "contrastMidpoint", 50);
				if (log.IsDebugEnabled) {
				    log.Debug("GET ConstrastMidpoint: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET ContrastMidpoint: " + value);
				}
				_config.writeInt("SHOOTING", "contrastMidpoint", value);
			}
		}

		/// <summary>
		/// If we are using contrast enhancement, select the amount of transform to apply.  Anything more then 5
		/// is a lot of enhancement.  Default is 2
		/// </summary>
		public int ContrastValue {
			get {
				int val =  _config.getInt("SHOOTING", "contrastValue", 2);
				if (log.IsDebugEnabled) {
				    log.Debug("GET ConstrastValue: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET ConstrastValue: " + value);
				}
				_config.writeInt("SHOOTING", "contrastValue", value);
			}
		}

		/// <summary>
		/// Do we want to set the EXIF info or not?
		/// </summary>
		public bool DoSetEXIF {
			get {
				bool val =  _config.getBool("SHOOTING", "doSetEXIF", true);
				if (log.IsDebugEnabled) {
				    log.Debug("GET DoSetEXIF: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET DoSetEXIF: " + value);
				}
				_config.writeBool("SHOOTING", "doSetEXIF", value);
			}
		}

		/// <summary>
		/// The host to upload the completed scan to.
		/// </summary>
		public string SftpHost {
			get {
				string val =  _config.getString("SFTP", "host", "prod.3dwares.co");
				if (log.IsDebugEnabled) {
				    log.Debug("GET SftpHost: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET SftpHost: " + value);
				}
				_config.writeString("SFTP", "host", value);
			}
		}

		/// <summary>
		/// The user account on <see cref="SftpHost"/> to use for the transfer
		/// </summary>
		public string SftpUser {
			get {
				string val =  _config.getEncryptedString("SFTP", "user", "scanneruser");
				if (log.IsDebugEnabled) {
				    log.Debug("GET SftpUser: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET SftpUser: " + value);
				}
				_config.writeEncryptedString("SFTP", "user", value);
			}
		}

		/// <summary>
		/// The password for <see cref="SftpUser"/>
		/// </summary>
		public string SftpPassword {
			get {
				string val =  _config.getEncryptedString("SFTP", "pass", "SugarCube4u!");
				if (log.IsDebugEnabled) {
				    log.Debug("GET SftpPassword: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET SftpPassword: " + value);
				}
				_config.writeEncryptedString("SFTP", "pass", value);
			}
		}

		/// <summary>
		/// The email address of the user who is running the scan
		/// </summary>
		public string UserEmail {
			get {
				string val =  _config.getString("SFTP", "email", "user@soundfit.me");
				if (log.IsDebugEnabled) {
				    log.Debug("GET UserEmail: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET UserEmail: " + value);
				}
				_config.writeString("SFTP", "email", value);
			}
		}

		/// <summary>
		/// The prefix to use for the shoot ID, combined with a datestamp to create the final shoot ID and folder
		/// name.  The only characters allowed in the prefix are A-Z, a-z, 0-9 and -
		///
		/// NOTE: If the ShootPrerfix that is stored in the config file contains illegal characters, they are stripped out and replaced by
		/// dashes.  If the user tries to set a prefix with invalid characters an exception is thrown
		/// </summary>
		public string ShootPrefix {
			get {
				string prefix = _config.getString("SFTP", "prefix", "");
				prefix = Regex.Replace(prefix, "[^A-Za-z0-9-]", "-");
				if (log.IsDebugEnabled) {
				    log.Debug("GET ShootPrefix: " + prefix);
				}
				return prefix;
			} set {
				if (Regex.IsMatch(value, "[^A-Za-z0-9-]")) {
					if (log.IsErrorEnabled) {
					    log.Error("ShootPrefix contains illegal characters: " + value);
					}
					throw new ArgumentException("The ShootPrefix can only contain the characters A-Z, a-z, 0-9 and -");
				}
				if (log.IsDebugEnabled) {
				    log.Debug("SET ShootPrefix: " + value);
				}
				_config.writeString("SFTP", "prefix", value);
			}
		}

		/// <summary>
		/// The root path on <see cref="SftpHost"/> to upload the shoot to. Will be combined with the shoot ID to get
		/// the final path.
		/// </summary>
		public string SftpUploadBase {
			get {
				string val =  _config.getEncryptedString("SFTP", "uploadbase", "/home/scanneruser/prod.3dwares.co/Scans/b3/new/");
				if (log.IsDebugEnabled) {
				    log.Debug("GET SftpUploadBase: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET SftpUploadBase: " + value);
				}
				_config.writeEncryptedString("SFTP", "uploadbase", value);
			}
		}

		/// <summary>
		/// The URL to call to trigger the scan processing on the SoundFit servers.  The actual path the shoot was uploaded to is
		/// passed in as a parameter to the URL
		/// </summary>
		public string TriggerURL {
			get {
				string val =  _config.getEncryptedString("SFTP", "triggerURL", @"http://prod.3dwarest.co/scan.php?uploadDir=");
				if (log.IsDebugEnabled) {
				    log.Debug("GET TriggerUrl: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET TriggerUrl: " + value);
				}
				_config.writeEncryptedString("SFTP", "triggerURL", value);
			}
		}

		/// <summary>
		/// Do we want to crop the images before sending them to ReCap.  Default is false
		/// </summary>
		public bool DoImageCropping {
			get {
				bool val =  _config.getBool("MODELING_OPTIONS", "doImageCropping", false);
				if (log.IsDebugEnabled) {
				    log.Debug("GET DoImageCropping: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET DoImageCropping: " + value);
				}
				_config.writeBool("MODELING_OPTIONS", "doImageCropping", value);
			}
		}

		/// <summary>
		/// The crop rectangle to use on the images if <see cref="DoImageCropping"/> is true
		/// </summary>
		public string ImageCropRectangle  {
			get {
				string val =  _config.getString("MODELING_OPTIONS", "imageCropRectangle", "450,340,1310,1190");
				if (log.IsDebugEnabled) {
				    log.Debug("GET ImageCropRectangle: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET ImageCropRectange: " + value);
				}
				_config.writeString("MODELING_OPTIONS", "imageCropRectangle", value);
			}
		}

		/// <summary>
		/// Do we want to scale the models?  Default is true.
		/// </summary>
		public bool DoModelScaling {
			get {
				bool val =  _config.getBool("MODELING_OPTIONS", "doModelScaling", true);
				if (log.IsDebugEnabled) {
				    log.Debug("GET DoModelScaling: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET DoModelScaling: " + value);
				}
				_config.writeBool("MODELING_OPTIONS", "doModelScaling", value);
			}

		}

		/// <summary>
		/// Do we want to crop the generated model?  Default is true.  Should be false if
		/// <see cref="DoModelScaling"/> is false.
		/// </summary>
		public bool DoModelCropping {
			get {
				bool val =  _config.getBool("MODELING_OPTIONS", "doModelCropping", true);
				if (log.IsDebugEnabled) {
				    log.Debug("GET DoModelCropping: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET DoModelCropping: " + value);
				}
				_config.writeBool("MODELING_OPTIONS", "doModelCropping", value);
			}
		}

		/// <summary>
		/// The bounding box for model cropping.
		/// </summary>
		public string BoundingBox {
			get {
				string val =  _config.getString("MODELING_OPTIONS", "boundingBox", "-40,-40,5,40,40,40");
				if (log.IsDebugEnabled) {
				    log.Debug("GET BoundingBox: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET BoundingBox: " + value);
				}
				_config.writeString("MODELING_OPTIONS", "boundingBox", value);
			}
		}

		/// <summary>
		/// The mesh level (complexity) of the generated model.  7 is normal, 8 is high and 9 is highest.
		/// Default is 7
		/// </summary>
		public int MeshLevel {
			get {
				int val =  _config.getInt("MODELING_OPTIONS", "meshlevel", 7);
				if (log.IsDebugEnabled) {
				    log.Debug("GET MeshLevel: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET MeshLevel: " + value);
				}
				_config.writeInt("MODELING_OPTIONS", "meshlevel", value);
			}
		}

		/// <summary>
		/// The name of the 3DP file to use as a template for the model.
		/// NOTE: This is deprecated, we now auto-generate the template based on the actual scan.
		/// </summary>
		public string Template3DP {
			get {
				string val =  _config.getString("MODELING_OPTIONS", "template3dp", "");
				if (log.IsDebugEnabled) {
				    log.Debug("GET Template3DP: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET Template3DP: " + value);
				}
				_config.writeString("MODELING_OPTIONS", "template3dp", value);
			}
		}

		/// <summary>
		/// Get the password to use with the external encryption software we use for encrypting our PDF order forms
		/// </summary>
		public string HCryptPassword {
			get {
				string val =  _orderPassword.getEncryptedString("SECURITY", "hcryptpass", "abc123");
				if (log.IsDebugEnabled) {
				    log.Debug("GET HCryptPassword: " + val);
				}
				return val;
			} set {
				if (log.IsDebugEnabled) {
				    log.Debug("SET HCryptPassword: " + value);
				}
				_orderPassword.writeEncryptedString("SECURITY", "hcryptpass", value);
			}
		}


		/// <summary>
		/// Get a string dictionary containing the names and descriptions of all the values we expect to find in the z string.
		/// The keys are the same as the keys in the Z string and the values are the user friendly description of that the
		/// keys mean
		/// </summary>
		public StringDictionary ZKeyDescriptions {
			get {
				StringDictionary result = _config.getSection("ZSTRING");
				if (result != null) {
					if (log.IsDebugEnabled) {
					    log.Debug("GET ZKeyDescriptions, found " + result.Count + " descriptions");
					}
					return result;
				} else {
					if (log.IsDebugEnabled) {
					    log.Debug("GET ZKeyDescriptions, none found");
					}
					return new StringDictionary();
				}
			} set {
				foreach (string key in value.Keys) {
					if (log.IsDebugEnabled) {
					    log.Debug("Saving " + value.Count + " ZKEY descriptions");
					}
					
					_config.writeString("ZSTRING", key, value[key]);
				}
			}
		}

		/// <summary>
		/// For a full description of how this function works, see http://msdn.microsoft.com/en-us/library/01escwtf%28v=vs.100%29.aspx
		///
		/// Note that the IsValidEmail method does not perform authentication to validate the email address. It merely determines whether
		/// its format is valid for an email address.
		/// </summary>
		/// <param name="eMail">The email address to check</param>
		/// <returns>True if the email format is valid, false if it is not</returns>
		public bool IsValidEmail(string eMail) {
			if (log.IsDebugEnabled) {
			    log.Debug("Checking if this is a valid email address: " + eMail);
			}
			
			_isValidEmail = true;
			if (String.IsNullOrEmpty(eMail)) {
				if (log.IsDebugEnabled) {
				    log.Debug("Email is empty");
				}
				
				return false;
			}

			// Use IdnMapping class to convert Unicode domain names.
			eMail = Regex.Replace(eMail, @"(@)(.+)$", this.DomainMapper);
			if (!_isValidEmail) { //may be set as a side effect of the call to DomainMapper
				if (log.IsDebugEnabled) {
				    log.Debug("DomainMapper set _isValidEmail to false");
				}
				
				return false;
			}
			if (log.IsDebugEnabled) {
			    log.Debug("Post DomainMapper, email is now " + eMail);
			}
			
			// Return true if strIn is in valid e-mail format.
			_isValidEmail = Regex.IsMatch(eMail,
										@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
										@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
										RegexOptions.IgnoreCase);
			if (log.IsDebugEnabled) {
			    log.Debug("Result if email validity check is " + _isValidEmail);
			}
			return _isValidEmail;
		}

		/// <summary>
		/// For a full description of how this function works, see http://msdn.microsoft.com/en-us/library/01escwtf%28v=vs.100%29.aspx
		/// </summary>
		private string DomainMapper(Match match) {
			// IdnMapping class with default property values.
			IdnMapping idn = new IdnMapping();

			string domainName = match.Groups[2].Value;
			try {
				domainName = idn.GetAscii(domainName);
			} catch (ArgumentException ex) {
				if (log.IsDebugEnabled) {
				    log.Debug("Could not call GetAscii for domainName" + domainName, ex);
				}
				
				_isValidEmail = false;
			}
			return match.Groups[1].Value + domainName;
		}

		/// <summary>
		/// Simple utility to turn an email into an acceptable shoot prefix
		/// </summary>
		/// <param name="email">The email address to use</param>
		/// <returns>An acceptable prefix string</returns>
		public string EmailToPrefix(string email) {
			string res = Regex.Replace(email, "[^A-Za-z0-9-]", "-");
			if (log.IsDebugEnabled) {
			    log.Debug("Converted email " + email + " to prefix " + res);
			}
			return res;
		}
		
		
		/// <summary>
		/// Returns the results of the last call to CheckKeyIsValidForScanID, intended to be used in a KeyPress event handler
		/// for a Scan ID text box like so:
		/// <code>
		/// void ScanIDPrefixKeyPress(object sender, KeyPressEventArgs e) {
		///    e.Handled = !_config.IsValidKeyForScanID;
		///    if (e.Handled) {
		///    MessageBox.Show("Scan ID Prefix can only contain the characters A-Z, a-z, 0-9 and -", "Configuration Error",
		///                     MessageBoxButtons.OK, MessageBoxIcon.Error);
		///    }
		/// }
		/// </code>
		/// </summary>
		public bool IsValidKeyForScanID {
			get {
				return _isValidKeyForScanID;
			}
		}

		/// <summary>
		/// Checks if the key that was just pressed is a valid key for a Scan ID.  Intended to be called in a KeyDown event handler
		/// for a Scan ID text box, like so:
		/// <code>
		/// void ScanIDPrefixKeyDown(object sender, KeyEventArgs e) {
		///    _config.CheckKeyIsValidForScanID(e);
		/// }
		/// </code>
		/// Once the check has been done, you also need to use IsValidKeyForScanID in the KeyPress event handler to process the results of
		/// this check
		/// </summary>
		/// <param name="e"></param>
		public void CheckKeyIsValidForScanID(KeyEventArgs e) {
			// Ignore the initial press of the Shift, Control and Alt (menu) keys
			if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu) {
				_isValidKeyForScanID = true;
				return;
			}

			// Assume that the key press is invalid
			_isValidKeyForScanID = false;

			// handle the user trying to select all, copy, cut or paste
			if (e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X)) {
				_isValidKeyForScanID = true;
				return;
			}

			// allow backspace
			if (e.KeyCode == Keys.Back) {
				_isValidKeyForScanID = true;
				return;
			}

			// check for characters (and number pad minus), we don't bother to check if shift is pressed
			if ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z) || e.KeyCode == Keys.Subtract) {
				_isValidKeyForScanID = true;
				return;
			}

			// now check our numbers (and top of keyboard dash), and we need to make sure that shift is NOT pressed
			if (!e.Shift && ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) || (e.KeyCode == Keys.OemMinus))) {
				_isValidKeyForScanID = true;
				return;
			}
			
			// finally, allow underscore
			if (e.Shift && e.KeyCode == Keys.OemMinus) {
				_isValidKeyForScanID = true;
				return;
			}
		}

	}
}
