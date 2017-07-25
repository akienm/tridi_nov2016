#!/usr/bin/env python3
# Uploader Server Program
import logger_utils
import ftplib
import os
import requests
from yaml import safe_load
from time import sleep
from multiprocessing import Pool, Value
from polling_server import polling_server

config = safe_load(open("config.yml"))
logger = logger_utils.get_logger()

def upload_process(args):

    # Counter is a multiprocessing Value used to keep track of uploading progress
    global counter

    files = args[0]
    job = args[1]
    # print('I am an upload process!!')
    # print('my files are:{}'.format(files))
    ip = config['remote_ip']
    user = config['remote_user']
    passwd = config['remote_passwd']
    # print('Starting my ftp client')
    ftp = ftplib.FTP(ip)
    ftp.login(user,passwd)
    # print('ftp client started!!')

    remote_dir = config['remote_directory'].format(job)
    print("Remote dir: {}".format(remote_dir))
    print("My files: {}".format(files))

    for file in files:
        f = open(file,'rb')
        remote_file = os.path.join(remote_dir, os.path.basename(file))
        logger.info("Uploading to: {}".format(remote_file))
        # print("Uploading to: {}".format(remote_file))
        ftp.storbinary('STOR '+ remote_file, f)
        f.close()
        with counter.get_lock():
            counter.value += 1
            print('Done with {} jobs'.format(counter.value))
    ftp.quit() 
    return "Multiprocessing pool worker done"

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

        print(msg)
        print(resp)
        return resp 

    def secondary_process(self):
        # Check if our multiprocessing pool result is in
        # If so clear it and start the next one (If it exists)
        if(self.result and self.result.ready()):
            self.pool.close()
            self.pool.join()
            logger.info('Finished upload of {}'.format(self.curr))
            logger.info(self.result.get())
            url = 'http://prod.3dwares.co/apps/_recap/trigger_markers_v1.php'
            folder = config['remote_directory'].format(self.curr)
            resp = requests.get(url, params = {'uploadDir':folder})
            print(resp.text)
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
        ip = config['remote_ip']
        user = config['remote_user']
        passwd = config['remote_passwd']
        directory = config['remote_directory'].format(job)

        ftp = ftplib.FTP(ip)
        ftp.login(user,passwd)
        ftp.pwd()
        try:
            ftp.mkd(directory)
            print('made a directory')
            print(directory)
        except Exception as e:
            # Should fix for DirectoryAlreadyExists error or something along those lines
            print(e)
        finally: 
            ftp.quit()

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

        ## Split the list into a list of ~equally sized lists
        ## Reason why is so we only need to call ftplib.FTP once per process
        infiles_chunk = []
        for i in range(N):
            start = len(infiles)//N*i
            end = len(infiles)//N*(i+1)
            infiles_chunk.append(infiles[start:end])
        infiles_chunk[-1] += infiles[len(infiles)//N*(i+1):]

        ## Pass in job as an arg by including it in the tuple
        infiles_chunk = [(f, job) for f in infiles_chunk]

        logger.info("Uploader starting")
        self.pool = Pool(N, initializer = init, initargs = (self.counter, ))
        # print(infiles_chunk)
        self.result = self.pool.map_async(upload_process,infiles_chunk)
        print("Pool started")

if __name__ == '__main__':
    HOST = config['host']
    PORTS = config['ports']
    server = uploading_server(HOST, PORTS['uploader'])
    server.run()
