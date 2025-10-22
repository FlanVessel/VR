using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VRPointAndClick : MonoBehaviour
{
    [Header("Raycast")]
    public Transform rayOrigin;              
    public float rayDistance = 10f;          
    public LayerMask rayMask;                // Que capas detecta el raycast

    [Header("Input Action")]
    public InputActionProperty selectAction; 

    [Header("References")]
    public NavMeshAgent agent;               // Personaje que se mueve
    public LineRenderer lineRenderer;        // Linea que dibuja la direccion
    public TaskManager taskManager;        // Maneja las tareas del personaje


    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, rayMask))
        {
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.enabled = true;

            if (selectAction.action.WasPressedThisFrame() && !taskManager.IsBusy)
            {
                if (hit.collider.CompareTag("Button"))
                {
                    taskManager.AssignTask(TaskType.Button, hit.collider.transform);
                }
                else if (hit.collider.CompareTag("Pickup"))
                {
                    taskManager.AssignTask(TaskType.Pickup, hit.collider.transform);
                }
                else if (hit.collider.CompareTag("Ground"))
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }

    }
}
