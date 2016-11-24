<?php

// changeBasis.php   
// Copyright 2014 by  Scott L. McGregor, SoundFit, LLC
// July 3, 2014
//
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

// Test code

$A = new Point(1,2,3);
echo "A = ".$A->printArray()."<br \>";
$newOffset = new Point(4,8,17);
$noOffset = new Point(0,0,0);
$originalAngles = new Point(0,0,0);
$noScale =  new Point(1,1,1);

$movedA = $A->changeBasis($newOffset,$originalAngles,$noScale);
echo "move (add) ".$newOffset->printArray()."<br />";
echo "movedA = ".$movedA->printArray()." - Should be ( 5, 10, 20 ) <br \>=======<br \><br \>";

echo "movedA = ".$movedA->printArray()."<br />";
$newScale = new Point(4,2,1);
echo "scale by (multiply) ".$newScale->printArray()."<br />";
$scaledA = $movedA->changeBasis($noOffset,$originalAngles,$newScale);
echo "scaledA = ".$scaledA->printArray()." - Should be ( 20, 20, 20 ) <br \>=======<br \><br \>";

echo "scaledA = ".$scaledA->printArray()."<br />";
$newAngles = new Point(180,0,0);
echo "rotate by ".$newAngles->printArray()."<br />";
$turnedAx = $scaledA->changeBasis($noOffset,$newAngles,$noScale);
echo "turnedAx = ".$turnedAx->printArray()." - Should be ( 20,  -20, -20 ) <br \>=======<br \><br \>";

echo "turnedAx = ".$turnedAx->printArray()."<br />";
$newAngles = new Point(0,180,0);
echo "rotate by ".$newAngles->printArray()."<br />";
$turnedAy = $turnedAx->changeBasis($noOffset,$newAngles,$noScale);
echo "turnedAy = ".$turnedAy->printArray()." - Should be ( -20,  -20, 20 ) <br \>=======<br \><br \>";


echo "turnedAy = ".$turnedAy->printArray()."<br />";
$newAngles = new Point(0,0,180);
echo "rotate by ".$newAngles->printArray()."<br />";
$turnedAz = $turnedAy->changeBasis($noOffset,$newAngles,$noScale);
echo "turnedAz = ".$turnedAz->printArray()." - Should be ( 20,  20, 20 ) <br \>=======<br \><br \>";



exit;

?>