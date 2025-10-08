using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PickupTaskHandler : MonoBehaviour
{
    public NavMeshAgent watcher;

    [Header("Pickup Settings")]
    public Transform pickupTarget;   // Objeto a recoger
    public Transform dropZone;       // Zona donde soltar

    private GameObject carriedObject;
    private bool isCarrying = false;
    private bool goingToPickup = false;
    private bool goingToDrop = false;

    void Update()
    {
        // Paso 1: Ir a recoger
        if (pickupTarget != null && !isCarrying && !goingToPickup)
        {
            watcher.SetDestination(pickupTarget.position);
            goingToPickup = true;
        }

        if (goingToPickup && !watcher.pathPending && watcher.remainingDistance <= watcher.stoppingDistance)
        {
            PickupObject();
        }

        // Paso 2: Ir a soltar
        if (isCarrying && dropZone != null && !goingToDrop)
        {
            watcher.SetDestination(dropZone.position);
            goingToDrop = true;
        }

        if (goingToDrop && !watcher.pathPending && watcher.remainingDistance <= watcher.stoppingDistance)
        {
            DropObject();
        }
    }

    void PickupObject()
    {
        if (pickupTarget == null) return;

        carriedObject = pickupTarget.gameObject;
        carriedObject.transform.SetParent(watcher.transform);
        carriedObject.transform.localPosition = new Vector3(0, 1.5f, 0);

        isCarrying = true;
        goingToPickup = false;

        Debug.Log("Objeto recogido");
    }

    void DropObject()
    {
        if (carriedObject == null) return;

        carriedObject.transform.SetParent(null);
        carriedObject.transform.position = dropZone.position;

        isCarrying = false;
        goingToDrop = false;

        Debug.Log("Objeto soltado");

        // Abrir puerta si tiene DoorController
        var door = dropZone.GetComponent<DoorController>();
        if (door != null)
            door.OpenDoor();
    }
}
