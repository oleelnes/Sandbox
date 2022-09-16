using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached script to weapon object
public class MeleeAttack : MonoBehaviour
{
    //weapon animation
    Animator anim;
    Collider weaponColl;

    [Header("Mouse buttons")]
    public KeyCode mouse0 = KeyCode.Mouse0;
    void Start()
    {
        anim = GetComponent<Animator>();
        weaponColl = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(mouse0))
        {
           //Trigger only when clicked
            weaponColl.isTrigger = true;

            //initialize animation
            anim.SetBool("attacking", true);

        }else if (Input.GetKeyUp(mouse0))
        {
            weaponColl.isTrigger = false;
            anim.SetBool("attacking", false);
        }
    }
}
