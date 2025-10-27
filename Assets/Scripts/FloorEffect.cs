using UnityEngine;

public enum FloorType
{
    None, Mud, Lava
}

[RequireComponent(typeof(Collider))]
public class FloorEffect : MonoBehaviour
{
    public FloorType floorType = FloorType.None;
    public float mudSpeedMultiplier = 0.5f;
    public float lavaDamagePerSecond = 10f;

    public void Start()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Watcher"))
        {
            switch (floorType)
            {
                case FloorType.Mud:
                    var agent = other.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.speed *= mudSpeedMultiplier;
                    }
                    break;
                case FloorType.Lava:
                    var watcher = other.GetComponent<Watcher>();
                    if (watcher != null)
                    {
                        watcher.TakeDamage(1);
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Watcher") && floorType == FloorType.Mud)
        {
            var agent = other.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.speed /= mudSpeedMultiplier;
        }
    }
}
