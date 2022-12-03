using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SetScene : MonoBehaviour
{
	
	// Used by UI elements to change scenes, like buttons OnClick properties
	
	public void Scene_SetToSceneIndex(int index){
        SceneManager.LoadScene(index);
	}
	
	public void Scene_SetToSceneString(string name){
        SceneManager.LoadScene(name);
	}
	
	
	public void Scene_MainMenu(){
		Debug.Log("Goin home");
		Scene_SetToSceneString("MainMenu");
	}
	
}
