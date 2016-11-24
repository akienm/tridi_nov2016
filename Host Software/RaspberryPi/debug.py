import shootString_v1 as shootString
import serial
import time
imageLocation = "/home/pi/CancunAlpha/TestImages"
ser = serial.Serial('/dev/ttyACM0', 9600, timeout=1) # Open serial port in arduino
time.sleep(10) # Must be given. don't know why though! Doesn't execute properly if not given
ser.flushInput()  #Flush the buffers
ser.flushOutput()
flag = 1 
while flag == 1:
    command = raw_input("Command to arduino (Type \"Halt\" to Exit) : ")
    if command == "Halt":
        flag ==0
        break
    if flag != 0:
        shootString.implementShootstring(command, imageLocation, ser)
