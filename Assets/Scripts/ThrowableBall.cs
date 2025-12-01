using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableBall : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isHeld = false;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float respawnDelay = 2f;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Transform _originalParent;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalParent = transform.parent;
    }

    public void TryPickup(Transform holder)
    {
        if (_isHeld) return;
        
        _isHeld = true;

        if (!_rb.isKinematic)
        {

            // 1. Frenar físicas ANTES de activar isKinematic
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            
        }


        // 2. Ahora sí convertirlo en kinematic
        _rb.isKinematic = true;

        // 3. "Adjuntar" la pelota al watcher
        transform.SetParent(holder);
        transform.localPosition = new Vector3(0, 1.5f, 0);

    }

    public void Throw(Vector3 direction)
    {
        if (!_isHeld) return;

        _isHeld = false;
        transform.SetParent(null);

        _rb.isKinematic = false;
        _rb.linearVelocity = direction.normalized * throwForce;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;

        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
        transform.SetParent(_originalParent);

        gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Botón normal
        var button = collision.collider.GetComponent<ButtonInteractable>();
        if (button != null)
        {
            button.StartInteraction();
            return;
        }

        // ButtonLight (que enciende luces o apaga)
        var lightButton = collision.collider.GetComponent<ButtonLight>();
        if (lightButton != null)
        {
            // Cambia "Activate" por el nombre real de tu método público en ButtonLight
            lightButton.Activate();
            return;
        }

        // Desactivar cañón si choca con uno
        var canon = collision.collider.GetComponent<Canon>();
        if (canon != null)
        {
            canon.SetActive(false);
            return;
        }

    }

}
