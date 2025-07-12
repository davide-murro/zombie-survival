using StarterAssets;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    SceneLoader sceneLoader;

    void Awake()
    {
        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            sceneLoader.GameOver();
        }
    }

    public void RecoverHealth(float amount)
    {
        health += amount;
    }
}
