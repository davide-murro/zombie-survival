using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponIndex = 0;

    struct Pose
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
    }

    Pose[] initialRootPoses;

    void Awake()
    {
        int childCount = transform.childCount;
        initialRootPoses = new Pose[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Transform weapon = transform.GetChild(i);
            initialRootPoses[i] = new Pose
            {
                localPosition = weapon.localPosition,
                localRotation = weapon.localRotation
            };
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplyWeaponStates();
    }

    // Update is called once per frame
    void Update()
    {
        int previous = currentWeaponIndex;
        ProcessKeyInput();
        ProcessScrollWheelInput();
        if (previous != currentWeaponIndex)
        {
            ApplyWeaponStates();
        }
    }

    void ApplyWeaponStates()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform weapon = transform.GetChild(i);
            bool makeActive = i == currentWeaponIndex;
            weapon.gameObject.SetActive(makeActive);

            if (makeActive)
            {
                if (weapon.TryGetComponent<Animator>(out var anim))
                {
                    anim.Play("Idle", 0, 0f);
                    anim.Update(0f);
                }
                else
                {
                    RestoreRootPose(weapon, i);
                }
            }
            else
            {
                RestoreRootPose(weapon, i);
            }
        }
    }

    void RestoreRootPose(Transform weapon, int index)
    {
        //weapon.localPosition = initialRootPoses[index].localPosition;
        //weapon.localRotation = initialRootPoses[index].localRotation;
        weapon.SetLocalPositionAndRotation(initialRootPoses[index].localPosition, initialRootPoses[index].localRotation);
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
