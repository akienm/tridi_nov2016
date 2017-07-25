import socket
import logging
import yaml
import json
import serial
from time import sleep

config = yaml.safe_load(open("config.yml"))

def sendall_log(s, data, log=None, println = False): 
    # Send json-ified data on socket s, log that we do it
    if(log is None):
        log = get_logger() 

    data = json.dumps(data)
    if(println): print(data)
    log.debug('Sending message {} to socket'.format(data))
    if(type(data) == str):
        data = str.encode(data)
    s.sendall(data) #convert to bytes and send

def recieve_log(s, log=None):
    # Recieve encoded json data on socket s and log it
    if(log is None):
        log = get_logger()
    data = s.recv(config['socket_buf_size'])
    data = data.decode('ascii')
    log.debug('Recieving message {} on socket'.format(data))
    if(data):
        return json.loads(data)
    else:
        return None

def get_logger():
    # All our applications share a single logger
    # That might need to change- log files are getting too verbose 
    # Probably should stop writing socket info to logs
    logger = logging.getLogger('SugarPi_Logger')
    #If we haven't configured it yet, do that!
    if not logger.hasHandlers():
        hdlr = logging.FileHandler(config['path_to_logfile'])
        formatter = logging.Formatter('%(asctime)s %(levelname)s %(message)s')
        hdlr.setFormatter(formatter)
        logger.addHandler(hdlr)
        logger.setLevel(config['log_level'])
    return logger

def send_command_log(ser, cmd):
    # Send message on serial port ser and log it
    logger = get_logger()
    logger.debug('Sending command {} on serial port'.format(cmd))
    if type(cmd) is str:
        cmd = cmd.encode()
    ser.write(cmd)

def get_response_log(ser):
    # Recieve message on serial port ser and log it
    logger = get_logger()
    resp = ser.readline().decode('ascii')
    logger.debug('Got response {} from ard'.format(resp.strip()))
    return resp

def get_ard_port():
    # Initializes arduino connection
    logger = get_logger()
    ### Will eventually be improved to search for/verify port and handle errors
    ser = serial.Serial('/dev/ttyACM0', 9600, timeout=2) # Open serial port in arduino
    logger.info('Opening serial port {}'.format(ser))
    sleep(10)
    for line in ser.readlines():
        logger.info(line)
    ser.flushInput()  #Flush the buffers
    ser.flushOutput() #Not sure why we do this, ask Scott
    return ser

def z_string_to_dict(ser):
    send_command_log(ser, 'z')
    z_string = get_response_log(ser)
    z_string = z_string.lstrip('0: GetData OK: ')
    1/0
    #Implement the rest here
