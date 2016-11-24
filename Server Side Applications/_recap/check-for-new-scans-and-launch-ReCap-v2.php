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


echo "\n------- check-for-new-scans-and-launch-ReCap-v2.php -----------------------------------\n\n";
	     
// check-for-new-scans.php  -- Written by Scott L. McGregor, SoundFit LLC, 06/17/2013
// revised 09/21/2013
//
// This program is launched as a cronjob periodically (every 10 min) 
// to check for new scanfiles that may have been uploaded since it last run.
//
// Operations:
//
// 1) Program gets a list of folders in the ~/test.soundfit.me/Scans/b1/new/ directory.
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
define ('ROOT_DIR', '/home/earchivevps/test.soundfit.me/Scans/b1/');
define ('NEW_DIR', '/home/earchivevps/test.soundfit.me/Scans/b1/new_');
define ('RECEIVED_DIR', '/home/earchivevps/test.soundfit.me/Scans/b1/received');
define ('RECEIVED_URL', 'http://test.soundfit.me/Scans/b1/received');
define ('LAUNCH_PGM', '/home/earchivevps/test.soundfit.me/apps/_recap/ReCapLaunchModeling-wScale+Crop5.php');
define ('INDEX_FILE_TEMPLATE', "/home/earchivevps/test.soundfit.me/templates/gallery.php");


// Use these default email addresses
$from = "sugarcube-daemon@soundfit.me";
$to = "scott@soundfit.me";   // use for testing
// $to = "rnd@soundfit.me";      // use for testing 
// $to = "PCL-orders@soundfit.me";  // use for production 

// ----------------  END OF DEFINITIONS AND GLOBAL VARIABLES ----------------------


// Program gets a list of folders in the ~/earchives.soundfit.me/Scans/b1/new directory.

//  ---------------- PROCESS FILES IN NEW DIR -------------------------------------
if ($handle = opendir(NEW_DIR)) {
	
	// ------------- FOR EACH FILE IN NEW DIR ------------------------------------
	while (false !== ($entry = readdir($handle))) {
		$subfolder = NEW_DIR."/".$entry;
		
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
										
										//  Check if there is an XML file in the same directory
										//  that might over-ride the defaults and set actual SENDER and RECIPIENTS.
										$xmlfiles = glob($subfolder2."/*.xml");
										if (count($xmlfiles) > 0)  {  // there is at least one XML file in the directory
											for ($i = 0 ; $i < count($xmlfiles) ; $i++ ) {   // ------ FOR EACH XML FILE  ---------
												$xml =  simplexml_load_file($xmlfiles[$i]);
												
												// check if the XML file sets a SENDER for all emails
												if ($xml -> sender ) {
													$sender = $xml -> sender;
													echo "            SENDER found in XML file: $sender \n";
												}
												
												// check if the XML file sets a RECIPIENT for all emails
												if ($xml -> recipient ) {
													$recipient = $xml -> recipient;
													echo "            RECIPIENT found in XML file: $recipient \n";
												} // ------ END OFFOR EACH XML FILE  ---------
											}
										}

										// --------- FOLDER APPEARS COMPLETE, MOVE TO RECEIVED -------------
										// completely uploaded folders are MOVED to the ~/test.soundfit.me/Scans/au1/received directory.
										// if Received dir already exists it will be intentionally overwritten!
										rename($subfolder2, RECEIVED_DIR."/".$entry2);  
					 
										echo "\n" . $subfolder2 . "\nMOVED TO\n" . RECEIVED_DIR."/".$entry2. "\n\n";
										// copy an index.php file to the new directory from the template
										copy (INDEX_FILE_TEMPLATE,RECEIVED_DIR."/".$entry2."/index.php");
										// --------- FOLDER APPEARS COMPLETE, MOVE TO RECEIVED -------------

									
										// ---------- Run ReCapLaunchModeling on the new folder --------
										$shellcmd =  "/usr/local/php53/bin/php -f" . LAUNCH_PGM . " scanID=" 
													.  $entry2 . " senderEmail=$sender  recipientEmail=$recipient ";

										$answer = `$shellcmd`;  // $answer has the stdoutput from the invoked program
										 
										
										echo "\nReCap Modeling launched:\n";
										echo $shellcmd. "\n\n";
										echo "\n\n$answer\n\n"; 
										// ---------- Run ReCapLaunchModeling on the new folder --------
					
					
										// 6) An email is composed containing a URL pointing to the new folder in:
										//    ~/earchives.soundfit.me/Scans/au1/received/orderID/

										$neworderfile =  basename($orderfile[0]);
					
										// echo "Order File name: $neworderfile \n\n"; 
										$subject = "Automated Notification: New order: $entry received.";
										$message = "New order: " . RECEIVED_URL ."/$entry/$neworderfile received.  Images in $ReceivedURL$entry. \n\n"
											. "$answer . \n\n";
										$headers = "From: " . $from;
	  
										// 7) Email is sent to PCL-orders@soundfit.me

										mail($to,$subject,$message,$headers);
										// echo "from: "   .$from.    "\n";
										// echo "to: "     .$to.      "\n";
										// echo "subject: ".$subject. "\n";
										// echo "message: ".$message. "\n";
										// echo "headers: ".$headers. "\n\n";
									}  // -------- END OF ORDER FILE IS OLD ENOUGH
									// --------- END OF WHEN ORDER FILE IS RECENT, TRY LATER -----------
									
									
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
exit;

?>