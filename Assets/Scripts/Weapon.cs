using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public enum FireMode
{
    Single,
    Auto
}

public class Weapon : MonoBehaviour
{
    [Header("Shoot / Fire")]
    [SerializeField] ParticleSystem muzzleFlash;    // particle for the shooting
    [SerializeField] GameObject bulletImpact;    // target impact effect
    [SerializeField] FireMode fireMode = FireMode.Single;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 20f;

    [Header("Aim / Zoom")]
    [SerializeField] float defaultZoom = 40f;
    [SerializeField] float aimZoom = 30f;
    [SerializeField] Canvas crossHairCanvas;

    [Header("Ammo")]
    [SerializeField] Ammo ammoSlot;    // ammo slot
    [SerializeField] AmmoType ammoType;    // ammo type it needs
    [SerializeField] int ammoPerShoot = 1;
    [SerializeField] TextMeshProUGUI ammoTextbox;

    CinemachineCamera cinemachineCamera;
    CinemachineImpulseSource impulseSource;    // impulse for recoil
    WeaponMagazine weaponMagazine;    // magazine ammo
    Animator animator;

    [HideInInspector] public bool canShoot = true;

    float nextFireTime = 0f;
    bool isAiming = false;
    float currentWeight = 0f;
    float aimTransitionSpeed = 5f;

    void Awake()
    {
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        weaponMagazine = GetComponent<WeaponMagazine>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // aim logig
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
        UpdateAim();

        // shooting logic
        if (fireMode == FireMode.Single && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (fireMode == FireMode.Auto && Input.GetButton("Fire1"))
        {
            Shoot();
        }

        // reload logic
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // ammo
        DisplayAmmo();
    }

    void UpdateAim()
    {
        float targetWeight;
        float targetAimZoom;

        if (isAiming)
        {
            targetWeight = 1f;
            crossHairCanvas.gameObject.SetActive(false);
            targetAimZoom = aimZoom;
        }
        else
        {
            targetWeight = 0f;
            crossHairCanvas.gameObject.SetActive(true);
            targetAimZoom = defaultZoom;
        }

        currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, aimTransitionSpeed * Time.deltaTime);
        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.Lens.FieldOfView, targetAimZoom, aimTransitionSpeed * Time.deltaTime);

        animator.SetLayerWeight(1, currentWeight);
    }

    void Shoot()
    {
        // check if it can shoot
        if (!canShoot) return;

        // check fire rate
        if (Time.time < nextFireTime) return;

        // check ammo
        if (ammoType != AmmoType.None)
        {
            if (weaponMagazine.GetCurrentAmmo() <= 0) return;

            int ammoToUse = ammoPerShoot;
            if (weaponMagazine.GetCurrentAmmo() - ammoToUse < 0) ammoToUse = weaponMagazine.GetCurrentAmmo();
            weaponMagazine.UseAmmo(ammoToUse);
        }

        // animation
        //animator.Play("Gun_shoot", 0, 0f);
        animator.ResetTrigger("shoot");
        animator.SetTrigger("shoot");
        ProcessRaycast();
        ProcessRecoil();
        PlayMuzzleFlash();

        nextFireTime = Time.time + fireRate;
    }

    void Reload()
    {
        if (ammoType != AmmoType.None)
        {
            if (ammoSlot.GetCurrentAmmo(ammoType) <= 0) return;

            int ammoToReload = weaponMagazine.GetCapacity() - weaponMagazine.GetCurrentAmmo();
            if (ammoSlot.GetCurrentAmmo(ammoType) - ammoToReload < 0) ammoToReload = ammoSlot.GetCurrentAmmo(ammoType);
            ammoSlot.ReduceCurrentAmmo(ammoType, ammoToReload);
            weaponMagazine.ReloadAmmo(ammoToReload);
        }
    }

    void ProcessRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(cinemachineCamera.transform.position, cinemachineCamera.transform.forward, out hit, range))
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

    void DisplayAmmo()
    {
        if (ammoType != AmmoType.None)
        {
            int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
            int weaponMagazineAmmo = weaponMagazine.GetCurrentAmmo();
            ammoTextbox.text = $"{weaponMagazineAmmo} | {currentAmmo}";
        }
        else
        {
            ammoTextbox.text = "";
        }
    }
}
