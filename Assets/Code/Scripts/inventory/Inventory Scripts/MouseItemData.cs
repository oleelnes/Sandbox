using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;
    public KeyCode mouse0 = KeyCode.Mouse0;

	private static Vector2 mousePosition;
	public void UpdateInput_MousePos(Vector2 pos){ MouseItemData.mousePosition = pos; }

    void Awake() {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
		MouseItemData.mousePosition  = new Vector2(0.0f, 0.0f);
    }

    public void UpdateMouseSlot(InventorySlot invSlot) {
        AssignedInventorySlot.AssignItem(invSlot);
        ItemSprite.sprite = invSlot.ItemData.Icon;
        ItemCount.text = invSlot.StackSize.ToString(); 
        ItemSprite.color = Color.white;  
    }

    private void Update() {
        if(AssignedInventorySlot.ItemData != null) {
            transform.position = MouseItemData.mousePosition; 

            // if(Input.GetKeyDown(mouse0) && !IsPointerOverUIObject()) { // TODO update
            //     ClearSlot();
            // }
        }
    }

	// TODO Call by attack by default
	public void DoClick(){
		if(!IsPointerOverUIObject()){
			ClearSlot();
		}
		Debug.Log(MouseItemData.mousePosition);
		
	}

    public void ClearSlot() {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear; 
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current); 
        eventDataCurrentPosition.position = MouseItemData.mousePosition; 
        List<RaycastResult> results = new List<RaycastResult>(); 
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0; 
    }

}
