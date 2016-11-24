// Based on 'SoundFit Scanner Embedded Interfaces.txt' in the
// Documentation directory

// This Arduino program listens to its serial port for Commands, does
// its best to carry them out, and delivers a status code and string
// back to the serial port indicating success or failure.  Some of the
// Commands are sensing commands, which return appropriate values to
// the serial port.
//
// The hardware:
//   Controls: two stepper motors, an RGB LED, and an illumination lamp.
//   Senses:  power bus voltage, home positions for motors, and drawer open.

// MOTORS AND MOTOR CONTROL:

// We are using the AccelStepper library from the MIT guy along with
// the Adafruit Motor Shield Library.
// http://www.airspayce.com/mikem/arduino/AccelStepper/
// https://github.com/adafruit/Adafruit-Motor-Shield-library.

#include <Arduino.h>  // Arduino IDE will prepend this to the cpp file
		      // if we don't, but we want to be able to
		      // compile without the IDE.

// Workaround for http://gcc.gnu.org/bugzilla/show_bug.cgi?id=34734
#ifdef PROGMEM
#undef PROGMEM
#define PROGMEM __attribute__((section(".progmem.data")))
#endif

#include <AccelStepper.h>
#include <AFMotor.h>
#include <EEPROM.h>

// The control circuit is based on http://www.adafruit.com/products/81
// Here is its schematic:
// http://learn.adafruit.com/system/assets/assets/000/009/769/original/mshieldv1-schem.png
// The chip controlling the motors:
// http://www.digikey.com/product-detail/en/L293DNE/296-9518-5-ND/379724

#include "svnversion.h"

// The version reported with 'v' started with 2.0
const float  version = 2.012;  // current version

byte echo = 0;       // Echo characters back to tty?
byte verbose = 0;  // Verbose for humans, not for host software
byte debug = 0;    // You probably don't want this on

bool ignoringInput; // True if in a comment

// Functions send their status report ("0: reset() OK") to the serial
// port when called directly, but not when called as part of carrying
// out a higher-level command, e.g., Status12v() doesn't report when
// called by reset().
#define ENABLE_REPORT 1
#define DISABLE_REPORT 0

// Motor port #1 is elevation motor, with 48 steps per rev, and 1/16
// gearing = 8*48 = 768 steps per shaft rotation (actually 1/16.032)
// http://www.adafruit.com/products/918
#define ELEV_MOTOR_STEPS 48   // 96 with interleave

// motor port #2 is rotation motor with 200 steps per rev
// motor with 200 steps per rev http://www.adafruit.com/products/324
#define ROT_MOTOR_STEPS 200  // 400 with INTERLEAVE

#define ROT_MAX_SPEED    200              // steps per second
#define ROT_ACCELERATION ROT_MAX_SPEED*4  // full speed in 1/4 revolution

#define ELEV_MAX_SPEED    200
#define ELEV_ACCELERATION ELEV_MAX_SPEED*4 // full speed in 1/4 rev (nongeared)


//
// All the status codes (success, error, other)
//

#define SUCCESS 0
#define ERROR 1

// Motor codes
#define ERROR_12v 2
// Rotation Motor
#define ERROR_OVERROTATE_ROTATION 3
#define LIMIT_SENSED_ROTATION 4
// Elevation Motor
#define ERROR_OVERROTATE_ELEVATION 5
#define LIMIT_SENSED_ELEVATION 6
// Drawer code
#define DRAWER_OPEN 7

// Distinctive value for default, shouldn't show up
#define ERROR_DEFAULT 99

// How much Arduino RAM do we have left?
int freeRam () {
  extern int __heap_start, *__brkval;
  int v;
  return (int) &v - (__brkval == 0 ? (int) &__heap_start : (int) __brkval);
}

void printRAM() {
  Serial.print(F("free RAM = "));
  Serial.println(freeRam());
}


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
  //Serial.flush();
}

void vprintln(const char *string) {
  if (verbose) {
    Serial.println(string);
  }
  // flush not needed to get message out, but does make this blocking
  // until message is out.
  //Serial.flush();
}


// All Commands return a single digit status code *AND* a human
// readable string to the serial port. The first section after the
// digit is the command being executed. After that is the human
// readable message in a form like:
//       0: StatusReady OK
//       1: StatusReady 12V Fail
//       2: StatusReady Drawer Open Fail
//
// If the status is SUCCESS, automatically prints OK.
int reportStatusToSerial(int value, const char *msg)
{
  // Print like so:  "v: msg OK"
  if (verbose)
    Serial.println("");
  Serial.print(value);
  Serial.print(": ");
  Serial.print(msg);
  if (value == SUCCESS)
    Serial.println(" OK");
  else
    Serial.println(" Error");  // This should be the specific message

  // Report on free memory if we're debugging
  if (debug)
    printRAM();

  // Wait for it all to get out there
  Serial.flush();
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


// SugarCube commands

const int  B = int('B');  // FUTURE EXPANSION:
                          //   Sets allowed vibration level/timeout
const int  E = int('E');  // Elevation motor       - Elevation(x) x=num degrees
const int  F = int('F');  // imaging lamps ofF     - ImagingOff()
const int  H = int('H');  // Home both motors to the zero position - Home()
const int  N = int('N');  // imaging lamps oN      - ImagingOn()
const int  R = int('R');  // Rotation motor        - Rotation(x) x=num degrees
const int  L = int('L');  // Status LED
                          //   - Status('R'|'G'|'B'|'O') (RGB and Off)
const int  P = int('P');  // Pause, processed by the host computer, not
                          //   the Arduino, so we should never see this.
const int  S = int('S');  // reSet                 - reset()
const int  T = int('T');  // Take Pic, processed by the host computer, not
                          //   the Arduino so we should never see this.
const int  V = int('V'); // toggle verbose mode (with arg, sets 0 or 1)
const int  X = int('X');  // assign Variables (V was taken)  Xdebug=1
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

// SugarCube Queries (all lowercase)
const int  b = int('b');  // FUTURE EXPANSION: returns 0 if vibration falls
                          //   below threshold within the timeout
const int  c = int('c');  // report Check ready status     - StatusReady()
const int  d = int('d');  // report Door status            - StatusDoor()
const int  e = int('e');  // report elevation at zero pos  - StatusElevation()
const int  r = int('r');  // report rotation at zero pos   - StatusRotation()
const int  t = int('t');  // report twelve volt status     - Status12V()
const int  v = int('v');  // report current version        - StatusVersion()
const int  x = int('x');  // report variables ('v' was taken)
const int  z = int('z');  // report Non-Volatile memory contents

// Arduino Interfaces
const int   pinElevSense   = A1; // Analog pin for elevation home sensor
                                 //   ~0 = At Home Pos
const int   pinRotSense    = A2; // Analog pin for rotation home sensor
                                 //   ~0 = At Home Pos
const int   pinImaging     = A0; // Analog pin for Imaging lamps control

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
const int   pwrOKthreshold = 500;  // Consider 12V power OK above this level.

const int   pinDrawerSense = A4; // Analog pin for drawer status sense

// Slight problem: We're 1 short of PWM-capable pins, so using
// digital pin 2 for red.  The other PWM capable pins are already being
// used by the motor control shield/chip.
const int   pinRed         = 2;  // Digital pin for status lamp control
const int   pinGreen       = 3;  // Analog pin for status lamp control
const int   pinBlue        = 9;  // Analog pin for status lamp control


// For old-style board with only one light pin, replaced with the above
// const int RGBpin = 9;
//
//const int   RGBoff = 0;  // rgb indicator off
//const int   RedOn = 96;  // Red on
//const int   GreenOn = 160;  // Green on
//const int   BlueOn = 255; // Blue on


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

const int   Rlim = 200;    // rotation step count limit (one shaft rotation)
const int   Elim = 192;    // elevation step count limit (1/4 shaft rotation)
const int   limval = 400;  // a/d converter limit value to decide if
                           // limit switch encountered

const int   left = FORWARD;
const int   right = BACKWARD;
const int   up = BACKWARD;
const int   down = FORWARD;


// The "raw" motors on ports 1 and 2.
AF_Stepper elevMotor(ELEV_MOTOR_STEPS, 1);
AF_Stepper rotMotor(ROT_MOTOR_STEPS, 2);

// After motorHome(), we don't use elevMotor or rotMotor directly,
// instead we use the AccelStepper wrapped version of the motors below
void upStepM1() {
  elevMotor.onestep(BACKWARD, INTERLEAVE);
}
void downStepM1() {
  elevMotor.onestep(FORWARD, INTERLEAVE);
}

void fwStepM2() {
  rotMotor.onestep(FORWARD, INTERLEAVE);
}
void bwStepM2() {
  rotMotor.onestep(BACKWARD, INTERLEAVE);
}

// After motorHome(), these, not elevMotor or rotMotor, are what we control
AccelStepper elevAccelMotor(downStepM1, upStepM1);
AccelStepper rotAccelMotor(bwStepM2, fwStepM2);


//============================================================================
// Support functions
//
//
// LED Control
void StatusLEDOff();

void StatusLEDBlue(int newValue)
{
  StatusLEDOff();
  analogWrite(pinBlue, newValue);
}

void StatusLEDRed(int newValue)
{
  StatusLEDOff();
  digitalWrite(pinRed, newValue);
}

void StatusLEDGreen(int newValue)
{
  StatusLEDOff();
  analogWrite(pinGreen, newValue);
}

void StatusLEDOff()
{
  digitalWrite(pinRed, 0);
  analogWrite(pinGreen, 0);
  analogWrite(pinBlue, 0);
}


// Should probably factor ImagingOn() and ImagingOff() into
// ImagingControl(on|off)

// Turn Imaging Lamp on.
int ImagingOn()
{
  analogWrite(pinImaging, 255);
  // analogWrite doesn't return anything, but might someday want to
  // check that it worked if we have hardware to do so.
  return SUCCESS;
}

int ImagingOff()
{
  analogWrite(pinImaging, 0);
  return SUCCESS;
}

// Rotation Motor Control
//
// Rotate to <destination> degrees
// Return status 0 on success
//
// The target position is the closest possible whole-step match to the
// requested number of degrees, given the stepping resolution of the
// motor.
int cubeRotate(int destDegreePos) {
  const float degreesToSteps = 1.11111;   // (400 steps / 360 degrees)
  int destStep;

  vprint("rotating to ");
  vprintDec(destDegreePos);
  vprint(" degrees (step ");
  destStep = int(destDegreePos * degreesToSteps + 0.5);
  vprintDec(destStep);
  vprintln(")");

  rotAccelMotor.runToNewPosition(destStep);  // blocking
  //rotAccelMotor.moveTo(destStep); // nonblocking
  return SUCCESS;
}


int cubeElevate(int destDegreePos)
{
  // I have no idea how many degrees per step the elevation motor
  // actually has.  My tests show that it takes way more steps for a
  // full rotation than I expect (48 steps per rotation * 2 for
  // interleaving * 16.032 for gearing = 1539), therefore this is
  // completely made up.
  const float degreesToSteps = 2.83333;  // This is completely empirical
  int destStep;

  vprint("elevating to ");
  vprintDec(destDegreePos);
  vprint(" degrees (step ");
  destStep = int(destDegreePos * degreesToSteps + 0.5);
  vprintDec(destStep);
  vprintln(")");
  elevAccelMotor.runToNewPosition(destStep);  // blocking
  // There is no feedback from runToNewPosition, so just assuming it
  // worked.  If we had hardware with feedback and wanted to add some
  // kind of check, this is where we would do it.
  return SUCCESS;
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
      rotMotor.step(1, left, INTERLEAVE);  // DOUBLE was a bit rough
      stepCount += 1;

      if (debug) {
	vprint("stepCount = ");
	vprintDec(stepCount);
	vprintln("");
      }

    }
  }
  rotMotor.release();

  // If we got out by sensing the limit, we're good.
  if (atZero)
    returnValue = SUCCESS;

  // If we got out by stepping too far, report the error
  if (stepCount > Rlim) returnValue = ERROR_OVERROTATE_ROTATION;

  return returnValue;

}

// Ought to factor this and cubeHomeRotation into cubeHomeMotor(whichMotor)
int cubeHomeElevation() {
  int returnValue = ERROR_DEFAULT;
  int stepCount = 0;
  bool atZero = false;

  // Step up until we sense home or too many steps
  while (!atZero && stepCount < Rlim) {
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
  elevMotor.release();

  // If we got out by sensing the limit, we're good.
  if (atZero)
    returnValue = SUCCESS;

  // If we got out by stepping too far, report the error
  if (stepCount > Elim) returnValue = ERROR_OVERROTATE_ELEVATION;

  return returnValue;
}


// cubeHome: home both the rotation and elevation motors.
// return SUCCESS or error code reported by motor homing function.

int cubeHome(bool report)
{
  int returnValue = ERROR_DEFAULT;   // assume an error

  // Home the elevation motor first to avoid dragging it around floor
  returnValue = cubeHomeElevation();
  if (returnValue == SUCCESS) {
    // That went OK, Home the rotation motor
    returnValue = cubeHomeRotation();
  }

  if (report)
    reportStatusToSerial(returnValue, "Home");

  return returnValue;
}

int Status12V(bool report);
int StatusDoor(bool report);

// Status reporting functions
int StatusReady()
{
  int returnValue = ERROR_DEFAULT;

  // check 12v
  analogWrite(pinRed, 1);
  returnValue = Status12V(DISABLE_REPORT);
  if (returnValue != SUCCESS)
    return reportStatusToSerial(returnValue, "StatusReady 12v failure");

  // check door
  analogWrite(pinBlue, 200);
  returnValue = StatusDoor(DISABLE_REPORT);
  if (returnValue != SUCCESS)
    return reportStatusToSerial(returnValue, "StatusReady Door Open failure");

  returnValue = SUCCESS;
  // set status light
  analogWrite(pinGreen, 200);

  return reportStatusToSerial(returnValue, "StatusReady");
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
      reportStatusToSerial(returnValue, "StatusDoor");
    else
      reportStatusToSerial(returnValue, "StatusDoor Drawer Open failure");
  }

  return returnValue;
}

// I don't think these Status.. functions are being used, although they should be.
// Is the Elevation sensor above the threshold?
bool StatusElevation()
{
  return isSensorZero(pinElevSense);
}

// Is the Rotation Sensor above the threshold?
bool StatusRotation()
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
    reportStatusToSerial(returnValue, "Status12v");

  return returnValue;
}

void statusVersion()
{
  if (verbose) {
    Serial.print(F(":version "));
    Serial.println(version);
    Serial.flush();
  }

  // This is a weird order for non-verbosity purposes for the host
  // software, but here it is:
  reportStatusToSerial(SUCCESS, "statusVersion");
  Serial.print(F(":version "));
  Serial.print(version);
  Serial.flush();
}


// Set cube to ready state:
//   Check 12v, position motors to zero (home), lights out, status green
//
//   If anything fails, leave the LED red and report an error
int reset() {
  int returnValue = 1;  // Assume an error

  returnValue = Status12V(DISABLE_REPORT);  // 0 if power OK
  if (returnValue != SUCCESS) goto reset_fail;

  returnValue = cubeHome(DISABLE_REPORT);
  if (returnValue != SUCCESS) goto reset_fail;

  returnValue = ImagingOff();
  if (returnValue != SUCCESS) goto reset_fail;

  StatusLEDGreen(255);

 reset_fail:  // Also falls through to here on success, so check returnValue
  if (returnValue != SUCCESS)
    StatusLEDRed(255);

  return reportStatusToSerial(returnValue, "Reset");
}

void serialDiscard() {
  while(Serial.available())
    Serial.read();
}


//=============================================================================
// Arduino functions setup() and loop()
//


// Setup: serial, pin i/o modes, motors, and reset()
void setup() {
  int i;
  // 9600 bps for the SugarCube
  Serial.begin(9600);
  serialDiscard();      // Discard any queued input, probably not
			// necessary, but trying to get rid of weird
			// startup glitches.

  Serial.print(F("\n\n\nSugarCube v"));
  Serial.print(version);
  Serial.print(F(" built "));
  Serial.print(F(__DATE__));
  Serial.print(F(" "));
  Serial.print(F(__TIME__));
  Serial.print(F(" from svn "));
  Serial.print(F(SVNVERSION));
  Serial.print(F(" starting"));

  // Don't use xxxMotor except for Homing.
  // Instead, use xxxAccelMotor below.
  // 50 for alpha motor with spring in beta box
  elevMotor.setSpeed(50);
  // rotation.  beyond 27, my box skips steps -- dg
  rotMotor.setSpeed(25);

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

  pinMode(pinImaging, OUTPUT);  // light control
  // pinMode(RGBpin, OUTPUT);  // RGB control (old-style)

  pinMode(pinRed, OUTPUT);   // Will be new style
  pinMode(pinGreen, OUTPUT); // Will be new style
  pinMode(pinBlue, OUTPUT);  // Will be new style

  // Cycle through RGB
  for (i = 0; i < 1; i++) {
    int d = 50;
    digitalWrite(pinRed, 1);  // Would like analog, but ran out of PWM pins
    delay(d);
    analogWrite(pinBlue, 200);
    delay(d);
    analogWrite(pinGreen, 200);
    delay(d);
    analogWrite(pinRed, 0);  // XYZZY
    delay(d);
  }

  // leave green
  analogWrite(pinGreen, 200);
  delay(10);

  Serial.println(F(" up."));
  // Make the cube ready to image

  printRAM();
  reset();
}

// This is where stuff that should get run as often as possible could
// go.  It's not the Arduino loop() because that contains all kinds of
// stateful code.  This is more like an interrupt routine (and might
// eventually be called from a timer interrupt.
void systemEvent() {
  return;
}

void waitForCharAvailable() {
  while (Serial.available() == 0)
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
    Serial.print(F("Verbose on."));
  }
  reportStatusToSerial(0, "toggleVerbose()");
}


// Read (the rest of) a line of input.
// Caution: No bounds checking Return the length we read.  (We may
// have already crashed if there was an overrun, but it could save a
// count later.)
int readline(char *buffer, int limit)
{
  uint8_t i = 0;
  char c;

  do {
    while (Serial.available() == 0) ; // Block until a character is available
    c = Serial.read();
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

}

// Read a line, find the variable name (before the "="), store
// (overwriting old value, if necessary) in EEPROM.
int storeNV() {
  char lineBuf[255];  // Caution: Arbitrary limit here, depends on
		      // available string space.
  int i, len;

  len = readline(lineBuf, 255);
  vprint("line(");
  vprintDec(len);
  vprintln("):");
  vprintln(lineBuf);

  printRAM();

  for (i=0; i<=len; i++) {  // <= to include null terminator
    EEPROM.write(i, lineBuf[i]);
  }
  return SUCCESS;  // fixme
}


int retrieveNV() {
  char lineBuf[255];  // Caution: Arbitrary limit here, depends on
		      // available string space.
  int i=0;

  do {
    lineBuf[i] = EEPROM.read(i);
  } while (lineBuf[i++] != '\0' && i < 255);

  Serial.print("'");
  Serial.print(lineBuf);
  Serial.println("'");

  return SUCCESS;  // fixme
}


// loop() reads input and executes it.
//
// It ignores spaces, CR, LF... Anything "whitespacy" Whitespace also
// signals the (allowed) premature end of entering an argument to a
// directive.  If the Loop() encounters '[', it ignores other
// characters until a ']' is received, but it keeps processing the
// test loop.

int toggle = 0;

void loop()  {
  int returnValue = 99; // Assume a weird error
  int serialInput;

  int angle = 0;

  serialInput = 0;

  // For verbose, print a prompt
  if (verbose) {
    Serial.print("> ");
    Serial.flush();
  }

  do {
    while (Serial.available() <= 0) ;
    serialInput = Serial.read();
    if (debug && isSpace(serialInput)) vprint(".");
  } while (isSpace(serialInput));

  //testModeBreak = 1;      // reset test loop mode if the user enters a key
  //testModeEnabled = 1;

  // Now we have a separate "echo on" instead of verbose
  maybe_echo(char(serialInput));
  if (ignoringInput && serialInput == Ce)  // End of a comment
    ignoringInput = false;
  if (!ignoringInput) {
    switch (serialInput) {
    case Cs: { // Comment Start
      ignoringInput = true;
      break;
    }
    case Ce: { // Comment End
      // Already (necessarily) handled above switch()
      break;
    }
    case c: { // Check Ready Status
      vprintln("");
      returnValue = StatusReady();
      reportStatusToSerial(returnValue, "CheckReady");
      break;
    }
    case S: {  // reSet
      vprintln("");
      reset();
      break;
    }
    case R: {  // Rotate needs more input, an int
      vprint("? ");  // prompt for it if we're verbose
      angle = Serial.parseInt();
      vprintDec(angle);
      returnValue = cubeRotate(angle);
      reportStatusToSerial(returnValue, "Rotate");
      break;
    }
    case E: { // eLevate needs more input, an int
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
      returnValue = cubeElevate(angle);
      reportStatusToSerial(returnValue, "Elevate");
      break;
    }
    case F: { // Imaging lamps oFf.
      vprintln("");
      returnValue = ImagingOff();
      reportStatusToSerial(returnValue, "Imaging Off");
      break;
    }
    case N: { // Imaging lamps oN.
      vprintln("");
      returnValue = ImagingOn();
      reportStatusToSerial(returnValue, "Imaging On");
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
    case X: {
      setVariable();
      break;
    }
    case x: {
      vprintln("");
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
	Serial.println("'.");
      }
      Serial.println(F("Commands:\n"));
      Serial.println(F("S -- reSet"));
      Serial.println(F("Rnnn -- Rotate to nnn"));
      Serial.println(F("Ennn -- eLevate to nnn (top = 0)"));
      Serial.println(F("V1 -- set Verbose"));
      Serial.println(F("V0 -- set non-Verbose"));
      Serial.println(F("N -- lamps oN"));
      Serial.println(F("F -- lamps oFf"));
      Serial.println(F("X<var>=<value> -- set variable"));
      Serial.println(F("Z<string value> -- NV store"));
      Serial.println(F("z -- NV retrieve\n"));

      Serial.print(F("verbose = "));
      Serial.println(verbose);
      Serial.print(F("debug = "));
      Serial.println(debug);
      Serial.print(F("echo = "));
      Serial.println(echo);
    }
    } // end switch
  } // end not ignoring input
  //TestProcessNext();
  delay(1);
}

// Most functions return an int, which is zero if the function worked
// correctly.  The exceptions are setup(), loop() and the tests.  As
// the rest of the functions return an int, the function status should
// be reported in the case statements calling the function (as opposed
// to the tests who report their status direct to the serial out)
