using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//attached script to weapon object
public class MeleeAttack : MonoBehaviour
{
    //weapon animation
    public Animator anim;
    Collider weaponColl;
    public bool disableAnimation = false;

    [Header("Mouse buttons")]
    public KeyCode mouse0 = KeyCode.Mouse0;


    [Header("Damage")]
    public float meleeDamage = 25f;

    [Header("AudioEvent")]
    [SerializeField]
    private UnityEvent enemyHitEvent;
    [SerializeField]
    private UnityEvent nonHitEvent;

    void Start()
    {
        anim = GetComponent<Animator>();
        weaponColl = GetComponent<Collider>();
    }

    
    // Update is called once per frame
    void Update()
    {
        checkInventoryStatus();
        
        checkInventoryStatus();
        
        if (!disableAnimation)
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
                //if player attacks enemy but isn't the current target yet
                collision.SendMessage("setCurrentTargetToPlayer", SendMessageOptions.DontRequireReceiver);
                enemyHitEvent.Invoke();
            }
        }
    }

    void attack()
    {
        if (Input.GetKeyDown(mouse0))
        {
            //audio
            nonHitEvent.Invoke();
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

    public void checkInventoryStatus() {
        if(PlayerCam.isBackpackOpen) {
            disableAnimation = true;
        } else {
            disableAnimation = false; 
        }
    }


}