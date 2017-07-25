import select
import socket
import queue
import logger_utils

logger = logger_utils.get_logger()

class polling_server():
    def __init__(self, address, port, listeners = 5, timeout = .1):
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.setblocking(0)
#        print(port) #its so dirty aaaah
        s.bind((address, port))
        s.listen(listeners)
        self.server = s
        self.inputs = [s]
        self.outputs = []
        self.message_queues = {}
        self.timeout = timeout

    def process_msg(self, msg):
        # Takes a dictionary called msg and returns a dictionary response.
        # Overload this method in other classes
        return {}

    def secondary_process(self):
        # This is the code that runs when we are not scanning sockets.
        # Overload this method in other classes
        pass

    def on_error(self, error):
        # This captures all errors not handled elsewhere in code
        # Used to clean up anything you don't want to leave open (Sockets/IO/etc)
        pass

    def scan_sockets(self):
        readable, writable, exceptional = select.select(self.inputs, self.outputs, self.inputs, self.timeout)
        for s in readable:
            if s is self.server:
                # A "readable" server socket is ready to accept a connection
                connection, client_address = s.accept()
                # logger.debug('new connection from {}'.format(client_address))
                connection.setblocking(0)
                self.inputs.append(connection)
                self.message_queues[connection] = queue.Queue()
            else:
                msg = logger_utils.recieve_log(s)
                if msg:
                    resp = self.process_msg(msg)
                    self.message_queues[s].put_nowait(resp)
                    if s not in self.outputs:
                        self.outputs.append(s)
                else:
                    # Interpret empty result as closed connection
                    # logger.debug('closing {} after reading no data'.format(s.getpeername()))
                    # Stop listening for input on the connection
                    if s in self.outputs:
                        self.outputs.remove(s)
                    self.inputs.remove(s)
                    s.close()
                    del self.message_queues[s]

        # Stupid list comprehension to make sure we didn't just remove that socket
        for s in [s for s in writable if s in self.message_queues]:
            try:
                next_msg = self.message_queues[s].get_nowait()
            except queue.Empty:
                # No messages waiting so stop checking for writability.
                # logger.debug('output queue for {} is empty'.format(s.getpeername()))
                self.outputs.remove(s)
            else:
                logger.debug('sending {} to {}'.format(next_msg, s.getpeername()))
                logger_utils.sendall_log(s,next_msg)

        for s in exceptional:
            logger.warning('handling exceptional condition for {}'.format(s.getpeername()))
            # Stop listening for input on the connection
            self.inputs.remove(s)
            if s in self.outputs:
                self.outputs.remove(s)
            s.close()

    def run(self):
        try:
            while True:
                self.scan_sockets()
                self.secondary_process()
        except Exception as e:
            # Exception stack trace is written to log
            logger.warning('BAD STUFF HAPPENED', exc_info=True)
            print(e)
            self.on_error(e)
