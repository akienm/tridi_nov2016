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
    
echo "\n------- modify-templates.php ------------------------------\n\n";

/*
 modify-templates.php
 
 Copyright 2013, 2014 (c) SoundFit, LLC. All rights reserved 
 
 Developed by Scott McGregor -- SoundFit
 August 2013 - February 2014
 */
 
 
// -------------------- PASSED PARAMETERS -----------------------------------------
// Various parameters should be passed by the calling program (normal case)
// or a user who runs the program from a command line (testing). 
// parameters, other than ScanID are optional and have a default value. 
// The program checks to see if it is run in a command line,  if it is, all parameters
// are pulled from the argv list and put into the $_GET array.   

if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}

define ('TEMPLATE3DP', '/home/earchivevps/test.soundfit.me/Scans/b1/templates/Template-beta-default.3dp');  // default template file 
define ('NEWTEMPLATE3DP', '/home/earchivevps/test.soundfit.me/Scans/b1/templates/Template-beta-new.3dp');  // default template file 

$template3DP = htmlspecialchars($_GET["template3DP"]);
echo "passed in template3DP = $template3DP \n";
if ($template3DP == NULL) {
	$template3DP = realpath(TEMPLATE3DP); // default template file 
	echo "template3DP = $template3DP \n";
}

$newtemplate3DP = htmlspecialchars($_GET["newtemplate3DP"]);
echo "passed in newtemplate3DP = $newtemplate3DP \n";
if ($newtemplate3DP == NULL) {
	$newtemplate3DP = realpath(NEWTEMPLATE3DP); // default template file 
	echo "newtemplate3DP = $newtemplate3DP \n";
}


//  -------------- EDIT THE TEMPLATE TO REMOVE URL  -------------------------
//  The template file gives ReCap known camera positions etc.
//  that ReCap uses to ensure that the 3D model is correctly scaled.
echo "\n";
$templateXML =  simplexml_load_file($template3DP);

echo "BEFORE EDITING:\n";
print_r($templateXML);

$shots = $templateXML-> SHOT;
$n = 0;
foreach ( $shots as $key => $shot) 
{
	$shot_name[$n] = $shot[0]['n'];
	$ipln = $shot -> IPLN;

	unset ( $templateXML->SHOT[$n][0]->IPLN[0]['img'] );
	
	$n++;
}


echo "AFTER EDITING:\n";
print_r($templateXML);


// we take the rewritten XML file we just edited and save it for use
	
echo "writing template to $newtemplate3DP";
$templateXML->asXml($newtemplate3DP);
//  ------- END OF EDIT THE TEMPLATE TO REMOVE URL  -------------------------
	
	
echo "\n------- End of modify-templates.php ------------------------------\n\n";
