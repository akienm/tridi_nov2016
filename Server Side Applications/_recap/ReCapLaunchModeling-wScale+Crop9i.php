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
echo "\n------- ReCapLaunchModeling-wScale+Crop9i.php ------------------------------\n\n";
$outputString = "Scan processing by ReCapLaunchModeling-wScale+Crop9i.php.\n\n";

// track when process started so we can determine total clock time from  receipt of model
// to end of model.
$startTime = time(); 
$today = date("D M Y j G:i:s T");
echo "$today\n\n"; 

/*
 ReCapLaunchModeling-wScale+Crop9i.php
 
 Copyright 2013, 2014 (c) SoundFit, LLC. All rights reserved 
 
 Developed by Scott McGregor -- SoundFit
 August 2013 - April 2014
 
 Portions of this work are derived from examples provided by 
 by Cyrille Fauvel - Autodesk Developer Network (ADN) August 2013
 
 Revision 9i -  adds change of basis (change of reference axes) to
 automated templates
 
 Revision 9f  --  This version will use the RxOffset and FOVx from the manifest
 (originating in the ZString for this particular unit) in creating the automated
 template, if these values are present in the manifest,  otherwise it will use
 system wide defaults.
 
 Revision 9h  -- supports scans with more than 41 images with fix for user templates
 
 PROGRAM DESCRIPTION:
 
 This program launches Autodesk's ReCap software, and instructs it to 
 convert a set of scan image files into a 3D model. 
 
 Once launched by this program, ReCap runs asynchronously.
 
 When the ReCap job completes, it will send an email to a defined email account,
 and it will start-up a  separate callback program,  
 	ReCapRetrieveOBJ-v9i.php 
 also located in this same directory.
 
 ReCapRetrieveOBJ-v9i.php will check that ReCap created a model, and if it did, 
 it downloads the 3D model files as a 3DP and as an OBJ with texture map files. 
 ReCapRetrieveOBJ-v9i.php converts the OBJ to an STL format, and then sends a 
 final confirmation emails to a intended recipient of the model.

 This program requires several parameters: 
 
 * The scanID of the incoming folder of image files to be processed into a 3D model.
 * The template3DP file to be used to set the scale for the 3D model.
 * The boundingBox parameters used to crop the 3D model.
 * The to to be notified when the files have been received and modeling started.
 * The from to be notified when the model is complete.
 * A doModelScaling boolean flag which controls whether the model will be scaled.
 * A doModelCropping boolean flag which controls whether the model will be cropped to the bounding box.
 
 the scanID is
 passed as either a get string (when this program is run in a browser) or as a
 command line argument if run as a Command line program 
 	
 Each set of scan images is uploaded to that directory, and stored therein in a subfolder
 with the name of the scanID.  The program below then uses that scanID to retrieve all the
 image files therein (whose file names match the "snapshot*.jpg" pattern).   These images are
 sent to the server to start processing with ReCap.
 
 This program relies on a stored OAuth token to connect to the ReCap Server.
 The token is stored at: 
 	access_token.txt 
 in the same directory as this PHP program.
 
 This token is refreshed daily by a cron job located at
 	/home/earchivevps/test.soundfit.me/apps/_recap/AutodeskOAuthRefresh.sh
 
 If the token expires without being renewed, you need to get a new token.  
 This must be done interactively by accessing the page:
 	http://test.soundfit.me/apps/_recap/ReCapInteractiveLogin.php
 in a browser, and logging in with a registered Autodesk userID and password. 
 
 PROGRAM DESCRIPTION:
 
 This program launches Autodesk's ReCap software, and instructs it to 
 convert a set of scan image files into a 3D model.  
 
 SENDER and RECIPIENT email addresses are passed which tell this program 
 where to send notifications when the scan files have been accepted and processing 
 starts, when the model has been submitted to ReCap (or that ReCap refused to accept 
 the job.   The SENDER and RECIPIENT email addresses are also passed to the callback
 program, which will notify the sender when the model is complete (or ReCap failed).
 
 This program also uploads a "template" file which Autodesk can use to ensure that the 
 returned model is properly scaled, and it passes a set of parameters called a
 bounding box or meshbbox that is used to crop away parts of the model that we
 don't want.  
 
 For example, each 2D image includes both the object (ear impression)
 we are trying to capture, and part of the floor or side wall that is behind it.  
 Because the floor and side walls are in the images they would ordinarily be in the
 3D model too.   However we know how far the walls are from the center of the object,
 and we know how far the floor is from the center of the object.  By specifying a
 bounding box that is larger than the object, but not so large that it includes the 
 walls or floor, ReCap will ignore model those model points -- leaving us with just
 the model points of the object we want to model.
 
 Once launched by this program, ReCap runs asynchronously.
 
 When the ReCap job completes, it will send an email to a defined email account,
 and it will start-up a callback program  (also located in this same directory), called
 
 	/home/earchivevps/test.soundfit.me/apps/_recap/ReCapRetrieveOBJ-v9i.php, 
 	
 that will check to see if the ReCap created a model. 
 
 The callback program attemps to download the 3D model files from the Autodesk server.
 It then converts the OBJ model to an STL format, and sends a final confirmation email to
 sender and recipient email accounts.  That completes processing.

 PARAMETERS:
 
 This program requires the uploadDir parameter, others can be passed parameters 
 or can be specified in the Manifest.XML file in the uploadDir to override defaults: 
 
 * The $scanID of the incoming folder of image files to be processed into a 3D model.
 * The $clientVersion is an identifier of the client software version that generated the Manifest.xml
 * The $serverVersion is an identifier of the excpected server version that will read the Manifest.xml
 * The $scanStartTime is a timeStamp from the scanner when the scan was started -- from Manifest.XML only
 * The $uploadStartTime is a timeStamp from the scanner when the scan was started -- from Manifest.XML only
 * The $from to be notified when the scan set is accepted.
 * The $to to be notified when the files have been received and modeling started.
 * The $referenceID is a field that users can use to record orderIDs or their own way of tracking the scanset.
 * The $subject that will be used in the email to $to when the model is complete.
 * The $body is  the email text that will be delivered along with the model
 * The $attachments is an array of filenames of attachments  within current directory that will be included in the model directory.
 * The $doImageCropping is a boolean which controls whether uploaded images are cropped before submission
 * The $imageCroppingRectangle is the left_x,top_y,right_x,bottom_y of the cropping area relative to the top left of the source images
 * The $doModelScaling is a boolean that controls whether the model will be set to a scale defined by the template3DP file
 * The $doModelCropping is a boolean that controls whether the model is cropped according to the bounding box -- ignored if doModelScaling is false.
 * The $meshlevel is either 7,8, or 9 and determines the density of the mesh that the modeling will create.
 * The $boundingBox parameters used to crop the 3D model.
 * The $template3DP file to be used to set the scale for the 3D model.
 * The $shootingString is the shooting string used for capturing the scan
 * The $cameraID, $imageResolution, $jpegQuality, $videoProcAmp, and $cameraControl settings are passed only through the Manifest.xml file
 * The $testerMessages and $SugarCubeMessages hold file names for logs stored in the local directory and are passed only through the Manifest.xml file
 * The $imagefiles is an array of filenames within the current directory, naming each image file in the order it should be sent to the modeling program.
 
 Here is a sample Manifest XML file
 
 
 <SugarCubeScan>
	<metadata>
		<clientVersion>v1234</clientVersion>
		<serverVersion>b3-8c</serverVersion>
		<scanID>SLMu5c75np-UML-YYYY03DD144740R-cropped</scanID>
		<scanStartTime>3/29/2014 6:51:47 PM GMT-7</scanStartTime>
		<uploadStartTime>3/29/2014 6:55:47 PM GMT-7</uploadStartTime>
	</metadata>
	<order_details>
		<from>scott@soundfit.me</from>
		<to>scott@soundfit.me</to>
		<subject><![CDATA[3D Model: SLMu5c75np-UML-YYYY03DD144740R-cropped is ready!]]></subject>
		<referenceID><![CDATA[User-defined-order-number-etc.]]></referenceID>
		<body>
		<![CDATA[
			This is a 3D Model from the SoundFit SugarCube: 
			SLMu5c75np-UML-YYYY03DD144740R-cropped is ready!
		]]>
		</body>
		<attachments>
			<filename>ORDER_tester_010101.pdf.hcrypt.head</filename>
			<filename>ORDER_tester_010101.pdf.hcrypt.data</filename>
		</attachments>
	</order_details>
	<modelling_options>
		<doImageCropping>true</doImageCropping>
		<imageCroppingRectangle>450,340,1310,1190</imageCroppingRectangle>
		<doModelScaling>false</doModelScaling>
		<doModelCropping>false</doModelCropping>
		<meshlevel>7</meshlevel>
		<boundingBox>-40,-40,-20,40,40,40</boundingBox>
		<template3DP>/home/earchivevps/test.soundfit.me/Scans/b3/templates/SLMb1u4r7-007830L-00j.3dp</template3DP>
	</modelling_options>
	<imageset id='Left Ear'>
		<image elevation='' rotation = ''>snapshot-14282280.jpg</image>
		<image elevation='' rotation = ''>snapshot-14282571.jpg</image>
			...
		<image elevation='' rotation = ''>snapshot-14302853.jpg</image>
	</imageset>
	<diagnostics>
		<shootingString>SNHhDDDDDDDDTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTHhDDDDDDTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTRTHhTFSx</shootingString>
		<cameraID>IZone</cameraID>
		<imageResolution>1600x1200</imageResolution>
		<jpegQuality>100</jpegQuality>
		<videoProcAmp>
			<brightness>0</<sugarCubeMessages>>
			<contrast>0</contrast>
			<hue>0</hue>
			<saturation>64</saturation>
			<sharpness>2</sharpness>
			<gamma>100</gamma>
			<whiteBalance>4600</whiteBalance>
			<backlightComp>3</backlightComp>
			<gain>0</gain>
			<colorEnable>false</colorEnable>
			<powerLineFrequency>60hz</powerLineFrequency>
		</videoProcAmp>
		<cameraControl>
			<zoom>disabled</zoom>
			<focus>disabled</focus>
			<exposure>disabled</exposure>
			<aperture>disabled</aperture>
			<pan>disabled</pan>
			<tilt>disabled</tilt>
			<roll>disabled</roll>
			<lowLightCompensation>false</lowLightCompensation>
		</cameraControl>
		<testerMessages>testerMsg.log</testerMessages>
		<SugarCubeMessages>SugarCubeMsg.log</SugarCubeMessages>
	<diagnostics>
</SugarCubeScan>
 
 OLD FORMAT:
 
 
 HOW THIS PROGRAM IS STARTED:
 	
 This program is usually invoked by a URL request from the client PC that has just
 completed uploading the scan image files to the directory:
 	http://test.soundfit.me/Scans/b3/new/senderID/scanID.
 In the first step of this program, these files are moved to the	
  	http://test.soundfit.me/Scans/b3/received/scanID
 directory.  This means that files in the new directory are in the process of being uploaded
 while files in the received directory have been completely uploaded, and processing by
 this program has started.
 	
 When the SugarCube software runs it creates a folder of the images it captures, which
 it stores on the local computer temporarily in a folder with the ScanID name. It then 
 moves the folder up to the SoundFit server using FTP).  
 	
 Each set of scan image folders is transfered to the received directory, and stored therein 
 in a subfolder with the name of the scanID.  This program  then uses that scanID to 
 retrieve all the image files within (whose file names match the "snapshot*.jpg" pattern).   
 These images are uploaded to the server to start processing with ReCap.
 
 OAUTH AUTHORIZATION
 
 Autodesk uses the OAUTH authorization service to ensure that only registered users
 are accessing the ReCap services and the private files they have created and stored
 in the cloud.   
 
 When ReCap is run interactively the user uses a UserID and Password as credentials.
 However since this program is running as a non-interactively, we log in once, and then
 store an OAuth "token" in a file.  We use that token to gain access to ReCap and
 we have a cron file that "renews" the token every night at midnight.
 
 HOW OAUTH AUTHORIZATION IS HANDLED
 
 This program relies on a stored OAuth token to connect to the ReCap Server.
 The token is stored at: 
 	/home/earchivevps/test.soundfit.me/apps/_recap/access_token.txt 
 in the same directory as this PHP program.
 
 This token is refreshed daily by a cron job located at
 	/home/earchivevps/test.soundfit.me/apps/_recap/AutodeskOAuthRefresh.sh
 
 If the token expires without being renewed, you need to get a new token.  
 This must be done interactively by accessing the page:
 	http://test.soundfit.me/apps/_recap/ReCapInteractiveLogin.php
 in a browser, and logging in with a registered Autodesk userID and password. 
 */
 
 
 
// ----------------  DEFINITIONS AND GLOBAL VARIABLES ----------------------------
// OAUTH:  The following definitions and include files are used to support OAUTH access 
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
define ('OAUTH_ACCESS_TOKEN', '/home/earchivevps/test.soundfit.me/apps/_recap/access_token.txt');  

// AUTODESK RECAP CONSTANTS / PARAMETERS
// choose the mesh quality you want 
// define ('MeshQuality', '9'); // 9 is highest quality, longest processing time
define ('MeshQuality', '7'); // 7 is "normal" quality, shortest processing time
// Autodesk says don't use levels below 7.

//SOUNDFIT SPECIFIC CONSTANTS AND DEFAULT VALUES FOR PARAMETERS
// Directory where scan folders ready for modelling are stored:
define ('NEW_DIR', '/home/earchivevps/test.soundfit.me/Scans/b3/new/');
// define ('OLD_DIR', '/home/earchivevps/test.soundfit.me/Scans/b3/old/');
define ('SCAN_DIR_ROOT', '/home/earchivevps/test.soundfit.me/Scans/b3/received/');
define ('SCAN_DIR_URL', 'http://test.soundfit.me/Scans/b3/received/');
define ('EMAILMAPDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/emailmaps/');
define ('MAPDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/maps/');
define ('INDEX_FILE_TEMPLATE', "/home/earchivevps/test.soundfit.me/apps/_recap/gallery.php");
define ('DOMAIN', "test.soundfit.me");

define ('TEMPLATE3DP', '');  // The default is NO template file -- which generates an automatic one.
//define ('MESHBBOX', '-40,-40,0.5,40,40,40');   // default bounding box
define ('MESHBBOX', '-120,-120,-120,120,120,120');   // default bounding box
define ('SENDER', 'scott@soundfit.me');        // default sender email
define ('RECIPIENT', 'scott@soundfit.me');     // default recipient email
define ('SUBJECT', 'A SugarCube Scan 3D Model is ready for you');     // default Subject
define ('BODY', 'A SugarCube Scan 3D Model is ready for you');     // default Body
define ('ATTACHMENTS', NULL );     // default Body
//  define ('RECIPIENT', 'rnd@soundfit.me');             // use for testing
//  define ('RECIPIENT', 'pcl-orders@soundfit.me');      // use for testing
define ('CALLBACKPGM', 'http://test.soundfit.me/apps/_recap/ReCapRetrieveOBJ-v9i.php?scanID=');
define ('DO_MODELSCALING', false);   // Use to toggle scaling on or off
define ('DO_MODELCROPPING', false);  // Use to toggle scaling on or off
define ('DOIMAGECROPPING', false);  // Use to toggle scaling on or off
// define ('IMAGECROPPINGRECTANGLE', '450,340,1310,1190' );  // used for automated image cropping
// define ('IMAGECROPPINGRECTANGLE', 'square' );  // used for automated image cropping

define (DEFAULTLENSEQUIV, "38");  // as reccomended by Ray. Dennis' simulator says 35.28
define (DEFAULTFOV, "54.06");  // as calculated using Dennis' simulator

define (DEFAULTSCALEFACTORX, "1.00");  // If scale factor not set, leave magnification unchanged = 1.0X
define (DEFAULTSCALEFACTORY, "1.00");  // If scale factor not set, leave magnification unchanged = 1.0X
define (DEFAULTSCALEFACTORZ, "1.00");  // If scale factor not set, leave magnification unchanged = 1.0X

define (DEFAULTRXOFFSET, "0");  // If Rx Angle offset not set, leave unchanged = 0
define (DEFAULTRYOFFSET, "0");  // If Ry Angle offset not set, leave unchanged = 0
define (DEFAULTRZOFFSET, "0");  // If Rz Angle offset not set, leave unchanged = 0

define (DEFAULTTXOFFSET, "0");  // If Tx displacemnt offset not set, leave unchanged = 0
define (DEFAULTTYOFFSET, "0");  // If Ty displacemnt offset not set, leave unchanged = 0
define (DEFAULTTZOFFSET, "0");  // If Tz displacemnt offset not set, leave unchanged = 0

define (DEFAULTAngleXOFFSET, "0");  // If Angle x offset not set, leave unchanged = 0
define (DEFAULTAngleYOFFSET, "0");  // If Angle y offset not set, leave unchanged = 0
define (DEFAULTAngleZOFFSET, "0");  // If Angle z offset not set, leave unchanged = 0

define (DEFAULTFOVS,"c");  // FOVS = c, means FOV is constant for all images, but search for value for one image
//define (DEFAULTFOVS,"k");  // FOVS = k, means FOFV is constant for all images, and do use value provided - do not change
//define (DEFAULTFOVS,"s");  // FOVS = s, means FOV varies between images, but search for value for each image


define (DEFAULTPPS,"c");  // PPS = c, means PPS is constant for all images, but search for value for one image
//define (DEFAULTPPS,"k");  // PPS = k, means PPS is constant for all images, and do use value provided - do not change
//define (DEFAULTPPS,"s");  // PPS = s, means PPS varies between images, but search for value for each image



// changeBasis converts points in one cartesian coordinate system into 
// coordinates in an alternate cartesian coordinate system whose origin
// is displaced by a vector called originTranslationVector, and whose
// axes are tilted relative to the old axes by angles defined in the angularVector,
// and whose scales are defined by scaleVector.

// oldPoint and newPoint are 3D  point objects whose coordinates are in mm.
// originTranslationVector is a 3D vector object whose coordinates are in mm
// angularVector is a 3D vector of angles in degrees
// scaleVector is a 3D vector of scaleless magnification factors.

//  example:

//  newPoint = $oldPoint->changeBasis($originTranslationVector, $angularVector, $scaleVector);

// MATRIX MATH FUNCTIONS
function dotproduct($array1, $array2)   
{
	$n =  array();
	$n = ($array1[0] * $array2[0]) + ($array1[1] * $array2[1]) + ($array1[2] * $array2[2]);
	return $n;
}

function multiply3DMatrixBy3DArray($oldArray, $matrix) {

		$newArray[0] = dotProduct($oldArray, $matrix[0]);
		$newArray[1] = dotProduct($oldArray, $matrix[1]);
		$newArray[2] = dotProduct($oldArray, $matrix[2]);
		
		/*
		echo "<table>";
		echo "<tr><td>|</td><td>".$oldArray[0]."</td><td>|</td><td width='25px'></td><td>| ".$matrix [0][0]."</td><td>,</td><td>".$matrix [0][1]."</td><td>,</td><td>".$matrix [0][2]."</td><td> |</td><td width='25px'> </td><td>|</td><td>".$newArray[0]."</td><td> |</td></tr>";
		echo "<tr><td>|</td><td>".$oldArray[1]."</td><td>|</td><td>X</td><td>| ".$matrix [1][0]."</td><td>,</td><td>".$matrix [1][1]."</td><td>,</td><td>".$matrix [1][2]."</td><td>|</td><td>=</td><td>|</td><td>".$newArray[1]."</td><td> |</td></tr>";
		echo "<tr><td>|</td><td>".$oldArray[2]."</td><td>|</td><td></td><td>| ".$matrix [2][0]."</td><td>,</td><td>".$matrix [2][1]."</td><td>,</td><td>".$matrix [2][2]."</td><td> |</td><td> </td><td>|</td><td>".$newArray[2]."</td><td> |</td></tr>";
		echo"<br />";
		*/

		return $newArray;
	}


// CLASS DEFINITION:

class Point 
{
	// properties:
	public  $x = 0.0;
	public  $y = 0.0;
	public  $z = 0.0;
	public  $coordinates = array("x" => 0.0 , "y" => 0.0 , "z" => 0.0 );
	
	// constructor method
	function __construct() 
    { 
    	$a = func_get_args(); 
        $i = func_num_args(); 

    	if ($i == 3) {  // $newPoint = new Point( (float) $x, (float) $y, (float) $z)
			$this->x = $a[0];
			$this->y = $a[1];
			$this->z = $a[2];
		} else if ($i == 1 )  {
			if (is_array($a[0]) )  // $newPoint = new Point( (array) $xyztriple)
			{
				print_r($a[0]);
				$this->x = $a[0]["x"];
        		$this->y = $a[0]["y"];
        		$this->z = $a[0]["z"];
        	} else if (is_object($a[0]) ) // $newPoint = new Point( (object) $xyztriple)
        	{
        		print_r($a);
        		$this->x = $a[0]->x;
        		$this->y = $a[0]->y;
        		$this->z = $a[0]->z;
			} else {   // one parameter, other than an array or object, doesn't make sense
				$this->x = 0.0;
        		$this->y = 0.0;
        		$this->z = 0.0;
        		echo "Warning: parameter (" .gettype($a[0]). ") is not an array or an object<br />";
        		print_r($a[0]); echo "<br/>";
        	} 
        } else { // $newPoint = new Point()  // zero parameters
        	$this->x = 0.0;
        	$this->y = 0.0;
        	$this->z = 0.0;
        	if ($i != 0 ) { // 2 parameters or more than 3 doesn't make sense
        		echo "Warning: wrong number of parameters($i). Should be 0, 1, or 3\n";
        	}
        }
        $this->coordinates = array("x" => $this->x , "y" => $this->y , "z" => $this->z );
        return $this;
    } 
		

	//methods
	public function moveBasis($originTranslationVector) 
	{
		$newPoint = new Point(
			$this->x + $originTranslationVector->x ,  
			$this->y + $originTranslationVector->y , 
			$this->z + $originTranslationVector->z 
		);
		return $newPoint;
	}
	
	public function rotateBasis($angularVector) 
	{
		$radianVector = new Point(
			deg2rad($angularVector->x),
			deg2rad($angularVector->y),
			deg2rad($angularVector->z)
		);
		
		$rotationMatrixX = 
			array(   array( 1 , 0						, 0 						),
					 array( 0 , cos($radianVector->x) 	, -sin($radianVector->x)   ),
					 array( 0 , sin($radianVector->x) 	, cos($radianVector->x) 	)
			);
			
		$rotationMatrixY = 
			array( array( cos($radianVector->y) 	, 0	, -sin($radianVector->y)	),
				   array( 0 						, 1 , 0 						),
				   array( 0 , -sin($radianVector->y) 	, cos($radianVector->y) 	)
			);	
			
		$rotationMatrixZ = 
			array( array( cos($radianVector->z) 	, -sin($radianVector->z)	, 0	),
				   array( sin($radianVector->z)	, cos($radianVector->z) 	, 0 ),
				   array( 0 						, 0							, 1 ) 	
			);		
		
		$startingPoint[0] = $this->coordinates["x"];
		$startingPoint[1] = $this->coordinates["y"];
		$startingPoint[2] = $this->coordinates["z"];
		//echo "startingPoint = ";print_r($startingPoint); echo "<br />";
		$xRotation = multiply3DMatrixBy3DArray( $startingPoint, $rotationMatrixX );
		//echo "xRotation = "; print_r($xRotation); echo "<br />";
		
		$yRotation = multiply3DMatrixBy3DArray( $xRotation, $rotationMatrixY );
		//echo "yRotation = "; print_r($yRotation); echo "<br />";
		
		$zRotation = multiply3DMatrixBy3DArray( $yRotation, $rotationMatrixZ );
		//echo "zRotation = "; print_r($zRotation); echo "<br />";
	   
		$newPoint = new Point($zRotation[0],$zRotation[1],$zRotation[2]);
		//echo "<br />newPoint = "; print_r($newPoint); echo "<br />";
		return $newPoint;
	}
	
	public function scaleBasis($scaleVector) 
	{
		$newPoint = new Point( 
			$this->x * $scaleVector->x ,  
			$this->y * $scaleVector->y , 
			$this->z * $scaleVector->z 
		);
		return $newPoint;
	}
	
	public function printArray ()
	{
		$printstring = "( " .$this->x." , ".$this->y." , ".$this->z." )";
		return $printstring;
	}
	
	public function changeBasis($originTranslationVector, $angularVector, $scaleVector) 
	{	
		$thisPoint	= $this;
		//echo "thisPoint =  ".$this->printArray()."<br />";
		//echo "originTranslationVector = " . $originTranslationVector->printArray() . "<br \>";
		$translatedCoordinates = $thisPoint->moveBasis($originTranslationVector);
		//echo "translatedCoordinates =  ".$translatedCoordinates->printArray()."<br /><br />";
		
		//echo "angularVector = " . $angularVector->printArray() . "<br \>";
		$angledCoordinates = $translatedCoordinates->rotateBasis($angularVector);
		//echo "angledCoordinates =  ".$angledCoordinates->printArray()."<br /><br />";
		
		//echo "scaleVector = " . $scaleVector->printArray() . "<br />";
		$scaledCoordinates = $angledCoordinates->scaleBasis($scaleVector);
		//echo "scaledCoordinates =  ".$scaledCoordinates->printArray()."<br /><br />";
		return $scaledCoordinates;
	}
	
}   // end of Point Object definition



// ----------------  END OF DEFINITIONS AND GLOBAL VARIABLES ----------------------


// -------------------- PASSED PARAMETERS -----------------------------------------
// Various parameters can be passed by the calling program (normal case)
// or a user who runs the program from a command line (testing).
// most of these parameters (except UploadDir which is mandatory)
// will be retrieved from the Manifest.XML file.  But if passed parameters
// are present they over-ride those in the Manifest.XML file. 
 
// These other parameters, other than ScanID are optional and have a default value
// if they are not specified in either the Manifest.XML file nor as passed parameters. 
// The program checks to see if it is run in a command line,  if it is, all parameters
// are pulled from the argv list and put into the $_GET array.   

if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}

$uploadDir = htmlspecialchars($_GET["uploadDir"]);
echo "passed in uploadDir = $uploadDir \n";
$outputString = $outputString . "passed in uploadDir = $uploadDir \n";
if ($uploadDir == NULL) {
	echo "ERROR: uploadDir is a required parameter.\n";  
	echo "uploadDir = $uploadDir \n";
	$outputString = $outputString . "ERROR: uploadDir is a required parameter.\n";
	$errorflag = true;
} else { 
	$uploadDir =  realpath($uploadDir);
	$uploadTime = filemtime($uploadDir);
	echo "uploadDir = $uploadDir \n";
	$errorflag = false; 
	$manifestXMLFile = $uploadDir.'/Manifest.xml';
	echo "Manifest file is $manifestXMLFile\n";
	$outputString = $outputString . "Manifest file is $manifestXMLFile\n";

	if (file_exists($manifestXMLFile)) { // Manifest file exists 
		$manifestXML = simplexml_load_file($manifestXMLFile);
		$manifestXMLString = $manifestXML ->asXML();
		// echo "Before shortening: manifestXMLString = $manifestXMLString\n";
		$manifestXMLString = strstr($manifestXMLString,"<SugarCubeScan");
		echo "After shortening: manifestXMLString = $manifestXMLString\n\n";
		
	} else {  // No Manifest file! 
		$errorflag = true;
		echo "Manifest.xml file does not exist.\n";
		$outputString = $outputString . "Manifest.xml file does not exist.\n";       	 
	} 
}
 


	




// PROCESS  METADATA IN MANIFEST.XML --------------------------------------------

// get <metadata> fields from the Manifest file into an XML string.
$metadata 	= 	$manifestXML -> metadata;
$metadataXMLString =  $manifestXML -> metadata -> asXML();

$scanID = htmlspecialchars($_GET["scanID"]);
if ($scanID == NULL) { 
	$scanID =	$manifestXML -> metadata -> scanID;
	echo "scanID in Manifest.xml = $scanID\n";	
} else echo "passed in scanID = $scanID \n";	
if ($scanID == NULL) { // the scanID should be the basename of the upload Dir
	$scanID = basename ($uploadDir);
}
echo "Retrieving scanID = $scanID from $uploadDir\n";
$outputString = $outputString . "Retrieving scanID = $scanID from $uploadDir\n";

// Metadata other than ScanID are not used right now.  They are just saved for troubleshooting.

// END PROCESS  METADATA IN MANIFEST.XML --------------------------------------------


// PROCESS DIAGNOSTICS IN MANIFEST.XML --------------------------------------------

// MANIFEST: get the <diagnostics> fields from the Manifest file into an XML string.
$diagnostics 	= 	$manifestXML -> diagnostics;
$diagnosticsXMLString =  $manifestXML -> diagnostics ->asXML();

// Diagnostics are not used right now.  They are just saved for troubleshooting.

// END PROCESS DIAGNOSTICS IN MANIFEST.XML --------------------------------------------


// PROCESS ZString IN MANIFEST.XML --------------------------------------------

// MANIFEST: get the <ZValues> fields from the Manifest file into an XML string.
$zstring 	= 	$manifestXML -> ZValues;
$zstringXMLString =  $manifestXML -> ZValues ->asXML();

// Right now one of the parameter we expect to get from the ZString is the RXOffset
// Manifest tag <RX> inside <ZValues>.
// If the RX parameter is set via the command line or query string, it over-rides
// the value in the Manifest.
// If the parameter is not set in the command line, query string or Manifest,  
// or  if the parameter is not a floating point number a system wide default is used.

$RxOffset = htmlspecialchars($_GET["RX"]);  // RX is the RxOffset 
if ($RxOffset == NULL) {
	$RxOffset 	=  trim($manifestXML -> ZValues -> RX);
	echo "RX in Manifest.xml = $RxOffset\n";
}	else {
	echo "passed in RX = $RxOffset \n";
}
if (!is_numeric($RxOffset)){
	$RxOffset 	=  floatval(DEFAULTRXOFFSET);
} else $RxOffset = floatval($RxOffset);

echo "Rx = $RxOffset \n";
$outputString = $outputString .  "RxOffset = $RxOffset \n";

$RyOffset = htmlspecialchars($_GET["RY"]);  // RY is the RyOffset 
if ($RyOffset == NULL) {
	$RyOffset 	=  trim($manifestXML -> ZValues -> RY);
	echo "RY in Manifest.xml = $RyOffset\n";
}	else {
	echo "passed in RY = $RyOffset \n";
}
if (!is_numeric($RyOffset)){
	$RyOffset 	=  floatval(DEFAULTRYOFFSET);
} else $RyOffset = floatval($RyOffset);

echo "RY = $RyOffset \n";
$outputString = $outputString .  "RyOffset = $RyOffset \n";


$RzOffset = htmlspecialchars($_GET["RZ"]);  // RZ is the RzOffset 
if ($RzOffset == NULL) {
	$RzOffset 	=  trim($manifestXML -> ZValues -> RZ);
	echo "RZ in Manifest.xml = $RzOffset\n";
}	else {
	echo "passed in RZ = $RzOffset \n";
}
if (!is_numeric($RzOffset)){
	$RzOffset 	=  floatval(DEFAULTRZOFFSET);
} else $RzOffset = floatval($RzOffset);

echo "RZ = $RzOffset \n";
$outputString = $outputString .  "RzOffset = $RzOffset \n";


$fieldOfView = htmlspecialchars($_GET["FX"]);  // FX is the horizontal Angle of View
if ($fieldOfView == NULL) {
	$fieldOfView 	=  trim($manifestXML -> ZValues -> FX);
	echo "FX in Manifest.xml = $fieldOfView\n";
}	else {
	echo "passed in FX = $fieldOfView \n";
}
if (!is_numeric($fieldOfView)){
	$fieldOfView 	=  floatval(DEFAULTFOV);
} else $fieldOfView = floatval($fieldOfView);

echo "FX = $fieldOfView \n";
$outputString = $outputString .  "FieldOfView = $fieldOfView \n";


$FOVS = htmlspecialchars($_GET["FOVS"]);  // FOVS tells ReCap how to use Field of View for scaling
if ($FOVS == NULL) {
	$FOVS 	=  trim($manifestXML -> ZValues -> FOVS);
	echo "FOVS in Manifest.xml = $FOVS\n";
}	else {
	echo "passed in FOVS = $FOVS \n";
}
if ($FOVS == NULL){
	$FOVS 	=  DEFAULTFOVS;
} 

echo "FOVS = $FOVS \n";
$outputString = $outputString .  "FOVS = $FOVS \n";



$PPS = htmlspecialchars($_GET["PPS"]);  // PPS tells ReCap how to use Principal Point camera information
if ($PPS == NULL) {
	$PPS 	=  trim($manifestXML -> ZValues -> PPS);
	echo "PPS in Manifest.xml = $PPS\n";
}	else {
	echo "passed in PPS = $PPS \n";
}
if ($PPS == NULL){
	$PPS 	=  DEFAULTPPS;
} 

echo "PPS = $PPS \n";
$outputString = $outputString .  "PPS = $PPS \n";






//  SCALE FACTORS:  $newScales =  new Point( $scaleFactorX,  $scaleFactorY, $scaleFactorZ);

$scaleFactorX = htmlspecialchars($_GET["SFX"]);  // SFX is the scaleFactor in the X dimension
if ($scaleFactorX == NULL) {
	$scaleFactorX 	=  trim($manifestXML -> ZValues -> SFX);
	echo "SFX in Manifest.xml = $scaleFactor\n";
}	else {
	echo "passed in SFX = $scaleFactorX \n";
}
if (!is_numeric($scaleFactorX)){
	$scaleFactorX 	=  floatval(DEFAULTSCALEFACTORX);
	echo "setting ScaleFactorX to system default = $scaleFactorX\n";
} else $scaleFactorX = floatval($scaleFactorX);

echo "SFX = $scaleFactorX \n";
$outputString = $outputString .  "scaleFactorX = $scaleFactorX \n";


$scaleFactorY = htmlspecialchars($_GET["SFY"]);  // SFY is the scaleFactor in the Y dimension
if ($scaleFactorY == NULL) {
	$scaleFactorY 	=  trim($manifestXML -> ZValues -> SFY);
	echo "SFY in Manifest.xml = $scaleFactor\n";
}	else {
	echo "passed in SFY = $scaleFactorY \n";
}
if (!is_numeric($scaleFactorY)){
	$scaleFactorY 	=  floatval(DEFAULTSCALEFACTORY);
	echo "setting ScaleFactorY to system default = $scaleFactorY\n";
} else $scaleFactorY = floatval($scaleFactorY);

echo "SFY = $scaleFactorY \n";
$outputString = $outputString .  "scaleFactoryY = $scaleFactorY \n";


$scaleFactorZ = htmlspecialchars($_GET["SFZ"]);  // SFZ is the scaleFactor in the Y dimension
if ($scaleFactorZ == NULL) {
	$scaleFactorZ 	=  trim($manifestXML -> ZValues -> SFZ);
	echo "SFZ in Manifest.xml = $scaleFactorZ\n";
}	else {
	echo "passed in SFZ = $scaleFactorZ \n";
}
if (!is_numeric($scaleFactorZ)){
	$scaleFactorZ 	=  floatval(DEFAULTSCALEFACTORZ);
	echo "setting ScaleFactorZ to system default = $scaleFactorZ\n";
} else $scaleFactorZ = floatval($scaleFactorZ);

echo "SFZ = $scaleFactorZ \n";
$outputString = $outputString .  "scaleFactorZ = $scaleFactorZ \n";



//	T OFFSETS: $newOffsets = new Point( $TxOffset, $TyOffset, $TzOffset );

$TxOffset = htmlspecialchars($_GET["TOX"]);  // TOX is the TxOffset 
if ($TxOffset == NULL) {
	$TxOffset 	=  trim($manifestXML -> ZValues -> TOX);
	echo "TOX in Manifest.xml = $TxOffset\n";
}	else {
	echo "passed in TOX = $TxOffset \n";
}
if (!is_numeric($TxOffset)){
	$TxOffset 	=  floatval(DEFAULTTXOFFSET);
} else $TxOffset = floatval($TxOffset);

echo "TOX = $TxOffset \n";
$outputString = $outputString .  "TxOffset = $TxOffset \n";

$TyOffset = htmlspecialchars($_GET["TOY"]);  // TOY is the TyOffset 
if ($TyOffset == NULL) {
	$TyOffset 	=  trim($manifestXML -> ZValues -> TOY);
	echo "TOY in Manifest.xml = $TyOffset\n";
}	else {
	echo "passed in TOY = $TyOffset \n";
}
if (!is_numeric($TyOffset)){
	$TyOffset 	=  floatval(DEFAULTTYOFFSET);
} else $TyOffset = floatval($TyOffset);

echo "TOY = $TyOffset \n";
$outputString = $outputString .  "TyOffset = $TyOffset \n";


$TzOffset = htmlspecialchars($_GET["TOZ"]);  // TOZ is the TzOffset 
if ($TzOffset == NULL) {
	$TzOffset 	=  trim($manifestXML -> ZValues -> TOZ);
	echo "TOZ in Manifest.xml = $TzOffset\n";
}	else {
	echo "passed in TOZ = $TzOffset \n";
}
if (!is_numeric($TzOffset)){
	$TzOffset 	=  floatval(DEFAULTTZOFFSET);
} else $TzOffset = floatval($TzOffset);

echo "TOZ = $TzOffset \n";
$outputString = $outputString .  "TzOffset = $TzOffset \n";



//	$newAngles =  new Point( $AnglexOffset, $AngleyOffset, $AnglezOffset );

$AnglexOffset = htmlspecialchars($_GET["AOX"]);  // AOX is the AnglexOffset 
if ($AnglexOffset == NULL) {
	$AnglexOffset 	=  trim($manifestXML -> ZValues -> AOX);
	echo "TOX in Manifest.xml = $AnglexOffset\n";
}	else {
	echo "passed in AOX = $AnglexOffset \n";
}
if (!is_numeric($AnglexOffset)){
	$AnglexOffset 	=  floatval(DEFAULTAngleXOFFSET);
} else $AnglexOffset = floatval($AnglexOffset);

echo "AOX = $AnglexOffset \n";
$outputString = $outputString .  "AnglexOffset = $AnglexOffset \n";

$AngleyOffset = htmlspecialchars($_GET["AOY"]);  // AOY is the AngleyOffset 
if ($AngleyOffset == NULL) {
	$AngleyOffset 	=  trim($manifestXML -> ZValues -> TOY);
	echo "AOY in Manifest.xml = $AngleyOffset\n";
}	else {
	echo "passed in AOY = $AngleyOffset \n";
}
if (!is_numeric($AngleyOffset)){
	$AngleyOffset 	=  floatval(DEFAULTAngleYOFFSET);
} else $AngleyOffset = floatval($AngleyOffset);

echo "AOY = $AngleyOffset \n";
$outputString = $outputString .  "AngleyOffset = $AngleyOffset \n";


$AnglezOffset = htmlspecialchars($_GET["AOZ"]);  // AOZ is the TzOffset 
if ($AnglezOffset == NULL) {
	$AnglezOffset 	=  trim($manifestXML -> ZValues -> AOZ);
	echo "AOZ in Manifest.xml = $AnglezOffset\n";
}	else {
	echo "passed in AOZ = $AnglezOffset \n";
}
if (!is_numeric($AnglezOffset)){
	$AnglezOffset 	=  floatval(DEFAULTAngleZOFFSET);
} else $AnglezOffset = floatval($AnglezOffset);

echo "AOZ = $AnglezOffset \n";
$outputString = $outputString .  "AnglezOffset = $AnglezOffset \n";






// END PROCESS ZString IN MANIFEST.XML --------------------------------------------


// PROCESS ORDER_DETAILS IN MANIFEST.XML --------------------------------------------

// get the <order_details> fields from the Manifest file into an XML string.
$order_details 	= 	$manifestXML -> order_details;
$order_detailsXMLString =  $manifestXML -> order_details ->asXML();
// extract the from, to, subject, referenceID, body and attachments from <order_details>

$to = htmlspecialchars($_GET["to"]);  // TO (SENDER)(DESTINATION, RECIPIENT)
if ($to == NULL) {
	$to 	=  $manifestXML -> order_details -> to;
	echo "to in Manifest.xml = $to\n";
	if ($to == NULL) $to = RECIPIENT;  // default sender email
} else {
	echo "passed in to = $to \n";
	$outputString = $outputString .  "passed in to = $to \n";
}	
echo "to = $to \n";
$outputString = $outputString .  "to = $to \n";

$from = htmlspecialchars($_GET["from"]);  // FROM SENDER
if ($from == NULL) {
	$from =	$manifestXML -> order_details -> from;
	echo "from in Manifest.xml = $from\n";
	if ($from == NULL) $from = SENDER;  // default recipient email
} else {
	echo "passed in from = $from \n";
	$outputString = $outputString .  "passed in from = $from \n";
}
echo "from = $from \n";
$outputString = $outputString .  "from = $from \n";

$subject = htmlspecialchars($_GET["subject"]);
if ($subject == NULL) {
	$subject 	=	$manifestXML ->  order_details -> subject;
	echo "subject in Manifest.xml = $subject\n";
	if ($subject == NULL) $subject = SUBJECT;  // default Subject
} else {
	echo "passed in subject = $subject \n";
	$outputString = $outputString .  "passed in subject = $subject \n";
}
echo "subject = $subject \n";
$outputString = $outputString .  "subject = $subject \n";

$referenceID = htmlspecialchars($_GET["referenceID"]);
if ($referenceID == NULL) {
	$referenceID 	=	$manifestXML ->  order_details -> referenceID;
	echo "referenceID in Manifest.xml = $referenceID\n";
	if ($referenceID == NULL) $referenceID = referenceID;  // default Subject
} else  {
	echo "passed in referenceID = $referenceID \n";
	$outputString = $outputString .  "passed in subject = $referenceID \n";
}
echo "referenceID = $referenceID \n";
$outputString = $outputString .  "referenceID = $referenceID \n";

$body = htmlspecialchars($_GET["body"]);
if ($body == NULL) {
	$body 	=	$manifestXML ->  order_details -> body;
	echo "body in Manifest.xml = $body\n";
	if ($body == NULL) $subject = BODY;  // default Subject
} else {
	echo "passed in body = $body \n";
	$outputString = $outputString .  "passed in body = $body \n";
}
echo "body = $body \n";
$outputString = $outputString .  "body = $body \n";

$attachmentsString = htmlspecialchars($_GET["attachments"]);
if ($attachmentsString == NULL) {
	$attachments =	$manifestXML -> attachments  ;
	
	$filenames = $manifestXML -> order_details -> attachments -> filename; 
	$attachmentsXMLString =  $manifestXML -> attachments ->asXML();
	echo "ATTACHMENTS XML STRING = $attachmentsXMLString\n";
	
	$numberOfAttachmentFiles =  count($filenames);
	echo "Number of attachment files = $numberOfAttachmentFiles\n";
	$outputString = $outputString . "Number of attachment files = $numberOfAttachmentFiles\n";
	
	$files =array () ;
	$attachmentsTextString = "";
	for ($i = 0;  $i < $numberOfAttachmentFiles; $i++ ) {
		$currentFilename = $filenames[$i];
		if ( $attachmentsTextString == "") $attachmentsTextString =  $currentFilename;
     	else $attachmentsTextString = $attachmentsTextString . "," . $currentFilename;		
	}
    
	echo "ATTACHMENTS TEXT STRING = $attachmentsTextString\n";
	 
	 // If no attachments in Manifest or passed parameters there are no attachments.
	 // there are no default attachments
	
} else {

	echo "passed in attachmentsString = $attachmentsString \n";
	$outputString = $outputString .  "passed in attachmentsString = $attachmentsString \n";
	$attachmentsArray = explode(",",$attachmentsString);
	$attachmentsXMLString = "<attachments>\n";
	$numberOfAttachments = count($attachmentsArray);
	for ($i = 0;  $i < $numberOfAttachments ; $i++ ) {
		$attachmentsXMLString = $attachmentsXMLString . "<filename>$i</$filename>\n";
	}
	$attachmentsXMLString = $attachmentsXMLString ."<\attachments>\n";
	
}	
echo "attachmentsTextString = $attachmentsTextString \n";
$outputString = $outputString .  "attachmentsTextString = $attachmentsTextString \n";
echo "attachmentsXMLString = $attachmentsXMLString \n";
$outputString = $outputString .  "attachmentsXMLString = $attachmentsXMLString \n";

// END PROCESS ORDER_DETAILS IN MANIFEST.XML --------------------------------------------


// PROCESS MODELLING_OPTIONS IN MANIFEST.XML --------------------------------------------

// get the <modelling_options> fields from the Manifest file into an XML string.
$modelling_options 	= 	$manifestXML -> modelling_options;
$modelling_optionsXMLString =  $manifestXML -> modelling_options ->asXML();

$template3DP = trim(htmlspecialchars($_GET["template3DP"]));
echo "****** passed in template3DP = $template3DP *******\n"; 
if (($template3DP == "") | ($template3DP == NULL)) {
	$template3DP 	= 	trim($manifestXML -> modelling_options -> template3DP);
	echo "template3DP in Manifest.xml = $template3DP\n";
} else echo "passed in template3DP = $template3DP \n";
if (($template3DP == "") | ($template3DP == NULL))  {
	$template3DP = NULL;
	 echo "No template provided, will automatically generate one!\n";
	 $outputString = $outputString . "No template provided, will automatically generate one!\n";
} else {
	echo "template3DP = $template3DP\n";
	$outputString = $outputString . "template3DP = $template3DP \n";	
} 	

$meshlevel = htmlspecialchars($_GET["meshlevel"]);
if ((!is_int($meshlevel)) or ($meshlevel < 7)  or ($meshlevel > 9)){
	$meshlevel 		=	$manifestXML -> modelling_options -> meshlevel;
	echo "meshlevel in Manifest.xml = $meshlevel\n";
	if ((!is_int($meshlevel)) or ($meshlevel < 7)  or ($meshlevel > 9)) $meshlevel = MeshQuality;  // default meshlevel
} else {
	echo "passed in meshlevel = $meshlevel \n";
	$outputString = $outputString .  "passed in meshlevel = $meshlevel \n";
}
echo "meshlevel = $meshlevel \n";
$outputString = $outputString .  "meshlevel = $meshlevel \n";

$boundingBox = htmlspecialchars($_GET["boundingBox"]);
if ($boundingBox == NULL) {
	$boundingBox 	=	$manifestXML -> modelling_options -> boundingBox;
	echo "boundingBox in Manifest.xml = $boundingBox\n";
	if ($boundingBox == NULL) $boundingBox = MESHBBOX;  // default bounding box	
} else echo "passed in boundingBox = $boundingBox \n";
echo "boundingBox = $boundingBox \n";
$outputString = $outputString . "boundingBox = $boundingBox \n";

$doModelScaling = htmlspecialchars($_GET["doModelScaling"]);
if ($doModelScaling == NULL) {
	$doModelScaling 		=	$manifestXML -> modelling_options -> doModelScaling;
	echo "doModelScaling in Manifest.xml = $doModelScaling\n";
	if ($doModelScaling == NULL) $do_ModelScaling = DO_MODELSCALING;  // default doModelScaling
	echo "doModelScaling = $doModelScaling \n";
	$outputString = $outputString . "doModelScaling = $doModelScaling \n";
} else {
	echo "passed in doModelScaling = $doModelScaling \n";
	$outputString = $outputString . "passed in doModelScaling = $doModelScaling \n";
}
if (strtolower($doModelScaling) == "false" or strtolower($doModelScaling) == "no" 
	or strtolower($doModelScaling) == "n" or strtolower($doModelScaling) == "0") 
	$doModelScaling = false;
else $doModelScaling = true;   // DEFAULT FOR doModelScaling is true
echo "doModelScaling = $doModelScaling \n";
$outputString = $outputString . "doModelScaling = $doModelScaling \n";

$doModelCropping = htmlspecialchars($_GET["doModelCropping"]);
if ($doModelCropping == NULL) {
	$doModelCropping 	=	$manifestXML -> modelling_options -> doModelCropping;
	echo "doModelCropping in Manifest.xml = $doModelCropping\n";
	if ($doModelCropping == NULL) $doModelCropping = DO_MODELCROPPING;  // default doModelCropping
	echo "doModelCropping = $doModelCropping \n";
	$outputString = $outputString . "doModelCropping = $doModelCropping \n";
} else {
	echo "passed in doModelCropping = $doModelCropping \n";
	$outputString = $outputString .  "passed in doModelCropping = $doModelCropping \n";
}
if (strtolower($doModelCropping) == "false" or strtolower($doModelCropping) == "no"
	or strtolower($doModelCropping) == "n" or strtolower($doModelCropping) == "0") 
	$doModelCropping = false;
else $doModelCropping = true;  // DEFAULT FOR doModelCropping is true
echo "doModelCropping = $doModelCropping \n";
$outputString = $outputString . "doModelCropping = $doModelCropping \n";

$doImageCropping = htmlspecialchars($_GET["doImageCropping"]);
if ($doImageCropping == NULL) {
	$doImageCropping =	$manifestXML -> modelling_options -> doImageCropping;
	echo "doImageCropping in Manifest.xml = $doImageCropping\n";
	if ($doImageCropping == NULL) $doImageCropping = DOIMAGECROPPING;  // default DOIMAGECROPPING
} else {
	echo "passed in doImageCropping = $doImageCropping \n";
	$outputString = $outputString .  "passed in doImageCropping = $doImageCropping \n";
}
if (strtolower($doImageCropping) == "true" or strtolower($doImageCropping) == "yes"
	or strtolower($doImageCropping) == "y" or strtolower($doImageCropping) == "1n") 
	$doImageCropping = true;
	
else  $doImageCropping = false; // DEFAULT FOR doImageCropping is false
echo "doImageCropping = $doImageCropping \n";
$outputString = $outputString .  "doImageCropping = $doImageCropping \n";

$imageCroppingRectangle = htmlspecialchars($_GET["imageCroppingRectangle"]);
if ($imageCroppingRectangle == NULL) {
	$imageCroppingRectangle =	$manifestXML -> modelling_options -> imageCroppingRectangle;
	echo "imageCroppingRectangle in Manifest.xml = $imageCroppingRectangle\n";
	if ($imageCroppingRectangle == NULL) $imageCroppingRectangle = IMAGECROPPINGRECTANGLE;  // default imageCroppingRectangle
} else {
	echo "passed in imageCroppingRectangle = $imageCroppingRectangle \n";
	$outputString = $outputString .  "passed in imageCroppingRectangle = $imageCroppingRectangle \n";
}
echo "imageCroppingRectangle = $imageCroppingRectangle \n";
$outputString = $outputString .  "imageCroppingRectangle = $imageCroppingRectangle \n";

//END PROCESS MODELLING_OPTIONS IN MANIFEST.XML -----------------------------------------


// PROCESS IMAGESET IN MANIFEST.XML --------------------------------------------

// MANIFEST: get the <imageset> fields from the Manifest file into an XML string.
$imageset 	= 	$manifestXML -> imageset;
$imagesetXMLString =  $manifestXML -> imageset ->asXML();

// Diagnostics are not used right now.  They are just saved for troubleshooting.

// END PROCESS DIAGNOSTICS IN MANIFEST.XML --------------------------------------------







	
// ----------------- END OF PASSED PARAMETERS --------------------------------------

// --------  IF ERROR NOTIFY SENDER THAT SCAN HAS FAILED  --------------------------
if ($errorflag) {
		 $to = $to;
		 $headers = "From: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com";
		 $subject = "SCAN MODELING FAILED: Scan: $scanID -- No Manifest.xml file";
		 $message = "A SoundFit SugarCube scan has failed."
			."\n\nSettings are:\n\n " 
			. print_r($_GET, true)
			. "\n\nUploaded folder: " . SCAN_DIR_URL . $scanID
			. "\n\n $outputString \n" ;
		 mail($to,$subject,$message,$headers,  "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com");
		 exit('Failed to open $manifestXMLFile\n');
}	 
// --------  END OF IF ERROR NOTIFY SENDER THAT JOB HAS FAILED  -------------------	  

// -------------------- DEFINE CALLBACK PARAMETERS ----------------------------------
// We define the callback emails and URL to call when the photoscene processing completes:
$callback_email = 'email://' . $from;
// use this if you want Autodesk to send a callback email
// $callback =  $callback_email . "," . CALLBACKPGM. $scanID  ; 
// Use this if you don't want the callback emails
$callback =  CALLBACKPGM. $scanID  ;

echo "callback string =  ($callback) \n";
$outputString = $outputString .  "callback string =  ($callback) \n";
// -------------------- END OF DEFINE CALLBACK PARAMETERS ------------------------------



// --------------------------- CREATE A GALLERY TEMPLATE INDEX PAGE ---------------------

$source = INDEX_FILE_TEMPLATE;
$galleryFilePath = $uploadDir."/index.php";

$shellcmd =  "/bin/cp $source $galleryFilePath";
$answer = `$shellcmd`;
echo "Copying gallery index page to $galleryFilePath.   Returns ($answer)\n";


// -----------------SAVE A COPY OF THE INCOMING FOLDER TO OLD DIR -----------------------
//					COPYING IS NOW TURNED OFF
//
// $source = $uploadDir;
// $dest= OLD_DIR.$scanID;
//
// $shellcmd =  "/bin/cp -r $source $dest";
// $answer = `$shellcmd`;
// echo "Copying incoming new folder to old.   Returns ($answer)\n";
//
// -----------------END OF SAVE A COPY OF THE INCOMING FOLDER TO OLD DIR ----------------


// -----------------MOVE THE NEW FOLDER TO RECEIVED ------------------------------------ 

//  echo "DISABLE RENAME OF scan_directory FOR TESTING *************************************\n";
$scan_directory = SCAN_DIR_ROOT.$scanID;  
rename($uploadDir, SCAN_DIR_ROOT.$scanID); 
// $scan_directory = $uploadDir; //  Set scan dir this way for testing

echo "\nMOVED " .  $uploadDir . "/" . $scanID . "\nTO\n" . SCAN_DIR_ROOT.$scanID. "\n\n";
$outputString = $outputString .  "\n MOVED " .  $uploadDir . "/" . $scanID . "\n TO\n" . SCAN_DIR_ROOT.$scanID. "\n\n";

// -----------------MOVE THE NEW FOLDER TO RECEIVED ------------------------------------ 



// ------------------------ RECAP OAUTH NEGOTIATION -------------------------------
// The OAuth negotiation  procedure is complex, but this code does it: 

//- Prepare the PHP OAuth for consuming Autodesk Oxygen service
$options =array (
	'consumer_key' => ConsumerKey,
	'consumer_secret' => ConsumerSecret,
	'server_uri' => BaseUrl,
	'request_token_uri' => BaseUrl . 'OAuth/RequestToken',
	'authorize_uri' => BaseUrl . 'OAuth/Authorize',
	'access_token_uri' => BaseUrl . 'OAuth/AccessToken',
) ;

OAuthStore::instance ('Session', $options) ;

// Get the OAuth token stored in the local directory as:  access_token.txt
$fname =realpath (OAUTH_ACCESS_TOKEN) ;
$access = unserialize (file_get_contents ($fname)) ;

// Create a client and provide a base URL
$client =new Client (ReCapApiUrl) ;
//  This program uses Guzzle to access OAuth.
//  See http://guzzlephp.org/guide/plugins.html for how Guzzle works.
//  The Guzzle Oauth plugin will put the Oauth signature in the HTML header automatically
$oauthClient =new OauthPlugin (array (
	'consumer_key'    => ConsumerKey,	  // defined by Autodesk and unique to SoundFit
	'consumer_secret' => ConsumerSecret,  // defined by Autodesk and unique to SoundFit
	'token'           => $access ['oauth_token'],        // the access_token OAuth uses
	'token_secret'    => $access ['oauth_token_secret'], // the access_token_secret
)) ;
$client->addSubscriber ($oauthClient) ;

echo "\nConnecting to ReCap Server with OAuth.\n";
$outputString = $outputString .  "\nConnecting to ReCap Server with OAuth.\n";

//  Requesting the ReCap service/date to start and check our connection/authentification
$request = $client->get ('service/date') ;
$request->getQuery()->clear() ; // Not needed as this is a new request object, 
								// but as we are going to use merge(), it is safer
$request->getQuery()->merge( array(
	'clientID' => ReCapClientID, 
	'timestamp' => time (),
));
$response = $request->send(); // Program must send a request for the transfer to occur
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n";

$xml = $response->xml();
echo "ReCap Server date/Time: {$xml->date}\n";
$outputString = $outputString .  "ReCap Server date/Time: {$xml->date}\n";

// Requesting the ReCap version
$request = $client->get ('version');
$request->getQuery()->clear() ; // Not needed as this is a new request object, 
								// but as we are going to use merge(), it is safer
$request->getQuery()->merge( array(
	'clientID' => ReCapClientID, 
	'timestamp' => time (),
));
$response = $request->send();
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n";

$xml = $response->xml();
echo "ReCap Version: {$xml->version}\n" ;
$outputString = $outputString .  "ReCap Version: {$xml->version}\n" ;

// -------------------- END OF RECAP OAUTH NEGOTIATION ----------------------------


// -------------------- CREATE PHOTOSCENE ----------------------------------------
echo "\nCreating the photoscene...\n";
$outputString = $outputString .  "\nCreating the photoscene...\n";

// Now we are ready to create our photoscene (3D model).
// Note that we pass the scanID parameter as part of the photoscene name, so that we can  
// match the 3D model created from this scan back to the original scan files.  
// Unfortunately also note that we don't know the PhotoSceneID until after we create it!
// So we will save this mapping of scanID to PhotosceneID in a map file (see below).
// When the 3D model is finally complete and the callback program runs it will be 
// passed the ScanID as a get parameter.   We can then look up the map file and 
// find out which photosceneID has the 3D model we want. 

// Creating the photoscene, setting the scenename, callbacks, meshquality and format:
$request =$client->post ('photoscene') ;
$timestamp = time ();

echo "doModelCropping =  $doModelCropping\n";
$outputString = $outputString .  "do_ModelCropping =  $do_ModelCropping\n";
if ( ($doModelCropping == true) AND ($doModelScaling == true) ) {
	// if we doModelScaling is true we MUST submit a template file
	// and we do NOT want to set: 
	// StitchingCreateInputFile=1

	$request->addPostFields (array (
		'scenename' => $scanID . "-" . $timestamp,  // Unique scene name
	    'callback' => $callback,           // set in global variables above  
		'meshquality' => $meshlevel,       // set in global variables or set as a parm
		'meshbbox' => $boundingBox,        // passed in as a parameter
		'metadata_name[0]'=>'StitchingQuality',
		'metadata_value[0]'=>'2',
		'format' => 'obj',				   // create OBJ files
		'clientID' => ReCapClientID, 
		'timestamp' => $timestamp,
	)) ;
	echo "Automatic CROPPING is ON\n";
	$outputString = $outputString .  "Automatic CROPPING is ON\n";
} else if ($doModelScaling == false) { 
	// if there doModelScaling is false, there will be no template, and cropping is not possible
	// we also have to set StitchingCreateInputFile=1 and StitchingQuality=1 per Autodesk
	$request->addPostFields (array (
		'scenename' => $scanID . "-" . $timestamp,  // Unique scene name
	    'callback' => $callback,           // set in global variables above  
		'meshquality' => $meshlevel,      // set in global variables above
	    // 'meshbbox' => $boundingBox,        // Don't set meshbbox if we aren't cropping
        // when there is no template, this must be set
		'metadata_name[0]'=>'StitchingCreateInputFile',
		'metadata_value[0]'=>'1',
		'metadata_name[1]'=>'StitchingQuality',
		'metadata_value[1]'=>'2',
		'format' => 'obj',				   // create OBJ files
		'clientID' => ReCapClientID, 
		'timestamp' => $timestamp,
	)) ;
	echo "Automatic SCALING is OFF\n";
	$outputString = $outputString .  "Automatic SCALING is OFF\n";
} else {	// $doModelScaling is true, but $doModelCropping is false
	$request->addPostFields (array (
		'scenename' => $scanID . "-" . $timestamp,  // Unique scene name
	    'callback' => $callback,           // set in global variables above  
		'meshquality' => $meshlevel,      // set in global variables above
	//	'meshbbox' => $boundingBox,        // Don't set meshbbox if we aren't cropping
		'metadata_name[0]'=>'StitchingQuality',
		'metadata_value[0]'=>'2',
		'format' => 'obj',				   // create OBJ files
		'clientID' => ReCapClientID, 
		'timestamp' => $timestamp,
	)) ;
	echo "Automatic CROPPING is OFF\n";
	$outputString = $outputString .  "Automatic CROPPING is OFF\n";
}

echo "ReCap Client ID: ".ReCapClientID."}\n";
$outputString = $outputString . "ReCap Client ID: ".ReCapClientID."}\n";
$response =$request->send();
// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;

$xml = $response->xml() ;
$photoSceneID =$xml->Photoscene->photosceneid ;
echo "My ReCap PhotoSceneID: {$photoSceneID}\n" ;
$outputString = $outputString . "My ReCap PhotoSceneID: {$photoSceneID}\n" ;

if ($xml->Error) { // if error stop here else continue
	$code =$xml->Error->code ;
	echo "My ReCap create photoscene error code: {$code}\n" ;
	$outputString = $outputString . "My ReCap create photoscene error code: {$code}\n" ;
	$msg  =$xml->Error->msg ;
	echo "My ReCap create photoscene error message: {$msg}\n" ;
	$outputString = $outputString . "My ReCap create photoscene error message: {$msg}\n" ; 
	exit($code);
} else { 
	echo "photoscene $photoSceneID accepted by Recap.\n"; 
	$outputString = $outputString . "photoscene $photoSceneID accepted by Recap.\n";
}
// ---------------- END CREATE PHOTOSCENE ----------------------------------------


// ---------------- MAP SCANID TO PHOTOSCENE --------------------------------------
// The callback program will need to know the PhotoSceneID that corresponds
// to the ScanID.  But we didn't know the PhotoSceneID until after submitting
// the model -- so we couldn't pass the PhotoSceneID is a GET string parameter.
// So we save the mapping into a file that the callback program can lookup by ScanID.

$mapfile = MAPDIR . $scanID;  // MAPDIR is the directory where mapping files are kept
echo "mapfile = $mapfile\n";
$outputString = $outputString . "mapfile = $mapfile\n";
file_put_contents (  $mapfile ,  $photoSceneID );

// ---------------- END OF MAP SCANID TO PHOTOSCENE --------------------------------


// ---------------- CREATE ARRAY OF SCAN FILES TO UPLOAD ---------------------------
echo "\nSelecting scan files to upload.\n";
$outputString = $outputString . "\nSelecting scan files to upload.\n";

//  Add files - resource POST /file
//  retrieve a scan folder from the RECEIVED directory
//  put the JPG files from the scan directory into an array

// RECAP API NOTES:
// A file can be associated by multiple way:
// upload: the file is uploaded normally
// reference: from a public URL, you can pass a full public URL as filename, 
// the server will download the file and store it on the ReCap server as a normal image. 
// (ex: http://farm1.staticflickr.com/123/330811704_282ee8a597_z.jpg). 
// URL can be http or ftp based. 
// Useful if the files are already uploaded to a public cloud service. 
// You can also set the permissions to access the files on your own private cloud service.

$imagefiles = $imageset ; 
//print_r($imagefiles);

$badJPGFileErrorFlag = false;
$numberOfJPGFilesWithErrors = 0;

$filenames = $manifestXML -> imageset -> image; 
$numberOfImageFiles =  count($filenames);


echo "Number of image files = $numberOfImageFiles\n";
$outputString = $outputString . "Number of image files = $numberOfImageFiles\n";
echo "testing image files:\n ";

$files =array () ;

for ($i = 0;  $i < $numberOfImageFiles; $i++ ) {
	// echo "filenames[$i] = $filenames[$i]\n";
	$currentFilename = $scan_directory."/".$filenames[$i];
	$im = imagecreatefromjpeg($currentFilename);
	if (!$im) { // $im is false if the image file is bad
		$badJPGFileErrorFlag = true;
		$numberOfJPGFilesWithErrors++;
		echo " $filenames[$i] is not a valid JPG file. \n";
		$outputString = $outputString . "Number of image files = $numberOfImageFiles\n";
	} else {  // good image file
		// put filename into array for ReCap
		// echo " $filenames[$i] is a valid JPG file. \n";
		$imagesize = getimagesize($currentFilename);
		$imagewidth		= $imagesize[0];
		$imageheight 	= $imagesize[1];
		
		// If doImageCropping is true crop the images and save new versions before uploading.
		if ($doImageCropping == true) {
			// E.g. <imageCroppingRectangle>450,340,1310,1190</imageCroppingRectangle>
			$imageCroppingRectangleArray = explode (",", $imageCroppingRectangle);
			if (count ($imageCroppingRectangleArray) == 4 ) { // see if $imageCroppingRectangle  is a 4-tuple:
			
				// it IS a 4-tuple! User has specified a cropping rectangle
				// check to see if they specified percentages
				if(substr($imageCroppingRectangleArray[0], -1) == '%') { // if it is a % calculate relative to image width
					$imageCroppingRectangleArray[0] =  $imagewidth * (substr($imageCroppingRectangleArray[0], 0, -1) /100 );
				} else {  // if it doesn't end in %, assume it is absolute pixels (treat as integer!)
					$imageCroppingRectangleArray[0] = preg_replace('/[^0-9]/','',$imageCroppingRectangleArray[0]);
				}
				if(substr($imageCroppingRectangleArray[1], -1) == '%')  { // if it is a % calculate relative to image height
					$imageCroppingRectangleArray[1] =  $imageheight * (substr($imageCroppingRectangleArray[1], 0, -1) /100 );
				} else {  // if it doesn't end in %, assume it is absolute pixels (treat as integer!)
					$imageCroppingRectangleArray[1] = preg_replace('/[^0-9]/','',$imageCroppingRectangleArray[1]);
				}
				if(substr($imageCroppingRectangleArray[2], -1) == '%')  { // if it is a % calculate relative to image width
					$imageCroppingRectangleArray[2] =  $imagewidth * (substr($imageCroppingRectangleArray[2], 0, -1) /100 );
				} else {  // if it doesn't end in %, assume it is absolute pixels (treat as integer!)
					$imageCroppingRectangleArray[2] = preg_replace('/[^0-9]/','',$imageCroppingRectangleArray[2]);
				}
				if(substr($imageCroppingRectangleArray[3], -1) == '%')  {// if it is a % calculate relative to image height
					$imageCroppingRectangleArray[3] =  $imageheight * (substr($imageCroppingRectangleArray[3], 0, -1) /100 );
				} else {  // if it doesn't end in %, assume it is absolute pixels (treat as integer!)
					$imageCroppingRectangleArray[3] = preg_replace('/[^0-9]/','',$imageCroppingRectangleArray[3]);
				}
	
			}  else { // if user didn't specify a 4-tuple the default action is to crop square with width = height, centered.
				$cropleft 		= ($imagewidth - $imageheight) / 2;
				$croptop		= 0;
				$cropright 		= $imageheight + ($imagewidth - $imageheight) / 2;
				$cropbottom		= $imageheight;
				$imageCroppingRectangleArray = array ($cropleft, $croptop, $cropright, $cropbottom );
			}
	
			$src_dir = $scan_directory;
			$dst_dir = $scan_directory;
			$src_x = $imageCroppingRectangleArray[0]; // Crop Start X position in original image
			$src_y = $imageCroppingRectangleArray[1]; // Crop Srart Y position in original image
			$src_w = $imageCroppingRectangleArray[2]; // $src_x + $dst_w Crop end X position in original image
			$src_h = $imageCroppingRectangleArray[3]; // $src_y + $dst_h Crop end Y position in original image

			$dst_x = 0;   // X-coordinate of destination point. 
			$dst_y = 0;   // Y --coordinate of destination point. 

			$dst_w = $src_w - $src_x; // crop width
			echo  "$dst_w = $dst_w\n";
			$dst_h = $src_h - $src_y; // crop height
			echo  "crop image size = $dst_w = $dst_w, $dst_h \n";

				
			// Get original image
			$src_image = $im; // convert JPG to image resource
			$fileName = $filenames[$i];               

			// Creating an image with true colors having cropped dimensions.( to merge with the original image )
			if (($dst_w == 0)  or  ($dst_h == 0) ) echo "Warning: Can't crop images to height or width = 0!  Image Cropping will not be done.\n";
			else {
				$dst_image = imagecreatetruecolor($dst_w,$dst_h);
				// Cropping 
				imagecopyresampled($dst_image, $src_image, $dst_x, $dst_y, $src_x, $src_y, $dst_w, $dst_h, $src_w, $src_h);
				// Saving 
				imagejpeg($dst_image, $dst_dir.$fileName, 100 ); // write out the cropped image as new jpg at Quality 100
			}
		}
		
		$files["file[$i]"] = "@" . $scan_directory."/".$filenames[$i];
		// echo ".";
		echo  $filenames[$i] ."\t ";
		// echo $i+1 .": " . $filenames[$i] ."\n ";
	}	
}
echo "\n\n";

if ($badJPGFileErrorFlag)  {
	echo "\n\nAll $numberOfImageFiles files in Manifest.xml verified.\n";
	$outputString = $outputString . "\n\nAll $numberOfImageFiles files in Manifest.xml verified.\n";
	
	// --------  NOTIFY to THAT SCAN HAS FAILED  --------------------------
 
	$to = $to;
	$headers = "From: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com";
	$subject = "SCAN MODELING FAILED: Scan: $scanID -- Failure reading JPG files";
	$message = "A SoundFit SugarCube scan has failed."
		."\n\nSettings are:\n\n " 
		. print_r($_GET, true)
		. "\n\nUploaded folder: " . SCAN_DIR_URL . $scanID
		. "\n\n $outputString \n" ;
	 mail($to,$subject,$message,$headers,  "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com");
	 exit('Failed reading files in $manifestXMLFile\n');
	 
	  // --------  END OF NOTIFY to THAT JOB HAS FAILED  -------------------
	 
}  // end of $badJPGFileErrorFlag processing
	

	 
// ----- END Retrieve and test image files listed in Manifest.XML  -------------------

	
// ----------- END OF CREATE ARRAY OF SCAN FILES TO UPLOAD --------------------------
	
	
	
			
// ------------------------ UPLOAD FILES ---------------------------------------------
echo "\nUploading scan files to the photoscene.\n";

define("IMAGEUPLOADLIMIT", 41);

// NOTE:  Autodesk only lets us upload 41 images at a time.  
// If there are more than 41 images we need to run this loop additional times
// on chunks of the array 41 images at a time.

$numberOfFileUploadPasses = ceil($numberOfImageFiles / IMAGEUPLOADLIMIT);
$remainderImages = $numberOfImageFiles % IMAGEUPLOADLIMIT;

echo "numberOfFileUploadPasses  = $numberOfFileUploadPasses \n";
echo "remainderImages  = $remainderImages \n";

$fileschunks = array_chunk($files, IMAGEUPLOADLIMIT);
echo "fileschunks = "; print_r($fileschunks);
$totalImagesUploaded = 0;

$xmlResponseArray = array();

$filecount = 0;
for ($uploadPassIndex = 0; $uploadPassIndex < $numberOfFileUploadPasses; $uploadPassIndex++ ) { 
	echo "uploadPassIndex = $uploadPassIndex\n";
	$nb = count($fileschunks[$uploadPassIndex]); 
	$totalImagesUploaded = $totalImagesUploaded + $nb;
	
	// Create the ReCap Request to add the scan files
	// This POST call is unusual, since we are going to use POST to upload files to ReCap,  
	// but we also need to pass the ClientID and Timestamp for OAuth validation 
	// as GET variables.

	$timestamp = time ();
	$url = "?clientID=" . ReCapClientID 
		 . "&timestamp=" . $timestamp 
		 . "&photosceneid={$photoSceneID}"
		 . "&type=image" ;
	$request =$client->post ("file" . $url) ; 
	$request->addPostFields (array ( 
		'photosceneid' 	=> $photoSceneID, 
		'type' 			=> 'image',
		'clientID' 		=> ReCapClientID, 
		'timestamp' 	=> $timestamp
	)) ;
	
	echo "fileschunks[$uploadPassIndex] = "; print_r( $fileschunks[$uploadPassIndex]);
	//$request->addPostFiles ($files) ; // This adds the files to upload
	$request->addPostFiles ($fileschunks[$uploadPassIndex]) ; // This adds the files to upload this pass
	
	$response =$request->send () ;

	// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;

	$xmlResponseArray[$uploadPassIndex]=$response->xml () ;
	echo "xmlResponseArray[$uploadPassIndex] ="; 
	echo var_dump($xmlResponseArray[$uploadPassIndex]); echo "\n\n"; 
	if ($xml->Error) { // if error stop here else continue
		$code =$xml->Error->code ;
		echo "My ReCap File Upload error code: {$code}\n" ;
		$msg  =$xml->Error->msg ;
		echo "My ReCap File Upload error message: {$msg}\n" ;
		exit($code);
	} else { echo "$nb scan files successfully uploaded and accepted by Recap.\n"; }

	// Whether or not we are doing scaling, we still calculate a template (camera positions)
	// that we can use to determine model quality: 

	// ---------------------- GET FILE HASH CODES FROM RETURN ---------------------------
	
	// The template file has to be edited to have the same hash codes and file names
	// that were returned for the image files we just uploaded.  

    $filesobj = $xmlResponseArray[$uploadPassIndex] ->Files->file;  

	echo "Assigning hashcodes to scan files in template. \n";
	foreach ($filesobj as $filesobjkey => $filesobjvalue) 
	{
		//print_r($filesobjvalue); echo "\n";  
		$filename = $filesobjvalue->filename;
		$filehashname = $filesobjvalue->fileid;
		// echo "$filecount: $filehashname becomes "; // uncomment to check renaming

		// The returned hash codes are in a slightly different form that
		// the edited hash codes we have to save.  In particular, we need to 
		// insert "/" after every 4 chars for 6 times.  

		// This code breaks the hash code into 6 four character segments
		$filehasharrray = array( 
		substr($filehashname, 0, 4),
		substr($filehashname, 4, 4),
		substr($filehashname, 8, 4),
		substr($filehashname, 12, 4),
		substr($filehashname, 16, 4),
		substr($filehashname, 20, 4),
		substr($filehashname, 24 )
		);

		// this code puts the parts back together with slashes in joining them
		$hashIDwithSlashes = implode("/", $filehasharrray);
		// echo "$filecount: $filename - $hashIDwithSlashes \n"; // show the mapping
		$filehashID[$filecount] = $hashIDwithSlashes ;

		echo " filehashID[$filecount] = $hashIDwithSlashes \n";  // uncomment to check renaming
		// $filehashID[$filecount] = $filehashname ;
		$filenames[$filecount] = $filename ;
		$filecount++;
	}

	// --------------- END OF GET FILE HASH CODES FROM RETURN ----------------------------
	
}

echo "filehashID = \n";
echo var_dump($filehashID);
echo "\n\n";

echo "Total of $totalImagesUploaded scan files successfully uploaded and accepted by Recap.\n"; 


// ---------------- END OF UPLOAD FILES ---------------------------------------------

//  -------------- EDIT THE AUTOMATED TEMPLATE USING THESE HASH CODES  -------------------------
//  The template file gives ReCap known camera positions etc.
//  that ReCap uses to ensure that the 3D model is correctly scaled.
//  We generate the values for the template based on information in the manifest.xml file,
//  and then use the hash codes just returned from rReCap above 
//  so it can use the known camera positions, etc. with the files just uploaded.

// set constants used in creating templates here, until we can get them from manifest file

$armRadius = 104.0;		// in mm
$rotationOffset = 0.0; 	// in degrees
$pivotHeight = 0.0;  	// in mm
//$fieldOfView = 49;		// in degrees (79 in datasheet)
//$fieldOfView = 50.6924;	// in degrees  - based on RKA template
// $fieldOfView is now set using data from ZString in manifest.
	
// Get Global values from Manifest or use defaults

$boundingBox 	=	$manifestXML -> modelling_options -> boundingBox;
if ($boundingBox == '') $boundingBox = '-120,-120,-10,120,120,120';	 //default
$meshLevel		=	$manifestXML -> modelling_options -> meshlevel;
if ($meshLevel == '') $meshLevel = '7';	 //default
$imagewxh		=	$manifestXML -> diagnostics -> imageResolution;
$pieces 		=   explode("x", $imagewxh);
$imageWidth 	=   $pieces[0]; // piece1	
if ($imageWidth == '') $imageWidth = '2048';	 //default 3MP
$imageHeight	=   $pieces[1]; // piece2
if ($imageHeight == '') $imageHeight = '1536';	 //default 3MP

// extract the filenames, elevations and rotations.

$filenames = $manifestXML -> imageset -> image;

$numberOfImageFiles =  count($filenames);
echo "Number of image files = $numberOfImageFiles\n";
$outputString = $outputString . "Number of image files = $numberOfImageFiles\n";
$n = 0;
foreach ($manifestXML -> imageset -> image as $imageObject) {
	$elevation[$n] =  floatval($imageObject['elevation'][0]);
	$rotation[$n] =   floatval($imageObject['rotation'][0]);
	echo "rotation[$n] = $rotation[$n]\n";
	$n++;
}

// functions for calculating cartesian coordinates for camera position relative to motors 
// from spherical coordinates.
// used in T (x, y,  z) tag in Template
//

function polar2CartesianX($armRadius, $theRotation, $rotationOffset, $theElevation) {
	$rotation = floatval($theRotation +  $rotationOffset );
	$elevation = floatval($theElevation);
	$x = $armRadius*sin(deg2rad($rotation))*sin(deg2rad ($elevation));
	return $x;
}

function polar2CartesianY($armRadius, $theRotation, $rotationOffset, $theElevation) { 
	$rotation = floatval($theRotation +  $rotationOffset );
	$elevation = floatval($theElevation);
	$y = -$armRadius*cos(deg2rad($rotation))*sin(deg2rad ($elevation));
	return $y;
}

function polar2CartesianZ($armRadius, $theElevation, $pivotHeight ) {
	$elevation = floatval($theElevation);
	$z = $armRadius*cos(deg2rad($elevation))+$pivotHeight;
	return $z;
}

//  Create the RZML Template object:
//  <   ?x m l   v e r s i o n = " 1 . 0 "   e n c o d i n g = " U T F - 1 6 "   s t a n d a l o n e = " y e s "?   > 
//  < R Z M L   v = " 1 . 4 . 9 "   a p p = " P r o j e c t   P h o t o f l y :   3 . 0 . 0 . 4 1 2 "   i d = " M 1 I U R A 1 A M K L G l K x 8 Q 3 m W I N 1 0 8 d g "  > 

$RZML = new SimpleXMLElement('<RZML></RZML>');
$RZML->addAttribute('v', '1.4.9');
$RZML->addAttribute('app', 'Project Photofly: 3.0.0.412');  
$RZML->addAttribute('id', 'M1IURA1AMKLFlKx8Q3mWIN108dg' ); 


// add the CINF tag:
//	 < C I N F   i = " 1 "   s w = " 2 0 4 8 "   s h = " 1 5 3 6 "   f o v s = " c"   f o v x = " 4 9 . 3 2 8 4 4 4 3 5 7 3 5 2 3 "   d s = " c"   p p s = " c "   d i s t o T y p e = " d i s t o 3 5 d " / > 
//                   ^         ^                   ^
//               $imageWidth  $imageHeight      $fieldOfView

$CINF = $RZML->addChild('CINF');
$CINF->addAttribute('i', '1');
$CINF->addAttribute('sw', "$imageWidth");
$CINF->addAttribute('sh', "$imageHeight");
$CINF->addAttribute('fovs', "$FOVS");   // c, k or s
$CINF->addAttribute('fovx', "$fieldOfView");
$CINF->addAttribute('ds', 'c');
$CINF->addAttribute('pps', "$PPS");
$CINF->addAttribute( 'distoType' , 'disto35d' );

// write out <SHOT>s for each image:
// for ($i = 0; $i < 3; $i++)   // use for testing
for ($i = 0; $i < $numberOfImageFiles; $i++) // use for production
{
	//	 < S H O T   i = " 1 "   n = " s n a p s h o t - 2 1 3 3 0 7 0 4 . j p g "   c i = " 1 "   w = " 2 0 4 8 "   h = " 1 5 3 6 " > 
	//           ^     ^                                  ^       ^
	//           $i     $filenames[$i]                $imageWidth  $imageHeight
	$SHOT = $RZML->addChild('SHOT');
	$j = $i + 1;
	$SHOT->addAttribute('i', "$j");
	$SHOT->addAttribute('n', "$filenames[$i]");
	$SHOT->addAttribute('ci',"1");
	$SHOT->addAttribute('w', "$imageWidth");
	$SHOT->addAttribute('h', "$imageHeight");

	//	 < I P L N   i m g = " s n a p s h o t - 2 1 3 3 0 7 0 4 . j p g "   h a s h = " P 4 J K / i t c O / e B 4 G / t f q T / i F j D / R f B p / l m Y = - 1 2 0 9 0 8 0 - 1 0 0 0 0 0 0 0 0 " / > 
	//             ^                             ^
	//			$filenames[$i]                  $filehashID[$i]
	$IPLN = $SHOT->addChild('IPLN');
	$IPLN->addAttribute('img', "$filenames[$i]");
	$IPLN->addAttribute('hash', "$filehashID[$i]"); 


	//	 < C F R M   f o v x = " 4 9 . 3 2 8 4 4 4 3 5 7 3 5 2 3 " > 
	//               ^
	//            $fieldOfView
	$CFRM = $SHOT->addChild('CFRM');
	$CFRM->addAttribute('fovx', "$fieldOfView");

	//	<  T   x = " - 1 . 1 5 9 0 7 8 5 0 9 8 9 8 8 7 "   y = " - 2 1 . 2 2 6 9 4 8 2 0 2 1 2 4 1 "   z = " 1 4 . 8 9 0 2 4 3 3 1 5 7 5 9 6 "  /  > 
	//		   ^                    ^                     ^
	//		x=Tx(rotation,elevation)  y=Ty(rotation,elevation) z=Tz(elevation)	
	
	// first convert POLAR coordinates (rotation, elevation, radius) to CARTESIAN coordinates (x,y,z)
		
	$motorX = polar2CartesianX($armRadius, -$rotation[$i], 0.0, $elevation[$i]) ;	
	$motorY = polar2CartesianY($armRadius, -$rotation[$i], 0.0, $elevation[$i]) ;
	$motorZ = polar2CartesianZ($armRadius, $elevation[$i], 0.0  /* $pivotHeight */ ) ;
	
	// Note: these are cartesian coordinates relative to the MOTORS.  But we need to
	// "change the basis" to a set of axes that ReCap uses that are derived from
	// where the CAMERA is pointing. 
	
	// CHANGE OF BASIS (change of reference system axes)
	$motorPoint  = new Point ($motorX, $motorY, $motorZ);
	
	// The ZString in the Manifest contains the parameters for the change.
	$newOffsets = new Point( $TxOffset, $TyOffset, $TzOffset );
	$newAngles =  new Point( $AnglexOffset, $AngleyOffset, $AnglezOffset );
	$newScales =  new Point( $scaleFactorX,  $scaleFactorY, $scaleFactorZ );
	
	$cameraPoint = $motorPoint->changeBasis($newOffsets,$newAngles,$newScales);
	$x = $cameraPoint->x;
	$y = $cameraPoint->y;
	$z = $cameraPoint->z;
	
	echo "T(x,y,z) =  T($x, $y, $z)\n";
	$T = $CFRM->addChild('T');
	$T->addAttribute('x', "$x");
	$T->addAttribute('y', "$y");
	$T->addAttribute('z', "$z");
	
	//	  < R   x = " 4 6 . 3 1 6 3 3 4 7 0 5 0 0 0 5 "   y = " - 1 . 1 2 0 5 0 0 1 8 4 8 4 5 9 2 "   z = " 1 5 0 . 8 8 3 7 7 0 5 3 1 4 7 2 " / > 
	//        ^                    ^                     ^
	//      x=elevation         always 0                z= -rotation   
	
	//   TO BE ADJUSTED!
	$Rx = floatval($elevation[$i] + $RxOffset);
	echo "elevation[$i] = $elevation[$i];  Rx = $Rx\n";
	$Ry = floatval($RyOffset);  // how "canted" is the camera in the roll direction 
	
	$R = $CFRM->addChild('R');
	$R->addAttribute('x', "$Rx");
	$R->addAttribute('y', "$Ry");
	
	// autodesk templates expect rotations to be between -180 and +180 degrees
	$Rz = floatval(-$rotation[$i] + $RzOffset);
	echo "1:rotation[$i] = $rotation[$i];Rz[$i] =  $Rz\n";
	if ($Rz <= -180 ) {
		$Rz = floatval($Rz + 360.0);
		//echo "2:rotation[$i] = $rotation[$i];Rz[$i] =  $Rz\n";
	} else if ($templateRotation > 180  ) {
		$Rz = floatval($Rz - 360.0);
		//echo "3:rotation[$i] = $rotation[$i];Rz[$i] =  $Rz\n";
	} else { // rotation is between -180 and +180
		// do nothing
		//echo "4:rotation[$i] = $rotation[$i];Rz[$i] =  $Rz\n";
	}
	$R->addAttribute('z', "$Rz");

	echo "rotation[$i] = $rotation[$i];Rz[$i] =  $Rz\n";
	
	 
	//	 < / C F R M > 
	//	 < / S H O T > 
}


//  Postlog   -- we don't need this per Stephane at Autodesk
//  ...
//  < X R E F   u r l = " 3 . 0 . 0 / X R E F / 4 a d v W 6 m n 0 E W s 6 b H G G W D B Q l L m g D c = "   t = " m a r k e r s 2 D "   i d = " 4 a d v W 6 m n 0 E W s 6 b H G G W D B Q l L m g D c = " / > 
//$XREF1 = $RZML->addChild('XREF');
//$XREF1->addAttribute('url', "3.0.0/XREF/4advW6mn0EWs6bHGGWD801LMgDc= ");
//$XREF1->addAttribute('t', " m a r k e r s 2 D");
//$XREF1->addAttribute('id',"4advW6mn0EWs6bHGGWD801LMgDc= ");
					

//  <  X R E F   u r l = " 3.0.0/XREF/dGDdTyZu73Uh0Blods0PsyjPqbY"   t = " l o c a t o r s "   i d = "dGDdTyZu73Uh0Blods0PsyjPqbY  = "  / > 
//$XREF2 = $RZML->addChild('XREF');
//$XREF2->addAttribute('url', "3.0.0/XREF/dGDdTyZu73Uh0Blods0PsyjPqbY");
//$XREF2->addAttribute('t', "locators");
//$XREF2->addAttribute('id', " dGDdTyZu73Uh0Blods0PsyjPqbY");

//  < X R E F   u r l = " "   t = " r e f p r o j e c t "   i d = " M 1 I U R A 1 A M K L G l K x 8 Q 3 m W I N 1 0 8 d g " / > 
//$XREF3 = $RZML->addChild('XREF');
//$XREF3->addAttribute('url', " ");
//$XREF3->addAttribute('t', "refproject");
//$XREF3->addAttribute('id', "M1IURA1AMKG1Kx8Q3mWIN108dg");

//	 < X R E F   u r l = " 3 . 0 . 0 / X R E F / Y k 3 g 4 f H i W E 0 c b Q z 3 h 4 h t 6 E P 1 R J A = "   t = " m e s h "   i d = " Y k 3 g 4 f H i W E 0 c b Q z 3 h 4 h t 6 E P 1 R J A = " / > 
//$XREF4 = $RZML->addChild('XREF');
//$XREF4->addAttribute('url',"3.0.0/XREF/YK3g4fHiWE0cbQz3h4ht6EP1RJA=");
//$XREF4->addAttribute('t', "mesh");
//$XREF4->addAttribute('id', "YK3g4fHiWE0cbQz3h4ht6EP1RJA=");


//	 < S O B J > 
// $SOBJ = $RZML->addChild('SOBJ');

//	 < C M E S H   x r e f i d = " Y k 3 g 4 f H i W E 0 c b Q z 3 h 4 h t 6 E P 1 R J A = "   q = " 7 "   b = " - 1 2 0 , - 1 2 0 , -  2 0 , 1 2 0 , 1 2 0 , 1 2 0 " / > 
//                                               ^         ^
//                                           $meshLevel  $boundingBox
//$CMESH = $SOBJ->addChild('CMESH');

//$CMESH->addAttribute( 'xrefid' , 'Yk3g4fHiWE0cbQz3h4t6EP1RJA=' );
//$CMESH->addAttribute('q', "$meshLevel");
//$CMESH->addAttribute('b', "$boundingBox" ); 

// </SOBJ>
// </RZML>

$templateXMLString = $RZML->asXML();


// we take the rewritten XML file we just edited and save it for use
$newtemplate3DP = SCAN_DIR_ROOT."$scanID/auto-template.3dp";  // new template file
echo "writing ON-THE-FLY GENERATED template to $newtemplate3DP\n";

// we need to change the header so it says UTF-16, and we want to add 
// line feeds to make it more readable
$templatepieces = explode ( ">", $templateXMLString );
$templatepieces[0] = '<?xml version="1.0" encoding="UTF-16" standalone="yes"?';
$templatePPXMLString = implode ($templatepieces, ">\n");

// ReCap expects the template file to be in UTF-16 format (not UTF-8 which PHP uses)
// so we convert it and save it as a file that ReCap can upload.
$copy_utf16 = iconv("UTF-8", "UTF-16", $templatePPXMLString);
file_put_contents($newtemplate3DP, $copy_utf16);

//$templateXML->asXml($newtemplate3DP);

//  ------- END OF EDIT THE AUTOMATED TEMPLATE USING THESE HASH CODES  -------------------------

if ($template3DP == NULL) {
	 echo "\nUsing Automated Template\n";
	 $template3DP = $newtemplate3DP;
}  else {
	echo "\nUsing User Provided Template: $template3DP\n";
	
	//  -------------- EDIT THE USER PROVIDED TEMPLATE USING THESE HASH CODES  -------------------------
	//  The template file gives ReCap known camera positions etc.
	//  that ReCap uses to ensure that the 3D model is correctly scaled.
	echo "\n";
	$templateXML =  simplexml_load_file($template3DP);

	echo "#########rewriting user provided template  #########   \n";
	
	for ($n = 0; $n < $filecount; $n++) 
	{
		// for each image file we rename the corresponding filenames and hash codes for each
		// shot in the template file with the actual names and code in the uploaded images
		
		echo  "template SHOT filename: (" . $templateXML->SHOT[$n][0]['n'] .") ";
		$templateXML->SHOT[$n][0]['n'] = $filenames[$n];
		echo  "-> (" . $filenames[$n] .")\n";
		
		echo  "template IPLN filename: (" . $templateXML->SHOT[$n][0]->IPLN[0]['img'] .") ";
		$templateXML->SHOT[$n][0]->IPLN[0]['img'] = $filenames[$n];
		echo  "-> (" . $filenames[$n] .")\n\n";
		
		// $templateXML->SHOT[$n][0]->IPLN[0]['url'] = $filehashID[$n];
		$templateXML->SHOT[$n][0]->IPLN[0]['hash'] = $filehashID[$n];
	}

	// we take the rewritten XML file we just edited and save it for use
	$newtemplate3DP = SCAN_DIR_ROOT."$scanID/user-template.3dp";  // new template file
	echo "\n#### writing template to $newtemplate3DP ###\n";
	$templateXML->asXml($newtemplate3DP);
	$template3DP = $newtemplate3DP;
	//  ------- END OF EDIT THE TEMPLATE USING THESE HASH CODES  -------------------------
	
}


// ************** BEGIN SCALING SECTION **********************************************
echo "doModelScaling = $doModelScaling\n";
if ($doModelScaling == true) {  // If do_ModelScaling is false no need to create / upload template files

	//  --------------- UPLOAD THE NEW TEMPLATE FILE AND ADD AS TYPE = SOURCE ------------


	echo "\nSelecting template file to upload.\n";
	// Create the ReCap Request to add the edited template file.
	// There is only one file to upload but the form takes an array of files so we put
	// it in "file[0]".

	$files =array () ;
	$files["file[0]"] = "@" . $template3DP;
	$filename = basename($template3DP);
	echo "using template: $filename   from $template3DP \n";

	echo "\nUploading template files to the photoscene.\n";

	// Create the ReCap Request to add the template file
	// This POST call is unusual, since we are going to use POST to upload files to ReCap,  
	// but we also need to pass the ClientID and Timestamp for OAuth validation as GET variables.
	$timestamp = time();
	$url = "?clientID=" . ReCapClientID 
		 . "&timestamp=" . $timestamp 
		 . "&photosceneid={$photoSceneID}"
		 . "&type=source" ;
	$request =$client->post ("file" . $url) ; 
	$request->addPostFields (array ( 
		'photosceneid' 	=> $photoSceneID, 
		'type' 			=> 'source',
		'clientID' 		=> ReCapClientID, 
		'timestamp' 	=> $timestamp
	)) ;
	$request->addPostFiles ($files) ; // This adds the template file to upload
	$response =$request->send () ;

	// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;

	$xml =$response->xml () ;
	// echo var_dump($xml);   // need to understand what comes back

	if ($xml->Error) {  // if error stop here else continue
		$code =$xml->Error->code ;
		echo "My ReCap File Upload error code: {$code}\n" ;
		$msg  =$xml->Error->msg ;
		echo "My ReCap File Upload error message: {$msg}\n" ;
		exit($code);
	} else { echo "$newtemplate3DP template file uploaded and accepted by Recap.\n"; }
	//  ------------------ END UPLOAD THE TEMPLATE FILE AND ADD AS TYPE = SOURCE ---------

}  else {
	echo "Automatic SCALING is OFF\n";
}
// ************** END SCALING SECTION ************************************************


//----------------- LAUNCH PHOTOSCENE PROCESSING -------------------------------------
//- Process the scene - resource POST /photoscene/{}
echo "\nLaunch the photoscene processing...\n";
$request =$client->post ("photoscene/$photoSceneID") ;
$reCapLaunchTime =  time ();

$request->addPostFields (array (
	'clientID' => ReCapClientID, 
	'timestamp' => $reCapLaunchTime,
)) ;
$response =$request->send () ;

// echo "Response Body\n-------\n", $response->getBody (), "======\n\n" ;

$xml =$response->xml () ;

if ($xml->Error) { // if error stop here, else continue
	$code =$xml->Error->code ;
	echo "My ReCap Launch Processing error code: {$code}\n" ;
	$msg  =$xml->Error->msg ;
	echo "My ReCap Launch Processing message: {$msg}\n" ;
	// --------  NOTIFY to THAT JOB HAS BEEN RECEIVED  --------------------------
			 $to = $to;
			 $headers = "From: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com";
			 $subject = "SCAN MODELING REQUEST FAILED: Scan: $scanID / PhotoScene: $photoSceneID";
			 $message = "A SoundFit SugarCube scan has been received"
				. " but the model failed,  Recap Error ($code) $msg \n\n"
				. "Settings were:\n\n " 
				. print_r($_GET, true)
				. "\n\nUploaded folder: " . SCAN_DIR_URL . $scanID ;
			 mail($to,$subject,$message,$headers, "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com");
// --------  END OF NOTIFY to THAT JOB HAS BEEN RECEIVED  -------------------
	exit($code);
} { echo "$photoSceneID successfully processing request accepted by Recap.\n"; }

echo "\nPhotoscene processing has been initiated. \n";
echo "Callbacks will be started when photoscene processing is complete:\n";
echo "$callback\n";
//----------------- END OF LAUNCH PHOTOSCENE PROCESSING ------------------------------

if ($to != $from) {
// --------  NOTIFY RECIPIENT (TO) THAT JOB HAS BEEN RECEIVED  --------------------------
	 $emailto = $to;
	 $headers = "From: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com";
	 $emailsubject = "SCAN MODELING LAUNCHED: Scan: $scanID / PhotoScene: $photoSceneID";
	 $message = "A SoundFit SugarCube scan has been received"
		." and is now being processed.\n\nSettings are:\n\n " 
		. print_r($_GET, true)
		. "\n\nUploaded folder: " . SCAN_DIR_URL . $scanID ;
	mail($emailto,$emailsubject,$message,$headers,  "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com");
// --------  END OF NOTIFY to THAT JOB HAS BEEN RECEIVED  -------------------	         
}	         
 
// --------  NOTIFY SENDER (FROM) THAT JOB HAS BEEN LAUNCHED  -----------------------
	 $emailto = $from;
	 $headers = "From: sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com";
	 $emailsubject = "SCAN MODELING LAUNCHED: Scan: $scanID / PhotoScene: $photoSceneID";
	 $message = "A SoundFit SugarCube scan has been received"
		." and is now being processed.\n\nSettings are:\n\n " 
		. print_r($_GET, true)
		. "\n\nUploaded folder: " . SCAN_DIR_URL . $scanID 
		. "\n\n $outputString \n" ;
	 mail($emailto,$emailsubject,$message,$headers,  "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@apache2-nads.sawtelle.dreamhost.com");
	 
	 
	// ---------------- SAVE SENDER AND RECIPIENT FOR USE BY CALLBACK PROGRAM  --------------
	// ReCap doesn't like passing the sender and recipients in the callback URL,
	// so instead of passing them in the callback URL, we save them in an XML file that
	// the callback program can retrieve them from. 
	$emailmapfile = EMAILMAPDIR . $scanID . ".xml";
	echo "emailmapfile = $emailmapfile\n";
	$outputString = $outputString .  "emailmapfile = $emailmapfile\n";

	$emailxmlstring
		 = "<xml>\n" .
		 	"<startTime>$startTime</startTime>\n" .
		 	"<startTimeText>$today</startTimeText>\n" .
		 	"<photoScene>$photoSceneID</photoScene>\n" .
			$manifestXMLString .
			"</xml>\n";
			
	$emailxml =	new SimpleXMLElement($emailxmlstring);
	// write out the file:
	$emailxml->asXml($emailmapfile);
	echo "\n\n-------------------------------------------------------------\n\n";
	echo "emailxmlstring  = $emailxmlstring\n";	
	echo "\n\n-------------------------------------------------------------\n\n";
	$outputString = $outputString . "emailxmlstring = $emailxmlstring\n";	
		
					
// ---------------- END OF SAVE SENDER(FROM) AND RECIPIENT FOR USE BY CALLBACK PROGRAM -------
	
// ----------------- Write data to database ----------------------------------------------

// This version INSERTS a new record and populates the values known, now.  
// The Callback program will UPDATE that record with the rest of the fields when 
// modeling is complete.

function serverFilePath($filepath)  {
	$URL =  "http://" . strstr($filepath, DOMAIN );
	return $URL;
}

$uploadDirURL = serverFilePath($uploadDir);
$manifestXMLURL = serverFilePath($manifestXMLFile);
$submittedXMLURL = serverFilePath($emailmapfile);
$generatedTemplate3DPURL = serverFilePath($newtemplate3DP);
$imageGalleryViewerURL = serverFilePath($galleryFilePath);
$uploadTimeStamp = 	date('Y-m-d H:i:s',$uploadTime);
$triggerPgmTimeStamp = 	date('Y-m-d H:i:s',$startTime);
$reCapLaunchTimeStamp =  date('Y-m-d H:i:s',$reCapLaunchTime);  


	$connection = new mysqli("mysql.test.soundfit.me", "soundfit_admin", "Use4SoundFit!", "sf_orders");
	$SQLQuery = 
	  "INSERT INTO templates ( "
	  . "ScanID, PhotoSceneID, UploadDirURL, ManifestXMLURL, SubmittedXMLURL, "
	  . "GeneratedTemplate3DPURL, ImageGalleryViewerURL, UploadTimeStamp, " 
	  . "TriggerPgmTimeStamp, ReCapLaunchTimeStamp ) VALUES ( "
	  . "'". "$scanID" . "'" . "," 
	  . "'". "$photoSceneID" . "'" . "," 
	  . "'". "$uploadDirURL" . "'" . "," 
	  . "'". "$manifestXMLURL" . "'" . ","  
	  . "'". "$submittedXMLURL" . "'" . ","  
	  . "'". "$generatedTemplate3DPURL" . "'" . ","  
	  . "'". "$imageGalleryViewerURL" . "'" . ","  
	  . "'".  "$uploadTimeStamp" . "'" . "," 
	  . "'".  "$triggerPgmTimeStamp" . "'" . "," 
	  . "'".  "$reCapLaunchTimeStamp" . "'" .
	  " )";
	
	$result = mysqli_query($connection, $SQLQuery );
	
	echo "updating database: \n";
	echo "scanID=$scanID\n"; 
	echo "photoSceneID=$photoSceneID\n"; 
	echo "uploadDirURL=$uploadDirURL\n"; 
	echo "manifestXMLURL=$manifestXMLURL\n"; 
	echo "submittedXMLURL=$submittedXMLURL\n"; 
	echo "generatedTemplate3DPURL=$generatedTemplate3DPURL,\n"; 
	echo "imageGalleryViewerURL=$imageGalleryViewerURL\n"; 
	echo "uploadTimeStamp=$uploadTimeStamp\n";
	echo "triggerPgmTimeStamp=$triggerPgmTimeStamp\n"; 
	echo "reCapLaunchTimeStamp=$reCapLaunchTimeStamp\n\n";
	echo "$SQLQuery \n\n";
	echo "RESULT =  $result \n";
	
	// $row = mysqli_fetch_assoc($result);
	mysqli_close($connection);

// ----------------- END Write data to database ------------------------------------------	
	 
	  
	 echo "\n-------- END OF ReCapLaunchModeling-wScale+Crop9i.php. ---------------\n\n";
	 
	 exit(0); // all done, okay.
			 
// --------  END OF NOTIFY RECIPIENT (TO) THAT JOB HAS BEEN RECEIVED  ----------------	


echo "</pre>";     
exit ;
?>
