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
    public GameObject destinationMarkerPrefab; // Prefab del marcador en el suelo
    public CharacterTaskHandler character;

    private GameObject currentMarker;

    void Update()
    {
        // Lanzamos el raycast desde la mano
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // Si el raycast pega en algo
        if (Physics.Raycast(ray, out hit, rayDistance, rayMask))
        {
            // Muestra la linea
            if (!lineRenderer.enabled)
            {

                lineRenderer.enabled = true;
                
            }
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);

            // Si se pulsa el boton(input action)
            if (selectAction.action.WasPressedThisFrame())
            {
                //Usamos tag(ground) para identificar el suelo
                if (hit.collider.CompareTag("Ground"))
                {
                    // Movemos el watcher al punto
                    agent.SetDestination(hit.point);

                    // Manejo del marcador de destino
                    if (currentMarker != null)
                    {
                        
                        Destroy(currentMarker);

                    }

                    currentMarker = Instantiate(destinationMarkerPrefab, hit.point, Quaternion.identity);
                }
                else if (hit.collider.CompareTag("Button"))
                {
                    // Movermos el watcher al boton
                    var button = hit.collider.GetComponent<ButtonInteractable>() ?? hit.collider.GetComponentInParent<ButtonInteractable>();

                    // Asegurarse de que character no es null
                    if (button != null && character != null)
                    {
                        character.MoveToButton(button);
                    }
                    else
                    {
                        Debug.LogWarning("No se encontro CharacterTaskHandler o ButtonInteractable.");
                    }
                }
            }
        }
        else
        {
            // Si el raycast no pega nada, ocultamos la l�nea
            lineRenderer.enabled = false;
        }

        // Si el agente llego al destino, destruye el marcador
        if (currentMarker != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(currentMarker);
        }
    }
}
