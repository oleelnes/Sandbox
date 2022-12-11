using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
    [Header("HealthbarUI")]
    public GameObject healthbarUI;
    public Slider healthSlider;

    EnemyAnimationManager enemyAnimationManager;


    // Start is called before the first frame update
    void Start()
    {
        maxHP = getMaxHealthWithMultiplier();
        currentHP = maxHP;

        healthSlider.value = CalculateSliderHealth();
        enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
    }

    public void handleHealth()
    {

        healthSlider.value = CalculateSliderHealth();

        //Destroy enemy when health is zero -> enemyLocomotionManager
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    private float CalculateSliderHealth()
    {
        return currentHP / maxHP;
    }
}
