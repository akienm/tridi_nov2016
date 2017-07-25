#*********************************************************************************#
#                   Generating XML Manifest                                       #
#*********************************************************************************#

from xml.dom import minidom
import csv
import datetime
import xml.etree.ElementTree as ET
import yaml
import os

config = yaml.safe_load(open("config.yml"))

def dictionary_to_XML(dictionary, root_name='SugarCubeScan'):
    root = ET.Element(root_name)
    _dictionary_to_XML(root, dictionary)
    return root

def _dictionary_to_XML(element, dictionary):
    assert type(dictionary) is dict
    for key in dictionary.keys():
        value = dictionary[key]
        sub = ET.SubElement(element, key) 
        if type(value) is dict:
            _dictionary_to_XML(sub, dictionary[key])
        else:
            text = str(value)
            if('[' in text  or ']' in text):
                text = make_cdata(text)
            sub.text = text

def make_cdata(string):
    return "<![CDATA[{}]]>".format(string)

def xml_from_config(settings= None, dataFile = None, imageList = {}):
    ##Create XML Manifest object, populate it with image data, then save to file
    print('Datafile to write to:" {}'.format(dataFile))
    print('imageList: {}'.format(imageList))
    if settings is None:
        settings = yaml.safe_load(open("scan_information.yml"))
    sugarCubeScan = dictionary_to_XML(settings)
    sugarCubeScan.set('manifest_version','r1.3')
    imageSet = ET.SubElement(sugarCubeScan,'ImageSet')
    imageSet.set("id", "TesterScan")       
    for img in imageList.keys():
        elem = ET.SubElement(imageSet,'image')
        elem.set("elevation", str(imageList[img]['elevation']))
        elem.set("rotation", str(imageList[img]['rotation']))
        elem.text = img
    if(dataFile):
        ET.ElementTree(sugarCubeScan).write(dataFile)
    return sugarCubeScan
