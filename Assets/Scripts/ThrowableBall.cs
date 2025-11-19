using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableBall : MonoBehaviour
{
    public Rigidbody rb;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
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
        // Desprenderse del holder
        transform.SetParent(null);
        rb.isKinematic = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = target;

        // Calculamos velocidad inicial para una parábola simple
        Vector3 dir = endPos - startPos;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        float distXZ = dirXZ.magnitude;

        float height = Mathf.Max(arcHeight, 0.1f);

        // Tiempo aproximado basado en distancia horizontal
        float time = Mathf.Max(distXZ / 5f, 0.3f); // 5 = velocidad horizontal base

        // Componente horizontal
        Vector3 vXZ = dirXZ / time;

        // Componente vertical (balística con gravedad)
        float g = Physics.gravity.y;
        float vY = (dir.y + 0.5f * -g * time * time) / time;

        Vector3 velocity = vXZ + Vector3.up * vY;

        rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detectar colisiones con botones
        var col = collision.collider;

        // Botón normal
        if (col.CompareTag("Button"))
        {
            var button = col.GetComponent<ButtonInteractable>();
            if (button != null)
            {
                button.StartInteraction();
            }
        }

        // Botón de Luz
        if (col.CompareTag("BotonLuz"))
        {
            var botonLuz = col.GetComponent<ButtonLight>();
            if (botonLuz != null)
            {
                botonLuz.Interactuar();
            }
        }

    }
}
