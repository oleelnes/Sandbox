using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    void Start()
    {
        maxHP = getMaxHealthWithMultiplier();
        currentHP = maxHP;
    }
}
