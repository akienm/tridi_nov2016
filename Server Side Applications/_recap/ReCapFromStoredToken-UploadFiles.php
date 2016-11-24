<?php
/*
 Copyright (c) Autodesk, Inc. All rights reserved 

 PHP ReCapFromStoredToken using stored OAuth token based on code by
 by Cyrille Fauvel - Autodesk Developer Network (ADN)
 August 2013
 
 Modified by Scott McGregor -- SoundFit
 August 2013
 Accessing stored token from access_token.txt
 */
 
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
// var_dump($options);
OAuthStore::instance ('Session', $options) ;

$fname =realpath (dirname (__FILE__)) . '/access_token.txt' ;
// var_dump($fname);
$access =unserialize (file_get_contents ($fname)) ;
// var_dump($access);

//- ReCap

// Create a client and provide a base URL
$client =new Client (ReCapApiUrl) ;
// var_dump($client);
//- http://guzzlephp.org/guide/plugins.html
//- The Guzzle Oauth plugin will put the Oauth signature in the HTML header automatically
$oauthClient =new OauthPlugin (array (
	'consumer_key' => ConsumerKey,
	'consumer_secret' => ConsumerSecret,
	'token' => $access ['oauth_token'], //- access_token
	'token_secret' => $access ['oauth_token_secret'], //- access_token_secret
)) ;
// var_dump($oauthClient);
$client->addSubscriber ($oauthClient) ;
// var_dump($client);
echo "OAuth Consumer key:".ConsumerKey."\n";
echo "OAuth Consumer secret:".ConsumerSecret."\n";


$request =$client->get ("photoscene/pMcPoROCHWcyegvcbxxqD2NN7Ss") ;
$request->getQuery ()->clear () ; // Not needed as this is a new request object, but.as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 
'clientID' => ReCapClientID, 'timestamp' => time (), 
'format' => 'obj')) ;
$response =$request->send () ;
//var_dump($response);
 echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
//$xml =$response->xml () ;

exit(0);

//- Requesting the ReCap service/date to start and check our connection/authentification
$request =$client->get ('service/date') ;
// var_dump($request);
$request->getQuery ()->clear () ; // Not needed as this is a new request object, but.as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
//- You must send a request in order for the transfer to occur
$response =$request->send () ;
// var_dump($response);
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
echo "ReCap Server date/Time: {$xml->date}\n" ;

//- Requesting the ReCap version
$request =$client->get ('version') ;
$request->getQuery ()->clear () ; // Not needed as this is a new request object, but.as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
$response =$request->send () ;
// var_dump($response);
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
echo "ReCap Version: {$xml->version}\n" ;

//- Create a photoscene
$request =$client->post ('photoscene') ;
$request->addPostFields (array (
	'scenename' => 'MyPhotoScene' . time (),
	//'callback' => 'http://....',
	'meshquality' => '9',
	'format' => 'obj',
	'clientID' => ReCapClientID, 'timestamp' => time (),
)) ;
echo "ReCap Client ID: ".ReCapClientID."}\n";
$response =$request->send () ;
// var_dump($response);
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
$photoSceneID =$xml->Photoscene->photosceneid ;
echo "My ReCap PhotoScene: {$photoSceneID}\n" ;

// Todo


//- Add files - resource POST /file

// retrieve a scan folder from the RECEIVED directory
$scan_directory = '/home/earchivevps/test.soundfit.me/Scans/au1/received/ADVHEARING1000765';
$scan_directory = '/home/earchivevps/test.soundfit.me/Scans/au1/received/test';

// put the files in the scan directory into an array
// $scan_array = scandir ($scan_directory);
$scan_array = glob($scan_directory."/*{.jpg,.JPG}",GLOB_BRACE);
// remove any files that are not JPGs
foreach ($scan_array as $key => $value) {
//	$file_array[$key] = fopen($value, "r");  // do if we n
//	$ext = pathinfo($value, PATHINFO_EXTENSION);
//	if (strtoupper ($ext) != 'JPG') { unset ($scan_array[$key] ); }
} 
//var_dump($scan_array);


$data = array(
'photosceneid' => $photoSceneID,
	'replace' => '1',
	'type' => 'image',
	'clientID' => ReCapClientID, 'timestamp' => time ());
	
$nb = 0;
foreach ($scan_array as $key => $value) 
{	
	$data["file[$nb]"] = "@" . $value;
	$nb++;
}

$res = CURLcall($data, "file", "post", "http://rc-api-adn.autodesk.com/3.0/API/");
print_r($res);
$xml = simplexml_load_string($res);


/*
// create the ReCap Request to add the scan files
$request = $client->post ('file') ;
$request -> addPostFields (array (
	'photosceneid' => $photoSceneID,
	'replace' => '1',
	'type' => 'image',
	'clientID' => ReCapClientID, 'timestamp' => time (),
)) ;
$request->addPostFiles(array('file[]' => '/home/earchivevps/test.soundfit.me/Scans/au1/received/ADVHEARING1000765/2013-08-071040-R36_ADVHEARING1_040_300.jpg'));


// var_dump($request);
$response =$request->send () ;
// var_dump($response);
echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;

*/

$code =$xml->Error->code ;
echo "My ReCap File Upload error code: {$code}\n" ;
$msg  =$xml->Error->msg ;
echo "My ReCap File Upload error message: {$msg}\n" ;

// if error stop here else continue

//- Process the scene - resource POST /photoscene/{}

$request =$client->post ("photoscene/$photoSceneID") ;
$request->addPostFields (array (
	'clientID' => ReCapClientID, 'timestamp' => time (),
)) ;
$response =$request->send () ;
//var_dump($response);
 echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;



//- Loop until the job is processed
//      Query status using resource GET /photoscene/{$photoSceneID}/progress
//- Get the resulting mesh - resource GET /photoscene/{$photoSceneID}

$request =$client->get ("photoscene/$photoSceneID") ;
$request->getQuery ()->clear () ; // Not needed as this is a new request object, but.as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
$response =$request->send () ;
// var_dump($response);
 echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;


//- Optionally, delete the photoscene
/* 
$request =$client->delete ("photoscene/{$photoSceneID}") ;
$request->addPostFields (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
$response =$request->send () ;
// var_dump($response);
echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
if ( isset ($xml->Photoscene->deleted) && $xml->Photoscene->deleted == 0 )
	echo "My ReCap PhotoScene is now deleted\n" ;
else
	echo "Failed deleting the PhotoScene and resources!\n" ;
 */
 
exit ;

?>
