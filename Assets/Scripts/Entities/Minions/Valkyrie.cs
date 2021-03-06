﻿using UnityEngine;
using System.Collections;

public class Valkyrie : BaseUnit {
	
	// Use this for initialization
	void Start () {
		state = EntityState.IDLE;
		//Finds player GameObject, sets BaseUnit player to that Object
		GameObject playerObj = GameObject.Find("Player");
		if (playerObj != null)
		{
			player = playerObj.GetComponent<BaseUnit>();
		}
		//set health and moveSpeed
		health = 20; //placeholder value
		moveSpeed = 15f; // faster than player base speed
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//code for death
		if (health <= 0) {
			Die ();
		}
		BaseUnit target = FindTarget ();//finds the closest enemy target
		//gives distance Valkyrie is from persephone
		float distFromPlayer = Vector3.Distance (player.transform.position, transform.position);
		//the distance that persephone can be from Valkyrie before he moves to follow
		float followDistance = 4f;
		float attackRange = 10f; 
		if (state == EntityState.IDLE) {
			
			// play idle animation
			
			//checks if target is not null
			if (target) {
				state = EntityState.ATTACKING;
			} else if (distFromPlayer > followDistance) {
				state = EntityState.MOVING;
			}
		}
		if (state == EntityState.MOVING) {
			//move to persephone
			Move (player);	
			//checks if target is not null
			if (target) {
				state = EntityState.ATTACKING;
			} else if (distFromPlayer <= followDistance) {
				state = EntityState.IDLE;
			}
		}
		if (state == EntityState.ATTACKING) {
			float distFromTarget= Vector3.Distance (target.transform.position, transform.position);;
			if (distFromTarget <= attackRange) {
				Attack (target);
			} else {
				Move (target);
			}
			
			if (target.health <= 0) {
				target = FindTarget ();//finds the closest enemy target
				if (target) {
					state = EntityState.ATTACKING;
				} else {
					state = EntityState.MOVING;
					Move (player);
				}
			}
			
		}
	}
	// this method checks the enemy's surroundings and finds the closest minion
	protected BaseUnit FindTarget()
	{
		//finds all objects with tag Enemy and assigns them to a group
		GameObject[] minions = GameObject.FindGameObjectsWithTag("Enemy");
		
		//iterates through array of enemies
		float closestMinionDist = 21; //max distance of Valkyrie is 20 feet
		float currentMinionDist = 21;//tracks the distance of target object 
		GameObject closestMinionObj = null;//tracks closest enemy object
		BaseUnit chosenTarget = null;
		foreach(GameObject targetMin in minions)
		{
			currentMinionDist = Vector3.Distance(targetMin.transform.position, transform.position);
			if (currentMinionDist < closestMinionDist)
			{
				closestMinionDist = currentMinionDist;
				closestMinionObj = targetMin;
			}
			
		}
		if (closestMinionObj != null)
		{
			chosenTarget = closestMinionObj.GetComponent<BaseUnit>();
		}
		
		return chosenTarget;
		
	}
	protected override void Move(BaseUnit targetUnit)
	{
		transform.position = Vector3.MoveTowards (transform.position, targetUnit.transform.position, moveSpeed * Time.deltaTime);
	}
	
	protected override void Attack(BaseUnit enemy)
	{
		//do Attack animation
		//code for damage dealt and received goes here
		
		
		
	}
	protected override void Die()
	{
		state = EntityState.DYING;
		Destroy (this.gameObject);
		//add code to give will back to persephone
		
	}
}


