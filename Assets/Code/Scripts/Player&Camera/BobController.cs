using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobController : MonoBehaviour
{

    [SerializeField] private bool doBob = true;

    [SerializeField, Range(0, 0.1f)] private float bobStrength = 0.02f;
    [SerializeField, Range(0, 50)  ] private float bobSpeed    = 8.00f;

    // Transform element of actual camera 
    [SerializeField] private Transform cameraTransform          = null;
    // Transform element of camera holder, not used yet, move cam relative to?
    [SerializeField] private Transform cameraHolderTransform    = null;
    
                     private Vector3   cameraNeutralPosition;

    // Speed to start bobbing at
    [SerializeField] private float bobbingSpeedTreshold = 3.0f;      
    // To know if the player is grounded
    [SerializeField] private PlayerMovement playerMovementScript;    
    // To get player velocity
    [SerializeField] private Rigidbody playerRigidbody;
    private float velocity = 0.0f;
    
    
    
    //private Vector3 bobOffset = Vector3(0.0f, 0.0f, 0.0f);
    //public  Vector3 GetBobOffset(){ return bobOffset; }

    // Whether bobbing should be intensified based on speed
    [SerializeField] private bool affectedBySpeed = false;
    [SerializeField] private float maxVelocity = 10.0f; 


    // Called at object initialization
    void Awake(){
        
        cameraNeutralPosition = cameraTransform.localPosition;
        
    }



    // Update is called once per frame
    void Update() {
        // If bobbing is enabled
        if(doBob){
            
            // Do bobbing
            BobIfShouldBob();
            // Reset bobbing (maybe add to conditional)
            ResetBob();
            
        }
        
    }
    
    
    /**
     * Move camera smoothly to its neutral position  
     *  
     **/
    void ResetBob() {
        // If not in neutral position        
        if(cameraTransform.localPosition != cameraNeutralPosition){
            // Lerp step to neutral position        
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraNeutralPosition, 1*Time.deltaTime);   
        }
    }
    
    
    /**
     * Get a bobbing vector based on current time 
     *
     **/
    private Vector3 BobMotion_Walking(){
        float bobMultiplier = ( affectedBySpeed ? 1+velocity/maxVelocity : 1 );
        Vector3 result = new Vector3(
            Mathf.Cos( Time.time * bobSpeed/2 ) * bobStrength * 2  * bobMultiplier, 
            Mathf.Sin( Time.time * bobSpeed   ) * bobStrength      * bobMultiplier, 
            0.0f);
        return result;
    }


    /**
     * Check conditions for head bobbing and apply appropriate headbob 
     *  (Headbob for jumping or falling can be implemented too)
     **/
    private void BobIfShouldBob(){
        // Get horizontal movement speed
        //  If implementing falling bobbing, get y velocity too but run different bob
        Vector3 velocityVector = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        velocity = velocityVector.magnitude;
        //velocity = playerRigidbody.velocity.magnitude;
        //Debug.Log(velocity);
        //ApplyOffsetFromNeutral(BobMotion_Walking());
        
        // Check if should bob and do be bobbety bob
        if ( velocity > bobbingSpeedTreshold ) {
            if (playerMovementScript.isGrounded()){
                Debug.Log("walking");
                ApplyOffsetFromNeutral(BobMotion_Walking());
            } /*else 
            if( playerRigidbody.velocity.y > bobbingSpeedTreshold){
                
            }*/ 
            
        } /*else // Maybe do next checks regardless of horizontal speed?
        if ( playerRigidbody.velocity.y > bobbingSpeedTreshold ){
            // Jumping without horizontal movement
            // Lerp camera down to cap
            // ApplyOffsetFromNeutral(jumping)
        } else 
        if ( playerRigidbody.velocity.y < -bobbingSpeedTreshold ){
            // Falling without horizontal movement
            // Lerp camera up to cap
        }*/
    }
    
    
    /**
     * Move camera to an offset of its neutral position
     *
     **/
    private void ApplyOffsetFromNeutral(Vector3 offset){
        
        // Create vec3 from neutral
        Vector3 newPos = cameraNeutralPosition;
        
        // Offset vec3
        newPos.x += offset.x;   
        newPos.y += offset.y;
        newPos.z += offset.z;
        
        // Move camera
        cameraTransform.localPosition = newPos;
        Debug.Log("newpos: "+newPos);
    }
    


}
