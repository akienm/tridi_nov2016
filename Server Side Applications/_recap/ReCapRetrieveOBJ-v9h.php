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

$reCapCallbackStartTime = time();
echo "<pre>";
echo "\n------- ReCapRetrieveOBJ-v9h.php -----------------------------------\n\n";
$output_string = "Model retrieved by ReCapRetrieveOBJ-v9h.php.\n\n";  

/*
 ReCapRetrieveOBJ-v9h.php
 
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

define ('EMAILMAPSDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/emailmaps/');
define ('MAPSDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/maps/');
define ('OLDDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/old/');
define ('LOGSDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/logs/');
define ('RECEIVEDDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/received/');
	
$received_dir_root =  '/home/earchivevps/test.soundfit.me/Scans/b3/received/';
$ReceivedURL = "http://test.soundfit.me/Scans/b3/received/";

$modeled_dir_root =  '/home/earchivevps/test.soundfit.me/Scans/b3/modeled/';
$ModeledURL =  'http://test.soundfit.me/Scans/b3/modeled/';
$errorflag = false;

$mapdir = '/home/earchivevps/test.soundfit.me/Scans/b3/maps/';


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
if ($from == "") $from = "sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com";
// ----------  END OF GET SENDER AND RECIPIENT EMAIL ADDRS ----------------------------


// ----------------  START-UP NOTIFICATION (DISABLED) ---------------------------
// If you want to send an email when this program starts (before it begins
// processing anything else, uncomment this code:
/*
             $to = "scott@soundfit.me";
	         $headers = "From: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com";
             $subject = "LAUNCHED: ScanID($scanID) / model($photoSceneID)";
             "ReCapRetrieveOBJ.php Callback function launched -- scanID = $scanID";
             $message = print_r($_GET, true);
	         mail($to,$subject,$message,$headers,  "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com");
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
	
	$EMAILMAPSDIR='/home/earchivevps/test.soundfit.me/Scans/b3/emailmaps/';
	$MAPSDIR='/home/earchivevps/test.soundfit.me/Scans/b3/maps/';
	$OLDDIR='/home/earchivevps/test.soundfit.me/Scans/b3/old/';
	$LOGSDIR='/home/earchivevps/test.soundfit.me/Scans/b3/logs/';
	$RECEIVEDDIR='/home/earchivevps/test.soundfit.me/Scans/b3/received/';

	symlink($EMAILMAPSDIR . $scanID . ".xml", 		$scan_directory. "/submitted.xml");
	symlink($MAPSDIR . $scanID, 					$scan_directory. "/photoscene-map");
	symlink($LOGSDIR . $scanID . ".stdio.log", 		$scan_directory. "/stdio.log");
	symlink($LOGSDIR . $scanID . ".err.log", 		$scan_directory. "/err.log");
	//symlink($OLDDIR . $scanID, 					$scan_directory. "/old");
	symlink($RECEIVEDDIR . "/auto-template.3dp",	$scan_directory. "/auto-template.3dp");
	$execCmd = "ln -s $OLDDIR" . $scanID . "$scan_directory/old"; 
	exec($execCmd);
	//symlink($RECEIVEDDIR . $scanID, 		$scan_directory. "/received");
	$execCmd = "ln -s $RECEIVEDDIR" . $scanID . "$scan_directory/received"; 
	exec($execCmd);


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
	
	$result4 = shell_exec("/bin/rm -f /tmp/$scanID\*");
	
	file_put_contents("/tmp/$scanID.3dp.gz", $file); // create our own copy from string
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

	// we need an uncompressed, UTF-8 version to create the diff table
	$result0 = shell_exec("/bin/rm -f /tmp/$scanID.3dp");
	
	$result1 = shell_exec("gunzip -f /tmp/$scanID.3dp.gz > /tmp/$scanID.3dp");
	echo "result1 = $result1\n\n";
	$result2 = shell_exec("iconv -f UTF-16 /tmp/$scanID.3dp"); 
	$result3 = shell_exec("/bin/rm -f /tmp/$scanID\*");
	$theUTF8_3dpString = strstr($result2, "<RZML");
	

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
		$OBJ_zipfileURL = $ModeledURL . $scanID . "/$scanID.obj.zip"; 
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
		
			$shellcmd =  "cp /home/earchivevps/test.soundfit.me/apps/_recap/View3DModel.php  "
						 . $scan_directory;
			$answer = `$shellcmd`;
			echo "$shellcmd\n   -- returns: $answer\n";
		
			// -------------------- CONVERT OBJ TO STL FORMAT -----------------------------
 
            // -------------------- GET THE ATTACHMENTS ----------------------------------
            
            // $attachments = $xml -> attachments; 
			// print_r($attachments);
	
			$badAttachmentsFileErrorFlag = false;
			$numberOfAttachmentsFilesWithErrors = 0;
	
			$filenames = $xml -> attachments -> filename; 
			$numberOfAttachmentFiles =  count($filenames);
			echo "Number of Attachment files = $numberOfAttachmentFiles\n";
			$output_string = $output_string . "Number of Attachment files = $numberOfAttachmentFiles\n";
	
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
			$subject = "SUCCESS: ScanID($scanID) / PhotoSceneID($photoSceneID) is ready for download";
			$message =  "$subject\n"
				. "FROM: $from\n"
				. "SENDER: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com \n"
				. "REPLY-TO: $from\n"
				. "TO: $to\n"
				. "SUBJECT: $subject\n"
				. "REFERENCE ID : $referenceID\n"
				. "ScanID: $scanID        PhotoSceneID: $photoSceneID\n"
				. "ATTACHMENTS:\n\t$attachmentString\n"
				. "BODY: $body\n\n"

				. "View 3D model at: $ModeledURL$scanID/View3DModel.php\n"
				. "View Scan images in: $ReceivedURL$scanID. \n"
				. "View Difference table at: $ModeledURL$scanID/DiffTable.html\n"
				. "View Manifest file at: $ReceivedURL$scanID/Manifest.xml\n\n"
				
				// . "New order: $ReceivedURL$scanID/$neworderfile received.\n"
				// . "OBJ Directory: $ModeledURL$scanID/\n"
				
				
				. "OBJ Zip file: $OBJ_zipfileURL\n"
				. "STL: $ModeledURL$scanID/mesh.stl\n"
				. "3DP: $ModeledURL$scanID/$scanID.3dp\n"
				. "JS: $ModeledURL$scanID/mesh.js\n\n"
			
				. "Scan  Received: \t $startTimeText\n" 
				. "Model Completed:\t $endTimeText\n"
				. "Model Duration: \t $modelingTimeString\n"
				. "CPU Time: \t $processingHours h: $processingRemainderMinutes m: $processingRemainderSeconds s\n\n"
				. $output_string;
			// -------------------- END OF PREPARE SUCCESS NOTIFICATION TEXT --------------- 
		}
	}


	// write the maximum and minimum X, Y, Z, and the one dimensional distance between them.
	
	// open the OBJ file
	$modeledDirPath  = "/home/earchivevps/test.soundfit.me/Scans/b3/modeled";
	$objfile = "$modeledDirPath/$scanID/mesh.obj";
	//echo "objfile = $objfile\n";
	$recordArray = file($objfile);
	//echo "recordArray =;"; print_r($recordArray);
	
	$maxX  =  $maxXx = $maxXy = $maxXz = -INF; 
	$minX  =  $minXx = $minXy = $minXz = INF; 
	
	$maxY  =  $maxYx = $maxYy = $maxYz = -INF; 
	$minY  =  $minYx = $minYy = $minYz = INF; 
				
	$maxZ  =  $maxZx = $maxZy = $maxZz = -INF; 
	$minZ  =  $minZx = $minZy = $minZz = INF; 
	
	// echo "print records\n";
	foreach ($recordArray as $objRecord) {
		//echo "$objRecord\n";
		$tokens =  explode(" ",$objRecord);
			
		if ( $tokens[0] == "v" ) {
		
			$x = $tokens[1];
			$y = $tokens[2];
			$z = trim($tokens[3]);
			
			if ($x < $minX) { 
				$minX  = $x;
				$minXx = $x; 
				$minXy = $y; 
				$minXz = $z;
			} else if ($x > $maxX) { 
				$maxX  = $x;
				$maxXx = $x; 
				$maxXy = $y; 
				$maxXz = $z;
			}
			
			if ($y < $minY) { 
				$minY  = $y;
				$minYx = $x; 
				$minYy = $y; 
				$minYz = $z;
			} else if ($y > $maxY) { 
				$maxY  = $y;
				$maxYx = $x; 
				$maxYy = $y; 
				$maxYz = $z;
			}
			
			if ($z < $minZ) { 
				$minZ  = $z;
				$minZx = $x; 
				$minZy = $y; 
				$minZz = $z;
			} else if ($z > $maxZ) { 
				$maxZ = $z;
				$maxZx = $x; 
				$maxZy = $y; 
				$maxZz = $z;
			}	
			$Xdifference = $maxX - $minX;
			$Ydifference = $maxY - $minY;
			$Zdifference = $maxZ - $minZ;
			
			//echo "\nmaxX=$maxX, minX=$minX -> $Xdifference\n maxY=$maxY, minY=$minY -> $Ydifference\n maxZ=$maxZ, minZ=$minZ -> $Zdifference;\n";
			
		} // if line is not a "v" vertex record do nothing.
	
	}  //  end foreach ($recordArray as $objRecord)
	
	echo "\n maxX=$maxX, minX=$minX -> $Xdifference\n maxY=$maxY, minY=$minY -> $Ydifference\n maxZ=$maxZ, minZ=$minZ -> $Zdifference;\n\n";
	
	// output the max, min and difference for the X, Y and Z cloud points
	
	// ----------------- Write a Difference Table to the directory ---------------------------

	echo "\n\nDIFFERENCE TABLE PROCESSING\n\n"; 

	$htmlHeader = '<!DOCTYPE html>'
		. 	'<html lang="en-US">'
		.	'<head>'
		.	'<title>SugarCube Model vs. Template Difference Tables</title>'
		. 	'<meta charset="utf-8">'
		.	'<meta name="viewport" content="width=device-width">'
		.	'<style>'
	
		.	'table{border-collapse:collapse;border:1px solid #000;font-size:10px;}'
		.	'table td{border:1px solid #000;font-size:10px;text-align: right;margin-right: 1em;}'
		.	'table th{border:1px solid #000;font-size:10px;text-align: center;margin-right: 1em;}'
		.	'p{font-size:10px;}' 

		.   '</style>'
		.	'</head>'
		.	'<body>';

	$DiffPageString = $htmlHeader;
	
	if (file_exists(RECEIVEDDIR . "$scanID/user-template.3dp")) {
		$templateXMLFile =  RECEIVEDDIR . "$scanID/user-template.3dp";
		$usertemplateString ="User provided template = <a href='".$ReceivedURL.$scanID."/user-template.3dp'>$ReceivedURL$scanID/user-template.3dp</a><br />";
	} else {
		$templateXMLFile =  RECEIVEDDIR . "$scanID/auto-template.3dp";
		$usertemplateString = "";
	}
	echo "templateXMLFile = $templateXMLFile \n";
	
	
	$htmlTrailer = '</body>';	
	
	$DiffPageString = $DiffPageString 
				. "<div><h2>SugarCube Scan</h2>"  
				. "FROM: $from\n"
				. "SENDER: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com \n"
				. "REPLY-TO: $from\n"
				. "TO: $to<br />"
				. "SUBJECT: $subject<br />"
				. "REFERENCE ID : $referenceID<br />"
				. "ScanID: $scanID     PhotoSceneID: $photoSceneID<br />"
				. "ATTACHMENTS:\n\t$attachmentString<br />"
				. "BODY: $body<br /><br />"
				
				. "View 3D model at: <a href='".$ModeledURL.$scanID."/View3DModel.php'>$ModeledURL$scanID/View3DModel.php</a><br />"
				. "View Scan images in: <a href='".$ReceivedURL.$scanID."'>$ReceivedURL$scanID</a><br />"
				. "Automated template = <a href='".$ReceivedURL.$scanID."/auto-template.3dp'>$ReceivedURL$scanID/auto-template.3dp</a><br />"
				. $usertemplateString 
				. "View Manifest file at: <a href='".$ReceivedURL.$scanID."/Manifest.xml'>$ReceivedURL$scanID/Manifest.xml</a><br /><br />"
				
				. "OBJ Directory: <a href='".$ModeledURL.$scanID."'>$ModeledURL$scanID</a><br />"
				. "OBJ Zip file: <a href='".$OBJ_zipfileURL."'>$OBJ_zipfileURL</a><br />"
				. "STL: <a href='".$ModeledURL.$scanID."/mesh.stl'>$ModeledURL$scanID/mesh.stl</a><br />"
				. "3DP: <a href='".$ModeledURL.$scanID."/$scanID.3dp'>$ModeledURL$scanID/$scanID.3dp</a><br />"
				. "JS: <a href='".$ModeledURL.$scanID."/mesh.js.'>$ModeledURL$scanID/mesh.js</a><br /><br />" 
				
				. "Scan  Received: \t $startTimeText<br />" 
				. "Model Completed:\t $endTimeText<br />"
				. "Model Duration: \t $modelingTimeString<br />"
				. "CPU Time: \t $processingHours h: $processingRemainderMinutes m: $processingRemainderSeconds s<br />"
				. "</div>";
				
	
	$DiffPageString = $DiffPageString .  "<div><h2>Target Object Size</h2>";
	
	$DiffPageString = $DiffPageString . "<table><tr><th>Variable</th><th>Max</th><th>Min</th><th>Diameter</th></tr>"
	 	. "<tr><td>X</td><td>$maxX</td><td>$minX</td><td>". $Xdifference . "</td></tr>"
		. "<tr><td>Y</td><td>$maxX</td><td>$minY</td><td>". $Ydifference . "</td></tr>"
		. "<tr><td>Z</td><td>$maxX</td><td>$minZ</td><td>". $Zdifference . "</td></tr>"
		. "</table>"
	 	. "</div>";



	$templateXML = simplexml_load_file($templateXMLFile);
	
	$modelXMLFile= $the_3dpfile;
	echo "modelXMLFile = $modelXMLFile \n";

	// echo "\n\ntheUTF8_3dpString = $theUTF8_3dpString\n\n";
	$modelXML = simplexml_load_string($theUTF8_3dpString);
	//$modelXML = simplexml_load_file($modelXMLFile);
	
	$templateShots = $templateXML -> SHOT;
	echo "\n\ntemplateShots = "; print_r($templateShots);
	$sumArray = array (
			"fovx" 		=> 0,
			"Tx" 		=> 0,
			"Ty" 		=> 0,
			"Tz" 		=> 0,
			"Rx" 		=> 0,
			"Ry" 		=> 0,
			"Rz" 		=> 0
	);
	$maxAray = array (
			"fovx" 		=> 0,
			"Tx" 		=> 0,
			"Ty" 		=> 0,
			"Tz" 		=> 0,
			"Rx" 		=> 0,
			"Ry" 		=> 0,
			"Rz" 		=> 0
	);

	// css: 
	// table{border-collapse:collapse;border:1px solid #000;}
	// table td{border:1px solid #000;}

	$tblheader = "</h3><table><tr><th>source</th><th>Shot</th><th>fovx(&deg)</th><th>Tx(mm)</th><th>Ty(mm)</th><th>Tz(mm)</th><th>Rx(&deg)</th><th>Ry(&deg)</th><th>Rz(&deg)</th></tr>";

	$templateTableString = "<div><h3>Template: ".$templateXMLFile. $tblheader;
	$modelTableString="<div><h3>Model: ".$modelXMLFile.$tblheader;
	$differenceTableString="<div><h3>Difference Table (float)".$tblheader;
	$differenceINTTableString="<div><h3>Difference Table (integer)".$tblheader;

	$index = 0;
	foreach ($templateShots as $templateShotValue) 
	{
		$shotNumber 	= intval($templateShotValue['i']);

		$templateShotArray =   array (
			"fovx" 		=> floatval($templateShotValue -> CFRM['fovx']),
			"Tx" 		=> floatval($templateShotValue -> CFRM -> T['x']),
			"Ty" 		=> floatval($templateShotValue -> CFRM -> T['y']),
			"Tz" 		=> floatval($templateShotValue -> CFRM -> T['z']),
			"Rx" 		=> floatval($templateShotValue -> CFRM -> R['x']),
			"Ry" 		=> floatval($templateShotValue -> CFRM -> R['y']),
			"Rz" 		=> floatval($templateShotValue -> CFRM -> R['z']),
		);	
	
		$templateTableString =	$templateTableString .
			"<tr> <td>Template" .
			"</td><td>" . $shotNumber .
			"</td><td>" . $templateShotArray['fovx'] .
			"</td><td>" . $templateShotArray['Tx'] .
			"</td><td>" . $templateShotArray['Ty'] .
			"</td><td>" . $templateShotArray['Tz'] .
			"</td><td>" . $templateShotArray['Rx'] .
			"</td><td>" . $templateShotArray['Ry'] .
			"</td><td>" . $templateShotArray['Rz'] . 
			"</td></tr>";
	
	
		// $modelShotValue = $modelXML -> SHOT[$shotNumber];
		$modelShotValue = $modelXML -> SHOT[$index];
	
		$modelShotArray =   array (
			"fovx" 		=> floatval($modelShotValue -> CFRM['fovx']),
			"Tx" 		=> floatval($modelShotValue -> CFRM -> T['x']),
			"Ty" 		=> floatval($modelShotValue -> CFRM -> T['y']),
			"Tz" 		=> floatval($modelShotValue -> CFRM -> T['z']),
			"Rx" 		=> floatval($modelShotValue -> CFRM -> R['x']),
			"Ry" 		=> floatval($modelShotValue -> CFRM -> R['y']),
			"Rz" 		=> floatval($modelShotValue -> CFRM -> R['z']),
		);
	
		$modelTableString =	$modelTableString .
			"<tr> <td>Model" .
			"</td><td>" . $shotNumber .
			"</td><td>" . $modelShotArray['fovx'] .
			"</td><td>" . $modelShotArray['Tx'] .
			"</td><td>" . $modelShotArray['Ty'] .
			"</td><td>" . $modelShotArray['Tz'] .
			"</td><td>" . $modelShotArray['Rx'] .
			"</td><td>" . $modelShotArray['Ry'] .
			"</td><td>" . $modelShotArray['Rz'] . 
			"</td></tr>";
		
	
		$differenceArray = array (
			"fovx" 		=> floatval($modelShotArray['fovx']) -   floatval($templateShotArray['fovx']),
			"Tx" 		=> floatval($modelShotArray['Tx']) - floatval($templateShotArray['Tx']),
			"Ty" 		=> floatval($modelShotArray['Ty']) - floatval($templateShotArray['Ty']),
			"Tz" 		=> floatval($modelShotArray['Tz']) - floatval($templateShotArray['Tz']),
			"Rx" 		=> floatval($modelShotArray['Rx']) - floatval($templateShotArray['Rx']),
			"Ry" 		=> floatval($modelShotArray['Ry']) - floatval($templateShotArray['Ry']),
			"Rz" 		=> floatval($modelShotArray['Rz']) - floatval($templateShotArray['Rz']),
		);
		
		// ReCap models for Rz vary between -180 and 180, the difference between -179 and 179 should really be -2, not 358
		if ($differenceArray["Rz"] >= 180 )   $differenceArray["Rz"] =  360 - $differenceArray["Rz"];
		else if ($differenceArray["Rz"] <= -180 )  $differenceArray["Rz"] = $differenceArray["Rz"] +360 ;
		
	
		
		$differenceTableString =	$differenceTableString .
			"<tr> <td>Difference" .
			"</td><td>" . $shotNumber .
			"</td><td>" . $differenceArray['fovx'] .
			"</td><td>" . $differenceArray['Tx'] .
			"</td><td>" . $differenceArray['Ty'] .
			"</td><td>" . $differenceArray['Tz'] .
			"</td><td>" . $differenceArray['Rx'] .
			"</td><td>" . $differenceArray['Ry'] .
			"</td><td>" . $differenceArray['Rz'] . 
			"</td></tr>";
		
		$differenceINTTableString =	$differenceINTTableString .
			"<tr> <td>Difference" .
			"</td><td>" . $shotNumber .
			"</td><td>" . intval($differenceArray['fovx']) .
			"</td><td>" . intval($differenceArray['Tx']) .
			"</td><td>" . intval($differenceArray['Ty']) .
			"</td><td>" . intval($differenceArray['Tz']) .
			"</td><td>" . intval($differenceArray['Rx']) .
			"</td><td>" . intval($differenceArray['Ry']) .
			"</td><td>" . intval($differenceArray['Rz']) . 
			"</td></tr>";
	
		$sumArray["fovx"] 	= $sumArray["fovx"] + abs($differenceArray["fovx"]);
		$sumArray["Tx"] 	= $sumArray["Tx"] + abs($differenceArray["Tx"]);
		$sumArray["Ty"] 	= $sumArray["Ty"] + abs($differenceArray["Ty"]);
		$sumArray["Tz"] 	= $sumArray["Tz"] + abs($differenceArray["Tz"]);
		$sumArray["Rx"] 	= $sumArray["Rx"] + abs($differenceArray["Rx"]);
		$sumArray["Ry"] 	= $sumArray["Ry"] + abs($differenceArray["Ry"]);
		$sumArray["Rz"] 	= $sumArray["Rz"] + abs($differenceArray["Rz"]);
	
		$maxArray["fovx"] 	= max($maxArray["fovx"] , abs($differenceArray["fovx"]));
		$maxArray["Tx"] 	= max($maxArray["Tx"] , abs($differenceArray["Tx"]));
		$maxArray["Ty"] 	= max($maxArray["Ty"] , abs($differenceArray["Ty"]));
		$maxArray["Tz"] 	= max($maxArray["Tz"] , abs($differenceArray["Tz"]));
		$maxArray["Rx"] 	= max($maxArray["Rx"] , abs($differenceArray["Rx"]));
		$maxArray["Ry"] 	= max($maxArray["Ry"] , abs($differenceArray["Ry"]));
		$maxArray["Rz"] 	= max($maxArray["Rz"] , abs($differenceArray["Rz"]));
	
		$index++;
	}

	$templateTableString =		$templateTableString . 	"</table></div>";
	$modelTableString =			$modelTableString . 	"</table></div>";

	$avgArray = array (
			"fovx" 	=> $sumArray["fovx"] / $shotNumber,
			"Tx" 	=> $sumArray["Tx"] / $shotNumber,
			"Ty" 	=> $sumArray["Ty"] / $shotNumber,
			"Tz" 	=> $sumArray["Tz"] / $shotNumber,
			"Rx" 	=> $sumArray["Rx"] / $shotNumber,
			"Ry" 	=> $sumArray["Rx"] / $shotNumber,
			"Rz" 	=> $sumArray["Rx"] / $shotNumber
		);
	
		$differenceTableString =	$differenceTableString .
			"<tr> <td>Absolute" .
			"</td><td>" . "AVG" .
			"</td><td>" . $avgArray['fovx'] .
			"</td><td>" . $avgArray['Tx'] .
			"</td><td>" . $avgArray['Ty'] .
			"</td><td>" . $avgArray['Tz'] .
			"</td><td>" . $avgArray['Rx'] .
			"</td><td>" . $avgArray['Ry'] .
			"</td><td>" . $avgArray['Rz'] . 
			"</td></tr>";	

	
		$differenceINTTableString =	$differenceINTTableString .
			"<tr> <td>Absolute" .
			"</td><td>" . "AVG" .
			"</td><td>" . intval($avgArray['fovx']) .
			"</td><td>" . intval($avgArray['Tx']) .
			"</td><td>" . intval($avgArray['Ty']) .
			"</td><td>" . intval($avgArray['Tz']) .
			"</td><td>" . intval($avgArray['Rx']) .
			"</td><td>" . intval($avgArray['Ry']) .
			"</td><td>" . intval($avgArray['Rz']) . 
			"</td></tr>";	
		
		
		$differenceTableString =	$differenceTableString .
			"<tr><td>Absolute" .
			"</td><td>" . "MAX" .
			"</td><td>" . $maxArray['fovx'] .
			"</td><td>" . $maxArray['Tx'] .
			"</td><td>" . $maxArray['Ty'] .
			"</td><td>" . $maxArray['Tz'] .
			"</td><td>" . $maxArray['Rx'] .
			"</td><td>" . $maxArray['Ry'] .
			"</td><td>" . $maxArray['Rz'] . 
			"</td></tr>";

		
		$differenceINTTableString =	$differenceINTTableString .
			"<tr><td>Absolute" .
			"</td><td>" . "MAX" .
			"</td><td>" . intval($maxArray['fovx']) .
			"</td><td>" . intval($maxArray['Tx']) .
			"</td><td>" . intval($maxArray['Ty']) .
			"</td><td>" . intval($maxArray['Tz']) .
			"</td><td>" . intval($maxArray['Rx']) .
			"</td><td>" . intval($maxArray['Ry']) .
			"</td><td>" . intval($maxArray['Rz']) . 
			"</td></tr>";
		
	$differenceTableString =	$differenceTableString .  "</table></div>";
	$differenceINTTableString =	$differenceINTTableString .  "</table></div>";

	$DiffPageString = $DiffPageString .  $differenceINTTableString;
	$DiffPageString = $DiffPageString .  $templateTableString;
	$DiffPageString = $DiffPageString .  $modelTableString;
	$DiffPageString = $DiffPageString .  $differenceTableString;

	$DiffPageString = $DiffPageString .  $htmlTrailer;

	$diffPageFile = $scan_directory . '/' . "DiffTable.html";
	file_put_contents($diffPageFile, $DiffPageString);

	echo "Difference Table at: $diffPageFile\n";
	echo "Difference Table  HTML at: $ModeledURL$scanID/DiffTable.html\n";


	// ----------------- END Write a Difference Table to the directory ---------------------------



} 
else {  // ($errorflag == true)
	// -------------------- PREPARE FAILURE NOTIFICATION TEXT ----------------------
	echo "ERROR! Couldn't retrieve $photoSceneID, file\n";
	$subject = "ERROR! Couldn't retrieve $photoSceneID";
	$message =  "$subject\n"
		. "3DP and OBJ files not retrieved.\n"
		. "New order: $ReceivedURL$scanID/$neworderfile - modeling failed.\n\n"
		. "View Scan images in $ReceivedURL$scanID. \n\n"
		. "Template Table  HTML at: $ReceivedURL$scanID/TemplateTable.html\n\n"
		. "View Manifest file at: $ReceivedURL$scanID/Manifest.xml\n"
		. "FROM: $from\n"
		. "SENDER: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com \n"
		. "REPLY-TO: $from\n"
		. "TO: $to\n"
		. "SUBJECT: $subject\n"
		. "REFERENCE ID : $referenceID\n"
		. "BODY: $body\n"
		. "ATTACHMENTS:\n\t$attachmentString\n\n"
		. "Scan  Received: \t $startTimeText\n" 
		. "Model Completed:\t $endTimeText\n"
		. "Model Duration: \t $modelingTimeString\n"
		. "CPU Time: \t $processingHours h: $processingRemainderMinutes m: $processingRemainderSeconds s\n\n"
		. $output_string;
		
	
	// -------------------- END OF PREPARE FAILURE NOTIFICATION TEXT ---------------
	
	// Save a template table for review
	
	// ----------------- Write Template Table to the directory ---------------------------

	echo "\n\nSaving a template file for review\n\n"; 

	$htmlHeader = '<!DOCTYPE html>'
		. 	'<html lang="en-US">'
		.	'<head>'
		.	'<title>SugarCube Template Table</title>'
		. 	'<meta charset="utf-8">'
		.	'<meta name="viewport" content="width=device-width">'
		.	'<style>'

		.	'table{border-collapse:collapse;border:1px solid #000;font-size:10px;}'
		.	'table td{border:1px solid #000;font-size:10px;text-align: right;margin-right: 1em;}'
		.	'table th{border:1px solid #000;font-size:10px;text-align: center;margin-right: 1em;}'
		.	'p{font-size:10px;}' 

		.   '</style>'
		.	'</head>'
		.	'<body>';

	$TempPageString = $htmlHeader;

	$htmlTrailer = '</body>';	

	$TempPageString = $TempPageString . "<div>";
	$TempPageString = $TempPageString .  "<h2>Template Table</h2>"; 

	$templateXMLFile =  RECEIVEDDIR . "$scanID/template-$scanID.3dp";
	echo "templateXMLFile = $templateXMLFile \n";

	$TempPageString = $TempPageString .  "<p>template = $templateXMLFile<br />";
	$templateXML = simplexml_load_file($templateXMLFile);


	$TempPageString = $TempPageString .  "</div>";

	$templateShots = $templateXML -> SHOT;


	$tblheader = "</h3><table><tr><th>source</th><th>Shot</th><th>fovx(mm)</th><th>Tx(mm)</th><th>Ty(mm)</th><th>Tz(mm)</th><th>Rx(&deg)</th><th>Ry(&deg)</th><th>Rz(&deg)</th></tr>";

	$templateTableString = "<div><h3>Template: ".$templateXMLFile. $tblheader;
	$modelTableString="<div><h3>Model: ".$modelXMLFile.$tblheader;

	foreach ($templateShots as $templateShotValue) 
	{
		$shotNumber 	= intval($templateShotValue['i']);

		$templateShotArray =   array (
			"fovx" 		=> floatval($templateShotValue -> CFRM['fovx']),
			"Tx" 		=> floatval($templateShotValue -> CFRM -> T['x']),
			"Ty" 		=> floatval($templateShotValue -> CFRM -> T['y']),
			"Tz" 		=> floatval($templateShotValue -> CFRM -> T['z']),
			"Rx" 		=> floatval($templateShotValue -> CFRM -> R['x']),
			"Ry" 		=> floatval($templateShotValue -> CFRM -> R['y']),
			"Rz" 		=> floatval($templateShotValue -> CFRM -> R['z']),
		);	

		$templateTableString =	$templateTableString .
			"<tr> <td>Template" .
			"</td><td>" . $shotNumber .
			"</td><td>" . $templateShotArray['fovx'] .
			"</td><td>" . $templateShotArray['Tx'] .
			"</td><td>" . $templateShotArray['Ty'] .
			"</td><td>" . $templateShotArray['Tz'] .
			"</td><td>" . $templateShotArray['Rx'] .
			"</td><td>" . $templateShotArray['Ry'] .
			"</td><td>" . $templateShotArray['Rz'] . 
			"</td></tr>";

	}

	$templateTableString =		$templateTableString . 	"</table></div>";

	$TempPageString = $TempPageString .  $templateTableString;

	$TempPageString = $TempPageString .  $htmlTrailer;

	file_put_contents(RECEIVEDDIR . "$scanID/TemplateTable.html", $TempPageString);

	echo "Template Table at: $diffPageFile\n";
	echo "Template Table  HTML at: " . $ReceivedURL . "$scanID/TemplateTable.html\n";


	// ----------------- END Write Template Table to the directory ---------------------------

	// -------------------- END OF UNZIP OBJ AND CONVERT TO STL  ----------------------
}


// ----------------- Write data to database ----------------------------------------------

// This version UPDATES a database record and populates the newly retrieved values.  

function serverFilePath($filepath)  {
	$URL =  "http://" . strstr($filepath, DOMAIN );
	return $URL;
}

$completedModel3DPURL = "$ModeledURL$scanID/$scanID.3dp";
$completedOBJDirURL = "$ModeledURL$scanID/$scanID.obj.zip";
$webGLModelURL = 	"$ModeledURL$scanID/$scanID.3dp" ;
$reCapCallbackStartTimeStamp = 	 date('Y-m-d H:i:s',$reCapCallbackStartTime);
$reCapCallbackCompletionTimeStamp =  date('Y-m-d H:i:s',time());


	$connection = new mysqli("mysql.test.soundfit.me", "soundfit_admin", "Use4SoundFit!", "sf_orders");
	$SQLQuery = 
	  "UPDATE templates SET "
	  . "CompletedModel3DPURL='". "$completedModel3DPURL" . "'" . ", "  
	  . "CompletedOBJDirURL='". "$completedOBJDirURL" . "'" . ", "  
	  . "WebGLModelViewerURL='". "$webGLModelURL" . "'" . ", "   
	  . "ReCapCallbackStartTimeStamp='". "$reCapCallbackStartTimeStamp" . "'" . ", "  
	  . "ReCapCallbackCompletionTimeStamp='". "$reCapCallbackCompletionTimeStamp" . "' "   
	  . " WHERE PhotoSceneID='" . $photoSceneID . "'";
	
	$result = mysqli_query($connection, $SQLQuery );
	
	echo "updating database: \n";
	echo "scanID=$scanID\n"; 
	echo "photoSceneID=$photoSceneID\n"; 
	echo "completedModel3DPURL=$completedModel3DPURL\n"; 
	echo "completedOBJDirURL=$completedOBJDirURL\n"; 
	echo "webGLModelViewerURL=$webGLModelURL\n"; 
	echo "reCapCallbackStartTimeStamp=$reCapCallbackStartTimeStamp\n"; 
	echo "reCapCallbackCompletionTimeStamp=$reCapCallbackCompletionTimeStamp\n"; 

	echo "$SQLQuery \n\n";
	echo "RESULT =  $result \n";
	
	//$row = mysqli_fetch_assoc($result);
	mysqli_close($connection);

// ----------------- END Write data to database ------------------------------------------	


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
$headers = "Sender: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com \nReply-To: " . $from."\nFrom: " . $from;
// echo "Headers = $headers <br />";
// echo "Subject:  $subject <br />";
// echo "Message = $message <br />";
mail($to,$subject,$message,$headers,  "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com");
// echo "#  Mail Sent.";
// -------------------- END OF SEND THE EMAIL NOTIFICATIONS  ----------------------



echo "\n------- End of ReCapRetrieveOBJ-v9h.php -----------------------------------\n\n";
echo "</pre>";
exit ;
?>

