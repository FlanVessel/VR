using UnityEngine;

public class Canon : MonoBehaviour
{
    private Transform _shootpoint;
    private GameObject _bulletPrefab;
    private float _shootForce = 10f;
    private float _fireRate = 4f;
    private float _nextFireTime = 0f;

    public void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (Time.time >= _nextFireTime)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _shootpoint.position, _shootpoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(_shootpoint.forward * _shootForce, ForceMode.Impulse);
            _nextFireTime = Time.time + 1f / _fireRate;
        }
    }

}
