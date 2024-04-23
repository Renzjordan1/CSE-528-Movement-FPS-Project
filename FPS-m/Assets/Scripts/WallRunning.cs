using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

// using System.Numerics;

using UnityEngine;
using UnityEngine.Rendering;

public class WallRunning : MonoBehaviour
{

    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    bool upwardsRunning;
    bool downwardsRunning;
    float horizontalInput;
    float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool wallLeft;
    bool wallRight;

    [Header("Exit")]
    bool exitingWall;
    public float exitWallTime;
    float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    PlayerMovement pm;
    Rigidbody rb;
    public PlayerCam cam;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    void FixedUpdate()
    {
        if(pm.wallrunning)
        {
            WallRunningMovement();
        }
    }

    void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    void StateMachine()
    {
        //Getting input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //State 1 - Wallrunning
        if((((wallLeft || wallRight) && verticalInput >= 0)) && AboveGround() && !exitingWall)
        {
            // start wallrun here
            if(!pm.wallrunning && ((wallLeft && Input.GetKey(KeyCode.A)) || (wallRight && Input.GetKey(KeyCode.D))) )
            {
                StartWallRun();
            }

            //wallrun timer
            if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }

            if(wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            //wall jump
            if(Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
        }

        //State 2 - Exiting
        else if(exitingWall)
        {
            if(pm.wallrunning)
            {
                StopWallRun();
            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            else
            {
                exitingWall = false;
            }
        }

        //State 3 - None
        else
        {
            if(pm.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //apply camera effects
        cam.DoFov(70f);
        if(wallLeft) cam.DoTilt(-5f);
        if(wallRight) cam.DoTilt(5f);

    }

    void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //Force towards player's direction
        if((orientation.forward - wallForward).magnitude > (orientation.forward + wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //forward force
        if(verticalInput > 0)
        {
            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        }
        
        //upwards/downwards force
        if(upwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }
        if(downwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
        }

        //push to wall
        if((wallLeft && horizontalInput < 0) || (wallRight && horizontalInput > 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        //weaken gravity
        if(useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }

    }

    void StopWallRun()
    {
        pm.wallrunning = false;

        //reset camera effects
        cam.DoFov(60f);
        cam.DoTilt(0f);


    }

    void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //check forward velocity
        float forwardVelocity = Vector3.Dot(transform.forward, rb.velocity);
        // Debug.Log("Wall: " + forwardVelocity);

        //moving forward on wall, horizontal jump
        if (Mathf.Abs(forwardVelocity) > 0.5f)
        {
            // Debug.Log("Running Wall Jump");
            forceToApply = transform.up * 0.66f * wallJumpUpForce + wallNormal * wallJumpSideForce;
        }

        //not moving on wall, vertical jump
        else if(Mathf.Abs(forwardVelocity) < 0.5f)
        {   
            // Debug.Log("Verticall Wall Jump");
            forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        }

        //reset y velocity and apply force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(forceToApply, ForceMode.Impulse);

    }
}
