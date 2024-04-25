using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using System;

// [RequireComponent(typeof(Animator))]

public class HitscanGun : MonoBehaviour
{

    //Gun stats
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting;
    public float spread;
    public float range;
    public float reloadTime;
    public float timeBetweenShots;
    public int magazineSize;
    public int bulletsPerTap;
    public bool allowButtonHold;
    public bool singleFire;

    int bulletsLeft;
    int bulletsShot;

    //bools
    bool shooting;
    bool readyToShoot;
    bool reloading;

    float shootingTime = 0;
    float recoverTime = -1f;

    //References
    [Header("References")]
    public Camera fpsCam;
    public Transform attackPoint;
<<<<<<< HEAD
    public Transform attackPoint2;
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    public GameObject bulletHole;
<<<<<<< HEAD
    public GameObject enemyHole;
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    public TextMeshProUGUI ammunitionDisplay;
    public TrailRenderer bulletTrail;
    // Animator animator;

<<<<<<< HEAD
    //Sounds
    [Header("Sounds")]
    [SerializeField] public AudioClip gunShot;
    [SerializeField] public AudioClip reloadSound;
    private AudioSource audioSource;
    public float audioVolumeGun;
    public float audioVolumeReload;

=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    //bug fixing
    public bool allowInvoke = true;

    void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        audioSource = GetComponent<AudioSource>();
=======
        
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        //Set ammo display
        if(ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }

        //Reset recoil when not shooting for certain time

        //if true and timer is not set
        // if(readyToShoot && recoverTime == -1f) recoverTime = Time.time;

        //if false reset timer
        // if(!readyToShoot) recoverTime = -1f;

        if(readyToShoot && (Time.time - recoverTime) > 0.15f) shootingTime -= (Time.deltaTime/4f);

        if(shootingTime <= 0 || (readyToShoot && (Time.time - recoverTime) > .75f)) shootingTime = 0;
  
    }

    void MyInput()
    {
        //Check if allowed to hold down fire
        if(allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);


        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;
        recoverTime = -1f;
        // animator.SetBool("IsShooting", true);

        //Calc spread
        float currSpread = spread;
        // Debug.Log("Spread Time: " + shootingTime);

        //Accurate then increase spread (single bullet fire)
        if(bulletsPerTap == 1)
        {
            currSpread = (shootingTime * 25f) * spread;
            if(currSpread > 1.25f * spread) currSpread = 1.25f * spread;
            Debug.Log("Spread: " + currSpread);
        }

        //Spread fire gets wider (shotgun)
        else
        {
            currSpread = spread * ((shootingTime + 0.05f) * 20f);
            if(currSpread > 1.75f * spread) currSpread = 1.75f * spread;
            Debug.Log("Shotgun Spread: " + currSpread);
        }

        float xSpread = Random.Range(-currSpread, currSpread);
        float ySpread = Random.Range(-currSpread, currSpread);

        //Calc new direction with spread
        Vector3 directionWithSpread = fpsCam.transform.forward + new Vector3(xSpread, ySpread, 0);


        //Find hit position
        if(Physics.Raycast(fpsCam.transform.position, directionWithSpread, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

<<<<<<< HEAD
            //bullet trails
            if(bulletTrail != null)
            {
                TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, rayHit));
            }
            
            TrailRenderer trail2;

            if(attackPoint2 != null)
            {
                trail2 = Instantiate(bulletTrail, attackPoint2.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail2, rayHit));
            }

            //hit enemy
            if (rayHit.collider.CompareTag("enemy"))
            {
                rayHit.collider.GetComponent<ReactiveTarget3>().ReactToHit(damage);
                //enemy impact 
                if(enemyHole != null) Instantiate(enemyHole, rayHit.point, Quaternion.Euler(0, 180, 0));
            }
            else
            {
                //bullet impact
                if(bulletHole != null) Instantiate(bulletHole, rayHit.point, Quaternion.Euler(0, 180, 0));
            }
        }

        //muzzle flash
        if(muzzleFlash != null) 
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            if(attackPoint2 != null)
                Instantiate(muzzleFlash, attackPoint2.position, Quaternion.identity);


        //count shots
=======
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHit));

            //hit enemy
            if(rayHit.collider.CompareTag("Enemy"))
            {
                // rayHit.collider.GetComponent<ShootingAI>().TakeDamage(damage);
            }
        }


        //Instantiate graphics
        //muzzle flash
        if(muzzleFlash != null) Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //bullet impact 
        if(bulletHole != null) Instantiate(bulletHole, rayHit.point, Quaternion.Euler(0, 180, 0));

>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
        bulletsLeft--;
        bulletsShot++;

        //Delay shots
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
<<<<<<< HEAD
            
            //gunshot sound
            PlayHitSound();
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
        }

        //many bulletsPerTap
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

    }

    void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
        shootingTime += Time.deltaTime;

        if (recoverTime == -1f) recoverTime = Time.time;
    }

    void Reload()
    {
<<<<<<< HEAD
        //reload sound
        PlayReloadSound();
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }


    public IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        // animator.SetBool("IsShooting", false);
        trail.transform.position = hit.point;
        
        Destroy(trail.gameObject, trail.time);
    }
<<<<<<< HEAD

    private void PlayHitSound()
    {
        audioSource.clip = gunShot;
        audioSource.volume = audioVolumeGun;
        audioSource.Play();
    }

    private void PlayReloadSound()
    {
        audioSource.clip = reloadSound;
        audioSource.volume = audioVolumeReload;
        audioSource.Play();
    }
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
}
