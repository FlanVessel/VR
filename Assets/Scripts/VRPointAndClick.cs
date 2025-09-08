using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VRPointAndClick : MonoBehaviour
{
    [Header("Configuracion")]
    public NavMeshAgent character;         // El personaje que haremos mover que tenga un componente con "NavMeshAgent"
    public Transform rayOrigin;            // El punto de origen para el Raycast
    public float rayLength = 20f;          // La distancia del Raycast
    public LayerMask raycastLayers;        // Capas donde se puede hacer click 

    [Header("Input Action")]
    public InputActionProperty selectAction; // Botón del control

    private void Update()
    {

        if (selectAction.action.WasPerformedThisFrame())
        {
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength, raycastLayers))
            {

                character.SetDestination(hit.point);


                Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);
            }
        }
    }
}
