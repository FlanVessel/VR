using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Detection")]
    public float visionRange = 15f;          // Qué tan lejos ve
    public float visionAngle = 90f;          // Ángulo del cono de visión

    [Header("Stats")]
    public float speed = 3.5f;               // Velocidad de persecución
    public int maxHealth = 100;              // Vida máxima
    public int attackDamage = 10;            // Daño al jugador
    public float attackRange = 2f;           // Distancia de ataque
    public float attackCooldown = 1.5f;      // Tiempo entre ataques

    private int currentHealth;
    private Transform watcher;
    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        watcher = GameObject.FindWithTag("Watcher").transform;

        currentHealth = maxHealth;
        agent.speed = speed;
    }

    void Update()
    {
        if (watcher == null) return;

        if (CanSeePlayer())
        {
            agent.SetDestination(watcher.position);

            float distance = Vector3.Distance(transform.position, watcher.position);
            if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (watcher.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < visionAngle / 2f)
        {
            if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, visionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Atacar al jugador
    private void AttackPlayer()
    {
        Debug.Log($"{gameObject.name} ataca al jugador por {attackDamage} de daño.");
    }

    // Recibir daño
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Reaccionar a golpes/lanzamientos
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 5f) 
        {
            int damage = Mathf.RoundToInt(collision.relativeVelocity.magnitude * 2f);
            TakeDamage(damage);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        Destroy(gameObject);
    }
}
