#!/usr/bin/env python

import time

import os
# pip install pyserial
import serial
from serial.tools import list_ports


def serial_ports():
    """
    Returns a generator for all available serial ports    """
    if os.name == 'nt':
        # windows
        for i in range(256):
            try:
                s = serial.Serial(i)
                s.close()
                yield 'COM' + str(i + 1)
            except serial.SerialException:
                pass
    else:
        # unix
        for port in list_ports.comports():
            yield port[0]


if __name__ == '__main__':
    print "Here are the serial ports I found on your computer:"
    spl = list(serial_ports())

    # Print the list nicely
    for i in range(0, len(spl)):
        print("  {}. {}".format(i+1, spl[i]))

    default = str(len(spl))  # I've found that the last serial port is often the most recently attached device
    prompt = "Which port do you want to use [" + default + "]?"

    answer = raw_input(prompt) or default

    myPort = spl[int(answer) - 1]
    print "Using",  myPort

    ser = serial.Serial(myPort, 9600)

    msg = "V1"
    ser.write(msg)

    for i in range(5):  # Just print enough to make sure we've seen the Arduino.
        line = ser.readline().rstrip()
        if len(line) > 0:
            print "  ", line

    ser.close()
