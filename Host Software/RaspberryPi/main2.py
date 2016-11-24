import serverUpload_v1 as serverUpload
import shootString_v1 as shootString
import glob
import os
import xmlManifest as xml
import xmlData as xmlData
from time import *
import config

def mainprogram():

    shootstring = config.shootingString
    day = strftime("%d %b %Y", gmtime())
    newpath = "/home/pi/CancunAlpha/Images/" + day + " Scan " + str(xmlData.scanCount)
    if not os.path.isdir(newpath):
        os.makedirs(newpath)
    ImageLocation = newpath
    xmlData.scanStartTime = strftime("%a, %d %b %Y %H:%M:%S GMT-7", gmtime())
    shootString.executeShootstring(shootstring, ImageLocation)
    #A = xmlData.MetaData("C1", "D1", "E1", "F1", "G1")
    #xml.populate()
    xmlData.uploadStartTime = strftime("%a, %d %b %Y %H:%M:%S GMT-7", gmtime())
    xml.generateManifestTemplate()
##    if os.path.isfile('/home/pi/CancunAlpha/config.py'):
##        os.remove("/home/pi/CancunAlpha/config.py")
##    if os.path.isfile('/home/pi/CancunAlpha/config.pyc'):
##        os.remove("/home/pi/CancunAlpha/config.pyc")

    file = open("scanId.py","w")
    file.write("scanCount = " + str(xmlData.scanCount + 1))
    file.close()

    ## Server upload

## Server upload

    # Server details
##    username='scanneruser'
##    password='SugarCube4u!'
##    server_address = 'prod.3dwares.co'
##    server_path = 'prod.3dwares.co/Scans/b3/new/debug/'
##    new_directory = 'NNtest6-22'
##    
##    comp_filepath= '/home/pi/CancunAlpha/Images/'
##    comp_filepath = newpath + "/"
##    print "begin upload"
##    
##    for files in glob.glob(os.path.join(comp_filepath,'*.jpg')):
##        serverUpload.upload_to_server(files, username, password, server_address, server_path, new_directory)
##    
##    print "upload complete"
##    return 1






##  dynamic directory test
##    
##    # Server details
##    ##username='scanneruser'
##    ##password='SugarCube4u!'
##    ##server_address = 'prod.3dwares.co'
##    ##server_path = 'prod.3dwares.co/Scans/b3/new/debug/'
##    ##new_directory = 'NNtest6-22'
##    fileNumber = len([name for name in os.list(server_path) if os.path.isfile(os.path.join(server_path,name))])
##    #new_directory = xmlData.username + "/Scan " + str(fileNumber +1)
##    for roots,dirs,files in os.walk():
##        if files!=[]:
##            for f in files:
##                ##comp_filepath = newpath 
##                ##
##                ##print "begin upload"
##                ##
##                ##for files in glob.glob(os.path.join(comp_filepath,'*.*')):
##                ##    serverUpload.upload_to_server(files, username, password, server_address, server_path, new_directory)
##                ##    os.remove(files)  
##                ##print "upload complete"
##    return 1
##    
