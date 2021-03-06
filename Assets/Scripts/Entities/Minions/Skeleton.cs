﻿using UnityEngine;
using System.Collections;

public class Skeleton : BaseUnit
{

    #region Global Variables
    float distFromPlayer;
    //the distance that persephone can be from skeleton before he moves to follow
    float followDistance;
    float attackRange;
    #endregion

    // Use this for initialization
    void Start()
    {
        state = EntityState.IDLE;
        //Finds player GameObject, sets BaseUnit player to that Object
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<BaseUnit>();
        }
        //set health and moveSpeed
        health = 30; //placeholder value
        moveSpeed = 15f; // higher than player base speed
        followDistance = 4f;//gives distance skeleton is from persephone

        attackRange = 4f;

    }

   /// <summary>
   /// This is the over all update method for the skeleton, contains the state machine and decision making.
   /// </summary>
    void Update()
    {
        //code for death
        if (health <= 0)
        {
            Die();
        }
        
        BaseUnit target = FindTarget();//finds the closest enemy target
        distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        //the distance that persephone can be from skeleton before he moves to follow
        #region IdleState
        if (state == EntityState.IDLE)
        {
            // play idle animation

            //checks if target is not null
            if (target != null)
            {
                state = EntityState.ATTACKING;
            }
            else if (distFromPlayer > followDistance)
            {
                state = EntityState.MOVING;
            }
        }
        #endregion 
        
        #region MovingState
        else if (state == EntityState.MOVING)
        {
            //move to persephone
            Move(player);
            //checks if target is not null
            if (target != null)
            {
                state = EntityState.ATTACKING;
            }
            else if (distFromPlayer <= followDistance)
            {
                state = EntityState.IDLE;
            }
        }
        #endregion

        #region AttackingState
        else if (state == EntityState.ATTACKING)
        {
            float distFromTarget = Vector3.Distance(target.transform.position, transform.position); ;
            if (distFromTarget <= attackRange)
            {
                Attack(target);
            }
            else
            {
                Move(target);
            }

            if (target.health <= 0)
            {//healthOfTarget <= 0
                target = FindTarget();//finds the closest enemy target
                if (target != null)
                {
                    state = EntityState.ATTACKING;
                }
                else
                {
                    state = EntityState.MOVING;
                    Move(player);
                }
            }
        }
        #endregion
    }
    
    /// <summary>
    /// This method finds all the positions of enemies and returns the closest enemy for the skeleon to navigate to.
    /// </summary>
    /// <returns></returns>
    protected BaseUnit FindTarget()
    {
        //finds all objects with tag Enemy and assigns them to a group
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Enemy");

        //iterates through array of enemies
        float closestMinionDist = 21; //max distance of skeleton is 20 feet
        float currentMinionDist = 21;//tracks the distance of target object 
        GameObject closestMinionObj = null;//tracks closest enemy object
        BaseUnit chosenTarget = null;
        foreach (GameObject targetMin in minions)
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
        transform.position = Vector3.MoveTowards(transform.position, targetUnit.transform.position, moveSpeed * Time.deltaTime);
    }

    protected override void Attack(BaseUnit enemy)
    {
        //do Attack animation
        //code for damage dealt and received goes here




    }

    protected override void Die()
    {
        state = EntityState.DYING;
        Destroy(this.gameObject);
        //add code to give will back to persephone

    }
}

//test line please ignore
