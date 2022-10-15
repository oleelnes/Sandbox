using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
