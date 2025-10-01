using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Movimiento del Enemigo")]

    public float visionRange = 15f;          // Qué tan lejos ve
    public float visionAngle = 90f;          // Ángulo del cono de visión
    public float moveSpeed = 3.5f;            // Velocidad de movimiento

    private GameObject pointVision;

    protected Transform watcher;
    protected NavMeshAgent agent;

    protected virtual void Start()
    {
        watcher = GameObject.FindGameObjectWithTag("Watcher").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        pointVision = GetComponent<GameObject>();
    }

    protected virtual void Update()
    {
        if (CanSeeWatcher())
        {
            agent.SetDestination(watcher.position);
        }
        else
        {
            agent.SetDestination(transform.position); // Detenerse si no ve al watcher
        }
    }

    protected bool CanSeeWatcher()
    {
        Vector3 directionToWatcher = watcher.position - transform.position;
        float distanceToWatcher = directionToWatcher.magnitude;

        if (distanceToWatcher <= visionRange)
        {
            float angleToWatcher = Vector3.Angle(transform.forward, directionToWatcher);
            if (angleToWatcher <= visionAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, directionToWatcher.normalized, out hit, visionRange))
                {
                    if (hit.transform == watcher)
                    {
                        return true; // Puede ver al watcher
                    }
                }
            }
        }
        return false; // No puede ver al watcher
    }
}
