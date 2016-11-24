<?php

function distance3D ($x1, $y1, $z1, $x2, $y2, $z2) {
	$distanceSquared = pow($x2 - $x1, 2) + pow($y2 - $y1, 2) + pow($z2 - $z1, 2);
	$distance = sqrt($distanceSquared);
	// echo "x1 = $x1, y1 = $y1, z1 = $z1,\n";
	// echo "x2 = $x2, y2 = $y2, z2 = $z2\n";
	// echo "distanceSquared = $distanceSquared, distance = $distance\n\n";
	return $distance;
}

if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}

$filePrefix = htmlspecialchars($_GET["scanIDprefix"]);
if ($filePrefix == null ) $filePrefix = "SLM-u8-Sstr201405270648-RefCubeVarTest4-";
echo "filePrefix = $filePrefix\n";

$csvFile = "/home/earchivevps/test.soundfit.me/Scans/b3/scaletests/$filePrefix-v3.csv";
echo "csvFile = $csvFile\n";

$modelDirArray = glob("/home/earchivevps/test.soundfit.me/Scans/b3/modeled/". $filePrefix . "*");

$fileCounter = 1;
$csvLine[0] = 
		"fileCounter," .
		"scanID," .
		"photosceneID," .
		
		"maxX-minXdistance," .
		"maxY-minYdistance," .
		"maxZ-minZdistance," .
		
		"minX(x--)," .
		"maxX(x--)," .
		"minY(-y-)," .
		"maxY(-y-)," .
		"maxZ(--z)," .
		"minZ(--z)," .
		
		"minX(-y-)," .  
		"minX(--z)," . 
		"maxX(-y-)," . 
		"maxX(--z)," . 	
		"minY(x--)," .
		"minY(-y-)," .
		"minY(--z)," .
		"maxY(x--)," .
		"maxY(-y-)," .
		"maxY(--z)," .
		"minZ(x--)," .
		"minZ(-y-)," . 
		"maxZ(x--)," .
		"maxZ(-y-)," .

		"maxX-maxYdistance," .
		"minX-maxYdistance," .
		"maxX-minYdistance," .
		"minX-minYdistance," .
		"maxX-maxZdistance," .
		"minX-maxZdistance," .
		"maxX-minZdistance," .
		"minX-minZdistance," .
		"maxZ-maxYdistance," .
		"minZ-maxYdistance," .
		"maxZ-minYdistance," .
		"minZ-minYdistance" 
		;
		


// print_r ($modelDirArray);

foreach ($modelDirArray as $modelDir) {

	$maxX = $minX = $maxY = $minY = $maxZ = $minZ = 0; 
	$maxXminXdistance = $maxYminYdistance = $maxZminZdistance = 0;
	$maxXmaxYdistance = $minXmaxYdistance = $maxXminYdistance = 0;
	$minXminYdistance = $maxXmaxZdistance = $minXmaxZdistance = 0;
	$maxXminZdistance = $minXminZdistance = $maxZmaxYdistance = 0;
	$minZmaxYdistance = $maxZminYdistance = $minZminYdistance = 0;
	
	$scanID = basename($modelDir);
	$photosceneID = file_get_contents("/home/earchivevps/test.soundfit.me/Scans/b3/maps/$scanID");
	$recordArray = file($modelDir."/mesh.obj");
	
	foreach ($recordArray as $objRecord) {
	
		$tokens =  explode(" ",$objRecord);
			
		if ( $tokens[0] == "v" ) {
		
			$x = $tokens[1];
			$y = $tokens[2];
			$z = trim($tokens[3]);
			
			if ($x < $minX) { 
				$minX  = $x;
				$minXX = $x; 
				$minXY = $y; 
				$minXZ = $z;
				$minXtuple = array ($x, $y, $z);
			} else if ($x > $maxX) { 
				$maxX  = $x;
				$maxXX = $x; 
				$maxXY = $y; 
				$maxXZ = $z;
				$maxXtuple = array ($x, $y, $z);
			}
			
			if ($y < $minY) { 
				$minY  = $y;
				$minYX = $x; 
				$minYY = $y; 
				$minYZ = $z;
				$minYtuple = array ($x, $y, $z);
			} else if ($y > $maxY) { 
				$maxY  = $y;
				$maxYX = $x; 
				$maxYY = $y; 
				$maxYZ = $z;
				$maxYtuple = array ($x, $y, $z);
			}
			
			if ($z < $minZ) { 
				$minZ  = $z;
				$minZX = $x; 
				$minZY = $y; 
				$minZZ = $z;
				$minZtuple = array ($x, $y, $z);
			} else if ($z > $maxZ) { 
				$maxZ = $z;
				$maxZX = $x; 
				$maxZY = $y; 
				$maxZZ = $z;
				$maxZtuple = array ($x, $y, $z);
			}	
			
		} // if line is not a "v" vertex record do nothing.
	
	}  //  end foreach ($recordArray as $objRecord)
	
	$maxXminXdistance = distance3D($maxXX, $maxXY, $maxXZ, $minXX,  $minXY,  $minXZ);
	$maxYminYdistance = distance3D($maxYX, $maxYY, $maxYZ, $minYX,  $minYY,  $minYZ);
	$maxZminZdistance = distance3D($maxZX, $maxZY, $maxZZ, $minZX,  $minZY,  $minZZ);
	
	$maxXmaxYdistance = distance3D($maxXX, $maxXY, $maxXZ, $maxYX,  $maxYY, $maxYZ);
	$minXmaxYdistance = distance3D($minXX, $minXY, $minXZ, $maxYX,  $maxYY, $maxYZ);
	$maxXminYdistance = distance3D($maxXX, $maxXY, $maxXZ, $minYX,  $minYY, $minYZ);
	$minXminYdistance = distance3D($minXX, $minXY, $minXZ, $minYX,  $minYY, $minYZ);
	
	$maxXmaxZdistance = distance3D($maxXX, $maxXY, $maxXZ, $maxZX,  $maxZY, $maxZZ);
	$minXmaxZdistance = distance3D($minXX, $minXY, $minXZ, $maxZX,  $maxZY, $maxZZ);
	$maxXminZdistance = distance3D($maxXX, $maxXY, $maxXZ, $minZX,  $minZY, $minZZ);
	$minXminZdistance = distance3D($minXX, $minXY, $minXZ, $minZX,  $minZY, $minZZ);
	
	$maxZmaxYdistance = distance3D($maxZX, $maxZY, $maxZZ, $maxYX,  $maxYY, $maxYZ);
	$minZmaxYdistance = distance3D($minZX, $minZY, $minZZ, $maxYX,  $maxYY, $maxYZ);
	$maxZminYdistance = distance3D($maxZX, $maxZY, $maxZZ, $minYX,  $minYY, $minYZ);
	$minZminYdistance = distance3D($minZX, $minZY, $minZZ, $minYX,  $minYY, $minYZ);
	
	$csvLine[$fileCounter] = 
		"$fileCounter," .
		"$scanID," .
		"$photosceneID," .
		"$maxXminXdistance," .
		"$maxYminYdistance," .
		"$maxZminZdistance," .
		"$minXX," .
		"$maxXX," .
		"$minYY," .
		"$maxYY," .
		"$maxZZ," .
		"$minZZ," .	
		"$minXY," .  
		"$minXZ," . 
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
		"$maxZX," .
		"$maxZY," .
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
		"$minZminYdistance" 
		;
		
	
	// echo "$csvLine[$fileCounter]\n";
	
	$fileCounter++;
	
} // end foreach ($objFileArray as $objFile)

$csvfilebuffer = implode ("\n", $csvLine );
	
file_put_contents($csvFile, $csvfilebuffer);
	
echo "csvfilebuffer=$csvfilebuffer\n";	 

?>