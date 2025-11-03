using UnityEngine;
using System.Collections;

public enum FloorType
{
    None, Mud, Lava, Acid
}

[RequireComponent(typeof(Collider))]
public class FloorEffect : MonoBehaviour
{
    public FloorType floorType = FloorType.None;
    public float mudSpeedMultiplier = 0.5f;
    public float acidSpeedMove = 5f;

    public void Start()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Update()
    {
        
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
                        MoveDownandUp();
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

    private void MoveDownandUp()
    {
        
    }
}
