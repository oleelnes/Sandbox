using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    EnemyLocomotionManager enemyLocomotionManager;
    EnemyStats enemyStats;

    private void Awake()
    {

        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyLocomotionManager.enemyRigidBody.velocity = Vector3.zero;
        enemyStats.handleHealth();

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

    public void receiveDamage(float damage)
    {
        enemyStats.currentHP -= damage;
    }

}
