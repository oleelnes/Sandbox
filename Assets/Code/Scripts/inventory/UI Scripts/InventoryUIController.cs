using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay chestPanel;    
    public DynamicInventoryDisplay backpackPanel; 


    private void Awake() {
        chestPanel.gameObject.SetActive(false);
        backpackPanel.gameObject.SetActive(false);
    }
    
    public void OnEnable() {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack; 
    }

    public void OnDisable() {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack; 
    }
    
    // Event triggered by input script
    public void CancelButtonPressed(){
        /* if(PlayerCam.isBackpackOpen && backpackPanel.gameObject.activeInHierarchy) { CloseBackpack(); } // Handled by PlayerInventoryHolder. */
        
        if(chestPanel.gameObject.activeInHierarchy) {
            chestPanel.gameObject.SetActive(false);
        }
    }
    
    public void CloseBackpack(){
        backpackPanel.gameObject.SetActive(false);
        PlayerCam.isBackpackOpen = false; 
    }

    void DisplayInventory(InventorySystem invToDisplay) {
        chestPanel.gameObject.SetActive(true); 
        chestPanel.RefreshDynamicInventory(invToDisplay);
    }

    public void DisplayPlayerBackpack(InventorySystem invToDisplay) {
    backpackPanel.gameObject.SetActive(true); 
    backpackPanel.RefreshDynamicInventory(invToDisplay);
    }
}
