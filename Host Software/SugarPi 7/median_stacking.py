from PIL import Image
import numpy as np
import utils
from subprocess import call

config = yaml.safe_load(open("config.yml"))["images"]

def medianStacking(imageLocation):
    N = config["median_stacking_count"]
    if N < 3: N = 3 #No point in doing median stacking with less than 3 images
    h = config['height']
    w = config['width']
    for(i in range(N)):
        call(["/usr/bin/fswebcam", "-r", "{}x{}".format(h,w), "--no-banner", "/home/pi/CancunAlpha/TestImages/median{}.jpg".format(i)]) # Image Capture
    images = []
    for(i in range(N)):
        im = np.asarray(Image.open("/home/pi/CancunAlpha/TestImages/median{}.jpg".format(i)))
        images.append(im)
    processed = np.median(images, axis=0)
    final = Image.fromarray(np.uint8(processed))
    final.save(imageLocation)
