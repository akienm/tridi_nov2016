<?php

	// automatedmailtest.php?to=user@domain&subject=test&message=testbody&from=sender@domain

	if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}
	if ($to == null) 		$to = "akien.macian@gmail.com";
	if ($subject == null ) 	$subject = "automated email test";
	if ($message == null ) 	$message = "<div src='http://test.soundfit.me/Scans/b3/modeled//AMM-20140818-PointTestSet002-201408181501/DiffTable.html' />";
	if ($from == null) 		$from = "akien.macian@gmail.com";
		
	$headers = "Sender: sugarcube-daemon@soundfit.me \nReply-To: " . $from . "\nFrom: " . $from;

	mail($to,$subject,$message,$headers, "-F 'SoundFit SugarCube Service' -f sugarcube-daemon@soundfit.me");
			
?>