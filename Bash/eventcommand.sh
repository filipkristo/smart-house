#!/bin/bash

fold="$HOME/.config/pianobar"
stl="$fold/stationlist"
ctlf="$fold/ctl"
nowplaying="$fold/nowplaying"
response="$fold/response.txt"
debug="$fold/debug.txt"
threshold=50 # the percentage of the song that must have been played to scrobble
playedenough=240 # or if it has played for this many seconds
minduration=30 # minimum duration for a song to be scrobblable

while read L; do
        k="`echo "$L" | cut -d '=' -f 1`"
        v="`echo "$L" | cut -d '=' -f 2`"
        export "$k=$v"
done < <(grep -e '^\(title\|artist\|album\|stationName\|pRet\|pRetStr\|wRet\|wRetStr\|songDuration\|songPlayed\|rating\|songDuration\|songPlayed\|coverArt\|stationCount\|station[0-9]\+\)=' /dev/stdin)

case "$1" in
        songstart)
        echo -e "$artist \n$title \n$stationName \n$rating \n$coverArt \n$album \n$songDuration" > "$nowplaying"
	curl "http://10.110.166.90:8081/api/Pandora/Refresh"
	curl "http://10.110.166.90:8081/api/Pandora/UpdateNowPlaying"
;;
   songlove)
        echo -e "$artist \n$title \n$stationName \n$rating \n$coverArt \n$album \n$songDuration" > "$nowplaying"
	curl "http://10.110.166.90:8081/api/Pandora/Refresh"
;;
   songban)
   echo -e "$artist\n$title\n$stationName\n$rating\n$converArt\n$album\n$songDuration" > "$nowplaing"
;;
  songfinish)
  if [[ $songPlayed -gt $minduration ]]; then
    curl "http://10.110.166.90:8081/api/Pandora/StartScrobble"
    if [[ $rating -gt 0 ]]; then
      curl -G "http://10.110.166.90:8081/api/LastFM/LoveSong?" --data-urlencode "artistName=${artist}&songName=${title}"
    fi
  fi
;; 
    usergetstations)
      if [[ $stationCount -gt 0 ]]; then
        rm -f "$stl"
        for stnum in $(seq 0 $(($stationCount-1))); do
          echo "$stnum) "$(eval "echo \$station$stnum") >> "$stl"
        done
      fi

      ;;
      stationcreate)
           if [[ $stationCount -gt 0 ]]; then
                  rm -f "$stl"
                  for stnum in $(seq 0 $(($stationCount-1))); do
                         echo "$stnum) "$(eval "echo \$station$stnum") >> "$stl"
                  done
           fi

           ;;
   stationaddgenre)
         if [[ $stationCount -gt 0 ]]; then
                  rm -f "$stl"
                  for stnum in $(seq 0 $(($stationCount-1))); do
                         echo "$stnum) "$(eval "echo \$station$stnum") >> "$stl"
                  done
           fi

           ;;
   songexplain)
          if [[ $stationCount -gt 0 ]]; then
                  rm -f "$stl"
                  for stnum in $(seq 0 $(($stationCount-1))); do
                         echo "$stnum) "$(eval "echo \$station$stnum") >> "$stl"
                  done
           fi

           ;;
           stationaddshared)
          if [[ $stationCount -gt 0 ]]; then
                  rm -f "$stl"
                  for stnum in $(seq 0 $(($stationCount-1))); do
                         echo "$stnum) "$(eval "echo \$station$stnum") >> "$stl"
                  done
           fi

           ;;
        stationdelete)
           if [[ $stationCount -gt 0 ]]; then
                  rm -f "$stl"
                  for stnum in $(seq 0 $(($stationCount-1))); do
                         echo "$stnum) "$(eval "echo \$station$stnum") >> "$stl"
                  done
           fi
      echo -e "" > "$nowplaying"
           ;;
esac
