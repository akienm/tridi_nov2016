<?php	     
session_start();
if(isset($_SESSION['user']))
    {
        $user = $_SESSION['user'];
        echo "$user";
    }
else
    {
    }


echo "\n------- check-for-new-scans-and-launch-ReCap-v7c.php -----------------------------------\n\n";
	     
// version 7 check-for-new-scans.php  -- Written by Scott L. McGregor, SoundFit LLC, 06/17/2013
// revised 02/7/2014
//
// This program is launched as a cronjob periodically (every 10 min) 
// to check for new scanfiles that may have been uploaded since it last run.
//
// Operations:
//
// 1) Program gets a list of folders in the ~/test.soundfit.me/Scans/b2/new/ directory.
// 2) each folder of the form name@domain is openned and all subfolders inside are examined
// 2b) Each subfolder is openned and ORDERS*.pdf file is searched for.  The folder name is the scanID.
// 3) if ORDERS*.pdf file is not found, the folder may still be in the process of being uploaded,
//    so nothing more is done for now  -- future use file locking, and check for folders that 
//    remain incomplete for more than 24 hours.
// 4) if ORDERS*.pdf file is found and more than 2 minutes old, 
//    the folder is presumed completely uploaded.
// 5) completely uploaded folders are MOVED to the ~/test.soundfit.me/Scans/au1/received directory for processing.
// 6) The PHP program  ReCapLaunchModeling-wScale+Crop5.php is launched, with the proper scanID, 
//    and the sender and recipient emails are set to user@domain
// 6) An email is composed containing a URL pointing to the new folder in:
//    ~/earchives.soundfit.me/Scans/au1/recieved/orderID/
// 7) Email is sent to user@domain acknowledging the new model
// 8) Program closes successfully and waits until chron restarts it again.

// ----------------  DEFINITIONS AND GLOBAL VARIABLES ----------------------------
define ('ROOT_DIR', '/home/earchivevps/test.soundfit.me/Scans/b2/');
define ('NEW_DIR', '/home/earchivevps/test.soundfit.me/Scans/b2/new/');
define ('RECEIVED_DIR', '/home/earchivevps/test.soundfit.me/Scans/b2/received/');
define ('OLD_DIR', '/home/earchivevps/test.soundfit.me/Scans/b2/old/');
define ('RECEIVED_URL', 'http://test.soundfit.me/Scans/b2/received');
define ('LAUNCH_PGM', '/home/earchivevps/test.soundfit.me/apps/_recap/ReCapLaunchModeling-wScale+Crop7c.php');
define ('INDEX_FILE_TEMPLATE', "/home/earchivevps/test.soundfit.me/templates/gallery.php");
define ('TEMPLATE3DP', '/home/earchivevps/test.soundfit.me/Scans/b2/templates/SLMb1u4r7-007830L-00j.3dp');  // default template file 
// Use these default email addresses
$from = "sugarcube-daemon@soundfit.me";
$to = "scott@soundfit.me";   // use for testing
// $to = "rnd@soundfit.me";      // use for testing 
// $to = "PCL-orders@soundfit.me";  // use for production 

// ----------------  END OF DEFINITIONS AND GLOBAL VARIABLES ----------------------


// Program gets a list of folders in the ~/earchives.soundfit.me/Scans/b2/new directory.

//  ---------------- PROCESS FILES IN NEW DIR -------------------------------------
if ($handle = opendir(NEW_DIR)) {
	
	// ------------- FOR EACH FILE IN NEW DIR ------------------------------------
	while (false !== ($entry = readdir($handle))) {
		$subfolder = NEW_DIR.$entry;
		
		// ------------- FOR EACH FILE THAT IS A FOLDER --------------------------------
		if (is_dir($subfolder)) {  // we only process directories
			
			// ------------- FOR EACH FOLDER OTHER THAN CURRENT OR PARENT --------------
			if ($entry != "." && $entry != "..") {
				echo "Scanning ".$entry." \n";
				$to = $entry;
				
				if ($handle2 = opendir($subfolder)) {
	
					// ------------- FOR EACH FILE IN NEW DIR SUBFOLDER -----------------------------------
					while (false !== ($entry2 = readdir($handle2))) {
						$subfolder2 = $subfolder."/".$entry2;
		
						// ------------- FOR EACH FILE THAT IS A SUBFOLDER --------------------------------
						if (is_dir($subfolder2)) {  // we only process directories
			
							// ------------- FOR EACH SUBFOLDER OTHER THAN CURRENT OR PARENT --------------
							if ($entry2 != "." && $entry2 != "..") {
								echo "    ".$entry2."\n";
								$order_glob_string = $subfolder2."/ORDER*.pdf*";
								// echo "order_glob_string = $order_glob_string\n";
								
								// Each folder is opened and an ORDERS*.pdf file is searched for.
								$orderfile = glob($order_glob_string);
								
								// --------- FOR FOLDERS WITH ORDER FILES ------------------------------
				
								if (count($orderfile) > 0) {  // ---- IF THERE IS AN ORDER FILE ----------
									echo "        ".basename($orderfile[0]). "\n";
									// if an order form exists we can proceed, otherwise we'll try again later.. 
									$now = date("U");
									$filemod = filemtime($orderfile[0]);
									$secssincelastchange = $now - $filemod;
														
									// --------- WHEN ORDER FILE IS RECENT, TRY LATER -----------------
									// if ORDERS*.pdf file is found and more than 2 minutes old,
									//  the folder is presumed completely uploaded.
									if ($secssincelastchange < 120)  { // -------- ORDER FILE IS NOT OLD ENOUGH
									    // echo "lastchange = ".$lastchange."<br />";
										// not old enough  -- go to next file
										echo "Order file not old enough. Files may still be in transit. Will try again later.\n";
										//sleep(120 - lastchange);	
										// echo "lastchange = ".$lastchange."<br />";		
									} // -------- END OF ORDER FILE IS NOT OLD ENOUGH
									else { // -------- ORDER FILE IS OLD ENOUGH
									
										//  set the default SENDER and RECIPIENT if none is defined
										//  Use the $to value set above as the default
										$sender = $to;     // use the folder name as the default
										$recipient =  $to;  // use the folder name as the default
										
										$meshlevel = 7;  // default setting
										$template3DP = TEMPLATE3DP;  // default setting
										//  Check if there is an XML file in the same directory
										//  that might over-ride the defaults and set actual SENDER and RECIPIENTS.
										$xmlfiles = glob($subfolder2."/SoundFit*.xml");
										echo "XML FILES FOUND\n ";
										print_r($xmlfiles);
										if (count($xmlfiles) > 0)  {  // there is at least one XML file in the directory
											echo "XML FILES FOUND IN CURRENT DIRECTORY\n";
											print_r($xmlfiles);
											for ($i = 0 ; $i < count($xmlfiles) ; $i++ ) {   // ------ FOR EACH XML FILE  ---------
												$xml =  simplexml_load_file($xmlfiles[$i]);
												
												// check if the XML file sets a SENDER for all emails
												if ($xml -> sender ) {
													$sender = $xml -> sender;
													echo "            SENDER found in local XML file: $sender \n";
												}
												
												// check if the XML file sets a RECIPIENT for all emails
												if ($xml -> recipient ) {
													$recipient = $xml -> recipient;
													echo "            RECIPIENT found in local XML file: $recipient \n";
												}
												
												// check if the XML file sets a MESHLEVEL 
												if ($xml -> meshlevel ) {
													$meshlevel = $xml -> meshlevel;
													echo "            MESHLEVEL found in local XML file: $meshlevel \n";
												} 
													
												// check if the XML file sets a TEMPLATE3DP file
												if ($xml -> template3DP ) {
													$template3DP = $xml -> template3DP;
													echo "            TEMPLATE3DP found in local XML file: $template3DP \n";
												}
												
												// check if the XML file sets a BOUNDINGBOX
													if ($xml -> boundingBox ) {
													$boundingBox = $xml -> boundingBox;
													echo "            BOUNDINGBOX found in local XML file: $template3DP \n";
												}	
																									
												// check if the XML file sets  DOCROPPING switch
													if ($xml -> doScaling ) {
													$doScaling = $xml -> doScaling;
													echo "            DOSCALING found in local XML file: $doScaling \n";
												}	
																									
												// check if the XML file sets  DOCROPPING switch
													if ($xml -> doCropping ) {
													$doCropping = $xml -> doCropping;
													echo "            DOCROPPING found in local XML file: $doCropping \n";
												}	
													
											} // ------ END OF FOR EACH XML FILE  ---------
											
										} else {  // If the XML file doesn't exist in this folder, we will try the parent folder
											$xmlfiles = glob($subfolder2."/../SoundFit*.xml");
											if (count($xmlfiles) > 0)  {  // there is at least one XML file in the directory
												echo "XML FILES FOUND IN PARENT DIRECTORY\n";
												print_r($xmlfiles);
												for ($i = 0 ; $i < count($xmlfiles) ; $i++ ) {   // ------ FOR EACH XML FILE  ---------
													$xml =  simplexml_load_file($xmlfiles[$i]);
												
													// check if the XML file sets a SENDER for all emails
													if ($xml -> sender ) {
														$sender = $xml -> sender;
														echo "            SENDER found in parent XML file: $sender \n";
													}
												
													// check if the XML file sets a RECIPIENT for all emails
													if ($xml -> recipient ) {
														$recipient = $xml -> recipient;
														echo "            RECIPIENT found in parent XML file: $recipient \n";
													} 
													
													// check if the XML file sets a MESHLEVEL 
													if ($xml -> meshlevel ) {
														$meshlevel = $xml -> meshlevel;
														echo "            MESHLEVEL found in parent XML file: $meshlevel \n";
													}	
																									
													// check if the XML file sets a TEMPLATE3DP file
													if ($xml -> template3DP ) {
														$template3DP = $xml -> template3DP;
														echo "            TEMPLATE3DP found in parent XML file: $template3DP \n";
													}	
													
													// check if the XML file sets a BOUNDINGBOX
														if ($xml -> boundingBox ) {
														$boundingBox = $xml -> boundingBox;
														echo "            BOUNDINGBOX found in parent XML file: $boundingBox \n";
													}	
																									
													// check if the XML file sets  DOCROPPING switch
														if ($xml -> doScaling ) {
														$doScaling = $xml -> doScaling;
														echo "            DOSCALING found in parent XML file: $doScaling \n";
													}	
																									
													// check if the XML file sets  DOCROPPING switch
														if ($xml -> doCropping ) {
														$doCropping = $xml -> doCropping;
														echo "            DOCROPPING found in parent XML file: $doCropping \n";
													}
													
												}// ------ END OF FOR EACH XML FILE  ---------
											} // ------- END OF  if(count($xmlfiles) > 0)  ------------
										} // END OF If the XML file doesn't exist in this folder, we will try the parent folder
									

										// --------- FOLDER APPEARS COMPLETE, LET'S PROCESS IT AND MOVE TO OLD -------------
										// completely uploaded folders are MOVED to the ~/test.soundfit.me/Scans/b2/old directory.
										// if old dir scan folder already exists this program will intentionally overwrite it!
										
										$scanID = $entry2; 
										echo "scanID = $scanID\n";
										
										$manifestnb = 0;
										$manifest_array = glob($subfolder2."/MANIFEST*{.txt,.TXT}",GLOB_BRACE);
										echo "scan_directory = $subfolder2\n";

										echo "found " . count($manifest_array) . " manifest files. \n";

										//  ---  DO THIS FOR EACH MANIFEST FILE
										foreach ($manifest_array as $manifestkey => $manifestvalue) 
										{	
											$manifestfilename = basename($manifestvalue);
											echo "($manifestnb): $manifestvalue \n";
	
											$scansetID = $scanID.$manifestfilename[9];
 											echo "scansetID: $scansetID \n";
										
											$dirname = $scansetID;
											$filename = RECEIVED_DIR . $dirname . "/";

											if (!file_exists($filename)) {
    											mkdir(RECEIVED_DIR . $dirname, 0777);
    											echo "The directory $dirname was successfully created in ". RECEIVED_DIR."\n";
											} else {
    											echo "The directory $dirname already exists in ". RECEIVED_DIR."\n";
											}
										    
										    echo "COPYING FILES FROM " . $subfolder2 . " TO" . RECEIVED_DIR.$scansetID. "\n\n";
										     
										    // Copy the ORDER files  
											$order_array = glob($subfolder2."/ORDER*");
											foreach ( $order_array as $orderkey => $ordervalue)
											{
												copy ($ordervalue ,RECEIVED_DIR.$scansetID."/".basename($ordervalue));
												echo "COPIED ORDERFILE " . $ordervalue . " TO " . RECEIVED_DIR.$scansetID."/".basename($ordervalue). "\n\n";
											}
											
											//Copy the XML files
											$xml_array = glob($subfolder2."/*{.xml,.XML}",GLOB_BRACE);
											foreach ( $xml_array as $xmlkey => $xmlvalue)
											{
												copy ($xmlvalue ,RECEIVED_DIR.$scansetID."/".basename($xmlvalue));
												echo "COPIED XML FILE " . $xmlvalue . " TO" . RECEIVED_DIR.$scansetID."/".basename($xmlvalue). "\n\n";

											}
										     
											// copy an INDEX.PHP file to the new directory from the template
											copy (INDEX_FILE_TEMPLATE, RECEIVED_DIR.$scansetID."/index.php");
											echo "COPIED INDEX FILE " . INDEX_FILE_TEMPLATE . " TO " . RECEIVED_DIR.$scansetID."/index.php". "\n\n";
				
											copy ($manifestvalue ,RECEIVED_DIR.$scansetID."/".$manifestfilename);
											echo "COPIED MANIFEST FILE " . $manifestvalue . " TO " . RECEIVED_DIR.$scansetID."/".$manifestfilename. "\n\n";

											
											// copy all the files listed within the MANIFEST FILE
											$scan_array = file( $manifestvalue, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
 											for ($idx = 0; $idx < count($scan_array) ; $idx++) 
 											{
 												copy( $subfolder2."/".$scan_array[$idx] ,RECEIVED_DIR.$scansetID."/".$scan_array[$idx]);
 												//echo "COPIED " . $subfolder2."/".$scan_array[$idx] . " TO " . RECEIVED_DIR.$scansetID."/".$scan_array[$idx]. "\n\n";
 											}
									
											// ---------- Run ReCapLaunchModeling on the new folder --------
											//
											//   LAUNCH SHELL HERE
											//
											$shellcmd =  "/usr/local/php53/bin/php -f" . LAUNCH_PGM 
														. " scanID=" .  $scansetID 
														. " senderEmail=$sender  recipientEmail=$recipient "
														. " meshlevel=$meshlevel  template3DP=$template3DP" 
														. " doScaling=$doScaling"
														. " doCropping=$doCropping"
														. " boundingBox=$boundingBox";

											$answer = `$shellcmd`;  // $answer has the stdoutput from the invoked program
										 
										
											echo "\nReCap Modeling launched:\n";
											echo $shellcmd. "\n\n";
											echo "\n\n$answer\n\n"; 
											// ---------- Run ReCapLaunchModeling on the new folder --------
					
					
											// 6) An email is composed containing a URL pointing to the new folder in:
											//    ~/earchives.soundfit.me/Scans/au1/received/orderID/

											$neworderfile =  basename($orderfile[0]);
					
											// echo "Order File name: $neworderfile \n\n"; 
											$subject = "Automated Notification: New order: $scansetID received.";
						
											$message = "New order: " . RECEIVED_DIR.$scansetID."/$neworderfile received. "
												. "\nImages in " . RECEIVED_DIR.$scansetID. "\n\n"
												. "$answer . \n\n";
											$headers = "From: " . $from;
	  
											// 7) Email is sent to PCL-orders@soundfit.me

											mail($to,$subject,$message,$headers);
											// echo "from: "   .$from.    "\n";
											// echo "to: "     .$to.      "\n";
											// echo "subject: ".$subject. "\n";
											// echo "message: ".$message. "\n";
											// echo "headers: ".$headers. "\n\n";
											
											$manifestnb++;
											
										}  // ---------END OF DO THIS FOR ALL MANIFEST FILES 
									
										echo "scanning directory: $scan_directory \n";
											
										// MOVE THE NEW FILE SOMEWHERE ELSE SO IT ISN'T PROCEESSED AGAIN					
										// --------- FOLDER APPEARS COMPLETE, MOVE TO OLD -------------
										rename($subfolder2, OLD_DIR.$entry2);
										echo "\n MOVING " . $subfolder2. "\n TO\n" . OLD_DIR.$entry2. "\n\n";

									} // -------- END OF ORDER FILE IS OLD ENOUGH
									
								} // ---- IF THERE IS AN ORDER FILE ----------
								
							} // ------------- END OF FOR EACH SUBFOLDER OTHER THAN CURRENT OR PARENT -------
				
						} //------------- END OF FOR EACH FILE THAT IS A SUBFOLDER -------------------------
					
					}  // ------------- END OF FOR EACH FILE IN NEW DIR SUBFOLDER -----------------------------
					
				} // --------- END OF FOR FOLDERS WITH ORDER FILES -----------------------
				
			} // ------------- END OF FOR EACH FOLDER OTHER THAN CURRENT OR PARENT -------
			
		} // ------------- END OF FOR EACH FILE THAT IS A FOLDER -------------------------	
		 	
	} // ------------- END OF FOR EACH FILE IN NEW DIR -----------------------------
	
} //  ---------------- END OF PROCESS FILES IN NEW DIR ------------------------------

// Program closes successfully and waits until chron restarts it again.

echo "\n------- end check-for-new-scans-and-launch-ReCap-v7c.php -----------------------------------\n\n";

exit;

?>