using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [Header("Tareas Disponibles")]
    public CharacterTaskHandler buttonTaskHandler;
    public PickupTaskHandler pickupTaskHandler;
    public BallThrowTaskHandler ballThrowTaskHandler;

    // Propiedad para saber si el personaje está ocupado
    public bool IsBusy =>
        (ballThrowTaskHandler != null && ballThrowTaskHandler.IsBusy) ||
        (pickupTaskHandler     != null && pickupTaskHandler.IsBusy);

    // Maneja el RaycastHit recibido y delega la acción al manejador correspondiente
    public void HandleRaycastHit(RaycastHit hit, NavMeshAgent agent)
    {
        if (hit.collider == null) return;

        Transform tr = hit.collider.transform;

        if (tr.TryGetComponent<ButtonLight>(out var buttonLight))
        {
            HandleButtonLightHit(buttonLight);
            return;
        }

        if (IsBusy) return;

        if (tr.TryGetComponent<ButtonInteractable>(out var button))
        {
            if (buttonTaskHandler != null)
            {
                buttonTaskHandler.MoveToButton(button);
            }
            return;
        }

        if (tr.TryGetComponent<PickupItem>(out var item))
        {
            if (pickupTaskHandler != null)
            {
                pickupTaskHandler.MoveToPickup(item);
            }
            return;
        }

        if (tr.TryGetComponent<ThrowableBall>(out var ball))
        {
            if (ballThrowTaskHandler != null)
            {
                // Solo ir a la pelota si todavía no trae una
                if (!ballThrowTaskHandler.IsCarrying)
                {
                    ballThrowTaskHandler.MoveToBall(ball);
                }
            }
            return;
        }

        if (agent != null)
        {
            // Opción A: seguir usando tag "Ground"
            if (tr.CompareTag("Ground"))
            {
                agent.SetDestination(hit.point);
                return;
            }
        }
    }

    // Maneja el lanzamiento de la pelota
    public void HandleThrowRay(RaycastHit hit)
    {
        if (ballThrowTaskHandler == null) return;

        if (!ballThrowTaskHandler.IsCarrying) return;

        ballThrowTaskHandler.ThrowToPoint(hit.point);
    }

    // Maneja la interacción directa con un botón (con pelota)
    public void HandleButtonHit(ButtonInteractable button)
    {
        if (button == null) return;

        button.StartInteraction();
    }

    // Maneja la interacción directa con una luz de botón (con pelota)
    public void HandleButtonLightHit(ButtonLight buttonLight)
    {
        if (buttonLight == null) return;

        buttonLight.Interactuar();
    }

}
