#include <HTTPClient.h>
#include <WiFi.h>
#include <espMqttClient.h>
#include <base64.hpp>

// thanks to https://hackaday.io/project/188839-gc9a01-flight-displays
#include "heading.h"
#include "plane.h"

#include <M5Unified.h>

#include <arduino_secrets.h>

const char* wifi_ssid = SECRET_WIFI_SSID;
const char* wifi_password = SECRET_WIFI_PASSWORD;

const char* mqtt_user = SECRET_MQTT_USER;
const char* mqtt_password = SECRET_MQTT_PASSWORD;

#define MQTT_HOST IPAddress(192, 168, 2, 34)
#define MQTT_PORT 1883

const char* home_assistant_prefix = "http://192.168.2.34:8123";

const char* mqtt_PlaneHeadingMagnetic_topic = "msfs/garming5/PlaneHeadingMagnetic";

float PlaneHeadingMagnetic;

espMqttClient mqttClient;
bool reconnectMqtt = false;
uint32_t lastReconnect = 0;

//M5Canvas MySprite(&M5.Display); 

M5Canvas fb(&M5.Display);
M5Canvas dial_s(&M5.Display);
M5Canvas plane_s(&M5.Display);


void WiFiEvent(WiFiEvent_t event) {
    Serial.printf("[WiFi-event] event: %d\n", event);
    switch (event) {
    case SYSTEM_EVENT_STA_GOT_IP:
        Serial.println("WiFi connected");
        Serial.println("IP address: ");
        Serial.println(WiFi.localIP());
        connectToMqtt();
        break;
    case SYSTEM_EVENT_STA_DISCONNECTED:
        Serial.println("WiFi lost connection");
        break;
    default:
        break;
    }
}

void connectToMqtt() {
    Serial.println("Connecting to MQTT...");
    if (!mqttClient.connect()) {
        reconnectMqtt = true;
        lastReconnect = millis();
        Serial.println("Connecting failed.");
    }
    else {
        reconnectMqtt = false;
    }
}

void onMqttConnect(bool sessionPresent) {
    Serial.println("Connected to MQTT.");
    Serial.print("Session present: ");
    Serial.println(sessionPresent);

    uint16_t packetIdSub = mqttClient.subscribe(mqtt_PlaneHeadingMagnetic_topic, 0);
    Serial.print("Subscribing at QoS 2, packetId: ");
    Serial.println(packetIdSub);

}

void onMqttDisconnect(espMqttClientTypes::DisconnectReason reason) {
    Serial.printf("Disconnected from MQTT: %u.\n", static_cast<uint8_t>(reason));

    if (WiFi.isConnected()) {
        reconnectMqtt = true;
        lastReconnect = millis();
    }
}

void onMqttMessage(const espMqttClientTypes::MessageProperties& properties, const char* topic, const uint8_t* payload, size_t len, size_t index, size_t total) {
    //Serial.println("Publish received:");
    //Serial.printf("  topic: %s\n  payload:", topic);

    if (strcmp(topic, mqtt_PlaneHeadingMagnetic_topic) == 0)
    {

        float planeHeadingMagneticNew = atof((char*)payload);
        // round to 1 digit
        planeHeadingMagneticNew = float(long(planeHeadingMagneticNew * 10)) / 10.0;

        //float planeHeadingMagneticNew = round(atof((char*)payload));

        if (planeHeadingMagneticNew != PlaneHeadingMagnetic) {

            PlaneHeadingMagnetic = planeHeadingMagneticNew;

            //MySprite.fillSprite(TFT_BLACK);
            //MySprite.drawFloat(PlaneHeadingMagnetic, 0, MySprite.width() / 2, MySprite.height() / 2);
            //MySprite.pushSprite(0, 0);

            dial_s.pushRotated(&fb, - PlaneHeadingMagnetic, 0xc);
            plane_s.pushSprite(&fb, 60, 7, 0);

            fb.pushSprite(0, 0);

        }

    }

}

void setup() {
    Serial.begin(115200);
    Serial.println();
    Serial.println();
    delay(1000);

    auto cfg = M5.config();
    M5.begin(cfg);

    //MySprite.createSprite(M5.Display.width(), M5.Display.height()); 
    //MySprite.setTextColor(GREEN, BLACK);
    //MySprite.setTextDatum(middle_center);
    //MySprite.setFont(&fonts::Roboto_Thin_24);
    //MySprite.setTextSize(1);

    //-----

    //M5.Display.setRotation(0);

    fb.setColorDepth(16);
    fb.createSprite(240, 240);
    fb.setPivot(120, 120);

    dial_s.setColorDepth(8);
    dial_s.createSprite(240, 240);
    dial_s.setPivot(120, 120);
    dial_s.pushImage(00, 00, 240, 240, heading);

    plane_s.setColorDepth(16);
    plane_s.createSprite(121, 190);
    plane_s.pushImage(00, 00, 121, 190, plane);

    //M5.Display.setPivot(120, 120);

    //-----
   

    mqttClient.setCredentials(mqtt_user, mqtt_password);
    mqttClient.setServer(MQTT_HOST, MQTT_PORT);

    mqttClient.onConnect(onMqttConnect);
    mqttClient.onDisconnect(onMqttDisconnect);
    mqttClient.onMessage(onMqttMessage);

    WiFi.setAutoConnect(false);
    WiFi.setAutoReconnect(true);
    WiFi.onEvent(WiFiEvent);

    //WiFi.mode(WIFI_STA);
    //WiFi.config(INADDR_NONE, INADDR_NONE, INADDR_NONE, INADDR_NONE);
    //WiFi.setHostname("abcd");

    Serial.println("Connecting to Wi-Fi...");
    WiFi.begin(wifi_ssid, wifi_password);
}

void loop() {

    M5.update();

    static uint32_t currentMillis = millis();

    if (reconnectMqtt && currentMillis - lastReconnect > 5000) {
        connectToMqtt();
    }

}