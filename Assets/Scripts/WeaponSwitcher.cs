using System.Collections;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetWeaponActive();
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeaponIndex = currentWeaponIndex;

        ProcessKeyInput();
        ProcessScrollWheelInput();

        if (previousWeaponIndex != currentWeaponIndex)
        {
            StartCoroutine(SwitchWeaponRoutine(previousWeaponIndex, currentWeaponIndex));
            //SetWeaponActive();
        }
    }

    IEnumerator SwitchWeaponRoutine(int prevIndex, int newIndex)
    {
        //isSwitching = true;

        // Play weapon put-away animation (optional)
        Transform currentWeapon = transform.GetChild(prevIndex);
        Animator currentAnimator = currentWeapon.GetComponent<Animator>();

        if (currentAnimator != null)
        {
            currentAnimator.SetTrigger("putAway");

            yield return new WaitUntil(() => currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("putAway") &&
                                            currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f); // Using 0.99f for safety


            //while (currentAnimator.IsInTransition(0) || currentAnimator.IsInTransition(1))
            //{
            //    yield return null;
            //}

            // Wait until animation is done
            //float animationDuration = currentAnimator.GetCurrentAnimatorStateInfo(0).length;
            //yield return new WaitForSeconds(animationDuration);
        }

        // Switch weapon
        SetWeaponActive();

        // Optionally: play draw animation on new weapon
        /*Transform newWeapon = transform.GetChild(currentWeaponIndex);
        Animator newAnimator = newWeapon.GetComponent<Animator>();
        if (newAnimator != null)
        {
            newAnimator.SetTrigger("Draw");
        }

        isSwitching = false;*/
    }

    void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach (Transform weapon in transform)
        {
            if (weaponIndex == currentWeaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            weaponIndex++;
        }
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
            if (currentWeaponIndex >= transform.childCount -1)
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
