using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    public float moveSpeed;
    public bool isSprinting = false;
    public bool isSuperSprinting = false;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;


    private float walkSpeed = 7;
    private float sprintSpeed = 14;
    private float superSprintSpeed = 7;

    // Input related
    private bool sprintKey;
    
    //[Header("Keybinds")]
    //public KeyCode jumpKey = KeyCode.Space;
    //public KeyCode sprintKey = KeyCode.LeftShift;
    //public KeyCode superSprintKey = KeyCode.Q;
    //public KeyCode crouchKey = KeyCode.LeftControl;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround; // baby don't hurt me
    public bool grounded;
    public bool isGrounded(){ return grounded; }


    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    bool spawned;
    EndlessTerrain world;

    Vector3 moveDirection;

    public Rigidbody rb;



    /*****************\
    | Input variables |
    \*****************/
    private Vector2 input_MovementVec;

    // TODO Could do these sfx in this file if modularity isn't a priority, or use these scripts statically
    [SerializeField]
    private PlayCrouchSound playCrouchSoundScript;
    [SerializeField]
    private PlayJumpSound playJumpSoundScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        moveSpeed = walkSpeed;
        spawned = false;
        world = FindObjectOfType<EndlessTerrain>();
    }

    private void Update()
    {

        
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        SpeedControl();
        //Destroying/interacting with objects       

        // handle drag
        if (grounded) {
            rb.drag = groundDrag;
        } else {   
            rb.drag = 0;
        }

        SpawnPlayer();
    }


    private void FixedUpdate() 
    {
        MovePlayer();
    }

    private void SpawnPlayer()
    {
        if (!spawned && world.GetHeight(10, 10) > -5)
        {
            transform.position = new Vector3(10, world.GetHeight(10, 10) + 2, 10);
            spawned = true;
        }
    }
    


    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * input_MovementVec.y + orientation.right * input_MovementVec.x;
        
        // Apply different movement force depending on if the player is grounded 
        if(grounded){
            // on ground
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        } else 
        if(!grounded) {
            // in air
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


    
    
    /***************************************\
        Methods called by Actions_OnFoot
    \***************************************/
    public void DoJump(){
        if (readyToJump && grounded){
            readyToJump = false;
            // Reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            // Add vertical force
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            // Queue resetjump
            Invoke(nameof(ResetJump), jumpCooldown);
            playJumpSoundScript.playJumpAudio();
        }
    }
    public void DoCrouch(){
            transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
            playCrouchSoundScript.PlayCrouchAudio();
    }
    public void ReleaseCrouch(){
            transform.localScale = new Vector3(0.8f, 1f, 0.8f);
    }
    
    public void DoSprint(){
        moveSpeed = sprintSpeed;
        isSprinting = true;
    }
    public void ReleaseSprint(){
        moveSpeed = walkSpeed;
        isSprinting = false;
        isSuperSprinting = false;
    }
    public void UpdateInput_Movement(Vector2 input){
        input_MovementVec = input;
    }
    
    // public void DoAttack(){
    //     Debug.Log("DoAttack called");
    // }
    public void DoInteract(){
        Debug.Log("DoInteract called");
    }
    // public void DoInventory(){
    //     Debug.Log("DoInventory called");
    // }

        
    
}