using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour 
{
	public float speed = 10.0f;
	public int damage = 1;

    void Start()
    {
        StartCoroutine("SelfDestruct");
    }

	void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Health player = other.transform.parent.gameObject.GetComponent<Health>();
            if (player != null) 
            {
                player.DamagePlayer(damage);
            }
            Destroy(this.gameObject);
        }
	}
    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
