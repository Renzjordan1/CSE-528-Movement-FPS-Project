using UnityEngine;
using System.Collections;

public class ReactiveTarget3 : MonoBehaviour 
{
    //private ParticleSystem blood;
<<<<<<< HEAD
    // private MeshRenderer flick;
=======
    private MeshRenderer flick;
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    private bool dead = false;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip shrinking;
    private AudioSource audioSource;
<<<<<<< HEAD
    public float health = 100;

=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

<<<<<<< HEAD
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
=======
    public void ReactToHit() 
    {
		WanderingAI behavior = GetComponent<WanderingAI>();
		if (behavior != null) 
        {
			behavior.SetAlive(false);
		}
        if (!dead)
        {
            if (audioSource != null) PlayHitSound();
            StartCoroutine(Die()); // dying animation
            
            GameObject em = GameObject.Find("EnemyManager");
            EnemyManager2 emScript = em.GetComponent<EnemyManager2>();
            emScript.numEnemies -= 1;

            GameObject ply = GameObject.Find("Player");
            PlayerCharacter pcScript = ply.GetComponent<PlayerCharacter>();
            pcScript.updateKills();
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
        }
	}

    private void PlayHitSound()
    {
        audioSource.clip = enemyHit;
<<<<<<< HEAD
        audioSource.volume = 0.03f;
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
        audioSource.Play();
    }

    private void PlayShrinkSound()
    {
        audioSource.clip = shrinking;
<<<<<<< HEAD
        audioSource.volume = 0.015f;
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
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
<<<<<<< HEAD


=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
}
