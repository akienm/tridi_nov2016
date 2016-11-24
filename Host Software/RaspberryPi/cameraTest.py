import StringIO
import subprocess
import os
import time
from datetime import datetime
from PIL import Image
from subprocess import call

threshold = 10
sensitivity = 20
def vibrationTest():
    while True:
        call(["/usr/bin/fswebcam", "-r", "100x75", "--no-banner", "/home/pi/CancunAlpha/TestImages/vibrationtest1.jpg"]) # Image Capture
        call(["/usr/bin/fswebcam", "-r", "100x75", "--no-banner", "/home/pi/CancunAlpha/TestImages/vibrationtest2.jpg"]) # Image Capture
        image1 = Image.open("/home/pi/CancunAlpha/TestImages/vibrationtest1.jpg")
        buffer1 = image1.load()
        image2 = Image.open("/home/pi/CancunAlpha/TestImages/vibrationtest2.jpg")
        buffer2 = image2.load()
        # Count changed pixels
        changedPixels = 0
        for x in xrange(0, 100):
            for y in xrange(0, 75):
                pixdiff = abs(buffer1[x,y][1] - buffer2[x,y][1])
                if pixdiff > threshold:
                    changedPixels += 1
        # break if no change
        if changedPixels < sensitivity:
            break
        else:
            print "change"
    if os.path.isfile("/home/pi/CancunAlpha/TestImages/vibrationtest1.jpg"):
        os.remove("/home/pi/CancunAlpha/TestImages/vibrationtest1.jpg")
    if os.path.isfile("/home/pi/CancunAlpha/TestImages/vibrationtest2.jpg"):
        os.remove("/home/pi/CancunAlpha/TestImages/vibrationtest2.jpg")
    
