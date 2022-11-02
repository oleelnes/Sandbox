using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    /**
    * Resume button, unpauses the game, locks the cursor and makes it invisible.
    **/
    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /**
    * Brings up the pause menu, sets the timescale to 0, and unlocks the cursor.
    **/
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /**
    * Changes the scene to the main menu scene.
    **/
    void LoadMenu()
    {
        Debug.Log("Loading Menu");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /**
    * Quits the application, and logs the button press to the console.
    **/
    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
