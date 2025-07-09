using StarterAssets;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;

    FirstPersonController firstPersonController;
    Weapon weapon;

    void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        weapon = FindFirstObjectByType<Weapon>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HandleDeath()
    {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        firstPersonController.enabled = false;
        weapon.canShoot = false;
    }
}
