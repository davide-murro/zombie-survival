using System.Collections;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponIndex = 0;
    int previousWeaponIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableAllWeapon();
        SetWeaponActive();
    }

    // Update is called once per frame
    void Update()
    {
        previousWeaponIndex = currentWeaponIndex;

        ProcessKeyInput();
        ProcessScrollWheelInput();

        if (previousWeaponIndex != currentWeaponIndex)
        {
            StartCoroutine(SwitchWeaponRoutine());
        }
    }

    void DisableAllWeapon()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    IEnumerator SwitchWeaponRoutine()
    {
        yield return SetWeaponDeactive();

        // Switch weapon
        SetWeaponActive();
    }
    IEnumerator SetWeaponDeactive()
    {
        Transform weapon = transform.GetChild(previousWeaponIndex);
        Animator animator = weapon.GetComponent<Animator>();
        animator.SetTrigger("putAway");

        yield return new WaitWhile(() => animator.IsInTransition(0));
        yield return new WaitWhile(() => (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f));

        weapon.gameObject.SetActive(false);
    }
    void SetWeaponActive()
    {
        Transform weapon = transform.GetChild(previousWeaponIndex);
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
