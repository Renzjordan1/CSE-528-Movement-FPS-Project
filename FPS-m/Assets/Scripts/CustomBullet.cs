using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemy;

    [Header("Stats")]
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;
    public float gravityMultiplier = 0;

    [Header("Lifetime")]
    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;
    public bool explodable = true;

    //Sounds
    [Header("Sounds")]
    [SerializeField] public AudioClip explodeSound;
    private AudioSource audioSource;
    public float audioVolumeExplode;


    int collisions;
    PhysicMaterial physicsMat;
    bool exploding;


    // Start is called before the first frame update
    void Start()
    {
        Setup();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!exploding)
        {
            //When to explode
            if(collisions > maxCollisions && explodable) Explode();

            //count down lifetime
            maxLifeTime -= Time.deltaTime;
            if(maxLifeTime <= 0 && explodable) Explode();
        }

    }

    void Setup()
    {
        //Create new Physic material
        physicsMat = new PhysicMaterial();
        physicsMat.bounciness = bounciness;
        physicsMat.frictionCombine = PhysicMaterialCombine.Minimum;
        physicsMat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physicsMat;

        //Add gravity
        rb.useGravity = useGravity;
        rb.AddForce(Physics.gravity*gravityMultiplier, ForceMode.Acceleration);
    }

    void Explode()
    {
        //call explode once
        exploding = true;

        //play boom
        AudioSource.PlayClipAtPoint(explodeSound, transform.position, audioVolumeExplode);

        //Instantiate explosion
        if(explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemy);
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<ReactiveTarget3>().ReactToHit(explosionDamage);
        }

        //delayed destroy
        Invoke("Delay", 0.02f);

    }

    void Delay()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if(collision.collider.CompareTag("enemy") && explodeOnTouch && explodable) Explode();

        //Non-explodable collision
        else if(collision.collider.CompareTag("enemy") && !explodable){
            collision.collider.GetComponent<ReactiveTarget3>().ReactToHit(explosionDamage);
        }

        //Non-explodable bounce noise
        else if (!explodable)
            AudioSource.PlayClipAtPoint(explodeSound, transform.position, audioVolumeExplode);



    }

    //draw radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    // private void PlayHitSound()
    // {
    //     audioSource.clip = explodeSound;
    //     audioSource.volume = audioVolumeExplode;
    //     audioSource.Play();
    // }

}
