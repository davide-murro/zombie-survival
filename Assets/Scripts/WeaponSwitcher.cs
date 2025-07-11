using System.Collections;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponIndex = 0;
    int previousWeaponIndex;
    bool isSwitching;

    struct Pose
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
    }

    Pose[] initialRootPoses;


    void Awake()
    {
        InitRootPoses();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableAllWeapon();
        SetWeaponActive(currentWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwitching) return;

        previousWeaponIndex = currentWeaponIndex;

        ProcessKeyInput();
        ProcessScrollWheelInput();

        if (previousWeaponIndex != currentWeaponIndex)
        {
            StartCoroutine(SwitchWeaponRoutine(previousWeaponIndex, currentWeaponIndex));
        }
    }

    void InitRootPoses()
    {
        int childCount = transform.childCount;
        initialRootPoses = new Pose[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Transform weapon = transform.GetChild(i);
            initialRootPoses[i] = new Pose
            {
                localPosition = weapon.localPosition,
                localRotation = weapon.localRotation,
                localScale = weapon.localScale
            };
        }
    }

    void RestoreRootPose(int weaponIndex)
    {
        Transform weapon = transform.GetChild(weaponIndex);
        weapon.localPosition = initialRootPoses[weaponIndex].localPosition;
        weapon.localRotation = initialRootPoses[weaponIndex].localRotation;
        weapon.localScale = initialRootPoses[weaponIndex].localScale;
    }

    void DisableAllWeapon()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    IEnumerator SwitchWeaponRoutine(int prevWeaponIndex, int nextWeaponIndex)
    {
        isSwitching = true;
        yield return SetWeaponDeactive(prevWeaponIndex);
        SetWeaponActive(nextWeaponIndex);
        isSwitching = false;
    }

    IEnumerator SetWeaponDeactive(int weaponIndex)
    {
        Transform weapon = transform.GetChild(weaponIndex);

        Animator animator = weapon.GetComponent<Animator>();
        animator.SetTrigger("putAway");

        yield return new WaitWhile(() => animator.IsInTransition(0));
        yield return new WaitWhile(() => (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f));

        weapon.gameObject.SetActive(false);
    }

    void SetWeaponActive(int weaponIndex)
    {
        RestoreRootPose(weaponIndex);

        Transform weapon = transform.GetChild(weaponIndex);
        weapon.gameObject.SetActive(true);

        Animator animator = weapon.GetComponent<Animator>();
        animator.Rebind();
        animator.Update(0f);
        animator.SetTrigger("pullOut");
    }

    void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeaponIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) currentWeaponIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) currentWeaponIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) currentWeaponIndex = 3;
    }

    void ProcessScrollWheelInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeaponIndex >= transform.childCount - 1)
            {
                currentWeaponIndex = 0;
            }
            else
            {
                currentWeaponIndex++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeaponIndex <= 0)
            {
                currentWeaponIndex = transform.childCount - 1;
            }
            else
            {
                currentWeaponIndex--;
            }
        }
    }
}
