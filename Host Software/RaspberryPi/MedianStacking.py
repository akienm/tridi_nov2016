from PIL import Image
import numpy
from subprocess import call
import StringIO
import os
import time
from datetime import datetime

def median(lst):
    return numpy.median(numpy.array(lst))
def medianStacking(imageLocation):
    call(["/usr/bin/fswebcam", "-r", "2592x1944", "--no-banner", "/home/pi/CancunAlpha/TestImages/median1.jpg"]) # Image Capture
    call(["/usr/bin/fswebcam", "-r", "2592x1944", "--no-banner", "/home/pi/CancunAlpha/TestImages/median2.jpg"]) # Image Capture
    call(["/usr/bin/fswebcam", "-r", "2592x1944", "--no-banner", "/home/pi/CancunAlpha/TestImages/median3.jpg"]) # Image Capture
    image1 = Image.open("/home/pi/CancunAlpha/TestImages/median1.jpg")
    buffer1 = image1.load()
    image2 = Image.open("/home/pi/CancunAlpha/TestImages/median2.jpg")
    buffer2 = image2.load()
    image3 = Image.open("/home/pi/CancunAlpha/TestImages/median3.jpg")
    buffer3 = image3.load()
    output = Image.new("RGB", (2592,1944),"white")
    buffer4 = output.load()
    for x in xrange(0, 100):
        for y in xrange(0, 75):
            list1 = [buffer1[x,y][0], buffer2[x,y][0], buffer3[x,y][0]]
            #print list1
            #buffer4[x,y][0] = [median(List)]
            #buffer4[x,y][0]=median(buffer1[x,y][0], buffer2[x,y][0], buffer3[x,y][0])
            list2 = [buffer1[x,y][1], buffer2[x,y][1], buffer3[x,y][1]]
            #print list2
            #buffer4[x,y][1] = [median(List)]
            #buffer4[x,y][1]=median(buffer1[x,y][1], buffer2[x,y][1], buffer3[x,y][1])
            list3 = [buffer1[x,y][2], buffer2[x,y][2], buffer3[x,y][2]]
            #print list3
            #buffer4[x,y][2] = [median(List)]
            #buffer4[x,y][2]=median(buffer1[x,y][2], buffer2[x,y][2], buffer3[x,y][2])


            buffer4[x,y] = tuple([int(median(list1)),int(median(list2)),int(median(list3))])
##            buffer4[x,y][0] = int(median(list1))
##            buffer4[x,y][1] = int(median(list2))
##            buffer4[x,y][2] = int(median(list3))
##            
    output.save(imageLocation)
    
