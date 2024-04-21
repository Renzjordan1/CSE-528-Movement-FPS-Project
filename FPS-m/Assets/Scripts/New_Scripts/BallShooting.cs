using UnityEngine;
using System.Collections;

public class BallShooting : MonoBehaviour 
{
    public GameObject Ball;
    public float Force = 50.0f;
    public Vector3 Torque = new Vector3(100, 0, 0);

    //private Gun _gun;

    private bool cursorLocked = false;

	void Start() 
    {
        LockCursor();

        //_gun = GameObject.Find("Gun").GetComponent<Gun>();
    }

	void Update() 
    {

        //check if the gun is ready to fire
        //if (_gun.ReadyToFire())
        {
            if (Input.GetMouseButtonDown(0))
            {
                //call Bang method to perform gun animation and sounds
                //_gun.Bang();

                // Note: transform.position returns object's position in the World space
                GameObject ball = (GameObject)Instantiate(Ball, transform.position, 
                    Quaternion.identity);
                Rigidbody ball_rb = ball.GetComponent<Rigidbody>();
                ball.name = "ball";
                // Fire the ball 2 unit forward from the camera
                ball.transform.position = transform.TransformPoint(2*Vector3.forward);
                //ball_rb.velocity = transform.TransformDirection(new Vector3(0, 0, Force));
                ball_rb.AddForce(transform.TransformDirection(new Vector3(0, 0, Force)));
                ball_rb.AddTorque(Torque);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
            LockCursor();
	}

    void LockCursor()
    {
        if (!cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorLocked = false;
        }
    }
}