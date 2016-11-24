import re
import time
from subprocess import call
import ftplib
import os
import glob
import serial
import xmlManifest as xmlManifest
import xmlData as xmlData
import xmlClasses as xmlClass
import MedianStacking
import cameraTest
rotation = 0
elevation = 0
zString = ""
def checkping(ser):        # Wait for ping back from arduino to proceed to next command
    global rotation
    global elevation
    readline = ser.readline()
    if 'OK' not in readline:
        print "checking for ping"
        checkping(ser)
    else:
        print "increment"
        if "Rotate" in readline:
            #print "testing" + readline
            rotation = readline.split(":")[2]
            rotation = rotation.replace(' ','')
            print "R position = " + rotation
        elif "Elevate" in readline:
            elevation = readline.split(":")[-1]
            print "E position = " + elevation
        elif "GetData" in readline:
            zString = readline
            readline = readline.split("OK: ")[1]
            readline = readline.split(";")
            print "Z String  : "
            for command in readline:
                before,sep,after = command.rpartition("=")
                if before == "SS":
                    xmlData.zSS = after
                elif before == "SC":
                    xmlData.zSC = after
                elif before == "MZ":
                    xmlData.zMZ = after
                elif before == "PH":
                    xmlData.zPH = after
                elif before == "CN":
                    xmlData.zCN = after
                elif before == "AL":
                    xmlData.zAL = after
                elif before == "FX":
                    xmlData.zFX = after
                elif before == "LE":
                    xmlData.zLE = after
                elif before == "FL":
                    xmlData.zFL = after
                elif before == "AR":
                    xmlData.zAR = after
                elif before == "SF":
                    xmlData.zSF = after
                    
                #eval("before") = after 
                print command

#**********************************************************************
## Convert shootstring from human readable lines to only commands seperated by spaces
def convertString(shootstring):    
    shootstring = shootstring.replace('*NEWLINE_TOKEN*','\n')
    shootstring = shootstring.replace('\n',' ') # Remove newline and replace with space
    print (shootstring)
    
    sstring = re.sub('\[[^\]]+\]','',shootstring)  # Deleting [ and ] from the string
    sstring = re.sub('\]','',sstring)
    sstring = "".join(sstring.split("\n"))
    print sstring
    i=0
    j=1
    for i in sstring:   # Removing extra spaces (if any) from the string
        if j==1:
            if i==" ":
                j=j-1
            string=i
        if j>1:
            if i!=" " or (t!=i and i==" "):
                string=string+i
        j=j+1
        t=i
    print (string)
    return string
#imageList = []
imageName = xmlData.referenceID + str(xmlData.scanCount) + "image " #referenceID and scanCount from xmlData

def imageCapture(elevation, rotation, imageCount, imageLocation):
    name = imageName + str(imageCount)
    cameraTest.vibrationTest()
    print imageLocation + "image " +str(imageCount) + ".jpg"
    if xmlData.medianStacking ==1:
        returnstring = MedianStacking.medianStacking(imageLocation + "/image " +str(imageCount) + ".jpg")
    else:
        returnstring = call(["/usr/bin/fswebcam", "-r", "2592x1944", "--no-banner", imageLocation + "/image " +str(imageCount) + ".jpg"]) # Image Capture
    #print returnstring
    xmlManifest.imageList.append(xmlClass.ImageSpecs(rotation, elevation, name))
def executeCommand(command, imageLocation, imageCount, count, ser):
    increment = 0
    if command == "T":
        print "capture image"
        imageCapture(elevation, rotation, imageCount, imageLocation)
        increment = 1
    elif command == "S":
        ser.write("E0 ")
        checkping(ser)
        ser.write("R0 ")
        checkping(ser)
        ser.write("I0 ")
        checkping(ser)
        ser.write("LB1 ")
        checkping(ser)
    elif command == "H":
        ser.write("R0 ")
        checkping(ser)
        ser.write("E0 ")
        checkping(ser)
    elif '*' in command:
        command = command.split('*')[0] + str(count) + " "
        print command
        ser.write(command)
        checkping(ser)
    else:
        print (command)
        ser.write(command)
        checkping(ser)
    return increment
#**********************************************************************
##Implementing shootstring

def implementShootstring(string, imageLocation, ser):
  #  global rotation
  #  global elevation
    string=string.split()
    it=iter(string)
    j=0
    k=0
    imageCount = 1
    for i in it:
            # If a loop is encountered. Example "{123213,123213,21321 @# &*&8 ^%6 }"
            if i[0]=='{':              # The syntax is always the same for any loop:
                t=i                    # { is immediately followed by 3 number seperated by a "," Example {0,10,2
                t= re.sub('{','',i)    # The first number is the starting value, middle is the end value and 3rd is the increment value
                t=t.split(',')         # The word is split by ifnoring the first character ('{') and by seperating the 3 numbers by ","
                index=j+1              # t contains these 3 numbers
                temp = ""              # The words in the string until } are all the commands to be execute in the loop
                k=0
                while (string[index]!='}'):
                    temp= temp + string[index] + " "    # temp will hold the string to be printed for every iteration in the loop
                    index=index+1
                    k=k+1
                    j=j+1
                j=j+1
                count=int(t[0])
                k=k+1
                while (count<int(t[1])):    # Iterating through the loop string
                    shoot_string=temp
                    shoot_string=shoot_string.split()
                    for command in shoot_string:
##                        if command == "T":
##                            print ("capture image")
##                            imageCapture(elevation, rotation, imageCount, imageLocation)
##                            imageCount = imageCount + 1
##                        elif "*" in command:
##                            tempCommand = ""
##                            flag=1
##                            for letter in command:
##                                if flag==0:
##                                    continue
##                                elif flag==1:
##                                    if letter == "*":
##                                        flag=0
##                                        continue
##                                    tempCommand = tempCommand + letter
##                            print (tempCommand + str(count) + " ")
##                            ser.write(tempCommand + str(count) + " ")
##                            checkping(ser)
                        increment = executeCommand(command, imageLocation, imageCount, count, ser)  
                        imageCount = imageCount + increment
##                        else:
##                            
####                            print (command)
####                            ser.write(temp)
####                            checkping(ser)
##                            increment = executeCommand(commmand, imageLocation, imageCount, count, ser)
                    count = count + int(t[2])
            elif k>0: # skip iteratins k number of times. This is done to skip the the commands that are in the loop as they have already been sent to the arduino
                k=k-1
                continue
            else :
##                if i == "T": #Hard coded since it is a host software function
##                    print ("capture image")
##                    imageCapture(elevation, rotation, imageCount, imageLocation)
##                    imageCount = imageCount + 1
##                else:
##                    print (i)
##                    ser.write(i)
##                    checkping(ser)
                increment = executeCommand(i, imageLocation, imageCount, 0, ser)
                imageCount = imageCount + increment
            j=j+1
    print ("\n\nComplete ")


def executeShootstring(shootstring, imageLocation):
    ser = serial.Serial('/dev/ttyACM0', 9600, timeout=1) # Open serial port in arduino
    time.sleep(10) # Must be given. don't know why though! Doesn't execute properly if not given
    i=0
    j=1
    ser.flushInput()  #Flush the buffers
    ser.flushOutput()
    ser.write('I1 ')
    checkping(ser)
    time.sleep(2)
    ser.write('I0 ')
    checkping(ser)
    ser.write('z')
    checkping(ser)

#    arduinoConnect()
  #  shootstring = " I1 R75 E143 T I0"
    string = convertString(shootstring)
    implementShootstring(string, imageLocation, ser)

######## troubleshoot
#executeShootstring("Zs","/home/pi/CancunAlpha/TestImages/test7_20")

    
