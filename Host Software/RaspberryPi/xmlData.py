import config as data
from scanId import scanCount

clientVersion = data.clientVersion
serverVersion = data.serverVersion
scanID = data.scanID + str(scanCount)
scanStartTime = ""
uploadStartTime = ""

From = data.From
to = data.to
subject = data.subject
referenceID = data.referenceID
body = data.body
attachments = data.attachments
if data.filename:   
    filename = data.filename
#Zstring data

zSS = data.zSS
zSC = data.zSC
zMZ = data.zMZ
zPH = data.zPH
zCN = data.zCN
zAL = data.zAL
zFX = data.zFX
zLE = data.zLE
zFL = data.zFL
zAR = data.zAR
zSF = data.zSF
    
#Modelling options
imageCropping = data.imageCropping
imageCroppingRectangle = data.imageCroppingRectangle
modelScaling = data.modelScaling
modelCropping = data.modelCropping
boundingBox = data.boundingBox
meshlevel = data.meshlevel
template3DP = data.template3DP

shootingString = data.shootingString
shootingStringcameraID = data.cameraID
imageResolution  = data.imageResolution
jpegQuality = data.jpegQuality
COMport = data.COMport
forceCOMPort = data.forceCOMPort
motionDetection = data.motionDetection
motionSensitivity = data.motionSensitivity
testerMessages = data.testerMessages
SugarCubeMessages = data.SugarCubeMessages

#not needed for now
brightness = data.brightness
contrast = data.contrast
hue = data.hue
saturation = data.saturation
sharpness = data.sharpness
gamma = data.gamma
whiteBalance = data.whiteBalance
backLightComp = data.backLightComp
gain = data.gain
colorEnable = data.colorEnable
powerLineFrequency = data.powerLineFrequency

#not needed for now
zoom = data.zoom
focus = data.focus
exposure = data.exposure
aperture = data.aperture
pan = data.pan
tilt = data.tilt
roll = data.roll
lowLightCompensation = data.lowLightCompensation

medianStacking = 1
