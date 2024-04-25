using UnityEngine;
using System.Collections;

public class ReactiveTarget3 : MonoBehaviour 
{
    //private ParticleSystem blood;
    // private MeshRenderer flick;
    private bool dead = false;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip shrinking;
    private AudioSource audioSource;
    public float health = 100;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReactToHit(float damage) 
    {
		WanderingAI behavior = GetComponent<WanderingAI>();

        //take damage
        health -= damage;
        Debug.Log("Pounded by: " + damage);

        if (!dead)
        {
            //hurt sound
            if (audioSource != null) PlayHitSound();

            //died
            if(health <= 0)

            {
                if (behavior != null) 
                    behavior.SetAlive(false);

                StartCoroutine(Die()); // dying animation
                
                //track enemy count
                GameObject em = GameObject.Find("EnemyManager");
                EnemyManager2 emScript = em.GetComponent<EnemyManager2>();
                emScript.numEnemies -= 1;

                // GameObject ply = GameObject.Find("Player");
                // PlayerCharacter pcScript = ply.GetComponent<PlayerCharacter>();
                // pcScript.updateKills();
            }
        }
	}

    private void PlayHitSound()
    {
        audioSource.clip = enemyHit;
        audioSource.volume = 0.03f;
        audioSource.Play();
    }

    private void PlayShrinkSound()
    {
        audioSource.clip = shrinking;
        audioSource.volume = 0.015f;
        audioSource.Play();
    }

    private IEnumerator Die()
    {
        dead = true;        
        int i = 0;
        int j = 0;
        int speed = 10;
        float shrinkSpeed = 1f;
        while (i < 15)
        {            
            this.transform.Rotate(-6, 0, 0);
            this.transform.Translate(0, -0.0333f, 0, Space.World);
            i++;
            yield return new WaitForFixedUpdate();
        }
        PlayShrinkSound();
        while (this.transform.localScale.x > 0.01f)
        {
            this.transform.Rotate(0, 0, j*speed);
            this.transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;
            j++;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }


}
