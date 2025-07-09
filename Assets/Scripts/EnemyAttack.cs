using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerHealth target;
    [SerializeField] float damage = 25f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        target = FindFirstObjectByType<PlayerHealth>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackHit()
    {
        if (target == null) return;

        target.TakeDamage(damage);
        target.GetComponent<PlayerDamage>().ShowDamageImpact();
    }
}
