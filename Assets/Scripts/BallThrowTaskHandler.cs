using UnityEngine;
using UnityEngine.AI;

public class BallThrowTaskHandler : MonoBehaviour
{
    private NavMeshAgent _watcher;
    private ThrowableBall _targetBall;
    private ThrowableBall _heldBall;

    public bool IsHolding => _heldBall != null;

    private void Awake()
    {
        _watcher = GetComponent<NavMeshAgent>();
    }

    public void MoveToBall(ThrowableBall ball)
    {
        _targetBall = ball;
        _watcher.SetDestination(ball.transform.position);
    }

    private void Update()
    {
        if (_targetBall == null) return;
        if (_watcher.pathPending) return;

        if (_watcher.remainingDistance <= _watcher.stoppingDistance)
        {
            // Cuando llega a la pelota, la recoge
            _targetBall.TryPickup(_watcher.transform);
            _heldBall = _targetBall;
            _targetBall = null;
        }
    }

    public void ThrowHeldBall(Vector3 direction)
    {
        if (_heldBall == null) return;

        _heldBall.Throw(direction);
        _heldBall = null;
    }
}
