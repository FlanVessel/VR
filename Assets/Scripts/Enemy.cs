using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Configuración de visión")]
    public float viewDistance = 10f;     
    public float viewAngle = 90f;        

    [Header("Movimiento")]
    public float speed = 3.5f;
    private NavMeshAgent agent;

    [Header("Watcher")]
    public Transform watcher;

    // Iniccia en busqueda del componente NavMeshAgent y el objeto con tag "Watcher"
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        watcher = GameObject.FindGameObjectWithTag("Watcher").transform;
    }

    // Por cada frame, llama la función de detección del watcher
    void Update()
    {
        // Llama la función de detección del watcher
        DetectWatcher();
    }

    void DetectWatcher()
    {
        // Direcciones izquierda y derecha de visión
        Vector3 leftLimit = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        // Vector hacia el watcher
        Vector3 dirToPlayer = (watcher.position - transform.position).normalized;

        // Devuelve el angulo entre dos vectores en grados
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        // Si el watcher está dentro del ángulo de visión
        if (angleToPlayer < viewAngle / 2f)
        {
            // Lanzamos un raycast desde el enemigo al watcher
            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, viewDistance))
            {
                // En caso de que el raycast golpee al watcher
                if (hit.transform.CompareTag("Watcher"))
                {
                    // Lo seguimos
                    Debug.Log("Jugador detectado!");
                    agent.SetDestination(watcher.position);
                }
            }
        }

        // Debug visual en la escena
        Debug.DrawRay(transform.position, leftLimit * viewDistance, Color.red);
        Debug.DrawRay(transform.position, rightLimit * viewDistance, Color.red);
        Debug.DrawRay(transform.position, transform.forward * viewDistance, Color.yellow);
    }
}
