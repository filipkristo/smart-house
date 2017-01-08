#!/usr/bin/env python
#coding: utf8
import lirc
import time
import requests
import os
import RPi.GPIO as GPIO

sockid=lirc.init("dev", blocking = False)

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)
GPIO.setup(21,GPIO.OUT)

while True:
    codeIR = lirc.nextcode()
    if codeIR != []:
        GPIO.output(21,GPIO.HIGH)
        print codeIR[0]
        if "KEY_VOLUMEUP" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/VolumeUp')
        elif "KEY_VOLUMEDOWN" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/VolumeDown')
        elif "KEY_FORWARD" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Next')
        elif "KEY_REWIND" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Prev')
        elif "KEY_POWER" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Power')
        elif "KEY_PLAY" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Play')
        elif "KEY_UP" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Remote/Up')
        elif "KEY_DOWN" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Remote/Down')
        elif "KEY_FN_F1" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Pandora')
        elif "KEY_FN_F2" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Music')
        elif "KEY_FN_F3" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/XBox')
        elif "KEY_FN_F4" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/TV')
        elif "KEY_KP1" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Remote/Love')
        elif "KEY_KP2" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Remote/Ban')
        elif "KEY_KP3" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Notify')
        else:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/TVCommand?c=' + codeIR[0])            
    time.sleep(0.02)
    GPIO.output(21,GPIO.LOW)

