<?php 

echo "\nSelecting scan files to upload.\n";
//  Add files - resource POST /file
//  retrieve a scan folder from the RECEIVED directory
//  put the JPG files from the scan directory into an array
$scan_directory="/home/earchivevps/test.soundfit.me/Scans/b1/received/131111-1347";
$scan_array = glob($scan_directory."/201*{.jpg,.JPG}",GLOB_BRACE);
echo "scanning directory: $scan_directory \n";
print_r($scan_array);
$nb = 0;
$files =array () ;
foreach ($scan_array as $key => $value) 
{	
	$files["file[$nb]"] = "@" . $value;
	$filename = basename($value);
 	echo "($nb) $filename \n";
	$nb++;
}

?>