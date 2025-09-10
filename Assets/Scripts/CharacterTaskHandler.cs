using UnityEngine;
using UnityEngine.AI;

public class CharacterTaskHandler : MonoBehaviour
{
    public NavMeshAgent watcher;
    private ButtonInteractable _currentButton;

    void Update()
    {

        if (_currentButton != null && !watcher.pathPending)
        {
            Debug.Log($"Destino asignado: {_currentButton.name}");
            Debug.Log($"Distancia restante: {watcher.remainingDistance}");
            Debug.Log($"StoppingDistance: {watcher.stoppingDistance}");
            Debug.Log($"PathPending: {watcher.pathPending}");

            if (watcher.remainingDistance <= watcher.stoppingDistance)
            {
                // Cuando llega al botón
                _currentButton.StartInteraction();
                Debug.Log("Si, llame al Metodo StartInteraction()");
                _currentButton = null; // evita que lo repita
                Debug.Log("Logre que no se repita");
            }
        }

    }

    public void MoveToButton(ButtonInteractable button)
    {
        _currentButton = button;
        watcher.SetDestination(button.transform.position);
    }
}
