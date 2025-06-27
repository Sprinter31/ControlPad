bool shouldBlink = false;
bool isLedOn = false;
int delayTime = 500;

void setup() 
{
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.begin(9600);
}

void loop() 
{
  if (Serial.available() > 0) 
  {
    String input = Serial.readStringUntil('\n');
    input.trim();
    
    if (input.indexOf("LED_ON") != -1) // input.Contains()
    {
      shouldBlink = true;
      Serial.println("inputOn: " + input);
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
      digitalWrite(LED_BUILTIN, LOW);      
      isLedOn = false;
    }
    else
    {
      digitalWrite(LED_BUILTIN, HIGH);
      isLedOn = true;
    }
  } 

  Serial.println(delayTime);
  delay(delayTime);
}
