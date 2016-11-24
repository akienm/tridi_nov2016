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

 ReCapDiffTblGenerator.php
 
 Copyright 2013, 2014 (c) SoundFit, LLC. All rights reserved 
 
 Developed by Scott McGregor -- SoundFit
 May 2014
 
 This program takes two 3DP files, a "template" which defines where 
 camera positions were expected to be,  and a "model" which describes where
 Autodesk's software calculated them to be.    This program then calculates
 and displays a "difference matrix" that shows differences between the values
 specified in the model vs. the template.  
 
 In the ideal case, all values would be zero.
 In practice small variations are expected.   However, big variations indicate 
 that the model was not calculated as expected.
 
 The 3DP files are both forms of XML files.   Each file has a series of "shots".
 Both files should have the same number of shots.
 
*/

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

echo $htmlHeader;
	
$htmlTrailer = '</body>';	
 
 

echo "<div>";
echo "<h2>Difference Table</h2>";  
$templateXMLFile = "/home/earchivevps/test.soundfit.me/Scans/b3/templates/BadTEMPLATE-1.3dp";
echo "<p>template = $templateXMLFile<br />";
$templateXML = simplexml_load_file($templateXMLFile);

$modelXMLFile= "/home/earchivevps/test.soundfit.me/Scans/b3/templates/GoodTEMPLATE.3dp";
echo "model = $modelXMLFile</p>";
$modelXML = simplexml_load_file($modelXMLFile);

echo "</div>";

$templateShots = $templateXML -> SHOT;

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

$tblheader = "</h3><table><tr><th>source</th><th>Shot</th><th>fovx(mm)</th><th>Tx(mm)</th><th>Ty(mm)</th><th>Tz(mm)</th><th>Rx(&deg)</th><th>Ry(&deg)</th><th>Rz(&deg)</th></tr>";

$templateTableString = "<div><h3>Template: ".$templateXMLFile. $tblheader;
$modelTableString="<div><h3>Model: ".$modelXMLFile.$tblheader;
$differenceTableString="<div><h3>Difference Table (float)".$tblheader;
$differenceINTTableString="<div><h3>Difference Table (integer)".$tblheader;

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
	
	
	$modelShotValue = $modelXML -> SHOT[$shotNumber];
	
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

echo $differenceINTTableString;
echo $templateTableString;
echo $modelTableString;
echo $differenceTableString;

echo $htmlTrailer;
?>