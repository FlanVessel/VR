using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isLaunched;
    private bool _isHeld;

    [Header("Configuración")]
    public float lifetime = 5f; // tiempo antes de apagarse
    public float playerThrowForce = 15f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 velocity)
    {
        _rb.linearVelocity = velocity;
        _isLaunched = true;
        _isHeld = false;

        // Desactivar después de cierto tiempo
        Invoke(nameof(Deactivate), lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        Watcher watcher = collision.gameObject.GetComponent<Watcher>();
        if (watcher != null)
        {
            watcher.TakeDamage(1);
            Deactivate();
        }

        Canon canon = collision.gameObject.GetComponent<Canon>();
        if (canon != null)
        {
            Deactivate();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
