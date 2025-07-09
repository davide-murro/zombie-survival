using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 5f;

    Transform target;

    NavMeshAgent navMeshAgent;
    EnemyHealth enemyHealth;
    EnemyAttack enemyAttack;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    // Start is called before the first frame update
    void Start()
    {
        target = FindFirstObjectByType<PlayerHealth>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.IsDead)
        {
            enabled = false;
            enemyAttack.enabled = false;
            navMeshAgent.enabled = false;
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    void EngageTarget()
    {
        FaceTarget();
        CheckVelocity();

        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        if (navMeshAgent && navMeshAgent.enabled)
            navMeshAgent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        GetComponentInChildren<Animator>().SetTrigger("Attack");
        //Debug.Log(name + " has seeked and is destroying " + target.name);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    void CheckVelocity()
    {
        Vector3 averageSpeed = new Vector3(navMeshAgent.velocity.x, 0, navMeshAgent.velocity.z);
        float speed = averageSpeed.magnitude;

        GetComponentInChildren<Animator>().SetFloat("MoveSpeed", speed);
    }
}

