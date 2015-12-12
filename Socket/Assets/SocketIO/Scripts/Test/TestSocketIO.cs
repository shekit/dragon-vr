#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;

public class TestSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;

	public float originalMoveSpeed = 10f;
	public float moveSpeed = 10f;

	public float diveSpeed = 20f;
	public float maxDiveSpeed = 25f;

	public float turnSpeed = 8f;
	public float bankSpeed = 20f;
	public float evenSpeed = 20f;

	public float rollSpeed = 50f;

	private Rigidbody rb;
	public float thrust = 5f;

	public float translateSpeed = 60f;

	private Vector3 euler; 

	private bool upCoroutineStarted = false;
	private bool downCoroutineStarted = false;
	private bool leftCoroutineStarted = false;
	private bool rightCoroutineStarted = false;
	private bool rollCoroutineStarted = false;

	private bool isFlying = true; //change this to false when cardboard is active


	public void Start() 
	{
		euler = transform.eulerAngles;
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		rb = GetComponent<Rigidbody> ();
		Transform tran = transform; //capture transform position to reset z when you stop diving or rising



		////////// REGULAR MOVEMENT USING TRANSFORM.ROTATE   //////////
		/*socket.On ("left", MoveLeftTranslate);
		socket.On ("right", MoveRightTranslate);
		socket.On ("up", MoveUpTranslate);
		socket.On ("down", MoveDownTranslate);*/


		////////// MOVEMENT USING FORCE   //////////
		/*socket.On ("left", MoveLeftForce);
		socket.On ("right", MoveRightForce);
		socket.On ("up", MoveUpForce);
		socket.On ("down", MoveDownForce);
		socket.On ("even", EvenOutForce);*/

		////////// MOVEMENT USING COROUTINES   //////////
		/*socket.On ("left", MoveLeftSmooth);
		socket.On ("right", MoveRightSmooth);
		socket.On ("up", MoveUpSmooth);
		socket.On ("down", MoveDownSmooth);
		socket.On ("even", EvenOutSmooth);*/

		////////// MOVEMENT USING EULER AND COROUTINES  //////////
		socket.On ("left", MoveLeftEuler);
		socket.On ("right", MoveRightEuler);
		socket.On ("up", MoveUpEuler);
		socket.On ("down", MoveDownEuler);
		socket.On ("evenup", EvenUpEuler);
		socket.On ("evendown", EvenDownEuler);
		socket.On ("evenleft", EvenLeftEuler);
		socket.On ("evenright", EvenRightEuler);
		socket.On ("roll", RollEuler);

	}

	///// REGULAR TRANSFORM FUNCTIONS ////
	public void MoveLeftTranslate(SocketIOEvent e){
		Debug.Log ("translate left");
		transform.Rotate (Vector3.up, -translateSpeed * Time.deltaTime);
	}

	public void MoveRightTranslate(SocketIOEvent e){
		Debug.Log ("translate right");
		transform.Rotate (Vector3.up, translateSpeed * Time.deltaTime);
	}

	public void MoveUpTranslate(SocketIOEvent e){
		Debug.Log ("translate up");
		transform.Rotate (Vector3.right, -translateSpeed * Time.deltaTime );
	}

	public void MoveDownTranslate(SocketIOEvent e){
		Debug.Log ("translate down");
		transform.Rotate (Vector3.right, translateSpeed * Time.deltaTime );
	}
	///// END OF REGULAR TRANFORM FUNCTIONS /////


	////// FORCE MOVEMENT FUNCTIONS ////// 

	public void MoveLeftForce(SocketIOEvent e){
		Debug.Log ("force left");
		rb.AddTorque (transform.up * thrust);
	}

	public void MoveRightForce(SocketIOEvent e){
		Debug.Log ("force right");
		rb.AddTorque (transform.up * -thrust);
	}

	public void MoveUpForce(SocketIOEvent e){
		Debug.Log ("force up");
		rb.AddTorque (transform.right * -thrust);
	}

	public void MoveDownForce(SocketIOEvent e){
		Debug.Log ("force down");
		rb.AddTorque (transform.right * thrust);
	}

	public void EvenOutForce(SocketIOEvent e){
		Debug.Log ("even out dragon");

		if (transform.rotation.x > 0) {
			StartCoroutine ("EvenRisingForce");
		}

		if (transform.rotation.x < 0) {
			StartCoroutine ("EvenDivingForce");
		}


	}
	
	private IEnumerator EvenRisingForce(){
		Debug.Log ("evening out rising coroutine");
		while(transform.rotation.x > 0){
			rb.AddTorque (transform.right * thrust * 5);
			yield return null;
		}
	}	

	private IEnumerator EvenDivingForce(){
		Debug.Log ("evening out diving coroutine");
		while(transform.rotation.x > 0){
			rb.AddTorque (transform.right * -thrust * 5);
			yield return null;
		}
	}


	///// END OF FORCE MOVEMENT FUNCTIONS /////



	////// SMOOTH MOVEMENT FUNCTIONS WITH COROUTINES /////

	public void MoveLeftSmooth(SocketIOEvent e){
		Debug.Log ("smooth left");
		StartCoroutine ("LeftSmooth");
	}

	private IEnumerator LeftSmooth(){
		float currentTime = Time.time;
		//rb.constraints = RigidbodyConstraints.FreezePositionX;
		while (Time.time <= currentTime + 1f) {
			transform.Rotate (Vector3.up, -turnSpeed * Time.deltaTime, Space.World);

			yield return null;
		}

	}

	private IEnumerator SmoothLeft(){
				
		while (euler.z > 0) {
			euler.z = (euler.z - bankSpeed * Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
	}

	public void MoveRightSmooth(SocketIOEvent e){
		Debug.Log ("smooth right");
		StartCoroutine ("RightSmooth");
	}
	
	private IEnumerator RightSmooth(){
		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			transform.Rotate (Vector3.up, turnSpeed * Time.deltaTime);
			yield return null;

		}

	}
	

	public void MoveUpSmooth(SocketIOEvent e){
		Debug.Log ("smooth up");
		StartCoroutine ("UpSmooth");
	}
	
	private IEnumerator UpSmooth(){
		Debug.Log ("Smooth up coroutine");
		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			transform.Rotate (Vector3.right, -turnSpeed * Time.deltaTime);
			yield return null;
		}

		//yield return new WaitForSeconds (0.3f);

		//StartCoroutine ("EvenRisingSmooth");
	}

	public void MoveDownSmooth(SocketIOEvent e){
		Debug.Log ("smooth down");
		StartCoroutine ("DownSmooth");
	}
	
	private IEnumerator DownSmooth(){
		Debug.Log ("Smooth down coroutine");
		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			transform.Rotate (Vector3.right, turnSpeed * Time.deltaTime);
			yield return null;
		}
	}

	public void EvenOutSmooth(SocketIOEvent e){
		if (transform.rotation.x > 0) {
			StartCoroutine ("EvenRisingSmooth");
		}
		
		if (transform.rotation.x < 0) {
			StartCoroutine("EvenDivingSmooth");
		}
	}
	
	private IEnumerator EvenRisingSmooth(){
		Debug.Log ("Even the rising up");
		while (transform.rotation.x > 0) {
			transform.Rotate(Vector3.right, turnSpeed * 3 * Time.deltaTime);
			yield return null;
		}
	}
	
	private IEnumerator EvenDivingSmooth(){
		Debug.Log ("Even the diving out");
		while (transform.rotation.x < 0) {
			transform.Rotate(Vector3.right, -turnSpeed * 3 * Time.deltaTime);
			yield return null;
		}
	}

	///// END OF SMOOTH MOVEMENT FUNCTIONS /////

	//// USING EULER ANGLES /////

	// LEFTTTTT
	public void MoveLeftEuler(SocketIOEvent e){
		

		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("LEFT");
			StartCoroutine ("LeftEuler");
		} else {
			Debug.Log ("NO LEFT- ROLL IN PROGRESS");
		}
	}
	
	private IEnumerator LeftEuler(){
		//if left button is pressed while dragon is evening out, cancel the evening out and turn more left
		if (leftCoroutineStarted == true) {
			Debug.Log ("CANCEL LEFT EVENING");
			leftCoroutineStarted = false;
			StopCoroutine("EvenLeftEulerCoroutine");
		}

		// if you switch from left to right suddenly

//		if (rightCoroutineStarted == true) {
//			rightCoroutineStarted = false;
//			StopCoroutine("EvenRightEulerCoroutine");
//		}

		float currentTime = Time.time;
		while (Time.time <= currentTime + 1f) {
			//turning angle
			euler.y = (euler.y - turnSpeed * Time.deltaTime);
			//banking angle
			if(euler.z < 40){
				euler.z = (euler.z + bankSpeed * Time.deltaTime);
			}
			transform.eulerAngles = euler;
			yield return null;
		}
	}

	public void EvenLeftEuler(SocketIOEvent e){
		
		// if roll in progress block other movements

		// if evening out on the right dont call evenleft as it causes it to 
		// jump to zero as while loop condition in evenleftEulerCoroutine is never met
		if (rollCoroutineStarted == false){// && rightCoroutineStarted == false) {
			Debug.Log ("EVEN LEFT");
			StartCoroutine ("EvenLeftEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN LEFT");
		}
	}

	private IEnumerator EvenLeftEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN LEFT");
		leftCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.z > 0) {
			euler.z = (euler.z - bankSpeed * Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
		// properly reset the z
		//euler.z = 0;
		transform.eulerAngles = euler;
		leftCoroutineStarted = false;
	}

	/// RIGHTTTT
	public void MoveRightEuler(SocketIOEvent e){
		


		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("RIGHT");
			StartCoroutine ("RightEuler");
		}
	}
	
	private IEnumerator RightEuler(){
		if (rightCoroutineStarted == true) {
			Debug.Log ("CANCEL RIGHT EVENING");
			rightCoroutineStarted = false;
			StopCoroutine("EvenRightEulerCoroutine");
		}

		//		if (leftCoroutineStarted == true) {
		//			Debug.Log ("Stop left evening coroutine");
		//			leftCoroutineStarted = false;
		//			StopCoroutine("EvenLeftEulerCoroutine");
		//		}

		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			euler.y = (euler.y + turnSpeed * Time.deltaTime);
			if(euler.z > -40){
				euler.z = (euler.z - bankSpeed * Time.deltaTime);
			}
			transform.eulerAngles = euler;
			yield return null;
		}
			
	}

	public void EvenRightEuler(SocketIOEvent e){
		
		// if roll in progress block other movements
		// check if it is evening out on the left side, if it is dont do anything as it caused the z to jump to 0 instantly
		if (rollCoroutineStarted == false){// && leftCoroutineStarted == false) {
			Debug.Log ("EVEN RIGHT");
			StartCoroutine ("EvenRightEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN RIGHT");
		}
	}

	private IEnumerator EvenRightEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN RIGHT");

		rightCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.z < 0) {
			euler.z = (euler.z + bankSpeed * Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}

		// properly reset the z DONT DO THIS - CAUSES WEIRD JUMPS WHEN TRANSITIONING ON SINGLE BUTTON CLCKS
		//euler.z = 0;
		transform.eulerAngles = euler;
		rightCoroutineStarted = false;
	}

	//// UPPPPPP
	public void MoveUpEuler(SocketIOEvent e){
		//Debug.Log ("euler up");
		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("UP");
			StartCoroutine ("UpEuler");
		}
	}
	
	private IEnumerator UpEuler(){
		// if the dragon is leveling out and asked to rise again, cancel the leveling out
		if (upCoroutineStarted == true) {
			Debug.Log ("CANCEL UP COROUTINE");
			upCoroutineStarted = false;
			StopCoroutine("EvenUpEulerCoroutine");
		}

//		if (downCoroutineStarted == true) {;
//			downCoroutineStarted = false;
//			StopCoroutine("EvenDownEulerCoroutine");
//		}


		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			if(euler.x > -70){
				euler.x = (euler.x-turnSpeed*Time.deltaTime);
				transform.eulerAngles = euler;
			}
			yield return null;
		}

	}

	public void EvenUpEuler(SocketIOEvent e){
		
		// if roll in progress block other movements
		if (rollCoroutineStarted == false){// && downCoroutineStarted == false) {
			Debug.Log ("EVEN UP");
			StartCoroutine ("EvenUpEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN UP");
		}
	}

	
	private IEnumerator EvenUpEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN UP");
		//change bool so we know if to cancel it
		upCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.x < 0) {
			
			euler.x = (euler.x + evenSpeed*Time.deltaTime);
			//euler.x = Mathf.LerpAngle(euler.x, 0.0f, Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
		//DONT DO THIS- CAUSES JUMPS WHEN TRANSITIONING FROM UP TO DOWN
		//euler.x = 0;
		transform.eulerAngles = euler;
		//set bool to false once over
		upCoroutineStarted = false;
	}

	///// DOWNNNN
	public void MoveDownEuler(SocketIOEvent e){
		//Debug.Log ("euler down");
		//moveSpeed = moveSpeed*2.0f;
		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("DOWN");
			StartCoroutine ("DownEuler");
		}
	}
	
	private IEnumerator DownEuler(){
		//Debug.Log ("Smooth down euler");
		if (downCoroutineStarted == true) {
			Debug.Log ("DOWN COROUTINE CANCELLED");
			downCoroutineStarted = false;
			StopCoroutine("EvenDownEulerCoroutine");
		}

//		if (upCoroutineStarted == true) {
//			upCoroutineStarted = false;
//			StopCoroutine("EvenUpEulerCoroutine");
//		}

		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			if(euler.x < 70){
				if(moveSpeed < maxDiveSpeed){
					moveSpeed += 0.08f;
				}

				//moveSpeed = Mathf.Lerp(moveSpeed, diveSpeed, Time.deltaTime);
				euler.x = (euler.x+turnSpeed*Time.deltaTime);
				//Debug.Log ("Euler x: "+ euler.x);
				transform.eulerAngles = euler;
			}
			yield return null;
		}

	}

	public void EvenDownEuler(SocketIOEvent e){

		// if roll in progress block other movements
		// if its evening up while rising and down button suddenly pressed, down called even down as it causes a jump to position 0 immediately
		if (rollCoroutineStarted == false){// && upCoroutineStarted == false) {
			Debug.Log ("EVEN DOWN");
			StartCoroutine ("EvenDownEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN DOWN");
		}

	}

	private IEnumerator EvenDownEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN DOWN");
		downCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.x > 0) {
			if(moveSpeed>originalMoveSpeed){
				moveSpeed -= 0.1f;
			}

			//moveSpeed = Mathf.Lerp(diveSpeed, moveSpeed, Time.deltaTime);
			euler.x = (euler.x - evenSpeed*Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}

		//DONT DO THIS- CAUSES JUMPS WHEN TRANSITIONING FROM UP TO DOWN
		//euler.x = 0;
		transform.eulerAngles = euler;
		moveSpeed = originalMoveSpeed;
//		while (moveSpeed > originalMoveSpeed) {
//			moveSpeed = Mathf.Lerp(moveSpeed, originalMoveSpeed, Time.deltaTime);
//			yield return null;
//		}

		downCoroutineStarted = false;
	}

	public void RollEuler(SocketIOEvent e){

		//only let second barrel roll start once one roll is over

		if (rollCoroutineStarted == false) {
			Debug.Log ("ROLL");
			StartCoroutine ("RollEulerCoroutine");
		} else {
			Debug.Log ("NO ROLL");
		}
	}
		
	private IEnumerator RollEulerCoroutine(){
		Debug.Log ("COROUTINE - ROLL");
		rollCoroutineStarted = true;
		float currentTime = Time.time;
		while (euler.z<=360f) {
			euler.z = (euler.z + rollSpeed * Time.deltaTime);
			Debug.Log (euler.z);
			transform.eulerAngles = euler;
			yield return null;
		}
		euler.z = 0;
		transform.eulerAngles = euler;
		Debug.Log (euler.z);
		rollCoroutineStarted = false;
	}
	
	//// END OF USING EULER ANGLES ////

	public void Update(){
		// start/stop flying on button click
//		if (Cardboard.SDK.Triggered) {
//			isFlying = !isFlying;
//		}
//
		if (isFlying) {
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}
	}
	

}
