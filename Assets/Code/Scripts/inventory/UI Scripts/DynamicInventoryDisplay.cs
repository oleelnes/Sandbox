using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlot_UI slotPrefab;

    protected override void Start() {
        InventoryHolder.OnDynamicInventoryDisplayRequested += RefreshDynamicInventory;
        base.Start();

        AssignSlot(inventorySystem);
    }

    private void OnDestroy() {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= RefreshDynamicInventory;

    }

    public void RefreshDynamicInventory(InventorySystem invToDisplay) {
        inventorySystem = invToDisplay; 

    }

    public override void AssignSlot(InventorySystem invToDisplay) {
       // ClearSlots(); 

        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>(); 

        if(invToDisplay == null) return; 

        for(int i = 0; i < invToDisplay.InventorySize; i++) {
            var uiSlot = Instantiate(slotPrefab, transform);
            slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]); 
            uiSlot.Init(invToDisplay.InventorySlots[i]); 
            uiSlot.UpdateUISlot(); 
        }
        
    }
}
