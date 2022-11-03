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
        handleHealth();

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

    private void handleHealth()
    {
        //Destroy enemy when health is zero
        if (enemyStats.currentHP < 0)
        {
            enemyStats.currentHP = 0;
        }
        if (enemyStats.currentHP == 0)
        {
            //ENEMY DEATH CODE HERE
            Destroy(gameObject);
        }
    }

    public void receiveDamage(float damage)
    {
        enemyStats.currentHP -= damage;
    }

    public void attackDamage(float damage)
    {
        enemyLocomotionManager.attackDamage(enemyStats.damage);
    }


}
