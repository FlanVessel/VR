using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableBall : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Referencia al TaskManager")]
    public TaskManager taskManager;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        if (taskManager == null)
        {
            taskManager = Object.FindFirstObjectByType<TaskManager>();
        }
    }

    public void AttachTo(Transform holder)
    {
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
    }

    public void ThrowTowards(Vector3 target, float arcHeight)
    {
        transform.SetParent(null);
        rb.isKinematic = false;

        Vector3 startPos = transform.position;
        Vector3 dir = target - startPos;

        // Separar componente horizontal y vertical
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        float distXZ = dirXZ.magnitude;

        if (distXZ < 0.1f)
        {
            // Muy cerca: tirar un poquito hacia adelante
            dirXZ = transform.forward;
            distXZ = 1f;
        }

        // Tiempo aproximado según distancia horizontal
        float time = Mathf.Max(distXZ / 5f, 0.3f); // 5 = velocidad horizontal base

        Vector3 vXZ = dirXZ / time;

        float g = Physics.gravity.y;
        float vY = (dir.y + 0.5f * -g * time * time) / time;

        Vector3 velocity = vXZ + Vector3.up * vY;
        rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var col = collision.collider;

        // 1) Si golpea un ButtonInteractable → lo mandamos al TaskManager
        if (col.TryGetComponent<ButtonInteractable>(out var button))
        {
            if (taskManager != null)
            {
                taskManager.HandleButtonHit(button);
            }
            else
            {
                // Plan B por si no hay TaskManager
                button.StartInteraction();
            }
        }

        if (col.TryGetComponent<ButtonLight>(out var buttonLight))
        {
            if (taskManager != null)
            {
                taskManager.HandleButtonLightHit(buttonLight);
            }
            else
            {
                // Plan B por si no hay TaskManager
                buttonLight.Interactuar();
            }
        }
    }
}
