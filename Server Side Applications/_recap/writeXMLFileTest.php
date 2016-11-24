<?php

	$emailxmlstring
		 = "<xml>\n" .
			"<startTime>$startTime</startTime>\n" .
			"<startTimeText>$today</startTimeText>\n" .
			"<clientVersion>$clientVersion</clientVersion>\n" .
		 	"<serverVersion>$serverVersion</serverVersion>\n" .
		 //	"<scanStartTime>$scanStartTime</scanStartTime>\n";
		 //	"<uploadStartTime>$uploadStartTime</uploadStartTime>\n";
		 //	"<scanID>$scanID</scanID>\n" .
		 //	"<photoScene>$photoSceneID</photoScene>\n" .
 		 //	"<to>$to</to>\n" .
 		 //	"<from>$from</from>\n" .
 		 //	"<subject>$subject</subject>\n" .
 		 //	"<body>$body</body>\n" .
 		 //	"<referenceID>$referenceID</referenceID>\n" .
 		 // "$attachmentsXMLString\n" .
 		 //	"<meshlevel>$meshlevel</meshlevel>\n" .
 		 //	"<template3DP>$template3DP</template3DP>\n" .
 		 //	"<shootingString>$shootingString</shootingString>\n" .
		 //	"<doImageCropping>$doImageCropping</doImageCropping>\n" .
		 //	"<imageCroppingRectangle>$imageCroppingRectangle</imageCroppingRectangle>\n" .
 		 //	"<doModelScaling>$doModelScaling</doModelScaling>\n" .
 		 //	"<doModelCropping>$doModelCropping</doModelCropping>\n" .
 		 //	"<boundingBox>$boundingBox</boundingBox>\n" .
 		 //	"<callbackProgram>$callback</callbackProgram>\n" .
			"</xml>\n";
	$emailxml =	new SimpleXMLElement($emailxmlstring);
	// write out the file:
	$emailmapfile = "emailmapfile.xml";
	$emailxml->asXml($emailmapfile);
	$shellcmd =  "/bin/cat emailmapfile.xml ";
  	$answer = `$shellcmd`;
  	echo $answer
	
?>