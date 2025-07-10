using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float recoverHealth = 30f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().RecoverHealth(recoverHealth);
            Destroy(gameObject);
        }
    }
}
