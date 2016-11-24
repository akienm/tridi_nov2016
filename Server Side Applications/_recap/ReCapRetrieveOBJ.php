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
    
/*
 Copyright (c) Autodesk, Inc. All rights reserved 

 PHP ReCapFromStoredToken using stored OAuth token based on code by
 by Cyrille Fauvel - Autodesk Developer Network (ADN)
 August 2013
 
 Modified by Scott McGregor -- SoundFit
 August 2013
 Accessing stored token from access_token.txt
 */
 
// This program is called with a URL that defines the scanID.
// if this program is run from the command line, the scanID is passed as a CLI argument.
if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}
echo "php_sapi_name =  " . php_sapi_name() . "\n";
print_r($_GET);

$scanID = htmlspecialchars($_GET["scanID"]);
// echo "Retrieving scanID = $scanID\n";

// Once we know the scanID, we can check the maps directory that has mappings between 
// scanIDs and photoSceneIDs.   We need the photoSceneID to retrieve the proper OBJ file.
$mapdir = '/home/earchivevps/test.soundfit.me/Scans/au1/maps/';
$mapfile = $mapdir . $scanID;
// echo "mapfile = $mapfile\n";
$photoSceneID = trim(file_get_contents ($mapfile ));
// echo "photoSceneID = $photoSceneID\n";


 // First thing: Notify the user the callback function has been launched.

             $to = "scott@soundfit.me";
	         $headers = "From: sugarcube-daemon@soundfit.me";
             $subject = "LAUNCHED: ScanID($scanID) / model($photoSceneID)";
             "ReCapRetrieveOBJ.php Callback function launched -- scanID = $scanID";
             $message = print_r($_GET, true);
	         mail($to,$subject,$message,$headers);

 
require 'vendor/autoload.php' ;
use Guzzle\Http\Client ;
use Guzzle\Plugin\Oauth\OauthPlugin ;

define ('ConsumerKey', '076e503f-19d8-48f7-8a5b-8a2964fcf46f') ;
define ('ConsumerSecret', '509738df-9e15-453b-abc4-27de10059692') ;
define ('BaseUrl' ,'https://accounts.autodesk.com/') ;

define ('ReCapClientID', 'KkQgk1o4Vjwsg1aBShj12mix90g') ;
define ('ReCapKey', 'Y wWKk95WhQVvKdww4whI+vib3FmA') ;
define ('ReCapApiUrl', 'http://rc-api-adn.autodesk.com/3.0/API/') ;

include_once "vendor/oauth/OAuthStore.php" ;
include_once "vendor/oauth/OAuthRequester.php" ;

include_once "clientcalls.inc.php";

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

$fname =realpath (dirname (__FILE__)) . '/access_token.txt' ;
$access =unserialize (file_get_contents ($fname)) ;

//- ReCap

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
// echo "OAuth Consumer key:".ConsumerKey."\n";
// echo "OAuth Consumer secret:".ConsumerSecret."\n";


//$request =$client->get ('photoscene/pMcPoROCHWcyegvcbxxqD2NN7Ss') ;
$request =$client->get ("photoscene/$photoSceneID") ;
$request->getQuery ()->clear () ; // Not needed as this is a new request object, but.as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 
'clientID' => ReCapClientID, 'timestamp' => time (), 
'format' => 'obj')) ;
$response =$request->send () ;
//var_dump($response);
echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
$objURL = $xml->Photoscene->scenelink ;
echo "objURL = $objURL\n";

// Now we will copy the OBJ file from ReCap into the original Scan directory
$scan_dir_root =  '/home/earchivevps/test.soundfit.me/Scans/au1/received/';
$scan_directory = $scan_dir_root.$scanID; 
$scan_zipfile = $scan_directory . '/' . $scanID . ".obj.zip";
// echo "scan_zipfile = $scan_zipfile\n";

//Copy the zipped OBJ file from ReCap into the local directory
set_time_limit(0); 
$file = file_get_contents($objURL);
file_put_contents($scan_zipfile, $file);

//unzip the OBJ file:

// get the absolute path to $file
$path = pathinfo(realpath($scan_zipfile), PATHINFO_DIRNAME);
// echo "path = $path\n";
$zip = new ZipArchive;
$res = $zip->open($scan_zipfile);
if ($res === TRUE) {
  // extract it to the path we determined above
  $zip->extractTo($path);
  $zip->close();
  echo "SUCCESS: $scan_zipfile extracted to $path\n";
  $subject = "SUCCESS: ScanID($scanID) / model($photoSceneID) extracted to $path";
  $message = $subject . "\n OBJ file complete, ready for manufacturing\n";
  
  // use meshconv to create the STL file from the OBJ file
  $infilename =  $scan_directory . '/' .  "mesh.obj";
  // $tempfilename = "./tmp/SCS" . date("U");
  $tempfilename = $scan_directory . '/' .  "mesh";
  $shellcmd =  "/home/earchivevps/test.soundfit.me/apps/meshconv "
	                  . $infilename . " -c STL -o " . $tempfilename;
  $answer = `$shellcmd`;
  echo "convert to OBJ return code: $answer\n";
} else {
  //echo "ERROR! Couldn't retrieve $photoSceneID, couldn't open $scan_zipfile\n";
  $subject = "ERROR! Couldn't retrieve $photoSceneID, couldn't open $scan_zipfile";
  $message = $subject . " \nOBJ file not retrieved.";
}


// Notify the user the result of the scan
$mailflag = true;
if ( $mailflag )  {
            $to = "scott@soundfit.me";
	         // echo "Sending Mail to:$to <br />";
	         $from = "sugarcube-daemon@soundfit.me";
	         // echo "From: $from <br />";
	         $headers = "From: " . $from;
	         // echo "Headers = $headers <br />";
	         // echo "Subject:  $subject <br />";
	         // echo "Message = $message <br />";
	         mail($to,$subject,$message,$headers);
	         // echo "#  Mail Sent.";
	     }

// This is commented out as we don't want to delete the photoscene, since we may 
// want to access it in a separate program.
/* 
$request =$client->delete ("photoscene/{$photoSceneID}") ;
$request->addPostFields (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
$response =$request->send () ;
echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
if ( isset ($xml->Photoscene->deleted) && $xml->Photoscene->deleted == 0 )
	echo "My ReCap PhotoScene is now deleted\n" ;
else
	echo "Failed deleting the PhotoScene and resources!\n" ;
 */
 
 

 
exit ;
?>
