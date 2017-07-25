#!/usr/bin/env python3
#Manually Jumpstart uploader.py
import socket
import logger_utils
import yaml
import argparse
import os

# If a job ID is given, tells the uploader to upload that job
# Otherwise uploads the most recent job again

def get_last_job():
	d = './Scansets/'
	jobs = [os.path.abspath(d + f) for f in os.listdir(d)]
	jobs.sort(key = os.path.getmtime)
	folder = os.path.basename(jobs[-1])
	print(folder[10:])
	return folder[10:]
	# return folder

if __name__ == '__main__':
	parser = argparse.ArgumentParser()
	parser.add_argument('job', nargs='?', default=get_last_job(),
		help='A job ID to upload. Default is most recent job')
	arg = parser.parse_args()
	print("Attempting to upload job ID: {}".format(arg.job))

	config = yaml.safe_load(open("config.yml"))
	logger = logger_utils.get_logger()
	HOST = config['host']
	PORTS = config['ports']
	uploader = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

	try:
		uploader.connect((HOST,PORTS['uploader']))
		logger.info('Telling the uploader to start')
		upload_msg = {'command':'upload', 'job': arg.job}
		logger_utils.sendall_log(uploader, upload_msg)
	except ConnectionRefusedError:
		print("Unable to connect to uploader")
	finally:
		uploader.close()
		print("Closing connection now")
