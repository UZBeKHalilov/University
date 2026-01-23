
#include <Arduino.h>

int task3Counter = 0;
const int ledPin = LED_BUILTIN;

// Prototypes
void task1();
void task2();
void task3();
void task4();
void task5();
void task6();
void task7();
void task8();
void task9();
void task10();
void task11();
void task12();
void task13();
void task14();
void task15();

void setup() {
    Serial.begin(9600);       // Start Serial Monitor
    pinMode(ledPin, OUTPUT);
    randomSeed(analogRead(0));
}

void loop() {
    // Existing example tasks (leave other tasks callable individually)
    task1();
    task2();

    task3Counter++;

    // Note: Additional tasks can be called here or invoked via Serial menu (task10)
}

void task1(){  
    Serial.println("Hello Students!");
    delay(1000);
}

void task2(){  
    for (int i = 0; i <= 50; i++)
    {
        Serial.println(i);
        delay(500);
    }
}

void task3(){    
    Serial.print("Task 3 Counter: ");
    Serial.println(task3Counter);

    task3Counter++;

    delay(300);  
}

void task4(){    
    for (int i = 100; i >= 0; i--)
    {
        Serial.print("Countdown: ");
        Serial.println(i);
        delay(200);
    }
}

// Task5 — Display LED Status Messages
void task5(){
    digitalWrite(ledPin, HIGH);
    Serial.println("LED is ON");
    delay(500);
    digitalWrite(ledPin, LOW);
    Serial.println("LED is OFF");
    delay(500);
}

// Task6 — Print a Pattern
void task6(){
    for (int row = 1; row <= 5; row++){
        String line = "";
        for (int j = 0; j < row; j++) line += "*";
        Serial.println(line);
    }
    delay(1000);
}

// Task7 — Random Number Generator
void task7(){
    int r = random(1, 101);
    Serial.print("Random: ");
    Serial.println(r);
    delay(700);
}

// Task8 — Serial Echo Program
void task8(){
    Serial.println("Type something:");
    while (Serial.available() == 0) { /* wait */ }
    String s = Serial.readStringUntil('\n');
    s.trim();
    Serial.print("You typed: ");
    Serial.println(s);
}

// Task9 — LED Control Using Serial Input
void task9(){
    Serial.println("Type 'on' or 'off' to control the LED:");
    while (Serial.available() == 0) { /* wait */ }
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();
    cmd.toLowerCase();
    if (cmd == "on"){
        digitalWrite(ledPin, HIGH);
        Serial.println("LED is now ON");
    } else if (cmd == "off"){
        digitalWrite(ledPin, LOW);
        Serial.println("LED is now OFF");
    } else {
        Serial.println("Unknown command");
    }
}

// Task10 — Create a Serial Menu
void task10(){
    Serial.println("--- Menu ---");
    Serial.println("1 -> Turn LED ON");
    Serial.println("2 -> Turn LED OFF");
    Serial.println("3 -> Print a random number");
    Serial.println("Type choice number:");
    while (Serial.available() == 0) { /* wait */ }
    String choice = Serial.readStringUntil('\n');
    choice.trim();
    if (choice == "1"){
        digitalWrite(ledPin, HIGH);
        Serial.println("LED is now ON");
    } else if (choice == "2"){
        digitalWrite(ledPin, LOW);
        Serial.println("LED is now OFF");
    } else if (choice == "3"){
        Serial.print("Random: ");
        Serial.println(random(1,101));
    } else {
        Serial.println("Invalid choice");
    }
}

// Task11 — Timer Using millis()
void task11(){
    static unsigned long last = 0;
    unsigned long now = millis();
    if (now - last >= 2000){
        Serial.println("2 seconds passed");
        last = now;
    }
}

// Task12 — Print the Alphabet
void task12(){
    for (char c = 'A'; c <= 'Z'; c++){
        Serial.println(c);
        delay(100);
    }
}

// Task13 — Print Two Variables in One Line
void task13(){
    for (int count = 1; count <= 20; count++){
        int r = random(1,101);
        Serial.print("Count: ");
        Serial.print(count);
        Serial.print(" Random: ");
        Serial.println(r);
        delay(400);
    }
}

// Task14 — Display a Loading Animation
void task14(){
    for (int i = 0; i < 3; i++){
        Serial.println("Loading.");
        delay(300);
        Serial.println("Loading..");
        delay(300);
        Serial.println("Loading...");
        delay(300);
    }
}

// Task15 — Password Check via Serial Monitor
void task15(){
    Serial.println("Enter password:");
    while (Serial.available() == 0) { /* wait */ }
    String pw = Serial.readStringUntil('\n');
    pw.trim();
    if (pw == "1234"){
        Serial.println("Access Granted");
    } else {
        Serial.println("Wrong Password");
    }
}