<?php
	session_start();
	if(isset($_SESSION['user']))
		{
			$user = $_SESSION['user'];
			echo "$user";
		}
	else
		{
		}


	//echo "<pre>\n";
	//echo "----------------------- START OF ReCapLaunchAndGo_b3r9e.php -----------------\n";
	
	// ReCapLaunchAndGo_b3r8e.php
	// Written by Scott Mcgregor 
	// April 2014
	
	// This program the "trigger program" called by the SoundFit scanner client whenever
	// a new scanset has been uploaded and is ready for processing.
	// 
	// To keep the client from having to wait until the ReCapLaunchModelling program 
	// completes, this program launches the other program in an asynchronous background
	// shell.   It does that using the "nohup" command to tell the background process
	// to ignore hangup signals and keep processing when the parent that spawned
	// the shell command completes first.  Nice is used to set the background process
	// scheduling to be a lower priority than web response priority.
	
	// To prevent messages from the spawned program written to STDOUT or STDERR from
	// causing an error, these streams are connected to /dev/null.
	//
	// A final $printf shell command is run to return the Linux process ID of
	// the spawned shell, so it can be checked for in the jobs queue later.

	
	// -------------------- PASSED PARAMETERS -----------------------------------------
	// Various parameters can be passed by the calling program (normal case)
	// or a user who runs the program from a command line (testing).
	// most of these parameters (except UploadDir which is mandatory) are optional.

	// all parameters passed to this program will be passed on to the
	// ReCapLaunchModeling-wScale+Crop8e.phpprogram which will be launched in a shell
	// that runs in background.

	// The program checks to see if it is run in a command line,  if it is, all parameters
	// are pulled from the argv list and put into the $_GET array.   
	if (php_sapi_name() === 'cli') {parse_str(implode('&', array_slice($argv, 1)), $_GET);}


	// We turn the GET parameters into commandline parameters separated by spaces:
	$queryArgString = " " . http_build_query($_GET, '', " ");
	
	// start command and path to the PHP binary
	define ('PHPPGM', 'nohup nice -n 10 /usr/local/php53/bin/php -f ');  
	
	// the program to launch: 
	define ('LAUNCHPGM', '/home/earchivevps/test.soundfit.me/apps/_recap/ReCapLaunchModeling-wScale+Crop9e.php');
 
 	// Where logs should be stored.
 	define ('LOGSDIR', '/home/earchivevps/test.soundfit.me/Scans/b3/logs/');
 	$uploadDir = $_GET["uploadDir"];
 	$logsdir = LOGSDIR ."/" . basename($uploadDir);
 	if ($logsdir) {
 		$pipeToLogs = " > " . $logsdir . ".stdio.log 2>" . $logsdir . ".err.log & printf" . ' "%u" $!';
 	
		// example shell command:
 
		// $shellcmd = 'nohup nice -n 10 /usr/local/php53/bin/php -f /home/earchivevps/test.soundfit.me/apps/_recap/ReCapLaunchModeling-wScale+Crop9b.php ' . $queryArgString . ' > /dev/null 2>/dev/null & printf "%u" $!';
	
		$shellcmd =  PHPPGM . LAUNCHPGM . $queryArgString . $pipeToLogs;
		$pid = shell_exec($shellcmd);  // Returns PID which is Linux ProcessID that can be checked by other programs later
	
		//echo "Launched " . LAUNCHPGM .  " ProcessID = $pid\n";
		echo "shellcmd = $shellcmd\n";
		//echo $pid. "\n";
	
		//echo "----------------------- END OF ReCapLaunchAndGo_b3r9e.php -----------------\n";	
		//echo "</pre>";
		exit ($pid) ;
	}   else { die("Error: no uploadDir defined."); }
	

?>
