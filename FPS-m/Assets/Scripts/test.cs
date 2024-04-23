using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization;
using UnityEngine.ProBuilder.MeshOperations;

public class test : MonoBehaviour
{

    [SerializeField] TMP_Text speed_txt;

    [Header("Movement")]
    float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float crouchSpeed;

    //Used for acceleration
    float desiredMoveSpeed;
    float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    //friction
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchYScale;
    float startYScale;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    [Header("Sliding")]
    public float slideForce;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    // public KeyCode slideKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Transforms")]
    public Transform orientation;
    public Transform playerObj;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    //States
    public enum MovementState
    {
            walking,
            sprinting,
            air,
            crouching,
            sliding,
            pound
    }

    public bool sliding;
    bool longJump;

    public MovementState state;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = playerObj.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        // JumpControl();
        StateHandler();

        //handle drag
        if(grounded)
        {
            Debug.Log("GROUNDED HI");
            rb.drag = groundDrag;
        }else
        {
            rb.drag = 0;
        }


    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    //Input
    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //start crouch or slide
        if(Input.GetKeyDown(crouchKey))
        {
            playerObj.localScale = new Vector3(playerObj.localScale.x, crouchYScale, playerObj.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        //stop crouch or slide
        if(Input.GetKeyUp(crouchKey))
        {
            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        }


    }

    void StateHandler()
    {
        //Mode - Ground Pound
        if(Input.GetKeyDown(crouchKey) && !grounded)
        {
            state = MovementState.pound;
            moveSpeed = 0;
        }
        else if(Input.GetKey(crouchKey) && grounded)
        {
            //Mode - Sliding
            if(OnSlope() && rb.velocity.y < 0.1f)
            {   
                state = MovementState.sliding;
                desiredMoveSpeed = slideSpeed;
            }

            //Mode - Crouching
            else
            {
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
            }
        }

        //Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        //Mode - Walking
        else if(grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //Mode - Air
        else
        {
            if(state != MovementState.pound)
            {
                state = MovementState.air;
            }
        }

        //check if desiredMoveSpeed has changed
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 0.1f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else{
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }
    
    IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            //Slope accel
            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }

            //Ground accel
            else
                time += Time.deltaTime * speedIncreaseMultiplier * speedIncreaseMultiplier * (1+speedIncreaseMultiplier);

            printSpeed();
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }


    void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //sliding down slope     
        if (state == MovementState.sliding)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * slideForce, ForceMode.Force);
        }

        //on slope
        else if(OnSlope() && !exitingSlope)
        {
            Debug.Log("move slope");
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //on ground
        else if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            Debug.Log("GROUND HI");
            longJump = false;
        }

        //on air
        else if(!grounded)
        {
            Debug.Log("move hi: " + longJump);
            if(longJump)
            {
                 rb.AddForce(moveDirection.normalized * moveSpeed * 75f, ForceMode.Force);
            }
            //in air
            else if(state != MovementState.pound)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }

            //ground pounding
            else{
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                rb.AddForce(Vector3.down * 120f, ForceMode.Force);
            }
        }

        //turn off gravity while on slope
        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        //limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        //limiting speed on ground or air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        if(Input.GetKey(crouchKey) && moveSpeed > 3){
            longJump = true;
            transform.position = transform.position + new Vector3(0, 2f, 0);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        //Check if on a slope
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        //Project movement on slope
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    void printSpeed()
    {
        Debug.Log("speed: " + moveSpeed);
        speed_txt.text = "speed: " + moveSpeed;
    }

}
