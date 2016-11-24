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
echo "\n------- ReCapRetrieveByPhotoSceneID.php -----------------------------------\n\n";
$output_string = "Model retrieved by ReCapRetrieveByPhotoSceneID.php.\n\n";  

/*
 ReCapRetrieveByPhotoSceneID.php PhotoSceneID=N1t3P4Jw0cvj4vmSZmuxzhL7Uvs
 
 Copyright 2014 (c) SoundFit, LLC. All rights reserved 
 
 Developed by Scott McGregor -- SoundFit
 June 2014
 
 Revision 1.0
 
 PROGRAM DESCRIPTION:
 
 This program is retrieves a ReCap photoscene by using its photosceneID.	
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

 
// ----------------  GET PHOTOSCENE ID ----------------------------
// This program is called with a URL that defines the photoSceneID.
// if this program is run from the command line, the photoSceneID is passed as a CLI argument.

if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}
$photoSceneID = htmlspecialchars($_GET["photoSceneID"]);
echo "photoSceneID = $photoSceneID\n";

// ----------------  END OF GET  PHOTOSCENE ID ---------------------

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

echo "\nRetrieving PhotoSceneID $photoSceneID\n\n";
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
  				. $msg;
  	$errorflag = true;
  	
} else { // If we get here there SHOULD be a 3DP model ready for download

	$errorflag = false;
	
	// echo "photoscene $photoSceneID completed by Recap.\n"; 
	// $output_string = $output_string .  "photoscene $photoSceneID completed by Recap.\n"; 
	$the3dpURL = $xml->Photoscene->scenelink ;
	echo "3DP model for ScanID: $scanID \nis at: $the3dpURL\n\n";
	$output_string = $output_string . "3DP model for ScanID: $scanID \nis at: $the3dpURL\n\n";
	// -------------------- END OF GET THE PHOTOSCENE 3DP URL -------------------------

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
  
 
	}
}






echo "\n------- End of ReCapRetrieveByPhotoSceneID.php -----------------------------------\n\n";
echo "</pre>";
exit ;
?>

