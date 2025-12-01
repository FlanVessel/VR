using UnityEngine;
using UnityEngine.AI;

public class CharacterTaskHandler : MonoBehaviour
{
    [Header("Referencias")]
    public NavMeshAgent watcher;
    private ButtonInteractable _currentButton;

    void Update()
    {

        if (_currentButton != null && !watcher.pathPending)
        {

            if (watcher.remainingDistance <= watcher.stoppingDistance)
            {

                EventManager.TriggerEvent("WatcherHere", null);
                // Cuando llega al boton
                _currentButton.StartInteraction();
                Debug.Log("Si, llame al Metodo StartInteraction()");
                _currentButton = null;
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
