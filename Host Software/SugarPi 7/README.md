# SugarPI Documentation
I'm going to give an overview of all the moving parts on the SugarPi as of June 9th, 2016. Most of the code is commented (To varying degrees of success) and I *hope* that I've managed to organize the project in a meaningful and logical way.
The current working directory structure is:
```
.
|-- Large_Scan_Sample
|-- Scan_Sample  
|-- XML_utils.py
|-- camera_utils.py
|-- config.yml
|-- logger_utils.py
|-- master.py
|-- median_stacking.py
|-- motion_python_api
|   |-- LICENSE
|   |-- README.md
|   |-- config.yml
|   `-- motion.py
|-- notes.txt
|-- overview.txt
|-- polling_server.py
|-- scratchspace
|   `-- lock_example.py
|-- server.py
|-- shootstring_utils.py
|-- tests
|   |-- shootstring_tests.py
|   `-- upload_trigger.py
`-- uploader.py
```
\_pycache_ directories and images have been pruned from this representation. The /static folder and its contents will be talked about lower down.

To get a decent overview of the backend, I'd start with polling_server, the class that master_server, uploader_server, arduino_server all inherit from. It's pretty much an object wrapper around the server from https://pymotw.com/2/select/ with a few improvements. I'd absolutely spend a few minutes on that link to get an overview of low-level socket programming, though the whole point of that class is to abstract away all of that from your buisness logic. There are three methods given in polling_server meant to be overloaded: process_msg, secondary_process, and on_error. The server is ~sorta based on RESTful principles, where each message is allocated a single response.

process_msg takes in a dictionary called msg and returns a dictionary called resp. Look at master.py or uploader.py to get a decent overview of this. Resp is placed in a queue for whichever client sent you the initial msg and is fired off as soon as possible.

Secondary_process is run after all the messages have been handled. Here, you'd do whatever it is you want the server to do when it isn't sending and recieving messages. For example, secondary_process in uploader.py checks to see if our upload is done and handles closing of the worker pool etc.

The brain of the project is master.py. Master.py spawns child processes (server.py and uploader.py) and attempts to initialize the Motion webcam server if it is not already turned on. Master.py waits for a message to start a scan. If the message contains a shootstring, master will use that. Otherwise it uses a default shootstring loaded from config. The shootstring is parsed and added to shootstring_buffer, where commands are popped off one at a time and sent to the arduino. The elevation and rotation for each photo are stored in a list which is used to create Manifest.XML at the end of the scan. Currently we don't really do anything with the arduino response except log it, but that should change. When master is done with the shoot string, it creates the XML manifest and sends a command to uploader.py to start its thing. 

I decided to use a buffer and pop off commands from the front for a couple reasons:
 - It makes it easier to pause a scan, insert new commands from developer mode, etc.
 - It provides for a logical way to seperate master's server duties (reading and responding to messages) and its buisness duties. Send a command, then check your messages.

Server.py is a simple flask server. I really didn't put as much effort into understanding how to create a RESTful API with a messaging standard and accurately named endpoints and kinda just went with whatever was easiest. Personally I think server.py can be merged with master.py and the arduino logic can be moved to a new server. All server.py currently does is pass messages to master and uploader and return the responses. Expect this to change if I have the time.

Trigger.py is a commandline tester. It bypasses everything flask to send signals directly to master.py and uploader.py, with no safety checks. (bad signals will be ignored anyways). It's not meant for consumers in any way, and I look forward to the day I replace it. --Jack Xie

Uploader.py handles uploading jobs to a remote directory. In order to do this, it creates 4 subprocesses using python's Multiprocessing module, and gives each of them a fourth of the files. Check out the scratchspace directory for some examples of ftp, multiprocessing with locks, and ftp with multiprocessing.
We found that for whatever reason, it was faster to upload from several processes instead of just one. Don't know the details on that.

And an sftp sample. Also wondering if multiprocess is still faster, now that ftp->sftp. Stay tuned. -- Jack Xie

*_utils files are all pretty self explanatory. They could probably be reorganized into a utils/ folder, or could be better grouped with the main processes they are used by (e.g. a master server folder, uploader server folder, etc). Check out the comments in them for details.

motion_python_api is an open-source project that I maintain (maintain is a loose word) at https://github.com/maxwellgerber/motion_python_api .  It is used to communicate with the Motion server. Right now all we do is take screenshots, though that may change.

Scratchspace is a folder where I'll put snippets of test code that I found useful. If I mock up an idea before adding it to the project, it will go in there.

Tests contains old testing utils (upload_trigger.py) and unit tests for parts of the project. Most of our project is over the web (REST API, sockets between processes, FTP) which makes it pretty hard to test. If you know of a framework to test parts of the project, PLEASE make use of it. To run the old tests, use the command nosetests from the root directory.

# Dependencies

We currently use pyyaml, motion_python_api, pyserial, flask, flask_restful, and nose. Expect flask_restul and pyyaml to be dropped in the near future. Motion_python_api isn't a pip package yet, but I might make it one in the near future if I have the time. For now just pull it from my github.

### Motion
Motion is the service we use to stream live video to our web browser. You can get it with 
```sh
$ apt-get install motion
```
or you can download it from their website, http://www.lavrsen.dk/foswiki/bin/view/Motion/WebcamServer.

When you download it, you need to change a couple parameters. In /etc/default/motion change start_motion_daemon to yes so we can run the process in the background. In /etc/motion/motion.conf change the height and width of the captured image to whatever value we are using (1024 x 768 currently). Change the default camera to whichever works best for you. On my laptop it is /dev/video1 because the webcam takes up /dev/video0. On the pi it is probably always going to be /dev/video0. Change the text in the lower right hand corner to be off. Change the output-normal parameter to be off.

There are a couple things you'll have to edit about the project to get it to work on your machine. Inside the config.yaml file there's a picture_directorry parameter that needs to change to match whatever settings you'd like it to. I'd like to revamp config.yaml in the near future to take out parts more closely related to our API. Each of the executables (master, server, uploader) needs to have executable privleges on your machine (chmod +x) and the shebang might need to change. On my current system it's python3 but on other systems I've seen python, py, and py3 be used. 

## Static Content
The current overview of the static folder is as follows:
``` sh
|-- static
|   |-- css
|   |   |-- bootstrap-dialog.min.css
|   |   |-- bootstrap.min.css
|   |   `-- index.css
|   |-- developer.html
|   |-- images
|   |   `-- SugarCube_Logo.jpg
|   |-- index.html
|   |-- index_app.html
|   |-- js
|   |   |-- angular.js
|   |   |-- app.js
|   |   |-- bootstrap-dialog.min.js
|   |   |-- bootstrap.min.js
|   |   |-- edit-table.js
|   |   |-- functions.js
|   |   |-- index.js
|   |   `-- jquery-2.2.4.min.js
|   |-- order_form.html
```
Index.html and developer.html are the two main pages we display. They are Single Page Applicatoins (SPA's) created with Bootstrap and AngularJS. I'm not a frontend guy at all and have next to no Angular experience. However my other internship is requiring me to learn Angular so after I get my experience there I'm probably going to tear down everything I've done and redo it. 

From a UI/UX perspective I've fallen in love with Material Design, a specification published by Google. Check out www.mdboostrap.com, polymer-project.org, and angular-ui.github.io/bootstrap/ for more. The UI right now is *functioning* but far from perfect. 


# Moving Forward

Although the foundation of the project is there, there is still a lot of work to do before we can even say we have a functioning prototype. There are many aspects of the project that we haven't even started. I'll attempt to list as many of them as I can in the todo section below.

I will also try to stay on the project for as much as my schedule allows me. I am taking a SugarCube with me to Foster City and will probably try to put in 1-2 hours of work a night depending on my other job. I'll also make myself available on Slack and through email as much as possible. I talked with Ben about coming in on the weekends and will try to do that as much as possible.

If I had to suggest a way to start working on the project for someone who was unfamillair with it, it would be:
 - Clone the git repo to your local machine. Install Motion and all python modules
 - Check out the *old* code on Asana under the SugarPi project. Read it and understand what we have done in the past and try to compare it to the new code. 
 - Run a few scans on the Windows app. Get a feel for the flow and start thinking about UI/UX on the browser side
 - If you don't know how to design a decent API, learn! I still haven't done this and it is by FAR my biggest mistake on the project. There's no concept of decent endpoints, no message standardization, it's a wreck.
 - Get an overview of what goes into Manifest.XML, the Z string, and what makes up a Shootstring. Ben has access to lots of documentation on that 
 - Spend at least a day or two looking over the codebase. Understand what plays well with what and what is hacky/ needs improvement
 
Ideally, you would do most of your work from developer.html, although it isn't properly implemented yet. Developer.html should end up being almost a 1-to-1 wrapper around the API, and it's hard to make that happen when there isn't an API. The most important thing to do IMO is to come up with an API and message protocol that accurately represents the Manifest parameters, the state of the robot, etc.

#### TODO
Master fails if no arduino detected  
Uploader fails if no network/ cannot connect to server  
Seperate log files for all processes please  
Refactor arduino control logic  
Implement Z string transfer between ard/etc.  
Merge server/master? Probably.  
Currently doing /here/is/my/folder_{} a lot  
Should probably change that to os.path.join   
Figure out API endpoints- read from file  
Move from config.yml to config.json and state.json - json all the way up!  
Verify XML format is correct  


####  DONE
Refactor polling_server into base class w/logging  
Refactor master into class  
Refactor uploader into class  
