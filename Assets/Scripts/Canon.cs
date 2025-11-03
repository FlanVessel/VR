using UnityEngine;

public enum TypeShoot
{
    Normal, Fast
}
public class Canon : MonoBehaviour
{
    [Header("Configuración del cañón")]
    public TypeShoot typeShoot = TypeShoot.Normal;
    public Transform spawnPoint;
    public GameObject bulletPrefab;

    [Header("Ajustes de disparo")]
    public float normalFireRate = 1f;
    public float normalSpeed = 10f;
    public float fastFireRate = 0.3f;
    public float fastSpeed = 20f;

    private float _nextShootTime;

    private void Update()
    {
        if (Time.time >= _nextShootTime)
        {
            switch (typeShoot)
            {
                case TypeShoot.Normal:
                    Shoot(normalSpeed);
                    _nextShootTime = Time.time + normalFireRate;
                    break;

                case TypeShoot.Fast:
                    Shoot(fastSpeed);
                    _nextShootTime = Time.time + fastFireRate;
                    break;
            }
        }
    }

    private void Shoot(float speed)
    {
        if (bulletPrefab == null || spawnPoint == null)
        {
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Launch(spawnPoint.forward * speed);
        }
    }
}
