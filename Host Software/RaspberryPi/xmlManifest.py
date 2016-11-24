#*********************************************************************************#
#                   Generating XML Manifest                                       #
#*********************************************************************************#

from xml.dom import minidom
import csv
from xml.etree.ElementTree import Element, SubElement, Comment, tostring
import datetime
import xml.etree.cElementTree as ET
#from ElementTree_pretty import prettify
import os
import xmlClasses as xmlClass
import xmlData as xmlData
from scanId import scanCount
#import main_v1 

dataFile = '/home/pi/CancunAlpha/Images/Manifest.xml'
imageList = []

def generateManifestTemplate():
    objectMetaData = xmlClass.MetaData(xmlData.clientVersion, xmlData.serverVersion, xmlData.scanID, xmlData.scanStartTime, xmlData.uploadStartTime)
    objectOrderDetails = xmlClass.OrderDetails(xmlData.From,xmlData.to,xmlData.subject,xmlData.referenceID,xmlData.body,xmlData.attachments, xmlData.filename)
    objectZValues = xmlClass.ZValues(xmlData.zSC,xmlData.zSS,xmlData.zMZ,xmlData.zPH,xmlData.zCN,xmlData.zAL, xmlData.zFX, xmlData.zLE, xmlData.zFL, xmlData.zAR, xmlData.zSF)    
    objectModellingOptions = xmlClass.ModellingOptions(xmlData.imageCropping,xmlData.imageCroppingRectangle,xmlData.modelScaling,xmlData.modelCropping,xmlData.boundingBox,xmlData.meshlevel,xmlData.template3DP)
    objectDiagnostics = xmlClass.Diagnostics(xmlData.shootingString,xmlData.shootingStringcameraID,xmlData.imageResolution,xmlData.jpegQuality,xmlData.COMport,xmlData.forceCOMPort,xmlData.motionDetection,xmlData.motionSensitivity,xmlData.testerMessages,xmlData.SugarCubeMessages)
    objectVideoProcAmp = xmlClass.VideoProcAmp(xmlData.brightness,xmlData.contrast,xmlData.hue,xmlData.saturation,xmlData.sharpness,xmlData.gamma,xmlData.whiteBalance,xmlData.backLightComp,xmlData.gain,xmlData.colorEnable,xmlData.powerLineFrequency)
    objectCameraControl = xmlClass.CameraControl(xmlData.zoom,xmlData.focus,xmlData.exposure,xmlData.aperture,xmlData.pan,xmlData.tilt,xmlData.roll,xmlData.lowLightCompensation)
    
    sugarCubeScan = Element('SugarCubeScan')
    sugarCubeScan.set('manifest_version','r1.3')
#metadata    
    metaData = SubElement(sugarCubeScan,'MetaData')
    clientVersion = SubElement(metaData,'clientVersion')
    print objectMetaData.clientVersion
    clientVersion.text=objectMetaData.clientVersion
    serverVersion = SubElement(metaData,'serverVersion')
    serverVersion.text=objectMetaData.serverVersion
    scanID = SubElement(metaData,'scanID')
    scanID.text=objectMetaData.scanID
    scanStartTime = SubElement(metaData,'scanStartTime')
    scanStartTime.text=objectMetaData.scanStartTime
    uploadStartTime = SubElement(metaData,'uploadStartTime')
    uploadStartTime.text=objectMetaData.uploadStartTime
#orderDetails
    orderDetails = SubElement(sugarCubeScan,'orderDetails')
    From = SubElement(orderDetails,'from')
    From.text=objectOrderDetails.From
    to = SubElement(orderDetails,'to')
    to.text = objectOrderDetails.to
    subject = SubElement(orderDetails,'subject')
    subject.text=objectOrderDetails.subject
    referenceID = SubElement(orderDetails,'referenceID')
    referenceID.text = objectOrderDetails.referenceID
    body = SubElement(orderDetails,'body')
    body.text = objectOrderDetails.body
    attachments = SubElement(orderDetails,'attachments')
    #attachments.text = objectOrderDetails.attachments
    if xmlData.filename:
        if xmlData.filename != "":
            for files in xmlData.filename:
                filename = SubElement(attachments,'filename') ############### to include two filename tags
                filename.text =files
#z values
    ZValues = SubElement(sugarCubeScan,'ZValues')
    SC = SubElement(ZValues,'SC')
    SC.text=objectZValues.SC
    SS = SubElement(ZValues,'SS')
    SS.text=objectZValues.SS
    MZ = SubElement(ZValues,'MZ')
    MZ.text=objectZValues.MZ
    PH = SubElement(ZValues,'PH')
    PH.text=objectZValues.PH
    CN = SubElement(ZValues,'CN')
    CN.text=objectZValues.CN
    AL = SubElement(ZValues,'AL')
    AL.text=objectZValues.AL
    FX = SubElement(ZValues,'FX')
    FX.text=objectZValues.FX
    LE = SubElement(ZValues,'LE')
    LE.text=objectZValues.LE
    FL = SubElement(ZValues,'FL')
    FL.text=objectZValues.FL
    AR = SubElement(ZValues,'AR')
    AR.text=objectZValues.AR
    SF = SubElement(ZValues,'SF')
    SF.text=objectZValues.SF
    
#modellingOptions      
    modellingOptions = SubElement(sugarCubeScan,'modellingOptions')
    imageCropping = SubElement(modellingOptions,'imageCropping')
    imageCropping.text = objectModellingOptions.imageCropping
    imageCroppingRectangle = SubElement(modellingOptions,'imageCroppingRectangle')
    imageCroppingRectangle.text = objectModellingOptions.imageCroppingRectangle
    modelScaling = SubElement(modellingOptions,'modelScaling')
    modelScaling.text = objectModellingOptions.modelScaling
    modelCropping = SubElement(modellingOptions,'modelCropping')
    modelCropping.text = objectModellingOptions.modelCropping
    boundingBox = SubElement(modellingOptions,'boundingBox')
    boundingBox.text = objectModellingOptions.boundingBox
    meshlevel = SubElement(modellingOptions,'meshlevel')
    meshlevel.text = objectModellingOptions.meshlevel
    template3DP = SubElement(modellingOptions,'template3DP')
    template3DP.text = objectModellingOptions.template3DP
# Image Set
    imageSet = SubElement(sugarCubeScan,'ImageSet')
    imageSet.set("id", "TesterScan")       
    for images in imageList:
        image = SubElement(imageSet,'image')
        image.set("elevation", str(images.elevation))
        image.set("rotation", str(images.rotation))
        image.text = images.name
#diagnostics    
    diagnostics = SubElement(sugarCubeScan,'Diagnostics')
    shootingString = SubElement(diagnostics,'shootingString')
    shootingString.text = objectDiagnostics.shootingString
    cameraID = SubElement(diagnostics,'cameraID')
    cameraID.text = objectDiagnostics.cameraID
    imageResolution = SubElement(diagnostics,'imageResolution')
    imageResolution.text = objectDiagnostics.imageResolution
    jpegQuality = SubElement(diagnostics,'jpegQuality')
    jpegQuality.text = objectDiagnostics.jpegQuality
    COMport = SubElement(diagnostics,'COMport')
    COMport.text = objectDiagnostics.COMport
    forceCOMPort = SubElement(diagnostics,'forceCOMPort')
    forceCOMPort.text = objectDiagnostics.forceCOMPort
    motionDetection = SubElement(diagnostics,'motionDetection')
    motionDetection.text = objectDiagnostics.motionDetection
    motionSensitivity = SubElement(diagnostics,'motionSensitivity')
    motionSensitivity.text = objectDiagnostics.motionSensitivity
    testerMessages = SubElement(diagnostics,'testerMessages')
    testerMessages.text = objectDiagnostics.testerMessages
    SugarCubeMessages = SubElement(diagnostics,'SugarCubeMessages')
    SugarCubeMessages.text = objectDiagnostics.SugarCubeMessages
#VideoProcAmp    
    videoProcAmp = SubElement(diagnostics,'videoProcAmp')
    brightness = SubElement(videoProcAmp,'brightness')
    brightness.text = objectVideoProcAmp.brightness
    contrast = SubElement(videoProcAmp,'contrast')
    contrast.text = objectVideoProcAmp.contrast
    hue = SubElement(videoProcAmp,'hue')
    hue.text = objectVideoProcAmp.hue
    saturation = SubElement(videoProcAmp,'saturation')
    saturation.text = objectVideoProcAmp.saturation
    sharpness = SubElement(videoProcAmp,'sharpness')
    sharpness.text = objectVideoProcAmp.sharpness
    gamma = SubElement(videoProcAmp,'gamma')
    gamma.text = objectVideoProcAmp.gamma
    whiteBalance = SubElement(videoProcAmp,'whiteBalance')
    whiteBalance.text = objectVideoProcAmp.whiteBalance
    backLightComp = SubElement(videoProcAmp,'backLightComp')
    backLightComp.text = objectVideoProcAmp.backLightComp
    gain = SubElement(videoProcAmp,'gain')
    gain.text = objectVideoProcAmp.gain
    colorEnable = SubElement(videoProcAmp,'colorEnable')
    colorEnable.text = objectVideoProcAmp.colorEnable
    powerLineFrequency = SubElement(videoProcAmp,'powerLineFrequency')
    powerLineFrequency.text = objectVideoProcAmp.powerLineFrequency
#cameraControl
    cameraControl = SubElement(diagnostics,'cameraControl')
    zoom = SubElement(cameraControl,'zoom')
    zoom.text = objectCameraControl.zoom
    focus = SubElement(cameraControl,'focus')
    focus.text = objectCameraControl.focus
    exposure = SubElement(cameraControl,'exposure')
    exposure.text = objectCameraControl.exposure
    aperture = SubElement(cameraControl,'aperture')
    aperture.text = objectCameraControl.aperture
    pan = SubElement(cameraControl,'pan')
    pan.text = objectCameraControl.pan
    tilt = SubElement(cameraControl,'tilt')
    tilt.text = objectCameraControl.tilt
    roll = SubElement(cameraControl,'roll')
    roll.text = objectCameraControl.roll
    lowLightCompensation = SubElement(cameraControl,'lowLightCompensation')
    lowLightCompensation.text = objectCameraControl.lowLightCompensation
    
    tree = ET.ElementTree(sugarCubeScan)
    tree.write(dataFile)
    
    
   # Zvalues = SubElement(sugarCubeScan,'Zvalues')
	
#generateManifestTemplate(dataFile)	


##i=1
###A = ImageSpecs
##instancelist = []#[ImageSpecs(1,2,"asd")]
##while i<5:
##    instancelist.append(ImageSpecs(i,i+10,"asd"))
##    #A[i] = ImageSpecs(i,i+10,"test")
##    i=i+1
##i=0
##for i in instancelist:
##    print i.rotation
