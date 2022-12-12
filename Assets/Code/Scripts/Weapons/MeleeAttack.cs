using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//attached script to weapon object
public class MeleeAttack : ClickEvent
{


    [Header("AudioEvent")]
    [SerializeField]
    private UnityEvent enemyHitEvent;
    [SerializeField]
    private UnityEvent nonHitEvent;
    void OnTriggerEnter(Collider collision)
    {

        if (collision.tag == "Enemy")
        {
            if (anim.GetBool("attacking"))
            {
                collision.SendMessage("receiveDamage", meleeDamage, SendMessageOptions.DontRequireReceiver);
                //if player attacks enemy but isn't the current target yet
                collision.SendMessage("setCurrentTargetToPlayer", SendMessageOptions.DontRequireReceiver);
                enemyHitEvent.Invoke();
            }
        }
    }

    override public void DoAttack()
    {
            //audio
            nonHitEvent.Invoke();
            //Trigger only when clicked
            weaponColl.isTrigger = true;

            //initialize animation
            if (!disableAnimation) anim.SetBool("attacking", true);
    }



}