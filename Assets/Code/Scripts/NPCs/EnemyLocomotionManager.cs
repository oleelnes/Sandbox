using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyLocomotionManager : MonoBehaviour
{

    EnemyAnimationManager enemyAnimationManager;
    public Rigidbody enemyRigidBody;
    public CharacterStats currentTarget;

    [Header("Detection")]
    //The higher (max) and lower (min) respectively these angles are the greater dectection field of view
    public float maximumDetectionAngle = 70f;
    public float minimumDetectionAngle = -70f;
    public float distance;
    public float triggerDistance = 20f;

    public float stoppingDistance = 2f;
    public float passiveDistance = 30f;
    public float movementSpeed = 3f;
    public float rotationSpeed = 3;


    private void Awake()
    {
        /* enemyManager = GetComponent<EnemyManager>();*/
        enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Enemy falls down (!)
        enemyRigidBody.isKinematic = false;
    }

    private void Update()
    {
        //calculate distance between enemy and player
        distance = Vector3.Distance(transform.position, Player.instance.transform.position);
    }

    public void HandleDetection()
    {
        //NOTE: should be currentTarget if target is not only player
        if (distance <= triggerDistance)
        {
            Vector3 targetDirection = Player.instance.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
            {
                setCurrentTargetToPlayer();
            }
        }

    }

    public void HandleMoveToTarget()
    {

        enemyRotationAndDirection();

        if (currentTarget != null)
        {
            if (distance > stoppingDistance)
            {
                Chase();
            }
            else if (distance <= stoppingDistance)
            {
                Attack();
            }

            //Return to idle state
            if (distance >= passiveDistance)
            {
                Idle();
            }
        }
    }

    private void Idle()
    {
        //IDLE CODE HERE
        setState("Chase State", false);
        currentTarget = null;

    }

    private void Chase()
    {
        //CHASE CODE HERE
        setState("Attack State", false);
        setState("Chase State", true);
    }

    private void Attack()
    {
        //ATTACK CODE HERE
        setState("Attack State", true);
    }

    public void setState(string state, bool active)
    {
        enemyAnimationManager.animator.SetBool(state, active);
    }

    public bool getState(string state)
    {
       return enemyAnimationManager.animator.GetBool(state);
    }

    public void playAudio(UnityEvent stateEvent)
    {
        stateEvent.Invoke();
    }

    public void enemyRotationAndDirection()
    {
        //only rotate on x and z axis
        var targetPos = Player.instance.transform.position;
        targetPos.y = transform.position.y;
        var relativePos = targetPos - transform.position;

        var rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;

        if(distance > stoppingDistance)
        {
            //Look at player and move towards player
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
        }

        //Stay on ground
        // Create RaycastHit variable.
        RaycastHit hit;

        // If the ray casted from this object (in your case, the tree) to below it hits something...
        if ((Physics.Raycast(transform.position, -Vector3.up, out hit, 10f)))
        {

            // and if the distance between object and hit is larger than 0.3 (I judge it nearly unnoticeable otherwise)
            if (hit.distance > 0.3f)
            {
                // Then bring object down by distance value.
                transform.position = new Vector3(transform.position.x, transform.position.y - hit.distance, transform.position.z);

            }

        }
    }

    public void setCurrentTargetToPlayer()
    {
        currentTarget = Player.instance.GetComponent<CharacterStats>();
    }

    public void attackDamage(float damage)
    {
        if(getState("Attack State"))
        Player.instance.GetComponent<PlayerHealth>().TakeDamage(damage);
    }


}