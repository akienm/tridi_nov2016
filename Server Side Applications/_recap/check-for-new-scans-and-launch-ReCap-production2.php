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


echo "\n------- check-for-new-scans-and-launch-ReCap-production2.php -----------------------------------\n\n";
	     
// check-for-new-scans.php  -- Written by Scott L. McGregor, SoundFit LLC, 06/17/2013
// revised 09/21/2013
//
// This program is launched as a cronjob periodically (every 10 min) 
// to check for new scanfiles that may have been uploaded since it last run.
//
// Operations:
//
// 1) Program gets a list of folders in the ~/earchives.soundfit.me/Scans/b1/new directory.
// 2) Each folder is openned and ORDERS*.pdf file is searched for.
// 3) if ORDERS*.pdf file is not found, the folder may still be in the process of being uploaded,
//    so nothing more is done for now  -- future use file locking, and check for folders that 
//    remain incomplete for more than 24 hours.
// 4) if ORDERS*.pdf file is found and more than 2 minutes old, 
//    the folder is presumed completely uploaded.
// 5) completely uploaded folders are MOVED to the ~/test.soundfit.me/Scans/au1/received directory.
// 6) An email is composed containing a URL pointing to the new folder in:
//    ~/earchives.soundfit.me/Scans/au1/recieved/orderID/
// 7) Email is sent to PCL-orders@soundfit.me
// 8) Program closes successfully and waits until chron restarts it again.

// ----------------  DEFINITIONS AND GLOBAL VARIABLES ----------------------------
$RootDir = '/home/earchivevps/test.soundfit.me/Scans/b1/';
// echo "RootDir = ".$RootDir."<br />";

$NewDir  = $RootDir."new/";
//echo "NewDir = ".$NewDir."<br /> \n";

$ReceivedDir = $RootDir."received/";
//echo "ReceivedDir = ".$ReceivedDir."<br /> \n";

$ReceivedURL = "http://test.soundfit.me/Scans/b1/received/";
//echo "ReceivedURL = ".$ReceivedURL."<br /> \n";

$new_index_file_template = "/home/earchivevps/test.soundfit.me/templates/gallery.php";

// Use these email addresses
$from = "sugarcube-daemon@soundfit.me";
// $to = "scotts@soundfit.me";   // use for testing
// $to = "rnd@soundfit.me";      // use for testing 
$to = "PCL-orders@soundfit.me";  // use for production 


// This version uses the ScannerID ($name) to lookup the Email notification address for that ScannerID
function getEmail($name){
	$mysqli = new mysqli("orders.earchive.soundfit.me", "soundfit_admin", "Use4SoundFit!", "sf_orders");
	$res = mysqli_query($mysqli, "SELECT email FROM user_lookup WHERE name='$name' LIMIT 1");
	$row = mysqli_fetch_assoc($res);
	return $row['email'];
}


					
// ----------------  END OF DEFINITIONS AND GLOBAL VARIABLES ----------------------


// Program gets a list of folders in the ~/earchives.soundfit.me/Scans/b1/new directory.

//  ---------------- PROCESS FILES IN NEW DIR -------------------------------------
if ($handle = opendir($NewDir)) {
	 // echo "handle = ".$handle."<br />";

	// ------------- FOR EACH FILE IN NEW DIR ------------------------------------
	while (false !== ($entry = readdir($handle))) {
		// echo "entry = ".$entry."<br />";

		// ------------- FOR EACH FILE THAT IS A FOLDER --------------------------------
		if (is_dir($NewDir.$entry)) {  // we only process directories
		
			// ------------- FOR EACH FOLDER OTHER THAN CURRENT OR PARENT --------------
			if ($entry != "." && $entry != "..") {

				// Each folder is openned and ORDERS*.pdf file is searched for.
				$orderfile = glob($NewDir.$entry.'/ORDER*.pdf');
				// echo "orderfile = <br />\n";
				// print_r ($orderfile);
				// echo "<br /> \n";
				// echo "<br /> \n";

				// --------- FOR FOLDERS WITH ORDER FILES ------------------------------
				if (count($orderfile) > 0) {

					//extract the scanner ID from the $orderfile
			    	$pattern='@^(?:ORDER_)?([^_]+)@i';
			    	preg_match($pattern, $orderfile[0], $matches);
			    	$name=$matches[1];
			    	echo '<br>';
			    	print_r($name);
			    	
			    	
					// if an order form exists we can proceed, otherwise we'll try again later.. 
					$now = date("U");
					// echo "now = $now <br />";
					$filemod = filemtime($orderfile[0]);
					// echo "filemod = $filemod <br />";
					$secssincelastchange = $now - $filemod;
					// echo "secssincelastchange = $secssincelastchange <br />";
					
					// --------- WHEN ORDER FILE IS RECENT, TRY LATER -----------------
					// if ORDERS*.pdf file is found and more than 2 minutes old,
					//  the folder is presumed completely uploaded.
					if ($secssincelastchange < 120)  {
					   // echo "lastchange = ".$lastchange."<br />";
						exit(1);  // QUIT FOR NOW, TRY AGAIN LATER
						//sleep(120 - lastchange);			
					}
					// echo "lastchange = ".$lastchange."<br />";
					// --------- END OF WHEN ORDER FILE IS RECENT, TRY LATER -----------

					// --------- FOLDER APPEARS COMPLETE, MOVE TO RECEIVED -------------
					// completely uploaded folders are MOVED to the ~/test.soundfit.me/Scans/b1/received directory.
					// if Received dir already exists it will be intentionally overwritten!
					rename($NewDir.$entry, $ReceivedDir.$entry);  
					 
					echo "\n" . $NewDir.$entry . " MOVED TO " . $ReceivedDir.$entry. "\n\n";
					// copy an index.php file to the new directory from the template
					copy ($new_index_file_template,$ReceivedDir.$entry."/index.php");
					// --------- FOLDER APPEARS COMPLETE, MOVE TO RECEIVED -------------

					// ---------- Run ReCapLaunchModeling on the new folder --------
					$shellcmd =  "/usr/local/php53/bin/php /home/earchivevps/test.soundfit.me/apps/_recap/ReCapLaunchModeling-wScale+Crop5.php scanID=" 
					  			.  $entry;
					$answer = `$shellcmd`;  // $answer has the stdoutput from the invoked program
					echo "\nReCap Modeling launched:\n";
					echo $shellcmd. "\n\n";
					// echo "\n$answer\n\n";
					// ---------- Run ReCapLaunchModeling on the new folder --------
					
					
					// 6) An email is composed containing a URL pointing to the new folder in:
					//    ~/earchives.soundfit.me/Scans/au1/received/orderID/

					$neworderfile =  basename($orderfile[0]);
					
					echo "Order File name: $neworderfile \n\n"; 

					//try to retrieve email from DB; if empty notify administrator of unregistered user!
					$email=getEmail($name);
					$to = (empty($email)? 'scott@soundfit.me' : $email);
					$subject = (empty($email)? "Order from unregistered user ".$name."received!" : "Automated Notification: New order: $entry received.");
					$message = "New order: " . RECEIVED_URL ."/$entry/$neworderfile received.  Images in $ReceivedURL$entry. \n\n"
						. "$answer . \n\n";
					$headers = "From: " . $from;
					
	  
					// 7) Email is sent to sender

					mail($to,$subject,$message,$headers);
					// echo "from: "   .$from.    "\n";
					// echo "to: "     .$to.      "\n";
					// echo "subject: ".$subject. "\n";
					// echo "message: ".$message. "\n";
					// echo "headers: ".$headers. "\n\n";
			    }
				// --------- END OF FOR FOLDERS WITH ORDER FILES -----------------------

			}
			// ------------- END OF FOR EACH FOLDER OTHER THAN CURRENT OR PARENT -------
			
		}
		// ------------- END OF FOR EACH FILE THAT IS A FOLDER -------------------------	
			
	}
	// ------------- END OF FOR EACH FILE IN NEW DIR -----------------------------
	 
}
//  ---------------- END OF PROCESS FILES IN NEW DIR ------------------------------

// Program closes successfully and waits until chron restarts it again.
exit;

?>