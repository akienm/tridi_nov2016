<html>
<head>
</head>
<body>
<table width="100%">

	<?php
		$COLUMNS = 4;
		$COLWIDTH =  100 / $COLUMNS; 	
		
		$filesindir = glob('*');
		$filecount = count($filesindir);
		for($i = 0; $i < $filecount; $i++) {
			$column = $i % $COLUMNS;
			$filename = $filesindir[$i];
			
			if ($column == 0 ) {  // start a row
				echo "<tr width='100%'> \n"; 
			}  // end new row processing
			
			echo "<td width='". $COLWIDTH . "%'> \n";
			$ext = pathinfo($filename, PATHINFO_EXTENSION);
			
			if ($ext == "jpg") {  // show thumbnail
				echo "<a href='" . $filename . "'> \n";
				    echo "<img src='" . $filename . "'" . " width='100%'><br /> \n";
				    echo $filename;
				echo "</a> \n";
				
			} else {  // not an image  - show other thumbnail
			
				echo "<a href='" . $filename . "'> \n";
					if ( $ext == "pdf" ) {  //show PDF icon
				    	echo "<img src='http://earchive.soundfit.me/images/Adobe_PDF_icon.png'" . " width='100px'><br /> \n";
				    } else {  // not an image and not a pdf
				        echo "<img src='http://earchive.soundfit.me/images/text-file-3-512.jpg'" . " width='100px'><br /> \n";
				    }
				    echo $filename;
				echo "</a> \n";
			}
			echo "</td> \n";
			if (($column == $COLWIDTH - 1) || ($i == $filecount -1 )) {  // end a row
				echo "</tr> \n" ;
			}			
		}

	?>
</table>
</body>
</html>