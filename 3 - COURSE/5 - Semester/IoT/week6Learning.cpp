void setup() {
  Serial.begin(9600);       // Start Serial Monitor
  pinMode(7, OUTPUT);      // LED pin
  Serial.println("Type 'on' or 'off' to control the LED.");
}

void loop() {
  // Check if any data is available from Serial Monitor
  if (Serial.available() > 0) {
    
    String command = Serial.readString();   // Read the input as a string
    command.trim();                         // Remove extra spaces or new lines

    if (command == "on") {
      digitalWrite(7, HIGH);
      Serial.println("LED is ON");
    }
    else if (command == "off") {
      digitalWrite(7, LOW);
      Serial.println("LED is OFF");
    }
    else {
      Serial.println("Unknown command. Type 'on' or 'off'.");
    }
  }
}
