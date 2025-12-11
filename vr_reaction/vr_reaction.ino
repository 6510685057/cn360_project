// int buttonPin = 2;
// int greenLED = 8;
// int redLED = 9;

// void setup() {
//   pinMode(buttonPin, INPUT_PULLUP);
//   pinMode(greenLED, OUTPUT);
//   pinMode(redLED, OUTPUT);

//   Serial.begin(9600);
// }

// void loop() {
//   if (digitalRead(buttonPin) == LOW) {   // กดปุ่ม
//     Serial.println("PRESSED");           // ส่งไป Unity
//     delay(200);                          // กันเด้ง
//   }

//   // เช็คว่ามีคำสั่งจาก Unity หรือไม่
//   if (Serial.available()) {
//     String cmd = Serial.readStringUntil('\n');

//     if (cmd == "HIT") {
//       digitalWrite(greenLED, HIGH);
//       digitalWrite(redLED, LOW);
//       delay(150);
//       digitalWrite(greenLED, LOW);
//     }

//     if (cmd == "MISS") {
//       digitalWrite(redLED, HIGH);
//       digitalWrite(greenLED, LOW);
//       delay(250);
//       digitalWrite(redLED, LOW);
//     }
//   }
// }


int greenLED = 8;
int redLED   = 9;
int buzzer   = 10;

void setup() {
  pinMode(greenLED, OUTPUT);
  pinMode(redLED, OUTPUT);
  pinMode(buzzer, OUTPUT);

  Serial.begin(9600);
}

void loop() {
  if (Serial.available()) {
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();   // ตัด \r \n ทิ้งถ้ามี

    // ===== HIT =====
    if (cmd == "HIT") {
      digitalWrite(greenLED, HIGH);
      digitalWrite(redLED, LOW);

      tone(buzzer, 2000);   // เสียงสูง สั้น
      delay(150);
      noTone(buzzer);

      digitalWrite(greenLED, LOW);
    }

    // ===== MISS =====
    if (cmd == "MISS") {
      digitalWrite(redLED, HIGH);
      digitalWrite(greenLED, LOW);

      tone(buzzer, 500);    // เสียงต่ำ นานกว่า
      delay(300);
      noTone(buzzer);

      digitalWrite(redLED, LOW);
    }

    // ===== PRESSURE MODE =====
    if (cmd == "PRESSURE") {
      // ไฟแดงกระพริบเร็ว ๆ + บี๊บถี่ สร้างความกดดัน
      for (int i = 0; i < 4; i++) {
        digitalWrite(redLED, HIGH);
        tone(buzzer, 800);
        delay(120);

        digitalWrite(redLED, LOW);
        noTone(buzzer);
        delay(80);
      }
    }
  }
}
