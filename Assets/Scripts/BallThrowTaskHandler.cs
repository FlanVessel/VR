using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BallThrowTaskHandler : MonoBehaviour
{
    [Header("Referencias")]
    public NavMeshAgent watcher;
    [Tooltip("Punto donde el gato sostiene la pelota (cabeza, mano, etc.)")]
    public Transform holdPoint;

    [Header("Tiempos")]
    [Tooltip("Tiempo en segundos que tarda en 'recoger' la pelota")]
    public float pickupTime = 0.5f;

    [Header("Lanzamiento")]
    [Tooltip("Altura aproximada del arco del lanzamiento")]
    public float arcHeight = 2f;

    private ThrowableBall currentBall;
    private bool isBusy = false;
    private bool isCarrying = false;

    public bool IsBusy => isBusy;
    public bool IsCarrying => isCarrying;

    void Update()
    {
        if (watcher == null) return;

        // Si vamos hacia una pelota y aún no la recogemos, revisar si ya llegamos
        if (!isBusy && !isCarrying && currentBall != null && ArrivedAt(currentBall.transform.position))
        {
            StartCoroutine(PickupRoutine());
        }
    }

    public void MoveToBall(ThrowableBall ball)
    {
        if (ball == null || watcher == null) return;
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

    private IEnumerator PickupRoutine()
    {
        isBusy = true;
        watcher.isStopped = true;

        yield return new WaitForSeconds(pickupTime);

        if (currentBall != null && holdPoint != null)
        {
            currentBall.AttachTo(holdPoint);
            isCarrying = true;
        }

        isBusy = false;
        watcher.isStopped = false;
    }

    public void ThrowToPoint(Vector3 targetPoint)
    {
        if (!isCarrying || currentBall == null) return;

        currentBall.ThrowTowards(targetPoint, arcHeight);

        isCarrying = false;
        currentBall = null;
    }
}
