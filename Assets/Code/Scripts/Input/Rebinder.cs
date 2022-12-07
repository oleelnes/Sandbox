using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rebinder : MonoBehaviour
{
	private bool doPrint = true;
	
	[Header("Select action to rebind")]
    [SerializeField] private InputActionReference inputActionReference; 
    ///< Reference to an action from from the input action asset (not class)
    [Header("Select binding by index")]
	[SerializeField] [Range(0, 4)] private int selectedBindingIndex;
	///< Selector for which binding of selected action to affect, using index. Code safety-checks if binding is valid before acting.
	// 		(Currently first index is the controller, and the second is the keyboard/mouse binding.)
	// 		Should create a button to switch the index of every rebinding prefab to either keyb/mouse or controller
	
    [Header("UI objects (Text objects using TextMesh Pro plugin)")]
    [SerializeField] private Button         button_Reset;
    [SerializeField] private Button         button_Rebind;
    [SerializeField] private TMPro.TMP_Text text_ActionName;
    [SerializeField] private TMPro.TMP_Text text_BindingName;
	[SerializeField] private Text 			text_BindingName_NonTMP;
	[SerializeField] private bool 			doUseNonTMPText = false;
    


    [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions; ///< A built-in enumerator to format the names of the bindings

    [Header("Info about currently selected binding")] // To see what binding is currently selected for this rebinding component
    [SerializeField] private InputBinding inputBindingInfo;  ///< Has information about the binding
    
	
	
	// Core variables
	private string actionName;	///< Name of currently selected action
    private int bindingIndex;	///< Index for currently selected binding
    


    private void OnEnable() {

		// Bind methods to buttons
        button_Rebind.onClick.AddListener(() => InputBindingManager.StartRebind(actionName, bindingIndex, text_BindingName_NonTMP, text_BindingName));
		button_Reset .onClick.AddListener(() => ResetBinding());
		
		// Bind events to update the given text object's text to show the current binding (or status text)    
        InputBindingManager.event_RebindingComplete  += RefreshUI;
        InputBindingManager.event_RebindingCancelled += RefreshUI;

        if(inputActionReference != null) {
			if (text_ActionName == null) UpdateBindingInfo();
			
			// Load previously changed bindings from player prefs storage
            InputBindingManager.ApplySavedCustomBinding(actionName);
            UpdateBindingInfo();
            RefreshUI();
        }
    }
    
	
	// Unbind events that update UI
    private void OnDisable() {
        // Unbind events
        InputBindingManager.event_RebindingComplete  -= RefreshUI;
        InputBindingManager.event_RebindingCancelled -= RefreshUI;
    }


    
    // Called when something in the inspector changes, update bindings and UI
    private void OnValidate() { 
        if (inputActionReference != null) {
			UpdateBindingInfo();
			RefreshUI();
		} 
    }

    
	/**
	 * Call to reset current binding and refresh the UI
	 **/
    private void ResetBinding() {
        InputBindingManager.ResetBinding(actionName, bindingIndex);
        RefreshUI();
    }


	/**
	 * Update local info on currently selected binding
	 **/
    private void UpdateBindingInfo() {
        if (inputActionReference.action != null){
			
			actionName = inputActionReference.action.name;

			// Make sure that selected binding index isn't above the number of bindings
			if (inputActionReference.action.bindings.Count > selectedBindingIndex) {
				
				// Update info on action and binding
				inputBindingInfo = inputActionReference.action.bindings[selectedBindingIndex]; 
				// Update index
				bindingIndex = selectedBindingIndex;
				
			}
			
		} 

    }


	/**
	 * Update the UI to reflect the current values for this action and selected binding
	 **/
    private void RefreshUI() {
        // Update action text element 
		if (text_ActionName  != null) text_ActionName.text = actionName;
		
		// Udpate binding text element  (allowing for legacy text instead of TMP)
        if (text_BindingName != null || (doUseNonTMPText && text_BindingName_NonTMP != null)) {
            
			// Update with different method if in play mode
            if (Application.isPlaying) {
				
				// If in play mode, getting info from the InputBindingManager
				InputBindingManager.InitializeInputActionClass();
				var an = InputBindingManager.playerInputActionsClass.asset.FindAction(actionName);
				var bi = an.GetBindingDisplayString(bindingIndex);
				string bindingText = bi.ToString();
				
				// Updating text
				text_BindingName.text = bindingText;
				if (doUseNonTMPText) text_BindingName_NonTMP.text = bindingText; // For some reason TMP text wouldn't appear, so allowing the use of legacy text.
				
            } else {
				
                // If in inspector, getting info directly from input action asset
				string bindingText = inputActionReference.action.GetBindingDisplayString(bindingIndex);
				// Updating text
                text_BindingName.text = bindingText;
				if (doUseNonTMPText) text_BindingName_NonTMP.text = bindingText;
				
            } 
        }
    }

	

}
