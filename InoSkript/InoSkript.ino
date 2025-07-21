const int slider1Pin = A0;
const int slider2Pin = A1;

const int switch1Pin = 10;
const int switch2Pin = 11;

void setup() {
  pinMode(switch1Pin, INPUT_PULLUP);
  pinMode(switch2Pin, INPUT_PULLUP);
  
  Serial.begin(115200);
}

void loop() {
  int slider1Value = analogRead(slider1Pin); // 0–1023
  int slider2Value = analogRead(slider2Pin);

  int switch1State = !digitalRead(switch1Pin); // 0/1
  int switch2State = !digitalRead(switch2Pin);

  Serial.print(slider1Value);
  Serial.print(",");
  Serial.print(slider2Value);
  Serial.print(",");
  Serial.print(switch1State);
  Serial.print(",");
  Serial.println(switch2State);

  delay(20);
}
