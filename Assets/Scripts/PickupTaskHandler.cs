using UnityEngine;

public class PickupTaskHandler : MonoBehaviour
{
    private Transform _watcher;
    private GameObject _carriedItem = null;
    public bool IsCarrying => _carriedItem != null;

    private void Awake()
    {
        _watcher = transform;
    }

    // Lo llama TaskManager cuando el watcher está frente al item
    public void PickupItem(PickupItem item)
    {
        if (_carriedItem != null) return;

        _carriedItem = item.gameObject;
        _carriedItem.transform.SetParent(_watcher);
        _carriedItem.transform.localPosition = new Vector3(0, 1.5f, 0);
    }

    // Lo llama TaskManager cuando el watcher termina su espera en dropzone
    public void DropItem(Transform dropZone)
    {
        if (_carriedItem == null) return;

        _carriedItem.transform.SetParent(null);
        _carriedItem.transform.position = dropZone.position;
        _carriedItem = null;
    }

}
