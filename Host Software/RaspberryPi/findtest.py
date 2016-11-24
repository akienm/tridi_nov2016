#!/usr/bin/env python
import sys, os
from subprocess import call
import shutil
from time import *
import glob
import xmlClasses
import xmlData
import main2
import shootString_v1
import xmlManifest
flag = 0
count = 0
i = 1
#while i == count
 
src_files = '/home/pi/CancunAlpha/updates/'

if len(os.listdir(src_files)) > 0:     
#Backup everything in currentVersion into backups 
    print " Performing update"
    backup_dest = '/home/pi/CancunAlpha/backups/' + strftime("%d %b %Y %H:%M:%S", gmtime())
    os.makedirs(backup_dest)
    current = '/home/pi/CancunAlpha/currentVersion/'
    for file_name in glob.glob(os.path.join(current,'*.*')):
#    for file_name in current:
        full_file_name = os.path.join(current, file_name)
        if (os.path.isfile(full_file_name)):
            print "copying file"
            shutil.copy2(full_file_name, backup_dest)
    print "copied"
#Delete all files in currentVersion
    files = glob.glob('/home/pi/CancunAlpha/currentVersion/*.*')
    for f in files:
        os.remove(f)
#    shutil.rmtree('/home/pi/CancunAlpha/currentVersion/*')
# Delete images that are 1 day old
#for files in

#Load contents from updates to currentVersion
    dest = '/home/pi/CancunAlpha/currentVersion/'
    for file_name in glob.glob(os.path.join(src_files,'*.*')):
        full_file_name = os.path.join(src_files, file_name)
        if (os.path.isfile(full_file_name)):
            shutil.copy2(full_file_name, dest)
    if glob.glob(os.path.join('/home/pi/CancunAlpha/currentVersion/','install.py')):
        print "calling install"
        call(["sudo", "python", "/home/pi/CancunAlpha/currentVersion/install.py"])
while True:
    call(["sudo", "chmod", "777", "/var/www/config.py"])
    while flag == count:
        ping = 0
        for file in os.listdir("/home/pi/CancunAlpha"):
            if file == "config.py":
                #call(["sudo", "chmod", "777", "/home/pi/CancunAlpha/config.py"])
                import main2 as main2
                count = count +1
                ping = main2.mainprogram()
                break
            if ping == 1:
                break
                #ping = call(["./CancunAlpha/main1.py"])
                #print(file)
    flag = flag + ping
    break
#    i = i + ping
