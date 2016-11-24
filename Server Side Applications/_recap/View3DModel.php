<html lang="en">
	<head>
		<title>SoundFit SugarCube 3D Model Viewer</title>
		<meta charset="utf-8">
		<meta name="viewport" content="width=device-width, user-scalable=no, minimum-scale=1.0, maximum-scale=1.0">
		<style>
			body {
				background:#fff;
				padding:0;
				margin:0;
				overflow:hidden;
				font-family:'trebuchet ms', 'lucida grande', 'lucida sans unicode', arial, helvetica, sans-serif;
				text-align:center;
			}
			canvas { pointer-events:none; z-index:10; }
			
			p { font-size: small;}

		</style>
	</head>

	<body>
		<div>
			<h2>SoundFit 3D Model Viewer for ScanID = <?php echo basename(dirname(__FILE__)); ?>
			</h2>
			<!-- p>by Scott L. McGregor, SoundFit</p -->
			<!-- p>adapted from the <a href="https://github.com/mrdoob/three.js">Three.js</a> example webgl_objconvert_test.html</p -->
			<!-- p>and based on <a href="http://dev.opera.com/articles/view/porting-3d-graphics-to-the-web-webgl-intro-part-2/"> 
			Porting 3D graphics to the web — WebGL intro part 2</a> by <href="http://dev.opera.com/author/dropoutwannabe">Luz Caballero</a>
			</p --> 

		</div>
		<script src="/render3D/three.js-master/build/three.js"></script>
		<script src="/render3D/three.js-master/examples/js/Detector.js"></script>		
		<script src="/render3D/js/RequestAnimationFrame.js"></script>
		<script>
		if ( ! Detector.webgl ) Detector.addGetWebGLMessage();

		var SCREEN_WIDTH = window.innerWidth;
		var SCREEN_HEIGHT = window.innerHeight;
		var FLOOR = 0;

		var container;

		var camera, scene;
		var webglRenderer;

		var zmesh, geometry;

		var mouseX = 0, mouseY = 0;

		var windowHalfX = window.innerWidth / 2;
		var windowHalfY = window.innerHeight / 2;
	
		document.addEventListener( 'mousemove', onDocumentMouseMove, false );
		init();

		function init() {
		  container = document.createElement( 'div' );
		  document.body.appendChild( container );
			
		  // camera
		  camera = new THREE.PerspectiveCamera( 50, SCREEN_WIDTH / SCREEN_HEIGHT, 1, 100000 );
		  camera.position.z = 75;
		  camera.position.y = 15;
		  camera.position.x = 0;
		  
		  // scene
		  scene = new THREE.Scene();

		  // lights
		  var ambient = new THREE.AmbientLight( 0x777777 ); // median lighting levels
		  scene.add( ambient );
			
		  // more lights
		  var directionalLight = new THREE.DirectionalLight( 0xffeedd );
		  directionalLight.position.set( 0, -70, 100 ).normalize();
		  scene.add( directionalLight );
		}

		// renderer
		webglRenderer = new THREE.WebGLRenderer();
		webglRenderer.setSize( SCREEN_WIDTH, SCREEN_HEIGHT );
		webglRenderer.domElement.style.position = "relative";
		container.appendChild( webglRenderer.domElement );

		// loader
		var loader = new THREE.JSONLoader(),
		
		callbackModel = function(geometry, materials) { 
			createScene( geometry, materials, 90, FLOOR, -50, 105 ) 
		};
		 
		loader.load( "mesh.js", callbackModel );
		
		function createScene( geometry, materials, x, y, z, b ) {
		  
		zmesh = new THREE.Mesh( geometry, new THREE.MeshFaceMaterial( materials ) );
		  
		  zmesh.material.materials[ 0 ].side = THREE.DoubleSide; 
		  zmesh.material.materials[ 1 ].side = THREE.DoubleSide; 
		  zmesh.rotation.set( - Math.PI / 2, 0, 0 ); 
		
		  zmesh.position.set( 0, 16, 0 );
		  zmesh.scale.set( 1, 1, 1 );
		  scene.add( zmesh );
		}
		
		animate();  // moved here per suggestion by federico_strazzullo in 
		// http://www.khronos.org/message_boards/showthread.php/8213-Porting-3D-graphics-to-the-web-Tutorial-issues

		function onDocumentMouseMove(event) {
		  mouseX = ( event.clientX - windowHalfX );
		  mouseY = ( event.clientY - windowHalfY );
		}

		function animate() {
		  requestAnimationFrame( animate );
		  render();
		}

		function render() {
		  zmesh.rotation.set(mouseY/100 + 1 -90, mouseX/200, 0);
		  webglRenderer.render( scene, camera );
		}
		</script>

	</body>
</html>
