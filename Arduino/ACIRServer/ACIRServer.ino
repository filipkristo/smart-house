#include <SPI.h>
#include <IRremote.h>

byte incomingByte;
IRsend irsend;
int khz = 38; //NB Change this default value as neccessary to the correct modulation frequency

// ON
unsigned ON[] = {3452, 1648, 476, 376, 500, 356, 496, 376, 472, 376, 476, 372, 500, 360, 496, 1224, 476, 372, 476, 372, 500, 356, 496, 376, 472, 380, 472, 376, 500, 356, 496, 372, 476, 380, 472, 376, 496, 356, 496, 1224, 476, 380, 472, 1224, 500, 356, 496, 372, 476, 380, 472, 376, 496, 356, 500, 372, 476, 372, 476, 376, 496, 360, 496, 376, 476, 1224, 472, 376, 496, 1212, 492, 1228, 476, 1224, 472, 376, 496, 1212, 496, 1224, 476, 372, 476, 372, 500, 1208, 492, 380, 472, 1228, 476, 372, 496, 356, 500, 1224, 472, 380, 476, 372, 496, 1208, 496, 380, 468, 380, 472, 376, 500, 1204, 496, 380, 472, 376, 472, 1228, 496, 1208, 496, 376, 472, 380, 468, 380, 496, 1208, 496, 1228, 472, 1228, 468, 1228, 500, 1204, 496, 380, 472, 376, 472, 380, 496, 1204, 496, 376, 476, 372, 476, 1228, 496, 360, 496, 1220, 476, 1224, 476, 1228, 492, 360, 496, 376, 476, 372, 476, 1224, 496, 1212, 496, 376, 472, 1224, 476, 376, 496, 1212, 496, 372, 476, 376, 472, 376, 496, 1212, 492, 376, 476, 1224, 476, 1224, 496, 1208, 496, 1228, 472, 1228, 472, 376, 496, 356, 496, 380, 472, 376, 472, 380, 496, 356, 496, 380, 468, 380, 472, 376, 500, 356, 496, 372, 476, 376, 476, 372, 500, 356, 496, 376, 476, 376, 476, 372, 496, 360, 492, 380, 472, 376, 472, 380, 496, 356, 496, 380, 472, 376, 472, 380, 496, 356, 496, 376, 472, 376, 476, 372, 500, 360, 496, 372, 476, 376, 472, 376, 496, 364, 488, 380, 472, 376, 476, 376, 496, 356, 496, 380, 468, 380, 472, 380, 496, 356, 496, 376, 472, 380, 472, 376, 496, 360, 492, 376, 476, 376, 472, 1228, 500, 356, 496, 372, 476, 1228, 472, 1228, 496, 1208, 496, 1224, 472, 376, 476};
// OFF  AC
unsigned OFF[] = {3452, 1648, 476, 376, 496, 360, 492, 380, 476, 372, 476, 376, 496, 356, 496, 1228, 472, 380, 472, 376, 496, 360, 496, 376, 472, 376, 472, 380, 496, 356, 496, 376, 476, 372, 476, 376, 496, 356, 496, 1228, 472, 380, 472, 1224, 500, 356, 496, 376, 472, 380, 472, 376, 500, 356, 496, 372, 476, 376, 476, 372, 500, 360, 496, 372, 476, 1224, 476, 372, 500, 1204, 496, 1228, 472, 1228, 472, 380, 496, 1204, 500, 1224, 472, 380, 472, 1224, 500, 356, 496, 376, 476, 1224, 476, 376, 496, 360, 496, 1220, 476, 376, 476, 372, 500, 1208, 496, 376, 476, 372, 476, 372, 500, 1204, 496, 380, 472, 376, 472, 1228, 496, 1208, 496, 376, 472, 380, 472, 376, 500, 1204, 496, 1224, 476, 1228, 472, 1228, 496, 1204, 500, 372, 476, 376, 476, 372, 496, 364, 496, 372, 476, 376, 472, 1224, 500, 356, 496, 1228, 472, 1228, 472, 1228, 496, 356, 496, 376, 472, 376, 476, 1228, 492, 1212, 496, 372, 476, 1224, 476, 376, 496, 1208, 496, 376, 472, 380, 468, 380, 496, 1208, 496, 380, 468, 1228, 472, 1228, 496, 1208, 496, 1224, 476, 1224, 476, 376, 496, 356, 496, 380, 472, 376, 472, 380, 496, 356, 496, 376, 472, 380, 472, 376, 500, 356, 496, 372, 476, 376, 476, 372, 496, 360, 496, 376, 472, 380, 468, 380, 496, 360, 492, 380, 472, 376, 472, 380, 496, 360, 492, 376, 476, 372, 476, 380, 492, 360, 492, 380, 476, 372, 476, 376, 496, 356, 496, 380, 468, 380, 472, 376, 500, 356, 496, 372, 476, 380, 472, 376, 500, 356, 496, 372, 476, 376, 476, 372, 496, 364, 488, 380, 472, 376, 472, 380, 496, 360, 492, 380, 472, 376, 472, 380, 496, 1204, 500, 1220, 476, 376, 476, 1228, 492, 1212, 496, 1224, 476, 372, 472};

void setup() {
  Serial.begin(9600);
}

void loop() {

  if (Serial.available() > 0) 
  {
    // read the incoming byte:
    incomingByte = Serial.read();

    if(incomingByte == 48)
    {
        irsend.sendRaw(OFF, sizeof(OFF) / sizeof(int), khz);     
        Serial.println("TurnOff");
    }      
    if(incomingByte == 49)
    {
        irsend.sendRaw(ON, sizeof(ON) / sizeof(int), khz);     
        Serial.println("TurnOn");
    }      
  }  
      
} // fine loop


