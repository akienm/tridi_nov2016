#!/usr/bin/env python3
# Master server program
import yaml
import socket
import logger_utils
import queue
from camera_utils import take_picture
from shootstring_utils import string_to_token_list
from XML_utils import xml_from_config
import os
from time import sleep, strftime
from subprocess import Popen
from sys import exit
from polling_server import polling_server
from arduino import arduino_communication_thread

config = yaml.safe_load(open("config.yml"))
scan_info = yaml.safe_load(open("scan_information.yml"))
logger = logger_utils.get_logger()

class master_server(polling_server):
    def __init__(self, address, port, **kwargs):
        super().__init__(address, port, **kwargs)
        logger.info('init '+__file__)
        self.pids = []
        self.instruction_buffer = []
        self.initial_len = 1
        self.imageList = {}
        self.rotation = 0
        self.elevation = 0
        self.curr = None
        self.cmd_complete = True
        self.spawn_children()

    def spawn_children(self):
        # Spawns other programs used 
        logger.info("Master program is starting subprograms")
#        Popen(['sudo', 'service', 'motion', 'start'])
        self.pids.append(Popen(["./uploader.py"]).pid)
        self.pids.append(Popen(["./server.py"]).pid)

        self.cmd_q = queue.Queue()
        self.resp_q = queue.Queue()
        self.ard = arduino_communication_thread(self.cmd_q, self.resp_q)
        self.ard.start()

    def on_error(self, error):
        #Kill all the children
        logger.info("Master program is killing subprograms")
        for pid in self.pids:
            Popen(["kill", str(pid)], shell=False)
#        Popen(['sudo', 'service', 'motion', 'stop'])
        self.ard.join()
        exit(0)

    def process_msg(self, msg):
        resp = {}

        if msg['task'] == 'start_scan':
            resp['msg'] = 'scan started successfully'
            if('shootstring' in msg):
                self.instruction_buffer += string_to_token_list(msg['shootstring'])
                resp['string'] = 'used given shootstring'
            else:
                self.instruction_buffer += string_to_token_list(config['default_shootstring'])
                resp['string'] = 'used default_shootstring'
            #print("Shoot string: {}".format(self.instruction_buffer))
            self.initial_len = len(self.instruction_buffer)
            self.curr = strftime(config['time_format'])
            resp['jobname'] = self.curr
            self.imageList = {};
        elif msg['task'] == 'stop_scan':
            self.instruction_buffer = []
            self.curr = None
            self.initial_len = 1
            resp['msg'] = 'scan stopped successfully'
        elif msg['task'] == 'scan_progress':
            resp['progress'] = int((self.initial_len - len(self.instruction_buffer))/self.initial_len*100)
        else:
            logger.warning("Feature {} not implemented yet!".format(msg['task']))
        print("master msg:\n{}".format(msg))
        print("master resp:\n{}".format(resp))
        return resp

    def secondary_process(self):
        # If we have a buffer send off the command and update internal state
        # If we have a job but empty buffer, then the job is done and we should upload it
        if self.instruction_buffer:
            if self.cmd_complete:
                cmd = self.instruction_buffer.pop(0)
                if cmd is 'T':
                    i = len(self.imageList)
#                    take_picture(job = self.curr, num = i)
                    print("PICTURES DISABLED")
#time.strftime(config['time_format'])+str(int(time.time()%60*100))
                    self.imageList['{}_{}.jpg'.format(self.curr, i)] = {'elevation': self.elevation, 'rotation': self.rotation}
                else:
                    self.cmd_q.put(cmd, True, 0.05)
                    self.cmd_complete = False
            try:
                resp = self.resp_q.get_nowait()
                self.cmd_complete = True
                if('Elevate' in resp):
                    self.elevation = float(resp.split('OK:')[1].strip())
                elif('Rotate' in resp):
                    self.rotation = float(resp.split('OK:')[1].strip())
            except queue.Empty:
                pass

        elif(self.curr):
            scan_info['metadata']['scanID'] = self.curr
            scan_info['order_details']['referenceID'] = self.curr
            scan_info['metadata']['scanStartTime'] = self.curr
            scan_info['metadata']['uploadStartTime'] = strftime(config['time_format'])
            scan_info['diagnostics']['shootingString'] = config['default_shootstring']
            with open('scan_information.yml', 'w') as outfile:
                outfile.write(yaml.dump(scan_info, default_flow_style=True))
            xml_file_loc = os.path.join(config['picture_directory'].format(self.curr), 'Manifest.xml')
            xml_from_config(scan_info, xml_file_loc, self.imageList)
            uploader = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            uploader.connect((HOST,PORTS['uploader']))
            logger.info('Telling the uploader to start')
            upload_msg = {'command':'upload', 'job': self.curr}
            logger_utils.sendall_log(uploader, upload_msg)
            uploader.close()
            self.curr = None
            self.initial_len = 1

if __name__ == '__main__':
    HOST = config['host']
    PORTS = config['ports']
    server = master_server(HOST, PORTS['master'])
    server.run()
