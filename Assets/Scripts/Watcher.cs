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

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // (Opcional) Slow motion 5s reales
    float originalTimeScale = Time.timeScale;
    Time.timeScale = 0.2f;
    Time.fixedDeltaTime = 0.02f * Time.timeScale;

    // Mostrar panel “Fallaste” y programar avance
    if (LevelUIFeedback.Instance != null)
        LevelUIFeedback.Instance.ShowFailAndAdvance(5f);

    // Espera 5s de tiempo real para que el jugador lo vea (puedes omitirlo si no quieres esperar aquí)
    float t = 0f; while (t < 5f) { t += Time.unscaledDeltaTime; yield return null; }

    // Restaurar tiempo
    Time.timeScale = originalTimeScale;
    Time.fixedDeltaTime = 0.02f;

    // Desactivar al Watcher (opcional si tu siguiente escena crea otro)
    gameObject.SetActive(false);
    }

}
