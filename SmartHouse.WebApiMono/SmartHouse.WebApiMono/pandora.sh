#!/bin/bash

#
#/etc/init.d/pianobar
#

NAME=pianobar
OPTIONS=""
PID=$(pidof $NAME)
USAGE=" * Usage: ./pandora.sh [start|status|stop|pause|skip|volup|voldown]"
VOLUME=`cat /var/www/pianobar/volume`
STATE=`cat /var/www/pianobar/state`
DIR=/usr/local/bin
fold=/var/www/pianobar
stl="$fold/stationlist"
ctlf="$fold/ctl"
nowplaying="$fold/nowplaying"
volumefile="$fold/volume"
statefile="$fold/state"

#** start - starts pianobar in the foreground (use with crontab)
function start {
	if [ "$PID" = "" ] ; then
		echo "Starting $NAME..."
		echo "0" > "$volumefile"
		echo "play" > "$statefile"
		"$DIR"/pianobar
		echo "  Done."
	else
		echo "$NAME is already running as $PID."
	fi
}

#** startbg - starts pianobar in the background 
function startbg {
	if [ "$PID" = "" ] ; then
		echo "Starting $NAME..."
		echo "0" > "$volumefile"
		echo "play" > "$statefile"
		pianobar > /dev/null 2>&1 &
		echo "Please wait..."
		sleep 3
		sleep 3
		sleep 3
		sleep 3
	else
		echo "$NAME is already running as $PID."
	fi
}

#** alarm - starts pianobar and sets volume to 10
function alarm {
	startbg
	sleep 20
	for i in {1..10}
		do 
			VOLUME=`cat /var/www/pianobar/volume`
			echo ')' > "$ctlf"
			echo `expr $VOLUME + 1` > "$volumefile"
			echo "Volume increased."
			sleep 2
		done
}

#** stop - stops pianobar
function stop {
	if [ "$PID" != "" ] ; then
		echo "Stopping $NAME..."
		kill $PID
		TIMEOUT=30
		START=$( date +%s)
		while [ $(( $( date +%s) - ${START} )) -lt ${TIMEOUT} ]; do
			PID=$(pidof $NAME)
			if [ "$PID" == "" ]; then break
			else sleep 2
			fi
		done
 		if [ "$PID" != "" ] ; then
			echo "Error: $NAME would not stop"
		else
			echo "" > "$nowplaying"
			echo "  Done."
		fi
	else
		echo "$NAME is not running."
	fi
}

#** pause - toggles pause/play of pianobar
function pause {
	if [ "$PID" != "" ] ; then
		echo 'p' > "$ctlf"
		if [ "$STATE" = "play" ] ; then
			echo "pause" > "$statefile"
			echo "Music paused."
		elif [ "$STATE" = "pause" ] ; then
			echo "play" > "$statefile"
			echo "Music resumed."
		fi
	fi
}

#** skip - skips song
function skip {
	if [ "$PID" != "" ] ; then
		echo 'n' > "$ctlf"
		echo "Song skipped."
		echo "Please wait..."
	fi
}

#** like - like song
function like {
	if [ "$PID" != "" ] ; then
		echo '+' > "$ctlf"
		echo "Song liked."
	fi
}

#** vol - set volume to a specified number
#param:
#$1 is target volume
function vol {
	VOLUME=`cat /var/www/pianobar/volume`
	if [ "$1" -lt "$VOLUME" ] ; then
		voldown
		vol $1
	elif [ "$1" -gt "$VOLUME" ] ; then
		volup
		vol $1
	fi
}

#** volup - increment volume by 1
function volup {
	if [ "$PID" != "" ] ; then
		echo ')' > "$ctlf"
		echo `expr $VOLUME + 1` > "$volumefile"
		echo "Volume increased."
	fi
}

#** volup - decrement volume by 1
function voldown {
	if [ "$PID" != "" ] ; then
		echo '(' > "$ctlf"
		echo `expr $VOLUME - 1` > "$volumefile"
		echo "Volume decreased."
	fi
}

#** status - displays information about current song, state, and volume
function status {
	if [ "$PID" = "" ]; then
		echo "$NAME is not running"
	else
		echo "$NAME is running with pid $PID"
		echo "$VOLUME"
		if [ "$STATE" = "play" ]; then
			echo "playing"
		else
			echo "paused"
		fi
		cat "$nowplaying"
	fi
}

if [ "$1" = "start" ]; then
	start
elif [ "$1" = "startbg" ]; then
	startbg
elif [ "$1" = "alarm" ]; then
	alarm
elif [ "$1" = "stop" ]; then
	stop
elif [ "$1" = "status" ]; then
	status
elif [ "$1" = "pause" ]; then
	pause
elif [ "$1" = "skip" ]; then
	skip
elif [ "$1" = "like" ]; then
	like
elif [ "$1" = "vol" ]; then
	vol $2
elif [ "$1" = "volup" ]; then
	volup
elif [ "$1" = "voldown" ]; then
	voldown
else
	echo "$USAGE";
fi
