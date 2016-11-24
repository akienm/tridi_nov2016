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

echo "\n------- END metricsForReference3DPs3.php ------------------------------\n\n";

function distance3D ($x1, $y1, $z1, $x2, $y2, $z2) {
	$distanceSquared = pow($x2 - $x1, 2) + pow($y2 - $y1, 2) + pow($z2 - $z1, 2);
	$distance = sqrt($distanceSquared);
	// echo "x1 = $x1, y1 = $y1, z1 = $z1,\n";
	// echo "x2 = $x2, y2 = $y2, z2 = $z2\n";
	// echo "distanceSquared = $distanceSquared, distance = $distance\n\n";
	return $distance;
}

function RotDiff ($rotation,  $rz) {
	if($rotation <180) $rotdiff = $rotation+$rz;
	else $rotdiff = 360-$rotation-$rz;
	return $rotdiff;
}

function calculate_median($arr) {
    sort($arr);
    $count = count($arr); //total numbers in array
    $middleval = floor(($count-1)/2); // find the middle value, or the lowest middle value
    if($count % 2) { // odd number, middle is the median
        $median = $arr[$middleval];
    } else { // even number, calculate avg of 2 medians
        $low = $arr[$middleval];
        $high = $arr[$middleval+1];
        $median = (($low+$high)/2);
    }
    return $median;
}

function calculate_average($arr) {
    $count = count($arr); //total numbers in array
    foreach ($arr as $value) {
        $total = $total + $value; // total value of array numbers
    }
    $average = ($total/$count); // get average value
    return $average;
}

function standard_deviation($aValues, $bSample = false)
{
    $fMean = array_sum($aValues) / count($aValues);
    $fVariance = 0.0;
    foreach ($aValues as $i)
    {
        $fVariance += pow($i - $fMean, 2);
    }
    $fVariance /= ( $bSample ? count($aValues) - 1 : count($aValues) );
    return (float) sqrt($fVariance);
}
    
echo "<pre>";    
echo "\n------- metricsForReference3DPs2.php ------------------------------\n\n";

/*    metricsForReference3DPs.php
 
 This program takes a ScanID file pattern (?pattern=....)
 For each 3DP folder matching in test.soundfit.me/Scans/b3/modeled/$pattern/
 This program creates a record in a table.   
 If there are N matching models there will be N data records
 
 The table contains the following fields (one per row,  columns are observations/models):
 
 ScanID
 PhotoSceneID
 View3DModel.php hyperlink
 DiffTable.html hyperlink
 Image gallery (received/$prefix/index.php) hyperlink
 Manifest hyperlink
 OBJ hyperlink
 3DP hyperlink
 
 For N "shots" in the 3DP file, 9 columns:  The shot number, then the next two are from the Manifest file
 and the last7 are from the 3DP file:
 	ShotNumber
  	Elevation -- in degrees, from the Manifest file
 	Rotation -- in degrees, from the Manifest file
 	T(x)  Camera position - cartesian coordinates in mm
 	T(y)  Camera position - cartesian coordinates in mm
 	T(z)  Camera position - cartesian coordinates in mm
 	R(x)  Camera angle -- in degrees
 	R(y)  Camera angle -- in degrees
 	R(z)  Camera angle -- in degrees
 	FOVx  Field of View angle in degrees
 	DiffElevation = Elevation -  R(x)
 	DiffRotation =  Rotation - R(z)
 end "shots" loop.
 
 In addition to the N shots, we will have 3 other measures, and 18 coordinates for each model.
 The MaxX-MinX corner to corner distance (from the OBJ file)
 The MaxY-MinY corner to corner distance (from the OBJ file)
 The MaxZ-MinZ corner to corner distance (from the OBJ file)
 MaxX(x), MaxX(y), MaxX(z), MinX(x), MinX(y), MinX(z)
 MaxY(x), MaxY(y), MaxY(z), MinY(x), MinY(y), MinY(z)
 MaxZ(x), MaxZ(y), MaxZ(z), MinZ(x), MinZ(y), MinZ(z)
 
 
 After this table is created,  we will calculate summary statistics across
 all the numerical values:
 Maximum Value,
 Minimum Value,
 Average (Mean) Value,
 Median Value,
 Spread (Difference between Maximum and Minimum values),
 Std. Deviation
 
 
 The table is output as a CSV file, in scaletests folder with the name of the pattern
 
 Since any one of these models could have been used to create a manual template to 
 create ALL of the other models, no manual template parameter could be more
 accurate than the mean value, and most template parameters would be within 2 standard deviations of
 the mean value.
 
 We do not know how much variation in template camera positions ReCap can handle. and 
 still create great models.  We can also look at how much variation there is in the 
 difference measurs -- which measure the amount of scale variance exist in the resulting
 models (as opposed to variance in camera positions.
 
 */
 
// If you want to run this as a command line program you can pass parameters on command line
if (php_sapi_name() === 'cli') {  // running from command line
	parse_str(implode('&', array_slice($argv, 1)), $_GET);
	$deg= "\u00B0";
}	
//  Get the Parameters
//  echo "USAGE: test.soundfit.me/apps/_recap/metricsForReference3DPs.php?pattern=FilePathpattern\n";
//  echo "USAGE: ?pattern= is REQUIRED. It is a fully qualified file path on server to the File patterns to use for selecting models to analyze.\n";

$receivedDirURL  = "http://test.soundfit.me/Scans/b3/received";
$receivedDirPath = "/home/earchivevps/test.soundfit.me/Scans/b3/received";
$modeledDirURL   = "http://test.soundfit.me/Scans/b3/modeled";
$modeledDirPath  = "/home/earchivevps/test.soundfit.me/Scans/b3/modeled";
$mapsDirPath     = "/home/earchivevps/test.soundfit.me/Scans/b3/maps/";
$scaletestsDirPath = "/home/earchivevps/test.soundfit.me/Scans/b3/scaletests/";
$scaletestsDirURL = "http://test.soundfit.me/Scans/b3/scaletests/";

$pattern = htmlspecialchars($_GET["pattern"]);
if ($pattern == NULL) {
	echo "ERROR: pattern is a required parameter.\n";
	echo "using: pattern=$modelledDirPath"."SLM-u8-Sstr201405270648-RefCubeVarTest4-*/ for demo purposes\n";  
	$pattern = "$modelledDirPath"."SLM-u8-Sstr201405270648-RefCubeVarTest4-*/ for demo purposes\n";
} else echo "passed in pattern = $pattern \n";

$help = htmlspecialchars($_GET["help"]);
if ($help != NULL) {
  	echo "USAGE: test.soundfit.me/apps/_recap/metricsForReference3DPs.php?pattern=FilePathpattern\n";
  	echo "USAGE: ?pattern= is REQUIRED. It is a fully qualified file path on server to the File patterns to use for selecting models to analyze.\n";
} 

$modelledDir = glob("$modeledDirPath"."$pattern*");

// print_r($modelledDir);

$modelMetrics[" label"] = array();
$modelNumber = 1;
foreach ( $modelledDir as $modelDir ) {
	
	$scanID = basename($modelDir); 
	$photosceneID = file_get_contents("$mapsDirPath"."$scanID");	
	$view3DModelURL = "$modeledDirURL/$scanID/View3DModel.php";
	$diffTableURL = "$modeledDirURL/$scanID/DiffTable.html";
	$imageGalleryURL = "$receivedDirURL/$scanID/index.php";
	$manifestURL = "$receivedDirURL/$scanID/Manifest.xml";
	$manifestfile = "$receivedDirPath/$scanID/Manifest.xml";
	$objURL  = "$modeledDirURL/$scanID/mesh.obj";
	$objfile = "$modeledDirPath/$scanID/mesh.obj";
	$the3dpURL  = "$modeledDirURL/$scanID/$scanID.3dp";
	$the3dpfile = "$modeledDirPath/$scanID/$scanID.3dp";
	
	echo "$modelNumber: ScanID = $scanID\n";
	
	// ******** get the elevation and rotation from the Manifest file  ***********
	if (file_exists($manifestfile)) { // Manifest file exists 
		$manifestXML = simplexml_load_file($manifestfile);
		$manifestXMLString = $manifestXML ->asXML();
		// echo "Before shortening: manifestXMLString = $manifestXMLString\n";
		$manifestXMLString = strstr($manifestXMLString,"<SugarCubeScan");
		// echo "After shortening: manifestXMLString = $manifestXMLString\n\n";
		
	} else {  // No Manifest file! 
		$errorflag = true;
		echo "Manifest.xml file does not exist.\n";       	 
	} 
	 
	$shotNumber=1; 
	foreach ($manifestXML -> imageset -> image as $imageObject) {
		$elevation[$shotNumber] =  floatval($imageObject['elevation'][0]);
		$rotation[$shotNumber] =   floatval($imageObject['rotation'][0]);
		// echo "Shot $shotNumber:  elevation = $elevation[$shotNumber] ; rotation = $rotation[$shotNumber] \n"; 
		$shotNumber++;
		
	}
	
	
	// ****** get the shots, T(x),T(y),T(z), R(x),R(y),R(z), and FOVX from the 3DP file ****
	
	// -------------------- UNCOMPRESS 3DP FILE INTO A STRING ----------------------------
	// Now we will copy the 3DP file from ReCap into the "modeled" Scan directory
	set_time_limit(0); 
	$file = file_get_contents($the3dpURL);  // get ReCap data file into string
	
	$result4 = shell_exec("/bin/rm -f /tmp/$scanID\*");
	
	file_put_contents("/tmp/$scanID.3dp.gz", $file); // create our own copy from string
	
	$filesize = filesize ($the3dpfile);
	if ($filesize == 0) {
		echo "ERROR! NO 3DP MODEL: 3DP file is ZERO length\n" ;		
	}

	// we need an uncompressed, UTF-8 version to create the diff table
	$result0 = shell_exec("/bin/rm -f /tmp/$scanID.3dp");
	
	$result1 = shell_exec("gunzip -f /tmp/$scanID.3dp.gz > /tmp/$scanID.3dp");
	// echo "result1 = $result1\n\n";
	$result2 = shell_exec("iconv -f UTF-16 /tmp/$scanID.3dp"); 
	$result3 = shell_exec("/bin/rm -f /tmp/$scanID\*");
	$theUTF8_3dpString = strstr($result2, "<RZML");
	
	// -------------------- END OF UNCOMPRESS 3DP FILE INTO A STRING  --------------------

	$modelXML = simplexml_load_string($theUTF8_3dpString);
	$modelShots = $modelXML -> SHOT;
	$shotNumber=1;
	foreach ($modelShots as $modelShotValue) 
	{
		$fovx[$shotNumber] 		= floatval($modelShotValue -> CFRM['fovx']);
		$Tx[$shotNumber] 		= floatval($modelShotValue -> CFRM -> T['x']);
		$Ty[$shotNumber] 		= floatval($modelShotValue -> CFRM -> T['y']);
		$Tz[$shotNumber] 		= floatval($modelShotValue -> CFRM -> T['z']);
		$Rx[$shotNumber] 		= floatval($modelShotValue -> CFRM -> R['x']);
		$Ry[$shotNumber] 		= floatval($modelShotValue -> CFRM -> R['y']);
		$Rz[$shotNumber] 		= floatval($modelShotValue -> CFRM -> R['z']);
		$shotNumber++;
	}
	
	// ********** get 3 diagonal distances & 18 coordinates from the OBJ file  ***********
	// save the T(x,y,z) where x, y, or z are the maximum or minimum.
	// $XDiagonal = MinTx(x,y,z)  from the MaxTx(x,y,z)
	// $YDiagonal = MinTy(x,y,z)  from the MaxTy(x,y,z)
 	// $ZDiagonal = MinTz(x,y,z)  from the MaxTz(x,y,z)

 	$maxX = $minX = $maxY = $minY = $maxZ = $minZ = 0; 
	$XDiagonal = $YDiagonal = $ZDiagonal = 0;
	
	
	// open the OBJ file
	//  $recordArray = file($objfile); // file is too large to read into memory at once :-(

	$handle = fopen($objfile, "r");
	if ($handle) {
    	while (($objRecord = fgets($handle)) !== false) {
        	// process the line read.
        	
        	$tokens =  explode(" ",$objRecord);
			
			if ( $tokens[0] == "v" ) {
		
				$x = $tokens[1];
				$y = $tokens[2];
				$z = trim($tokens[3]);
			
				if ($x < $minX) { 
					$minX  = $x;
					$minXx = $x; 
					$minXy = $y; 
					$minXz = $z;
				} else if ($x > $maxX) { 
					$maxX  = $x;
					$maxXx = $x; 
					$maxXy = $y; 
					$maxXz = $z;
				}
			
				if ($y < $minY) { 
					$minY  = $y;
					$minYx = $x; 
					$minYy = $y; 
					$minYz = $z;
				} else if ($y > $maxY) { 
					$maxY  = $y;
					$maxYx = $x; 
					$maxYy = $y; 
					$maxYz = $z;
				}
			
				if ($z < $minZ) { 
					$minZ  = $z;
					$minZx = $x; 
					$minZy = $y; 
					$minZz = $z;
				} else if ($z > $maxZ) { 
					$maxZ = $z;
					$maxZx = $x; 
					$maxZy = $y; 
					$maxZz = $z;
				}	
			
			} // if line is not a "v" vertex record do nothing.
    	}
	} else {
    	// error opening the file.
	} 
	fclose($handle);
	
	$XDiagonal = distance3D($maxXx, $maxXy, $maxXz, $minXx,  $minXy,  $minXz);
	$YDiagonal = distance3D($maxYx, $maxYy, $maxYz, $minYx,  $minYy,  $minYz);
	$ZDiagonal = distance3D($maxZx, $maxZy, $maxZz, $minZx,  $minZy,  $minZz);
	$XDiameter =  $maxXx - $minXx;
	$YDiameter =  $maxYy - $minYy;
	$ZDiameter =  $maxZz - $minZz;

 	// ********** END get 3 diagonal distances & 18 coordinates from the OBJ file  *******
 	
 	
	
	$thisModelArray["modelNumber"] 		= $modelNumber;
	$thisModelArray["scanID"] 			= $scanID;
	$thisModelArray["photosceneID"]	 	= $photosceneID;
	$thisModelArray["view3DModelURL"] 	= "$view3DModelURL";
	$thisModelArray["diffTableURL"] 	= "$diffTableURL";
	$thisModelArray["UserComment"] 		= " ";
	$thisModelArray["imageGalleryURL"] 	= "$imageGalleryURL";
	$thisModelArray["manifestURL"]	 	= "$manifestURL";
	$thisModelArray["objURL"] 			= "$objURL";
	$thisModelArray["the3dpURL"] 		= "$the3dpURL";

	$fovxArray = array();
	$diffRotationArray  = array();
	$diffElevationArray  = array();
	
	// Add all the shots	
	for ($i = 1; $i < $shotNumber; $i++) {
		$thisModelArray["shotNumber[$i]"]	= $i;
		$thisModelArray["rotation[$i]"]		= $rotation[$i];	
		$thisModelArray["elevation[$i]"]	= $elevation[$i];
		$thisModelArray["Tx[$i]"]			= $Tx[$i];
		$thisModelArray["Ty[$i]"]			= $Ty[$i];
		$thisModelArray["Tz[$i]"]			= $Tz[$i];
		$thisModelArray["Rx[$i]"]			= $Rx[$i];
		$thisModelArray["Ry[$i]"]			= $Ry[$i];
		$thisModelArray["Rz[$i]"]			= $Rz[$i];
		$thisModelArray["fovx[$i]"]			= $fovx[$i];
		$thisModelArray["diffRotation[$i]"]		= RotDiff($rotation[$i],$Rz[$i]);
		$thisModelArray["diffElevation[$i]"]	= $elevation[$i] - $Rx[$i];
		$fovxArray["$i"] = $thisModelArray["fovx[$i]"];
		$diffRotationArray["$i"] = $thisModelArray["diffRotation[$i]"];
		$diffElevationArray["$i"] = $thisModelArray["diffElevation[$i]"];
		
	}
	
	$thisModelArray["XDiagonal"]		= $XDiagonal;
	$thisModelArray["YDiagonal"]		= $YDiagonal;
	$thisModelArray["ZDiagonal"]		= $ZDiagonal;
	
	$thisModelArray["XDiameter"]		= $XDiameter;
	$thisModelArray["YDiameter"]		= $YDiameter;
	$thisModelArray["ZDiameter"]		= $ZDiameter;
	
	/*  
	$thisModelArray["maxXx"]			= $maxXx;
	$thisModelArray["maxXy"]			= $maxXy;
	$thisModelArray["maxXz"]			= $maxXz;
	$thisModelArray["minXx"]			= $minXx;
	$thisModelArray["minXy"]			= $minXy;
	$thisModelArray["minXz"]			= $minXz;
	
	$thisModelArray["maxYx"]			= $maxYx;
	$thisModelArray["maxYy"]			= $maxYy;
	$thisModelArray["maxYz"]			= $maxYz;
	$thisModelArray["minYx"]			= $minYx;
	$thisModelArray["minYy"]			= $minYy;
	$thisModelArray["minYz"]			= $minYz;
	
	$thisModelArray["maxZx"]			= $maxZx;
	$thisModelArray["maxZy"]			= $maxZy;
	$thisModelArray["maxZz"]			= $maxZz;
	$thisModelArray["minZx"]			= $minZx;
	$thisModelArray["minZy"]			= $minZy;
	$thisModelArray["minZz"]			= $minZz;
	*/
	
	$thisModelArray["maxFOVx"]			= max($fovxArray);
	$thisModelArray["minFOVx"]			= min($fovxArray);
	$thisModelArray["meanFOVx"]			= calculate_average($fovxArray);
	$thisModelArray["medianFOVx"]		= calculate_median($fovxArray);
	$thisModelArray["spreadFOVx"]		= max($fovxArray)-min($fovxArray);
	$thisModelArray["stddevFOVx"]		= standard_deviation($fovxArray);

	$thisModelArray["maxdiffRotation"]			= max($diffRotationArray);
	$thisModelArray["mindiffRotation"]			= min($diffRotationArray);
	$thisModelArray["meandiffRotation"]			= calculate_average($diffRotationArray);
	$thisModelArray["mediandiffRotation"]		= calculate_median($diffRotationArray);
	$thisModelArray["spreaddiffRotation"]		= max($diffRotationArray) - min($diffRotationArray);
	$thisModelArray["stddevdiffRotation"]		= standard_deviation($diffRotationArray);
	
	$thisModelArray["maxdiffElevation"]			= max($diffElevationArray);
	$thisModelArray["mindiffElevation"]			= min($diffElevationArray);
	$thisModelArray["meandiffElevation"]		= calculate_average($diffElevationArray);
	$thisModelArray["mediandiffElevation"]		= calculate_median($diffElevationArray);
	$thisModelArray["spreaddiffElevation"]		= max($diffElevationArray) - min($diffElevationArray);
	$thisModelArray["stddevdiffElevation"]		= standard_deviation($diffElevationArray);
	
	
	//$thisModelArray["EOL"]				= "\n";
	//print_r($thisModelArray);
	
	// Save thisModelArray as an element in the modelMetrics array;
	$modelMetrics["$scanID"] = $thisModelArray;
	
	$modelNumber++;
}

// add the headers

foreach ($thisModelArray as $key =>  $value) {
	$thisHeaderArray[$key] = $key;
}

$modelMetrics[" label"] = $thisHeaderArray;


function flipDiagonally($arr) {
    $out = array();
    foreach ($arr as $key => $subarr) {
    	foreach ($subarr as $subkey => $subvalue) {
    		$out[$subkey][$key] = $subvalue;
    	}
    }
    return $out;
}

$transposedMetrics =  flipDiagonally($modelMetrics);


//add summary statistics to each row
$textlines = array(	"scanID", "photosceneID", "view3DModelURL", "diffTableURL",
					"UserComment","imageGalleryURL","manifestURL","objURL","the3dpURL");
foreach ($transposedMetrics as $rowTitle => $rowValueArray) {
	$newrowValueArray = $rowValueArray;
	if ($rowTitle == "modelNumber") {
		$newrowValueArray["max"] = "max";
		$newrowValueArray["min"] = "min";
		$newrowValueArray["mean"] = "mean";
		$newrowValueArray["median"] = "median";
		$newrowValueArray["spread"] = "spread";
		$newrowValueArray["std dev"] = "std dev";
		$newrowValueArray["title"] = "title";
	} else if (in_array($rowTitle, $textlines)) {
		$newrowValueArray["max"] = "";
		$newrowValueArray["min"] = "";
		$newrowValueArray["mean"] = "";
		$newrowValueArray["median"] = "";
		$newrowValueArray["spread"] = "";
		$newrowValueArray["std dev"] = "";
		$newrowValueArray["title"] = "";
	} 
	else {
		array_shift($rowValueArray);  // toss the label for calculating metrics
		$newrowValueArray["max"] = max($rowValueArray);
		$newrowValueArray["min"] = min($rowValueArray);
		$newrowValueArray["mean"] = calculate_average($rowValueArray);
		$newrowValueArray["median"] = calculate_median($rowValueArray);
		$newrowValueArray["spread"] = max($rowValueArray) - min($rowValueArray);
		$newrowValueArray["std dev"] = standard_deviation($rowValueArray);
		$newrowValueArray["title"] = $rowTitle;
	} 
	$newtransposedMetrics[$rowTitle] = $newrowValueArray;
}	
	


$csvFileName = "$scaletestsDirPath/". urlencode($pattern).".csv";
$filepointer = fopen($csvFileName,'w');

$heading = true;
foreach ($newtransposedMetrics as $fields) {
	fputcsv($filepointer, $fields);
}

fclose($filepointer);
echo "CSV file = \n$scaletestsDirURL". urlencode($pattern).".csv";

// iterate over the $modelMetrics array  and output a CSV file 
//	for each element in $modelMetrics  create a new LINE,
//	within each line iterate over the internal array adding the value, a comma and a tab
// between elements.
// add summary statistics lines to the CSV
// Max, Min, Average, Median, Spread, Std Dev.


echo "\n------- END metricsForReference3DPs3.php ------------------------------\n\n";

?>