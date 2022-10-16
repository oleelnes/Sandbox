using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{

    /* EnemyManager enemyManager;*/
    EnemyAnimationManager enemyAnimationManager;
    public Rigidbody enemyRigidBody;
    public CharacterStats currentTarget;

    //The higher and lower respectively these angles are the greater dectection field of view
    [Header("Detection")]
    public float maximumDetectionAngle = 50f;
    public float minimumDetectionAngle = -50f;
    public float distance;
    public float triggerDistance = 20f;

    public float stoppingDistance = 2f;
    public float passiveDistance = 30f;
    public float movementSpeed = 3f;

    private void Awake()
    {
        /* enemyManager = GetComponent<EnemyManager>();*/
        enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {

        //Enemy falls down
        enemyRigidBody.isKinematic = false;
    }

    private void Update()
    {
        //calculate distance between enemy and player
        distance = Vector3.Distance(transform.position, Player.instance.transform.position);
    }

    public void HandleDetection()
    {
       /* enemyAnimationManager.animator.SetBool("Chase State", true);*/
        //NOTE: should be currentTarget if target is not only player
        if (distance <= triggerDistance)
        {
            Vector3 targetDirection = Player.instance.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
            {
                currentTarget = Player.instance.GetComponent<CharacterStats>();
            }
        }

    }

    public void HandleMoveToTarget()
    {

        transform.LookAt(Player.instance.transform.position);

        if (currentTarget != null)
        {
            if (distance > stoppingDistance)
            {
                enemyAnimationManager.animator.SetBool("Attack State", false);
                enemyAnimationManager.animator.SetBool("Chase State", true);
                transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position, movementSpeed * Time.deltaTime);
            }
            else if (distance <= stoppingDistance)
            {
                //ATTACK CODE HERE
                enemyAnimationManager.animator.SetBool("Attack State", true);
            }

            //Return to idle state
            if (distance >= passiveDistance)
            {
                enemyAnimationManager.animator.SetBool("Chase State", false);
                currentTarget = null;
            }
        }
    }
}