<?php

	$filehashname = " o S t B  m n U S  U v U p  A 2 5 F  u P C yy h y M l G Q = - 3 0 8 3 4 9 - 3 2 0 0 0 0 0 ";
	// $filehashname = " o S t B / m n U S / U v U p / A 2 5 F / u P C y / y h y M / l G Q = - 3 0 8 3 4 9 - 3 2 0 0 0 0 0 ";

	$m = 0;
	$n = 0;
	$hashIDwithSlashes = "";
	
	$filehasharrray = array( 
	substr($filehashname, 0, 8),
	substr($filehashname, 8, 8),
	substr($filehashname, 16, 8),
	substr($filehashname, 24, 8),
	substr($filehashname, 32, 8),
	substr($filehashname, 40, 8),
	substr($filehashname, 48 )
	);
	
	$hashIDwithSlashes = implode("/", $filehasharrray);
	
	print_r($filehasharrray );
	echo "hashIDwithSlashes = $hashIDwithSlashes \n";
	
?>