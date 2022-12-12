using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Actions_OnFoot : MonoBehaviour
{

	[SerializeField] private bool disable = false;
	
    /*************************************\
    | Pointers to scripts called by input |
    \*************************************/
    [SerializeField] private PlayerMovement playerMovementScript; ///< Player movement script to call when actions are taken
    [SerializeField] private PlayerCam      playerCameraScript;
    [SerializeField] private MeleeAttack    playerMeleeAttackScript;
    [SerializeField] private DestroyObject  destroyObjectScript;
    [SerializeField] private PauseMenu      pauseMenu; ///< Reference to relevant instance of pauseMenu script in scene (Could alternatively just search for it, but this is more robust)
    //[SerializeField] private InventoryUIController inventoryUIController; ///< Reference to relevant inventoryUIController script, makes one if null.
    [SerializeField] private PlayerInventoryHolder playerInventoryHolder;
    [SerializeField] private InventoryUIController playerInventoryUIController;
	
    [SerializeField] private WeaponSwitching weaponSwitching;
	[SerializeField] private Interactor interactor;
	[SerializeField] private MouseItemData mouseItemData;
	// [SerializeField] private InventoryDisplay[] inventoryDisplays;
	[SerializeField] private StaticInventoryDisplay staticInventoryDisplay;
	[SerializeField] private DynamicInventoryDisplay dynamicInventoryDisplay1;
	[SerializeField] private DynamicInventoryDisplay dynamicInventoryDisplay2;
    
    /**********************\
    | Configurables values |
    \**********************/
    [SerializeField] private float movementInputSensitivity = 0.02f;
    [SerializeField] private float cameraInputSensitivity   = 0.02f;
    
    
    
    //[SerializeField] private bool createNewInventory = true; ///< Whether to create a new inventory for the player if its missing
    //[SerializeField]  private int inventorySize = 10; ///< If making a new inventory, this is the size of the backpack inventory system
    //[SerializeField] private InventorySystem inventorySystem; ///< If not making a new inventory, set this.


    // [SerializeField]
    // private InputBindingManager InputBindingManager;

    
    // Polling input directly
    public Vector2 input_movement {
        get { return inputActions.OnFoot.Movement.ReadValue<Vector2>() * movementInputSensitivity; }
        set { return; }
    }
    public Vector2 input_cameraDelta {
        get { return inputActions.OnFoot.Camera.ReadValue<Vector2>()   * cameraInputSensitivity; }
        set { return; }
    }
    public Vector2 input_mousePos {
        get { return inputActions.OnFoot.MousePos.ReadValue<Vector2>(); }
        set { return; }
    }
    
    


    /***************\
    | Helper values |
    \***************/
    private bool hasBeenEnabled = false;
    private bool inputActionsHasBeenInitialized = false;
    [SerializeField]
    private bool doPrint = false;
    [SerializeField]
    private bool doPrintVerbose = false;



    /********************\
    | Input action asset |
    \********************/
    private PlayerInputActionsClass inputActions; // Local instance of input action asset, from InputBindingManager's instance if possible 
    // (Note: InputBindingManager not committed due to it not being fully implemented yet, the idea is that it contains an instance of each input action class, 
    //        and that in order to rebind inputs, it will change the properties of this class during runtime)



    /************************\
    | Updating polled values |
    \************************/    
    private void FixedUpdate(){
        if(inputActions != null && !disable){
            // Could check if the variables are changed before updating, but I think calculating the magnitude of a vector is more performance intensive than just updating a variable
            playerMovementScript.UpdateInput_Movement(	input_movement		);
            playerCameraScript  .UpdateInput_Camera(	input_cameraDelta 	);
			mouseItemData		.UpdateInput_MousePos(	input_mousePos		);
            // Updating input in player movement script (because this script calls the player movement script, not the other way around)
        } 
    }
    



    /**
     * @brief Called when object is enabled, initializes actions.
     **/
    private void OnEnable()
    {
        
        if (doPrint) Debug.Log("[Actions OnFoot> \tOnEnable called");

    }

    private void Update(){
        
        if( inputActionsHasBeenInitialized == false 
        &&  InputBindingManager.playerInputActionsClass != null) {
            if(doPrint) Debug.Log("[Actions OnFoot> \tFound InputBindingManager.playerInputActionsClass");
            inputActions = InputBindingManager.playerInputActionsClass;
            inputActionsHasBeenInitialized = true;
            if(!disable) Enable();
        }
        
    }
    

    /**
     * Enables and binds action inputs
     **/
    private void Enable()
    {
        hasBeenEnabled = true;
        if (doPrint) Debug.Log("[Actions OnFoot> \tEnable() called");


        // Subscribing actions to functions
        /*\ Jump action \*/
        inputActions.OnFoot.Jump        .performed   += context => playerMovementScript.DoJump();
        
        /*\ Sprint action \*/
        inputActions.OnFoot.Sprint      .started     += context => playerMovementScript.DoSprint();
        inputActions.OnFoot.Sprint      .canceled    += context => playerMovementScript.ReleaseSprint();
		
		// for(int i=0; i<inventoryDisplays.Length; i++){
		// 	inputActions.OnFoot.Sprint      .started     += context => inventoryDisplays[i].DoPressShift();
		// 	inputActions.OnFoot.Sprint      .canceled    += context => inventoryDisplays[i].DoReleaseShift();
		// 	if(doPrintVerbose) Debug.Log("inventory display index: "+i);
		// }
		
		inputActions.OnFoot.Sprint      .started     += context => staticInventoryDisplay.DoPressShift();
		inputActions.OnFoot.Sprint      .canceled    += context => staticInventoryDisplay.DoReleaseShift();
		inputActions.OnFoot.Sprint      .started     += context => dynamicInventoryDisplay1.DoPressShift();
		inputActions.OnFoot.Sprint      .canceled    += context => dynamicInventoryDisplay1.DoReleaseShift();
		inputActions.OnFoot.Sprint      .started     += context => dynamicInventoryDisplay2.DoPressShift();
		inputActions.OnFoot.Sprint      .canceled    += context => dynamicInventoryDisplay2.DoReleaseShift();
        
		
        /*\ Crouch action \*/
        inputActions.OnFoot.Crouch      .started     += context => playerMovementScript.DoCrouch();
        inputActions.OnFoot.Crouch      .canceled    += context => playerMovementScript.ReleaseCrouch();
        inputActions.OnFoot.Crouch      .performed   += context => playerInventoryUIController.CancelButtonPressed();
        
        /*\ Attack action \*/
        inputActions.OnFoot.Attack      .started     += context => playerMeleeAttackScript.DoAttack();
        inputActions.OnFoot.Attack      .canceled    += context => playerMeleeAttackScript.ReleaseAttack();
        inputActions.OnFoot.Attack      .started     += context => destroyObjectScript.DoAttack();
        inputActions.OnFoot.Attack      .canceled    += context => destroyObjectScript.ReleaseAttack();
		/*\ Click action \*/
		inputActions.OnFoot.Attack      .canceled    += context => mouseItemData.DoClick();
        
		
        /*\ Interact action \*/
        // inputActions.OnFoot.Interact    .performed   += context => playerMovementScript.DoInteract();
        inputActions.OnFoot.Interact    .performed   += context => interactor.DoInteract();
        
        /*\ Inventory action \*/
        inputActions.OnFoot.Inventory   .performed   += context => playerInventoryHolder.OpenBackpack();
		
		/*\ Slot change action \*/
		inputActions.OnFoot.SlotChange	.performed	 += context => weaponSwitching.ChangeSlot(inputActions.OnFoot.SlotChange.ReadValue<float>());
		
		/*\ Slot set actions \*/
		inputActions.OnFoot.SlotSet_1   .performed 	 += context => weaponSwitching.SetSlot(0);
		inputActions.OnFoot.SlotSet_2   .performed 	 += context => weaponSwitching.SetSlot(1);
		inputActions.OnFoot.SlotSet_3   .performed 	 += context => weaponSwitching.SetSlot(2);
		inputActions.OnFoot.SlotSet_4   .performed 	 += context => weaponSwitching.SetSlot(3);
		inputActions.OnFoot.SlotSet_5   .performed 	 += context => weaponSwitching.SetSlot(4);
		inputActions.OnFoot.SlotSet_6   .performed 	 += context => weaponSwitching.SetSlot(5);
		inputActions.OnFoot.SlotSet_7   .performed 	 += context => weaponSwitching.SetSlot(6);
		inputActions.OnFoot.SlotSet_8   .performed 	 += context => weaponSwitching.SetSlot(7);
		inputActions.OnFoot.SlotSet_9   .performed 	 += context => weaponSwitching.SetSlot(8);
		
        /*\ Pause action \*/
        inputActions.OnFoot.Pause       .performed   += context => pauseMenu.TogglePause(); //(pauseMenu != null ? pauseMenu.TogglePause() : inventoryUIController.DisplayPlayerBackpack(inventorySystem));
        inputActions.OnFoot.Pause       .performed   += context => playerInventoryUIController.CancelButtonPressed();
        
        
        // Note: movement & Camera are both vec2 https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/Actions.html

        // Enabling the action inputs
        inputActions.OnFoot.Jump        .Enable();
        inputActions.OnFoot.Sprint      .Enable();
        inputActions.OnFoot.Crouch      .Enable();
        inputActions.OnFoot.Attack      .Enable();
        inputActions.OnFoot.Interact    .Enable();
        inputActions.OnFoot.Inventory   .Enable();
        inputActions.OnFoot.Pause       .Enable();
        inputActions.OnFoot.Movement    .Enable();
        inputActions.OnFoot.Camera      .Enable();
		
		inputActions.OnFoot.MousePos    .Enable();
		inputActions.OnFoot.SlotChange	.Enable();
		
		inputActions.OnFoot.SlotSet_1	.Enable();
		inputActions.OnFoot.SlotSet_2	.Enable();
		inputActions.OnFoot.SlotSet_3	.Enable();
		inputActions.OnFoot.SlotSet_4	.Enable();
		inputActions.OnFoot.SlotSet_5	.Enable();
		inputActions.OnFoot.SlotSet_6	.Enable();
		inputActions.OnFoot.SlotSet_7	.Enable();
		inputActions.OnFoot.SlotSet_8	.Enable();
		inputActions.OnFoot.SlotSet_9	.Enable();
		
		
    }



    /**
     * Called when object is disabled.
     **/
    private void OnDisable() {
        if (doPrintVerbose) Debug.Log("[Actions OnFoot> \tOnDisable called");
        Disable();
    }
    
    
    /**
     * Disables all input actions.
     **/
    private void Disable() {
        if (doPrint) Debug.Log("[Actions OnFoot> \nDisable called");
        // Disabling the action inputs, so they won't call
        inputActions.OnFoot.Jump        .Disable();
        inputActions.OnFoot.Sprint      .Disable();
        inputActions.OnFoot.Crouch      .Disable();
        inputActions.OnFoot.Attack      .Disable();
        inputActions.OnFoot.Interact    .Disable();
        inputActions.OnFoot.Inventory   .Disable();
        inputActions.OnFoot.Pause       .Disable();
        inputActions.OnFoot.Movement    .Disable();
        inputActions.OnFoot.Camera		.Disable();
		
		inputActions.OnFoot.MousePos    .Disable();
		inputActions.OnFoot.SlotChange	.Disable();
		
		inputActions.OnFoot.SlotSet_1	.Disable();
		inputActions.OnFoot.SlotSet_2	.Disable();
		inputActions.OnFoot.SlotSet_3	.Disable();
		inputActions.OnFoot.SlotSet_4	.Disable();
		inputActions.OnFoot.SlotSet_5	.Disable();
		inputActions.OnFoot.SlotSet_6	.Disable();
		inputActions.OnFoot.SlotSet_7	.Disable();
		inputActions.OnFoot.SlotSet_8	.Disable();
		inputActions.OnFoot.SlotSet_9	.Disable();
		
    }

}
