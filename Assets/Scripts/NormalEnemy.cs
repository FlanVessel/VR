using UnityEngine;

public class NormalEnemy : Enemy
{
    [Header("Daño")]
    public int damage = 1;                
    public float attackCooldown = 2f;     

    private bool _canAttack = true;

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

    private System.Collections.IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }
}
