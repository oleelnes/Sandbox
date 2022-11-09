using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int backpackSize; 
    [SerializeField] protected InventorySystem secondaryInventorySystem; 
    
    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;

    protected override void Awake() {
        base.Awake(); 

        secondaryInventorySystem = new InventorySystem(backpackSize); 
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)){
            OnPlayerBackpackDisplayRequested?.Invoke(secondaryInventorySystem);
            PlayerCam.isBackpackOpen = true; 
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
