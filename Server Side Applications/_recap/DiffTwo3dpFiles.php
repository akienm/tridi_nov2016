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


	// If you want to run this as a command line program you can pass parameters on command line
if (php_sapi_name() === 'cli') {  // running from command line
	parse_str(implode('&', array_slice($argv, 1)), $_GET);
	$deg= "\u00B0";
	$detailHeader ="\n\nsource\t\tShot\tfovx(mm)\tTx(mm)\tTy(mm)\tTz(mm)\tRx(deg)\tRy(deg)\tRz(deg)\n";
	$diffTableHeader ="\nShot\tfovx(mm)\tTx(mm)\tTy(mm)\tTz(mm)\tRx(deg)\tRy(deg)\tRz(deg)\n";
	
} else {  // running in a browser
	$detailHeader ="\n\nsource\t\tShot\tfovx(mm)\tTx(mm)\tTy(mm)\tTz(mm)\tRx(&deg)\tRy(&deg)\tRz(&deg)\n";
	$diffTableHeader ="\nShot\tfovx(mm)\tTx(mm)\tTy(mm)\tTz(mm)\tRx(&deg)\tRy(&deg)\tRz(&deg)\n";
}
 

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
	
	$htmlTrailer = '</body>';	
	$DiffPageString = $DiffPageString . "<div>";

	$DiffPageString = $DiffPageString . "<p>USAGE: test.soundfit.me/apps/_recap/ReCap3DPCompare.php?template=TemplateFilePath&model=ModelFilePath&detail=true\n";
	$DiffPageString = $DiffPageString . "<br>USAGE: ?template= is REQUIRED. It is a fully qualified file path on server to the TEMPLATE file to use.\n";
	$DiffPageString = $DiffPageString . "<br>USAGE: &model= is REQUIRED. It is a fully qualified file path on server to the MODEL file to use.\n";
	$DiffPageString = $DiffPageString . "<br>USAGE: &detail= is OPTION.   If &detail= has  any value you'll see the a longer print out, which shows you each\n";
	$DiffPageString = $DiffPageString . "<br>       model and template line detail, along with each difference line.  If this value is not set you'll";
	$DiffPageString = $DiffPageString . "<br>       only see the summary differences table.\n\n</p>"; 

	$templateXMLFile = htmlspecialchars($_GET["template"]);
	if ($templateXMLFile == NULL) {
		$DiffPageString = $DiffPageString . "ERROR: template filepath is a required parameter.\n";
		$DiffPageString = $DiffPageString . "using: template=/home/earchivevps/test.soundfit.me/Scans/b3/templates/BadTEMPLATE-1.3dp for demo purposes\n";  
		$templateXMLFile = "/home/earchivevps/test.soundfit.me/Scans/b3/templates/BadTEMPLATE-1.3dp";
	} else $DiffPageString = $DiffPageString . "passed in templateXMLFile = $templateXMLFile \n";
	$templateXML = simplexml_load_file($templateXMLFile);

	$modelXMLFile = htmlspecialchars($_GET["model"]);
	if ($modelXMLFile == NULL) {
		$DiffPageString = $DiffPageString . "ERROR: model filepath is a required parameter.\n"; 
		$DiffPageString = $DiffPageString . "using: model=/home/earchivevps/test.soundfit.me/Scans/b3/templates/GoodTEMPLATE.3dp for demo purposes\n";
		$modelXMLFile= "/home/earchivevps/test.soundfit.me/Scans/b3/templates/GoodTEMPLATE.3dp";
	} else $DiffPageString = $DiffPageString . "passed in modelXMLFile = $modelXMLFile \n";
	$modelXML = simplexml_load_file($modelXMLFile);
	$DiffPageString = $DiffPageString . "</div>";
	
	
	$DiffPageString = $DiffPageString . "<div>";
	$DiffPageString = $DiffPageString .  "<h2>Difference Table</h2>"; 

	$DiffPageString = $DiffPageString .  "<p>template = $templateXMLFile<br />";
	$templateXML = simplexml_load_file($templateXMLFile);

	$DiffPageString = $DiffPageString .  "model = $modelXMLFile</p>";

	// echo "\n\ntheUTF8_3dpString = $theUTF8_3dpString\n\n";
	// $modelXML = simplexml_load_string($theUTF8_3dpString);
	// $modelXML = simplexml_load_file($modelXMLFile);

	$DiffPageString = $DiffPageString .  "</div>";

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

	// $diffPageFile = $scan_directory . '/' . "DiffTable.html";
	// file_put_contents($diffPageFile, $DiffPageString);

	// echo "Difference Table at: $diffPageFile\n";
	// echo "Difference Table  HTML at: $ModeledURL$scanID/DiffTable.html\n";

	echo $DiffPageString;
	
	?>
