using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VRPointAndClick : MonoBehaviour
{
    [Header("Raycast")]
    public Transform rayOrigin;              // Mano o cámara desde donde sale el raycast
    public float rayDistance = 10f;          // Distancia máxima del raycast
    public LayerMask rayMask;                // Qué capas detecta el raycast

    [Header("Input Action")]
    public InputActionProperty selectAction; // El trigger o botón que usas para mover

    [Header("References")]
    public NavMeshAgent agent;               // Personaje que se mueve
    public LineRenderer lineRenderer;        // Línea que dibuja la dirección
    public GameObject destinationMarkerPrefab; // Prefab del marcador en el suelo

    private GameObject currentMarker;

    void Update()
    {
        // Lanzamos el raycast desde la mano/cámara
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, rayMask))
        {
            // Mostrar línea
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;

            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);

            // Si presiono el input, mover agente y poner marcador
            if (selectAction.action.WasPressedThisFrame())
            {
                agent.SetDestination(hit.point);

                // Si ya había un marcador, lo destruimos
                if (currentMarker != null)
                    Destroy(currentMarker);

                // Instanciar nuevo marcador en el punto de impacto
                currentMarker = Instantiate(destinationMarkerPrefab, hit.point, Quaternion.identity);
            }
        }
        else
        {
            // Si el raycast no pega nada, ocultamos la línea
            lineRenderer.enabled = false;
        }

        // Si el agente ya llegó a destino, destruir el marcador
        if (currentMarker != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(currentMarker);
        }
    }
}
