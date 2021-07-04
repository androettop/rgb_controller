
int pinR = 5;
int pinG = 6;
int pinB = 3;

String lastCommand = "";

int valueR = 0;
int valueG = 0;
int valueB = 0;

int targetR= 0;
int targetG = 0;
int targetB = 0;

int effect = 0;

// Breath
bool breathIn = true;
double breathStepSize = 0.1;
double breathStep = 0.1;
double pi = 3.1416;

// Rainbow
int rainbowStep = 0;
int colorDelay = 1000;
int colorDelayCount = 0;


void setup() {
  pinMode(pinR, OUTPUT);
  pinMode(pinG, OUTPUT);
  pinMode(pinB, OUTPUT);

  Serial.begin(9600);
}

void setTarget(){
  String color = lastCommand;
  if(color.length() >= 9){
    targetR = color.substring(0,3).toInt();
    targetG = color.substring(3,6).toInt();
    targetB = color.substring(6,9).toInt();
  }
}

void animateBreath(){
  double multiplicator = (sin(breathStep)/2+0.5);
  if (multiplicator >= 1){
    multiplicator = 1;
    breathIn = !breathIn;
  }
  if (multiplicator <= 0){
    multiplicator = 1;
    breathIn = !breathIn;
  }
  valueR = (int) (((double)targetR)*multiplicator);
  valueG = (int) (((double)targetG)*multiplicator);
  valueB = (int) (((double)targetB)*multiplicator);
  
  Serial.println(multiplicator);

  if (breathIn){
    breathStep += breathStepSize;
  } else{
    breathStep -= breathStepSize;
  }
  
  delay(80);
}

void animateRainbow(){
  //set color
  if(colorDelayCount == 0){
    targetR = 0;
    targetG = 0;
    targetB = 0;
    switch (rainbowStep){
      case 0:
        targetR = 255;
        break;
      case 1:
        targetG = 255;
        break;
      case 2:
        targetB = 255;
        break;
    }
  }
  colorDelayCount += 10;
  if(colorDelayCount >= colorDelay){
    colorDelayCount = 0;
    rainbowStep = (rainbowStep+1) % 3;
  }  
  animateFixed();
}


void animateFixed(){
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
  
}

void animate(){
  switch(effect){
    case 0:
      animateFixed();
      break;
    case 1:
      animateBreath();
      break;
    case 2:
      animateRainbow();
      break;
  }
}

void setEffect(){
  if( lastCommand.length() >= 1 && lastCommand.length() <= 2){
    effect = lastCommand.toInt();
  }
}

void displayColors(){
  analogWrite(pinR,valueR);
  analogWrite(pinG,valueG);
  analogWrite(pinB,valueB);
  delay(4);
}

void loop() {
  displayColors();
  
  if (Serial.available() > 0) {
    lastCommand = Serial.readString();
  }
   
  setTarget();
  setEffect();
  animate();
}
