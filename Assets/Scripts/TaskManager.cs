using UnityEngine.AI;
using UnityEngine;

public enum TaskType
{
    None,
    Button,
    Pickup 
}

public class TaskManager : MonoBehaviour
{
    [Header("Tareas Disponibles")]
    public CharacterTaskHandler buttonTaskHandler;
    public PickupTaskHandler pickupTaskHandler;

    public bool IsBusy => (pickupTaskHandler != null && pickupTaskHandler.IsBusy);

    public void HandleRaycastHit(RaycastHit hit, NavMeshAgent agent)
    {
        if (hit.collider == null) return;
        if (IsBusy) return;

        var t = hit.collider.transform;

        if (t.CompareTag("Button"))
        {
            var button = t.GetComponent<ButtonInteractable>();
            if (buttonTaskHandler != null && button != null)
            {
                buttonTaskHandler.MoveToButton(button);
            }
            return;
        }

        if (t.CompareTag("Pickup"))
        {
            var item = t.GetComponent<PickupItem>();
            if (pickupTaskHandler != null && item != null)
            {
                pickupTaskHandler.MoveToPickup(item);
            }
            return;
        }

        if (t.CompareTag("Ground") && agent != null)
        {
            agent.SetDestination(hit.point);
        }
    }

    public void AssignTask(TaskType taskType, Transform target)
    {
        switch (taskType)
        {
            case TaskType.Pickup:
                var item = target.GetComponent<PickupItem>();
                if (pickupTaskHandler != null && item != null)
                {
                    pickupTaskHandler.MoveToPickup(item);
                }
                break;

            case TaskType.Button:
                var button = target.GetComponent<ButtonInteractable>();
                if (buttonTaskHandler != null && button != null)
                {
                    buttonTaskHandler.MoveToButton(button);
                }
                break;
        }
    }
}
