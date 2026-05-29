using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    //shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //spread
    public float spreadIntensity;



    // bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f; //seconds

    public GameObject muzzleEffect;
    private Animator animator;




    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            //holding down left mouse button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // clicking left mouse button once 
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        //instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //destroy the bullet after some time 
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //check if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //burst mode
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) //we already shoot once before this
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }


    public Vector3 CalculateDirectionAndSpread()
    {
        // shooting from the middle of the screen to check where we are pointing at 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //hitting something
            targetPoint = hit.point;
        }
        else
        {
            //shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint- bulletSpawn.position;
        
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //returning the shooting direction and speed
        return direction + new Vector3(x, y, 0);
    }






    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
