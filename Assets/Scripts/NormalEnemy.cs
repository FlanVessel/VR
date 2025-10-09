using UnityEngine;
using System.Collections;

public class NormalEnemy : Enemy
{
    [Header("Daño y Enfriamiento")]
    public int damage = 1;                
    public float attackCooldown = 2f;     

    private bool _canAttack = true;

    private Watcher _lastWatcher;

    // Detecta colisiones con el Watcher
    private void OnCollisionEnter(Collision collision)
    {
        // Si choca con el Watcher 
        if (collision.gameObject.CompareTag("Watcher") && _canAttack)
        {
            Watcher watcher = collision.gameObject.GetComponent<Watcher>();

            if (watcher != null)
            {
                watcher.TakeDamage(damage);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    protected override void AttackWatcher()
    {
        if (_lastWatcher != null)
        {
            _lastWatcher.TakeDamage(damage);
            StartCoroutine(AttackCooldown());
        }
    }


    // Enfriamiento entre ataques (Empieza la corutina)
    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }
}
