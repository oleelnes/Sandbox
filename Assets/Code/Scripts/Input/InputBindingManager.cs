using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;


public class InputBindingManager : MonoBehaviour
{
    public static PlayerInputActionsClass playerInputActionsClass; 
    ///< Global instance of input action class 
    ///< Rebinding works by changing this global class object at runtime

    
    
    // Global action events, to bind methods to rebinding events
    public static event Action<InputAction, int>    event_RebindingStarted;
    public static event Action                      event_RebindingComplete;
    public static event Action                      event_RebindingCancelled;

	[SerializeField] private static bool doPrint = true;

    private void Awake()
    {
		InitializeInputActionClass();
        ApplyAllSavedCustomBindings();
    }
	
	
	/**
	 * Checks if global player input action class is initailized, initializes it if not
	 **/	
	public static void InitializeInputActionClass(){
        if (playerInputActionsClass == null) {
            Debug.Log("[InputBindingManager> \tInitializing playerInputActionsClass.");
			playerInputActionsClass = new PlayerInputActionsClass();
		}
	}




    public static void StartRebind(string actionName, int bindingIndex, Text bindingText, TMPro.TMP_Text bindingText_tmp) {
        // Try to find input action from given name
        InputAction inputAction = playerInputActionsClass.asset.FindAction(actionName); 
        
        // Check if matching action found
        if (inputAction == null 
        ||  inputAction.bindings.Count <= bindingIndex
        ){
            // Couldn't find action
            Debug.Log("[InputBindingManager> \tAction with name "+actionName+" not found, or bindingIndex "+bindingIndex+" out of range ("+inputAction.bindings.Count+")");
            return;
        }
        
        // Check if binding is a composite (e.g. W+A+S+D => Vector2)
        if (inputAction.bindings[bindingIndex].isComposite)
		{
            if (inputAction.bindings[bindingIndex+1].isPartOfComposite	// Is part of a composite
            &&  bindingIndex+1 < inputAction.bindings.Count)			// Binding index is within range
			{
                // Recursively rebind next part of the composite
                DoRebind(inputAction, bindingIndex+1, bindingText, bindingText_tmp, true); 
            }
        } 
        else 
        {
            // Rebind normally
            DoRebind(inputAction, bindingIndex, bindingText, bindingText_tmp, false);
        }
    }
    

    private static void DoRebind(InputAction actionToRebind, int bindingIndex, Text bindingText, TMPro.TMP_Text bindingText_tmp, bool isComposite) 
    {
		
        if (actionToRebind != null 
		&&  bindingIndex   >= 0)
		{	
			Debug.Log("Rebinding triggered for action "+actionToRebind + " with expected type " + actionToRebind.expectedControlType);
			
			// Change text to communicate to the user that they need to press a button
			if ( bindingText == null) bindingText_tmp.text = $"Press {actionToRebind.expectedControlType}";
			else 					  bindingText.text     = $"Press {actionToRebind.expectedControlType}";

			// Disabling action before rebinding
			actionToRebind.Disable();

			// Use built-in input system rebinding object
			var rebindingObject = actionToRebind.PerformInteractiveRebinding(bindingIndex); 

			// When rebinding is complete
			rebindingObject.OnComplete(job =>
			{
				actionToRebind.Enable(); 	// Re-enable action
				job.Dispose(); 				// Delete the rebinding job
				
				// If composite binding
				if(isComposite)
				{
					var nextBindingIndex = bindingIndex + 1;
					// Recursively increase index until next binding isn't a composite
					if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isPartOfComposite)
						DoRebind(actionToRebind, nextBindingIndex, bindingText, bindingText_tmp, isComposite);
				}
				Debug.Log("Finished binding");
				SaveCustomBinding(actionToRebind);
				// Invoke if something is subscribed to it
				event_RebindingComplete?.Invoke(); 

			}
			);

			// If rebinding is cancelled
			rebindingObject.OnCancel(job =>
			{
				actionToRebind.Enable(); 			// Re-enable action
				job.Dispose(); 						// Delete the rebinding job
				event_RebindingCancelled?.Invoke(); // Trigger the rebinding cancelled event
			}
			);

			// Button that will cancel the rebinding process
			rebindingObject.WithCancelingThrough( "<Keyboard>/escape"  );
			// Trigger the rebinding started event
			event_RebindingStarted?.Invoke(actionToRebind, bindingIndex);
			// Start the rebinding process
			rebindingObject.Start(); 

		}	
    }


	
    public static void ResetBinding(string actionName, int bindingIndex)
    {
		// Get action asset by name
        InputAction action = playerInputActionsClass.asset.FindAction(actionName); 
		
        if (action == null || action.bindings.Count <= bindingIndex) {
            Debug.Log("[InputBindingManager> \tResetBinding called, but "+ (action.bindings.Count <= bindingIndex ? "the binding index is too high." : "can't find specified action by name: "+actionName));
            return;
        }


        // If binding is a composite (multiple inputs for one action, like W+A+S+D which is interpreted as a single movement vector2)
        if (action.bindings[bindingIndex].isComposite) {
            // Loop through each binding in this composite (recusrively)
            for (
				int i = bindingIndex; 
				i < action.bindings.Count 
					&& (action.bindings[i].isPartOfComposite || action.bindings[i].isComposite); 
				i++) 
			{
                // Remove custom binding
                action.RemoveBindingOverride(i);
            }
        }
        else
            // If  not a composite, remove normally
            action.RemoveBindingOverride(bindingIndex);

        // Save the reset binding to storage
        SaveCustomBinding(action);
    }



    private static void SaveCustomBinding(InputAction action) 
	{
        // Loop over bindings
        for (int i = 0; i < action.bindings.Count; i++) {
			// Create reference for binding in a parseable string format
			string key 	 = action.actionMap + action.name + i;	// Like "OnFootJumping1"
			string value = action.bindings[i].overridePath;		// Like "keyboard/space"
			
			// Save to playerprefs
            PlayerPrefs.SetString(key, value); 
        }
    }



    public static void ApplySavedCustomBinding(string actionName)
    {
        InitializeInputActionClass();
		
		// Get default action info
        InputAction action = playerInputActionsClass.asset.FindAction(actionName);
        
        // Loop over every binding
        for (int i=0; i<action.bindings.Count; i++) {
            // Check if binding override for this binding is in memory
			string memoryString = action.actionMap + action.name + i;
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(memoryString)))
				// Apply saved binding to this binding
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(memoryString));
        }
    }

    public static void ApplyAllSavedCustomBindings() {
		InitializeInputActionClass();
        // Loop through action maps
        var actionMaps = playerInputActionsClass.asset.actionMaps;
        for(int i=0; i < actionMaps.Count; i++) {
            // Loop through  actions
            for (int j=0; j < actionMaps[i].actions.Count; j++) {
                // Load saved binding for this action (if one exists)
                ApplySavedCustomBinding(actionMaps[i].actions[j].name);
            }
        }
    }







}