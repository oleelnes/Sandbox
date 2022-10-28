using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
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
