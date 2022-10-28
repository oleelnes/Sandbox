using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached script to weapon object
public class MeleeAttack : MonoBehaviour
{
    //weapon animation
    Animator anim;
    Collider weaponColl;
    public bool disableAnimation = false;

    [Header("Mouse buttons")]
    public KeyCode mouse0 = KeyCode.Mouse0;


    [Header("Damage")]
    public float meleeDamage;
    void Start()
    {
        anim = GetComponent<Animator>();
        weaponColl = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {   
        checkInventoryStatus();
        
        if(!disableAnimation)
        {
            attack();
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.tag == "Enemy")
        {
            if (anim.GetBool("attacking"))
            {
                collision.SendMessage("receiveDamage", meleeDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void attack()
    {
        if(!disableAnimation)
        {
            attack();
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.tag == "Enemy")
        {
            if (anim.GetBool("attacking"))
            {
                collision.SendMessage("receiveDamage", meleeDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void attack()
    {
        if (Input.GetKeyDown(mouse0))
        {
            //Trigger only when clicked
            weaponColl.isTrigger = true;

            //initialize animation
            anim.SetBool("attacking", true);

        }
        else if (Input.GetKeyUp(mouse0))
        {
            weaponColl.isTrigger = false;
            anim.SetBool("attacking", false);
        }
    }


