#include <Process.h>    // include the Process class of the Bridge lib


// URLS FOR PARAMETERS



//String upUrl = "http://128.122.6.128:3000/up/"; 

//String downUrl = "http://128.122.6.128:3000/down/"; 

String leftUrl = "http://128.122.6.128:3000/left/"; //CHECK UR IP

String rightUrl = "http://128.122.6.128:3000/right/"; 

String evenUpUrl = "http://128.122.6.128:3000/even-up/";

String evenDownUrl = "http://128.122.6.128:3000/even-down/";

String evenRightUrl = "http://128.122.6.128:3000/even-right/";

String evenLeftUrl = "http://128.122.6.128:3000/even-left/";

String upDownUrl = "http://128.122.6.128:3000/move/";

String upUrl = "";

// PROCESS VARIABLES
Process up;
Process down;
Process left;
Process right;
Process evenUp;
Process evenDown;
Process evenLeft;
Process evenRight;

// BUTTONS

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
  
  
  if(upButtonState == HIGH){
    Serial.println(upDownUrl+"up");
    prevUpButtonState = HIGH;
    up.begin("curl");
    up.addParameter(upDownUrl+"up");
    up.run();
  }
  
  if(upButtonState == LOW && prevUpButtonState == HIGH){
     Serial.println("up button released");
     prevUpButtonState = LOW;
//     evenUp.begin("curl");
//     evenUp.addParameter(evenUpUrl);
//     evenUp.run(); 
  }
  
  if(downButtonState == HIGH){
     Serial.println("down");
     prevDownButtonState = HIGH;
//     down.begin("curl");
//     down.addParameter(downUrl);
//     down.run();     
  }
  
  if(downButtonState == LOW && prevDownButtonState == HIGH){
     Serial.println("down button released");
     prevDownButtonState = LOW;
     up.begin("curl");
     up.addParameter(upDownUrl+"down");
     up.run();
//     evenDown.begin("curl");
//     evenDown.addParameter(evenDownUrl);
//     evenDown.run(); 
  }
  
  if(leftButtonState == HIGH){
     Serial.println("left");
     prevLeftButtonState = HIGH;
//     left.begin("curl");
//     left.addParameter(leftUrl);
//     left.run(); 
  }
  
  if(leftButtonState == LOW && prevLeftButtonState == HIGH){
    Serial.println("left button released");
     prevLeftButtonState = LOW;
//     evenLeft.begin("curl");
//     evenLeft.addParameter(evenLeftUrl);
//     evenLeft.run(); 
  }
  
  if(rightButtonState == HIGH){
    Serial.println("right");
     prevRightButtonState = HIGH;
//     right.begin("curl");
//     right.addParameter(rightUrl);
//     right.run(); 
  }
  
  if(rightButtonState == LOW && prevRightButtonState == HIGH){
    Serial.println("right button released");
     prevRightButtonState = LOW;
//     evenRight.begin("curl");
//     evenRight.addParameter(evenRightUrl);
//     evenRight.run(); 
  }
  
  

 
  delay(20);
 

}

