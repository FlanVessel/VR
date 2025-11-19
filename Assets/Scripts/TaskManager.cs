using UnityEngine.AI;
using UnityEngine;

public enum TaskType
{
    None,
    Button,
    Pickup,
    ThrowBall
}

public class TaskManager : MonoBehaviour
{
    [Header("Tareas Disponibles")]
    public CharacterTaskHandler buttonTaskHandler;
    public PickupTaskHandler pickupTaskHandler;
    public BallThrowTaskHandler ballThrowTaskHandler;

    public bool IsBusy => (pickupTaskHandler != null && pickupTaskHandler.IsBusy) || (ballThrowTaskHandler != null && ballThrowTaskHandler.IsBusy);

    public void HandleRaycastHit(RaycastHit hit, NavMeshAgent agent)
    {
        if (hit.collider == null) return;

        var ray = hit.collider.transform;

        if (ballThrowTaskHandler != null && ballThrowTaskHandler.IsCarrying)
        {
            if (ray.CompareTag("Button") || ray.CompareTag("ButtonLight"))
            {
                ballThrowTaskHandler.TryThrowToTarget(ray);
                return;
            }
        }

        if (IsBusy) return;

        if (ray.CompareTag("Button"))
        {
            var button = ray.GetComponent<ButtonInteractable>();
            if (buttonTaskHandler != null && button != null)
            {
                buttonTaskHandler.MoveToButton(button);
            }
            return;
        }

        if (ray.CompareTag("Pickup"))
        {
            var item = ray.GetComponent<PickupItem>();
            if (pickupTaskHandler != null && item != null)
            {
                pickupTaskHandler.MoveToPickup(item);
            }
            return;
        }

        if (ray.CompareTag("ButtonLight"))
        {
            var buttonluz = ray.GetComponent<ButtonLight>();
            if (buttonluz != null)
            {
                buttonluz.Interactuar();
            }
            return;
        }
        
        if (ray.CompareTag("Ball"))
        {
            var ball = ray.GetComponent<ThrowableBall>();
            if (ballThrowTaskHandler != null && ball != null)
            {
                ballThrowTaskHandler.MoveToBall(ball);
            }
            return;
       }

        if (ray.CompareTag("Ground") && agent != null)
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
