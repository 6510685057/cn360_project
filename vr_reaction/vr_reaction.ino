int buttonPin = 2;
int greenLED = 8;
int redLED = 9;

void setup() {
  pinMode(buttonPin, INPUT_PULLUP);
  pinMode(greenLED, OUTPUT);
  pinMode(redLED, OUTPUT);

  Serial.begin(9600);
}

void loop() {
  if (digitalRead(buttonPin) == LOW) {   // กดปุ่ม
    Serial.println("PRESSED");           // ส่งไป Unity
    delay(200);                          // กันเด้ง
  }

  // เช็คว่ามีคำสั่งจาก Unity หรือไม่
  if (Serial.available()) {
    String cmd = Serial.readStringUntil('\n');

    if (cmd == "HIT") {
      digitalWrite(greenLED, HIGH);
      digitalWrite(redLED, LOW);
      delay(150);
      digitalWrite(greenLED, LOW);
    }

    if (cmd == "MISS") {
      digitalWrite(redLED, HIGH);
      digitalWrite(greenLED, LOW);
      delay(250);
      digitalWrite(redLED, LOW);
    }
  }
}

