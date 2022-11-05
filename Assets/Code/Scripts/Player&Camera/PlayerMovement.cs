using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    private float walkSpeed = 7;
    private float sprintSpeed = 14;
    private float superSprintSpeed = 50;

    
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode superSprintKey = KeyCode.Q;
    public KeyCode crouchKey = KeyCode.LeftControl;

    Ray ray;
    RaycastHit hit;
    bool hitting = false;
    float startTime = 0.0f;
    float hitDuration = 0.0f;
    int trees = 0;
    float progress = 0;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        Sprint();
        Crouch();

        //Destroying/interacting with objects
        checkHit();
       

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void checkHit()
    {
        PlayerCam pC = FindObjectOfType<PlayerCam>();
        pC.GetComponent<Camera>();
        ray = pC.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        
        if(Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            MoveCamera moveCamScript = FindObjectOfType<MoveCamera>();
            GameObject hitObject = hit.collider.gameObject;
            if(Vector3.Distance(moveCamScript.getCameraPosition(), hitObject.transform.position) < 4.0f)
            {
                if(hit.collider.tag == "tree" && !hitting)
                {
                    //Shrinking the object. Will be changed later.
                    //hitObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    startTime = Time.time;
                    hitting = true;
                    Debug.Log("hit an object");
                }
            }
            if (hitting)
            {
                if (((Time.time - startTime) % 10) + 1 >= progress + 1)
                {
                    progress += 1;
                    Debug.Log("hitting an object, progress: " + progress * 10 + "%");

                }
                if (Time.time - startTime > 10)
                {
                    trees++;
                    Debug.Log("you have " + trees + " trees.");
                    hitObject.tag = "deleteTree";
                    hitting = false;
                }
            }
        }
        else if (hitting && !Input.GetMouseButtonDown(0) && !Physics.Raycast(ray, out hit))
        {
            Debug.Log("stopped hitting");
            hitting = false;
        }
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Sprint() {
        if (Input.GetKey(sprintKey)) {
            moveSpeed = sprintSpeed;
        } 
        else if(Input.GetKey(superSprintKey))
        {
            moveSpeed = superSprintSpeed;
        }
        else {
            moveSpeed = walkSpeed;
        }
        
    }

    private void Crouch() {
        if (Input.GetKey(crouchKey)) {
            transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
        } else {
            transform.localScale = new Vector3(0.8f, 1f, 0.8f);
        }

    }
}