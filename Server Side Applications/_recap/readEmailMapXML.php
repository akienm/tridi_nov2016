<?php

$scanID = 'ADV-131007-1741';
$emailmapdir = '/home/earchivevps/test.soundfit.me/Scans/b1/emailmaps/';
$emailmapfile = $emailmapdir . $scanID . ".xml";

if (file_exists($emailmapfile)) {
    $xml = simplexml_load_file($emailmapfile);
 	$to = 	$xml->sender;
 	$from = $xml->recipient;
	echo "to:   $to\n";
	echo "from: $from\n";
	
} else {
    echo "Failed to open $emailmapfile. Will use default emails addresses";
}

?>