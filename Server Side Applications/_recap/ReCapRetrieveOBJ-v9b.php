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

echo "<pre>";
echo "\n------- ReCapRetrieveOBJ-v9b.php -----------------------------------\n\n";
    
/*
 ReCapRetrieveOBJ-v9b.php
 
 Copyright 2013 (c) SoundFit, LLC. All rights reserved 
 
 Developed by Scott McGregor -- SoundFit
 August 2013 - September 2013
 
 Portions of this work are derived from examples provided by 
 by Cyrille Fauvel - Autodesk Developer Network (ADN)
 August 2013
 
 Revision 1.0
 
 PROGRAM DESCRIPTION:
 
 This program is launched by ReCap when ReCap is done processing a Scan into a 
 3D Model.    When it is launched, it is passed the ScanID of the original scan
 folder.   It uses this ScanID for to determine what was the photosceneID of the model.
 This is done by looking at the contents of a file with that ScanID filename in 
 	http://test.soundfit.me/Scans/b3/maps
 Next, this program will create a folder with the name of the ScanID in the
  	http://test.soundfit.me/Scans/b3/modeled
 directory, and it then downloads the OBJ files created by ReCap into that folder.
 Next, it runs the OBJ to STL conversion program at:
 /home/earchivevps/test.soundfit.me/apps/meshconv
 and creates the corresponding STL file, which it also stores in that directory.
 
 Finally, it sends an email notification that the OBJ and STL files are ready for 
 additional CAD processing or 3D printing to the intended modeler manufacturer. 	
 */
 
 
// ----------------  DEFINITIONS AND GLOBAL VARIABLES ----------------------------
 
require 'vendor/autoload.php' ;
use Guzzle\Http\Client ;
use Guzzle\Plugin\Oauth\OauthPlugin ;
define ('ConsumerKey', '076e503f-19d8-48f7-8a5b-8a2964fcf46f') ;
define ('ConsumerSecret', '509738df-9e15-453b-abc4-27de10059692') ;
define ('BaseUrl' ,'https://accounts.autodesk.com/') ;
define ('ReCapClientID', 'KkQgk1o4Vjwsg1aBShj12mix90g') ;
define ('ReCapKey', 'Y wWKk95WhQVvKdww4whI+vib3FmA') ;
define ('ReCapApiUrl', 'http://rc-api-adn.autodesk.com/3.1/API/') ;
include_once "vendor/oauth/OAuthStore.php" ;
include_once "vendor/oauth/OAuthRequester.php" ;
include_once "clientcalls.inc.php";

define ('EMAILMAPSDIR', '/home/earchivevps/test.soundfit.me/Scans/x3/emailmaps/');
define ('MAPSDIR', '/home/earchivevps/test.soundfit.me/Scans/x3/maps/');
define ('OLDDIR', '/home/earchivevps/test.soundfit.me/Scans/x3/old/');
define ('LOGSDIR', '/home/earchivevps/test.soundfit.me/Scans/x3/logs/');
define ('RECEIVEDDIR', '/home/earchivevps/test.soundfit.me/Scans/x3/received/');
	
$received_dir_root =  '/home/earchivevps/test.soundfit.me/Scans/b3/received/';
$ReceivedURL = "http://test.soundfit.me/Scans/b3/received/";

$modeled_dir_root =  '/home/earchivevps/test.soundfit.me/Scans/b3/modeled/';
$ModeledURL =  'http://test.soundfit.me/Scans/b3/modeled/';
$errorflag = false;

$mapdir = '/home/earchivevps/test.soundfit.me/Scans/b3/maps/';

$output_string = "";
// ----------------  END OF DEFINITIONS AND GLOBAL VARIABLES ----------------------

 
// ----------------  GET SCANID AND PHOTOSCENE ID ----------------------------
// This program is called with a URL that defines the scanID.
// if this program is run from the command line, the scanID is passed as a CLI argument.
if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}
$scanID = htmlspecialchars($_GET["scanID"]);
// echo "Retrieving scanID = $scanID\n";

// Once we know the scanID, we can check the maps directory that has mappings between 
// scanIDs and photoSceneIDs.   We need the photoSceneID to retrieve the proper OBJ file.

$mapfile = $mapdir . $scanID;
// echo "mapfile = $mapfile\n";
$photoSceneID = trim(file_get_contents ($mapfile ));
// echo "photoSceneID = $photoSceneID\n";
$orderfile = glob($received_dir_root.$scanID.'/ORDER*.pdf');
$neworderfile =  basename($orderfile[0]);
// ----------------  END OF GET SCANID AND PHOTOSCENE ID ---------------------

// ----------------  GET SENDER AND RECIPIENT EMAIL ADDRS ----------------------------
// use these addresses for where to send email notifications to and from

$emailmapdir = '/home/earchivevps/test.soundfit.me/Scans/b3/emailmaps/';
$emailmapfile = $emailmapdir . $scanID . ".xml";

if (file_exists($emailmapfile)) {
    $xml = simplexml_load_file($emailmapfile);
    print_r($xml);
    
    $startTime =  $xml->startTime;
    $startTimeText =  $xml->startTimeText;
    $clientVersion = $xml->clientVersion;
    $serverVersion = $xml->serverVersion;
    // $scanID =  $xml->to;
    // $photosceneID = $xml->to;
 	$to = 	$xml-> SugarCubeScan-> order_details->to;
 	$from = $xml-> SugarCubeScan->order_details->from;
 	$subject = $xml-> SugarCubeScan->order_details->subject;
 	$body = $xml-> SugarCubeScan->order_details->body;
 	$referenceID = $xml-> SugarCubeScan->order_details->referenceID;
 	$attachments = $xml-> SugarCubeScan->order_details->attachments;
 	$meshlevel = $xml-> SugarCubeScan->modelling_options->meshlevel;
 	$template3DP = $xml-> SugarCubeScan->modelling_options->template3DP;
 	$shootingString = $xml-> SugarCubeScan->modelling_options->shootingString;
 	$doImageCropping = $xml-> SugarCubeScan->modelling_options->doImageCropping;
 	$imageCroppingRectangle = $xml-> SugarCubeScan->imodelling_options->mageCroppingRectangle;
 	$doModeScaling = $xml-> SugarCubeScan->modelling_options->doModeScaling;
 	$doModelCropping = $xml-> SugarCubeScan->modelling_options->doModelCropping;
 	$boundingbox = $xml-> SugarCubeScan->modelling_options->boundingbox;
 	$callbackProgram = $xml-> SugarCubeScan->modelling_options->callbackProgram;
 	
	echo "to:   $to\n";
	$output_string = $output_string . "Model sent to:   $to\n";
	echo "from: $from\n";
	$output_string = $output_string . "Scans sent from: $from\n";
	
	// SAMPLE XML FILE:
	/*
			"<startTime>$startTime</startTime>\n" .
		 	"<startTimeText>$today</startTimeText>\n" .
		 	"<clientVersion>$clientVersion</clientVersion>\n" .
		 	"<serverVersion>$serverVersion<serverVersion>\n" .
		 	"<scanID>$scanID</scanID>\n" .
		 	"<photoScene>$photoSceneID</photoScene>\n" .
 			"<to>$to</to>\n" .
 			"<from>$from</from>\n" .
 			"<subject>$subject</subject>\n" .
 			"<body>$body</body>\n" .
 			"<attachments>$attachments</attachments>\n" .
 			"<meshlevel>$meshlevel</meshlevel>\n" .
 			"<template3DP>$template3DP</template3DP>\n" .
 			"<shootingString>$shootingString</<shootingString>\n" .
			"<doImageCropping>$doImageCropping</doImageCropping>\n" .
			"<imageCroppingRectangle>$imageCroppingRectangle</imageCroppingRectangle>\n" .
 			"<doModelScaling>$doModelScaling</doModelScaling>\n" .
 			"<doModelCropping>$doModelCropping</doModelCropping>\n" .
 			"<boundingBox>$boundingBox</boundingBox>\n" .
 			"<callbackProgram>$callback</callbackProgram>\n" .	
 	*/
 	echo "copy $emailmapfile to $modeled_dir_root".$scanID."/SugarCube.xml\n";
	copy ( $emailmapfile, $modeled_dir_root.$scanID."/SugarCube.xml" );
} else {
    echo "Failed to open $emailmapfile. Will use default emails addresses\n";
    $output_string = $output_string . "Failed to open $emailmapfile. Will use default emails addresses\n";
}


// $to = htmlspecialchars($_GET["to"]);
if ($to == "") $to = "pcl-orders@soundfit.me";
			// $to = "scott@soundfit.me";
			// $to = "rnd@soundfit.me";

// $from = htmlspecialchars($_GET["from"]);
if ($from == "") $from = "sugarcube-daemon@soundfit.me";
// ----------  END OF GET SENDER AND RECIPIENT EMAIL ADDRS ----------------------------


// ----------------  START-UP NOTIFICATION (DISABLED) ---------------------------
// If you want to send an email when this program starts (before it begins
// processing anything else, uncomment this code:
/*
             $to = "scott@soundfit.me";
	         $headers = "From: sugarcube-daemon@soundfit.me";
             $subject = "LAUNCHED: ScanID($scanID) / model($photoSceneID)";
             "ReCapRetrieveOBJ.php Callback function launched -- scanID = $scanID";
             $message = print_r($_GET, true);
	         mail($to,$subject,$message,$headers);
*/	         
// ----------------  END OF START-UP NOTIFICATION (DISABLED) ---------------------	         


// ------------------------ RECAP OAUTH NEGOTIATION -------------------------------
//- Prepare the PHP OAuth for consuming our Oxygen service
$options =array (
	'consumer_key' => ConsumerKey,
	'consumer_secret' => ConsumerSecret,
	'server_uri' => BaseUrl,
	'request_token_uri' => BaseUrl . 'OAuth/RequestToken',
	'authorize_uri' => BaseUrl . 'OAuth/Authorize',
	'access_token_uri' => BaseUrl . 'OAuth/AccessToken',
) ;
OAuthStore::instance ('Session', $options) ;

// we retrieve the OAuth token from a file where it is updated regularly:
$fname =realpath (dirname (__FILE__)) . '/access_token.txt' ;
$access =unserialize (file_get_contents ($fname)) ;

// Create a client and provide a base URL
$client =new Client (ReCapApiUrl) ;
//- http://guzzlephp.org/guide/plugins.html
//- The Guzzle Oauth plugin will put the Oauth signature in the HTML header automatically
$oauthClient =new OauthPlugin (array (
	'consumer_key' => ConsumerKey,
	'consumer_secret' => ConsumerSecret,
	'token' => $access ['oauth_token'], //- access_token
	'token_secret' => $access ['oauth_token_secret'], //- access_token_secret
)) ;
$client->addSubscriber ($oauthClient) ;
// -------------------- END OF RECAP OAUTH NEGOTIATION ----------------------------

echo "\nRetrieving PhotoSceneID $photoSceneID for ScanID $scanID\n\n";
$output_string = $output_string . "\nRetrieving PhotoSceneID $photoSceneID for ScanID $scanID\n\n";

// -------------------- GET THE PHOTOSCENE PROCESSING TIME --------------------------------

// Now we retrieve the properties from ReCap

$request =$client->get ("photoscene/$photoSceneID/processingtime") ;
$request->getQuery ()->clear () ; 	// Not needed as this is a new request object, but
									//as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 
	'clientID' => ReCapClientID, 
	'timestamp' => time () 
)) ;
$response =$request->send () ;
//echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;

if ($xml->Error) { // if error display message but continue
	$code =$xml->Error->code ;
	echo "My ReCap get photoscene model processingtime error code: {$code}\n" ;
	$output_string = $output_string . "My ReCap get photoscene model processingtime error code: {$code}\n" ;
	$msg  =$xml->Error->msg ;
	echo "My ReCap get photoscene model processingtime error message: {$msg}\n" ;
	$output_string = $output_string . "My ReCap get photoscene model processingtime error message: {$msg}\n" ;
} else { 
	$processingTime =  $xml->Photoscene->processingtime ;
	$processingHours = intval($processingTime / 3600);
	$processingRemainderMinutes = intval(($processingTime % 3600) / 60);
	$processingRemainderSeconds = ($processingTime % 3600) % 60;
	
	echo "photoscene $photoSceneID completed by Recap in $processingHours h: $processingRemainderMinutes m: $processingRemainderSeconds s\n"; 
	$output_string = $output_string . 
		"photoscene $photoSceneID completed by Recap in $processingHours h: $processingRemainderMinutes m: $processingRemainderSeconds s\n\n";  
}

// -------------------- END OF GET THE PHOTOSCENE PROCESSING TIME -------------------------



// -------------------- GET THE PHOTOSCENE PROPERTIES --------------------------------
// Now we retrieve the properties from ReCap

$request =$client->get ("photoscene/$photoSceneID/properties") ;
$request->getQuery ()->clear () ; 	// Not needed as this is a new request object, but
									//as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 
	'clientID' => ReCapClientID, 
	'timestamp' => time ()
)) ;
$response =$request->send () ;
//echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
//echo "photoscene $photoSceneID properties:\n" . $response->getBody () . "\n"; 
//$output_string = $output_string . "photoscene $photoSceneID properties:\n" . $response->getBody () . "\n"; 

$xml =$response->xml () ;

if ($xml->Error) { // if error print error message and code 
	$code =$xml->Error->code ;
	echo "My ReCap get photoscene model properties error code: {$code}\n" ;
	$output_string = $output_string . "My ReCap get photoscene model properties error code: {$code}\n" ;
	$msg  =$xml->Error->msg ;
	echo "My ReCap get photoscene model properties error message: {$msg}\n" ;
	$output_string = $output_string . "My ReCap get photoscene model properties error message: {$msg}\n" ;
} 

// -------------------- END OF GET THE PHOTOSCENE PROPERTIES -------------------------

// -------------------- GET THE PHOTOSCENE 3DP URL --------------------------------
// Now we retrieve the 3D model from ReCap

$request =$client->get ("photoscene/$photoSceneID") ;
$request->getQuery ()->clear () ; 	// Not needed as this is a new request object, but
									//as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 
	'clientID' => ReCapClientID, 
	'timestamp' => time (), 
	'format' => '3dp'
)) ;
$response =$request->send () ;
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;

if ($xml->Error) { // if error print and quit gracefully
	$code =$xml->Error->code ;
	echo "My ReCap get 3DP file get photoscene model error code: {$code}\n" ;
	$output_string = $output_string . "My ReCap get 3DP fileget photoscene model error code: {$code}\n" ;
	$msg  =$xml->Error->msg ;
	echo "My ReCap get get 3DP file photoscene model error message: {$msg}\n" ;
	$errorflag = true;
} else if ($xml->Photoscene->progressmsg == "ERROR") { 

	// Sometimes ReCap returns an "On Success" object instead of "On Error" object,
	// but reports the error in the progressmsg and then doesn't create any files.
	// if that happens we fall through here.
	
	$msg = "Response Body\n-------\n" . $response->getBody () . "======\n\n" ;
	echo "ERROR: My ReCap get 3DP file get photoscene model error message:\n {$msg}\n";
	$output_string = $output_string . "My ReCap get 3DP file get photoscene model error message:\n {$msg}\n" ;
	$subject = "ERROR! Get 3DP $photoSceneID FAILED";
  	$output_string = $output_string . "$subject\n"
  				. "New order: $ReceivedURL$scanID/$neworderfile - modeling failed.\n"
  				. "Scan images in $ReceivedURL$scanID. \n\n"
  				. $msg;
  	$errorflag = true;
  	
} else { // If we get here there SHOULD be a 3DP model ready for download

	$errorflag = false;
	
	// -------------------- CREATE THE SCANID FOLDER IN MODELED DIRECTORY --------------
	$scan_directory = $modeled_dir_root.$scanID;
	if (!file_exists($scan_directory)) {$retval = mkdir($scan_directory,0775);}
	
	symlink(EMAILMAPSDIR . $scanID, 	$scan_directory. "/emailmaps");
	symlink(MAPSDIR . $scanID, 			$scan_directory. "/maps");
	symlink(OLDDIR . $scanID, 			$scan_directory. "/old");
	symlink(LOGSDIR . $scanID, 			$scan_directory. "/logs");
	symlink(RECEIVEDDIR . $scanID, 		$scan_directory. "/received");

	// -------------------- END OF CREATE THE SCANID FOLDER IN MODELED DIRECTORY --------

	// echo "photoscene $photoSceneID completed by Recap.\n"; 
	// $output_string = $output_string .  "photoscene $photoSceneID completed by Recap.\n"; 
	$the3dpURL = $xml->Photoscene->scenelink ;
	echo "3DP model for ScanID: $scanID \nis at: $the3dpURL\n\n";
	$output_string = $output_string . "3DP model for ScanID: $scanID \nis at: $the3dpURL\n\n";
	// -------------------- END OF GET THE PHOTOSCENE 3DP URL -------------------------

	// -------------------- DOWNLOAD 3DP FILE ---------------------------------------
	// Now we will copy the 3DP file from ReCap into the "modeled" Scan directory
	$the_3dpfile = $scan_directory . '/' . $scanID . ".3dp";
	set_time_limit(0); 
	$file = file_get_contents($the3dpURL);  // get ReCap data file into string
	file_put_contents($the_3dpfile, $file); // create our own copy from string
	echo "The 3DP file = $the_3dpfile\n";
	$output_string = $output_string . "The 3DP file = $the_3dpfile\n";
	
	$filesize = filesize ($the_3dpfile);
	if ($filesize == 0) {
		echo "ERROR! NO 3DP MODEL: 3DP file is ZERO length\n" ;
		$output_string = $output_string . "NO MODEL: 3DP file is ZERO length\n" ;
		$subject = "ERROR! NO 3DP MODEL: 3DP file is ZERO length\n";
  		$output_string = $output_string . "$subject\n"
  				. "New order: $ReceivedURL$scanID/$neworderfile - modeling failed.\n"
  				. "Scan images in $ReceivedURL$scanID. \n\n";
  				
	}

	// -------------------- END OF DOWNLOAD 3DP FILE --------------------------------

} 

if ($errorflag == false) {  // If we get here there SHOULD be an OBJ to download

	// -------------------- GET THE PHOTOSCENE OBJ URL --------------------------------
	// Now we retrieve the 3D model from ReCap


	$request =$client->get ("photoscene/$photoSceneID") ;
	$request->getQuery ()->clear () ; 	// Not needed as this is a new request object, but
										//as I am going to use merge(), it is safer
	$request->getQuery ()->merge (array ( 
		'clientID' => ReCapClientID, 
		'timestamp' => time (), 
		'format' => 'obj'
	)) ;
	$response =$request->send () ;
	// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
	$xml =$response->xml () ;

	if ($xml->Error) { // if error stop here else continue
		$code =$xml->Error->code ;
		echo "My ReCap get photoscene model error code: {$code}\n" ;
		$output_string = $output_string . "My ReCap get photoscene model error code: {$code}\n" ;
		$msg  =$xml->Error->msg ;
		echo "My ReCap get photoscene model error message: {$msg}\n" ;
		$output_string = $output_string . "My ReCap get photoscene model error message: {$msg}\n" ;
		$subject = "ERROR! $photoSceneID FAILED - $code: $msg";
		$output_string =  $output_string . "$subject\n"
					. "New order: $ReceivedURL$scanID/$neworderfile - modeling failed.\n"
					. "Scan images in $ReceivedURL$scanID. \n\n";
	
	} else if ($xml->Photoscene->progressmsg == "ERROR") { 

		// Sometimes ReCap returns an "On Success" object instead of "On Error" object,
		// but reports the error in the progressmsg and then doesn't create any files.
		// if that happens we fall through here.
	
		$msg = "Response Body\n-------\n" . $response->getBody () . "======\n\n" ;
		echo "ERROR: My ReCap get OBJ file get photoscene model error message:\n {$msg}\n";
		$output_string = $output_string . "My ReCap get OBJ file get photoscene model error message:\n {$msg}\n" ;
		$subject = "ERROR! Get OBJ $photoSceneID FAILED";
  		$output_string = $output_string .  "$subject\n"
  				. "New order: $ReceivedURL$scanID/$neworderfile - modeling failed.\n"
  				. "Scan images in $ReceivedURL$scanID. \n\n"
  				. $msg;
  		$errorflag = true;
  	
	} else { // If we get here there SHOULD be an OBJ  ready for download
	
		// echo "photoscene $photoSceneID completed by Recap.\n"; 
		// $output_string = $output_string .  "photoscene $photoSceneID completed by Recap.\n"; 
		$objURL = $xml->Photoscene->scenelink ;
		echo "OBJ model for ScanID: $scanID \nis at: $objURL\n\n";
		$output_string = $output_string . "OBJ model for ScanID: $scanID \nis at: $objURL\n\n";
		// -------------------- END OF GET THE PHOTOSCENE OBJ URL -------------------------

		// -------------------- CREATE THE SCANID FOLDER IN MODELED DIRECTORY --------------
		$scan_directory = $modeled_dir_root.$scanID;
		if (!file_exists($scan_directory)) {$retval = mkdir($scan_directory,0775);}
		// -------------------- END OF CREATE THE SCANID FOLDER IN MODELED DIRECTORY --------

		// -------------------- DOWNLOAD OBJ ZIP FILE ---------------------------------------
		// Now we will copy the OBJ file from ReCap into the "modeled" Scan directory
		$scan_zipfile = $scan_directory . '/' . $scanID . ".obj.zip";
		$OBJ_zipfileURL = ModeledURL . $scanID . ".obj.zip"; 
		set_time_limit(0); 
		$file = file_get_contents($objURL);       // get the OBJ file from ReCap
		file_put_contents($scan_zipfile, $file);  // save a copy of the OBJ locally.

		//unzip the OBJ file:
		// get the absolute path to $file
		$path = pathinfo(realpath($scan_zipfile), PATHINFO_DIRNAME);
		// echo "path = $path\n";
		$zip = new ZipArchive;
		$res = $zip->open($scan_zipfile);
		// -------------------- END OF DOWNLOAD OBJ ZIP FILE --------------------------------


		// -------------------- UNZIP OBJ AND CONVERT TO STL  -------------------------------
		if ($res === TRUE) {  // If download was successful, Unzip and convert 
			// -------------------- UNZIP -----------------------------
			// extract it to the path we determined above
			$zip->extractTo($path);
			$zip->close();
			// -------------------- END OF UNZIP -----------------------
  
			// -------------------- CONVERT OBJ TO STL FORMAT -----------------------------
			// use meshconv to create the STL file from the OBJ file
			$infilename =  $scan_directory . '/' .  "mesh.obj";
			// $tempfilename = "./tmp/SCS" . date("U");
			$tempfilename = $scan_directory . '/' .  "mesh";
			$shellcmd =  "/home/earchivevps/test.soundfit.me/apps/meshconv "
						 . $infilename . " -c STL -o " . $tempfilename;
			$answer = `$shellcmd`;
			// echo "convert OBJ  to  STL stdout: $answer\n";
			// -------------------- CONVERT OBJ TO STL FORMAT -----------------------------
  
			// -------------------- CONVERT OBJ TO JS FORMAT -----------------------------
			// use meshconv to create the STL file from the OBJ file
			$infilename =  $scan_directory . '/' .  "mesh.obj";
			$outfilename = $scan_directory . '/' .  "mesh.js";
			$shellcmd =  "python /home/earchivevps/test.soundfit.me/apps/convert_obj_three.py -i "
						 . $infilename . "  -o " . $outfilename;
			$answer = `$shellcmd`;
			// echo "convert OBJ to JS stdout: $answer\n";
		
			// copy the View3DModel.html file to the scan directory
		
			$shellcmd =  "cp /home/earchivevps/test.soundfit.me/templates/View3DModel.php  "
						 . $scan_directory;
			$answer = `$shellcmd`;
		
			// -------------------- CONVERT OBJ TO STL FORMAT -----------------------------
 
            // -------------------- GET THE ATTACHMENTS ----------------------------------
            
            // $attachments = $xml -> attachments; 
			// print_r($attachments);
	
			$badAttachmentsFileErrorFlag = false;
			$numberOfAttachmentsFilesWithErrors = 0;
	
			$filenames = $xml -> attachments -> filename; 
			$numberOfAttachmentFiles =  count($filenames);
			echo "Number of Attachment files = $numberOfAttachmentFiles\n";
			$outputString = $outputString . "Number of Attachment files = $numberOfAttachmentFiles\n";
	
			$attachmentFiles =array () ;
	
			for ($i = 0;  $i < $numberOfAttachmentFiles; $i++ ) {
				$currentFilename = $scan_directory."/".$filenames[$i];
				$attachmentFiles["file[$i]"] = $scan_directory."/".$filenames[$i];
			}

			$attachmentString = implode( "\n\t", $attachmentFiles ) ;
			
            // -------------------- END OF GET THE ATTACHMENTS ---------------------------
	
			// track when  callback process started so we can determine total clock time from  receipt of model
			// to end of model.
			$endTime = time(); 
			$endTimeText = date("D M Y j G:i:s T");
			$modelingDuration = $endTime - $startTime;
			$modelingHours = intval($modelingDuration / 3600);
			$modelingRemainderMinutes = intval(($modelingDuration % 3600) / 60);
			$modelingRemainderSeconds = ($modelingDuration % 3600) % 60;
			$modelingTimeString = "$modelingHours h: $modelingRemainderMinutes m: $modelingRemainderSeconds s";
			echo "  endTime   \t= $endTime   \t endTimeText   \t= $endTimeText\n";
			echo "- startTime \t= $startTime \t startTimeText \t= $startTimeText\n";
			echo "-----------------------------------------------------------------------------\n";
			echo "\t\t  $modelingDuration seconds \t\t\t\t\t  $modelingTimeString \n"; 
			
			$modelingDurationText =  date('H \h: i \m: s \s', $modelingDuration);
  
			// -------------------- PREPARE SUCCESS NOTIFICATION TEXT ----------------------
			echo "SUCCESS: $scanID 3DP, OBJ and STL models now in $ModeledURL$scanID/\n\n";
			$output_string = $output_string . "SUCCESS: $scanID 3DP, OBJ and STL models now in $ModeledURL$scanID/\n\n";
			$subject = "SUCCESS: ScanID($scanID) / model($photoSceneID) is ready for download";
			$message =  "$subject\n"
				. "FROM: $from\n"
				. "TO: $to\n"
				. "SUBJECT: $subject\n"
				. "REFERENCE ID : $referenceID\n"
				. "BODY: $body\n"
				. "ATTACHMENTS:\n\t$attachmentString\n\n"
				// . "New order: $ReceivedURL$scanID/$neworderfile received.\n"
				// . "OBJ Directory: $ModeledURL$scanID/\n"
				. "OBJ Zip file: $$OBJ_zipfileURL\n"
				. "STL: $ModeledURL$scanID/mesh.stl\n"
				. "3DP: $ModeledURL$scanID/$scanID.3dp\n"
				. "JS: $ModeledURL$scanID/mesh.js\n\n"
				. "View 3D model at: $ModeledURL$scanID/View3DModel.php\n"
				. "View Scan images in $ReceivedURL$scanID. \n\n"
				. "Scan  Received: \t $startTimeText\n" 
				. "Model Completed:\t $endTimeText\n"
				. "Model Duration: \t $modelingTimeString\n"
				. "CPU Time: \t $processingHours h: $processingRemainderMinutes m: $processingRemainderSeconds s\n\n"
				. $output_string;
			// -------------------- END OF PREPARE SUCCESS NOTIFICATION TEXT --------------- 
		}
	}
} 
else {  // ($errorflag == true)
  		// -------------------- PREPARE FAILURE NOTIFICATION TEXT ----------------------
  		echo "ERROR! Couldn't retrieve $photoSceneID, file\n";
  		$subject = "ERROR! Couldn't retrieve $photoSceneID";
  		$message =  "$subject\n"
  			. "3DP and OBJ files not retrieved.\n"
  			. "New order: $ReceivedURL$scanID/$neworderfile - modeling failed.\n"
  			. "Scan images in $ReceivedURL$scanID. \n\n"
  			. $output_string;
  		
  		// -------------------- END OF PREPARE FAILURE NOTIFICATION TEXT ---------------

	// -------------------- END OF UNZIP OBJ AND CONVERT TO STL  ----------------------
}




// -------------------- DELETE THE PHOTOSCENE (DISABLED)  -------------------------
// This is commented out for now as we might not want to delete the photoscene yet, 
// since we may want to access it in a separate program.  Ultimately we should
// enable this when we are in production to keep our Autodesk directory small.
// 
// $request =$client->delete ("photoscene/{$photoSceneID}") ;
// $request->addPostFields (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
// $response =$request->send () ;
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
// $xml =$response->xml () ;
// if ( isset ($xml->Photoscene->deleted) && $xml->Photoscene->deleted == 0 )
//  	echo "My ReCap PhotoScene is now deleted\n" ;
// else
//  	echo "Failed deleting the PhotoScene and resources!\n" ;
//
// -------------------- END OF DELETE THE PHOTOSCENE (DISABLED)  ------------------
 
 
// -------------------- SEND THE EMAIL NOTIFICATIONS  -----------------------------
// Notify the user of the result of the scan
// echo "Sending Mail to:$to <br />";
// echo "From: $from <br />";
$headers = "From: " . $from;
// echo "Headers = $headers <br />";
// echo "Subject:  $subject <br />";
// echo "Message = $message <br />";
mail($to,$subject,$message,$headers);
// echo "#  Mail Sent.";
// -------------------- END OF SEND THE EMAIL NOTIFICATIONS  ----------------------



echo "\n------- End of ReCapRetrieveOBJ-v9b.php -----------------------------------\n\n";
echo "</pre>";
exit ;
?>

