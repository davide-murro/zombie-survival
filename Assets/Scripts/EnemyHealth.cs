using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    public void TakeDamage(float damage)
    {
        BroadcastMessage(nameof(EnemyController.OnDamageTaken));

        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


}
