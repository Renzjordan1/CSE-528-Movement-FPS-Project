using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AudioSource))]
public class BallScript : MonoBehaviour 
{
	public float targetThresh = 45.0f;
    public float canThresh = 5.0f;

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }
    
    void OnCollisionEnter(Collision collisionInfo)  // Collision class includes info for physics interaction
                                                    // OnTriggerEnter (Collider collider)
	{
		//Debug.Log (Collisioninfo.impactForceSum.magnitude.ToString());

        if (collisionInfo.gameObject.tag == "enemy") //&& 
            //collisionInfo.impactForceSum.magnitude > targetThresh)
		{
            collisionInfo.gameObject.SendMessage("ReactToHit");
            Destroy(gameObject);
        }     
	}

    void OnTriggerEnter(Collider co) // detect if it hit the enemy fire ball which has "is Trigger" on
    {
        if (co.gameObject.tag == "Fire")
        {
            Destroy(co.gameObject); print("destroy fire ball");
            Destroy(gameObject);
        }
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
