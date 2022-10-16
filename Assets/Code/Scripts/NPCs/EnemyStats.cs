using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    [Header("Enemy stats")]
    public float maxHP = 100f;
    public float currentHP = 100f;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = SetMaxHealthInFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthInFromHealthLevel()
    {
        return 10 * healthLevel;
    }
}
