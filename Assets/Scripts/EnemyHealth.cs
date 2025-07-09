using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    bool isDead = false;

    public bool IsDead
    {
        get { return isDead; }
    }

    public void TakeDamage(float damage)
    {
        BroadcastMessage(nameof(EnemyController.OnDamageTaken));

        health -= damage;
        if (health <= 0)
        {
            Die();
            //Destroy(gameObject);
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponentInChildren<Animator>().SetTrigger("Dead");
    }
}
