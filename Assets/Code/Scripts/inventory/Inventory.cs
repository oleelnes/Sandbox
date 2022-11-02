using System.Collections.Generic;
using UnityEngine; 


public class Inventory : MonoBehaviour {

    public List<Item> characterItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;
    public ABInventory actionBarUI;

    public static bool inventoryStatus = false;

    private void Start() {
        GiveItem(0);
        GiveItem(1);
        inventoryUI.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
            if(inventoryUI.gameObject.activeSelf){
                inventoryStatus = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                inventoryStatus = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        } 
    }

    public void GiveItem(int id) {
        Item itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public void GiveItem(string itemName) {
        Item itemToAdd = itemDatabase.GetItem(itemName);
        characterItems.Add(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public Item CheckForItem(int id) {
        return characterItems.Find(item => item.id == id); 
    }

    public void RemoveItem(int id) {
        Item item = CheckForItem(id);
        if (item != null) {
            characterItems.Remove(item);
            inventoryUI.RemoveItem(item);
            Debug.Log("Item removed: " + item.title);
        }
    }

}

// https://medium.com/@yonem9/create-an-unity-inventory-part-2-configure-the-inventory-3a990eff8cba

// https://medium.com/@yonem9/create-an-unity-inventory-part-3-ui-cd4c5e8dedba

// https://medium.com/@yonem9/create-an-unity-inventory-part-4-display-items-in-ui-6cdac8f734b7

// https://medium.com/@yonem9/create-an-unity-inventory-part-5-drag-and-drop-d4374201539b

// https://medium.com/@yonem9/create-an-unity-inventory-part-6-generate-tooltip-c50dedcf7457