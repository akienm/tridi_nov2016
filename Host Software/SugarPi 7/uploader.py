#!/usr/bin/env python3
# Uploader Server Program
import logger_utils
import pysftp
import os
import requests
from yaml import safe_load
from time import sleep
from multiprocessing import Pool, Value
from polling_server import polling_server

config = safe_load(open("config.yml"))
logger = logger_utils.get_logger()
mycnopts = pysftp.CnOpts()

def upload_process(args):
    # Counter is a multiprocessing Value used to keep track of uploading progress
    global counter

    files = args[0]
    job = args[1]

    print('I am a pool worker')
    # print('my files are:{}'.format(files))
    remote_dir = config['remote_directory'].format(job)
    print("Remote dir: {}".format(remote_dir))

    print('Starting my sftp client')
    with pysftp.Connection(config['remote_ip'],username=config['remote_user'],private_key=config['remote_pem'],cnopts=mycnopts) as sftp:
      print('sftp client started!')
      sftp.chdir(remote_dir)
      logger.info("In {}".format(remote_dir))
      for file in files:
        filename = os.path.basename(file)
        logger.info("Uploading: {}".format(filename))
        sftp.put(file, filename)
        with counter.get_lock():
          counter.value += 1
          if(counter.value == 1):
            print('Done with', end=' ', flush=True)
          print(counter.value, end=' ', flush=True)

    #sftp and related all magically quit at end of with: blocks
    return "Pool worker done"

class uploading_server(polling_server):
    def __init__(self, address, port, **kwargs):
        super().__init__(address, port, **kwargs)
        logger.info('init '+__file__)
        self.state = 'done'
        self.queue = []
        self.curr = None

        self.pool = None
        self.result = None

        self.counter = None
        self.job_size = None

    @property
    def progress(self):
        # Generates list of jobs and current progress for display on UI
        # Probably could be a bit neater
        # Not sure how I feel about the @property flag
        jobs = []
        if self.curr:
            jobs.append({'name':self.curr, 'value': int(self.counter.value / self.job_size * 100)})
        for todo in self.queue:
            jobs.append({'name':todo,'value':0})
        return jobs

    def process_msg(self, msg):
        resp = {}
        if(msg['command'] == 'upload'):
            job = msg['job']
            if(self.state =='done'):
                self.state ='uploading'
                self.curr = job
                logger.info('Starting upload of {}'.format(job))
                self.upload_job(job)
            elif(self.state == 'uploading'):
                self.queue.push(job)
                logger.info('adding {} to job queue'.format(job))
        if(msg['command']) == 'upload_status':
            resp['jobs'] = self.progress
        if(msg['command'] == 'terminate'):
            #Should probably implement this
           print('cry') 

        print("uploader msg:\n{}".format(msg))
        print("uploader resp:\n{}".format(resp))
        return resp

    def secondary_process(self):
        # Check if our multiprocessing pool result is in
      if  (self.result and self.result.ready):
        # If so clear it and start the next one (If it exists)
        self.pool.close()
        self.pool.join()
        print('\nFinished upload of {}'.format(self.curr))
        logger.info('Finished upload of {}'.format(self.curr))
        logger.info(self.result.get())
        url = 'http://{}/scan.php'.format(config['remote_ip'])
        folder = config['remote_directory'].format(self.curr)
        folder = folder[folder.find('Scans/'):] #cut until 'Scans/...'
        resp = requests.get(url,params={'uploadDir':folder})
        print('html request returned: '+resp.text)
        resp.close()
        self.curr = None
        if(self.queue):
            job = self.queue.pop()
            self.curr = job
            logger.info('Starting delayed upload of {}'.format(job))
            self.upload_job(job)
        else:
            self.state = 'done'
            self.result = None  

    def make_job_dir(self, job):
        # Creates the remote directory we are uploading into

        directory = config['remote_directory'].format(job)
        with pysftp.Connection(config['remote_ip'],username=config['remote_user'], private_key=config['remote_pem'],cnopts=mycnopts) as sftp:
            print('Making directory: {}'.format(directory))
            if(not sftp.isdir(directory)):
                sftp.makedirs(directory)
                print('Directory created')
                logger.info("Created directory: {}".format(directory))
            else:
                print('Directory exists')
        #sftp magically quits at end of with: block

    def upload_job(self, job):
        directory = config['picture_directory'].format(job)
        remote = config['remote_directory'].format(job)
        infiles = os.listdir(directory)
        infiles = [os.path.join(directory, file) for file in infiles]
        self.make_job_dir(job);
        N = config['num_uploader_processes']

        self.job_size = len(infiles)
        self.counter = Value('i', 0)

        def init(args):
            ''' store the counter for later use '''
            # Don't ask me why we need to make it global, 
            # I just copied this stuff from StackOverflow until it worked
            global counter
            counter = args

        # Split the list into a list of ~equally sized lists (4 chunks)
        # Reason is so we only need sftp.connect once per process
        infiles_chunk = []
        for i in range(N-1): #chunk 0,1,2
            start = len(infiles)//N*i
            end = len(infiles)//N*(i+1)
            infiles_chunk.append(infiles[start:end])
        infiles_chunk.append(infiles[len(infiles)//N*(N-1):]) #chunk 3

        # Pass in job as an arg by including it in the tuple
        infiles_chunk = [(f, job) for f in infiles_chunk]

        logger.info("Uploader starting")
        self.pool = Pool(N, initializer = init, initargs = (self.counter, ))
        #print(infiles_chunk)
        self.result = self.pool.map_async(upload_process,infiles_chunk)
        print("Pool started")

if __name__ == '__main__':
    HOST = config['host']
    PORTS = config['ports']
    server = uploading_server(HOST, PORTS['uploader'])
    server.run()
