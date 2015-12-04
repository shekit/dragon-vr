﻿#region License
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

	public float moveSpeed = 10f;
	public float turnSpeed = 10f;
	public float bankSpeed = 5f;
	public float evenSpeed = 5f;
	private Rigidbody rb;
	public float thrust = 5f;

	public float translateSpeed = 60f;

	private Vector3 euler; 

	private bool rising = false;
	private bool diving = false;

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
		Debug.Log ("euler left");
		StartCoroutine ("LeftEuler");
	}
	
	private IEnumerator LeftEuler(){
		float currentTime = Time.time;
		while (Time.time <= currentTime + 1f) {
			euler.y = (euler.y - turnSpeed * Time.deltaTime);
			if(euler.z < 40){
				euler.z = (euler.z + bankSpeed * Time.deltaTime);
			}
			//Debug.Log (euler.z);
			transform.eulerAngles = euler;
			yield return null;
		}
		
		yield return new WaitForSeconds (0.2f);
		
		StartCoroutine ("EvenLeftEuler");

	}
	public void EvenLeftEuler(SocketIOEvent e){
		Debug.Log ("even up");
		StartCoroutine ("EvenLeftEulerCoroutine");
	}

	private IEnumerator EvenLeftEulerCoroutine(){
		Debug.Log ("even right euler coroutine");
		yield return new WaitForSeconds (0.2f);
		while (euler.z > 0) {
			euler.z = (euler.z - bankSpeed * Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
		// properly reset the z
		euler.z = 0;
		transform.eulerAngles = euler;
	}

	/// RIGHTTTT
	public void MoveRightEuler(SocketIOEvent e){
		Debug.Log ("euler right");
		StartCoroutine ("RightEuler");
	}
	
	private IEnumerator RightEuler(){
		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			euler.y = (euler.y + turnSpeed * Time.deltaTime);
			if(euler.z > -40){
				euler.z = (euler.z - bankSpeed * Time.deltaTime);
			}
			//Debug.Log (euler.z);
			transform.eulerAngles = euler;
			yield return null;
		}
		
		yield return new WaitForSeconds (0.2f);
		
		StartCoroutine ("EvenRightEuler");
	}
	public void EvenRightEuler(SocketIOEvent e){
		Debug.Log ("even right");
		StartCoroutine ("EvenRightEulerCoroutine");
	}

	private IEnumerator EvenRightEulerCoroutine(){
		Debug.Log ("even right euler coroutine");
		yield return new WaitForSeconds (0.2f);
		while (euler.z < 0) {
			euler.z = (euler.z + bankSpeed * Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}

		// properly reset the z
		euler.z = 0;
		transform.eulerAngles = euler;
	}

	//// UPPPPPP
	public void MoveUpEuler(SocketIOEvent e){
		Debug.Log ("euler up");
		StartCoroutine ("UpEuler");
	}
	
	private IEnumerator UpEuler(){
		Debug.Log ("Smooth up euler");
		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			if(euler.x > -90){
				euler.x = (euler.x-evenSpeed*Time.deltaTime);
				transform.eulerAngles = euler;
			}
			yield return null;
		}

	}

	public void EvenUpEuler(SocketIOEvent e){
		Debug.Log ("even up");
		StartCoroutine ("EvenUpEulerCoroutine");
	}

	
	private IEnumerator EvenUpEulerCoroutine(){
		Debug.Log ("Even up euler");
		yield return new WaitForSeconds (0.2f);
		while (euler.x < 0) {
			euler.x = (euler.x + evenSpeed*Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
		euler.x = 0;
		transform.eulerAngles = euler;
	}

	///// DOWNNNN
	public void MoveDownEuler(SocketIOEvent e){
		Debug.Log ("euler down");
		StartCoroutine ("DownEuler");
	}
	
	private IEnumerator DownEuler(){
		Debug.Log ("Smooth down euler");
		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			if(euler.x < 90){
				euler.x = (euler.x+evenSpeed*Time.deltaTime);
				transform.eulerAngles = euler;
				yield return null;
			}
		}

		yield return new WaitForSeconds (0.2f);
		
		StartCoroutine ("EvenDownEuler");

	}

	public void EvenDownEuler(SocketIOEvent e){
		Debug.Log ("even down");
		StartCoroutine ("EvenDownEulerCoroutine");
	}

	private IEnumerator EvenDownEulerCoroutine(){
		Debug.Log ("Even down euler");
		yield return new WaitForSeconds (0.2f);
		while (euler.x > 0) {
			euler.x = (euler.x - evenSpeed*Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
		euler.x = 0;
		transform.eulerAngles = euler;
	}
	
	//// END OF USING EULER ANGLES ////

	public void Update(){
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);

	}
	

}
