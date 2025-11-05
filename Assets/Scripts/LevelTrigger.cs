using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour
{
    [Header("Configuración")]
    public float waitBeforeNextLevel = 5f;
    private bool triggered = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (triggered) return;

        Watcher watcher = collision.GetComponent<Watcher>();
        if (watcher == null) return;
        triggered = true;
        StartCoroutine(HandleLevelCompletion());
    }

    private IEnumerator HandleLevelCompletion()
    {
        if (LevelUIFeedback.Instance != null)
            LevelUIFeedback.Instance.ShowSuccessAndAdvance(waitBeforeNextLevel);

        float t = 0f; while (t < waitBeforeNextLevel) { t += Time.unscaledDeltaTime; yield return null; }
    }
}
