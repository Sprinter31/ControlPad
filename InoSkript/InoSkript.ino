bool shouldBlink = false;
bool isLedOn = false;

void setup() 
{
  pinMode(13, OUTPUT);
  Serial.begin(9600);
}

void loop() 
{
  int delayTime = 500;

  if (Serial.available() > 0) 
  {
    String input = Serial.readStringUntil('\n');
    input.trim();

    if (input.indexOf("LED_ON") != -1) // input.Contains()
    {
      shouldBlink = true;

      int sepIndex = input.indexOf(':');
      if (sepIndex != -1) 
      {
        String numberPart = input.substring(sepIndex + 1);
        delayTime = numberPart.toInt();
      }
    } 
    else if (input == "LED_OFF") 
    {
      shouldBlink = false;
    }
  }

  if(shouldBlink)
  {
    if(isLedOn)
    {
      digitalWrite(13, LOW);      
      isLedOn = false;
    }
    else
    {
      digitalWrite(13, HIGH);
      isLedOn = true;
    }
  }
  delay(delayTime);
}
