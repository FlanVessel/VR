using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour
{
    [Header("Configuración")]
    public float waitBeforeNextLevel = 5f;
    private bool triggered = false;

    public void Start()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (triggered) return;

        Watcher watcher = collision.GetComponent<Watcher>();
        if (watcher == null)
            watcher = collision.GetComponentInParent<Watcher>();

        if (watcher == null) return;
        
        triggered = true;
        StartCoroutine(HandleLevelCompletion());
    }

    private IEnumerator HandleLevelCompletion()
    {
        // 1) Intenta mostrar UI con el simple (paneles públicos)
        if (LevelUIFeedback.Instance != null)
        {
            LevelUIFeedback.Instance.ShowSuccessAndAdvance(waitBeforeNextLevel);
        }
        // 2) O con el dinámico (canvas generado por código)
        else if (LevelUIFeedback.Instance != null)
        {
            LevelUIFeedback.Instance.ShowSuccessAndAdvance(waitBeforeNextLevel);
        }
        // 3) Si no hay UI, al menos avanza
        else
        {
            LevelSequenceManager.Instance?.LoadNextLevel();
        }

        // Espera en tiempo real para que el jugador vea el mensaje
        float t = waitBeforeNextLevel;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
