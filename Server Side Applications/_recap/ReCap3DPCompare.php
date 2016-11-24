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
echo "\n------- ReCap3DPCompare.php ------------------------------\n\n";

/*

 ReCap3DPCompare.php.php
 
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
 


echo "USAGE: test.soundfit.me/apps/_recap/ReCap3DPCompare.php?template=TemplateFilePath&model=ModelFilePath&detail=true\n";
echo "USAGE: ?template= is REQUIRED. It is a fully qualified file path on server to the TEMPLATE file to use.\n";
echo "USAGE: &model= is REQUIRED. It is a fully qualified file path on server to the MODEL file to use.\n";
echo "USAGE: &detail= is OPTION.   If &detail= has  any value you'll see the a longer print out, which shows you each\n";
echo "       model and template line detail, along with each difference line.  If this value is not set you'll";
echo "       only see the summary differences table.\n\n"; 

$templateXMLFile = htmlspecialchars($_GET["template"]);
if ($templateXMLFile == NULL) {
	echo "ERROR: template filepath is a required parameter.\n";
	echo "using: template=/home/earchivevps/test.soundfit.me/Scans/b3/templates/BadTEMPLATE-1.3dp for demo purposes\n";  
	$templateXMLFile = "/home/earchivevps/test.soundfit.me/Scans/b3/templates/BadTEMPLATE-1.3dp";
} else echo "passed in templateXMLFile = $templateXMLFile \n";
$templateXML = simplexml_load_file($templateXMLFile);

$modelXMLFile = htmlspecialchars($_GET["model"]);
if ($modelXMLFile == NULL) {
	echo "ERROR: model filepath is a required parameter.\n"; 
	echo "using: model=/home/earchivevps/test.soundfit.me/Scans/b3/templates/GoodTEMPLATE.3dp for demo purposes\n";
	$modelXMLFile= "/home/earchivevps/test.soundfit.me/Scans/b3/templates/GoodTEMPLATE.3dp";
} else echo "passed in modelXMLFile = $modelXMLFile \n";
$modelXML = simplexml_load_file($modelXMLFile);

echo "\n";
$detail = htmlspecialchars($_GET["detail"]);
if ($detail == NULL) { $detailSwitch = false; echo "detail=FALSE\n"; } 
else { $detailSwitch = true; echo "detail=TRUE\n"; }


$templateShots = $templateXML -> SHOT;
$templateMatrix = array();
$modelMatrix = array();
$differenceMatrix = array();
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
	
	if ($detailSwitch == true) {
		echo $detailHeader;
		echo "template\t$shotNumber" .
		"\t" . intval($templateShotArray['fovx']) .
		"\t\t" . intval($templateShotArray['Tx']) .
		"\t" . intval($templateShotArray['Ty']) .
		"\t" . intval($templateShotArray['Tz']) .
		"\t" . intval($templateShotArray['Rx']) .
		"\t" . intval($templateShotArray['Ry']) .
		"\t" . intval($templateShotArray['Rz']) . "\n";
	}
	
	$templateMatrix[$shotNumber] = $templateShotArray;
	
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
	
	if ($detailSwitch == true) {
		echo "model\t\t$shotNumber" .
		"\t" . intval($modelShotArray['fovx']) .
		"\t\t" . intval($modelShotArray['Tx']) .
		"\t" . intval($modelShotArray['Ty']) .
		"\t" . intval($modelShotArray['Tz']) .
		"\t" . intval($modelShotArray['Rx']) .
		"\t" . intval($modelShotArray['Ry']) .
		"\t" . intval($modelShotArray['Rz']) . "\n";
	}
	
	$modelMatrix[$shotNumber] = $modelShotArray;	
	
	$differenceArray = array (
		"fovx" 		=> abs(floatval($modelShotArray['fovx']) -   floatval($templateShotArray['fovx'])),
		"Tx" 		=> abs(floatval($modelShotArray['Tx']) - floatval($templateShotArray['Tx'])),
		"Ty" 		=> abs(floatval($modelShotArray['Ty']) - floatval($templateShotArray['Ty'])),
		"Tz" 		=> abs(floatval($modelShotArray['Tz']) - floatval($templateShotArray['Tz'])),
		"Rx" 		=> abs(floatval($modelShotArray['Rx']) - floatval($templateShotArray['Rx'])),
		"Ry" 		=> abs(floatval($modelShotArray['Ry']) - floatval($templateShotArray['Ry'])),
		"Rz" 		=> abs(floatval($modelShotArray['Rz']) - floatval($templateShotArray['Rz'])),
	);
	$differenceMatrix[$shotNumber] = $differenceArray;
	
	if ($detailSwitch == true) {
		echo "difference\t$shotNumber" .
		"\t" . intval($differenceArray['fovx']) .
		"\t\t" . intval($differenceArray['Tx']) .
		"\t" . intval($differenceArray['Ty']) .
		"\t" . intval($differenceArray['Tz']) .
		"\t" . intval($differenceArray['Rx']) .
		"\t" . intval($differenceArray['Ry']) .
		"\t" . intval($differenceArray['Rz']) . "\n";
	}
	
	
	$sumArray["fovx"] 	= $sumArray["fovx"] + $differenceArray["fovx"];
	$sumArray["Tx"] 	= $sumArray["Tx"] + $differenceArray["Tx"];
	$sumArray["Ty"] 	= $sumArray["Ty"] + $differenceArray["Ty"];
	$sumArray["Tz"] 	= $sumArray["Tz"] + $differenceArray["Tz"];
	$sumArray["Rx"] 	= $sumArray["Rx"] + $differenceArray["Rx"];
	$sumArray["Ry"] 	= $sumArray["Ry"] + $differenceArray["Ry"];
	$sumArray["Rz"] 	= $sumArray["Rz"] + $differenceArray["Rz"];
	
	$maxArray["fovx"] 	= max($maxArray["fovx"] , $differenceArray["fovx"]);
	$maxArray["Tx"] 	= max($maxArray["Tx"] , $differenceArray["Tx"]);
	$maxArray["Ty"] 	= max($maxArray["Ty"] , $differenceArray["Ty"]);
	$maxArray["Tz"] 	= max($maxArray["Tz"] , $differenceArray["Tz"]);
	$maxArray["Rx"] 	= max($maxArray["Rx"] , $differenceArray["Rx"]);
	$maxArray["Ry"] 	= max($maxArray["Ry"] , $differenceArray["Ry"]);
	$maxArray["Rz"] 	= max($maxArray["Rz"] , $differenceArray["Rz"]);
	
}

$avgArray = array (
		"fovx" 	=> $sumArray["fovx"] / $shotNumber,
		"Tx" 	=> $sumArray["Tx"] / $shotNumber,
		"Ty" 	=> $sumArray["Ty"] / $shotNumber,
		"Tz" 	=> $sumArray["Tz"] / $shotNumber,
		"Rx" 	=> $sumArray["Rx"] / $shotNumber,
		"Ry" 	=> $sumArray["Rx"] / $shotNumber,
		"Rz" 	=> $sumArray["Rx"] / $shotNumber
	);

echo "\n\nDIFFERENCE ARRAY \n";
echo "template=$templateXMLFile \n";
echo "model=$modelXMLFile \n";
echo $diffTableHeader;
for ($i=1; $i<= $shotNumber; $i++) {
	echo "$i" .
	"\t" . intval($differenceMatrix[$i]['fovx']) .
	"\t\t" . intval($differenceMatrix[$i]['Tx']) .
	"\t" . intval($differenceMatrix[$i]['Ty']) .
	"\t" . intval($differenceMatrix[$i]['Tz']) .
	"\t" . intval($differenceMatrix[$i]['Rx']) .
	"\t" . intval($differenceMatrix[$i]['Ry']) .
	"\t" . intval($differenceMatrix[$i]['Rz']) . "\n";	
}

echo "AVG" .
	"\t" . intval($avgArray['fovx']) .
	"\t\t" . intval($avgArray['Tx']) .
	"\t" . intval($avgArray['Ty']) .
	"\t" . intval($avgArray['Tz']) .
	"\t" . intval($avgArray['Rx']) .
	"\t" . intval($avgArray['Ry']) .
	"\t" . intval($avgArray['Rz']) . "\n";	

echo "MAX" .
	"\t" . intval($maxArray['fovx']) .
	"\t\t" . intval($maxArray['Tx']) .
	"\t" . intval($maxArray['Ty']) .
	"\t" . intval($maxArray['Tz']) .
	"\t" . intval($maxArray['Rx']) .
	"\t" . intval($maxArray['Ry']) .
	"\t" . intval($maxArray['Rz']) . "\n";		 
 
  
//$modelShots = $modelXML -> SHOT;
//$modelMatrix = array();

//foreach ($modelShots as $modelShotValue) 
//{
//	$shotNumber 	= intval($modelShotValue['i']);
//	$modelShotArray =   array (
//		"fovx" 		=> $modelShotArray -> CFRM['fovx'],
//		"Tx" 		=> $modelShotArray -> CFRM -> T['x'],
//		"Ty" 		=> $modelShotArray -> CFRM -> T['y'],
//		"Tz" 		=> $modelShotArray -> CFRM -> T['z'],
//		"Rx" 		=> $modelShotArray -> CFRM -> R['x'],
//		"Ry" 		=> $modelShotArray -> CFRM -> R['y'],
//		"Rz" 		=> $modelShotArray -> CFRM -> R['z'],
//	);

//	//print_r($modelShotArray);
//	$modelMatrix[$shotNumber] = $modelShotArray;
//}


//print_r ($templateMatrix);


echo "\n------- END ReCap3DPCompare.php ------------------------------\n\n";

?>