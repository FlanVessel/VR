using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BallThrowTaskHandler : MonoBehaviour
{
    [Header("Referencias")]
    public NavMeshAgent watcher;
    public Transform holdPoint;

    [Header("Tiempos")]
    public float chargeTime = 3f;

    [Header("Lanzamiento")]
    public float arcHeight = 2f;

    private ThrowableBall currentBall;

    private bool isBusy = false;
    private bool isCarrying = false;

    public bool IsBusy => isBusy;
    public bool IsCarrying => isCarrying;

    void Update()
    {
        if (watcher == null) return;

        // Cuando llega a la pelota y aún no la ha recogido
        if (!isBusy && !isCarrying && currentBall != null && ArrivedAt(currentBall.transform.position))
        {
            StartCoroutine(PickUpAndChargeRoutine());
        }
    }

    public void MoveToBall(ThrowableBall ball)
    {
        if (ball == null) return;
        if (isBusy || isCarrying) return;

        currentBall = ball;
        watcher.isStopped = false;
        watcher.SetDestination(ball.transform.position);
    }

    private bool ArrivedAt(Vector3 target)
    {
        float dist = Vector3.Distance(watcher.transform.position, target);
        return dist <= (watcher.stoppingDistance + 0.05f);
    }

    private IEnumerator PickUpAndChargeRoutine()
    {
        isBusy = true;
        watcher.isStopped = true;

        // Tomar la pelota y ponerla en el holdPoint
        if (currentBall != null && holdPoint != null)
        {
            currentBall.AttachTo(holdPoint);
            isCarrying = true;
        }

        // Esperar el tiempo de carga
        float elapsed = 0f;
        while (elapsed < chargeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ya está listo para lanzar, pero NO lanza aún
        isBusy = false;
        watcher.isStopped = false;
    }

    public void TryThrowToTarget(Transform target)
    {
        if (!isCarrying || currentBall == null || target == null) return;

        Vector3 targetPoint = target.position;
        currentBall.ThrowTowards(targetPoint, arcHeight);

        // Después del lanzamiento, ya no trae pelota
        isCarrying = false;
        currentBall = null;
    }
}
