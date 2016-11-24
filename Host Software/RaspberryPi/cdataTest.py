#from lxml import etree
import xml.etree.ElementTree as ElementTree

s = str(566)
CONTENT = "![CDATA[" + s + "]]"

##def parse_with_lxml():
##    root = etree.fromstring(CONTENT)
##    for log in root.xpath("//log"):
##        print log.text

def parse_with_stdlib():
    root = ElementTree.fromstring(CONTENT)
    for log in root.iter('log'):
        
        log.text = log.text + 1
        print log.text

if __name__ == '__main__':
  #  parse_with_lxml()
    parse_with_stdlib()
