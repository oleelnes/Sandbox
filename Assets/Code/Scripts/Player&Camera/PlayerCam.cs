using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public DynamicInventoryDisplay backpackPanel; 
    public PlayerInventoryHolder abc; 
    public static bool isBackpackOpen = false; 

    
    private Vector2 input_CameraVec;
    public void UpdateInput_Camera(Vector2 delta){
        input_CameraVec = delta;
    }



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBackpackOpen) {
            float mouseX = input_CameraVec.x * Time.deltaTime * sensX;
            float mouseY = input_CameraVec.y * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Applying rotation to object this script is attached to (camera)
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            // Applying rotation to given transform (Empty CameraPos object under Player)
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


    }
    
    
    
}
