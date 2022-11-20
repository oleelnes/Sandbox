// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// public class PauseMenu : MonoBehaviour
// {
// //     // Start is called before the first frame update
// //     void Start()
// //     {
        
// //     }

// //     // Update is called once per frame
// //     void Update()
// //     {
        
// //     }
	
	
	
// //     private static void tryGetPauseMenuObject(bool expected)
// //     {
// //         if (GameObject.FindGameObjectWithTag("PauseMenu"))
// //         {
// //             pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").transform.GetChild(0).gameObject;

// //             pauseMenu.SetActive(false);
// //         }
// //         else
// //         {
// //             if (expected) Debug.LogWarning("<InputManager> \tGameObject with tag \"PauseMenu\" not found! Can't use pause menu, the prefab needs to be enabled in the scene when playing.");
// //         }
// //     }
// //     private static bool inScenario()
// //     {
// //         bool inScenario = true;
// //         var sceneName = SceneManager.GetActiveScene().name;
// //         var nonScenarioSceneNames = new string[] { "M_Main", "M_Rebind" };
// //         for (int i = 0; i < nonScenarioSceneNames.Length; i++)
// //         {
// //             if (sceneName == nonScenarioSceneNames[i]) inScenario = false;
// //         }
// //         return inScenario;
// //     }

// //     /**********************************************************************\
// //     |                      SECTION: INPUT ACTION FUNCTIONS                 |
// //     \**********************************************************************/
// //     public static void TogglePause()
// //     {
// //         // Only proceed if inside a scenario where the pause menu should be openable
// //         if (inScenario())
// //         {
// //             Debug.Log("<InputManager> \tPause called, pausedvariable=" + paused + " & activeMap=" + (inputActions.Player.enabled ? "player" : "train"));
// //             if (paused) {
// //                 Unpause();
// //             } else {
// //                 Pause();
// //             }
// //         }
// //     }
// //     public static void Pause()
// //     {
// //         Debug.Log("<InputManager> \tPausing");
// //         // Pause
// //         paused = true;
// //         Time.timeScale = 0;

// //         // Try to get and activate pause menu
// //         if (pauseMenu)
// //         {
// //             pauseMenu.SetActive(true);
// //         }
// //         else
// //         {
// //             tryGetPauseMenuObject(true);
// //         }

// //         // Disable all inputs
// //         actionMapEnabledBeforePause = (inputActions.Player.enabled ? "player" : "none");
// //         inputActions.Train.Disable();
// //         inputActions.Player.Disable();

// //         // Re-enable "pause" input for current action map
// //         if (actionMapEnabledBeforePause == "player")
// //         {
// //             Debug.Log("<InputManager> \tPlayer action map was enabled before pausing, enabling the Menu input");
// //             inputActions.Player.Menu.Enable();
// //         }
// //     }
// //     public static void Unpause()
// //     {
// //         Debug.Log("<InputManager> \tUnausing");
// //         // Unpause
// //         paused = false;
// //         Time.timeScale = 1;

// //         // Try to get and deactivate pause menu
// //         if (pauseMenu != null)
// //         {
// //             pauseMenu.SetActive(false);
// //             // pauseMenu.transform.localScale.Set(0, 0, 0);
// //         }
// //         else
// //         {
// //             tryGetPauseMenuObject(true);
// //         }

        
// // 		Debug.Log("<InputManager> \tPlayer action map was enabled before pausing, enabling the player action map");
// // 		inputActions.OnFoot.Enable();
	
// //     }

	
	
	
	
// }
