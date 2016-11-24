/* SugarCube Host Software - SugarCube API
 * Copyright (c) 2014-2015 Chad Ullman
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Camera_NET;
using DirectShowLib;
using ImageMagick;
using log4net;
using XnaFan.ImageComparison;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// This static class hosts a number of small utility functions for image processing
	/// </summary>
	public static class CubeImageUtils {
		/// <summary>
		/// Initalizes our static logger reference with the type name of the class
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(typeof(CubeImageUtils));
		
		/// <summary>
		/// Wait for all visible motion to settle to a level defined by the motionSensitivity parameter
		/// </summary>
		/// <param name="camControl">A reference to our webcam control to grab the source image from</param>
		/// <param name="motionSensitivity">How sensitive to motion we should be, 1 is very sensitive, greater than 4 is not very at all</param>
		private static void WaitForMotionToStop(CameraControl camControl, int motionSensitivity) {
			if (log.IsInfoEnabled) {
			    log.Info("WaitForMotionToStop called with sensitivity threshold of " + motionSensitivity);
			}
			
			byte sensitivity = Convert.ToByte(motionSensitivity);
			Bitmap snap1 = camControl.SnapshotSourceImage();
			Thread.Sleep(80);
			Bitmap snap2 = camControl.SnapshotSourceImage();
			float diff = snap1.PercentageDifference(snap2, sensitivity);
			if (log.IsDebugEnabled) {
			    log.Debug("Initial image diff is " + diff);
			}
			
			while (diff > 0) {
				snap1 = snap2;
				Thread.Sleep(80);
				snap2 = camControl.SnapshotSourceImage();
				diff = snap1.PercentageDifference(snap2, sensitivity);
				if (log.IsDebugEnabled) {
				    log.Debug("Current image diff is " + diff);
				}
			}
			// Things have stabilized, time to take the actual photo
		}

		/// <summary>
		/// Capture the actual image.  If useMedianMerging is true, calls out to <see cref="CaptureUsingMedianStacking"/> to get the
		/// merged image.  If useContrastEnhancement is true, calls <see cref="ContrastEnhanceImage"/> before returning.
		/// </summary>
		/// <param name="filePath">The path to save the image to (used for debugging)</param>
		/// <param name="camControl">A reference to our webcam control to grab the source image from</param>
		/// <param name="useMedianMerging">Do we want to use median stacking to reduce noise</param>
		/// <param name="useContrastEnhancement">Do we want to apply sigmoidal contrast enhancement</param>
		/// <param name="contrast">The contrast level to use</param>
		/// <param name="midpoint">The midpoint to use for contrast enhancement</param>
		/// <returns>The snapshot as a MagickImage object</returns>
		private static MagickImage CaptureImage(string filePath, CameraControl camControl, bool useMedianMerging, bool useContrastEnhancement, double contrast, double midpoint) {
			if (log.IsInfoEnabled) {
			    log.InfoFormat("Capture image called, param values: filePath = {0}; useMedianMerging = {1}, useContrastEnhancement = {2}, contrast = {3}, midpoint = {4}",
				                new Object[] {filePath, useMedianMerging, useContrastEnhancement, contrast, midpoint});
			}
			
			MagickImage theImage;
			if (useMedianMerging) {
				theImage = CaptureUsingMedianStacking(filePath, camControl);
			} else {
				theImage = new MagickImage(camControl.SnapshotSourceImage());
				/*
				    #if DEBUG
				    	theImage.Quality = 100;
				    	theImage.Write(filePath + "-simple.jpeg");
				    #endif
				    */
			}

			if (useContrastEnhancement) {
				ContrastEnhanceImage(ref theImage, contrast, midpoint);
				/*
				#if DEBUG
					theImage.Quality = 100;
					theImage.Write(filePath + "-contrast-enhanced.jpeg");
				#endif
				*/
			}

			return theImage;
		}

		/// <summary>
		/// Takes five images 100ms apart and then merges then using a median merge to reduce image noise
		/// </summary>
		/// <param name="filePath">The path to save the interim images to (for debugging)</param>
		/// <param name="camControl">A reference to our webcam control to grab the source image from</param>
		/// <returns>The snapshot as a MagickImage object</returns>
		private static MagickImage CaptureUsingMedianStacking(string filePath, CameraControl camControl) {
			MagickImage theImage;
			if (log.IsInfoEnabled) {
			    log.Info("Capturing image using median stacking");
			}
			
			using (MagickImageCollection images = new MagickImageCollection()) {
				for (int i = 0; i < 2; i++) {
					images.Add(new MagickImage(camControl.SnapshotSourceImage()));
					Thread.Sleep(50);
				}
				images.Add(new MagickImage(camControl.SnapshotSourceImage()));
				theImage = images.Evaluate(EvaluateOperator.Median);
				/*
				#if DEBUG
					for (int i = 0; i < 5; i++) {
				   	images[i].Quality = 100;
				   	images[i].Write(filePath + "-merge-source-" + i + ".jpeg");
					}
					theImage.Quality = 100;
					theImage.Write(filePath + "-median-merged.jpeg");
				#endif
				*/
			}

			return theImage;
		}

		/// <summary>
		/// Given an image, performs a sigmoidal contrast enhancement on that image
		/// </summary>
		/// <param name="theImage"></param>
		/// <param name="contrast">The contrast level to use</param>
		/// <param name="midpoint">The midpoint to use for contrast enhancement</param>
		private static void ContrastEnhanceImage(ref MagickImage theImage, double contrast, double midpoint) {
			if (log.IsInfoEnabled) {
			    log.Info("Enhancing image contrast using contrast value of " + contrast + " and midpoint of " + midpoint);
			}
			
			theImage.SigmoidalContrast(false, contrast, midpoint);
		}

		/// <summary>
		/// Takes a snapshot from the webcam stream.  No motion detection, median merging or contrast enhancement
		/// </summary>
		/// <param name="filePath">The path to save the image to</param>
		/// <param name="camControl">A reference to our webcam control to grab the source image from</param>
		/// <param name="imageQuality">The JPEG image quality</param>
		public static void TakeSnapshot(string filePath, CameraControl camControl, int imageQuality) {
			TakeSnapshot(filePath, camControl, imageQuality, false, 0, false, false, 0, 0);

		}

		/// <summary>
		/// Takes a snapshot from the webcam stream.  Can use motion detection and set sensitivity, but no median merging or contrast
		/// enhancement
		/// </summary>
		/// <param name="filePath">The path to save the image to</param>
		/// <param name="camControl">A reference to our webcam control to grab the source image from</param>
		/// <param name="imageQuality">The JPEG image quality</param>
		/// <param name="useMotionDetection">True if we want to use motion detection to wait for stability before taking the image</param>
		/// <param name="motionSensitivity">How sensitive to motion we should be, 1 is very sensitive, greater than 4 is not very at all</param>
		public static void TakeSnapshot(string filePath, CameraControl camControl, int imageQuality, bool useMotionDetection, int motionSensitivity) {
			if (log.IsInfoEnabled) {
			    log.Info("TakeSnapshot called with no median merging or contrast enhancement");
			}
			
			TakeSnapshot(filePath, camControl, imageQuality, useMotionDetection, motionSensitivity, false, false, 0, 0);
		}

		/// <summary>
		/// Takes a snapshot from the webcam stream.  You can set motion detection and median merging but no contrast enhancement
		/// </summary>
		/// <param name="filePath">The path to save the image to</param>
		/// <param name="camControl">A reference to our webcam control to grab the source image from</param>
		/// <param name="imageQuality">The JPEG image quality</param>
		/// <param name="useMotionDetection">True if we want to use motion detection to wait for stability before taking the image</param>
		/// <param name="motionSensitivity">How sensitive to motion we should be, 1 is very sensitive, greater than 4 is not very at all</param>
		/// <param name="useMedianMerging">Do we want to use median stacking to reduce noise</param>
		public static void TakeSnapshot(string filePath, CameraControl camControl, int imageQuality, bool useMotionDetection, int motionSensitivity,  bool useMedianMerging) {
			if (log.IsInfoEnabled) {
			    log.Info("TakeSnaphshot called with no contrast enahancement");
			}
			
			TakeSnapshot(filePath, camControl, imageQuality, useMotionDetection, motionSensitivity, useMedianMerging, false, 0, 0);
		}


		/// <summary>
		/// Takes a snapshot from the webcam stream.  You can just take a simple grab of what's in the stream, or use a combination of
		/// motion detection, median image stacking and/or contrast enhancement to try and perfect the image
		/// </summary>
		/// <param name="filePath">The path to save the image to</param>
		/// <param name="camControl">A reference to our webcam control to grab the source image from</param>
		/// <param name="imageQuality">The JPEG image quality</param>
		/// <param name="useMotionDetection">True if we want to use motion detection to wait for stability before taking the image</param>
		/// <param name="motionSensitivity">How sensitive to motion we should be, 1 is very sensitive, greater than 4 is not very at all</param>
		/// <param name="useMedianMerging">Do we want to use median stacking to reduce noise</param>
		/// <param name="useContrastEnhancement">Do we want to apply sigmoidal contrast enhancement</param>
		/// <param name="contrast">The contrast level to use</param>
		/// <param name="midpoint">The midpoint to use for contrast enhancement</param>
		public static void TakeSnapshot(string filePath, CameraControl camControl, int imageQuality, bool useMotionDetection, int motionSensitivity,  bool useMedianMerging, bool useContrastEnhancement, double contrast, double midpoint) {
			if (log.IsInfoEnabled) {
			    log.InfoFormat("TakeSnapshot called with poarameters: filePath={0}, imageQuality={1}, useMotionDetection={2}, motionSensitivity={3}, useMedianMerging={4}, useContrastEnhancement={5}, contrast={6}, midpoint={7}",
				               new Object[] {filePath, imageQuality, useMotionDetection, motionSensitivity, useMedianMerging, useContrastEnhancement, contrast, midpoint});
			}
			
			if (useMotionDetection) {
				WaitForMotionToStop(camControl, motionSensitivity);
			}

			using (MagickImage theImage = CaptureImage(filePath, camControl, useMedianMerging, useContrastEnhancement, contrast, midpoint)) {
				theImage.Quality = imageQuality;
				theImage.Write(filePath);
			}
		}

		/// <summary>
		/// Given a file path, sets a few image attributes for all JPG images in that path using exiftool.
		/// The data fields that are set are:
		/// <list type="bullet">
		/// <item><term>Focal Length In 35mm Format</term> <description>set to 38</description></item>
		/// <item><term>Resolution Unit</term> <description>set to inches</description></item>
		/// <item><term>Y Resolution</term> <description>set to 97 (dpi)</description></item>
		/// <item><term>X Resolution</term> <description>set to 97 (dpi)</description></item>
		/// </list>
		/// </summary>
		/// <param name="filePath">The path to the images to set</param>
		/// <param name="le">The 35mm Equivalent Focal Length, read from the Z string</param> 
		/// <returns>The stderr and stdout from exiftool as a string</returns>
		public static string SetExifInfo(string filePath, string le) {
			if (log.IsInfoEnabled) {
			    log.InfoFormat("Running SetExifInfo on path {0} with le of {1}", filePath, le);
			}
			
			if (!File.Exists("exiftool.exe")) {
				if (log.IsErrorEnabled) {
				    log.Error("exiftool.exe is missing");
				}
				
				throw new Exception("Unable to set EXIF data for files, exiftools.exe is missing");
			}
			Process exifprog = new Process();
			StreamReader exifstdout, exifstderr;
			StreamWriter exifstdin;
			ProcessStartInfo cmdStartInfo = new ProcessStartInfo("exiftool.exe");
			cmdStartInfo.UseShellExecute = false;
			cmdStartInfo.RedirectStandardInput = true;
			cmdStartInfo.RedirectStandardOutput = true;
			cmdStartInfo.RedirectStandardError = true;
			cmdStartInfo.CreateNoWindow = true;
			filePath = Path.Combine(filePath, "*.jpg");
			cmdStartInfo.Arguments = "-EXIF:FocalLengthIn35mmFormat=" + le + " -JFIF:ResolutionUnit=inches -JFIF:YResolution=97 -JFIF:XResolution=97 -overwrite_original \"" + filePath + "\"";
			exifprog.StartInfo = cmdStartInfo;
			if (log.IsDebugEnabled) {
			    log.Debug("Ready to start exiftoo.exe with command line: " + cmdStartInfo.Arguments);
			}
			exifprog.Start();
			exifstdin = exifprog.StandardInput;
			exifstdout = exifprog.StandardOutput;
			exifstderr = exifprog.StandardError;
			exifstdin.AutoFlush = true;
			exifstdin.Close();
			string result = exifstdout.ReadToEnd();
			string error = exifstderr.ReadToEnd();
			if (log.IsDebugEnabled) {
			    log.Debug("StdOut for exiftool: " + result);
			    log.Debug("StdErr for exiftool: " + error);
			}
			
			if (!String.IsNullOrWhiteSpace(error)) {
				result = result + "Errors: " + error;
			}
			return result;
		}

	}
}
