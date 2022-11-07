using System.Collections.Generic;
using UnityEngine; 


public class ItemDatabase : MonoBehaviour {

    public List<Item> items = new List<Item>();

    public void Awake(){
        BuildDatabase();
    }

    public Item GetItem(int id) {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName) {
        return items.Find(item => item.title == itemName);
    }

    void BuildDatabase() {
        
        items = new List<Item>() {
            new Item(0, "Sword_diamond", "Sword made with diamonds"), 
            new Item(1, "pick_diamond", "Axe made with diamonds"),
            new Item(3, "ore_diamond", "Wood")
        };

    }

}