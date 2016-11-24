<?php
    echo "Opening File";

    $clientVersion = "SugarCube 1.3";
    $newFile = fopen("config.py", "w+");
    echo "\nFile Opened\n";
    
    if ($_POST["clientVersion"] != $clientVersion){
        $clientVersion = $_POST["clientVersion"];
        fwrite($newFile, "clientVersion = \"$clientVersion\"");
    }


    
    fclose($newFile); 
    echo "\nFile Done";

    
?>

<!DOCTYPE html>
<html>
  <head>
    <title>SettingsSaved</title>
  </head>
  <body>
    <form method="link" action="index.php">
      <input type="submit" value="Settings">
    </form>
  </body>
</html>
