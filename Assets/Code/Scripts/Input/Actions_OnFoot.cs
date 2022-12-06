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
    [SerializeField] private PauseMenu      pauseMenu; ///< Reference to relevant instance of pauseMenu script in scene (Could alternatively just search for it, but this is more robust)
    //[SerializeField] private InventoryUIController inventoryUIController; ///< Reference to relevant inventoryUIController script, makes one if null.
    [SerializeField] private PlayerInventoryHolder playerInventoryHolder;
    [SerializeField] private InventoryUIController playerInventoryUIController;
    
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
            playerMovementScript.UpdateInput_Movement(input_movement);
            playerCameraScript  .UpdateInput_Camera(  input_cameraDelta  );
            // Updating input in player movement script (because this script calls the player movement script, not the other way around)
        } 
    }
    



    /**
     * @brief Called when object is enabled, initializes actions.
     **/
    private void OnEnable()
    {
        
        if (doPrint) Debug.Log("[Actions OnFoot> \tOnEnable called");

        // if (inventoryUIController == null)   inventoryUIController = new InventoryUIController();
        // if (inventorySystem == null)         inventorySystem = new InventorySystem(inventorySize);

        // Check if inputActions not initialized yet
        // if (InputBindingManager.playerInputActionsClass != null  &&  inputActions == null)  {
        //     if(doPrint) Debug.Log("[Actions OnFoot> \tFound playerInputActionsClass in InputBindingManager at first try");
        //     // Initialize user input actions
        //     inputActions = InputBindingManager.playerInputActionsClass;    
        //     inputActionsHasBeenInitialized = true;
        //     // Enable action map
        //     Enable();
        // } else {
        //     // Wait for it to be initialized
        //     if(doPrintVerbose) Debug.Log("[Actions OnFoot> \tInputBindingManager.playerInputActionsClass not initialized, waiting with enabling");
        //     inputActionsHasBeenInitialized = false;
        // }
        // else 
        // if (inputActions == null) {                  
        //     Debug.Log("kgjri");
        //     // Create local input actions if no InputBindingManager 
        //     inputActions = new PlayerInputActionsClass();
        //     Enable();   // Enable action map 
        // } else {
        //     Debug.Log("[Actions OnFoot> \tOnEnable, input actions already initialized locally at time of enabling.");
        // }
        
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
        
        /*\ Crouch action \*/
        inputActions.OnFoot.Crouch      .started     += context => playerMovementScript.DoCrouch();
        inputActions.OnFoot.Crouch      .canceled    += context => playerMovementScript.ReleaseCrouch();
        inputActions.OnFoot.Crouch      .performed   += context => playerInventoryUIController.CancelButtonPressed();
        
        /*\ Attack action \*/
        inputActions.OnFoot.Attack      .started     += context => playerMeleeAttackScript.DoAttack();
        inputActions.OnFoot.Attack      .canceled    += context => playerMeleeAttackScript.ReleaseAttack();
        
        /*\ Interact action \*/
        inputActions.OnFoot.Interact    .performed   += context => playerMovementScript.DoInteract();
        
        /*\ Inventory action \*/
        inputActions.OnFoot.Inventory   .performed   += context => playerInventoryHolder.OpenBackpack();
        
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
        inputActions.OnFoot.Jump         .Disable();
        inputActions.OnFoot.Sprint       .Disable();
        inputActions.OnFoot.Crouch       .Disable();
        inputActions.OnFoot.Attack       .Disable();
        inputActions.OnFoot.Interact     .Disable();
        inputActions.OnFoot.Inventory    .Disable();
        inputActions.OnFoot.Pause        .Disable();
        inputActions.OnFoot.Movement     .Disable();
        inputActions.OnFoot.Camera       .Disable();
    }

}
