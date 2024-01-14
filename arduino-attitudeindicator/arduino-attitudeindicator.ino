#include <WiFi.h>
#include <espMqttClient.h>
#include <math.h>

#include <M5Unified.h>

#include <arduino_secrets.h>

const char* wifi_ssid = SECRET_WIFI_SSID;
const char* wifi_password = SECRET_WIFI_PASSWORD;

const char* mqtt_user = SECRET_MQTT_USER;
const char* mqtt_password = SECRET_MQTT_PASSWORD;

#define MQTT_HOST IPAddress(192, 168, 2, 34)
#define MQTT_PORT 1883

const char* mqtt_BankDegrees_topic = "msfs/garming5/BankDegrees";
const char* mqtt_PitchDegrees_topic = "msfs/garming5/PitchDegrees";

float BankDegrees  = 0;
float PitchDegrees = 0;

espMqttClient mqttClient;
bool reconnectMqtt = false;
uint32_t lastReconnect = 0;

M5Canvas canvas(&M5.Display);

// thanks to https://www.hackster.io/SeeedStudio/wio-terminal-attitude-indicator-eae8d6

#define EARTH24 0xB45F06U
#define SKY24   0x0000FFU
#define DEARTH24 0xA55706U
#define DSKY24   0x0000DBU

M5Canvas bank(&canvas);
M5Canvas pitch(&bank);

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

    uint16_t packetIdSub1 = mqttClient.subscribe(mqtt_BankDegrees_topic, 0);
    Serial.print("Subscribing at QoS 0, packetId: ");
    Serial.println(packetIdSub1);

    uint16_t packetIdSub2 = mqttClient.subscribe(mqtt_PitchDegrees_topic, 0);
    Serial.print("Subscribing at QoS 0, packetId: ");
    Serial.println(packetIdSub2);

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
    canvas.setColorDepth(16);
    canvas.createSprite(180, 180);
    canvas.setPivot(120, 120);

    bank.setColorDepth(16);
    bank.createSprite(180, 180);

    pitch.setColorDepth(16);
    pitch.createSprite(140, 140);

    //Pitch sprite
    pitch.fillArc(70, 70, 70, 1, 0, 180, DEARTH24);
    pitch.fillArc(70, 70, 70, 1, 180, 360, DSKY24);
    pitch.drawFastHLine(0, 70, 140, TFT_WHITE);   //0
    pitch.drawFastHLine(30, 30, 80, TFT_WHITE);   //20
    pitch.drawFastHLine(50, 50, 40, TFT_WHITE);   //10
    pitch.drawFastHLine(55, 60, 30, TFT_WHITE);   //5
    pitch.drawFastHLine(55, 40, 30, TFT_WHITE);   //15

    //Bank sprite
    pitch.setPivot(70, 70);
    pitch.pushRotateZoom(90, 90, 0, 1, 1);

    bank.fillArc(90, 90, 90, 70, 0, 180, EARTH24);
    bank.fillArc(90, 90, 90, 70, 180, 360, SKY24);
    bank.fillTriangle(90, 20, 85, 3, 95, 3, TFT_WHITE);
    bank.drawFastHLine(0, 90, 20, TFT_WHITE);
    bank.drawFastHLine(160, 90, 20, TFT_WHITE);
    bank.drawLine(90 - 90.0 * cos(30.0 / 180.0 * PI), 90 - 90.0 * sin(30.0 / 180.0 * PI), 90 - 70.0 * cos(30.0 / 180.0 * PI), 90 - 70.0 * sin(30.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 - 90.0 * cos(60.0 / 180.0 * PI), 90 - 90.0 * sin(60.0 / 180.0 * PI), 90 - 70.0 * cos(60.0 / 180.0 * PI), 90 - 70.0 * sin(60.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 - 80.0 * cos(70.0 / 180.0 * PI), 90 - 80.0 * sin(70.0 / 180.0 * PI), 90 - 70.0 * cos(70.0 / 180.0 * PI), 90 - 70.0 * sin(70.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 - 80.0 * cos(80.0 / 180.0 * PI), 90 - 80.0 * sin(80.0 / 180.0 * PI), 90 - 70.0 * cos(80.0 / 180.0 * PI), 90 - 70.0 * sin(80.0 / 180.0 * PI), TFT_WHITE);
    bank.fillTriangle(40, 40, 35, 31, 31, 35, TFT_WHITE);
    bank.drawLine(90 + 90.0 * cos(30.0 / 180.0 * PI), 90 - 90.0 * sin(30.0 / 180.0 * PI), 90 + 70.0 * cos(30.0 / 180.0 * PI), 90 - 70.0 * sin(30.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 + 90.0 * cos(60.0 / 180.0 * PI), 90 - 90.0 * sin(60.0 / 180.0 * PI), 90 + 70.0 * cos(60.0 / 180.0 * PI), 90 - 70.0 * sin(60.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 + 80.0 * cos(70.0 / 180.0 * PI), 90 - 80.0 * sin(70.0 / 180.0 * PI), 90 + 70.0 * cos(70.0 / 180.0 * PI), 90 - 70.0 * sin(70.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 + 80.0 * cos(80.0 / 180.0 * PI), 90 - 80.0 * sin(80.0 / 180.0 * PI), 90 + 70.0 * cos(80.0 / 180.0 * PI), 90 - 70.0 * sin(80.0 / 180.0 * PI), TFT_WHITE);
    bank.fillTriangle(139, 40, 144, 31, 148, 35, TFT_WHITE);

    bank.setPivot(90, 90);
    bank.pushRotateZoom(90, 90, 0, 1, 1);

    canvas.drawTriangle(90, 20, 85, 30, 95, 30, TFT_ORANGE);
    canvas.drawCircle(90, 90, 5, TFT_RED);
    canvas.drawFastHLine(30, 90, 50, TFT_ORANGE);
    canvas.drawFastHLine(100, 90, 50, TFT_ORANGE);
    canvas.drawFastVLine(80, 90, 5, TFT_ORANGE);
    canvas.drawFastVLine(100, 90, 5, TFT_ORANGE);
    canvas.drawCircle(90, 90, 70, TFT_BLACK);

    canvas.pushSprite(30, 30);

}

void AttitudeIndicator()
{
    //pitch sprite
    pitch.fillRect(0, 0, 140, 70 + 2 * PitchDegrees, DSKY24);
    pitch.fillRect(0, 70 + 2 * PitchDegrees, 140, 140, DEARTH24);
    pitch.drawFastHLine(0, 70 + 2 * PitchDegrees, 140, TFT_WHITE);   //0
    pitch.drawFastHLine(30, 30 + 2 * PitchDegrees, 80, TFT_WHITE);   //20
    pitch.drawFastHLine(50, 50 + 2 * PitchDegrees, 40, TFT_WHITE);   //10
    pitch.drawFastHLine(55, 60 + 2 * PitchDegrees, 30, TFT_WHITE);   //5
    pitch.drawFastHLine(55, 40 + 2 * PitchDegrees, 30, TFT_WHITE);   //15
    pitch.drawFastHLine(30, 110 + 2 * PitchDegrees, 80, TFT_BLACK);   //20
    pitch.drawFastHLine(50, 90 + 2 * PitchDegrees, 40, TFT_BLACK);   //10
    pitch.drawFastHLine(55, 80 + 2 * PitchDegrees, 30, TFT_BLACK);   //5
    pitch.drawFastHLine(55, 100 + 2 * PitchDegrees, 30, TFT_BLACK);   //15
    //pitch.fillArc(70,70,70,90,0,359,TFT_BLACK);
    pitch.fillTriangle(0, 0, 41, 0, 0, 41, TFT_BLACK);
    pitch.fillTriangle(140, 0, 99, 0, 140, 41, TFT_BLACK);
    pitch.fillTriangle(140, 140, 140, 99, 99, 140, TFT_BLACK);
    pitch.fillTriangle(0, 140, 41, 140, 0, 99, TFT_BLACK);
    pitch.setTextSize(1);
    pitch.drawNumber(20, 20, 27 + 2 * PitchDegrees);  pitch.drawNumber(20, 110, 27 + 2 * PitchDegrees);
    pitch.drawNumber(10, 20, 47 + 2 * PitchDegrees);  pitch.drawNumber(10, 110, 47 + 2 * PitchDegrees);
    pitch.drawNumber(20, 20, 107 + 2 * PitchDegrees);  pitch.drawNumber(20, 110, 107 + 2 * PitchDegrees);
    pitch.drawNumber(10, 20, 87 + 2 * PitchDegrees);  pitch.drawNumber(10, 110, 87 + 2 * PitchDegrees);

    //Bank sprite
    pitch.setPivot(70, 70);
    pitch.pushRotateZoom(90, 90, 0, 1, 1);

    bank.fillArc(90, 90, 90, 70, 0, 180, EARTH24);
    bank.fillArc(90, 90, 90, 70, 180, 360, SKY24);
    bank.fillTriangle(90, 20, 85, 3, 95, 3, TFT_WHITE);
    bank.drawFastHLine(0, 90, 20, TFT_WHITE);
    bank.drawFastHLine(160, 90, 20, TFT_WHITE);
    bank.drawLine(90 - 90.0 * cos(30.0 / 180.0 * PI), 90 - 90.0 * sin(30.0 / 180.0 * PI), 90 - 70.0 * cos(30.0 / 180.0 * PI), 90 - 70.0 * sin(30.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 - 90.0 * cos(60.0 / 180.0 * PI), 90 - 90.0 * sin(60.0 / 180.0 * PI), 90 - 70.0 * cos(60.0 / 180.0 * PI), 90 - 70.0 * sin(60.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 - 80.0 * cos(70.0 / 180.0 * PI), 90 - 80.0 * sin(70.0 / 180.0 * PI), 90 - 70.0 * cos(70.0 / 180.0 * PI), 90 - 70.0 * sin(70.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 - 80.0 * cos(80.0 / 180.0 * PI), 90 - 80.0 * sin(80.0 / 180.0 * PI), 90 - 70.0 * cos(80.0 / 180.0 * PI), 90 - 70.0 * sin(80.0 / 180.0 * PI), TFT_WHITE);
    bank.fillTriangle(40, 40, 35, 31, 31, 35, TFT_WHITE);
    bank.drawLine(90 + 90.0 * cos(30.0 / 180.0 * PI), 90 - 90.0 * sin(30.0 / 180.0 * PI), 90 + 70.0 * cos(30.0 / 180.0 * PI), 90 - 70.0 * sin(30.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 + 90.0 * cos(60.0 / 180.0 * PI), 90 - 90.0 * sin(60.0 / 180.0 * PI), 90 + 70.0 * cos(60.0 / 180.0 * PI), 90 - 70.0 * sin(60.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 + 80.0 * cos(70.0 / 180.0 * PI), 90 - 80.0 * sin(70.0 / 180.0 * PI), 90 + 70.0 * cos(70.0 / 180.0 * PI), 90 - 70.0 * sin(70.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 + 80.0 * cos(80.0 / 180.0 * PI), 90 - 80.0 * sin(80.0 / 180.0 * PI), 90 + 70.0 * cos(80.0 / 180.0 * PI), 90 - 70.0 * sin(80.0 / 180.0 * PI), TFT_WHITE);
    bank.fillTriangle(139, 40, 144, 31, 148, 35, TFT_WHITE);

    bank.setPivot(90, 90);
    pitch.setPivot(70, 70);

    bank.pushRotateZoom(90, 90, BankDegrees, 1, 1);

    canvas.drawTriangle(90, 20, 85, 30, 95, 30, TFT_ORANGE);
    canvas.drawCircle(90, 90, 5, TFT_RED);
    canvas.drawFastHLine(30, 90, 50, TFT_ORANGE);
    canvas.drawFastHLine(100, 90, 50, TFT_ORANGE);
    canvas.drawFastVLine(80, 90, 5, TFT_ORANGE);
    canvas.drawFastVLine(100, 90, 5, TFT_ORANGE);
    canvas.drawCircle(90, 90, 70, TFT_BLACK);

    canvas.pushSprite(30, 30);
} 

void onMqttMessage(const espMqttClientTypes::MessageProperties& properties, const char* topic, const uint8_t* payload, size_t len, size_t index, size_t total) {
    //Serial.println("Publish received:");
    Serial.printf("  topic: %s\n  payload:", topic);

    char* strval = new char[len + 1];

    memcpy(strval, payload, len);
    strval[len] = '\0';
    
    if (strcmp(topic, mqtt_PitchDegrees_topic) == 0)
    {
        float pitchDegreesNew = atof((char*)strval);
        // round to 1 digit
        pitchDegreesNew = float(long(pitchDegreesNew * 10)) / 10.0;

        //float pitchDegreesNew = round(atof((char*)strval));

        Serial.printf("pitch: %f\n", pitchDegreesNew);

        if (pitchDegreesNew != PitchDegrees) {

            PitchDegrees = pitchDegreesNew;

            AttitudeIndicator();
        }

    }
    else if (strcmp(topic, mqtt_BankDegrees_topic) == 0)
    {
        float bankDegreesNew = atof((char*)strval);
        // round to 1 digit
        bankDegreesNew = float(long(bankDegreesNew * 10)) / 10.0;

        //float bankDegreesNew = round(atof((char*)strval));

        Serial.printf("bank: %f\n", bankDegreesNew);

        if (bankDegreesNew != BankDegrees) {

            BankDegrees = bankDegreesNew;

            AttitudeIndicator();
        }

    } 

    delete[] strval;

}

void setup() {
    Serial.begin(115200);
    Serial.println();
    Serial.println();
    delay(1000);

    auto cfg = M5.config();
    M5.begin(cfg);

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