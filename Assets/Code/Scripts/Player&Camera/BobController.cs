using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobController : MonoBehaviour
{

    [SerializeField] private bool doBob = true;

    [SerializeField, Range(0, 0.1f)] private float bobStrength = 2.02f;
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
        // Get neutral camera position
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
     * @brief Move camera smoothly to its neutral position  
     **/
    void ResetBob() {
        // If not in neutral position        
        if(cameraTransform.localPosition != cameraNeutralPosition){
            // Lerp step to neutral position        
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraNeutralPosition, 1*Time.deltaTime);   
        }
    }
    
    
    /**
     * @brief Create a bobbing-offset vector, based on current time 
     **/
    private Vector3 BobMotion_Walking(){
        float bobMultiplier = ( affectedBySpeed ? 1+velocity/maxVelocity : 1 );
        float x = Mathf.Cos( Time.time * bobSpeed/2 ) * bobStrength * 2  * bobMultiplier; 
        float y = Mathf.Sin( Time.time * bobSpeed   ) * bobStrength      * bobMultiplier;
        Vector3 result = new Vector3(x, y, 0.0f);
        return result;
    }


    /**
     * @brief Check conditions for head bobbing and apply appropriate headbob 
     *   (Headbob for jumping or falling can be implemented too)
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
                
;
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
     * @brief Calculates the angle between two points, like Vector3.Angle but keeping the polarity sign
     * Yoinked from https://unitynotebook.blogspot.com/2018/07/unity-calculate-360-angle-between-2.html
     */
    public static float AngleFromToPoint(Vector3 fromPoint, 
                                         Vector3 toPoint, 
                                         Vector3 zeroDirection)
    {
        Vector3 targetDirection = toPoint - fromPoint;
        float sign = Mathf.Sign(zeroDirection.x * targetDirection.y - zeroDirection.y * targetDirection.x);
        return Vector3.Angle(zeroDirection, targetDirection) * sign;
    } 


    /**
     * @brief Move camera to an offset of its neutral position
     * @param offset {Vector3} - The offset to apply to the camera
     * @note Currently contains a lot of unused code for left-and-right bobbing, I tried to rotate a 2d plane
     *       so that the left&right offset would align with the direction of the camera.
     **/
    private void ApplyOffsetFromNeutral(Vector3 offset){
        
        // Create vec3 from neutral
        Vector3 newPos = cameraNeutralPosition;
        // Vector3 newPos = cameraTransform.forward;//cameraNeutralPosition;
        
        
        // // Point offset vec3 in direction of camera
        // Vector2 neutralDir           = Vector2.up; //new Vector2(cameraNeutralPosition.x,     cameraNeutralPosition.z);
        // Vector2 horizontalLookingDir = new Vector2(cameraTransform.forward.x,   cameraTransform.forward.z);
        
        // float angleDiffFromNeutral = AngleFromToPoint(cameraTransform.forward, Vector3.forward, Vector3.up);
        // // float angleDiffFromNeutral = Vector2.Angle(neutralDir, horizontalLookingDir);
    
        
        // // Vector2 jge = Vector2.Transform(horizontalLookingDir, Matrix.CreateRotationX(angleDiffFromNeutral));
        
        // Vector3 offsetPre  = offset;
        // Quaternion rotation = Quaternion.Euler(0, angleDiffFromNeutral, 0);
        // // Vector3 offsetPost = Quaternion.AngleAxis(rotation, Vector3.up) * offset;
        // Vector3 offsetPost = Quaternion.Euler(0, 0, angleDiffFromNeutral) * offset;
        // Debug.Log("Change: "+offsetPre+" -> "+offsetPost+"   (anglediff "+angleDiffFromNeutral+"   <= neutral:"+neutralDir+" x "+horizontalLookingDir+")");
        
        // Debug.DrawLine(cameraHolderTransform.position, cameraTransform.forward  * 100, Color.blue);
        // Debug.DrawLine(cameraHolderTransform.position, offset                   * 100, Color.red);
        // Debug.DrawLine(cameraHolderTransform.position, offsetPost               * 100, Color.orange);
        
        
        // float angleDiff = Vector2.Angle(horizontalLookingDir, neutralDir);
        // offset = Quaternion.AngleAxis(angleDiff, Vector3.up) * offset;
        
        // Vector3 v = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z) - cameraNeutralPosition;
        // offset = Quaternion.FromToRotation(Vector3.right, v) * offset;
        
        
        // Apply offset
        // newPos.x += offsetPost.x;   
        // newPos.y += offsetPost.y;
        // newPos.z += offsetPost.z;
        
        newPos.y += offset.y;

        // Move camera
        cameraTransform.localPosition = newPos;
    }
    


}
