class MetaData:
    def __init__(self, clientVersion, serverVersion, scanID, scanStartTime, uploadStartTime):
        self.clientVersion = clientVersion
        self.serverVersion = serverVersion
        self.scanID = scanID
        self.scanStartTime = scanStartTime
        self.uploadStartTime = uploadStartTime

class ImageSpecs:
    def __init__(self, rotation, elevation, name):
        self.elevation = elevation
       # print elevation
        self.rotation = rotation
       # print rotation
        self.name = name
        print name

class OrderDetails:
    def __init__(self, From, to, subject, referenceID, body, attachments, filename):
        self.From = From
        self.to = to
        self.subject = "<![CDATA[" + subject + "]]>"
        self.referenceID = "<![CDATA[" + referenceID + "]]>"
        self.body = "<![CDATA[" + body + "]]>"
        self.attachments = attachments
        self.filename = filename 

class ZValues:
    def __init__(self, SS, SC, MZ, PH, CN, AL, FX, LE, FL, AR, SF):
        self.SS = SS
        self.SC = SC
        self.MZ = MZ
        self.PH = PH
        self.CN = CN
        self.AL = AL
        self.FX = FX
        self.LE = LE
        self.FL = FL
        self.AR = AR
        self.SF = SF

class ModellingOptions:
    def __init__(self, imageCropping, imageCroppingRectangle, modelScaling, modelCropping, boundingBox, meshlevel, template3DP):
        self.imageCropping = imageCropping
        self.imageCroppingRectangle = imageCroppingRectangle
        self.modelScaling = modelScaling
        self.modelCropping = modelCropping
        self.boundingBox = boundingBox
        self.meshlevel = meshlevel
        self.template3DP = template3DP
class Diagnostics:
    def __init__(self, shootingString, cameraID, imageResolution, jpegQuality, COMport, forceCOMPort, motionDetection, motionSensitivity, testerMessages, SugarCubeMessages):
        self.shootingString = "<![CDATA[" + shootingString + "]]>"
        self.cameraID = cameraID
        self.imageResolution = imageResolution
        self.jpegQuality = jpegQuality
        self.COMport = COMport
        self.forceCOMPort = forceCOMPort
        self.motionDetection = motionDetection
        self.motionSensitivity = motionSensitivity
        self.testerMessages = testerMessages
        self.SugarCubeMessages = SugarCubeMessages
class VideoProcAmp:
    def __init__(self, brightness, contrast, hue, saturation, sharpness, gamma, whiteBalance, backLightComp, gain, colorEnable, powerLineFrequency):
        self.brightness = brightness
        self.contrast = contrast 
        self.hue = hue
        self.saturation = saturation
        self.sharpness = sharpness
        self.gamma = gamma
        self.whiteBalance = whiteBalance
        self.backLightComp = backLightComp
        self.gain = gain
        self.colorEnable = colorEnable
        self.powerLineFrequency = powerLineFrequency
class CameraControl:
    def __init__(self, zoom, focus, exposure, aperture, pan, tilt, roll, lowLightCompensation):
        self.zoom = zoom
        self.focus = focus
        self.exposure = exposure
        self.aperture = aperture
        self.pan = pan
        self.tilt = tilt
        self.roll = roll
        self.lowLightCompensation = lowLightCompensation
        
        
