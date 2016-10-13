#!/usr/bin/python
import mpd
 
client = mpd.MPDClient()
client.connect("localhost", 6600)
 
cs = client.currentsong()
 
if 'title' in cs:
   SongTitle = cs['title']
elif 'file' in cs:
   SongTitle = cs['file']

print cs['name']
print SongTitle 

client.disconnect();
