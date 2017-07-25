from motion_python_api import take_snapshot
from logger_utils import get_logger
import yaml
import shutil
import os
from time import sleep

config = yaml.safe_load(open("config.yml"))
logger = get_logger()

def take_picture(job = 'test', num=0):
    ## NEED TO DO:
    # Check Vibration
    # Median Stacking
    # Start up motion server if it isn't running
    # Check to make sure photo is acceptable
    location = config['picture_directory'].format(job)
    take_snapshot()
    if not os.path.exists(location):
        os.makedirs(location)
    try:
        shutil.copy('/tmp/motion/lastsnap.jpg', location + '/{}_{}.jpg'.format(job, num))
        logger.info('Took a picture')
    except FileNotFoundError:
        sleep(.2)
        try:
            shutil.copy('/tmp/motion/lastsnap.jpg', location + '/{}_{}.jpg'.format(job, num))
            logger.info('Took a picture')
        except FileNotFoundError:
            logger.error('FAILED TO TAKE picture')

    
