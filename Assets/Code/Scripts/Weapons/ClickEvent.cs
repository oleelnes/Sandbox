using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//attached script to weapon object
public abstract class ClickEvent : MonoBehaviour
{
    //weapon animation
    public Animator anim;
    public Collider weaponColl;
    public bool disableAnimation = false;

    [Header("Mouse buttons")]
    public KeyCode mouse0 = KeyCode.Mouse0;


    [Header("Damage")]
    public float meleeDamage = 25f;

    void Start()
    {
        anim = GetComponent<Animator>();
        weaponColl = GetComponent<Collider>();
    }


    // Update is called once per frame
    void Update()
    {
        checkInventoryStatus();
    }

    public abstract void DoAttack();

    public void ReleaseAttack()
    {
        weaponColl.isTrigger = false;
        if (!disableAnimation) anim.SetBool("attacking", false);
    }

    public void checkInventoryStatus()
    { // TODO Disable action map when opening inventory and deprecate this
        if (PlayerCam.isBackpackOpen)
        {
            disableAnimation = true;
        }
        else
        {
            disableAnimation = false;
        }
    }


}