<?php
    
    $clientVersion = "SugarCube_v3.1";
    $serverVersion = " Version 1.0 ";
    $scanID = "Object";
    $From = "anand.jagan@gmail.com";
    $to = "anand.jagan@gmail.com";
    $subject = "";
    $referenceID = "";
    $body = "";
    $attachments = "";
    $filename = "";
    $imageCropping = "False";
    $imageCroppingRectangle = "430,340,1310,1190";
    $modelScaling = "True";
    $modelCropping = "True";
    $boundingBox = "15,-15,5,15,15,10";
    $meshlevel = "7";
    $template3DP = "";
    $shootingString = "][close comments]*NEWLINE_TOKEN*[2 layer 60 and 30 degrees]*NEWLINE_TOKEN**NEWLINE_TOKEN*[Usual] S V0 I1 LO LG3*NEWLINE_TOKEN**NEWLINE_TOKEN*[Spatial Calibration Images]*NEWLINE_TOKEN*R0 E0 E80 R20 R0*NEWLINE_TOKEN*{0,351,90*NEWLINE_TOKEN*R* T*NEWLINE_TOKEN*}*NEWLINE_TOKEN**NEWLINE_TOKEN*[Set Elevation to lower level] *NEWLINE_TOKEN*R0 E0 E60 R20 R0*NEWLINE_TOKEN*{0,351,20*NEWLINE_TOKEN*R* T*NEWLINE_TOKEN*}*NEWLINE_TOKEN**NEWLINE_TOKEN*[Set Elevation to mid level]  *NEWLINE_TOKEN*R0 E0 E30 R20 R0*NEWLINE_TOKEN*{0,351,20*NEWLINE_TOKEN*R* T*NEWLINE_TOKEN*}*NEWLINE_TOKEN**NEWLINE_TOKEN*[Usual] E0 R0 S LO LB6*NEWLINE_TOKEN*[done]";
    $cameraID = "IZONE UVC 5M CAMERA";
    $imageResolution = "2048x1536";
    $jpegQuality = "80";
    $COMport = "";
    $forceCOMPort = "";
    $motionDetection = "True";
    $motionSensitivity = "1";
    $SugarCubeMessages = "";
    $brightness = "";
    $contrast = "6";
    $hue = "";
    $saturation = "";
    $sharpness = "";
    $gamma = "";
    $whiteBalance = "";
    $backLightComp = "";
    $gain = "";
    $colorEnable = "";
    $powerLineFrequency = "";
    $zoom = "";
    $focus = "";
    $exposure = "";
    $aperture = "";
    $pan = "";
    $tilt = "";
    $roll = "";
    $lowLightCompensation = "";

    $le="35mm Focal Length Equivalent (mm)";
    $sfy="Scaling Factor Y (1=100%)";
    $fl="Actual Focal Length (mm)";
    $tox="Ref Orig offset X (mm)";
    $rz="RzOffset (deg)";
    $sfz="Scaling Factor Z (1=100%)";
    $fx="Field of View X (deg)";
    $sf="Saling Factor (1=100%)";
    $sc="Unit Serial Number";
    $ss="Sensor Size (in)";
    $aoy="Ang offset Y (deg)";
    $cn="Camera Name";
    $ph="Pivot height (mm)";
    $pps="";
    $toy="Ref Orig offset Y (mm)";
    $rx="RxOffset (deg)";
    $fovs="FOVStatus";
    $aox="Ang offset X (deg)";
    $sfx="Scaling Factor X (1=100%)";
    $toz="Ref Orig offset Z (mm)";
    $aoz="Ang offset Z (deg)";
    $ry="RyOffset (deg)";
    $al="Arm length (mm)";


    
    echo "Opening File";
    $newFile = fopen("config.py", "w+");
    echo "\nFile Opened";
    $fileContents = file_get_contents("config.py");
    
    if(0 == filesize($newFile)){
        echo "nothing in file ";
    }

    echo "$fileContents";



    fwrite($newFile, "#These are the default values\n");
    fwrite($newFile, "clientVersion = \"$clientVersion\"\n");
    fwrite($newFile, "serverVersion = \"$serverVersion\"\n");
    fwrite($newFile, "scanID = \"$scanID\"\n");

    fwrite($newFile, "From= \"$From\"\n");
    fwrite($newFile, "to = \"$to\"\n");
    fwrite($newFile, "subject = \"$subject\"\n");
    fwrite($newFile, "referenceID = \"$referenceID\"\n");
    fwrite($newFile, "body = \"$bSugarCubeMessagesody\"\n");
    fwrite($newFile, "attachments = \"$attachments\"\n");
    fwrite($newFile, "filename = \"$filename\"\n");

    fwrite($newFile, "imageCropping = \"$imageCropping\"\n");
    fwrite($newFile, "imageCroppingRectangle = \"$imageCroppingRectangle\"\n");
    fwrite($newFile, "modelScaling = \"$modelScaling\"\n");
    fwrite($newFile, "modelCropping = \"$modelCropping\"\n");
    fwrite($newFile, "boundingBox = \"$boundingBox\"\n");
    fwrite($newFile, "meshlevel = \"$meshlevel\"\n");
    fwrite($newFile, "template3DP = \"$template3DP\"\n");

    fwrite($newFile, "shootingString = \"$shootingString\"\n");
    fwrite($newFile, "cameraID = \"$cameraID\"\n");
    fwrite($newFile, "imageResolution = \"$imageResolution\"\n");
    fwrite($newFile, "jpegQuality = \"$jpegQuality\"\n");
    fwrite($newFile, "COMport = \"$COMport\"\n");
    fwrite($newFile, "forceCOMPort = \"$forceCOMPort\"\n");
    fwrite($newFile, "motionDetection = \"$motionDetection\"\n");
    fwrite($newFile, "motionSensitivity = \"$motionSensitivity\"\n");
    fwrite($newFile, "testerMessages = \"$testerMessages\"\n");    
    fwrite($newFile, "SugarCubeMessages = \"$SugarCubeMessages\"\n");


    fwrite($newFile, "brightness = \"$brightness\"\n");
    fwrite($newFile, "contrast = \"$contrast\"\n");
    fwrite($newFile, "hue = \"$hue\"\n");
    fwrite($newFile, "saturation = \"$saturation\"\n");
    fwrite($newFile, "sharpness = \"$sharpness\"\n");
    fwrite($newFile, "gamma = \"$gamma\"\n");
    fwrite($newFile, "whiteBalance = \"$whiteBalance\"\n");
    fwrite($newFile, "backLightComp = \"$backLightComp\"\n");
    fwrite($newFile, "gain = \"$gain\"\n");
    fwrite($newFile, "colorEnable = \"$colorEnable\"\n");
    fwrite($newFile, "powerLineFrequency = \"$powerLineFrequency\"\n");

    fwrite($newFile, "zoom = \"$zoom\"\n");
    fwrite($newFile, "focus = \"$focus\"\n");
    fwrite($newFile, "exposure = \"$exposure\"\n");
    fwrite($newFile, "aperture = \"$aperture\"\n");
    fwrite($newFile, "pan = \"$pan\"\n");
    fwrite($newFile, "tilt = \"$tilt\"\n");
    fwrite($newFile, "roll = \"$roll\"\n");
    fwrite($newFile, "lowLightCompensation = \"$lowLightCompensation\"\n");
           

    




    fwrite($newFile, "#Here are the changed values");
    fwrite($newFile, $fileContents);
    
    echo "\nFile Saved";

    if( copy('config.py', '/home/pi/CancunAlpha/config.py'))
    {
        unlink('config.py');
    }   
    

?>



<!DOCTYPE html>
<html>
  <head>
    <title>FileSaved</title>
  </head>
  <body>
    <form method="link" action="index.php">
      <input type="submit" value="Settings">
    </form>
  </body>
</html>
