const int sliderPins[] = {A1, A2, A3, A4, A5, A6};
const int switchPins[] = {2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

const int numSliders = sizeof(sliderPins) / sizeof(sliderPins[0]);
const int numSwitches = sizeof(switchPins) / sizeof(switchPins[0]);

void setup() {
  for (int i = 0; i < numSwitches; i++) {
    pinMode(switchPins[i], INPUT_PULLUP);
  }
  Serial.begin(115200);
}

void loop() {
  
  for (int i = 0; i < numSliders; i++) {
    int value = analogRead(sliderPins[i]);
    Serial.print(value);
    Serial.print(",");
  }


  for (int i = 0; i < numSwitches; i++) {
    int state = !digitalRead(switchPins[i]);
    Serial.print(state);
    if (i < numSwitches - 1) Serial.print(",");
  }

  Serial.println();
  delay(20);
}