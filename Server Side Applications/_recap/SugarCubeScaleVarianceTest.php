<?php

function distance3D ($tuple1, $tuple2) {
	$x1 = $tuple1[0];
	$y1 = $tuple1[1];
	$z1 = $tuple1[2];
	$x2 = $tuple2[0];
	$y2 = $tuple2[1];
	$z2 = $tuple2[2];
	$distanceSquared = pow($x2 - $x1, 2) + pow($y2 - $y1, 2) + pow($z2 - $z1, 2);
	$distance = sqrt($distanceSquared);
	// echo "x1 = $x1, y1 = $y1, z1 = $z1,\n";
	// echo "x2 = $x2, y2 = $y2, z2 = $z2\n";
	// echo "distanceSquared = $distanceSquared, distance = $distance\n\n";
	return $distance;
}

$fileCounter = 0;
$filePrefix = "SLM-u8b3r9c-slmss140521-CubeForVarTests*";
$csvFile = "/home/earchivevps/test.soundfit.me/Scans/b3/scaletest.csv";

$modelDirArray = glob("/home/earchivevps/test.soundfit.me/Scans/b3/modeled/". $filePrefix);

// print_r ($modelDirArray);

foreach ($modelDirArray as $modelDir) {

	$maxX = $minX = $maxY = $minY = $maxZ = $minZ = 0; 
	$maxXminXdistance = $maxYminYdistance = $maxZminZdistance = 0;
	$maxXmaxYdistance = $minXmaxYdistance = $maxXminYdistance = 0;
	$minXminYdistance = $maxXmaxZdistance = $minXmaxZdistance = 0;
	$maxXminZdistance = $minXminZdistance = $maxZmaxYdistance = 0;
	$minZmaxYdistance = $maxZminYdistance = $minZminYdistance = 0;
	
	$scanID = basename($modelDir);
	$recordArray = file($modelDir."/mesh.obj");
	
	foreach ($recordArray as $objRecord) {
	
		$tokens =  explode(" ",$objRecord);
			
		if ( $tokens[0] == "v" ) {
		
			$x = $tokens[1];
			$y = $tokens[2];
			$z = $tokens[3];
			
			if ($x < $minX) { 
				$minX = $x;
				$minXtuple = array ($x, $y, $z);
			} else if ($x > $maxX) { 
				$maxX = $x;
				$maxXtuple = array ($x, $y, $z);
			}
			
			if ($y < $minY) { 
				$minY = $y;
				$minYtuple = array ($x, $y, $z);
			} else if ($y > $maxY) { 
				$maxY = $y;
				$maxYtuple = array ($x, $y, $z);
			}
			
			if ($z < $minZ) { 
				$minZ = $Z;
				$minZtuple = array ($x, $y, $z);
			} else if ($z > $maxZ) { 
				$maxZ = $z;
				$maxZtuple = array ($x, $y, $z);
			}	
			
		} // if line is not a "v" vertex record do nothing.
	
	}  //  end foreach ($recordArray as $objRecord)
	
	$maxXminXdistance = distance3D($maxXtuple, $minXtuple);	
	$maxYminYdistance = distance3D($maxYtuple, $minYtuple);
	$maxZminZdistance = distance3D($maxZtuple, $minZtuple);
	
	$maxXmaxYdistance = distance3D($maxXtuple, $maxYtuple);
	$minXmaxYdistance = distance3D($minXtuple, $maxYtuple);
	$maxXminYdistance = distance3D($maxXtuple, $minYtuple);
	$minXminYdistance = distance3D($minXtuple, $minYtuple);
	
	$maxXmaxZdistance = distance3D($maxXtuple, $maxZtuple);
	$minXmaxZdistance = distance3D($minXtuple, $maxZtuple);
	$maxXminZdistance = distance3D($maxXtuple, $minZtuple);
	$minXminZdistance = distance3D($minXtuple, $minZtuple);
	
	$maxZmaxYdistance = distance3D($maxZtuple, $maxYtuple);
	$minZmaxYdistance = distance3D($minZtuple, $maxYtuple);
	$maxZminYdistance = distance3D($maxZtuple, $minYtuple);
	$minZminYdistance = distance3D($minZtuple, $minYtuple);
	
	
	$minXX = $minXtuple[0]; $minXY = $minXtuple[1]; $minXZ = $minXtuple[2]; 
	$maxXX = $minXtuple[0]; $maxXY = $minXtuple[1]; $maxXZ = $minXtuple[2];
	
	$minYX = $minXtuple[0]; $minYY = $minXtuple[1]; $minYZ = $minXtuple[2]; 
	$maxYX = $minXtuple[0]; $maxYY = $minXtuple[1]; $maxYZ = $minXtuple[2];
	
	$minZX = $minXtuple[0]; $minZY = $minXtuple[1]; $minZZ = $minXtuple[2]; 
	$maxZX = $minXtuple[0]; $maxZY = $minXtuple[1]; $maxZZ = $minXtuple[2];
		

		
		
	$pointsArray[$fileCounter] = array( 
		$fileCounter, 
		$scanID,		
		$minXX,
		$minXY,
		$minXZ, 
		$maxXX, 
		$maxXY, 
		$maxXZ, 	
		$minYX,
		$minYY,
		$minYZ,
		$maxYX,
		$maxYY,
		$maxYZ,
		$minZX,
		$minZY,
		$minZZ, 
		$maxZX,
		$maxZY,
		$maxZZ,
		$maxXminXdistance,
		$maxYminYdistance,
		$maxZminZdistance,
		$maxXmaxYdistance,
		$minXmaxYdistance,
		$maxXminYdistance,
		$minXminYdistance,
		$maxXmaxZdistance,
		$minXmaxZdistance,
		$maxXminZdistance,
		$minXminZdistance,
		$maxZmaxYdistance,
		$minZmaxYdistance,
		$maxZminYdistance,
		$minZminYdistance	
	);
	
	echo
		"minXX=$minXX," .
		"minXY=$minXY," .
		"minXZ=$minXZ," . 
		"maxXX=$maxXX," . 
		"maxXY=$maxXY," . 
		"maxXZ=$maxXZ," . 	
		"minYX=$minYX," .
		"$minYY," .
		"$minYZ," .
		"$maxYX," .
		"$maxYY," .
		"$maxYZ," .
		"$minZX," .
		"$minZY," .
		"$minZZ," . 
		"$maxZX," .
		"$maxZY," .
		"$maxZZ," .
		"$maxXminXdistance," .
		"$maxYminYdistance," .
		"$maxZminZdistance," .
		"$maxXmaxYdistance," .
		"$minXmaxYdistance," .
		"$maxXminYdistance," .
		"$minXminYdistance," .
		"$maxXmaxZdistance," .
		"$minXmaxZdistance," .
		"$maxXminZdistance," .
		"$minXminZdistance," .
		"$maxZmaxYdistance," .
		"$minZmaxYdistance," .
		"$maxZminYdistance," .
		"$minZminYdistance";	
	
	$csvLine[$fileCounter] = 
		"minXX=$minXX," .
		"minXY=$minXY," .
		"$minXZ," . 
		"$maxXX," . 
		"$maxXY," . 
		"$maxXZ," . 	
		"$minYX," .
		"$minYY," .
		"$minYZ," .
		"$maxYX," .
		"$maxYY," .
		"$maxYZ," .
		"$minZX," .
		"$minZY," .
		"$minZZ," . 
		"$maxZX," .
		"$maxZY," .
		"$maxZZ," .
		"$maxXminXdistance," .
		"$maxYminYdistance," .
		"$maxZminZdistance," .
		"$maxXmaxYdistance," .
		"$minXmaxYdistance," .
		"$maxXminYdistance," .
		"$minXminYdistance," .
		"$maxXmaxZdistance," .
		"$minXmaxZdistance," .
		"$maxXminZdistance," .
		"$minXminZdistance," .
		"$maxZmaxYdistance," .
		"$minZmaxYdistance," .
		"$maxZminYdistance," .
		"$minZminYdistance";
	//echo "$csvLine[$fileCounter]\n";
	$csvFields = explode("\n",$csvLine[$fileCounter]);
	$metrics = implode("",$csvFields);
	$theString = "$metrics,$fileCounter,$scanID";
	echo "\n$theString\n";
	$csvString[$fileCounter] = "$fileCounter,$scanID,$metrics";
	// echo "\n$fileCounter, $scanID,\n$metrics \n";
	// echo "$csvString[$fileCounter]\n";
	// $csvLine[$fileCounter] = implode(",",$pointsArray[$fileCounter]) . "\n";
	
	$fileCounter++;
	
} // end foreach ($objFileArray as $objFile)

$csvfilebuffer = implode ("\n", $csvString );
	
file_put_contents($csvFile, $csvfilebuffer);
	
// print_r ($csvfilebuffer);	 

echo "\n\nDone";

?>