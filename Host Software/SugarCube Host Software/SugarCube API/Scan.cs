/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

using log4net;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// A custom attribute we can use when defining scan data structs to allow us to serialize certain fields
	/// as CDATA rather then plain XML text
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class CData: System.Attribute {
	}

	/// <summary>
	/// General scan metadata
	/// </summary>
	public struct ScanMetadata {
		public string clientVersion;
		public string serverVersion;
		public string scanID;
		public string scanStartTime;
		public string uploadStartTime;
	}

	/// <summary>
	/// Scan details related to order information
	/// </summary>
	public struct OrderDetails {
		public string from;
		public string to;
		[CData()]
		public string subject;
		[CData()]
		public string referenceID;
		[CData()]
		public string body;
		[NonSerialized()]
		public List<string> attachments;
	}

	/// <summary>
	/// Scan data related to model generation options
	/// </summary>
	public struct ModelingOptions {
		public string doImageCropping;
		public string imageCroppingRectangle;
		public string doModelScaling;
		public string doModelCropping;
		public string boundingBox;
		public string meshlevel;
		public string template3DP;
	}

	/// <summary>
	/// Represents a single image included in the scan.  Each scan has a number of ScanImages stashed into
	/// an <see cref="ImageSet"/>
	/// </summary>
	public class ScanImage {
		public string elevation;
		public string rotation;
		public string name;

		public ScanImage(string el, string rot, string nm) {
			elevation = el;
			rotation = rot;
			name = nm;
		}
	}

	/// <summary>
	/// A set of <see cref="ScanImage"/> objects
	/// </summary>
	public struct ImageSet {
		public string id;
		public List<ScanImage> images;
	}

	/// <summary>
	/// Used for debugging, taken from DirectShow
	/// </summary>
	public struct VideoProcAmpDetails {
		public string brightness;
		public string contrast;
		public string hue;
		public string saturation;
		public string sharpness;
		public string gamma;
		public string whiteBalance;
		public string backlightComp;
		public string gain;
		public string colorEnable;
		public string powerLineFrequency;
	}

	/// <summary>
	/// Used for debugging, taken from DirectShow
	/// </summary>
	public struct CameraControlDetails {
		public string zoom;
		public string focus;
		public string exposure;
		public string aperture;
		public string pan;
		public string tilt;
		public string roll;
		public string lowLightCompensation;
	}

	/// <summary>
	/// Generally useful debug info stashed in to the scan
	/// </summary>
	public struct Diagnostics {
		[CData()]
		public string shootingString;
		public string cameraID;
		public string imageResolution;
		public string jpegQuality;
		public string COMport;
		public string forceCOMPort;
		public string motion_detection;
		public string motion_sensitivity;
		public string testerMessages;
		public string SugarCubeMessages;
		[NonSerialized()]
		public CameraControlDetails cameraConrol;
		[NonSerialized()]
		public VideoProcAmpDetails videoProcAmp;
	}


	/// <summary>
	/// This class represent a single scan session, and stores all the data associated with that scan.  This data is
	/// then used to build a Manifest.xml file that is uploaded with the scanned images
	/// </summary>
	public class Scan {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(Scan));
		
		/// <summary>
		/// The standard date format used for scan metadata: M/d/yyyy h:m:s tt G\MTz
		/// </summary>
		public const string DATE_FORMAT = @"M/d/yyyy h:m:s tt G\MTz";

		/// <summary>
		/// Our scan metadata
		/// </summary>
		public ScanMetadata metadata = new ScanMetadata();
		/// <summary>
		/// The order details
		/// </summary>
		public OrderDetails orderDetails = new OrderDetails();
		/// <summary>
		/// Our modeling options
		/// </summary>
		public ModelingOptions modelingOptions = new ModelingOptions();
		/// <summary>
		/// The set of images in this scan
		/// </summary>
		public ImageSet imageSet = new ImageSet();
		/// <summary>
		/// Diagnostic info for this scan
		/// </summary>
		public Diagnostics diagnostics = new Diagnostics();
		/// <summary>
		/// The folder where all the scan files are being created
		/// </summary>
		public string LocalScanFolder;
		/// <summary>
		/// The Z data from the cube's EEPROM
		/// </summary>
		public StringDictionary zData;

		/// <summary>
		/// Initializes the scan and any necessary metadata objects.
		/// </summary>
		public Scan() {
			if (log.IsInfoEnabled) {
			    log.Info("Initializing new Scan");
			}
			
			metadata.clientVersion = "b3";
			metadata.serverVersion = "b3-8c";
			metadata.scanStartTime = DateTime.Now.ToString(DATE_FORMAT);
			orderDetails.attachments = new List<string>();
			imageSet.images = new List<ScanImage>();
			diagnostics.cameraConrol = new CameraControlDetails();
			diagnostics.videoProcAmp = new VideoProcAmpDetails();
		}

		/// <summary>
		/// Helper function for <see cref="WriteManifest"/>.  Given a struct, will serialize all the fields to XML.
		/// Uses the <see cref="CData"/> custom attribute.
		/// </summary>
		/// <param name="dataType">The Type of the object to be serialized</param>
		/// <param name="dataSet">The object to be serialized</param>
		/// <returns></returns>
		private string GetStringValuesAsXML(Type dataType, object dataSet) {
			if (log.IsInfoEnabled) {
			    log.Info("Getting string values as XML for type " + dataType.FullName);
			}
			
			FieldInfo[] fields = dataType.GetFields();
			string buffer = "";
			foreach (FieldInfo field in fields) {
				if (!field.IsNotSerialized) {
					object[] customAttributes = field.GetCustomAttributes(false);
					if (customAttributes.Length > 0 && customAttributes[0].GetType().Name == "CData") {
						buffer = buffer + String.Format("<{0}><![CDATA[{1}]]></{0}>\n", field.Name, field.GetValue(dataSet));
					} else {
						buffer = buffer + String.Format("<{0}>{1}</{0}>\n", field.Name, field.GetValue(dataSet));
					}
				}
			}
			return buffer;
		}

		/// <summary>
		/// Generates the Manifest.XML file in <see cref="LocalScanFolder"/>.
		/// </summary>
		public void WriteManifest() {
			if (log.IsInfoEnabled) {
			    log.Info("Generating manifest XML");
			}
			
			if (String.IsNullOrWhiteSpace(LocalScanFolder)) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to generate manifest, local scan folder not set");
				}
				
				throw new ArgumentException("LocalScanFolder has not been set");
			}

			if (!Directory.Exists(LocalScanFolder)) {
				if (log.IsErrorEnabled) {
				    log.Error("Unable to generate manifest, local scan folder not found: " + LocalScanFolder);
				}
				
				throw new DirectoryNotFoundException(LocalScanFolder + " not found");
			}
			StringBuilder buffer = new StringBuilder();
			buffer.AppendLine("<SugarCubeScan manifest_version='r1.3'>");

			buffer.AppendLine("<metadata>");
			buffer.Append(GetStringValuesAsXML(typeof(ScanMetadata), metadata));
			buffer.AppendLine("</metadata>");

			buffer.AppendLine("<order_details>");
			buffer.Append(GetStringValuesAsXML(typeof(OrderDetails), orderDetails));
			buffer.AppendLine("<attachments>");
			foreach (string attachment in orderDetails.attachments) {
				buffer.AppendFormat("<filename>{0}</filename>\n", attachment);
			}
			buffer.AppendLine("</attachments>");
			buffer.AppendLine("</order_details>");

			buffer.AppendLine("<ZValues>");
			foreach (string key in zData.Keys) {
				buffer.AppendFormat("<{0}>{1}</{0}>\n", key.ToUpper(), zData[key]);
			}
			buffer.AppendLine("</ZValues>");
			
			buffer.AppendLine("<modelling_options>");
			buffer.Append(GetStringValuesAsXML(typeof(ModelingOptions), modelingOptions));
			buffer.AppendLine("</modelling_options>");


			buffer.AppendFormat("<imageset id='{0}'>\n",imageSet.id);
			foreach (ScanImage image in imageSet.images) {
				buffer.AppendFormat("<image elevation='{0}' rotation='{1}'>{2}</image>\n", image.elevation, image.rotation, image.name);
			}
			buffer.AppendLine("</imageset>");

			buffer.AppendLine("<diagnostics>");
			buffer.Append(GetStringValuesAsXML(typeof(Diagnostics), diagnostics));
			buffer.AppendLine("<videoProcAmp>");
			buffer.Append(GetStringValuesAsXML(typeof(VideoProcAmpDetails), diagnostics.videoProcAmp));
			buffer.AppendLine("</videoProcAmp>");
			buffer.AppendLine("<cameraControl>");
			buffer.Append(GetStringValuesAsXML(typeof(CameraControlDetails), diagnostics.cameraConrol));
			buffer.AppendLine("</cameraControl>");
			buffer.AppendLine("</diagnostics>");

			buffer.AppendLine("</SugarCubeScan>");

			string fileName = Path.Combine(LocalScanFolder, "Manifest.xml");
			if (log.IsInfoEnabled) {
			    log.Info("Writing manifest to file " + fileName);
			}
			
			File.WriteAllText(fileName, buffer.ToString());
		}
	}
}
