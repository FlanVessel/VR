using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [Header("Patrulla")]
    public Transform[] patrolPoints;
    public float waitTime = 2f;

    private int _currentPoint = 0;

    public float loseSightTime = 3f;
    private float _loseTimer = 0f;

    [Header("Configuración de visión")]
    public float viewDistance = 10f;     
    public float viewAngle = 90f;        

    [Header("Movimiento")]
    public float speed = 3.5f;

    [Header("Referencias")]
    private NavMeshAgent _agent;
    private Transform _watcher;
    private Coroutine _enemyCoroutine;

    private bool _isChasing = false;

    // Inicia en busqueda del componente NavMeshAgent y el objeto con tag "Watcher"
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _watcher = GameObject.FindGameObjectWithTag("Watcher").transform;
    }

    void Start()
    {
        //Inicia Coroutine
        _enemyCoroutine = StartCoroutine(EnemyRoutine());
    }

    // Por cada frame, llama la función de detección del watcher
    void Update()
    {
        // Llama la función de detección del watcher
        DetectWatcher();
    }

    IEnumerator EnemyRoutine()
    {
        while(true)
        {
            _agent.SetDestination(patrolPoints[_currentPoint].position);

            while (_agent.pathPending || _agent.remainingDistance > 0.5f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            _currentPoint = (_currentPoint + 1) % patrolPoints.Length;
        }
    }

    void DetectWatcher()
    {
        // Direcciones izquierda y derecha de visión
        Vector3 leftLimit = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        // Vector hacia el watcher
        Vector3 dirToPlayer = (_watcher.position - transform.position).normalized;

        // Devuelve el angulo entre dos vectores en grados
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        // Si el watcher está dentro del ángulo de visión
        if (angleToPlayer < viewAngle / 2f && Vector3.Distance(transform.position, _watcher.position) < viewDistance)
        {
            // Lanzamos un raycast desde el enemigo al watcher
            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, viewDistance))
            {
                // En caso de que el raycast golpee al watcher
                if (hit.transform.CompareTag("Watcher"))
                {
                    // Si no estaba persiguiendo, detiene la corutina de patrulla
                    if (!_isChasing)
                    {
                        if (_enemyCoroutine != null)
                        {
                            StopCoroutine(_enemyCoroutine);
                            _isChasing = true;
                        }
                    }

                    _loseTimer = 0;
                    // Lo seguimos
                    //Debug.Log("Jugador detectado!");
                    _agent.SetDestination(_watcher.position);

                    if (Vector3.Distance(transform.position, _watcher.position) < 2f)
                    {
                        AttackWatcher();
                    }

                    return;
                } 

            }

        }

        //Lo perdi de vista
        if (_isChasing)
        {

            _loseTimer += Time.deltaTime;
            
            //Si pasa el tiempo de perder de vista, vuelve a patrullar
            if (_loseTimer >= loseSightTime)
            {
                _isChasing = false;
                //vuelve a empezar coroutina
                _enemyCoroutine = StartCoroutine(EnemyRoutine());
            }
        }

        // Debug visual en la escena
        Debug.DrawRay(transform.position, leftLimit * viewDistance, Color.red);
        Debug.DrawRay(transform.position, rightLimit * viewDistance, Color.red);
        Debug.DrawRay(transform.position, transform.forward * viewDistance, Color.yellow);

    }

    protected virtual void AttackWatcher()
    {

    }
}
