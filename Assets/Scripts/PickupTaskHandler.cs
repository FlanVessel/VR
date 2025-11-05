using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PickupTaskHandler : MonoBehaviour
{
    [Header("Referencias")]
    public NavMeshAgent watcher;

    [Header("Tiempos de interacción")]
    public float pickupTime = 3f;
    public float dropTime   = 3f;

    private PickupItem currentItem;
    private GameObject carriedObject;

    // Estado
    private bool isCarrying = false;
    private bool isBusy     = false;

    public bool IsCarrying => isCarrying;
    public bool IsBusy     => isBusy;
    public PickupItem CurrentItem => currentItem;

    void Update()
    {
        if (watcher == null) return;

        if (!isBusy && !isCarrying && currentItem != null && ArrivedAt(currentItem.transform.position))
        {
            StartCoroutine(PickupRoutine());
        }
    }

    public void MoveToPickup(PickupItem item)
    {
        if (item == null) return;
        if (isBusy || isCarrying) return;

        currentItem = item;
        watcher.SetDestination(item.transform.position);
    }

    public void OnEnterDropZone(DropZone dz)
    {
        if (isBusy) return;
        if (!isCarrying || currentItem == null || currentItem.linkedDropZone == null) return;
        if (dz != currentItem.linkedDropZone) return;

        StartCoroutine(DropRoutine());
    }

    private bool ArrivedAt(Vector3 target)
    {
        float dist = Vector3.Distance(watcher.transform.position, target);
        return dist <= (watcher.stoppingDistance + 0.05f);
    }

    private IEnumerator PickupRoutine()
    {
        isBusy = true;

        yield return new WaitForSeconds(pickupTime);

        carriedObject = currentItem.gameObject;
        carriedObject.transform.SetParent(watcher.transform);
        carriedObject.transform.localPosition = new Vector3(0f, 1.5f, 0f);

        isCarrying = true;
        isBusy = false;
    }

    private IEnumerator DropRoutine()
    {
        isBusy = true;

        yield return new WaitForSeconds(dropTime);

        if (carriedObject != null && currentItem != null && currentItem.linkedDropZone != null)
        {
            carriedObject.transform.SetParent(null);
            carriedObject.transform.position = currentItem.linkedDropZone.transform.position;

            if (currentItem.linkedDropZone.linkedDoor != null)
            {
                currentItem.linkedDropZone.linkedDoor.OpenDoor();
            }
        }

        carriedObject = null;
        currentItem   = null;
        isCarrying    = false;
        isBusy        = false;
    }
}
