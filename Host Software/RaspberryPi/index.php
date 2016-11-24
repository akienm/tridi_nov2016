<!DOCTYPE html>
<html>
<head>
  <title>Test</title>
</head>
<body>



  <div style="text-align: left;"><span style="font-size:12px;"><span style="font-family:verdana,geneva,sans-serif;"><img alt="" src="http://prod.3dwares.co/images/3DWaresWhiteLogo.png" style="width: 650px; height: 82px;" /><hr></span></span></div>

  <h2>SuagrCube_v2</h2>
  <p><span class="error">* required field.</span></p>
  <form method="post" action="file.php"> 

    Name: <input type="text" name="name"  required>
    <br><br>

    To: <input type="email" name="to"  required>
    <br><br>

    From: <input type="email" name="from"  required>
    <br><br>
       
    ScanID: <input type="text" name="scanID"  required>
    <br><br>

    Subject: <input type="text" name="subject">
    <br><br>
       
    Message: <textarea name="message" rows="5" cols="40"></textarea>
    <br><br>
  
    <input type="submit" name="Submit" value="Submit"/>
      
    </form>
    <form method="link" action="settings_v1.php">
            <input type="submit" value="Settings">
    </form>
    
</body>
</html>
