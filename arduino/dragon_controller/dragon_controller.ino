#include <Process.h>    // include the Process class of the Bridge lib


// URLS FOR PARAMETERS

String ipAdd = "http://128.122.6.245:3000/";

String upUrl = ipAdd+"up/"; 

String downUrl = ipAdd+"down/"; 

String leftUrl = ipAdd+"left/";

String rightUrl = ipAdd+"right/"; 

String evenUpUrl = ipAdd+"even-up/";

String evenDownUrl = ipAdd+"even-down/";

String evenLeftUrl = ipAdd+"even-left/";

String evenRightUrl = ipAdd+"even-right/";

String rollUrl = ipAdd+"barrel-roll/";

// PROCESS VARIABLES
Process up;
Process down;
Process left;
Process right;
Process evenUp;
Process evenDown;
Process evenLeft;
Process evenRight;
Process roll;

// BUTTONS
int rollButton = 6;
int rollButtonState = 0;
int prevRollButtonState = 0;

int upButton = 5;
int upButtonState = 0;
int prevUpButtonState = 0;

int downButton = 4;
int downButtonState = 0;
int prevDownButtonState = 0;

int leftButton = 3;
int leftButtonState = 0;
int prevLeftButtonState = 0;

int rightButton = 2;
int rightButtonState = 0;
int prevRightButtonState = 0;


void setup() {
  Bridge.begin();      // Initialize Bridge
  Serial.begin(9600);  // Initialize Serial
  
  pinMode(upButton, INPUT);
  pinMode(downButton, INPUT);
  pinMode(leftButton, INPUT);
  pinMode(rightButton, INPUT);
}

void loop() {
  
  upButtonState = digitalRead(upButton);
  downButtonState = digitalRead(downButton);
  leftButtonState = digitalRead(leftButton);
  rightButtonState = digitalRead(rightButton);
  rollButtonState = digitalRead(rollButton);
  
  
  if(rollButtonState == HIGH){
     prevRollButtonState = HIGH;
     roll.begin("curl");
     roll.addParameter(rollUrl);
     roll.run();
  }
  
  if(upButtonState == HIGH){
    //Serial.println("up");
    prevUpButtonState = HIGH;
    up.begin("curl");
    up.addParameter(upUrl);
    up.run();
  }
  
  if(upButtonState == LOW && prevUpButtonState == HIGH){
     //Serial.println("up button released");
     prevUpButtonState = LOW;
     evenUp.begin("curl");
     evenUp.addParameter(evenUpUrl);
     evenUp.run(); 
  }
  
  if(downButtonState == HIGH){
     //Serial.println("down");
     prevDownButtonState = HIGH;
     down.begin("curl");
     down.addParameter(downUrl);
     down.run();
//     up.begin("curl");
//     up.addParameter(upDownUrl+"down");
//     up.run();
  }
  
  if(downButtonState == LOW && prevDownButtonState == HIGH){
     //Serial.println("down button released");
     prevDownButtonState = LOW;
     
     evenDown.begin("curl");
     evenDown.addParameter(evenDownUrl);
     evenDown.run(); 
  }
  
  if(leftButtonState == HIGH){
     //Serial.println("left");
     prevLeftButtonState = HIGH;
     left.begin("curl");
     left.addParameter(leftUrl);
     left.run(); 
  }
  
  if(leftButtonState == LOW && prevLeftButtonState == HIGH){
    //Serial.println("left button released");
     prevLeftButtonState = LOW;
     evenLeft.begin("curl");
     evenLeft.addParameter(evenLeftUrl);
     evenLeft.run(); 
  }
  
  if(rightButtonState == HIGH){
    //Serial.println("right");
     prevRightButtonState = HIGH;
     right.begin("curl");
     right.addParameter(rightUrl);
     right.run(); 
  }
  
  if(rightButtonState == LOW && prevRightButtonState == HIGH){
    //Serial.println("right button released");
     prevRightButtonState = LOW;
     evenRight.begin("curl");
     evenRight.addParameter(evenRightUrl);
     evenRight.run(); 
  }
  
  

 
  delay(20);
 

}

