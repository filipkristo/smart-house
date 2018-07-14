#include <SPI.h>
#include <IRremote.h>

byte incomingByte;
byte state;

IRsend irsend;
int khz = 38; //NB Change this default value as neccessary to the correct modulation frequency

// ON
unsigned ON[] =  {3448, 1656, 492, 360, 496, 376, 468, 384, 468, 380, 496, 360, 492, 380, 468, 1228, 476, 380, 492, 356, 496, 380, 472, 380, 468, 380, 492, 360, 496, 376, 472, 380, 472, 376, 496, 360, 496, 376, 468, 1232, 468, 384, 492, 1208, 496, 380, 468, 384, 468, 380, 492, 360, 492, 384, 468, 376, 476, 376, 496, 360, 492, 380, 472, 376, 472, 1232, 492, 360, 492, 1232, 468, 1232, 468, 1228, 496, 360, 492, 1228, 472, 1232, 468, 380, 496, 360, 488, 1232, 472, 380, 468, 384, 488, 360, 496, 380, 468, 380, 472, 1228, 496, 360, 492, 380, 472, 376, 472, 1232, 492, 360, 492, 1232, 468, 380, 468, 384, 492, 1208, 496, 1228, 472, 376, 472, 1232, 492, 360, 492, 1232, 468, 1232, 468, 1228, 496, 1208, 496, 1228, 472, 380, 468, 380, 496, 360, 488, 1232, 472, 380, 468, 384, 488, 360, 496, 380, 468, 384, 468, 376, 496, 360, 496, 376, 472, 380, 468, 380, 496, 360, 488, 384, 468, 384, 468, 380, 492, 1212, 492, 1232, 468, 384, 468, 380, 492, 360, 492, 384, 468, 376, 476, 376, 492, 364, 492, 376, 476, 376, 468, 384, 492, 360, 492, 380, 468, 384, 468, 384, 464, 388, 492, 380, 468, 384, 468, 380, 492, 1208, 496, 380, 472, 380, 468, 384, 488, 360, 496, 380, 468, 380, 468, 380, 496, 360, 492, 380, 468, 384, 468, 380, 496, 360, 488, 384, 468, 380, 468, 384, 488, 368, 488, 384, 468, 380, 468, 384, 488, 364, 492, 384, 468, 380, 468, 384, 488, 360, 496, 380, 464, 384, 468, 380, 496, 360, 492, 380, 468, 384, 468, 380, 492, 364, 488, 384, 468, 384, 468, 380, 492, 360, 492, 384, 468, 380, 468, 384, 488, 1212, 492, 1232, 468, 380, 468, 384, 468, 1236, 492, 380, 468, 1232, 472, 380, 468};
// OFF  AC
unsigned OFF[] = {3464, 1632, 496, 380, 468, 380, 492, 364, 488, 360, 492, 380, 468, 384, 492, 1212, 488, 360, 496, 380, 468, 380, 492, 360, 492, 360, 492, 380, 468, 380, 496, 360, 464, 388, 488, 384, 468, 380, 492, 1212, 488, 360, 496, 1228, 468, 384, 492, 360, 488, 364, 492, 380, 468, 384, 488, 360, 492, 360, 492, 384, 468, 376, 496, 360, 492, 1208, 492, 380, 468, 1236, 488, 1212, 488, 1212, 496, 376, 468, 1232, 472, 1236, 484, 364, 492, 1232, 468, 380, 492, 360, 492, 360, 492, 380, 468, 384, 488, 368, 484, 1212, 492, 384, 464, 380, 496, 360, 464, 1236, 492, 380, 468, 1236, 488, 364, 488, 360, 492, 1232, 468, 1232, 488, 368, 484, 1216, 488, 384, 464, 1232, 496, 1208, 488, 1216, 488, 1232, 468, 1236, 488, 360, 492, 360, 488, 384, 468, 384, 488, 364, 488, 364, 492, 380, 468, 384, 488, 364, 488, 360, 488, 384, 468, 384, 464, 388, 460, 392, 492, 380, 468, 384, 488, 364, 488, 364, 464, 1260, 464, 1232, 464, 392, 456, 392, 492, 380, 472, 380, 488, 368, 484, 364, 488, 384, 464, 388, 464, 412, 432, 420, 460, 384, 468, 384, 464, 416, 432, 416, 464, 384, 444, 408, 464, 412, 436, 412, 464, 1240, 464, 380, 464, 416, 436, 416, 464, 384, 440, 408, 464, 412, 440, 412, 464, 388, 464, 384, 464, 412, 440, 412, 464, 380, 472, 380, 464, 416, 436, 416, 436, 408, 440, 412, 464, 412, 440, 412, 464, 384, 464, 388, 464, 412, 436, 416, 464, 380, 444, 408, 464, 412, 440, 412, 464, 384, 464, 388, 464, 412, 440, 412, 464, 384, 468, 380, 464, 416, 436, 416, 436, 408, 440, 412, 468, 408, 440, 412, 464, 384, 464, 388, 464, 412, 436, 416, 464, 1232, 492, 360, 520, 1180, 516, 336, 512};

void setup() {
  Serial.begin(9600);
  state = 0;
}

void loop() {

  if (Serial.available() > 0) 
  {
    // read the incoming byte:
    incomingByte = Serial.read();

    if(incomingByte == 48)
    {
        irsend.sendRaw(OFF, sizeof(OFF) / sizeof(int), khz);     
        state = 0;
        Serial.println("TurnOff");
    }
    else if(incomingByte == 49)
    {
        irsend.sendRaw(ON, sizeof(ON) / sizeof(int), khz);
        state = 1;
        Serial.println("TurnOn");
    }
    else if(incomingByte == 63)
    {
        Serial.println(state);    
    }
  }  
      
} // fine loop


