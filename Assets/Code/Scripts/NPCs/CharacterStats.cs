using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float HPMultiplier = 1;
    public float maxHP;
    public float currentHP;
    public float movementSpeed = 3f;
    public float damage = 1f;

    public float getMaxHealthWithMultiplier()
    {
        return maxHP * HPMultiplier;
    }
}
