#!/usr/bin/env python

# find the serial port and possibly flash the SugarCube using avrdude.
# Invoke with -q to simply return best guess at the serial port.

from __future__ import print_function

import sys  # for sys.stdout stuff
import time
import os
import glob
from subprocess import Popen
import subprocess

# pip install pyserial
import serial
from serial.tools import list_ports



# This OUGHT to find the serial ports on either Windows or Unix, but I
# don't test it much on Windows.
def serial_ports():
    """
    Returns a generator for all available serial ports    """

    notFoundErrorString = \
        "WindowsError(2, 'The system cannot find the file specified.')"

    if os.name == 'nt':
        # windows
        for i in range(16):
            try:
                s = serial.Serial(i)
                s.close()
                yield 'COM' + str(i + 1)
            except serial.SerialException as err:
                # Only print the errors that aren't simple "not found"
                if not notFoundErrorString in str(err):
                    print('ERROR: %s\n' % str(err))
                pass
    else:
        # unix
        for port in list_ports.comports():
            yield port[0]

# Used for finding avrdude executable on Windows
def is_exe(fpath):
    return os.path.isfile(fpath) and os.access(fpath, os.X_OK)


def heySugarCube(aPort, quiet=False):
    try:
        ser = serial.Serial(aPort, 9600, timeout=0.2)
    except:
        if interactive:
            print("Couldn't open serial port", aPort)
        return False

    # Doesn't really matter what this is since the serial open above
    # causes the box to reset, which prints a banner anyway.
    msg = "b\n"
    ser.write(msg)
    line = ""

    for i in range(15):
        if i % 3 == 0:  # kick it again every (timeout * 3)
            ser.write(msg)
        try:
            line = ser.readline().strip()
        except serial.SerialException as e:
            if interactive:
                print(" killing miniterm", end='')
            cmd = "pkill -f 'miniterm.py.*" + aPort + "'"
            # print("cmd = " + cmd)
            os.system(cmd)

        if line.startswith("SugarCube v"):  # found it
            break
        if len(line) == 0 and not quiet:
            if interactive: print('.', end="")
            sys.stdout.flush()
        #else:
            # print('\n> ', line, end="")

    ser.close()

    if line.startswith("SugarCube v"):
        # print(line)
        if not quiet:
            if interactive: print(" SugarCube detected.")
        return line.split(" ")
    else:
        if not quiet:
            if interactive: print('')
        return False

global interactive
if __name__ == '__main__':
    #
    #  Find a serial port that might have a SugarCube on it
    #

    interactive = True

    for arg in sys.argv:
    #    print("Arg", arg)
        if arg == '-q':  # quiet/non-interactive.  Print best answer without questions.
            interactive = False

    scPort = ""

    spl = list(serial_ports())
    spl.reverse()

    if len(spl) < 1:
        print("Can't find any serial ports.  Do you need to install a driver?")
        if os.name == 'nt':
            print("try http://www.ftdichip.com/Drivers/CDM/CDM%20v2.10.00%20WHQL%20Certified.exe")
        sys.exit(1)


    # Print the list nicely
    if interactive: print("Serial ports:")
    for i in range(0, len(spl)):
        if interactive: print("  {}. {}".format(i+1, spl[i]), end="")
        foundCube = heySugarCube(spl[i]);
        if foundCube:
            scPort = spl[i]
            break

    if not scPort:
        # didn't find one we could talk to, ask the user
        default = str(1)  # Guess the first one, since we reversed the list
        if interactive:
            print("Didn't yet find a SugarCube.")
            print("The first serial port is often the most recently attached device")
            prompt = "Which port do you want to use [" + default + "]?"
            answer = raw_input(prompt) or default
            scPort = spl[int(answer) - 1]
        else:
            scPort = default

        # Try to talk to it one more time.  If it's a brand new
        # Arduino, it won't answer back, so this isn't fatal, just
        # nice to print the version number if it's really there now.
        foundCube = heySugarCube(scPort)

    if not interactive:
        print(scPort)
        sys.exit(0)

    if foundCube:
        foundVersion = foundCube[1].rstrip(',')
    else:
        foundVersion = "<not found>"

    # Is SugarCube up to date?
    print("Device on port", scPort, "reports", foundVersion)

    if not interactive:
        sys.exit(0)

    #
    # Invoke avrdude with the .hex file
    #

    avrPath = "/Applications/Arduino.app/Contents/Resources/Java/hardware/tools/avr"
    if os.name == 'nt':
        if is_exe("avrdude.exe"):  # Use the local one first, I guess
            avrPath = "."
        else:
            avrPath = "C:/Program Files/Arduino/hardware/tools/avr"

    avrDudeBinary = avrPath + "/bin/avrdude"
    avrDudeConf = avrPath + "/etc/avrdude.conf"

    # Fix up backslash wackiness
    avrDudeBinary = os.path.normpath(avrDudeBinary)
    if os.name == 'nt':
        avrDudeBinary = avrDudeBinary + '.exe'
    avrDudeConf = os.path.normpath(avrDudeConf)

    print("avrdude at", avrDudeBinary)


    # Find the most recent .hex file, something like
    # "SoundFitMicroX_660M.hex"
    hexFile = max(glob.iglob('*.hex'), key=os.path.getctime)

    if not os.access(avrDudeBinary, os.X_OK):  # Executable?
        print("Can't run", avrDudeBinary);
        sys.exit(1)

    if not os.access(avrDudeConf, os.R_OK):  # Readable?
        print("Can't read avrdude conf file", avrDudeConf);
        sys.exit(1)

    portArg = "-P" + scPort

    command_as_list = [avrDudeBinary,
                       "-V","-C",
                       avrDudeConf,
                       "-patmega328p",
                       "-carduino",
                       portArg,
                       "-b115200",
                       "-D",
                       "-Uflash:w:" + hexFile + ":i",
                       "-q"]  # Use -q twice for extra quiet

    print("Flashing latest .hex file,", hexFile, end="")
    p = subprocess.Popen(command_as_list,
                         stdout=subprocess.PIPE,
                         stderr=subprocess.STDOUT);

    # Grab stdout line by line as it becomes available.  This will loop until
    # p terminates.
    while p.poll() is None:
        l = p.stdout.readline().strip() # This blocks until it receives a newline.
        if len(l) > 0:
            #print("  >", l)
            print(". ", end="")
    # When the subprocess terminates there might be unconsumed output
    # that still needs to be processed.
    l = p.stdout.read().strip()
    if l:
        print("  >",  p.stdout.read())

    newFound = heySugarCube(scPort, quiet=True)
    print("\nAfter flash, device reports", newFound[1].rstrip(','));
