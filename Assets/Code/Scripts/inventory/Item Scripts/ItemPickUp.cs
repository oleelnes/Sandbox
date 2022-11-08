 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItemData ItemData;

    private SphereCollider myCollider; 

    private void Awake() {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true; 
        myCollider.radius = PickUpRadius;

    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("hit the ball");
        Player player = FindObjectOfType<Player>();
        var inventory = player.transform.GetComponent<InventoryHolder>();

        if (!inventory)
        {
            Debug.Log("inventory doesn't exist!");
            return;
        }

        if(inventory.InventorySystem.AddToInventory(ItemData, 1)) {
            Destroy(this.gameObject);
        }
    }

    public void AddToInventory(GameObject objectToAdd)
    {
        Player player = FindObjectOfType<Player>();
        var inventory = player.transform.GetComponent<InventoryHolder>();

        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(ItemData, 1))
        {
            //Destroy(this.gameObject);
        }
    }
}
