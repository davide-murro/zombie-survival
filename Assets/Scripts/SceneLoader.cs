using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] Canvas pauseCanvas;

    FirstPersonController firstPersonController;
    Weapon weapon;

    void Awake()
    {
        firstPersonController = FindFirstObjectByType<FirstPersonController>();
        weapon = FindFirstObjectByType<Weapon>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverCanvas.enabled = false;
        pauseCanvas.enabled = false;
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }
    void OnDestroy()
    {
        EnableWorld();
    }

    public void PauseGame()
    {
        pauseCanvas.enabled = true;
        DisableWorld();
    }

    public void PlayGame()
    {
        pauseCanvas.enabled = false;
        EnableWorld();
    }

    public void ReloadGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void GameOver()
    {
        gameOverCanvas.enabled = true;
        DisableWorld();
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    void EnableWorld()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (firstPersonController != null) firstPersonController.enabled = true;
        if (weapon != null) weapon.canShoot = true;
    }
    void DisableWorld()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (firstPersonController != null) firstPersonController.enabled = false;
        if (weapon != null) weapon.canShoot = false;
    }
}
