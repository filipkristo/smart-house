/**
   BasicHTTPClient.ino

    Created on: 24.05.2015

*/
#include <DHT.h>;
#include <Arduino.h>
#include <ArduinoJson.h>

#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>

#include <ESP8266HTTPClient.h>

#include <WiFiClient.h>

#define DHTPIN 5        // what pin we're connected to
#define DHTTYPE DHT22   // DHT 22  (AM2302)

DHT dht(DHTPIN, DHTTYPE); //// Initialize DHT sensor for normal 16mhz Arduino
ESP8266WiFiMulti WiFiMulti;

const char* ssid     = "PiperNetwork5";         // The SSID (name) of the Wi-Fi network you want to connect to
const char* password = "XXX";     // The password of the Wi-Fi network

float hum;        //Stores humidity value
float temp;       //Stores temperature value
float heatIndex;  //Stores heat index value

void setup() {

  Serial.begin(115200);
  //Serial.setDebugOutput(true);

  Serial.println();
  Serial.println();
  Serial.println();

  for (uint8_t t = 4; t > 0; t--) {
    Serial.printf("[SETUP] WAIT %d...\n", t);
    Serial.flush();
    delay(1000);
  }

  WiFi.begin(ssid, password);
  Serial.print("Connecting to ");
  Serial.print(ssid); Serial.println(" ...");

  int i = 0;
  while (WiFi.status() != WL_CONNECTED) { // Wait for the Wi-Fi to connect
    delay(1000);
    Serial.print(++i); Serial.print(' ');
  }
  
  // wait for WiFi connection
  if ((WiFiMulti.run() == WL_CONNECTED)) {

    dht.begin();
    delay(3000);
    //Read data and store it to variables hum and temp
    hum = dht.readHumidity();
    temp = dht.readTemperature();
    heatIndex = dht.computeHeatIndex(temp, hum, false);
    
    //Print temp and humidity values to serial monitor
    Serial.print("Temp: ");
    Serial.print(temp);
    Serial.println(" Celsius");
    Serial.print(heatIndex);
    Serial.println(" Heat index");
    Serial.print("Humidity: ");
    Serial.print(hum);
    Serial.println(" %");    
    
    WiFiClient client;

    HTTPClient http;
    
    Serial.print("[HTTP] begin...\n");
    StaticJsonDocument<300> JSONbuffer;   //Declaring static JSON buffer
    JsonObject JSONencoder = JSONbuffer.to<JsonObject>(); 
    
    JSONencoder["deviceId"] = "2";
    JSONencoder["temperature"] = temp;
    JSONencoder["humidity"] = hum;
    JSONencoder["heatIndex"] = heatIndex;
    
    char JSONmessageBuffer[300];    
 
    http.begin("http://10.110.166.99/telemetryservice/api/AirSensor");      //Specify request destination
    http.addHeader("Content-Type", "application/json");  //Specify content-type header

    serializeJson(JSONbuffer, JSONmessageBuffer);
    
    int httpCode = http.POST(JSONmessageBuffer);   //Send the request
    String payload = http.getString();                                        //Get the response payload
 
    Serial.println(httpCode);   //Print HTTP return code
    Serial.println(payload);    //Print request response payload
 
    http.end();  //Close connection
    } else {
      Serial.printf("[HTTP} Unable to connect\n");
  }

  ESP.deepSleep(30e6); // 30e6 is 30 microseconds
}

void loop() {

}
