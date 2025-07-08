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

    bool isAiming = false;
    float currentWeight = 0f;
    float animationTransitionDuration = 0.2f;
    float aimTransitionSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aimTransitionSpeed = 1f / animationTransitionDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // aim
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        Aim();

        // shooting logic
        if (fireMode == FireMode.Single && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (fireMode == FireMode.Auto && Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    void Aim()
    {
        float targetWeight;

        if (isAiming)
        {
            targetWeight = 1f;
        }
        else
        {
            targetWeight = 0f;
        }

        currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, aimTransitionSpeed * Time.deltaTime);

        animator.SetLayerWeight(1, currentWeight);
    }

    void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            animator.Play("Gun_shoot", 0, 0f);
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
