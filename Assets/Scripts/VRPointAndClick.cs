using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VRPointAndClick : MonoBehaviour
{
    [Header("Raycast")]
    public Transform rayOrigin;
    public float rayDistance = 10f;
    public LayerMask rayMask;

    [Header("Input Action")]
    public InputActionProperty selectAction;
    public InputActionProperty throwAction;

    [Header("References")]
    public NavMeshAgent agent;
    public LineRenderer lineRenderer;
    public TaskManager taskManager;

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, rayMask))
        {

            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.enabled = true;

            if (selectAction.action.WasPressedThisFrame())
            {
                // ¿Es un PickupItem?
                var pickup = hit.collider.GetComponent<PickupItem>();
                if (pickup != null)
                {
                    taskManager.AssignPickupTask(pickup);
                    return;
                }

                var button = hit.collider.GetComponent<ButtonInteractable>();
                if (button != null)
                {
                    taskManager.AssignButtonTask(button);
                    return;
                }

                var ball = hit.collider.GetComponent<ThrowableBall>();
                if (ball != null)
                {
                    taskManager.AssignBallTask(ball);
                    return;
                }

                // 4) ButtonLight que se activa con el rayo directamente
                var lightButton = hit.collider.GetComponent<ButtonLight>();
                if (lightButton != null)
                {
                    taskManager.AssignButtonLightTask(lightButton);
                    return;
                }

                // ¿Es suelo? (Mover al watcher sin tareas)
                if (hit.collider.CompareTag("Ground"))
                {
                    agent.SetDestination(hit.point);
                    return;
                }
            }


        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (throwAction.action.WasPressedThisFrame())
        {
            taskManager.ThrowBall(rayOrigin.forward);
        } 

    }

}
