import ftplib
import os
import glob    
    
def upload_to_server(filename, username, password, server_address, server_path, new_directory):
   
    session = ftplib.FTP(server_address,username,password)
    print(session.getwelcome())

    file = open(filename,'rb')                  # file to send
    server_uploadedfile=os.path.basename(filename) #to obtain only the name of the file
   
    if server_path+new_directory in session.nlst(server_path):

        session.storbinary('STOR '+server_path+new_directory+'/'+server_uploadedfile, file)     # send the file
    else:
        session.mkd(server_path+new_directory)                                                  # create a directory if it does not exist
        session.storbinary('STOR '+server_path+new_directory+'/'+server_uploadedfile, file)     # send the file
        
    file.close()                                    # close file and FTP
    session.quit()

