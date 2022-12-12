using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantCaveInventory : MonoBehaviour
{

    public InventoryItemData treeItemData;
    public InventoryItemData keyItemData; 

    void Start()
    {
        Player player = FindObjectOfType<Player>();
        var inventory = player.transform.GetComponent<InventoryHolder>();
        inventory.InventorySystem.AddToInventory(treeItemData, PlayerPrefs.GetInt("wood"));

        var chest = GameObject.Find("chest");
        var chestInv = chest.transform.GetComponent<ChestInventory>();
        chestInv.InventorySystem.AddToInventory(keyItemData, 1);
    }

}
