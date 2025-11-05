using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Header("Referencias")]
    public DoorController linkedDoor;

    [Header("Opcional")]
    public string watcherTag = "Watcher";

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(watcherTag) && other.CompareTag(watcherTag))
        {
            TryNotifyPickupHandler(other);
            return;
        }
        TryNotifyPickupHandler(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!string.IsNullOrEmpty(watcherTag) && other.CompareTag(watcherTag))
        {
            TryNotifyPickupHandler(other);
            return;
        }
        TryNotifyPickupHandler(other);
    }

    private void TryNotifyPickupHandler(Component maybeWatcher)
    {
        var handler = maybeWatcher.GetComponentInParent<PickupTaskHandler>();
        if (handler == null) handler = maybeWatcher.GetComponent<PickupTaskHandler>();
        if (handler == null) return;

        handler.OnEnterDropZone(this);
    }
}
