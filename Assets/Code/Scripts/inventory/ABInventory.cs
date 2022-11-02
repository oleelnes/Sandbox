using System.Collections.Generic;
using UnityEngine; 


public class ABInventory : MonoBehaviour { 

    public List<UIItem> ABItems = new List<UIItem>();
    public GameObject ABslotPrefab;
    public Transform ABslotPanel;
    

    public int numberOfABSlots = 10;

    private void Awake() {
        for(int i = 0; i < numberOfABSlots; i++) {
            GameObject instance = Instantiate(ABslotPrefab);
            instance.transform.SetParent(ABslotPanel);
            ABItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    public void UpdateSlot(int slot, Item item) {
        ABItems[slot].UpdateItem(item);
    }

    public void AddNewItem(Item item) {
        UpdateSlot(ABItems.FindIndex(i => i.item == null), item);
    }

    public void RemoveItem(Item item) {
        UpdateSlot(ABItems.FindIndex(i => i.item == item), null);
    }

}