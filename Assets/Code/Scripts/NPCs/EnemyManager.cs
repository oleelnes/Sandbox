using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    /*    [Header("Enemy stats")]
        public float maxHP = 100f;
        public float currentHP;

        [Header("Attack player")]
        public float triggerDistance;
        public float movementSpeed;

        private float distance;

        [Header("Patroling")]
        public T
    ransform[] waypoints;
        int waypoints_index = 0;
        public float waypointDistance;
        public float patrolSpeed;
        public float speed;
        private float time;
        private float timeStore;
    */

    EnemyLocomotionManager enemyLocomotionManager;
    public bool isPerformingAction;

    [Header("AI Settings")]
    public float detectionRadius = 20f;

    //The higher and lower respectively these angles are the greater dectection field of view
    public float maximumDetectionAngle = 50f;
    public float minimumDetectionAngle = -50f;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        /*        Patroling();

                //chase player
                distance = Vector3.Distance(transform.position, playerPos.position);
                if (distance <= triggerDistance)
                {
                    transform.LookAt(playerPos);
                    transform.position = Vector3.MoveTowards(transform.position, playerPos.position, movementSpeed * Time.deltaTime);
                }

                //Destroy enemy when health is zero
                if(currentHP < 0)
                {
                    currentHP = 0;
                }
                if(currentHP == 0)
                {
                    death();
                }*/

    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (enemyLocomotionManager.currentTarget == null)
        {
            enemyLocomotionManager.HandleDetection();
        }
        else
        {
            enemyLocomotionManager.HandleMoveToTarget();
        }
    }

/*    private void death()
    {
        Destroy(gameObject);
    }

    public void receiveDamage(float damage)
    {
        currentHP -= damage;
    }*/
/*
    private void Patroling()
    {
        waypointDistance = Vector3.Distance(transform.position, waypoints[waypoints_index].position);
        if (waypointDistance > 8f)
        {
            transform.LookAt(waypoints[waypoints_index].position);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypoints_index].position, patrolSpeed * Time.deltaTime);
        }
        else
        {
            waypoints_index = (waypoints_index + 1) % waypoints.Length;
        }
    }*/
}
