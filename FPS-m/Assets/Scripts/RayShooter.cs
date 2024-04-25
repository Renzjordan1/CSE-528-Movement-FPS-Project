using UnityEngine;
using System.Collections;
using TMPro;


public class RayShooter : MonoBehaviour 
{

	private Camera _camera;
    private bool cursorLocked = false;

    [SerializeField] private AudioClip gunShot;
    private AudioSource audioSource;


	void Start() 
    {
		_camera = GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();


        LockCursor();	
	}

/*
	void OnGUI() 
    {
		int size = 12;
		float posX = _camera.pixelWidth/2 - size/4;
		float posY = _camera.pixelHeight/2 - size/2;
		GUI.Label(new Rect(posX, posY, size, size), "*");
	}
*/
	void Update() 
    {
        
		if (Input.GetMouseButtonDown(0) /* || Input.GetButton("Fire1")*/) 
        {
            if (audioSource != null)
            {
                audioSource.clip = gunShot;
                audioSource.Play();
            }
			Vector3 point = new Vector3(_camera.pixelWidth/2, _camera.pixelHeight/2, 0);
			Ray ray = _camera.ScreenPointToRay(point);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) 
            {
				GameObject hitObject = hit.transform.gameObject;
				ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
				if (target != null) 
                {
					target.ReactToHit();
				} else 
                {
                    ReactiveTarget2 target2 = hitObject.GetComponent<ReactiveTarget2>();
                    if (target2 != null)
                    {
                        target2.ReactToHit();

                    }
                    else
                    {
                        ReactiveTarget3 target3 = hitObject.GetComponent<ReactiveTarget3>();
                        if (target3 != null)
                        {
<<<<<<< HEAD
                            target3.ReactToHit(100);
=======
                            target3.ReactToHit();
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    
                        }
                        else
                        {
                            StartCoroutine(SphereIndicator(hit.point));
                        }
                    }
				}
			}
		}

        if (Input.GetKeyDown(KeyCode.C))
            LockCursor();
	}

	private IEnumerator SphereIndicator(Vector3 pos) 
    {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = pos;

        sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); 

		yield return new WaitForSeconds(1);

		Destroy(sphere);
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