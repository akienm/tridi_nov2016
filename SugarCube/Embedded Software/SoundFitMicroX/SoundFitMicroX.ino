//==========================================================================
// SoundFitMicroX.ino
// Copyright (c) 2014 SoundFit LLC
// By Dennis Gentry

//==========================================================================
// Include Arduino.h 
// Arduino IDE will prepend this to the cpp file if we don't, but we want 
// to be able to compile without the IDE.
#include <Arduino.h>  	


//==========================================================================
// Flash/EEPROM storage setup
// The Arduino has Flash (32K Program Memory), RAM (2K volatile), and
// EEPROM (1K bytes). We use this to make strings storable in program 
// memory only instead of both in program memory and RAM.  This makes 
// the F() macro work properly.
// Workaround for http://gcc.gnu.org/bugzilla/show_bug.cgi?id=34734
#ifdef PROGMEM
#undef PROGMEM
#define PROGMEM __attribute__((section(".progmem.data")))
#endif

#define __ASSERT_USE_STDERR
#include <assert.h>

//==========================================================================
// Overview
// This Arduino program listens to its serial port for Commands, does
// its best to carry them out, and delivers a status code and string
// back to the serial port indicating success or failure.  Some of the
// Commands are sensing commands, which return appropriate values to
// the serial port.
//
// The hardware:
//   Controls: two stepper motors, an RGB LED, and an illumination lamp.
//   Senses:  power bus voltage, home positions for motors, and drawer open.
//
// MOTORS AND MOTOR CONTROL:
//
// We are using the AccelStepper library from the MIT guy along with
// the Adafruit Motor Shield Library.  (Both are required).
// http://www.airspayce.com/mikem/arduino/AccelStepper/
// https://github.com/adafruit/Adafruit-Motor-Shield-library.

#include <AccelStepper.h>
#include <AFMotor.h>
#include <EEPROM.h>

// The control circuit is based on http://www.adafruit.com/products/81
// Here is its schematic:
// http://learn.adafruit.com/system/assets/assets/000/009/769/original/mshieldv1-schem.png
// The chip controlling the motors:
// http://www.digikey.com/product-detail/en/L293DNE/296-9518-5-ND/379724

//==========================================================================
// Version Reporting Headers
// The version reported with 'v' started with 2.0
const char *version = "2.05c";  // current version, "c" for check homes.
// The makefile updates this with every compile.  The IDE does not.
#include "svnversion.h"
// For "production" versions, this stuff should all start off.
// Turn it on with, e.g., V1 and Decho=1

//==========================================================================
// Global flags
byte echo = 0;     // Echo characters back to tty?
byte verbose = 0;  // Verbose for humans, not for host software
byte debug = 0;    // You probably don't want this on

bool rotErr = false;   // amm rotation error flag
bool elevErr = false;  // amm elevation error flag

bool ignoringInput; // True if in a comment

//==========================================================================
// Operational Constants

// Functions send their status report ("0: resetCube() OK") to the serial
// port when called directly, but not when called as part of carrying
// out a higher-level command, e.g., Status12v() doesn't report when
// called by resetCube().
#define ENABLE_REPORT 1
#define DISABLE_REPORT 0

// Motor port #1 is elevation motor, with 48 steps per rev, and 1/16
// gearing = 8*48 = 768 steps per shaft rotation (adafruit says 1/16.032)
// http://www.adafruit.com/products/918
#define ELEV_MOTOR_STEPS 48   // 96 with interleave
// On observation, our motor seems to have 512 steps in a rotation, so
// it must have 1/10.666 gearing (or a different number of steps per
// rotation) so I'm going to use that as the gearing constant.
#define ELEV_MOTOR_GEARING 10.666

// motor port #2 is rotation motor with 200 steps per rev
// motor with 200 steps per rev http://www.adafruit.com/products/324
#define ROT_MOTOR_STEPS 200  // 400 with INTERLEAVE

#define ROT_MAX_SPEED    200              // steps per second
#define ROT_ACCELERATION ROT_MAX_SPEED*4  // full speed in 1/4 revolution

#define ELEV_MAX_SPEED    200
#define ELEV_ACCELERATION ELEV_MAX_SPEED*4 // full speed in 1/4 rev (nongeared)

// For the imaging lamps
#define OFF 0
#define ON 1

// All the status codes (success, error, other)

#define SUCCESS 0
#define ERROR 1

// Motor and position errors
#define ERROR_12v 2
// Rotation Motor
#define ERROR_OVERROTATE_ROTATION 3
#define LIMIT_SENSED_ROTATION 4
#define ERROR_ROTATION_HOME_NOT_SENSED 5
// Elevation Motor
#define ERROR_OVERROTATE_ELEVATION 6
#define LIMIT_SENSED_ELEVATION 7
#define ERROR_ELEVATION_HOME_NOT_SENSED 8

// Drawer code
#define DRAWER_OPEN 9

// Uninitialized EEPROM
#define UNINITIALIZED_EEPROM 10

// CRC Error
#define CRC_ERROR 11

// Don't report variables unless verbose=1 error
#define ERROR_REPORT_VARS 12

// Distinctive value for default, shouldn't show up
#define ERROR_DEFAULT 99

//==========================================================================
// Print If Verbose Functions
// Bunch of "print if verbose" functions.  Should probably make these
// polymorphic or macros.
void maybe_echo(char c) {
  if (echo) Serial.print(c);
}

void vprintc(const char c) {
  if (verbose)
    Serial.print(c);
}

void vprintDec(const int d) {
  if (verbose)
    Serial.print(d);
}

void vprint(const char *string) {
  if (verbose) {
    Serial.print(string);
  }
}

void vprintln(const char *string) {
  if (verbose) {
    Serial.println(string);
  }
}

//==========================================================================
// Diagnostics (internal)
// handle diagnostic informations given by assertion and abort program execution:
void __assert(const char *__func, const char *__file, int __lineno, const char *__sexp) {
  // transmit diagnostic informations through serial link.
  Serial.println(__func);
  Serial.println(__file);
  Serial.println(__lineno, DEC);
  Serial.println(__sexp);
  Serial.flush();
  // abort program execution.
  // abort();
}

//==========================================================================
// Nonvolitile storage
// How much Arduino RAM do we have left?
int freeRam () {
  extern int __heap_start, *__brkval;
  int v;
  // Free RAM is just the difference between the top of the stack (&v) and the
  // bottom of the heap.
  return (int) &v - (__brkval == 0 ? (int) &__heap_start : (int) __brkval);
}

void printRAM() {
  Serial.print(F("free RAM = "));
  Serial.println(freeRam());
}

// Caution: Arbitrary limit here, depends on available RAM.  Use above
// functions to figure out minimum RAM available.  As of 8 May 2014,
// we have enough free RAM (~1250 bytes) to read/write the whole 1K
// EEPROM, but I wouldn't count on that to last.
const int zBufferSize = 255;

// All Commands return a single digit status code *AND* a human
// readable string to the serial port. The first section after the
// digit is the command being executed. After that is the human
// readable message in a form like:
//       0: CheckReady OK
//       1: CheckReady 12V Fail
//       2: CheckReady Drawer Open Fail
//
// If the status is SUCCESS, automatically prints OK.
int reportStatusToSerial(int value, const char *msg, const char *eol)
{
  // Print like so:  "v: msg OK"
  if (verbose)
    Serial.println("");
  Serial.print(value);
  Serial.print(": ");
  Serial.print(msg);
  if (value == SUCCESS)
    Serial.print(" OK");
  else
    Serial.print(" Error");  // This should be the specific message

  Serial.print(eol);

  // Report on free memory if we're debugging
  if (debug)
    printRAM();

  // Wait for it all to get out there
  Serial.flush();
  return value;
}

int reportStatusWithData(int value, const char *msg, const char *additional)
{
  reportStatusToSerial(value, msg, "");
  Serial.print(":");
  Serial.println(additional);
  return value;
}

// Most (C/Arduino) functions return an integer (like the ones listed
// below and attached to specific directives/queries).  Functions
// return 0 for success and a non-zero positive integer for an error.

// When libraries return only an error number, we add an English
// readable message as well.


// (SugarCube) Commands are case-sensitive and single character.  Some
// commands, in particular, R (Rotation), E (Elevation) and L (Led),
// accept additional data from the serial line.  R controls the
// position of the rotation motor, and E controls the position of the
// elevation motor.  The desired position is specified as absolute
// integer degrees right (clockwise) of the Home position.
//
// R & E take up to 3 digits to specify degrees from home position.
// For example, 'R180' requests rotation to "due south."
//
// L takes one additional letter, RGB or O, and for RGB, 3 digits. O
// (oh, not zero) turns off all three. So 'LO' is valid, and turns off
// all 3 status LEDs.  LR255 is also valid and turns the red LED all
// the way up.
//
// When digits are expected, and a non digit value is received, the
// input is aborted and processing continues with the data received.
// So 'R45 ' as well as 'R045' both rotate to 45 degrees right of
// zero.


//==========================================================================
// SugarCube command constants (directives)
// All directives are upper case

const int  B = int('B');  // FUTURE EXPANSION:
                          //   Sets allowed vibration level/timeout
const int  E = int('E');  // Elevation motor       - Elevation(x) x=num degrees
const int  F = int('F');  // imaging lamps ofF     - ImagingOff()
const int  H = int('H');  // Home both motors to the zero position - Home()
const int  I = int('I');  // imaging lamps on/off (I1 or I0)
const int  N = int('N');  // imaging lamps oN      - ImagingOn()
const int  R = int('R');  // Rotation motor        - Rotation(x) x=num degrees
const int  L = int('L');  // Status LED
                          //   - Status('R'|'G'|'B'|'O') (RGB and Off)
const int  P = int('P');  // Pause, processed by the host computer, not
                          //   the Arduino, so we should never see this.
const int  S = int('S');  // reSet                 - resetCube()
const int  T = int('T');  // Take Pic, processed by the host computer, not
                          //   the Arduino so we should never see this.
const int  V = int('V');  // toggle verbose mode (with arg, sets 0 or 1)
const int  X = int('X');  // assign Data variables (d and V were taken)  Ddebug=1
const int  Z = int('Z');  // Set non-volatile memory contents (Z<some string><cr>)

// Comment directives:
//
// The open square brace '[' disables command processing until a ']'
// is received or the arduino itself is reset, such as from a power
// down.  Complimentary to that is ']', which is allowed anywhere, and
// ends the ignore functionality.  This allows us to put ']S' at the
// beginning of a command string to force a reset, clearing any
// unanticipated 'ignore commands' state.

const int  Cs = int('['); // Comment Start (ignore all letters except ']')
const int  Ce = int(']'); // Comment End (a comment end without a
                          //   comment start is allowed)

//==========================================================================
// SugarCube command constants (queries)
// Queries are all lowercase

const int  b = int('b');  // print Banner (startup message)
const int  c = int('c');  // report Check ready status     - CheckReady()
const int  d = int('d');  // report Door status            - StatusDoor()
const int  e = int('e');  // report elevation at zero pos  - StatusElevation()
const int  r = int('r');  // report rotation at zero pos   - StatusRotation()
const int  t = int('t');  // report twelve volt status     - Status12V()
const int  v = int('v');  // report current version        - StatusVersion()
const int  x = int('x');  // report variables ('v' was taken)
const int  z = int('z');  // report Non-Volatile memory contents

//==========================================================================
// Arduino Interfaces
const int   pinElevSense   = A1; // Analog pin for elevation home sensor
                                 //   ~0 = At Home Pos
const int   pinRotSense    = A2; // Analog pin for rotation home sensor
                                 //   ~0 = At Home Pos
const int   pinImaging     = A0; // Analog input pin for Imaging lamps
								 // control which we're using as a
								 // digital output.

// position sensors are analog optical
// With positionSensorThreshold at 400, motor doesn't rotate fully left
const int   positionSensorThreshold = 900;

// pin12VSense reads the voltage divider that divides by 3 to keep the
// pin voltage < 5V.  The voltage will be around 4V when 12V is
// available, and around 1.5V if it isn't.  Pin12VSense should return
// a value above 500 when 12V is available.
const int   pin12VSense    = A3; // Analog pin for 12 Volt Sensing

// Is 12v present?  This is through a 3:1 voltage divider.
// 1024 = 15.00v
// 819 = 12.00v
// 700 = 10.25v
// 500 = 7.32v
const int   pwrOKthreshold = 500;  // Consider 12V power OK above this level.

const int   pinDrawerSense = A4; // Analog pin for drawer status sense

// Slight problem: We're 1 short of PWM-capable pins, so using
// digital pin 2 for red.  The other PWM capable pins are already being
// used by the motor control shield/chip.
const int pinRed          = 2;  // Digital pin for status lamp control
const int pinGreen        = 3;  // Analog pin for status lamp control
const int pinBlue         = 9;  // Analog pin for status lamp control

const int LEDburnout      = 200; // Above this value, the Arduino pins
				 // for green and blue might burn out
				 // because the current-limiting
				 // resistor is a bit too small for
				 // 5v.  Future hardware will fix this.
const int redFullBright   = 1;  // Digital, so 0=off, 1=on.
const int greenFullBright = 6;  // Max brightness value.  Caution: Do
				// not increase this above 200, lest
				// you burn out the Arduino pin, which
				// has an insufficient
				// current-limiting resistor for the
				// full 5v.
const int blueFullBright  = 6;  // See above caution about maximum value.


// Motor uses pins 4, 10, 7 and 8.

// Motor constants
// rotationDegreesPerStep = 1.8;
// If using whole steps:
// deg -> steps, error
// 0 -> 0, 0
// 1 -> 1.8, 0.8
// 2 -> 1.8, 0.2
// 3 -> 3.6, 0.6
// 4 -> 3.6, 0.4
// 5 -> 5.4, 0.4
// 6 -> 5.4, 0.6
// 7 -> 7.2, 0.2
// 8 -> 7.2, 0.8
// 9 -> 9.0, 0

// elevationDegreesPerStep = 0.70175 maybe

//const int   Rlim = 400;    // rotation step count limit (one shaft rotation, INTERLEAVEd)
const int   Rlim = 200;    // rotation step count limit (one shaft rotation, NON-INTERLEAVEd) amm
const int   Elim = 128;    // elevation step count limit (1/4 shaft rotation, single step)

const int   left = FORWARD;
const int   right = BACKWARD;
const int   up = BACKWARD;
const int   down = FORWARD;


//==========================================================================
// Motor control primitives

// The "raw" motors on ports 1 and 2.
AF_Stepper elevMotor(ELEV_MOTOR_STEPS, 1);
AF_Stepper rotMotor(ROT_MOTOR_STEPS, 2);

// Except for motorHome(), we don't use elevMotor or rotMotor
// directly, instead we use the AccelStepper wrapped version of the
// motors below
void upStepM1() {
  //elevMotor.onestep(BACKWARD, INTERLEAVE); // adafruit lib
  elevMotor.onestep(BACKWARD, SINGLE); // adafruit lib - AMM
}
void downStepM1() {
  //elevMotor.onestep(FORWARD, INTERLEAVE); // adafruit lib
  elevMotor.onestep(FORWARD, SINGLE); // adafruit lib - AMM
}

void fwStepM2() {
  //rotMotor.onestep(FORWARD, INTERLEAVE); // adafruit lib
  rotMotor.onestep(FORWARD, SINGLE); // adafruit lib - AMM
}
void bwStepM2() {
  //rotMotor.onestep(BACKWARD, INTERLEAVE); // adafruit lib
  rotMotor.onestep(BACKWARD, SINGLE); // adafruit lib - AMM
}

// After motorHome(), these, not elevMotor or rotMotor, are what we control
AccelStepper elevAccelMotor(downStepM1, upStepM1);
AccelStepper rotAccelMotor(bwStepM2, fwStepM2);


//============================================================================
// LED Control
void StatusLEDOff(bool report);

void StatusLEDBlue(int newValue, bool report)
{
  analogWrite(pinBlue, newValue);
  if (report == ENABLE_REPORT) //CU
    reportStatusToSerial(SUCCESS, "SetLED Blue", "\n");
}

void StatusLEDRed(int newValue, bool report)
{
  digitalWrite(pinRed, newValue);
  if (report == ENABLE_REPORT)
    reportStatusToSerial(SUCCESS, "SetLED Red", "\n");
}

void StatusLEDGreen(int newValue, bool report)
{
  analogWrite(pinGreen, newValue);
  if (report == ENABLE_REPORT) //CU
    reportStatusToSerial(SUCCESS, "SetLED Green", "\n");
}

void StatusLEDOff(bool report)
{
  digitalWrite(pinRed, 0);
  analogWrite(pinGreen, 0);
  analogWrite(pinBlue, 0);
  if (report == ENABLE_REPORT)
    reportStatusToSerial(SUCCESS, "SetLED Off", "\n");
}


//==========================================================================
// ImagingControl(on|off)

// Turn Imaging Lamp on or off
void ImagingControl(int onOrOff)
{
  if (onOrOff)
    digitalWrite(pinImaging, 1); // 255);
  else
    digitalWrite(pinImaging, 0);
  // analogWrite doesn't return anything, but might someday want to
  // check that it worked if we have hardware to do so.
}

//==========================================================================
// Motor control and position sensing

bool isRotationHomeSensed();
bool isElevationHomeSensed();
int cubeHomeRotation();

// Rotation Motor Control
//
// Rotate to <destination> degrees
// Return status 0 on success
//
// The target position is the closest possible whole-step match to the
// requested number of degrees, given the stepping resolution of the
// motor.
float cubeRotate(int destDegreePos) {
  // (400 steps / 360 degrees).  The 2.0 is for INTERLEAVEd motor control.
  //const float stepsPerDegree = ROT_MOTOR_STEPS * 2.0 / 360.0;
  const float stepsPerDegree = ROT_MOTOR_STEPS * 1.0 / 360.0;  //amm
  int destStep;
  float actualDegrees;
  bool homed;

  vprint("requested ");
  vprintDec(destDegreePos);
  vprint(" degrees (step ");
  destStep = int(destDegreePos * stepsPerDegree + 0.5);
  vprintDec(destStep);
  vprint(" is ");
  actualDegrees = destStep / stepsPerDegree;
  vprintDec(actualDegrees);
  vprintln(" deg)");

  if (destStep != 0) {
    delay(50);
	rotMotor.release();
	rotAccelMotor.runToNewPosition(destStep);  // blocking
  }
  //rotAccelMotor.moveTo(destStep); // nonblocking.  Would need to
  //call an accel function regularly in loop() for this to work.

  homed = isRotationHomeSensed();
  if (destStep == 0 && !homed) {
	rotErr = false;
    int tryAgain;

    //reportStatusToSerial(ERROR_ROTATION_HOME_NOT_SENSED, "expected rotation home sensor at step 0", "\n");
    tryAgain = cubeHomeRotation();
    //if (tryAgain != SUCCESS) {
    //  reportStatusToSerial(tryAgain, "Unable to zero rotation.", "  ");
    //}
	homed = isRotationHomeSensed();
	rotErr = !homed;

  } //else if (destStep != 0 && homed) {
    //reportStatusToSerial(LIMIT_SENSED_ROTATION, "Sensor says rotated home unexpectedly.", "  ");
  //}

  homed = isRotationHomeSensed();
  if (homed) {
    rotErr = false;
	rotAccelMotor.setCurrentPosition(0); //amm
  } 
  
  return actualDegrees;
}


int cubeHomeElevation();

float cubeElevate(int destDegreePos)
{
  // 2.0 is for INTERLEAVE (effectively doubles number of steps),
  // gearing also multiplies number of steps per rotation, 360 converts
  // steps/rot to degrees/rot
  // const float stepsPerDegree = (ELEV_MOTOR_STEPS * 2.0 * ELEV_MOTOR_GEARING) / 360.0;
  const float stepsPerDegree = (ELEV_MOTOR_STEPS * 1.0 * ELEV_MOTOR_GEARING) / 360.0; // amm
  int destStep;
  float actualDegrees;
  bool homed;

  vprint("requested ");
  vprintDec(destDegreePos);
  vprint(" degrees (step ");
  destStep = int(destDegreePos * stepsPerDegree + 0.5);
  vprintDec(destStep);
  vprint(" is ");
  actualDegrees = destStep / stepsPerDegree;
  vprintDec(actualDegrees);
  vprintln(" deg)");

  if (destStep != 0) {
	elevAccelMotor.runToNewPosition(destStep);  // blocking
  }
  // There is no feedback from runToNewPosition, so just assuming it
  // worked.  If we had hardware with feedback and wanted to add some
  // kind of check, this is where we would do it.

  // Hey look we do have one bit of feedback -- if destStep was 0, the
  // elevation home sensor should say so, otherwise it shouldn't.
  homed = isElevationHomeSensed();
  if (destStep == 0 && !homed) {
	elevErr = false;
    int tryAgain;

    //reportStatusToSerial(ERROR_ELEVATION_HOME_NOT_SENSED, "Elevation home not sensed", "  ");
    tryAgain = cubeHomeElevation();
    //if (tryAgain != SUCCESS) {
    //  reportStatusToSerial(tryAgain, "Unable to zero elevation.", "  ");
    //}
	homed = isElevationHomeSensed();
	elevErr = !homed;

	//} else if (destStep != 0 && homed) {
    //reportStatusToSerial(LIMIT_SENSED_ELEVATION, "Sensor says elevation home unexpectedly.", "  ");
  }

  homed = isElevationHomeSensed();
  if (homed) {
	elevErr = false;
	elevAccelMotor.setCurrentPosition(0); //amm
  }

  return actualDegrees;
}

bool isSensorZero(int thePin) {
  int value = 0;

  // read the analog value on the (e.g., rotation) sense input pin
  value = analogRead(thePin);
  if (debug) {
    vprint("sense(");
    vprintDec(thePin);
    vprint(") = ");
    vprintDec(value);
    vprintln("");
  }

  // If it's high enough, report that we're zeroed.
  if (value > positionSensorThreshold)    // Currently using one
    return true;			  // threshold for the
					  // sensors, but might want two.

  // We're somewhere else
  return false;
}



//  cubeHomeRotation() - Sets rotation motor to home position using the
//                       rotation limit sensor
//
//  returns: SUCCESS = command executed ok
//           ERROR_OVERROTATE_ROTATION = rotation limit exceeded
//
// Steps left until the motor has reached the home position (as
// reported by the input from that optical switch). if isRotSensorZero()
// doesn't return true after some maximum amount of rotation, cubeHome()
// returns an error.
int cubeHomeRotation() {
  int returnValue = ERROR_DEFAULT;
  int stepCount = 0;
  bool atZero = false;

  // Step up until we sense home or too many steps
  while (!atZero && stepCount < Rlim) {
    atZero = isSensorZero(pinRotSense);

    // Are we there yet?
    if (!atZero) {
      // No.
      //rotMotor.step(1, left, INTERLEAVE);  // DOUBLE was a bit rough
      rotMotor.step(1, left, SINGLE);  // DOUBLE was a bit rough
      stepCount += 1;
	  delay(10);

      if (debug) {
		vprint("stepCount = ");
		vprintDec(stepCount);
		vprintln("");
      }

    }
  }
  // Might want to not do this if we're at an (INTERLEAVEd) half-step.
  delay(300);
  rotMotor.release();

  // If we got out by sensing the limit, we're good.
  if (atZero) {
    returnValue = SUCCESS;
	rotAccelMotor.setCurrentPosition(0); //amm
  } 
  else
    // Home not sensed here, we must have tried stepping too far, so
    // report that error
    if (stepCount >= Rlim)
      returnValue = ERROR_OVERROTATE_ROTATION;
    else
      assert(false);  // This shouldn't be possible.

  return returnValue;
}

// Ought to factor this and cubeHomeRotation into cubeHomeMotor(whichMotor)
int cubeHomeElevation() {
  int returnValue = ERROR_DEFAULT;
  int stepCount = 0;
  bool atZero = false;

  // Step up until we sense home or too many steps
  while (!atZero && stepCount < Elim) {
    atZero = isSensorZero(pinElevSense);

    // Are we there yet?
    if (!atZero) {
      // No.
      elevMotor.step(1, right, SINGLE);  // DOUBLE was a bit rough
      stepCount += 1;

      vprint("stepCount = ");
      vprintDec(stepCount);
      vprintln("");
    }
  }
  
  atZero = isSensorZero(pinElevSense);
  if (!atZero) {
      elevMotor.step(20, left, SINGLE);  // DOUBLE was a bit rough
	  stepCount = 0;
  }  
  while (!atZero && stepCount < Elim) {
    atZero = isSensorZero(pinElevSense);

    // Are we there yet?
    if (!atZero) {
      // No.
      elevMotor.step(1, right, SINGLE);  // DOUBLE was a bit rough
      stepCount += 1;

      vprint("stepCount = ");
      vprintDec(stepCount);
      vprintln("");
    }
  }
  // Reasons to not release: It draws less current than the rotation
  // motor, plus we have gravity and the spring trying to move it.
  // Reasons to release: It could be sitting there powered for days
  // after the cube is started up.
  //elevMotor.release();

  // If we got out by sensing the limit, we're good.
  if (atZero) {
    returnValue = SUCCESS;
	elevAccelMotor.setCurrentPosition(0); //amm
  }
  else
    // Home not sensed here, we must have tried stepping too far, so
    // report that error
    if (stepCount >= Elim)
      returnValue = ERROR_OVERROTATE_ELEVATION;
    else
      assert(false);  // This shouldn't be possible.

  return returnValue;
}


// cubeHome: home both the rotation and elevation motors.
// return SUCCESS or error code reported by motor homing function.

int cubeHome(bool report)
{
  int returnValue = ERROR_DEFAULT;   // assume an error

  // Home the elevation motor first to avoid dragging it around floor
  returnValue = cubeHomeElevation();
  if (debug) {
    Serial.print(F("intermediate cubeHomeElevation()="));
    Serial.println(returnValue);
  }
  if (returnValue == SUCCESS) {
    // That went OK, Home the rotation motor
    returnValue = cubeHomeRotation();
  }

  if (report)
    reportStatusToSerial(returnValue, "Home", "\n");

  return returnValue;
}

int Status12V(bool report);
int StatusDoor(bool report);

// Status reporting functions
int CheckReady() //CU
{
  int returnValue = ERROR_DEFAULT;

  // check 12v
  analogWrite(pinRed, 1);
  returnValue = Status12V(DISABLE_REPORT);
  if (returnValue != SUCCESS)
    return reportStatusToSerial(returnValue, "CheckReady 12v failure", "\n");

  // check door
  returnValue = StatusDoor(DISABLE_REPORT);
  if (returnValue != SUCCESS)
    return reportStatusToSerial(returnValue, "CheckReady Door Open failure", "\n");

  returnValue = SUCCESS;
  // set status light
  StatusLEDOff(DISABLE_REPORT);
  StatusLEDBlue(blueFullBright, DISABLE_REPORT);

  return reportStatusToSerial(returnValue, "CheckReady", "\n");
}

int StatusDoor(bool report)
{
  int returnValue = ERROR_DEFAULT;

  if (isSensorZero(pinDrawerSense))
    returnValue = SUCCESS;
  else
    returnValue = DRAWER_OPEN;

  if (report) {
    if (returnValue == SUCCESS)
      reportStatusToSerial(returnValue, "StatusDoor", "\n");
    else
      reportStatusToSerial(returnValue, "StatusDoor Drawer Open", "\n");
  }

  return returnValue;
}


// Is the Elevation sensed to be at home? (i.e. above the threshold)?
bool isElevationHomeSensed()
{
  return isSensorZero(pinElevSense);
}

// Is the Rotation sensed to be at home?
bool isRotationHomeSensed()
{
  return isSensorZero(pinRotSense);
}


// Return SUCCESS if 12v power is OK, 1 if not
// print report or not depending on <report>
int Status12V(bool report)
{
  int returnValue = ERROR_DEFAULT;  // assume an error
  int pwrLevel;

  pwrLevel = analogRead(pin12VSense);

  if (debug) {
    vprint("power level = ");
    vprintDec(pwrLevel);
  }

  if (pwrLevel > pwrOKthreshold)
    returnValue = SUCCESS;  // We're above the threshold
  else
    returnValue = ERROR_12v;

  if (report == ENABLE_REPORT)
    reportStatusToSerial(returnValue, "Status12v", "\n");

  return returnValue;
}

// Common to the banner and the v command
void printVersion() {
  Serial.print(version);
  Serial.print(F(".r"));
  Serial.print(F(SVNVERSION));
}

void statusVersion()
{
  // This is a weird order for the host software, but here it is:
  Serial.print (SUCCESS);
  Serial.print(F(": Version OK: "));
  printVersion();
  Serial.println();
}


// Set cube to ready state:
//   Check 12v, position motors to zero (home), lights out, status green
//
//   If anything fails, leave the LED red and report an error
int resetCube() {
  int returnValue = 1;  // Assume an error

  returnValue = Status12V(DISABLE_REPORT);  // 0 if power OK
  if (returnValue != SUCCESS) goto reset_fail;

  returnValue = cubeHome(DISABLE_REPORT);
  if (returnValue != SUCCESS) goto reset_fail;

  ImagingControl(OFF);
  StatusLEDOff(DISABLE_REPORT);
  StatusLEDBlue(greenFullBright, DISABLE_REPORT); //amm was here

 reset_fail:  // Also falls through to here on success, so check returnValue
  if (returnValue != SUCCESS)
    StatusLEDRed(1, DISABLE_REPORT);

  return reportStatusToSerial(returnValue, "Reset", "\n");
}

void serialDiscard() {
  while(Serial.available())
    Serial.read();
}

// Print the startup/version banner
void printBanner() {
  Serial.print(F("\n\n\nSugarCube v"));
  printVersion();
  Serial.print(F(", built "));
  Serial.print(F(__DATE__));
  Serial.print(F(" "));
  Serial.print(F(__TIME__));
}


//=============================================================================
// Arduino functions setup() and loop()
//


// Setup: serial, pin i/o modes, motors, and resetCube()
void setup() {
  // 9600 bps for the SugarCube
  Serial.begin(9600);
  serialDiscard();      // Discard any queued input, probably not
			// necessary, but trying to get rid of weird
			// startup glitches.

  printBanner();
  Serial.print(F(" starting"));
  Serial.flush();

  // Don't use xxxMotor except for Homing.
  // Instead, use xxxAccelMotor below.
  // 50 for alpha motor with spring in beta box
  //
  // These are the raw Adafruit motors, and this and cubeHome() are
  // the only places we talk to them.  (We single-step in cubeHome().)
  elevMotor.setSpeed(50);  // RPM
  // rotation.  beyond 27, my box skips steps -- dg
  rotMotor.setSpeed(25);   // RPM

  // Set up the Accel versions of the motors
  // Rotation
  rotAccelMotor.setMaxSpeed(ROT_MAX_SPEED);
  rotAccelMotor.setAcceleration(ROT_ACCELERATION);

  // Elevation
  elevAccelMotor.setMaxSpeed(ELEV_MAX_SPEED);
  elevAccelMotor.setAcceleration(ELEV_ACCELERATION);

  pinMode(pinElevSense, INPUT);   // elevation sense
  pinMode(pinRotSense, INPUT);   // rotation sense
  pinMode(pin12VSense, INPUT);   // 12V sense
  pinMode(pinDrawerSense, INPUT);   // drawer sense

  pinMode(pinImaging, OUTPUT);  // Illumination light control

  pinMode(pinRed, OUTPUT);
  pinMode(pinGreen, OUTPUT);
  pinMode(pinBlue, OUTPUT);

  StatusLEDOff(DISABLE_REPORT);
  StatusLEDBlue(blueFullBright, DISABLE_REPORT);
  // was:
  // analogWrite(pinGreen, 0);
  // analogWrite(pinBlue, GREEN_FULL_BRIGHT);

  delay(10); // ms
  Serial.println(F(" up."));

  // Make the cube ready to image

  vprint("Resetting. ");
  if (resetCube() == SUCCESS) { // checks home sensors so we don't have to.
    vprint("Calibrating. ");

    // Let's try the full range of movement and back
    cubeRotate(0);
    cubeRotate(350);

    StatusLEDOff(DISABLE_REPORT);
    StatusLEDBlue(blueFullBright, DISABLE_REPORT);

    cubeElevate(85);
    cubeElevate(0);

    if (!isElevationHomeSensed())
      Serial.println(F(" failure: Elevation not homed."));

    cubeRotate(0);
    if (!isRotationHomeSensed())
      Serial.println(F(" failure: Rotation not homed."));
  }

  // leave green
  StatusLEDOff(DISABLE_REPORT);
  StatusLEDBlue(blueFullBright, DISABLE_REPORT);

  printRAM();
  resetCube();
}

// This is where stuff that should get run as often as possible could
// go.  It's not the Arduino loop() because that contains all kinds of
// stateful code.  This is more like an interrupt routine (and might
// eventually be called from a timer interrupt.
void systemEvent() {
  return;
}

void waitForCharAvailable() {
  while (Serial.available() == 0)  // block until a char is available
    systemEvent();
}

// If there next input is a digit, set verbose to that.
// If not, just toggle verbose.
void toggleOrSetVerbose() {

  waitForCharAvailable();

  if (isDigit(Serial.peek()))
    verbose = Serial.parseInt();
  else
    verbose = !verbose;

  if (verbose) {
    vprint("Verbose on.");
  }

  reportStatusToSerial(SUCCESS, "Set Verbose", "\n");
}


// Wait until a char is available, then return it.
char waitForAchar() {
  while (Serial.available() <= 0) 	updateErrorLamps(); //amm
  return Serial.read();  // reads a single char
}


// Read (the rest of) a line of input.  Bounds check to limit, and
// return only the length we read.  (We may have already crashed if
// there was an overrun, but it could save a count later.)

int readline(char *buffer, int limit)
{
  uint8_t i = 0;
  char c;

  do {
    c = waitForAchar();
    maybe_echo(c);
    buffer[i++] = c;
  } while (c != '\n' && c != '\r' && i < limit);

  buffer[--i] = 0;  // Stomp on the <cr> or <nl> with a null terminator
  return i;
}

void setVariable() {
  char lineBuf[255];  // Caution: Arbitrary limit here
  int len;
  char *equalsIndex;
  char *varStr, *valueStr;

  // Variables we can set are:  echo, debug, verbose
  len=readline(lineBuf, 255);
  equalsIndex = strchr(lineBuf, '=');
  *equalsIndex = '\0';  // Turn = into null terminator
  varStr = lineBuf;
  valueStr = ++equalsIndex;
  vprint("\n'");
  vprint(varStr);
  vprint("<=");
  vprint(valueStr);
  vprintln("'");
}

void reportVariables() {
  if (!verbose) {
    // This is to prevent confusing the host software
    reportStatusToSerial(ERROR_REPORT_VARS,
			 "Report Variables: Verbose Not Set", "\n");
    return;
  }
  Serial.print(F("echo = ")); Serial.println(echo);
  Serial.print(F("verbose = ")); Serial.println(verbose);
  Serial.print(F("debug  = ")); Serial.println(debug);
  Serial.println(F("Constants:"));
  Serial.print(F("ELEV_MAX_SPEED = ")); Serial.println(ELEV_MAX_SPEED);
  Serial.print(F("ELEV_ACCELERATION = ")); Serial.println(ELEV_ACCELERATION);
  Serial.print(F("ROT_MAX_SPEED = "));  Serial.println(ROT_MAX_SPEED);
  Serial.print(F("ROT_ACCELERATION = "));  Serial.println(ROT_ACCELERATION);
}

// ------- CRC stuff here, could well go in its own file  --------
#include <avr/pgmspace.h>

static const PROGMEM unsigned long int crc_table[16] = {
  0x00000000, 0x1db71064, 0x3b6e20c8, 0x26d930ac,
  0x76dc4190, 0x6b6b51f4, 0x4db26158, 0x5005713c,
  0xedb88320, 0xf00f9344, 0xd6d6a3e8, 0xcb61b38c,
  0x9b64c2b0, 0x86d3d2d4, 0xa00ae278, 0xbdbdf21c
};

unsigned long crc_update(unsigned long crc, byte data)
{
  byte tbl_idx;
  tbl_idx = crc ^ (data >> (0 * 4));
  crc = pgm_read_dword_near(crc_table + (tbl_idx & 0x0f)) ^ (crc >> 4);
  tbl_idx = crc ^ (data >> (1 * 4));
  crc = pgm_read_dword_near(crc_table + (tbl_idx & 0x0f)) ^ (crc >> 4);
  return crc;
}

// crc_string("some string") will give you a 4 byte CRC
unsigned long crc_string(char *s)
{
  unsigned long crc = ~0L;
  while (*s)
    crc = crc_update(crc, *s++);
  crc = ~crc;
  return crc;
}

// -------- End of CRC stuff -----------

// Write a 4 byte int <value> to EEPROM at <addr>
void writeLongToEEPROM(const int addr, unsigned long int wValue) {
  int i;

  if (debug) {
    Serial.print("write 0x");
    Serial.print(wValue, HEX);
    Serial.print(F(" to 0x"));
    Serial.println(addr, HEX);
  }

  for (i = addr; i < addr + 4; i++) {
    byte b;

    // I guess we'll go with big-endian
    b = (wValue & 0xff000000) >> 24;
    wValue = wValue << 8;

    EEPROM.write(i, b);
  }
}

// Return a 4 byte int from EEPROM address <addr>
unsigned long int readLongFromEEPROM(const int addr) {
  int i;
  unsigned long int theLong = 0;

  for (i = addr; i < addr + 4; i++) {
    byte b;
    b = EEPROM.read(i);
    theLong = theLong << 8;
    theLong = theLong | b;
  }
  if (debug) {
    Serial.print(F("read "));
    Serial.print(theLong, HEX);
    Serial.print(F(" from 0x"));
    Serial.println(addr, HEX);
  }
  return theLong;
}

// Memory layout: 4 bytes for magic cookie, 4 bytes for CRC, 4 bytes
// for write count, <n> bytes for string, then null terminator.
// Limited to (12 bytes +) zBufferSize.
#define MAGIC_COOKIE_OFFSET 0
#define CRC_OFFSET 4
#define WRITE_COUNT_OFFSET 8
#define EEPROM_STRING_OFFSET 12
#define MAGIC_COOKIE 0xfeedf00d

// Read a line, store (overwriting old value, if necessary) in EEPROM.
// Note that readline() includes null terminator as is required.
int storeNV() {
  char lineBuf[zBufferSize];
  int i, j, len;
  unsigned long int crc, write_count;
  unsigned long int mc;

  len = readline(lineBuf, zBufferSize);
  vprint("line(");
  vprintDec(len);
  vprint("): ");
  vprintln(lineBuf);

  if (verbose) {
    printRAM();
  }

  // Thuper thecret "Zzero" command to zero out Arduino.  Doesn't pave
  // over write count, but in case you want to write zeros everywhere,
  // use "Zzero!"  Used for testing EEPROM code.
  if (!strcmp(lineBuf, "zero")) {
    writeLongToEEPROM(MAGIC_COOKIE_OFFSET, 0L);
    writeLongToEEPROM(CRC_OFFSET, 0L);
    if (lineBuf[4] == '!') {
      writeLongToEEPROM(WRITE_COUNT_OFFSET, 0L);
    }
    for (i=EEPROM_STRING_OFFSET; i < EEPROM_STRING_OFFSET + zBufferSize ; i++) {
      EEPROM.write(i, 0);
    }
    return reportStatusToSerial(SUCCESS, "Write EEPROM Zeroed", "\n");
  }

  mc = readLongFromEEPROM(MAGIC_COOKIE_OFFSET);
  if (mc == MAGIC_COOKIE or mc==0) {
    // This could be included in the CRC; we'd have to read the buffer
    // and check CRC before writing.  Or we just do this.
    write_count = readLongFromEEPROM(WRITE_COUNT_OFFSET);
  } else {
    write_count = 0;
  }

  crc = crc_string(lineBuf);
  writeLongToEEPROM(CRC_OFFSET, crc);

  // lineBuf needs to include null terminator
  i = 0;
  j = EEPROM_STRING_OFFSET;
  do {
    EEPROM.write(j++, lineBuf[i]);
  } while (lineBuf[i++] != '\0' && i < zBufferSize);

  write_count++;
  writeLongToEEPROM(WRITE_COUNT_OFFSET, write_count);

  // Don't write magic cookie until we've written the valid string,
  // write count, and CRC.  If it's already the magic cookie, save the
  // EEPROM writes.
  if (mc != MAGIC_COOKIE) {
    writeLongToEEPROM(MAGIC_COOKIE_OFFSET, MAGIC_COOKIE);
  }

  return reportStatusToSerial(SUCCESS, "Write EEPROM", "\n");
}


int retrieveNV() {
  char lineBuf[zBufferSize];
  int i, j;
  unsigned long int mc, read_crc, computed_crc;

  mc = readLongFromEEPROM(MAGIC_COOKIE_OFFSET);
  if (mc != MAGIC_COOKIE) {
    return reportStatusToSerial(UNINITIALIZED_EEPROM,
				"GetData: Uninitialized EEPROM", "\n");
  }
  read_crc = readLongFromEEPROM(CRC_OFFSET);

  i = 0;
  j = EEPROM_STRING_OFFSET;
  do {
    lineBuf[i] = EEPROM.read(j++);
  } while (lineBuf[i++] != '\0' && i < zBufferSize);

  computed_crc = crc_string(lineBuf);

  if (debug) {
    Serial.print("line = ");
    Serial.println(lineBuf);

    Serial.print("crc = ");
    Serial.println(computed_crc, HEX);
    if (computed_crc != read_crc) {
      Serial.print("read crc = ");
      Serial.println(read_crc, HEX);
    }
  }

  if (verbose) {
    long int write_count;

    write_count = readLongFromEEPROM(WRITE_COUNT_OFFSET);
    Serial.print("write count = ");
    Serial.println(write_count);
  }

  if (computed_crc != read_crc) {
    return reportStatusToSerial(CRC_ERROR, "GetData:  CRC Error", "\n");
  }

  Serial.print("0: GetData OK: ");
  Serial.println(lineBuf);

  return SUCCESS;
}


// updateErrorLamps() // amm
void updateErrorLamps()
{
  // update status LED if error
  // first check for 12V error
  if (Status12V(DISABLE_REPORT) != SUCCESS) {
	StatusLEDOff(DISABLE_REPORT);
	StatusLEDRed(redFullBright, DISABLE_REPORT);	
  } else {
	// now check for motor errors
	if (rotErr == true || elevErr == true) {
		StatusLEDOff(DISABLE_REPORT);
		StatusLEDRed(redFullBright, DISABLE_REPORT);
		StatusLEDBlue(blueFullBright, DISABLE_REPORT);
	}
  }
}

// loop() reads input and executes it.
//
// It ignores spaces, CR, LF... Anything "whitespacy" Whitespace also
// signals the (allowed) premature end of entering an argument to a
// directive.  If the Loop() encounters '[', it ignores other
// characters until a ']' is received, but it keeps processing the
// test loop.

void loop()  {
  char serialInput;

  int angle = 0;

  // For verbose, print a prompt
  if (verbose) {
    Serial.print("> ");
    Serial.flush();
  }

  do {
    serialInput = waitForAchar();
    if (debug && isSpace(serialInput)) vprint(".");
	updateErrorLamps();
  } while (isSpace(serialInput));
	

  // Now we have a separate "echo on" instead of verbose
  maybe_echo(char(serialInput));
  if (ignoringInput && serialInput == Ce)  // End of a comment
    ignoringInput = false;
  if (!ignoringInput) {
    // The following cases are in alphabetical order, with upper/lower
    // versions of commands next to each other, except that Comments
    // are first because Comment End has to go before the switch().
    switch (serialInput) {
		case Cs: { // Comment Start
		  ignoringInput = true;
		  break;
		}
		case Ce: { // Comment End
		  // Already (necessarily) handled above switch()
		  break;
		}
		case b: { // Print banner
		  printBanner();
		  Serial.println();
		  break;
		}
		case c: { // Check Ready Status
		  vprintln("");
		  CheckReady();
		  break;
		}
		case d: {
		  vprintln("");
		  StatusDoor(ENABLE_REPORT);
		  break;
		}
		case E: { // eLevate needs more input, an int
		  float actualDegrees;
		  char reportStr[11] = "xxx.yyyy  ";
		  char *s;

		  vprint("? ");
		  angle = Serial.parseInt();
		  if (verbose) {
			Serial.print(angle);
		  }
		  if (verbose) {
			Serial.print(F("Elevate to "));
			Serial.print(angle);
			Serial.println(F(" degrees from top."));
		  }
		  actualDegrees = cubeElevate(angle);
		  s = dtostrf(actualDegrees, 8, 4, reportStr);
		  reportStatusWithData(SUCCESS, "Elevate", reportStr);
		  break;
		}
		case e: { // Elevation at home position?
		  vprintln("");
		  if (isElevationHomeSensed()) {
			reportStatusToSerial(SUCCESS, "Elevation Homed", "\n");
		  } else {
			reportStatusToSerial(ERROR_ELEVATION_HOME_NOT_SENSED, "Elevation not Homed", "\n");
		  }
		  break;
		}
		case F: { // Imaging lamps oFf.
		  vprintln("");
		  ImagingControl(OFF);
		  reportStatusToSerial(SUCCESS, "Imaging Off", "\n");
		  break;
		}
		case H: { // Home both motors, leave motors energized
		  cubeHome(ENABLE_REPORT);
		  break;
		}
		case I: { // Imaging lamps on/off
		  int setting;
		  char *reportStr;

		  vprint("?");  // prompt for parameter if verbose
		  Serial.flush();
		  setting = Serial.parseInt();
		  if (echo)
			Serial.println(setting);
		  if (setting != 0)  // Make it be zero or one
			setting = 1;
		  ImagingControl(setting);
		  reportStr = (char *)"SetImaging(X)";
		  reportStr[11] = '0' + setting;  // Change the X to a 0 or 1.
		  reportStatusToSerial(SUCCESS, reportStr, "\n");
		  break;
		}
		case N: { // Imaging lamps oN.
		  vprintln("");
		  ImagingControl(ON);
		  reportStatusToSerial(SUCCESS, "Imaging On", "\n");
		  break;
		}
		case L: { // LED
		  char which;
		  int value = 0;  // Shut the compiler up about possibly uninitialized var

		  vprint("[RGBO]?");
		  which = waitForAchar();
		  which = toupper(which);
		  if (which == 'R' || which == 'G' || which == 'B') {
			value = Serial.parseInt();
			vprintDec(value);
		  }
		  switch (which) {
			case 'R': {
				// Red is digital anyway, so 0 = 0, anything else = 1.
				StatusLEDRed(value, ENABLE_REPORT);
				break;
			}
			case 'G': {
				// limit to burnout value
				if (value > LEDburnout) value = LEDburnout;
				StatusLEDGreen(value, ENABLE_REPORT);
				break;
			}
			case 'B': {
				// limit to burnout value
				if (value > LEDburnout) value = LEDburnout;
				StatusLEDBlue(value, ENABLE_REPORT);
				break;
			}
			case 'O': {
				StatusLEDOff(ENABLE_REPORT);
				break;
			}
		  }
		  break;
		}
		case R: {  // Rotate needs more input, an int
		  float actualDegrees;
		  char reportStr[11] = "xxx.yyyy  ";
		  char *s;

		  vprint("? ");  // prompt for it if we're verbose
		  angle = Serial.parseInt();  // This times out after about a
					  // second and doesn't echo
					  // characters.
		  vprintDec(angle);
		  actualDegrees = cubeRotate(angle);
		  s = dtostrf(actualDegrees, 8, 4, reportStr);
		  reportStatusWithData(SUCCESS, "Rotate", reportStr);
		  break;
		}
		case r: { // Rotation at home position?
		  vprintln("");
		  if (isRotationHomeSensed()) {
			reportStatusToSerial(SUCCESS, "Rotation Homed", "\n");
		  } else {
			reportStatusToSerial(ERROR_ROTATION_HOME_NOT_SENSED, "Rotation not Homed", "\n");
		  }
		  break;
		}
		case S: {  // reSet
		  vprintln("");
		  resetCube();
		  break;
		}
		case t: { // 12V OK?
		  Status12V(ENABLE_REPORT);
		  break;
		}
		case V: {
		  vprintln("");
		  toggleOrSetVerbose();
		  break;
		}
		case v: {
		  vprintln("");
		  statusVersion();
		  break;
		}
		case X: { // Set variable values (r and v were already taken).
		  vprintln("");
		  reportStatusToSerial(0, "Maybe later", "\n");
		  break;
		}
		case x: { // Report variable values (r and v were already taken).
		  reportVariables();
		  break;
		}
		case Z: {
		  storeNV();
		  break;
		}
		case z: {
		  vprintln("");
		  retrieveNV();
		  break;
		}

		default: {
      vprintln("");
      if (serialInput != '?' && serialInput != 'h') {
		Serial.print(F("unrecognized command '"));
		Serial.print((char) serialInput);
		Serial.println("'.  Type ? for more info.");
		break;
      }
      Serial.println(F("Commands:\n"));
      Serial.println(F("S -- reSet"));
      Serial.println(F("Rnnn -- Rotate to nnn"));
      Serial.println(F("Ennn -- eLevate to nnn (top = 0)"));
      Serial.println(F("N -- lamps oN"));
      Serial.println(F("F -- lamps oFf"));
      Serial.println(F("V1 -- set Verbose"));
      Serial.println(F("V0 -- set non-Verbose"));
      // Serial.println(F("X<var>=<data> -- set (RAM) variable <var> to <data>"));
      Serial.println(F("Z<string value> -- NV store"));
      Serial.println(F("z -- NV retrieve\n"));

      Serial.print(F("verbose = "));
      Serial.println(verbose);
      Serial.print(F("debug = "));
      Serial.println(debug);
      Serial.print(F("echo = "));
      Serial.println(echo);
    } // end default
    } // end switch
  } // end not ignoring input
  //TestProcessNext();
  updateErrorLamps();
}
