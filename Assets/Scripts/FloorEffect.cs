using UnityEngine;
using System.Collections;

public enum FloorType
{
    None, Mud, Lava, Acid
}

[RequireComponent(typeof(Collider))]
public class FloorEffect : MonoBehaviour
{

    [Header("Tipo de Suelo")]
    public FloorType floorType = FloorType.None;

    [Header("Configuración de Lodo")]
    public float mudSpeedMultiplier = 0.5f;

    [Header("Configuración de Ácido")]
    public float acidMoveDistance = 0.25f;
    public float acidMoveSpeed = 1f;
    public float acidWaitTime = 1f;

    private Vector3 _startPos;
    private bool _isMoving = false;

    public void Start()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        if (floorType == FloorType.Acid)
        {
            StartCoroutine(AcidMovementRoutine());
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        Watcher watcher = collision.GetComponent<Watcher>();
        if (watcher != null)
        {
            switch (floorType)
            {
                case FloorType.Mud:
                    var agent = collision.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.speed *= mudSpeedMultiplier;
                    }
                    break;
                case FloorType.Lava:
                    if (watcher != null)
                    {
                        watcher.TakeDamage(1);
                    }
                    break;
                case FloorType.Acid:
                    if (watcher != null)
                    {
                        watcher.TakeDamage(1);
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Watcher") && floorType == FloorType.Mud)
        {
            var agent = collision.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.speed /= mudSpeedMultiplier;
        }
    }

    private IEnumerator AcidMovementRoutine()
    {
        _isMoving = true;
        Vector3 startPos = transform.position;

        float upY = startPos.y + acidMoveDistance;
        float downY = startPos.y - acidMoveDistance;

        while (true)
        {

            yield return MoveToPosition(upY);
            yield return new WaitForSeconds(acidWaitTime);

            yield return MoveToPosition(downY);
            yield return new WaitForSeconds(acidWaitTime);
        }
    }

    private IEnumerator MoveToPosition(float targetY)
    {
        Vector3 pos = transform.position;

        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, targetY, acidMoveSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }
    }
}
