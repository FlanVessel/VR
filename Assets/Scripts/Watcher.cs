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

    [Header("UI")]
    public GameObject failMenu;
    public Transform menuSpawn;

    private void Start()
    {
        _currentHealth = maxHealth;
        originalColor = watcherRenderer.material.color;
        if (failMenu != null) failMenu.SetActive(false);
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
        // Menos vida = parpadeo más rápido

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
        // El Watcher se vuelve "invisible" para los enemigos temporalmente
        _isInvulnerable = true;

        // Cambiar capa o tag para que los enemigos no lo detecten
        gameObject.tag = "Invisible";

        yield return new WaitForSeconds(2f); // Duración del "polvo"
        gameObject.tag = "Watcher";

        _isInvulnerable = false;
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (failMenu != null)
        {
            failMenu.SetActive(true);

            // Lo colocamos frente al jugador (en menuSpawnPoint si lo hay)
            if (menuSpawn != null)
            {
                failMenu.transform.position = menuSpawn.position;
                failMenu.transform.rotation = menuSpawn.rotation;
            }
            else
            {
                failMenu.transform.position = transform.position + transform.forward * 2f + Vector3.up * 1.5f;
                failMenu.transform.LookAt(Camera.main.transform);
            }
        }

        Time.timeScale = 0f;
    }

}
