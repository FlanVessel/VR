using Unity.VisualScripting;
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
                taskManager.HandleRaycastHit(hit, agent);
            }

            if (throwAction.action.WasPressedThisFrame())
            {
                taskManager.HandleThrowRay(hit);
            }

        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
