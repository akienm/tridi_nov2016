//  3DPGrapher  PsuedoCode

//	This program takes a list of 3DP files and creates graphs where each camera position is 
//	in the 3DP file is shown 
 
// 	create 3 1000x1000 pixel image canvases for graphing camera positions: 
//	CameraXY, CameraXZ, and CameraYZ

//	We should probably create graphs for where the cameras are pointing too (Rx, Ry, Rz)
//  But I am not sure how we should render them yet.
//	create 3 1000x1000 pixel image canvases for graphing camera directions:
//  CameraRX,  CameraRY, and Camera RZ

//  Define ArmRadius = 52.5;
// 	Define CanvasSize = 1000;

// 	Let the pen color be "gray".  
//	For each of the images canvases.
//		Draw a grid on each with ten equal lines 
//		For i = 0 to 10
//			draw gray line from (0,  CanvasSize * i / 10 ) to ( CanvasSize,  CanvasSize * i / 10 )
//			draw gray line from ( CanvasSize * i / 10, 0 ) to ( CanvasSize * i / 10, CanvasSize  )
//		end  i loop
//  	draw the end lines:
//		draw gray line from ( 0, CanvasSize ) to ( CanvasSize, CanvasSize )
//		draw gray line from ( CanvasSize, 0 ) to ( CanvasSize, CanvasSize )	

// 	Define an array of Colors, each file gets a different color in the graph:  
//  Colors = [ "Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet" ]' 

//  Define Normalize() function to convert a coordinate in the 3DP file into a 2D point
// 	within the image canvas:

//  function normalize(coordinate) { return CanvasSize * coordinate / ArmRadius }

// 	Read a list of 3DP files into  Array3DFileNames
 
// 	For each file in  Array3DPFileNames as Filename
// 		open Filename as XMLStructure
//		change the pen color to the corresponding index in the color array

// 		For each ShotTag  in XMLStructure
// 			get the TSubtag attributes  and save as Tx, Ty and Tz
// 			get the RSubtag attributes  and save as Rx, Ry and Rz

// 			Draw a point on CameraXY at normalize(Tx), normalize(Ty)
// 			Draw a point on CameraXZ at normalize(Tx), normalize(Tz)
// 			Draw a point on CameraXY at normalize(Ty), normalize(Tz)

//			If this isn't the first camera position {  // draw arrows from last position to current position
// 				Draw an arrow from normalize(oldTx), normalize(oldTy) to  normalize(Tx), normalize(Ty)
// 				Draw an arrow from normalize(oldTx), normalize(oldTz) to  normalize(Tx), normalize(Tz)
// 				Draw an arrow from normalize(oldTy), normalize(oldTz) to  normalize(Ty), normalize(Tz)
//			}

//			oldTx  = Tx; oldTy = Ty;  oldTz = Tz;

//			Draw graphs on the CameraRX, CameraRY and CameraRZ canvases however they should be...

//		End Next Shot loop
//
//		Write out the name of the file in the corresponding color in the saved "Legend" HTML.

//	End next file in Array3DPFileNames

//	Save the image canvases as PNG files

// Write HTML to the page to display the graphs using the saved PNG files
// Output the saved "Legend" HTML


