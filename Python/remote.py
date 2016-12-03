#!/usr/bin/env python
#coding: utf8
import lirc
import time
import requests
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
        elif "KEY_UP" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Pandora/NextStation')
        elif "KEY_DOWN" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Pandora/PrevStation')
        elif "KEY_RIGHT" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Next')
        elif "KEY_LEFT" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Prev')
        elif "KEY_POWER" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/TurnOn')
        elif "KEY_POWER2" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/TurnOff')
        elif "KEY_PLAY" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Play')
        elif "KEY_1" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Pandora')
        elif "KEY_2" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Music')
        elif "KEY_3" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/XBox')
        elif "KEY_4" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/TV')
        elif "KEY_6" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Pandora/Tired')
        elif "KEY_7" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Pandora/ThumbDown')
        elif "KEY_8" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/SmartHouse/Notify')
        elif "KEY_9" in codeIR[0]:
            requests.get('http://10.110.166.90:8081/api/Pandora/ThumbUp')
    time.sleep(0.02)
    GPIO.output(21,GPIO.LOW)
