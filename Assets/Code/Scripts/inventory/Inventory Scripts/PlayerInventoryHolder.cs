using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int backpackSize; 
    [SerializeField] protected InventorySystem secondaryInventorySystem; 
    
    [SerializeField] private InventoryUIController inventoryUIController;
    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;

    protected override void Awake() {
        base.Awake(); 

        secondaryInventorySystem = new InventorySystem(backpackSize); 
    }

    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.I)){ 
    //     } 
    // }
    
    public void OpenBackpack(){
        if( ! PlayerCam.isBackpackOpen ){
            // If backpack closed, open it
            OnPlayerBackpackDisplayRequested?.Invoke(secondaryInventorySystem);
            PlayerCam.isBackpackOpen = true; 
        } else {
            inventoryUIController.CloseBackpack();
        }
    }
    

    public bool AddToInventory(InventoryItemData data, int amount) {
        if(inventorySystem.AddToInventory(data, amount)){
            return true; 
        }
        else if(secondaryInventorySystem.AddToInventory(data, amount)) {
            return true; 
        }
        return false; 
    }
}
