using Unity.Cinemachine;
using UnityEngine;

public enum FireMode
{
    Single,
    Auto
}

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;   // First person camera

    [SerializeField] FireMode fireMode = FireMode.Single;
    [SerializeField] float fireRate = 0.1f;

    [SerializeField] float range = 100f;

    [SerializeField] float damage = 20f;

    [SerializeField] ParticleSystem muzzleFlash;    // particle for the shooting
    [SerializeField] GameObject bulletImpact;    // target impact effect
    [SerializeField] CinemachineImpulseSource impulseSource;    // impulse for recoil
    [SerializeField] Animator animator;

    float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // aim
        if (Input.GetButton("Fire2"))
        {
            AimIn();
        }
        else
        {
            AimOut();
        }

        // shoot
        if (fireMode == FireMode.Single && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (fireMode == FireMode.Auto && Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    void AimIn()
    {
        //animator.SetBool("isAiming", true);
        animator.SetLayerWeight(1, 1f);

    }
    void AimOut()
    {
        //animator.SetBool("isAiming", false);
        animator.SetLayerWeight(1, 0f);
    }

    void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            animator.SetTrigger("shoot");
            ProcessRaycast();
            ProcessRecoil();
            PlayMuzzleFlash();
            nextFireTime = Time.time + fireRate;
        }
    }

    void ProcessRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit this thing: " + hit.transform.name);

            // if i hit something
            CreateHitImpact(hit);

            // if has helth
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    void ProcessRecoil()
    {
        if (impulseSource)
        {
            impulseSource.GenerateImpulse();
        }
    }

    void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    void CreateHitImpact(RaycastHit hit)
    {
        if (bulletImpact)
        {
            GameObject impact = Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.1f);
        }
    }
}
