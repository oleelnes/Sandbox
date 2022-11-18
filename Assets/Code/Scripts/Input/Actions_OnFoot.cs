using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Actions_OnFoot : MonoBehaviour
{

    /*************************************\
    | Pointers to scripts called by input |
    \*************************************/
    [SerializeField] private PlayerMovement playerMovementScript; ///< Player movement script to call when actions are taken
    [SerializeField] private PlayerCam playerCameraScript;
    [SerializeField] private MeleeAttack playerMeleeAttackScript;
    [SerializeField] private PauseMenu pauseMenu; ///< Reference to relevant instance of pauseMenu script in scene (Could alternatively just search for it, but this is more robust)
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
    // private ActionMapManager actionMapManager;

    private bool debugVerbose = false;
    
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
    [SerializeField]
    private bool debug = false;
    [SerializeField]
    private bool verboseDebug = false;



    /********************\
    | Input action asset |
    \********************/
    private PlayerInputActionsClass inputActions; // Local instance of input action asset, from ActionMapManager's instance if possible



    /************************\
    | Updating polled values |
    \************************/    
    private void FixedUpdate(){
        // Could check if the variables are changed before updating, but I think calculating the magnitude of a vector is more performance intensive than just updating a variable
        playerMovementScript.UpdateInput_Movement(input_movement);
        playerCameraScript  .UpdateInput_Camera(  input_cameraDelta  );
       // Updating input in player movement script (because this script calls the player movement script, not the other way around)
    }
    



    /**
     * @brief Called when object is enabled, initializes actions.
     **/
    private void OnEnable()
    {
        
        if (debug) Debug.Log("<Actions OnFoot> \tOnEnable called");

        // if (inventoryUIController == null)   inventoryUIController = new InventoryUIController();
        // if (inventorySystem == null)         inventorySystem = new InventorySystem(inventorySize);

        // Check if inputActions not initialized yet
        // if (ActionMapManager.inputActions != null  &&  inputActions == null)  {
        //     // Get input actions from parent ActionMapManager if possible
        //     inputActions = ActionMapManager.inputActions;   // Initialize user input actions
        //     Enable();                                       // Enable action map 
        // } else 
        if (inputActions == null) {                  
            // Create local input actions if no ActionMapManager 
            inputActions = new PlayerInputActionsClass();
            Enable();   // Enable action map 
        } else {
            Debug.Log("<Actions OnFoot> \tOnEnable, input actions already initialized locally at time of enabling.");
        }
        
    }



    /**
     * Enables and binds action inputs
     **/
    private void Enable()
    {
        hasBeenEnabled = true;
        if (debug) Debug.Log("<Actions OnFoot> \tEnable() called");


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
        if (debugVerbose) Debug.Log("<Actions OnFoot> \tOnDisable called");
        Disable();
    }
    
    
    /**
     * Disables all input actions.
     **/
    private void Disable() {
        if (debug) Debug.Log("<Actions OnFoot> \nDisable called");
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
