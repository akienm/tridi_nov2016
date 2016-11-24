<?
/*
 Copyright (c) Autodesk, Inc. All rights reserved 

 PHP Autodesk Oxygen/ReCap Sample
 by Cyrille Fauvel - Autodesk Developer Network (ADN)
 August 2013

 Permission to use, copy, modify, and distribute this software in
 object code form for any purpose and without fee is hereby granted, 
 provided that the above copyright notice appears in all copies and 
 that both that copyright notice and the limited warranty and
 restricted rights notice below appear in all supporting 
 documentation.

 AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
 AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
 MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
 DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
 UNINTERRUPTED OR ERROR FREE.
 
 Oxygen
 ----------
 This sample uses the stagging Oxygen server to demo the '3 legs' process to authentify
 a user on the Autodesk Cloud infrastructure.
 
 After installing PHP on your system, you may need to install the php_oauth.dll if your 
 distribution does not yet include it. Copy the dll into your PHP extension folder (I.e.: <PHP folder>\ext)
 and add the following lines in your php.ini 
 
		[PHP_OAUTH]
		extension=php_oauth.dll

 You can get precompiled php_oauth.dll from:
 http://windows.php.net/downloads/pecl/releases/oauth/1.2.3/

 The PHP Oauth API is documented here:
 http://php.net/manual/en/book.oauth.php

 The '3 legs' process is as follow:
 a- Get a 'request token' from the system
 b- Authorize the received token. Note here that Autodesk currently require you to manual log on Oxygen
      for authorization. This is why the sample is using your default browser for logging.
 c- Get an 'access token' and a session
 
 The sample also does a log-out at the end to complete the sample.
 
 ReCap
 ----------
  todo
 
 Dependencies
 --------------------
 * Guzzle ( http://www.guzzlephp.org/ )
    Guzzle requires cURL present in your PHP distribution
    Installing Guzzle in your project ( http://guzzlephp.org/getting-started/installation.html )
	
	If you want to debug http request
	   Configure a PHP/cURL application to use Fiddler ( http://fiddler2.com/documentation/Configure-Fiddler/Tasks/ConfigurePHPcURL )
	   curl_setopt ($handle, CURLOPT_PROXY, '127.0.0.1:8888') ;
	   ( for Guzzle append the above line in'vendor\guzzle\guzzle\src\Guzzle\Http\Curl\CurlHandle.php' line #195 )
	   
    Guzzle source code available https://github.com/guzzle/guzzle
*/

// http://davss.com/tech/php-rest-api-frameworks/

// Composer: http://getcomposer.org/


// PestXML (curl)
// http://github.com/educoder/pest

// Guzzle (http://www.guzzlephp.org/)
// https://github.com/guzzle/guzzle

require 'vendor/autoload.php' ;
use Guzzle\Http\Client ;
use Guzzle\Plugin\Oauth\OauthPlugin ;

define ('DefaultBrowser' ,'"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" ') ;

define ('ConsumerKey', '076e503f-19d8-48f7-8a5b-8a2964fcf46f') ;
define ('ConsumerSecret', '509738df-9e15-453b-abc4-27de10059692') ;
define ('BaseUrl' ,'https://accounts.autodesk.com/') ;

define ('ReCapClientID', 'Your ReCap ClientID') ;
define ('ReCapKey', 'Your ReCap Secret Key') ;
define ('ReCapApiUrl', 'http://rc-api-adn.autodesk.com/3.0/API/') ;

//- Oxygen
$token ='' ;
$access ='' ;

//- Prepare the PHP OAuth for consuming our Oxygen service
//- Disable the SSL check to avoid an exception with invalidate certificate on the server
$oauth =new OAuth (ConsumerKey, ConsumerSecret, OAUTH_SIG_METHOD_HMACSHA1, OAUTH_AUTH_TYPE_URI) ;
$oauth->enableDebug () ;
$oauth->disableSSLChecks () ;

try {
	//- 1st leg: Get the 'request token'
	$token =$oauth->getRequestToken (BaseUrl . "OAuth/RequestToken") ;
	//- Set the token and secret for subsequent requests.
	$oauth->setToken ($token ['oauth_token'], $token ['oauth_token_secret']) ;

	//- 2nd leg: Authorize the token
	//- Currently, Autodesk Oxygen service requires you to manually log into the system, so we are using your default browser
	$url =BaseUrl . "OAuth/Authorize" . "?oauth_token=" . urlencode (stripslashes ($token ['oauth_token'])) ;
	exec (DefaultBrowser . $url) ;
	//- We need to wait for the user to have logged in
	echo "Press [Enter] when logged" ;
	$psLine =fgets (STDIN, 1024) ;
	
	//- 3rd leg: Get the 'access token' and session
	$access =$oauth->getAccessToken (BaseUrl . "OAuth/AccessToken") ;
	//- Set the token and secret for subsequent requests.
	$oauth->setToken ($access ['oauth_token'], $access ['oauth_token_secret']) ;
	
	//- To refresh the 'Access token' before it expires, just call again
	//- $access =$oauth->getAccessToken (BaseUrl . "OAuth/AccessToken") ;
	//- Note that at this time the 'Access token' never expires
} catch (OAuthException $e) {
	echo "OAuth\n", 'Caught exception: ',  $e->getMessage (), "\n";
	exit ;
} catch (Exception $e) {
	echo "OAuth/Authorize\n", 'Caught exception: ',  $e->getMessage (), "\n";
	exit ;
}

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
 
//- Once you are done, log-out from Oxygen
try {
	$access =$oauth->getAccessToken (BaseUrl . "OAuth/InvalidateToken" . "?oauth_session_handle=" . urlencode (stripslashes ($access ['oauth_session_handle']))) ;
	echo "You logged out!\n" ;
	//- Clear the token and secret for subsequent requests.
	$oauth->setToken ('', '') ;
} catch (OAuthException $e) {
	echo "OAuth/InvalidateToken\n", 'Caught exception: ',  $e->getMessage (), "\n";
	exit ;
}

//- Done
exit ;

?>