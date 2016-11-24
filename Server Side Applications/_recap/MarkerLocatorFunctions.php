<?php

// pseudo code for finding center of a locator-marker point, and generating the proper 
// template statements.

// target locator positions:  US penny stacks 1, 2, 3 and 4 pennies high 
// N (0, -20, 1.52),  W (-20,0, 3.04), S (0,20, 4.56),  W (20,0, 6.08)




//----------------------- DEFINE THE MARKER/LOCATOR POSITIONS ----------------------------

// Identify the calibration locators using 4 locators in 5 shots, at 12 different marker positions

// define the markers in the overhead shot (shot #1)
//                        (     x,     y,     z, LID,  LName, ShotID, Shotname, width, height, hash,      w,    h	)								
$m[1] = new MarkerLocator (   0.0, -35.0,	-17.0, 1,   "frontPt", 1, "zenith.jpg", 2048, 1536, "hash1", 75.0, 50.0	);  
$m[2] = new MarkerLocator (   0.0,  35.0,	-15.0, 2,   "backPt",  1, "zenith.jpg", 2048, 1536, "hash1", 25.0, 50.0	); 
$m[3] = new MarkerLocator ( -35.0,   0.0,	-12.0, 3,   "leftPt",  1, "zenith.jpg", 2048, 1536, "hash1", 50.0, 25.0	);  
$m[4] = new MarkerLocator (  35.0,   0.0,	 -8.0, 4,   "rightPt", 1, "zenith.jpg", 2048, 1536, "hash1", 50.0, 75.0	);

// define the markers visible in the left rear at equator  shot  (shot #2) 
$m[5] = new MarkerLocator (   0.0,  35.0,	-15.0, 2,   "backPt",  2, "leftrear.jpg", 2048, 1536, "hash2", 25.0, 30.0	); 
$m[6] = new MarkerLocator ( -35.0,   0.0,	-12.0, 3,   "leftPt",  2, "leftrear.jpg", 2048, 1536, "hash2", 75.0, 40.0	);

// define the markers visible in the left front at equator  shot  (shot #3) 
$m[7] = new MarkerLocator ( -35.0,   0.0,	-12.0, 3,   "leftPt",  3, "leftfront.jpg", 2048, 1536, "hash3", 25.0, 40.0	);
$m[8] = new MarkerLocator (   0.0, -35.0,	-17.0, 1,   "frontPt", 3, "leftfront.jpg", 2048, 1536, "hash3", 75.0, 20.0	);

// define the markers visible in the right front at equator  shot  (shot #4) 
$m[9] = new MarkerLocator (   0.0, -35.0,	-17.0, 1,   "frontPt", 4, "rightfront.jpg", 2048, 1536, "hash4", 25.0, 20.0	);
$m[10] = new MarkerLocator(  35.0,   0.0,	 -8.0, 4,   "rightPt", 4, "rightfront.jpg", 2048, 1536, "hash4", 75.0, 50.0	);

// define the markers visible in the left rear at equator  shot  (shot #5)   
$m[11] = new MarkerLocator(   0.0,  35.0,	-15.0, 2,   "backPt",  5, "rightrear.jpg", 2048, 1536, "hash5", 25.0, 30.0	); 
$m[12] = new MarkerLocator( -35.0,   0.0,	-12.0, 3,   "leftPt",  5, "rightrear.jpg", 2048, 1536, "hash5", 75.0, 50.0	);  

//----------------------- END OF MARKER/LOCATOR POSITION DEFINITIONS  --------------------

	

//--------------------- MAIN PROGRAM: PRINT THE RZML TEMPLATE RECORDS  -------------------
  						
// Now lets print out the RZML records
// We can get the full arrays of Locators, Prels, and Shots from the Global Variables

echo "<RZML>\n";

foreach ($locators as $locator)
{
	echo "\t". $locator->printTag() . "\n";
}

foreach ($prels as $prel)
{
	echo "\t". $prel->printTag() . "\n";
}

foreach ($shots as $shot)
{
	echo "\t". $shot->printTag() . "\n";
}

echo "</RZML>\n";


die("end of test code.\n");
    								
// ------  FUNCTIONS FOR FINDING MARKERS WITHIN AN IMAGE  -------------------------------



function getPixelColor($image, $current_x, $current_y)
{
	// get the color of the pixel in the image at the indicated x and y
}
 

function find_locators_in_field_of_view ( ) //	$camera->T->x,  $camera->T->y, $camera->T->z,  // where the camera is
  											 // $camera->R->x,  $camera->R->y, $camera->R->z,  // where it is pointing
  											 // $fovx,  $fovy  )
{
  		// RETURN an ARRAY of locator ids and EXPECTED Marker (x,y) locations 
  		// that should be visible in field of view
}		

function lookElsewhere($current_x, $current_y, $counter)
{
	// Break the image into a 3x3 matrix,  starting at (2,2) keep changing the center with
	// each counter  so you traverse (2,2) -> (3, 2) -> (3,3) -> (2, 3) -> (1,3) -> (2,1)
	// (1,1) -> (1,2) -> (1,3).    
	// if we get to all 9 cells and still don't find an inside, give up
	// RETURN (new_current_x, new_current_y, "stop")
	// else
	// RETURN  (new_current_x, new_current_y, "tryAgain")
}	

function find_locator(	$image, $width, $height, $diameter, 
						$innerColor, $edgeColor, $centerColor,
						$zone_x, $zone_y )  
{

	// Given we have an image, "$image", as a 2D array of pixels.
	// of dimensions "$width" x "$height"
	// Within that image we are seeking a marker point.
	//
	// The marker consists of a circle ($innerColor) with a contrasting edge ($boundarycolor) 
	// and center dot ($centercolor).
	//
	// Let the $diameter be an odd number of pixels in diameter so that the center is a pixel.
	// E.g. 99 pixels in diameter, with  $center = 1+ ($diameter-1)/2  =  50
	//
	// Assume we have a square search zone in which we are looking for this circle.
	// Let the width,  $zonewidth = $zoneheight = 3*$diameter.
	// Let the xy coordinates of the center of the search zone ($zone_x , $zone_y)
	// be calculated relative to the upper left of the image.

	// To find the center, 
	$current_x = $zone_x;
	$current_y = $zone_y;
	$currentPT  = array ($current_x, $current_y);
	$centerPT = null;
	$centerFound = false;
	$cannotFind = false;
	$counter = 0;
	
	while ( ($centerFound == false) and ($cannotFind == false) )
	{
		$pixelColor = getPixelColor($image,$current_x, $current_y);
	
// 		if (nearMatch($centerColor, $pixelColor) == true) $centerPt = checkCenter($current_x, $current_y);
//		else if (nearMatch($edgeColor, $pixelColor) == true) $centerPt = checkEdge($current_x, $current_y);
//		else if (nearMatch($innerColor, $pixelColor) == true) $centerPt = checkInner(($current_x, $current_y, $innerColor);
//		else $centerPT = null); // advance the counter and look again		

		if ($centerPT == null) 
		{  	// center point not found 
			// ($current_x, $current_y) = lookElsewhere($current_x, $current_y,$counter); // this moves ($current_x, $current_y) for next search
			$counter = $counter + 1;
		} else
		{  	// center point was found! return it 
			// RETURNS a center point ($locator->x_percent, $locator->y_percent) in terms of 
			// a percent of the $width and $height of the image, with an origin at the upper left.
			$centerFound == true;
			$locator->x_percent = 100 * ($centerPT->x / $width);  // This is a FLOATING POINT percentage
			$locator->y_percent = 100 * ($centerPT->y / $height);  // This is a FLOATING POINT percentage
			return $centerPT;
		}
	
		if ($counter > MaxSearchLocations) {
			$cannotFind == true;  // if we can't find it after we have checked every space, give up
			return null;
		}

	}
	//
	// RETURNS null if locator point center was not found
}


function nearMatch($color1, $color2)
{
// RETURNS boolean "true" if the colors match within an acceptable range of values, 
// RETURNS false otherwise.
}



function checkCenter($current_x, $current_y) 
{
	// check the pixel colors all around the current pixel.  If they are all insideColor pixels
	// and the current pixel is centerColor, then we are probably at the center now
	// set state=center and 
	// return (center_x, center_y, "center" ) 
	// else set state=inside and return an inside pixel starting point 
	// return (current_x+1, current_y+1, "inside") 
}

function checkEdge($current_x, $current_y)
{
	// move down until you are "inside" or hit the end of the zone.
	// if you hit the zone end, go up from center to top end of zone.
	// if you don't hit the inside up or down, try first right, then left
	// if you don't find the inside in any of these trips,
	// return ($current_x, $current_y, "lookElsewhere")
	// if you do find an inside, call the checkInner() function and return its value.
}


//------------- CHECKINNER FUNCTION ------------------------------------------------------

function checkInner($current_x, $current_y, $image, $innerColor)
{
	// this is the main function that finds the center of the ellipse that is
	// the reference marker. 
	// It will be a circle if the marker is a physical 3D sphere.
	// It will be an ellipse if it is just a circle on a wall or floor.

	
    // --- Step 1: Draw a chord somewhere across the ellipse.  ------

	$x = current_x;
	$y = current_y;
	
	// We are already inside the ellipse, so just walk RIGHT until we hit an edge.
	while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
	{
		$x++;  // move RIGHT one pixel
	}
	$chord1endx= $x-1; $chord1endy = $y;
	
	// Now go back to the starting point and walk LEFT until we hit an edge.  
	$x = current_x;
	while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
	{
		$x--;  // move LEFT one pixel
	}
	$chord1startx= $x+1; $chord1endy = $y;
	// Now we have the first chord.
	

    // ---- Step 2: Draw a second chord perpendicular to the first -------
    // so that the two chords intersect somewhere inside the ellipse. 
    // Ideally, they should intersect close to the border of the ellipse.
    
    $x = $x+5;  // step back a few pixels from the most recent edge.
    // Now move DOWN until we hit an edge,
    while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
    {
		$y++;  // move DOWN one pixel
	}
	$chord2endx= $x; $chord2endy = $y-1;
	while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
	{
		$y--;  // move UP one pixel
	}
	$chord2startx= $x; $chord2endy = $y+1;
	// Now we have the second chord.    
	

	// ------ Step 3: Draw a third chord perpendicular to the second (and parallel to the first),  ------
	// again making sure that the two intersect within the ellipse.
	
	// Move DOWN the same number of pixels from the most recent edge, 
	$y = $y+5;  
	
	// and then move LEFT until we find an edge.
	while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
	{
		$x--;  // move LEFT one pixel
	}
	$chord3endx= $x-1; $chord3endy = $y;
	
	// Now go back to the starting point and walk RIGHT until we hit an edge.  
	$x = current_x;
	while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
	{
		$x--;  // move RIGHT one pixel
	}
	$chord3startx= $x+1; $chord3endy = $y;
	// Now we have the 3rd chord.


	// ----- Step 4: Draw a fourth chord perpendicular to the third (and parallel to the second) ----
	// so that they intersect within the ellipse. 
	
	// Again move a certain number of pixels rightward from the edge and then start moving
	// up and down until we find edges.  
	$x = $x-5;  // step back a few pixels from the most recent edge.
    // Now move DOWN until we hit an edge,
    while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
    {
		$y++;  // move DOWN one pixel
	}
	$chord4endx= $x; $chord4endy = $y-1;
	while ( nearmatch(getPixelColor($image,$x, $y), $innerColor) )
	{
		$y--;  // move UP one pixel
	}
	$chord4startx= $x; $chord4endy = $y+1;
	
	//  After completing Steps 1 through 4 you should have four line segments 
	// (two pairs of two parallel lines) that form a rectangle inside the ellipse.
	// Calculate the center point in FLOATING POINT to be more precise than a single pixel.
	

	//  ------- Step 5: Calculate the midpoints of each of the four lines. --------
	
	//	(x1 + x2)/2 for horizontal lines  and  (y1 -y2)/2 for vertical lines
	$midpt1x = ($chord1endx + $chord1startx ) / 2; $midpt1y = $chord1endy;
	$midpt3x = ($chord3endx + $chord3startx ) / 2; $midpt3y = $chord3endy;
	$midpt2y = ($chord2endy + $chord2starty ) / 2; $midpt2x = $chord2endx;
	$midpt4y = ($chord4endy + $chord4starty ) / 2; $midpt4x = $chord4endx;	

	// Step 6: Draw two line segments, each connecting a midpoint to its midpoint friend 
	// on the parallel line. Where these segments intersect is the center of the ellipse.
	// calculate the center point in FLOATING POINT to be more precise than a single pixel.
	
	$centerpointx = ($midpt1x + $midpt3x) /2;
	$centerpointy = ($midpt2y + $midpt4y) /2;
	
	// RETURN  the center as an array
	$theCenter = array( $centerpointx, $centerpointy); 
	return $theCenter;

}
//  ---- END OF CHECKINNER FUNCTION ------------------------------------------------------


exit ;


//----------------------- CLASS DEFINITIONS ---------------------------------------

// DEFINE a LOCATOR class  
// label for a 3D x,y,z location, independent of images   
class Locator
{
    // property declarations
    public $id = 0;
    public $name = "Locator";
    public $usage = "a";
    public $shots =  "";  // e.g. shotIDs separated by spaces: "15 18 38"
    
    public $prel = null;
    

    // method declarations
    public function __construct($id,$name) {
    	$this->id = $id;
    	$this->name = $name;
    	$this->usage = "a";
    	$this->shots = "";
  	}
  	
    public function addShot($newShotID) {
        $this->shots = trim($this->shots . " $newShotID");
        return $this->shots;
    }
    
    public function setPrel($prel) {
  		$this->prel =  $prel;
  	}
    
    public function printTag() {
        $LTag = '<L i="'.$this->id.'" n="'.$this->name.'" u="'.$this->usage.'" v="'.$this->shots.'">';
        return $LTag;
    }
}



// DEFINE a PREL - POINT RELATION  class  
// a 2D x,y location and a specific image
class PRel
{
    // property declarations
    public $type = "p";
    public $locatorID = "";
    
    public $point = null;
    public $locator = null;
    

    // method declarations
    public function __construct($locatorID,$point) {
    	$this->type = "p";
    	$this->locatorID = $locatorID;
    	$this->locator = null;
    	$this->point = $point;
  	}
  	
  	public function setPoint($point) {
  		$this->point =  $point;
  	}
  	
    public function setLocator($locator) {
  		$this->locator =  $locator;
  	}
  	
    public function printTag() {
        $PRelTag = '<PREL t="'.$this->type.'" ll="'.$this->locatorID.'">' ."\n\t\t". $this->point->printTag() . "\n\t</PREL>";
        return $PRelTag;
    }
}


 
// DEFINE a POINT  class  
// a point in 3 Space
class Point
{
    // property declarations
    public $x = 0.0;
    public $y = 0.0;
    public $z = 0.0;
    
	public $prel = null;
    

    // method declarations
    public function __construct($x,$y,$z) {
    	$this->x = $x;
    	$this->y = $y;
    	$this->z = $z;
    	$this->prel =  null;
  	}
  	
  	public function setPrel($prel) {
  		$this->prel =  $prel;
  	}
  	
    public function printTag() {
        $PTag = '<P x="'.$this->x.'" y="'.$this->y.'" z="'.$this->z.'" />';
        return $PTag;
    }
}
 
 



// DEFINE a MARKER class  // a 2D x,y location and a specific image
class Marker
{
    // property declaration
    public $id = 0;
    public $kind = "p";
    public $x = 0.0;  // x position as a percent of the image width
    public $y = 0.0;  // y position as a percent of the image height
    public $z = 0.0;  // always floating point zero
    public $iFrame = null;
    

    // method declaration
     public function __construct($id,$x,$y,$z) {
     	$this->id = 0;
    	$this->kind = "p";
    	$this->x = $x;
    	$this->y = $y;
    	$this->z = $z;
    	$this->iFrame = null;
  	}
  	
  	public function setIFrame($iframe){
  		$this->iFrame = $iframe;
  	}
  	
    public function printTag() {
        $MTag = "\t\t\t\t".'<M  i="'.$this->id.'" k="'.$this->kind.'" x="'.$this->x.'" y="'.$this->y.'" z="'.$this->z.'" />';
        return $MTag;
    }
}



// DEFINE an IFrame (IFRM) -  Image Frame class
// IFrames are holders for an array of Markers that are all in the same image
class IFrame
{
    // property declaration
    public $markers = array();
    public $iPlane =  null;
    

    // method declaration
    public function __construct() {
    	$this->markers = array();
    	$this->iPlane =  null;
  	}
  	
  	public function addMarker($marker) {
  		array_push($this->markers, $marker);
  	}
  	
  	public function setIPlane($iPlane) {
  		$this->iPlane =  $iPlane;
  	}
  	
    public function printTag() {
    	$markerTags = "";
    	foreach ($this->markers as $value) {
    		$markerTags = $markerTags . $value->printTag() . "\n";
    	}
        $IFRMTag = "\n\t\t\t<IFRM>\n".$markerTags."\t\t\t</IFRM>"; 
        return $IFRMTag;
    }
}


// DEFINE an IPLN - Image Plane class
// IPlane objects hold Markers that apply to a specific image
class IPlane
{
    // property declaration
    public $imageName = "";
    public $hash = "";
    public $iframe = null;
    public $shot = null;
    

    // method declaration
    public function __construct($imageName, $hash) {
    	$this->imageName = $imageName;
    	$this->hash = $hash;
    	$this->iframe = null;
    	$this->shot = null;
  	}
  	
  	public function setiFrame($iframe) {
  		$this->iframe = $iframe;
  	}
  	
  	public function setShot($shot) {
  		$this->shot = $shot;
  	}
  	
    public function printTag() {
        $IPLNTag = '<IPLN img="'.$this->imageName.'" hash="'.$this->hash.'">'
        			. $this->iframe->printTag()
        			. "\n\t\t</IPLN>"; 
        return $IPLNTag;
    }
}


// DEFINE a  SHOT class
// this defines an image and its size.   It also allows attachment of an IPLN IPlane object
class Shot
{
    // property declaration
    public $id = 1;
    public $name = "";
    public $ci = "1";
    public $width = 0;   // image width in pixels
    public $height = 0;  // image height in pixels
    public $iPlane = null;
    

    // method declaration
    public function __construct($id,$name, $width, $height) {
    	$this->id = $id;
    	$this->name = $name;
    	$this->ci = "1";
    	$this->width = $width;
    	$this->height = $height;
    	$this->iPlane =  null;
  	}
  	
  	public function setiPlane($iplane) {
  		$this->iPlane = $iplane;
  	}
  	
    public function printTag() {
        $ShotTag = '<SHOT i="'.$this->id.'" n="'.$this->name.'" ci="'.$this->ci.'" w="'.$this->width.'" h="'.$this->height.'">'. "\n"
        				 . "\t\t" . $this->iPlane->printTag()
        				 . "\n\t</SHOT>";
        return $ShotTag;
    }
}



$points = array();
$prels  = array();
$locators =  array();
$markers = array();
$iFrames = array();
$iPlanes = array();
$shots = array();

//DEFINE a MARKERLOCATOR class
//
class MarkerLocator
{
	// property declarations
	public $x = 0.0;
	public $y = 0.0;
	public $z = 0.0; 
	public $point = null;
	
	public $prel = null;
	
	public $locator = null;
	public $locatorID = 1;
	public $locatorName = '';
	
	
	public $shot = null;
	public $shotID = 1;
	public $shotName = "";
	public $shotwidth = 2048;
	public $shotheight = 1536;
	
	public $iPlane = null;
	public $iPlaneName = "";
	public $iPlaneHash = "";
	
	public $iFrame = null;
	
	public $marker = null;
	public $h = 50.0;
	public $w = 50.0;

	// method declarations
    public function __construct(	$x ,$y, $z, $locatorID, $locatorName,
    								$shotID, $shotName, $shotwidth, $shotheight,
    							 	$iPlaneHash,
    								$w, $h )
    {
		global $points;
		global $prels;
		global $locators;
		global $markers;
		global $iFrames;
		global $iPlanes;
		global $shots;
		
		$points = $points;
		$prels = $prels;
		$locators = $locators;
		$markers = $markers;
		$iFrames = $iFrames;
		$iPlanes =  $iPlanes;
		$shots =  $shots;
    
    	// set the Locator, Prel and Point
		$this->x = $x;
		$this->y = $y;
		$this->z = $z; 
		$this->locatorID = $locatorID;
		$this->locatorName = $locatorName;
		
		if ($locators[$locatorID]!= null) { // we already created the object, so just reuse them.
		
			$this->point 	= $points[$locatorID];
			$this->prel  	= $prels[$locatorID];
			$this->locator 	= $locators[$locatorID];
			
			if (strpos($this->locator->shots, $shotID) == false) {
				$this->locator->addShot($shotID);
			}	else {
				// already in shots list, do nothing
			}
			
			$theLocator = $locators[$locatorID];
			//$theLocator->addShot($shotID);
			
		} else { // locator is not already in the locator array. Create Point, Prel and Locator.
		
			// create the objects
			$thePoint =  new Point ($x, $y, $z);
			$points[$locatorID] = $thePoint;
			$thePrel = new Prel($locatorID,$thePoint);
			$prels[$locatorID] = $thePrel;
			$theLocator = new Locator($locatorID,$locatorName);
			$locators[$locatorID] = $theLocator;
			
			// link the objects to each other
			// $thePoint->setPrel($thePrel);
			$thePrel->setPoint($thePoint);
			// $thePrel->setLocator($theLocator);
			$theLocator->setPrel($thePrel);
			
			// set the instance variables
			$this->point = $thePoint;
			$this->prel  = $thePrel;
			$this->locator =  $theLocator;
			$this->locator->addShot($shotID);
			
		}  
		
		// set the Shot, iPlane, iFrame and Marker
		$this->shotID = $shotID;
		$this->shotName =  $shotName;
		$this->shotwidth = $shotwidth;
		$this->shotheight = $shotheight;
		$this->iPlaneName = $shotName;
		$this->iFrameName =	$iFrameName;

		$theMarker = new Marker ($locatorID, $h, $w, 0.0);
		
		if ($shots[$shotID]!= null) { // we already created the object, so just reuse them.
			
			$this->shot 	=  $shots[$shotID];
			$this->iPlane 	=  $iPlanes[$shotID];
			$this->iFrame 	=  $iFrames[$shotID];
			
		} else { // shot is not already in shot arrray.  Create Shot, iPlane and iFrame.
		
			// create the objects
			$theShot = new Shot ($shotID, $shotName, $shotwidth, $shotheight);
			$shots[$shotID] = $theShot;
			$theIPlane = new IPlane ($shotName, $iPlaneHash);
			$iPlanes[$shotID] = $theIPlane;
			$theIFrame = new IFrame ();
			$iFrames[$shotID] = $theIFrame;
			
			//link the object to each other
			$theShot->setiPlane($theIPlane);
			// $theIPlane->setShot($theShot);
			$theIPlane->setIFrame($theIFrame);
			// $theIFrame->setIPlane($theIPlane);
			
			$theIFrame->addMarker($theMarker);
			
			//set the instance variables
			$this->shot = $theShot;
			$this->iPlane = $theIPlane;
			$this->iFrame = $theIFrame;
			
			
		}	// end Else
		
		
		$markers[$shotID.".".$locatorID] = $theMarker;
		
		$theMarker->setIFrame($theIFrame);
		
		
		
		$this->marker = $theMarker;
		
		
	}  // end __construct function

}  // end MarkerLocator Class definition

//----------------------- END OF CLASS DEFINITIONS ---------------------------------------
?>