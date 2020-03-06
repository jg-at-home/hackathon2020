import urllib
import urllib.request
import re
import numpy as np
from bs4 import BeautifulSoup
import xml.etree.ElementTree as ET

num = 0
tree = ET.parse("Data/posts.xml")               # Location of posts xml file
root = tree.getroot()
numPosts = int(len(root))

with open("Data/keywords.txt", "r") as f:
    f_kw = f.readlines()
for line in f_kw:
    key_words = line.split()

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

        unique_hit = 0

        text = '\n'.join(chunk for chunk in chunks if chunk)
        for i in range(len(key_words)):
            r = re.compile(key_words[i], flags=re.I | re.X)
            match = r.findall(text)
            if match:
                print("Dubious source found: We found ", np.size(match), "matches to {}".format(key_words[i]))
                mNumber = np.size(match)
                unique_hit += 1

        if unique_hit == 0:
            print("Keyword matches: {}\nSource looks good!".format(unique_hit))
        elif unique_hit == 1:
            print("Keyword matches: {}\nSource is dubious".format(unique_hit))
        elif unique_hit == 2:
            print("Keyword matches: {}\nSource is likely bullshit".format(unique_hit))
        else:
            print("Keyword matches: {}\nSource is total Garbage, you Nigel Farage!".format(unique_hit))
    # if match < 1:
    #     print("Seems Legit - Green Alert")

    # if match >1 && < 3:
    #     print("Possible Fake News Outlet - Amber Alert")
    # elif match > 3:
    #     print("Fake News Outlet - Red ALert")
