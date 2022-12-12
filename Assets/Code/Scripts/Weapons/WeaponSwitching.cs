using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    public int selectedWeapon = 0;
    public float weaponHolder_x = 0.86f;
    public float weaponHolder_y = -0.36f;
    public float weaponHolder_z = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        selectWeapon();
    }

    public void SetSlot(int index)
    {
        Debug.Log("Changing slot to " + index);
        if (index >= 0 && index <= 8
        && transform.childCount >= index + 1)
            selectedWeapon = index;
    }

    public void ChangeSlot_Right()
    {
        int index = selectedWeapon + 1;
        if (index > 8)
            index = 0;

        SetSlot(index);
    }
    public void ChangeSlot_Left()
    {
        int index = selectedWeapon - 1;
        if (index < 0)
            index = transform.childCount; // ? -1 maybe

        SetSlot(index);
    }
    public void ChangeSlot(float direction)
    {
        if (direction < 0) ChangeSlot_Left();
        if (direction > 0) ChangeSlot_Right();
    }


    private void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

}
