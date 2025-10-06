using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Movimiento del Enemigo")]

    public float visionRange = 15f;          // Qué tan lejos ve
    public float visionAngle = 90f;          // Ángulo del cono de visión
    public float moveSpeed = 3.5f;            // Velocidad de movimiento
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    protected Transform watcher;
    protected NavMeshAgent agent;
    protected bool watcherVisible;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();    //obtenemos componente de NavMesh
        agent.speed = moveSpeed;    //mencionamos que la velocidad del nav sera igual al moveSpeed

        GameObject watch = GameObject.FindGameObjectWithTag("Watcher");

        if(watch != null)
        {
            watcher = watch.transform;
        }
    }

    protected virtual void Update()
    {
        if (watcher == null) return;
        {
            CheckVision();
        }

        if (watcherVisible)
        {
            OnDetectPlayer();
        }
        else
        {
            OnLosePlayer();
        }
    }

    public void CheckVision()
    {
        watcherVisible = false;

        Vector3 movToPlayer = (watcher.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, watcher.position);

        if (distanceToPlayer < visionRange)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, movToPlayer);

            if (angleToPlayer < visionRange / 2)
            {
                if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, movToPlayer, distanceToPlayer, obstacleMask))
                {
                    watcherVisible = true;
                }    
            }    
        }
    }

    /*
    protected Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
            
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    */

    public void OnDetectPlayer()
    {
        agent.SetDestination(watcher.position);
    }

    public void OnLosePlayer()
    {
        agent.ResetPath();
    }
}
