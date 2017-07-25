#!/usr/bin/env python3
# Echo server program
import socket
import yaml
import logger_utils as utils
import json
from time import sleep
from sys import exit
from flask import Flask, send_from_directory
from flask_restful import reqparse, abort, Api, Resource
import logging

config = yaml.safe_load(open("config.yml"))
logger = utils.get_logger()
HOST = config['host']
PORTS = config['ports']

app = Flask(__name__)
api = Api(app)

parser = reqparse.RequestParser()
for i in config['parser_arguments']:
    parser.add_argument(i)

log = logging.getLogger('werkzeug')
log.setLevel(logging.ERROR)
### THIS IS ALL TRASH 
### Literally just a wrapper around Master. Should be done much better
### Ignore this file I'm going to try to replace it in the next few days.

def talk_to_master(args):
        master = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        master.connect((HOST,PORTS['master']))
        utils.sendall_log(master, args)
        print('made it MADE IT made it')
        resp = utils.recieve_log(master)
        master.close()
        return resp

def talk_to_uploader(args):
        uploader = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        uploader.connect((HOST,PORTS['uploader']))
        utils.sendall_log(uploader, args)
        print('made it MADE IT made it')
        resp = utils.recieve_log(uploader)
        uploader.close()
        return resp

class MainServer(Resource):
    def get(self):
        logger.info("Server recieved get request")
        return {'from_email': 'mgerber@berkeley.edu', 'to_email':'scott@soundfit.me'}

    def post(self):
        args = parser.parse_args()
        logger.info("Server recieved args: {} on post request".format(args))
        resp = talk_to_master(args)
        return resp, 201

class ScanStatus(Resource):
    def get(self):
        return talk_to_master({'task':'scan_progress'})

class UploadStatus(Resource):
    def get(self):
        return talk_to_uploader({'command':'upload_status'})

api.add_resource(MainServer, '/api')
api.add_resource(ScanStatus, '/api/scanstatus')
api.add_resource(UploadStatus, '/api/uploadstatus')

## This is not a very good way to serve static content
@app.route('/<path:path>')
def send_static(path):
    return send_from_directory('static', path)

if __name__ == '__main__':
    logger.info("Server program started")
    app.run(debug=True)
