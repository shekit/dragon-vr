#include <Process.h>    // include the Process class of the Bridge lib


// URLS FOR PARAMETERS



String upUrl = "http://128.122.6.128:3000/up/"; 

String downUrl = "http://128.122.6.128:3000/down/"; 

String leftUrl = "http://128.122.6.128:3000/left/"; //CHECK UR IP

String rightUrl = "http://128.122.6.128:3000/right/"; 

String evenUpUrl = "http://128.122.6.128:3000/even-up/";

String evenDownUrl = "http://128.122.6.128:3000/even-down/";

String evenRightUrl = "http://128.122.6.128:3000/even-right/";

String evenLeftUrl = "http://128.122.6.128:3000/even-left/";



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
    prevUpButtonState = HIGH;
    //Serial.println("button is high");
    up.begin("curl");
    up.addParameter(upUrl);
    up.run();
  }
  
  if(upButtonState == LOW && prevUpButtonState == HIGH){
     prevUpButtonState = LOW;
     //Serial.println("changed to low");
     evenUp.begin("curl");
     evenUp.addParameter(evenUpUrl);
     evenUp.run(); 
  }
  
  if(downButtonState == HIGH){
     prevDownButtonState = HIGH;
     down.begin("curl");
     down.addParameter(downUrl);
     down.run();     
  }
  
  if(downButtonState == LOW && prevDownButtonState == HIGH){
     prevDownButtonState = LOW;
     evenDown.begin("curl");
     evenDown.addParameter(evenDownUrl);
     evenDown.run(); 
  }
  
  if(leftButtonState == HIGH){
     p2.begin("curl");
     p2.addParameter(leftUrl);
     p2.run(); 
  }
  
  if(rightButtonState == HIGH){
     p3.begin("curl");
     p3.addParameter(rightUrl);
     p3.run(); 
  }
  //READING AND CREATING FONT URL
//  fontPinVal = analogRead(fontPin);
//  
//  //fontVal = map(fontPinVal, 0 , 1023, 0, 10);
//  
//  newFontUrl = fontUrl + fontPinVal;
//  
//  if(fontPinVal != prevFontVal){
//     //Process p2 for font
//    p.begin("curl");
//    p.addParameter(newFontUrl);
//    p.run();
//    
//    prevFontVal = fontPinVal;
//  }
//  
//  
//  //READING AND CREATING SIZE URL
//  sizePinVal = analogRead(sizePin);
//  
//  //sizeVal = map(sizePinVal, 0, 1023, 0 , 10);
//  
//  newSizeUrl = sizeUrl + sizePinVal;
//  
//  if(sizePinVal != prevSizeVal){
//    //Process p for size;
//    p1.begin("curl");                    
//    p1.addParameter(newSizeUrl); 
//    p1.run();   
//    
//    prevSizeVal = sizePinVal;
//  }
//  
//  
//  //READING AND CREATING MOVE URL
//  movePinVal = analogRead(movePin);
//  
//  //moveVal = map(movePinVal, 0 , 1023, 0, 500);
//  
//  newMoveUrl = moveUrl + movePinVal;
//  
//  if(movePinVal != prevMoveVal){
//     //Process p1 for move;
//    p2.begin("curl");
//    p2.addParameter(newMoveUrl);
//    p2.run();
//    
//    prevMoveVal = movePinVal;
//  }
  

  delay(20);
 

}

