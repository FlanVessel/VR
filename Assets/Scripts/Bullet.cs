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
        if (_isHeld) return;

        if (collision.gameObject.CompareTag("Watcher"))
        {
            Watcher watcher = collision.gameObject.GetComponent<Watcher>();
            if (watcher != null)
                watcher.TakeDamage(1);

            Deactivate();
        }
        else if (collision.gameObject.CompareTag("Canon"))
        {
            // Desactiva el cañón si lo golpea
            collision.gameObject.SetActive(false);
            Deactivate();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Simplemente desaparece al tocar paredes
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador agarra la bala
        if (other.CompareTag("Player"))
        {
            _isHeld = true;
            _isLaunched = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.isKinematic = true;
            transform.SetParent(other.transform);
        }
    }

    private void Update()
    {
        // Si está en manos del jugador y presiona una tecla, se lanza
        if (_isHeld && Input.GetKeyDown(KeyCode.Space))
        {
            _isHeld = false;
            _rb.isKinematic = false;
            transform.SetParent(null);
            _rb.AddForce(transform.forward * playerThrowForce, ForceMode.VelocityChange);
            Invoke(nameof(Deactivate), lifetime);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
