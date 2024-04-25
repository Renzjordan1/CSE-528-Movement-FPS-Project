using UnityEngine;
using System.Collections;

public class WanderingAI : MonoBehaviour 
{
	public float speed = 3.0f;  // Wandering forward speed
	public float obstacleRange = 2.0f;

    public float Force = 50.0f;
    public Vector3 Torque = new Vector3(100, 0, 0);


    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;

    private bool _alive;

    void Start()
    {
        _alive = true;
    }

    void Update() 
    {
        if (!_alive) return; // this enemy may die before this enemy game object is destroyed

        GetComponent<Rigidbody>().velocity = new Vector3(0, -9.8f, 0);


        Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
        if (Physics.SphereCast(ray, 0.33f, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.tag == "Player")
            {
                if (_fireball == null)
                {
                    _fireball = Instantiate(fireballPrefab);
                    _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                    _fireball.transform.rotation = transform.rotation;
                    _fireball.GetComponent<Rigidbody>().velocity =
                                transform.TransformDirection(new Vector3(0, 0, Force));
                    _fireball.GetComponent<Rigidbody>().AddTorque(Torque);
                }
            }
            // else // && hitObject.tag != "Fire")
            // {
            //     GameObject go1 = GameObject.Find("Player");
            //     // float angle = Random.Range(-110, 110);
            //     // transform.Rotate(0, angle, 0);
            //     if (go1 != null){
            //         Transform pTransform = go1.transform;
            //         transform.LookAt(pTransform);
            //     }
                
            // }
        }

        GameObject go = GameObject.Find("Player");
        // float angle = Random.Range(-110, 110);
        // transform.Rotate(0, angle, 0);
        if (go != null){
            Transform pTransform = go.transform;
            transform.LookAt(pTransform);
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }
}
