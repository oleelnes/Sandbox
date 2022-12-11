using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantCaveInventory : MonoBehaviour
{

    public InventoryItemData treeItemData;

    void Start()
    {
        Player player = FindObjectOfType<Player>();
        var inventory = player.transform.GetComponent<InventoryHolder>();
        inventory.InventorySystem.AddToInventory(treeItemData, PlayerPrefs.GetInt("wood"));

        Debug.Log("------------------------" + PlayerPrefs.GetInt("wood"));
        Debug.Log("------------------------" + PlayerPrefs.HasKey("wood"));

        var chest = GameObject.Find("chest");
        var chestInv = chest.transform.GetComponent<ChestInventory>();
        Debug.Log(chestInv);
        chestInv.InventorySystem.AddToInventory(treeItemData, PlayerPrefs.GetInt("wood"));
    }

}
