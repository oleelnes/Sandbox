using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{

    /*    [Header("Enemy stats")]
        public float maxHP = 100f;
        public float currentHP = 100f;
        public float damage = 1f;*/

    [Header("HealthbarUI")]
    public GameObject healthbarUI;
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        /*        //get the child object Slider
                healthSlider = GameObject.FindGameObjectWithTag("EnemyHealthbar").GetComponent<Slider>();*/

        maxHP = getMaxHealthWithMultiplier();
        currentHP = maxHP;

        healthSlider.value = CalculateSliderHealth();
    }

    public void handleHealth()
    {

        healthSlider.value = CalculateSliderHealth();

        //Destroy enemy when health is zero
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        if (currentHP == 0)
        {
            //ENEMY DEATH CODE HERE
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            Destroy(gameObject);

        }
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    private float CalculateSliderHealth()
    {
        return currentHP / maxHP;
    }
}
