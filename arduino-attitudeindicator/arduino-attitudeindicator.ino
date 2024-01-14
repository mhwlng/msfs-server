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
    canvas.setColorDepth(8);
    canvas.createSprite(240, 240);
    canvas.setPivot(120, 120);

    bank.setColorDepth(8);
    bank.createSprite(240, 240);

    pitch.setColorDepth(8);
    pitch.createSprite(187, 187);

}


#define EARTH 0xB45F06U
#define SKY   0x0000FFU
#define DEARTH 0x824C13U
#define DSKY   0x1414B8U

void AttitudeIndicator()
{
    float mult = 240.0 / 180.0; // original image was 180x180

    //pitch sprite
    pitch.fillRect(0, 0, 140 * mult, 70 * mult + 2 * mult * PitchDegrees, DSKY);
    pitch.fillRect(0, 70 * mult + 2 * mult * PitchDegrees, 140 * mult, 140 * mult, DEARTH);
    pitch.drawFastHLine(0, 70 * mult + 2 * mult * PitchDegrees, 140 * mult, TFT_WHITE);   //0
    pitch.drawFastHLine(30 * mult, 30 * mult + 2 * mult * PitchDegrees, 80 * mult, TFT_WHITE);   //20
    pitch.drawFastHLine(50 * mult, 50 * mult + 2 * mult * PitchDegrees, 40 * mult, TFT_WHITE);   //10
    pitch.drawFastHLine(55 * mult, 60 * mult + 2 * mult * PitchDegrees, 30 * mult, TFT_WHITE);   //5
    pitch.drawFastHLine(55 * mult, 40 * mult + 2 * mult * PitchDegrees, 30 * mult, TFT_WHITE);   //15
    pitch.drawFastHLine(30 * mult, 110 * mult + 2 * mult * PitchDegrees, 80 * mult, TFT_BLACK);   //20
    pitch.drawFastHLine(50 * mult, 90 * mult + 2 * mult * PitchDegrees, 40 * mult, TFT_BLACK);   //10
    pitch.drawFastHLine(55 * mult, 80 * mult + 2 * mult * PitchDegrees, 30 * mult, TFT_BLACK);   //5
    pitch.drawFastHLine(55 * mult, 100 * mult + 2 * mult * PitchDegrees, 30 * mult, TFT_BLACK);   //15

    pitch.fillTriangle(0, 0, 41 * mult, 0, 0, 41 * mult, TFT_BLACK);
    pitch.fillTriangle(140 * mult, 0, 99 * mult, 0, 140 * mult, 41 * mult, TFT_BLACK);
    pitch.fillTriangle(140 * mult, 140 * mult, 140 * mult, 99 * mult, 99 * mult, 140 * mult, TFT_BLACK);
    pitch.fillTriangle(0, 140 * mult, 41 * mult, 140 * mult, 0, 99 * mult, TFT_BLACK);
    pitch.setTextSize(1);
    pitch.drawNumber(20, 20 * mult, 27 * mult + 2 * mult * PitchDegrees);
    pitch.drawNumber(20, 110 * mult, 27 * mult + 2 * mult * PitchDegrees);
    pitch.drawNumber(10, 20 * mult, 47 * mult + 2 * mult * PitchDegrees);
    pitch.drawNumber(10, 110 * mult, 47 * mult + 2 * mult * PitchDegrees);
    pitch.drawNumber(20, 20 * mult, 107 * mult + 2 * mult * PitchDegrees);
    pitch.drawNumber(20, 110 * mult, 107 * mult + 2 * mult * PitchDegrees);
    pitch.drawNumber(10, 20 * mult, 87 * mult + 2 * mult * PitchDegrees);
    pitch.drawNumber(10, 110 * mult, 87 * mult + 2 * mult * PitchDegrees);

    //Bank sprite
    pitch.setPivot(70 * mult, 70 * mult);
    pitch.pushRotateZoom(90 * mult, 90 * mult, 0, 1, 1);

    bank.fillArc(90 * mult, 90 * mult, 90 * mult, 70 * mult, 0, 180, EARTH);
    bank.fillArc(90 * mult, 90 * mult, 90 * mult, 70 * mult, 180, 360, SKY);
    bank.fillTriangle(90 * mult, 20 * mult, 85 * mult, 3 * mult, 95 * mult, 3 * mult, TFT_WHITE);
    bank.drawFastHLine(0, 90 * mult, 20 * mult, TFT_WHITE);
    bank.drawFastHLine(160 * mult, 90 * mult, 20 * mult, TFT_WHITE);
    
    bank.drawLine(90 * mult - 90.0 * mult * cos(30.0 / 180.0 * PI), 90 * mult - 90.0 * mult * sin(30.0 / 180.0 * PI), 90 * mult - 70.0 * mult * cos(30.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(30.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 * mult - 90.0 * mult * cos(60.0 / 180.0 * PI), 90 * mult - 90.0 * mult * sin(60.0 / 180.0 * PI), 90 * mult - 70.0 * mult * cos(60.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(60.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 * mult - 80.0 * mult * cos(70.0 / 180.0 * PI), 90 * mult - 80.0 * mult * sin(70.0 / 180.0 * PI), 90 * mult - 70.0 * mult * cos(70.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(70.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 * mult - 80.0 * mult * cos(80.0 / 180.0 * PI), 90 * mult - 80.0 * mult * sin(80.0 / 180.0 * PI), 90 * mult - 70.0 * mult * cos(80.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(80.0 / 180.0 * PI), TFT_WHITE);
    
    bank.fillTriangle(40 * mult, 40 * mult, 35 * mult, 31 * mult, 31 * mult, 35 * mult, TFT_WHITE);
    
    bank.drawLine(90 * mult + 90.0 * mult * cos(30.0 / 180.0 * PI), 90 * mult - 90.0 * mult * sin(30.0 / 180.0 * PI), 90 * mult + 70.0 * mult * cos(30.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(30.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 * mult + 90.0 * mult * cos(60.0 / 180.0 * PI), 90 * mult - 90.0 * mult * sin(60.0 / 180.0 * PI), 90 * mult + 70.0 * mult * cos(60.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(60.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 * mult + 80.0 * mult * cos(70.0 / 180.0 * PI), 90 * mult - 80.0 * mult * sin(70.0 / 180.0 * PI), 90 * mult + 70.0 * mult * cos(70.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(70.0 / 180.0 * PI), TFT_WHITE);
    bank.drawLine(90 * mult + 80.0 * mult * cos(80.0 / 180.0 * PI), 90 * mult - 80.0 * mult * sin(80.0 / 180.0 * PI), 90 * mult + 70.0 * mult * cos(80.0 / 180.0 * PI), 90 * mult - 70.0 * mult * sin(80.0 / 180.0 * PI), TFT_WHITE);
    
    bank.fillTriangle(139 * mult, 40 * mult, 144 * mult, 31 * mult, 148 * mult, 35 * mult, TFT_WHITE);

    bank.setPivot(90 * mult, 90 * mult);
    pitch.setPivot(70 * mult, 70 * mult);

    bank.pushRotateZoom(90 * mult, 90 * mult, BankDegrees, 1, 1);

    canvas.drawTriangle(90 * mult, 21 * mult, 85 * mult, 30 * mult, 95 * mult, 30 * mult, TFT_ORANGE);
    canvas.drawCircle(90 * mult, 90 * mult, 5 * mult, TFT_RED);
    canvas.drawFastHLine(30 * mult, 90 * mult, 50 * mult, TFT_ORANGE);
    canvas.drawFastHLine(100 * mult, 90 * mult, 50 * mult, TFT_ORANGE);
    canvas.drawFastVLine(80 * mult, 90 * mult, 5 * mult, TFT_ORANGE);
    canvas.drawFastVLine(100 * mult, 90 * mult, 5 * mult, TFT_ORANGE);
    canvas.drawCircle(90 * mult, 90 * mult, 70 * mult, TFT_BLACK);

    canvas.pushSprite(0, 0);
} 

void onMqttMessage(const espMqttClientTypes::MessageProperties& properties, const char* topic, const uint8_t* payload, size_t len, size_t index, size_t total) {
    //Serial.println("Publish received:");
    //Serial.printf("  topic: %s\n  payload:", topic);

    char* strval = new char[len + 1];

    memcpy(strval, payload, len);
    strval[len] = '\0';
    
    if (strcmp(topic, mqtt_PitchDegrees_topic) == 0)
    {
        float pitchDegreesNew = - atof((char*)strval); // invert !
        // round to 1 digit
        pitchDegreesNew = float(long(pitchDegreesNew * 10)) / 10.0;

        //float pitchDegreesNew = round(atof((char*)strval));

        //Serial.printf("pitch: %f\n", pitchDegreesNew);

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

        //Serial.printf("bank: %f\n", bankDegreesNew);

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