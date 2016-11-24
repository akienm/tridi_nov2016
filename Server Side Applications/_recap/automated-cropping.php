<?php

echo "USAGE: automated-cropping.php?src_dir=sourceDir&dst_dir=destDir&src_x=450&src_y=340&src_w=1310&src_h=1190\n\n";
echo "src_dir - the directory with the source images.\n";
echo "dst_dir - the directory to received the cropped images. Will be created if it doesn't exist.\n";
echo "src_x   - the top left position of the crop area in pixels from the left of the source image.\n";
echo "src_y   - the top left position of the crop area in pixels from the top of the source image.\n";
echo "dst_x   - the bottom right position of the crop area in pixels from the left of the source image.\n";
echo "dst_y   - the bottom right position of the crop area in pixels from the top of the source image.\n\n";


if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}
src_dir = htmlspecialchars($_GET["src_dir"]);
$dst_dir = htmlspecialchars($_GET["dst_dir"]);
if ($src_dir == NULL) $src_dir  = '/home/earchivevps/test.soundfit.me/Scans/b2/received/SLMu5c75np-UML-YYYY03DD144740R/';
if ($src_dir == NULL) $dst_dir = '/home/earchivevps/test.soundfit.me/Scans/b2/croppedDir/SLMu5c75np-UML-YYYY03DD144740R-cropped/';

$src_x = $_GET["src_x"]; // Crop Start X position in original image
$src_y = $_GET["src_y"]; // Crop Srart Y position in original image
$src_w = $_GET["src_w"]; // $src_x + $dst_w Crop end X position in original image
$src_h = $_GET["src_h"]; // $src_y + $dst_h Crop end Y position in original image
if ($src_x == NULL) $src_x = 450; // Crop Start X position in original image
if ($src_y == NULL) $src_y = 340; // Crop Srart Y position in original image
if ($src_w == NULL) $src_w = 1310; // $src_x + $dst_w Crop end X position in original image
if ($src_h == NULL) $src_h = 1190; // $src_y + $dst_h Crop end Y position in original image

echo "Source Directory = $src_dir\n";
echo "Destination Directory = $dst_dir\n";
echo "src_x = $src_x\n";
echo "src_y = $src_x\n";
echo "src_w = $src_x\n";
echo "src_h = $src_x\n\n";

$dst_x = 0;   // X-coordinate of destination point. 
$dst_y = 0;   // Y --coordinate of destination point. 

$dst_w = $src_w - $src_x; // crop width
$dst_h = $src_h - $src_y; // crop height

if (!file_exists($dst_dir)) {
    if (!mkdir($dst_dir, 0777, true)) {
    	die('Failed to create new directory...'. $dst_dir."\n");
    }
}

// get list of image files
$imagefiles = glob($src_dir."/snapshot*{.jpg,.JPG}",GLOB_BRACE);

$i = 0;
foreach ($imagefiles as $key => $value) 
{	
	// Get original image
	$src_image = imagecreatefromjpeg($value); // convert JPG to image resource
	$fileName = basename($value);
	
    if(!$src_image) { // image import failed
    	echo "image import failed: $value\n";
    } else { // image import succeeded               
		
		// Creating an image with true colors having cropped dimensions.( to merge with the original image )
		$dst_image = imagecreatetruecolor($dst_w,$dst_h);
		// Cropping 
		imagecopyresampled($dst_image, $src_image, $dst_x, $dst_y, $src_x, $src_y, $dst_w, $dst_h, $src_w, $src_h);
		// Saving 
		imagejpeg($dst_image, $dst_dir.$fileName, 100 ); // write out the cropped image as new jpg at Quality 100

		echo "$i: cropped $fileName\n";
		$i++;
	}
}
echo "\n$i cropped files written.\n";

imagedestroy($src_image); // clean up memory
imagedestroy($dst_image); // clean up memory
echo"\nall done!\n";
exit ;
?>