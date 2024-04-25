using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] TMP_Text speed_txt;

    [Header("Movement")]
    float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float crouchSpeed;
    public float wallRunSpeed;

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
    float crouchTime;

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

<<<<<<< HEAD
    [Header("Enemy")]
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public GameObject explosion;
    public float explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //Sounds
    [Header("Audio")]
    [SerializeField] public AudioClip poundSound;
    private AudioSource audioSource;
    public float audioVolume;


=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    [Header("Transforms")]
    public Transform orientation;
    public Transform playerObj;

<<<<<<< HEAD

=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
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
            pound,
            wallrunning
    }

    public bool sliding;
    bool longJump;
    public bool wallrunning;
<<<<<<< HEAD
    bool hitEnemy;
    bool exploded;
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46

    public MovementState state;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = playerObj.localScale.y;
<<<<<<< HEAD

        audioSource = GetComponent<AudioSource>();
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
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
        if(grounded || wallrunning)
        {
            // Debug.Log("GROUNDED HI");
            rb.drag = groundDrag;
        }else
        {
            rb.drag = 0;
        }

<<<<<<< HEAD
        //allow ground pound enemy
        if(state != MovementState.pound)
        {
            hitEnemy = false;
            exploded = false;
        }

=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
        //Buffer a little before checking to reset longJump
        Invoke(nameof(ResetLongJump), jumpCooldown*2);


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
            crouchTime = 0;
        }

        //stop crouch or slide
        if(Input.GetKeyUp(crouchKey))
        {
            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
            crouchTime = 0;
        }


    }

    void StateHandler()
    {
<<<<<<< HEAD
        Debug.Log("State: " + state);
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
        //Mode - Wallrunning
        if(wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
        }

        //Mode - Ground Pound
        else if(Input.GetKeyDown(crouchKey) && !grounded)
        {
            state = MovementState.pound;
            moveSpeed = 0;
        }

        //Crouch
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
                crouchTime += Time.deltaTime * 4f;
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

        //record speed
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
        }

        //on air
        else if(!grounded)
        {
            //add more gravity
            rb.AddForce(Physics.gravity/2f, ForceMode.Acceleration);

            //ground pounding
            if(state == MovementState.pound)
            {
<<<<<<< HEAD

                //Hit Enemy once and explode
                if(Physics.Raycast(transform.position, Vector3.down, out rayHit, playerHeight * 0.5f + 1f, whatIsEnemy) || Physics.Raycast(transform.position, Vector3.down, out rayHit, playerHeight * 0.5f + 1f, whatIsGround))
                {
                    // Debug.Log("pounded: " + rayHit.collider.name);
                    if (rayHit.collider.CompareTag("enemy") && !hitEnemy)
                    {
                        rayHit.collider.GetComponent<ReactiveTarget3>().ReactToHit(100f);
                        hitEnemy = true;
                    }
                    if(!exploded)
                    {   
                        Explode();
                        exploded = true;
                    }
                }

=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                rb.AddForce(Vector3.down * 120f, ForceMode.Force);
            }

<<<<<<< HEAD

=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
            //long jumping
            else if(longJump)
            {
                Debug.Log("move long hi: " + longJump);
                rb.AddForce(moveDirection.normalized * moveSpeed * 100f, ForceMode.Force);
            }

            //in air
            else{
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

        //turn off gravity while on slope
        if(!wallrunning) rb.useGravity = !OnSlope();
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

            //limit long jump speed
            if(longJump)
            {
                if(flatVel.magnitude > moveSpeed * 2)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }

            //limit velocity
            else if(flatVel.magnitude > moveSpeed)
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


        //long jump
        if(Input.GetKey(crouchKey) && moveSpeed > 5 && state != MovementState.sliding){
            transform.position = transform.position + new Vector3(0, 1f, 0);
            rb.AddForce(transform.up * jumpForce*0.7f, ForceMode.Impulse);

            longJump = true;
        }

        //backflip (high) jump
        else if(Input.GetKey(crouchKey) && moveSpeed < 5 && state != MovementState.sliding)
        {
            if(crouchTime > 1){
                crouchTime = 1;
            }
            // Debug.Log("crouch time: " + crouchTime);
            rb.AddForce(transform.up * jumpForce * (1 + crouchTime * 0.6f), ForceMode.Impulse);
            crouchTime = 0;
        }

        //regular jump
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

     void ResetLongJump()
    {
        //ground check
        bool grounded2 = Physics.Raycast(transform.position, Vector3.down, playerObj.localScale.y + 0.3f, whatIsGround);

        if(grounded2)
        {
            Debug.Log("GROUND HI");
            longJump = false;
        }
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
<<<<<<< HEAD
        // Debug.Log("speed: " + moveSpeed);
        speed_txt.text = "speed: " + moveSpeed;
    }

    
    void Explode()
    {
        AudioSource.PlayClipAtPoint(poundSound, transform.position, audioVolume);

        Debug.Log("Explode impact: " + rb.velocity.magnitude);
        explosionRange = rb.velocity.magnitude/3;
        explosionDamage = rb.velocity.magnitude * 2;


        //Instantiate explosion
        if(explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemy);
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<ReactiveTarget3>().ReactToHit(explosionDamage);
        }

    }


=======
        Debug.Log("speed: " + moveSpeed);
        speed_txt.text = "speed: " + moveSpeed;
    }

>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
}
