SoundFitMicro firmware for the Arduino.
---------------------------------------

20 April 2014, Dennis Gentry


Building and uploading
----------------------

I build this using Make.

  make clean
  make
  make upload

You can edit PORT in the Makefile (or call Make like this:)

  make upload PORT=/dev/whatever

I was previously (2014-04-11) using the Arduino IDE (1.0.5) to build
and upload, relying on the Arduino IDE to generate forward references
in the temporary file it creates to compile.  Have now worked around
that by creating our own forward references in the code, but they may
become out of date if people revert to using the IDE (which should
still work fine).  You still need to have the Arduino IDE installed,
since it contains the compilers that make uses.



Setting up to build:
--------------------


Downloads:

  https://github.com/adafruit/Adafruit-Motor-Shield-library/archive/master.zip
  http://www.airspayce.com/mikem/arduino/AccelStepper/AccelStepper-1.39.zip

(Preserved copies in the svn repo at:)

  SugarCubeBeta/3rd Party/Adafruit-Motor-Shield-library-master.zip
                    . . ./AccelStepper-1.39.zip


You'll need to unpack and install both of those Arduino library zips.
There is some newfangled way to import libraries using the IDE -- see
http://arduino.cc/en/Guide/Libraries for more details, but on a Mac,
they go into ~/Documents/Arduino/libraries/.  I did this:

  cd ~Documents/Arduino/libraries
  brew install wget
  wget https://github.com/adafruit/Adafruit-Motor-Shield-library/archive/master.zip
  unzip master.zip
  mv Adafruit-Motor-Shield-library-master AFMotor
  wget http://www.airspayce.com/mikem/arduino/AccelStepper/AccelStepper-1.39.zip
  unzip AccelStepper-1.39.zip

The only tricky thing about that is that the name for the Adafruit
library is long and unacceptable to the IDE, thus the rename.

Oh, and restart the Arduino IDE after you've done the above if you
want them to show up, although the compile/upload should work even
without the restart.


Other Prerequisites I have noticed
----------------------------------

I needed to install pyserial so the python programs that use the
serial port to talk to the Arduino would work.

  sudo chown gentry /Library/Python/2.7/site-packages
  sudo easy_install pip
  pip install pyserial


General Notes on the Code
-------------------------
String space is tight:  http://jeelabs.org/2011/05/22/atmega-memory-use/
