#!/usr/bin/python
import mpd
import sys

client = mpd.MPDClient()
client.connect("localhost", 6600)
 
cs = client.currentsong()
 
if 'title' in cs:
   SongTitle = cs['title']
elif 'file' in cs:
   SongTitle = cs['file']

if len(sys.argv) == 1:
   print ('listallinfo')
   print ('stats')
   print ('currentsong')
   print ('status')
   print ('listplaylists')
   print ('station')
elif sys.argv[1] == 'listallinfo':
   print (client.listallinfo())
elif sys.argv[1] == 'stats':
   print(client.stats())
elif sys.argv[1] == 'currentsong':
   print (client.currentsong())
elif sys.argv[1] == 'status':
   status = client.status()
   print 'songId:' + status['songid']
   print 'playlistLength:' + status['playlistlength']
   print 'state:' + status['state']
   print 'bitrate:' + status['bitrate']
   print 'audio:' + status['audio']
   print 'volume:' + status['volume']
elif sys.argv[1] == 'listplaylists':
   print (client.listplaylists())
elif sys.argv[1] == 'station':
   print (cs['file'])
else:
   print ('error')
