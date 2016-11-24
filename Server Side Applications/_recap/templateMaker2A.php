<?php
    
echo "<pre>";    
echo "\n------- template maker.php ------------------------------\n\n";

	//set test arrays here:
	//$filecount = 3;
	//$filenames = array ("a", "b", "c" );
	//$filehashID = array ("A", "B", "C" );
	//$rotation =  array ("15", "30", "45" );
	//$elevation = array ("30", "45", "60" );
	

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
	//if (file_exists($manifestXMLFile)) { // Manifest file exists 
	//	$manifestXML = simplexml_load_file($manifestXMLFile);
	//	$manifestXMLString = $manifestXML ->asXML();
	// echo "Before shortening: manifestXMLString = $manifestXMLString\n";
	//$manifestXMLString = strstr($manifestXMLString,"<SugarCubeScan");
	//}
	
	$manifestXMLString = <<<EOF
<SugarCubeScan manifest_version='r1.3'>
<metadata>
<clientVersion>b3</clientVersion>
<serverVersion>b3-8c</serverVersion>
<scanID>TesterScan201404241833</scanID>
<scanStartTime>4/24/2014 6:33:25 PM GMT-7</scanStartTime>
<uploadStartTime>4/24/2014 6:39:35 PM GMT-7</uploadStartTime>
</metadata>
<order_details>
<from>scott@soundfit.me</from>
<to>scott@soundfit.me</to>
<subject><![CDATA[SugarCube Tester Generated Scan]]></subject>
<referenceID><![CDATA[TesterScan201404241833]]></referenceID>
<body><![CDATA[]]></body>
<attachments>
<filename>ORDER_tester_010101.pdf.hcrypt.data</filename>
<filename>ORDER_tester_010101.pdf.hcrypt.head</filename>
</attachments>
</order_details>
<modelling_options>
<doImageCropping>False</doImageCropping>
<imageCroppingRectangle>450,340,1310,1190</imageCroppingRectangle>
<doModelScaling>False</doModelScaling>
<doModelCropping>False</doModelCropping>
<boundingBox>-120,-120,-10,120,120,120</boundingBox>
<meshlevel>7</meshlevel>
<template3DP>/home/earchivevps/test.soundfit.me/Scans/b3/templates/47ImageTemplate.3dp</template3DP>
</modelling_options>
<imageset id='SLMu11b3mp3-201404221408-'>
<image elevation='60' rotation='0'>snapshot-18335817.jpg</image>
<image elevation='60' rotation='15'>snapshot-18340070.jpg</image>
<image elevation='60' rotation='30'>snapshot-18341085.jpg</image>
<image elevation='60' rotation='45'>snapshot-18341366.jpg</image>
<image elevation='60' rotation='60'>snapshot-18341648.jpg</image>
<image elevation='60' rotation='75'>snapshot-18342082.jpg</image>
<image elevation='60' rotation='90'>snapshot-18342800.jpg</image>
<image elevation='60' rotation='105'>snapshot-18343125.jpg</image>
<image elevation='60' rotation='120'>snapshot-18343407.jpg</image>
<image elevation='60' rotation='135'>snapshot-18343973.jpg</image>
<image elevation='60' rotation='150'>snapshot-18344303.jpg</image>
<image elevation='60' rotation='165'>snapshot-18344666.jpg</image>
<image elevation='60' rotation='180'>snapshot-18344956.jpg</image>
<image elevation='60' rotation='195'>snapshot-18345513.jpg</image>
<image elevation='60' rotation='210'>snapshot-18345804.jpg</image>
<image elevation='60' rotation='225'>snapshot-18350553.jpg</image>
<image elevation='60' rotation='240'>snapshot-18351021.jpg</image>
<image elevation='60' rotation='255'>snapshot-18351305.jpg</image>
<image elevation='60' rotation='270'>snapshot-18351593.jpg</image>
<image elevation='60' rotation='285'>snapshot-18352716.jpg</image>
<image elevation='60' rotation='300'>snapshot-18353147.jpg</image>
<image elevation='60' rotation='315'>snapshot-18353541.jpg</image>
<image elevation='60' rotation='330'>snapshot-18353870.jpg</image>
<image elevation='60' rotation='345'>snapshot-18354257.jpg</image>
<image elevation='30' rotation='0'>snapshot-18355449.jpg</image>
<image elevation='30' rotation='15'>snapshot-18355822.jpg</image>
<image elevation='30' rotation='30'>snapshot-18360289.jpg</image>
<image elevation='30' rotation='45'>snapshot-18360640.jpg</image>
<image elevation='30' rotation='60'>snapshot-18360960.jpg</image>
<image elevation='30' rotation='75'>snapshot-18361867.jpg</image>
<image elevation='30' rotation='90'>snapshot-18362730.jpg</image>
<image elevation='30' rotation='105'>snapshot-18363016.jpg</image>
<image elevation='30' rotation='120'>snapshot-18363335.jpg</image>
<image elevation='30' rotation='135'>snapshot-18363655.jpg</image>
<image elevation='30' rotation='150'>snapshot-18363981.jpg</image>
<image elevation='30' rotation='165'>snapshot-18364376.jpg</image>
<image elevation='30' rotation='180'>snapshot-18364666.jpg</image>
<image elevation='30' rotation='195'>snapshot-18364946.jpg</image>
<image elevation='30' rotation='210'>snapshot-18365560.jpg</image>
<image elevation='30' rotation='225'>snapshot-18370622.jpg</image>
<image elevation='30' rotation='240'>snapshot-18371339.jpg</image>
<image elevation='30' rotation='255'>snapshot-18371628.jpg</image>
<image elevation='30' rotation='270'>snapshot-18371980.jpg</image>
<image elevation='30' rotation='285'>snapshot-18372602.jpg</image>
<image elevation='30' rotation='300'>snapshot-18372954.jpg</image>
<image elevation='30' rotation='315'>snapshot-18373522.jpg</image>
<image elevation='30' rotation='330'>snapshot-18374102.jpg</image>
</imageset>
<diagnostics>
<shootingString><![CDATA[][Akien test shooting string 201404221408] [Reset Cube] S   [Verbose Off] V0  [Lights On] N  [Calibrate] R400 R359 R180 R90 R15 R0 E120 E90 E60 E30 R15 R0 E0 [status to blue]LO LB128  [Set Elevation to lower level] R0 E60 R15 R0  [Take 24 photos 15 degrees apart] {0,345,15 R* T }  [Set Elevation to higher level] R0 E30 R15 R0  [Take 24 photos 30 degrees apart] {0,330,15 R* T }  [Return to the home position] R0 E0  [Final reset] S   [status to green]LO LG128 [done] x]]></shootingString>
<cameraID>IZONE UVC 5M CAMERA</cameraID>
<imageResolution>2048x1536</imageResolution>
<jpegQuality>100</jpegQuality>
<COMport>COM8</COMport>
<forceCOMPort>false</forceCOMPort>
<motion_detection>on</motion_detection>
<motion_sensitivity>1</motion_sensitivity>
<testerMessages></testerMessages>
<SugarCubeMessages></SugarCubeMessages>
<videoProcAmp>
<brightness></brightness>
<contrast></contrast>
<hue></hue>
<saturation></saturation>
<sharpness></sharpness>
<gamma></gamma>
<whiteBalance></whiteBalance>
<backlightComp></backlightComp>
<gain></gain>
<colorEnable></colorEnable>
<powerLineFrequency></powerLineFrequency>
</videoProcAmp>
<cameraControl>
<zoom></zoom>
<focus></focus>
<exposure></exposure>
<aperture></aperture>
<pan></pan>
<tilt></tilt>
<roll></roll>
<lowLightCompensation></lowLightCompensation>
</cameraControl>
</diagnostics>
</SugarCubeScan>
EOF;
	
	$manifestXML = simplexml_load_string($manifestXMLString);
	
	//print_r($manifestXML);
	// echo "\n\n";
	
	$boundingBox 	=	$manifestXML -> modelling_options -> boundingBox;	
	$meshLevel		=	$manifestXML -> modelling_options -> meshlevel;
	$imagewxh		=	$manifestXML -> diagnostics -> imageResolution;
	$pieces 		=   explode("x", $imagewxh);
	$imageWidth 	=   $pieces[0]; // piece1	
	$imageHeight	=   $pieces[1]; // piece2
	
	
	// extract the filenames, hashcodes, etc.
	
	$filenames = $manifestXML -> imageset -> image;
	// print_r($filenames);
		 
	$numberOfImageFiles =  count($filenames);
	echo "Number of image files = $numberOfImageFiles\n";
	$outputString = $outputString . "Number of image files = $numberOfImageFiles\n";
	echo "testing image files:\n ";
	
	$n = 0;
	//$elevation = array();
	//$rotation = array();
	foreach ($manifestXML -> imageset -> image as $imageObject) {
		$elevation[$n] =  $imageObject['elevation'][0];
		$rotation[$n] =   $imageObject['rotation'][0];
    	$n++;
	}

	print_r($elevation[0][0]);
	 

	print_r($rotation[0][0]);
	//exit;


	// ############ NEW CODE FOR MAKING TEMPLATE ON THE FLY GOES HERE   #############
	
	
	
	// functions for calculating cartesian coordinates for camera position 
	// from spherical coordinates.
	// used in T (x, y,  z) tag in Template
	//

	function Tx($armRadius, $theRotation, $rotationOffset, $theElevation) {
		$rotation = doubleval($theRotation +  $rotationOffset );
		$elevation = doubleval($theElevation);
		$x = $armRadius*cos(deg2rad($rotation))*sin(deg2rad ($elevation));
		return $x;
	}

	function Ty($armRadius, $theRotation, $rotationOffset, $theElevation) { 
		$rotation = doubleval($theRotation +  $rotationOffset );
		$elevation = doubleval($theElevation);
		$y = $armRadius*sin(deg2rad (-($effectiveRotation)))*cos(deg2rad ($elevation));
		return $y;
	}

	function Tz ($armRadius, $theElevation, $pivotHeight ) {
		$elevation = doubleval($theElevation);
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
	for ($i = 0; $i < 3; $i++)   // use for testing
	// for ($i = 0; $i < $numberOfImageFiles; $i++) 
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
		
		echo "rotation[$i] = $rotation[$i], elevation[$i] = $elevation[$i]\n";
		
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
	
	