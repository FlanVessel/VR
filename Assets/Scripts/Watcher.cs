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
    public ParticleSystem hitEffect;
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
        else
        {
            StartCoroutine(TemporaryInvisibility());
        }
    }

    private IEnumerator DamageFeedback()
    {
        // Efecto visual de golpe
        float blinkSpeed = Mathf.Lerp(0.4f, 0.1f, 1f - (_currentHealth / (float)maxHealth));
        // Menos vida = parpadeo m�s r�pido

        if (hitEffect != null)
            hitEffect.Play();

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

    private IEnumerator TemporaryInvisibility()
    {
        // El Watcher se vuelve "invisible"
        _isInvulnerable = true;

        // Cambiar tag para no ser papeado
        gameObject.tag = "Invisible";

        yield return new WaitForSeconds(2f); // Duraci�n del "polvo"
        gameObject.tag = "Watcher";

        //puede ser papeado de nuevo
        _isInvulnerable = false;
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Time.timeScale = 0f;

        // Mostrar panel de Game Over
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("GameOverManager no est� asignado en el inspector.");
        }

        // Desactivar el Watcher
        gameObject.SetActive(false);
    }

}
