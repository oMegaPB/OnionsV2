#include <ESP8266WiFi.h>
#include <espnow.h>

unsigned long lastTime = 0;
unsigned long timerDelay = 10;
const int delta = 40000;
int r2,r3;
uint8_t MAC_2[] = {0x68,0xC6,0x3A,0xF8,0x8B,0xC5};
typedef struct message{
  int id = 0;
  bool status;
  bool svetik;
  bool prolet;
} message;
message rData;
message circle1, circle2;
message track[2] = {circle1, circle2};

void SerialWriteInt(int val) {
  char str[10];
  sprintf(str, "%d", val);
  Serial.write(str);
}

void OnDataRecv(uint8_t *mac, uint8_t *data, uint8_t len) {
  memcpy(&rData, data, sizeof(rData));
  
  track[rData.id-1].id = rData.id;
  track[rData.id-1].status = rData.status;
  track[rData.id-1].svetik = rData.svetik;
  track[rData.id-1].prolet = rData.prolet;
}

void getCircles() {
  String packet = "";
  for (int i = 0; i < 2; i++) {
      SerialWriteInt(track[i].id);
      Serial.write(":");
      if (track[i].status==1) {
        Serial.write("on");
      }
      else {
        Serial.write("off");
      }
      Serial.write(";");
    }
}


void setup() {
  track[0].id = 1;
  track[1].id = 2;
  Serial.begin(115200);
  WiFi.mode(WIFI_STA);
  if (esp_now_init() != 0) {
    Serial.println("52");
    return;
  }
  esp_now_set_self_role(ESP_NOW_ROLE_COMBO);
  esp_now_register_recv_cb(OnDataRecv);
  esp_now_add_peer(MAC_2, ESP_NOW_ROLE_COMBO, 1, NULL, 0);
}


void loop() {
  if ((millis() - lastTime) > timerDelay){
    for (int i = 0; i < 2; i++) {
      if (track[i].prolet == 1) {
        SerialWriteInt(track[i].id);
        Serial.write(':');
        Serial.write("prolet");
      }
    }
    if (Serial.available() > 0) {
      String str = "";
      while (Serial.available()) {
        char c = Serial.read();
        str += String(c);
      }
      if (str == "hello") {
        Serial.write("hi");
        return;
      }
      if (str == "getCircles") {
        getCircles();
        return;
      }
      String num;
      num = str[0];
      int n = num.toInt();
      if (str.endsWith("on")) {
        track[n-1].status = 1;
      }
      if (str.endsWith("off")) {
        track[n-1].status = 0;
      }
      if (str.endsWith("svet")) {
        track[n-1].svetik = 1;
      }
    }
    esp_now_send(MAC_2, (uint8_t *)&track[0], sizeof(track[0]));
    lastTime = millis();
  }
  delay(100);
}
