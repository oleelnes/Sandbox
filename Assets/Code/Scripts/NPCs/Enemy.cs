using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy stats")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("Attack player")]
    public float triggerDistance;
    public float movementSpeed;
    private Transform playerPos;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    private void death()
    {
        Destroy(gameObject);
    }

    public void receiveDamage(float damage)
    {
        currentHP -= damage;
    }
/*    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Melee")
        {
            receiveDamage();
        }
    }*/
}
