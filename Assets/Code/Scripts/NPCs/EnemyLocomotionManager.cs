using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{

    EnemyManager enemyManager;
    EnemyAnimationManager enemyAnimationManager;
    NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidBody;

    public CharacterStats currentTarget;
    private GameObject player;


    [Header("Detection")]
    public float triggerDistance = 20f;
    public float distance;
    public float stoppingDistance = 1f;

    public float rotationSpeed = 15f;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        /*navMeshAgent = GetComponentInChildren<NavMeshAgent>();*/
        enemyRigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        /*navMeshAgent.enabled = false;*/
        enemyRigidBody.isKinematic = false;
    }

    public void HandleDetection()
    {
        //should be currentTarget if target is not only player
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= triggerDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
            {
                currentTarget = player.GetComponent<CharacterStats>();
            }
        }

    }

    public void HandleMoveToTarget()
    {
        Debug.Log("HANDLEMOVE");
        Vector3 targetDiretion = currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDiretion, transform.forward);

        enemyAnimationManager.animator.SetBool("Patrol State", true);
        //if we are performing an action, stop our movement!
/*        if (enemyManager.isPerformingAction)
        {
            *//*enemyAnimationManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);*//*
            enemyManager.
            *//*navMeshAgent.enabled = false;*//*

        }
        else
        {
            if(distance > stoppingDistance)
            {
                enemyAnimationManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
            else if (distance <= stoppingDistance)
            {
                enemyAnimationManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
        }
*/
        /*HandleRotateTowardsTarget();*/

/*        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;*/
    }

    public void HandleRotateTowardsTarget()
    {
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
         //Rotate with pathfinding (navmesh)
         else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyRigidBody.velocity;

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(currentTarget.transform.position);
            enemyRigidBody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);

        }

    }
}