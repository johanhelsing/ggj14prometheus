﻿using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	public float cameraSpeed = 10F;
	public float cameraOffsetRatio = 0.3F;
	public float directionChangeOffsetRatio = 0.4F;
	Transform target;
	int currentDirection = 1;
	Vector2 goalPosition;

	void Start(){
		Messenger.AddListener<GameObject>(Events.PlayerSpawned, OnPlayerSpawned);
		goalPosition = transform.position.To2D ();
		if (target == null) {
			target = FindObjectOfType<Player>().transform;
		}
	}

	void Update(){
		var curPos = transform.position;
		var targetPos = target.transform.position;

		var cameraWidth = camera.orthographicSize * camera.aspect;
		var cameraOffsetX =  cameraWidth * cameraOffsetRatio * currentDirection * -1;
		var cameraOffset = cameraOffsetX*Vector3.right;

		var dirChangeOffsetX =  cameraWidth * directionChangeOffsetRatio * currentDirection * -1;
		var dirChangeOffset = cameraOffsetX*Vector3.right;

		var targetOffsetX = targetPos.x - curPos.x;

		//change direction if needed
		if (currentDirection > 0) {
			if(targetOffsetX<dirChangeOffsetX){
				Debug.Log("Changing to left");
				currentDirection = -1;
			}
		} else {
			if(targetOffsetX>dirChangeOffsetX){
				Debug.Log("Changing to right");
				currentDirection = 1;
			}
		}

		var newYPos = target.transform.position.y;
		if (currentDirection > 0) {
			if(target.position.x > curPos.x + cameraOffsetX){
				var newXPos = target.transform.position.x - cameraOffsetX;
				goalPosition = new Vector3(newXPos, newYPos, curPos.z); 
			}
		} else {
			if(target.position.x < curPos.x + cameraOffsetX){
				var newXPos = target.transform.position.x - cameraOffsetX;
				goalPosition = new Vector3(newXPos, newYPos, curPos.z); 
			}
		}

		//move towards goalpos
		var offset = goalPosition.To3D (curPos.z) - curPos;
		transform.position = curPos + offset * cameraSpeed * Time.deltaTime;
	}

	void OnPlayerSpawned(GameObject player){
		this.target = player.transform;
		goalPosition = target.transform.position;
	}

}
