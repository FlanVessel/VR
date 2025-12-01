using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public enum TaskType
{
    None,
    Pickup
}

public class TaskManager : MonoBehaviour
{
    [Header("Referencias")]
    public NavMeshAgent watcher;
    public PickupTaskHandler pickupHandler;
    public CharacterTaskHandler buttonTaskHandler;
    public BallThrowTaskHandler ballThrowTaskHandler;

    [Header("Estados")]
    public bool IsBusy { get; private set; }
    private TaskType _currentTask = TaskType.None;

    private PickupItem _activeItem;
    private Transform _activeDropZone;

    // ================== PICKUP (llave + dropzone) ==================
    public void AssignPickupTask(PickupItem item)
    {
        if (IsBusy) return;

        _currentTask = TaskType.Pickup;
        _activeItem = item;
        _activeDropZone = item.linkedDropZone.transform;

        IsBusy = true;

        watcher.SetDestination(item.transform.position);

        StartCoroutine(PickupTaskRoutine());
    }

    private IEnumerator PickupTaskRoutine()
    {
        // 1) Llegar al item
        yield return new WaitUntil(() =>
            !watcher.pathPending &&
            watcher.remainingDistance <= watcher.stoppingDistance
        );

        // 2) Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // 3) Recoger físicamente
        pickupHandler.PickupItem(_activeItem);

        // 4) Esperar a que el jugador lo lleve manualmente al dropzone
        yield return new WaitUntil(() =>
            !watcher.pathPending &&
            Vector3.Distance(watcher.transform.position, _activeDropZone.position) <= watcher.stoppingDistance
        );

        // 5) Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // 6) Soltar item
        pickupHandler.DropItem(_activeDropZone);

        // 7) Abrir puerta
        var door = _activeDropZone.GetComponent<DoorController>();
        if (door != null)
            door.OpenDoor();

        // 8) Limpieza
        _currentTask = TaskType.None;
        _activeItem = null;
        _activeDropZone = null;
        IsBusy = false;
    }

    // ================== BOTÓN normal ==================
    public void AssignButtonTask(ButtonInteractable button)
    {
        if (IsBusy) return; // si quieres que no interrumpa tareas de pickup
        if (buttonTaskHandler != null && button != null)
        {
            buttonTaskHandler.MoveToButton(button);
        }
    }

    public void AssignButtonLightTask(ButtonLight buttonLight)
    {
        if (IsBusy) return;

        IsBusy = true;

        watcher.SetDestination(buttonLight.transform.position);

        StartCoroutine(ButtonLightRoutine(buttonLight));
    }

    private IEnumerator ButtonLightRoutine(ButtonLight buttonLight)
    {
        yield return new WaitUntil(() =>
            !watcher.pathPending &&
            watcher.remainingDistance <= watcher.stoppingDistance
        );

        // Activar el botón
        buttonLight.Activate();

        IsBusy = false;
    }

    // ================== PELOTA ==================
    public void AssignBallTask(ThrowableBall ball)
    {
        if (ballThrowTaskHandler != null && ball != null)
        {
            ballThrowTaskHandler.MoveToBall(ball);
        }
    }

    public void ThrowBall(Vector3 direction)
    {
        if (ballThrowTaskHandler != null)
        {
            ballThrowTaskHandler.ThrowHeldBall(direction);
        }
    }

}
