<html>
	<head>
	</head>
	<body>
		<div>
 	              

<?php
/*
 Copyright (c) Autodesk, Inc. All rights reserved 

 PHP ReCap using stored OAuth token based on code by
 by Cyrille Fauvel - Autodesk Developer Network (ADN)
 August 2013
 
 Modified by Scott McGregor -- SoundFit
 August 2013
 
echo 'ReCapFromStoredToken.php';

require 'vendor/autoload.php' ;
use Guzzle\Http\Client ;
use Guzzle\Plugin\Oauth\OauthPlugin ;

define ('DefaultBrowser' ,'"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" ') ;

define ('ConsumerKey', '076e503f-19d8-48f7-8a5b-8a2964fcf46f') ;
define ('ConsumerSecret', '509738df-9e15-453b-abc4-27de10059692') ;
define ('BaseUrl' ,'https://accounts.autodesk.com/') ;

define ('ReCapClientID', 'KkQgk1o4Vjwsg1aBShj12mix90g') ;
define ('ReCapKey', 'Y wWKk95WhQVvKdww4whI+vib3FmA') ;
define ('ReCapApiUrl', 'http://rc-api-adn.autodesk.com/3.0/API/') ;

include_once "vendor/oauth/OAuthStore.php" ;
include_once "vendor/oauth/OAuthRequester.php" ;

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

//- Requesting the ReCap service/date to start and check our connection/authentification
$request =$client->get ('service/date') ;
$request->getQuery ()->clear () ; // Not needed as this is a new request object, but.as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
//- You must send a request in order for the transfer to occur
$response =$request->send () ;
//echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
echo "ReCap Server date/Time: {$xml->date}\n" ;

//- Requesting the ReCap version
$request =$client->get ('version') ;
$request->getQuery ()->clear () ; // Not needed as this is a new request object, but.as I am going to use merge(), it is safer
$request->getQuery ()->merge (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
$response =$request->send () ;
//echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
echo "ReCap Version: {$xml->version}\n" ;

//- Create a photoscene
$request =$client->post ('photoscene') ;
$request->addPostFields (array (
	'scenename' => 'MyPhotoScene' . time (),
	'meshquality' => '1',
	'format' => 'fbx',
	'clientID' => ReCapClientID, 'timestamp' => time (),
)) ;
$response =$request->send () ;
//echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
$photoSceneID =$xml->Photoscene->photosceneid ;
echo "My ReCap PhotoScene: {$photoSceneID}\n" ;

// Todo

//- Add files - resource POST /file
//- Process the scene - resource POST /photoscene/{$photoSceneID}
//- Loop until the job is processed
//      Query status using resource GET /photoscene/{$photoSceneID}/progress
//- Get the resulting mesh - resource GET /photoscene/{$photoSceneID}

//- Optionally, delete the photoscene
/* $request =$client->delete ("photoscene/{$photoSceneID}") ;
$request->addPostFields (array ( 'clientID' => ReCapClientID, 'timestamp' => time (), )) ;
$response =$request->send () ;
//echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;
$xml =$response->xml () ;
if ( isset ($xml->Photoscene->deleted) && $xml->Photoscene->deleted == 0 )
	echo "My ReCap PhotoScene is now deleted\n" ;
else
	echo "Failed deleting the PhotoScene and resources!\n" ;
 */
 
exit ;

?>
		
 	    </div>
	</body>
</html>
