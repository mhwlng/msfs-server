#include "FS.h"
#include "SPIFFS.h"
#include <WiFi.h>
#include <espMqttClient.h>

#include <M5Unified.h>

#include <arduino_secrets.h>

const char* wifi_ssid = SECRET_WIFI_SSID;
const char* wifi_password = SECRET_WIFI_PASSWORD;

const char* mqtt_user = SECRET_MQTT_USER;
const char* mqtt_password = SECRET_MQTT_PASSWORD;

#define MQTT_HOST IPAddress(192, 168, 2, 34)
#define MQTT_PORT 1883

const char* mqtt_PlaneHeadingMagnetic_topic = "msfs/garming5/PlaneHeadingMagnetic";

float PlaneHeadingMagnetic = 0;

espMqttClient mqttClient;
bool reconnectMqtt = false;
uint32_t lastReconnect = 0;

//M5Canvas MySprite(&M5.Display); 

M5Canvas canvas(&M5.Display);

M5Canvas dial_s(&canvas);
M5Canvas plane_s(&canvas);

void WiFiEvent(WiFiEvent_t event) {
    Serial.printf("[WiFi-event] event: %d\n", event);
    switch (event) {
    case SYSTEM_EVENT_STA_GOT_IP:
        Serial.println("WiFi connected");
        Serial.println("IP address: ");
        Serial.println(WiFi.localIP());

        InitCanvas();

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
    Serial.print("Subscribing at QoS 0, packetId: ");
    Serial.println(packetIdSub);

}

void onMqttDisconnect(espMqttClientTypes::DisconnectReason reason) {
    Serial.printf("Disconnected from MQTT: %u.\n", static_cast<uint8_t>(reason));

    if (WiFi.isConnected()) {
        reconnectMqtt = true;
        lastReconnect = millis();
    }
}

void InitCanvas()
{
    //MySprite.createSprite(M5.Display.width(), M5.Display.height()); 
    //MySprite.setTextColor(GREEN, BLACK);
    //MySprite.setTextDatum(middle_center);
    //MySprite.setFont(&fonts::Roboto_Thin_24);
    //MySprite.setTextSize(1);

    canvas.setColorDepth(16);
    canvas.createSprite(240, 240);
    canvas.setPivot(120, 120);

    //-----

    dial_s.setColorDepth(8);
    dial_s.createSprite(240, 240);
    dial_s.setPivot(120, 120);
  
    auto dialBmp = SPIFFS.open("/dial.bmp");
    dial_s.drawBmp(&dialBmp, 0, 0);

    plane_s.setColorDepth(8);
    plane_s.createSprite(85, 106);

    auto planeBmp = SPIFFS.open("/plane.bmp");
    plane_s.drawBmp(&planeBmp, 0, 0);

}

void HeadingIndicator()
{
    //MySprite.fillSprite(TFT_BLACK);
    //MySprite.drawFloat(PlaneHeadingMagnetic, 0, MySprite.width() / 2, MySprite.height() / 2);
    //MySprite.pushSprite(0, 0);

    dial_s.pushRotated( - PlaneHeadingMagnetic, 0xc);

    plane_s.pushSprite(78, 63);

    canvas.fillRect(119, 15, 3, 51, 0xF9A0);

    canvas.pushSprite(0, 0);
}

void onMqttMessage(const espMqttClientTypes::MessageProperties& properties, const char* topic, const uint8_t* payload, size_t len, size_t index, size_t total) {
    //Serial.println("Publish received:");
    //Serial.printf("  topic: %s\n  payload:", topic);

    char* strval = new char[len+1];

    memcpy(strval, payload, len);
    strval[len] = '\0';

    if (strcmp(topic, mqtt_PlaneHeadingMagnetic_topic) == 0)
    {
        float planeHeadingMagneticNew = atof((char*)strval);
        // round to 1 digit
        planeHeadingMagneticNew = float(long(planeHeadingMagneticNew * 10)) / 10.0;

        //float planeHeadingMagneticNew = round(atof((char*)strval));

        //Serial.printf("payload: %f\n", planeHeadingMagneticNew);

        if (planeHeadingMagneticNew != PlaneHeadingMagnetic) {

            PlaneHeadingMagnetic = planeHeadingMagneticNew;

            HeadingIndicator();

        }

    }

    delete[] strval;


}

void setup() {
    Serial.begin(115200);
    Serial.println();
    Serial.println();
    delay(1000);

    SPIFFS.begin();

    auto cfg = M5.config();
    M5.begin(cfg);

    //------------

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