#include <Arduino.h>

void setup() {
    Serial.begin(9600);       // Start Serial Monitor
}

void loop() {

    task1();
    task2();

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
    int number;

  
}
