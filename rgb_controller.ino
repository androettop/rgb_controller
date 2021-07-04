int pinR = 5;
int pinG = 6;
int pinB = 3;

int valueR = 0;
int valueG = 0;
int valueB = 0;


int targetR= 0;
int targetG = 0;
int targetB = 0;

void setup() {
  pinMode(pinR, OUTPUT);
  pinMode(pinG, OUTPUT);
  pinMode(pinB, OUTPUT);

  Serial.begin(9600);
}

void setTarget(){
  if (Serial.available() > 0) {
    String color = Serial.readString();
    if(color.length() >= 9){
      targetR = color.substring(0,3).toInt();
      targetG = color.substring(3,6).toInt();
      targetB = color.substring(6,9).toInt();
    }
  }
}

void animateTarget(){
  if(valueR < targetR){
    valueR++;
  }else if(valueR > targetR){
    valueR--;  
  }

  if(valueG < targetG){
    valueG++;
  }else if(valueG > targetG){
    valueG--;  
  }

  if(valueB < targetB){
    valueB++;
  }else if(valueB > targetB){
    valueB--;  
  }
  delay(4);
}

void displayColors(){
  analogWrite(pinR,valueR);
  analogWrite(pinG,valueG);
  analogWrite(pinB,valueB);
}

void loop() {
  displayColors();
  setTarget();
  animateTarget();
}
