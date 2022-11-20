// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using System;


// public class ActionMapManager : MonoBehaviour
// {
//     private static bool debug = false;

//     // Reference to input action asset
//     public static PlayerInputActionsClass inputActions; 
//     public static event Action<InputActionMap> actionMapChanged;

//     // Rebinding stuff
//     public static event Action rebindComplete;
//     public static event Action rebindCancelled;
//     public static event Action<InputAction, int> rebindStarted;

//     // User-changeable values
//     public static bool invertLook = false;

//     // Pausing
//     private static string actionMapEnabledBeforePause = "";
//     private static GameObject pauseMenu;
//     public static bool paused = false;
//     public static bool IsPaused {   get { return paused; }
//                                     set { } }


//     /**
//      * Called before the first frame
//      **/
//     private void Start()
//     {
//         // Disable all action maps
//         // inputActions.Disable(); // Disable all action maps
        
//         // Set action map        
//         // SwitchToActionMap(inputActions.OnFoot);
        
//         // GetUIElements();
        
//         // Get pause menu into pauseMenu GameObject variable if in a scenario
//         // if (inScenario()) tryGetPauseMenuObject(true);
//     }



//     /**
//      * Called when awoken
//      **/
//     private void Awake()
//     {
//         // Safety check
//         if (inputActions == null) {
//             inputActions = new PlayerInputActionsClass();
//         }

//         // GetUIElements();
//         // LoadAllBindingOverrides();
//         // if (inScenario()) tryGetPauseMenuObject(false);
//     }



// }
