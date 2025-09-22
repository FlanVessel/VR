using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float visionRange = 10f;        // Distancia de visión
    public float visionAngle = 45f;        // Ángulo del cono
    public Transform eyePoint;             // Desde dónde mira el enemigo
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Watcher").transform;

        if (eyePoint == null)
            eyePoint = transform; // Por defecto mira desde el centro del enemigo
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            // Persigue al jugador
            agent.SetDestination(player.position);
        }
        else
        {
            // Si quieres, aquí pon patrulla o idle
            agent.SetDestination(transform.position);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - eyePoint.position).normalized;

        // 1. ¿Está dentro del ángulo de visión?
        float angle = Vector3.Angle(eyePoint.forward, directionToPlayer);
        if (angle > visionAngle) return false;

        // 2. ¿Está dentro del rango?
        float distance = Vector3.Distance(eyePoint.position, player.position);
        if (distance > visionRange) return false;

        // 3. ¿Hay línea de visión (Raycast)?
        if (Physics.Raycast(eyePoint.position, directionToPlayer, out RaycastHit hit, visionRange))
        {
            if (hit.collider.CompareTag("Watcher"))
            {
                return true;
            }
        }

        return false;
    }
}
