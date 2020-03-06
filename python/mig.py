import urllib
import urllib.request
import re
import numpy as np
from bs4 import BeautifulSoup
import xml.etree.ElementTree as ET
# import string
num = 0
tree = ET.parse("Data/posts.xml")
root = tree.getroot()
numPosts = int(len(root))

for num in range(numPosts):
    def strip_url(id):
        print("Searching for Post ID: {}".format(id))
        for x in range(len(root)):  # Find post ID matching the requested id
            child = root[x].attrib
            if int(child["ID"]) == id:  # When found...
                for i in range(len(branch)):    # Search in it for the "body tag"
                    if branch[i].tag == "body":     # When found...
                        print("Original Comment: {}".format(branch[i].text))
                        return branch[i].text       # Return the string
    # Get URLs from XML file

    branch = root[num]
    result = strip_url(num)
    r = re.findall(r"(https?://\S+)", result)       # Strip the URL from "result"
    url = r

    if url:
        print(url)
    else:
        print("No URL found")

    for webs in url:
        print("URL ", num, ": ", webs)
        # url = "https://bullshitnews.org"
        html = urllib.request.urlopen(webs).read()
        soup = BeautifulSoup(html)

        # kill all script and style elements
        for script in soup(["script", "style"]):
            script.extract()    # rip it out

        # get text
        text = soup.get_text()

        # break into lines and remove leading and trailing space on each
        lines = (line.strip() for line in text.splitlines())
        # break multi-headlines into a line each
        chunks = (phrase.strip() for line in lines for phrase in line.split("  "))
        # drop blank lines
        text = '\n'.join(chunk for chunk in chunks if chunk)

        r = re.compile(r'\bMAINSTREAM\sMEDIA\b', flags=re.I | re.X)
        s = re.compile(r'\bTRUMP\b', flags=re.I | re.X)
        t = re.compile(r'\bHOAX\b', flags=re.I | re.X)
        match = r.findall(text)
        matchs = s.findall(text)
        matcht = t.findall(text)
        if match:
            print("Dubious source found: We found ", np.size(match), "matches to mainstream media")
            mNumber = np.size(match)
        if matchs:
            print("Dubious source found: We found ", np.size(matchs), "matches to Trump")
            mNumbers = np.size(matchs)
        if matcht:
            print("Dubious source found: We found ", np.size(matcht), "matches to hoax")
            mNumbert = np.size(matcht)

    # if match < 1:
    #     print("Seems Legit - Green Alert")

    # if match >1 && < 3:
    #     print("Possible Fake News Outlet - Amber Alert")
    # elif match > 3:
    #     print("Fake News Outlet - Red ALert")
