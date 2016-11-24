<!DOCTYPE html>
<html>
  <head>
    <title>Settings</title>
  </head>
  <body>
    <div style="text-align: left;"><span style="font-size:12px;"><span style="font-family:verdana,geneva,sans-serif;"><img alt="" src="http://prod.3dwares.co/images/3DWaresWhiteLogo.png" style="width: 650px; height: 82px;" /><hr></span></span></div>

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
      $imageCropping = "";
      $imageCroppingRectangle = "";
      $modelScaling = "";
      $modelCropping = "";
      $boundingBox = "";
      $meshlevel = "";
      $template3DP = "";
      $shootingString = "";
      $cameraID = "";
      $imageResolution = "";
      $jpegQuality = "";
      $COMport = "";
      $forceCOMPort = "";
      $motionDetection = "";
      $motionSensitivity = "";
      $SugarCubeMessages = "";
      $brightness = "";
      $contrast = "";
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
    ?> 

    <form method="post" action="saveSettings.php">
      
    Client Verison: <input type="text" name="clientVersion" value="<?php echo $clientVersion; ?>">
    <br><br>
    Server Verison: <input type="text" name="serverVersion" value="<?php echo $serverVersion; ?>">
    <br><br>
    JPEG Quality: <input type ="range" min="1" max="100" name="jpegQuality" vaule="50" onchange="showValue(this.value)"><span id="range">50</span>
    <br><br>


    <script type="text/javascript">

      function showValue(newValue)
      {
        document.getElementById("range").innerHTML=newValue;
      }
    </script>
      <input type="submit" value="Submit">
    </form>


  </body>
</html>