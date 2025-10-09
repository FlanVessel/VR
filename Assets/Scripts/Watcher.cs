using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Watcher : MonoBehaviour
{

    [Header("Vida")]
    public int maxHealth = 3;
    private int _currentHealth;

    [Header("Referencia")]
    public Renderer watcherRenderer;
    public ParticleSystem deathEffect;

    [Header("Color Daño")]
    public Color hitColor = Color.red;
    public Color originalColor;
    private bool _isInvulnerable = false;

    private void Start()
    {
        _currentHealth = maxHealth;
        originalColor = watcherRenderer.material.color;
    }

    public void TakeDamage(int damage)
    {
        if (_isInvulnerable) return;

        _currentHealth -= damage;
        StartCoroutine(DamageFeedback());

        if (_currentHealth <= 0)
        {
            Die();
        }

    }

    private IEnumerator DamageFeedback()
    {
        // Efecto visual de golpe
        float blinkSpeed = Mathf.Lerp(0.4f, 0.1f, 1f - (_currentHealth / (float)maxHealth));

        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            watcherRenderer.material.color = hitColor;
            yield return new WaitForSeconds(blinkSpeed);
            watcherRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed * 2;
        }
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Time.timeScale = 0f;

        // Desactivar el Watcher
        gameObject.SetActive(false);
    }

}
