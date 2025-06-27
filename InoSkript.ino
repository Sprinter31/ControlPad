int relaisPin = 13;

void setup() 
{
    pinMode(relaisPin, OUTPUT);
}

void loop() 
{
    digitalWrite(relaisPin, HIGH);
    delay(200);
    digitalWrite(relaisPin, LOW);
    delay(100); 
}
