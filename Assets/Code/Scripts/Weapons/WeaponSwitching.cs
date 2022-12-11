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
        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        int previousSelectedWeaopn = selectedWeapon;

        /*if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }*/

        // CheckNumberKeyPressed();

        if (previousSelectedWeaopn != selectedWeapon)
        {
            selectWeapon();
        }
    }

    private void CheckNumberKeyPressed()
    {
    /*    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
        {
            selectedWeapon = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 6)
        {
            selectedWeapon = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && transform.childCount >= 7)
        {
            selectedWeapon = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && transform.childCount >= 8)
        {
            selectedWeapon = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && transform.childCount >= 9)
        {
            selectedWeapon = 8;
        }
		*/
    }
	
	public void SetSlot(int index){
		Debug.Log("Changing slot to "+index);
		if( index >= 0 && index <= 8 
		&&  transform.childCount >= index+1)
			selectedWeapon = index;
	}
	
	public void ChangeSlot_Right(){
		int index = selectedWeapon + 1;
		if (index > 8)
			index = 0;
			
		SetSlot(index);
	}
	public void ChangeSlot_Left(){
		int index = selectedWeapon - 1;
		if (index < 0)
			index = transform.childCount; // ? -1 maybe
		
		SetSlot(index);
	}
	public void ChangeSlot(float direction){
		if(direction < 0) ChangeSlot_Left();
		if(direction > 0) ChangeSlot_Right();
	}


    private void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }

}
