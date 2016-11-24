<?php
    
echo "<pre>";    
echo "\n------- template maker.php ------------------------------\n\n";

	//set test arrays here:
	$filecount = 3;
	$filenames = array ("a", "b", "c" );
	$filehashID = array ("A", "B", "C" );
	$rotation =  array ("15", "30", "45" );
	$elevation = array ("30", "45", "60" );
	

	// set constants here:
	$armRadius = 104;		// in mm
	$rotationOffset = 90; 	// in degrees
	$pivotHeight = 24;  	// in mm
	$fieldOfView = 49;		// in degrees (79 in datasheet)
	//$imageWidth = 2048;		// in pixels -- 3MP
	//$imageHeight= 1536;		// in pixels -- 3MP
	// $boundingBox = "-120,-120,-10,120,120,120";
	// $meshLevel = 7; 
	
	
		

	// read the XML file
	if (file_exists($manifestXMLFile)) { // Manifest file exists 
		$manifestXML = simplexml_load_file($manifestXMLFile);
		$manifestXMLString = $manifestXML ->asXML();
		// echo "Before shortening: manifestXMLString = $manifestXMLString\n";
		$manifestXMLString = strstr($manifestXMLString,"<SugarCubeScan");
	}
	
	$boundingBox 	=	$manifestXML -> modelling_options -> boundingBox;
	$meshLevel		=	$manifestXML -> modelling_options -> meshlevel;
	
	$imagewxh		=	$manifestXML -> diagnostics -> imageResolution;
	$pieces 		=   explode("x", $imagewxh);
	$imageWidth 	=   $pieces[0]; // piece1
	$imageHeight	=   $pieces[1]; // piece2
	
	
	// extract the filenames, hashcodes, etc.
	
	$filenames = $manifestXML -> imageset -> image;
	$elevation = $manifestXML -> imageset -> image['elevation']; 
	$rotation  = $manifestXML -> imageset -> image['rotation']; 
	$numberOfImageFiles =  count($filenames);
	echo "Number of image files = $numberOfImageFiles\n";
	$outputString = $outputString . "Number of image files = $numberOfImageFiles\n";
	echo "testing image files:\n ";
	
		



	// ############ NEW CODE FOR MAKING TEMPLATE ON THE FLY GOES HERE   #############
	
	
	
	// functions for calculating cartesian coordinates for camera position 
	// from spherical coordinates.
	// used in T (x, y,  z) tag in Template
	//

	function Tx($armRadius, $rotation, $rotationOffset, $elevation) { 
		$x = $armRadius*cos(deg2rad($rotation+$rotationOffset))*sin(deg2rad ($elevation));
		return $x;
	}

	function Ty($armRadius, $rotation, $rotationOffset, $elevation) { 
		$y = $armRadius*sin(deg2rad (-($rotation+$rotationOffset)))*cos(deg2rad ($elevation));
		return $y;
	}

	function Tz ($armRadius, $elevation, $pivotHeight ) {
		$z = $armRadius*cos(deg2rad($elevation))+$pivotHeight;
		return $z;
	}
	
	//	EXAMPLE
	//
	//  Create the RZML Template object:
	//  <   ?x m l   v e r s i o n = " 1 . 0 "   e n c o d i n g = " U T F - 1 6 "   s t a n d a l o n e = " y e s "?   > 
	//  < R Z M L   v = " 1 . 4 . 9 "   a p p = " P r o j e c t   P h o t o f l y :   3 . 0 . 0 . 4 1 2 "   i d = " M 1 I U R A 1 A M K L G l K x 8 Q 3 m W I N 1 0 8 d g "  > 
	
	$RZML = new SimpleXMLElement("<RZML></RZML>");
	$RZML->addAttribute('v', '1.4.9');
	$RZML->addAttribute('app', 'Project Photofly: 3.0.0.412');  
	$RZML->addAttribute('id', 'M1IURA1AMKLFlKx8Q3mWIN108dg' ); 
	
	
	// add the CINF tag:
	//	 < C I N F   i = " 1 "   s w = " 2 0 4 8 "   s h = " 1 5 3 6 "   f o v s = " s "   f o v x = " 4 9 . 3 2 8 4 4 4 3 5 7 3 5 2 3 "   d s = " s "   p p s = " c "   d i s t o T y p e = " d i s t o 3 5 d " / > 
	//                   ^         ^                   ^
	//               $imageWidth  $imageHeight      $fieldOfView

	$CINF = $RZML->addChild('CINF');
	$CINF->addAttribute('i', '1');
	$CINF->addAttribute('sw', "$imageWidth");
	$CINF->addAttribute('sh', "$imageHeight");
	$CINF->addAttribute('fovs', "s");
	$CINF->addAttribute('fovx', "$fieldOfView");
	$CINF->addAttribute('ds', 's');
	$CINF->addAttribute('pps', 'c');
	$CINF->addAttribute( 'distoType' , 'disto35d' );
	
	
	// write out <SHOT>s for each image:
	for ($i = 0; $i < $filecount; $i++) 
	{
		//	 < S H O T   i = " 1 "   n = " s n a p s h o t - 2 1 3 3 0 7 0 4 . j p g "   c i = " 1 "   w = " 2 0 4 8 "   h = " 1 5 3 6 " > 
		//           ^     ^                                  ^       ^
		//           $i     $filenames[$i]                $imageWidth  $imageHeight
		$SHOT = $RZML->addChild('SHOT');
		$j = $i+1;
		$SHOT->addAttribute('i', "$j");
		$SHOT->addAttribute('n', "$filenames[$i]");
		$SHOT->addAttribute('w', "$imageWidth");
		$SHOT->addAttribute('h', "$imageHeight");
		
		//	 < I P L N   i m g = " s n a p s h o t - 2 1 3 3 0 7 0 4 . j p g "   h a s h = " P 4 J K / i t c O / e B 4 G / t f q T / i F j D / R f B p / l m Y = - 1 2 0 9 0 8 0 - 1 0 0 0 0 0 0 0 0 " / > 
		//             ^                             ^
		//			$filenames[$i]                  $filehashID[$i]
		$IPLN = $SHOT->addChild('IPLN');
		$IPLN->addAttribute('img', "$filenames[$i]");
		$IPLN->addAttribute('hash', "$filehashID[$i]");
		

		
		//	 < C F R M   f o v x = " 4 9 . 3 2 8 4 4 4 3 5 7 3 5 2 3 " > 
		//               ^
		//            $fieldOfView
		$CFRM = $SHOT->addChild('CFRM');
		$CFRM->addAttribute('fovx', "$fieldOfView");
		
		//	<  T   x = " - 1 . 1 5 9 0 7 8 5 0 9 8 9 8 8 7 "   y = " - 2 1 . 2 2 6 9 4 8 2 0 2 1 2 4 1 "   z = " 1 4 . 8 9 0 2 4 3 3 1 5 7 5 9 6 "  /  > 
		//		   ^                    ^                     ^
		//		x=Tx(rotation,elevation)  y=Ty(rotation,elevation) z=Tz(elevation)
		
		
		$x = Tx($armRadius, $rotation[$i], $rotationOffset, $elevation[$i]) ;	
		$y = Ty($armRadius, $rotation[$i], $rotationOffset, $elevation[$i]) ;
		$z = Tz($armRadius, $elevation[$i], $pivotHeight ) ;
		$T = $CFRM->addChild('T');
		$T->addAttribute('x', "$x");
		$T->addAttribute('y', "$y");
		$T->addAttribute('z', "$z");
				
		//	  < R   x = " 4 6 . 3 1 6 3 3 4 7 0 5 0 0 0 5 "   y = " - 1 . 1 2 0 5 0 0 1 8 4 8 4 5 9 2 "   z = " 1 5 0 . 8 8 3 7 7 0 5 3 1 4 7 2 " / > 
		//        ^                    ^                     ^
		//      x=elevation         always 0                z= -rotation    
		$R = $CFRM->addChild('R');
		$R->addAttribute('x', "$elevation[$i]");
		$R->addAttribute('y', '0');
		$R->addAttribute('z', "$rotation[$i]");
		         
		//	 < / C F R M > 
		//	 < / S H O T > 
	}
	
	
	//  Postlog
	//  ...
	//  < X R E F   u r l = " 3 . 0 . 0 / X R E F / 4 a d v W 6 m n 0 E W s 6 b H G G W D B Q l L m g D c = "   t = " m a r k e r s 2 D "   i d = " 4 a d v W 6 m n 0 E W s 6 b H G G W D B Q l L m g D c = " / > 
	$XREF1 = $RZML->addChild('XREF');
	$XREF1->addAttribute('url', "3.0.0/XREF/4advW6mn0EWs6bHGGWD801LMgDc= ");
	                            
	$XREF1->addAttribute('t', " m a r k e r s 2 D");
	$XREF1->addAttribute('id', " 4advW6mn0EWs6bHGGWD801LMgDc= ");
	
	//  <  X R E F   u r l = " 3.0.0/XREF/dGDdTyZu73Uh0Blods0PsyjPqbY"   t = " l o c a t o r s "   i d = "dGDdTyZu73Uh0Blods0PsyjPqbY  = "  / > 
	$XREF2 = $RZML->addChild('XREF');
	$XREF2->addAttribute('url', "3.0.0/XREF/dGDdTyZu73Uh0Blods0PsyjPqbY");
	
	$XREF2->addAttribute('t', "locators");
	$XREF2->addAttribute('id', " dGDdTyZu73Uh0Blods0PsyjPqbY  =");

    //  < X R E F   u r l = " "   t = " r e f p r o j e c t "   i d = " M 1 I U R A 1 A M K L G l K x 8 Q 3 m W I N 1 0 8 d g " / > 
	$XREF3 = $RZML->addChild('XREF');
	$XREF3->addAttribute('url', " ");
	$XREF3->addAttribute('t', "refproject");
	$XREF3->addAttribute('id', "M1IURA1AMKLG1Kx8Q3mWIN108dg");

	//	 < X R E F   u r l = " 3 . 0 . 0 / X R E F / Y k 3 g 4 f H i W E 0 c b Q z 3 h 4 h t 6 E P 1 R J A = "   t = " m e s h "   i d = " Y k 3 g 4 f H i W E 0 c b Q z 3 h 4 h t 6 E P 1 R J A = " / > 
	$XREF4 = $RZML->addChild('XREF');
	$XREF4->addAttribute('url',"3.0.0/XREF/YK3g4fHiWE0cbQz3h4ht6EP1RJA=");
	
	$XREF4->addAttribute('t', "mesh");
	$XREF4->addAttribute('id', "YK3g4fHiWE0cbQz3h4ht6EP1RJA=");
	
	
	//	 < S O B J > 
	$SOBJ = $RZML->addChild('SOBJ');
	
	//	 < C M E S H   x r e f i d = " Y k 3 g 4 f H i W E 0 c b Q z 3 h 4 h t 6 E P 1 R J A = "   q = " 7 "   b = " - 1 2 0 , - 1 2 0 , - 1 2 0 , 1 2 0 , 1 2 0 , 1 2 0 " / > 
	//                                               ^         ^
	//                                           $meshLevel  $boundingBox
	$CMESH = $SOBJ->addChild('CMESH');
	
	$CMESH->addAttribute( 'xrefid' , 'Yk3g4fHiWE0cbQz3h4t6EP1RJA=' );
	$CMESH->addAttribute('q', "$meshLevel");
	$CMESH->addAttribute('b', "$boundingBox" ); 
	
	// </SOBJ>
	// </RZML>
	//  in future, write the RZML template to a file, for now just echo it: 
	
	
	//Header('Content-type: text/xml');
	echo $RZML->asXML();
	
echo "</pre>";     
exit ;
?>	
	
	