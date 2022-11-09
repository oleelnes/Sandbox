using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;

    public float health, maxHealth;
    void Start()
    {
        //health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Player gets hurt: " + amount);
        health -= amount;
        OnPlayerDamaged?.Invoke();

        if (health <= 0)
        {
            health = 0;
            Debug.Log("You are dead");
            OnPlayerDeath?.Invoke();
        }
    }


}
