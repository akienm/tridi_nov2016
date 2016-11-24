<?php

$template3DP = "./template.3dp";  // default template file
$project3DP = "./project.3dp";  // default template file
$newtemplate3DP =  "./edited-template.xml";  // default template file

$templateXML =  simplexml_load_file($template3DP);
$projectXML =   simplexml_load_file($project3DP);

// read the names and hashes from the project 
$shots = $projectXML-> SHOT;
$n = 0;
foreach ( $shots as $key => $shot) 
{
	$shot_name[$n] = $shot[0]['n'];
	$ipln = $shot -> IPLN;

	$templateXML->SHOT[$n][0]['n'] = $shot[0]['n'];
	$templateXML->SHOT[$n][0]->IPLN[0]['img'] = $ipln[0]['img'];
	$templateXML->SHOT[$n][0]->IPLN[0]['hash'] = $ipln[0]['hash'];
	
	$n++;
}

//var_dump($templateXML);
// Saving the whole modified XML to a new filename
$newtemplateXML = $templateXML->asXml();
echo "$newtemplateXML\n";
//file_put_contents($newtemplate3DP,$newtemplateXML);

$templateXML->asXml($newtemplate3DP);



?>