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

    void Update()
    {
       if(chestPanel.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape)) {
        chestPanel.gameObject.SetActive(false);
       }

        if(backpackPanel.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape)) {
        backpackPanel.gameObject.SetActive(false);
       }
    }

    void DisplayInventory(InventorySystem invToDisplay) {
        chestPanel.gameObject.SetActive(true); 
        chestPanel.RefreshDynamicInventory(invToDisplay);
    }

    void DisplayPlayerBackpack(InventorySystem invToDisplay) {
    backpackPanel.gameObject.SetActive(true); 
    backpackPanel.RefreshDynamicInventory(invToDisplay);
    }
}
