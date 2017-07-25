#!/usr/bin/env python3
#Manually Jumpstart uploader.py
import socket
import logger_utils
import yaml
import os
from sys import argv

# If a job ID is given, tells the uploader to upload that job
# Otherwise uploads the most recent job again

config = yaml.safe_load(open("config.yml"))
logger = logger_utils.get_logger()

def get_last_job():
	d = './Scansets/'
	jobs = [os.path.abspath(d + f) for f in os.listdir(d)]
	jobs.sort(key = os.path.getmtime) #sort by time
	folder = os.path.basename(jobs[-1]) #newest
	print(folder[10:]) #cut off the 'slm-tri-di'
	return folder[10:]
	# return folder

if __name__ != '__main__':
    print(__file__+' called but not __main__: wtf')
    logger.debug(__file__+' called but not __main__: wtf')
    exit()
else:
  logger.info('init '+__file__)
  HOST = config['host']
  PORTS = config['ports']
  commands_for={'master':('start_scan','stop_scan','scan_progress'),
             'uploader':('upload','upload_status','terminate')}
  #recipient = argv[1]
  msg={}
  if (not argv[1] in PORTS):
    print(list(PORTS.keys()))
    exit()
  for i in range(2,len(argv),2):
    if (argv[i+1] in commands_for[argv[1]]):
      msg[argv[i]]=argv[i+1]
    else:
      print(commands_for[arg.recipient])
      exit()

  server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)  
  try:
      print("Connecting socket")
      server.connect((HOST,PORTS[argv[1]]))

      if ('command' in msg and msg['command'] == 'upload'
        and not 'job' in msg):
        msg['job'] = get_last_job()

      print("Sending msg to {}:\n{}".format(argv[1],msg))
      logger.info("Sending msg to {}:\n{}".format(argv[1],msg))
      logger_utils.sendall_log(server,msg)
      print('Response:\n{}'.format(logger_utils.recieve_log(server)))
  except ConnectionRefusedError:
    print("Unable to connect to "+argv[1])
  finally:
    print("Closing socket")
    server.close()
