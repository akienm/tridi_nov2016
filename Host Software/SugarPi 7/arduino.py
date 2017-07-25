import os, time
import threading, queue
import logger_utils

## Largely based on example found at http://eli.thegreenplace.net/2011/12/27/python-threads-communication-and-stopping
## Also with help from http://stackoverflow.com/questions/17553543/pyserial-non-blocking-read-loop
## 

class arduino_communication_thread(threading.Thread):
    """ A worker thread that sends commands to an Arduino over Serial and returns
        completed responses

        Input is done by commands (as strings) in the Queue object passed in by
        command_q. Inputs are popped off one at a time. It is recommended to have only 
        one input at a time if the results are important.

        Output is done by placing strings into the Queue passed in result_q.

        Ask the thread to stop by calling its join() method.
    """
    def __init__(self, command_q, result_q):
        super(arduino_communication_thread, self).__init__()
        self.command_q = command_q
        self.result_q = result_q
        self.stoprequest = threading.Event()

    def run(self):
        # As long as we weren't asked to stop, try to take new tasks from the
        # queue. The tasks are taken with a blocking 'get', so no CPU
        # cycles are wasted while waiting.
        # Also, 'get' is given a timeout, so stoprequest is always checked,
        # even if there's nothing in the queue.
        self.ser = logger_utils.get_ard_port()
        while not self.stoprequest.isSet():
            resp = ''
            try:
                cmd = self.command_q.get(True, 0.05)
                print("Sending {}".format(cmd))
                logger_utils.send_command_log(self.ser, cmd)
                cmd = None

            except queue.Empty:
                continue

            finally:
                resp += logger_utils.get_response_log(self.ser)
                if '\n' in resp:
                    tokens = resp.split('\n')
                    completed_response = tokens[0]
                    resp = '\n'.join(tokens[1:])
                    self.result_q.put(completed_response, True, 0.05)
        self.ser.close()

    def join(self, timeout=None):
        self.stoprequest.set()
        super(arduino_communication_thread, self).join(timeout)

# result = queue.Queue()
# command = queue.Queue()
# worker = arduino_communication_thread(command, result)
# worker.start()
