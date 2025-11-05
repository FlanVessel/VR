using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour
{
    [Header("Configuración de ZonaVictoria")]
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
        {
            watcher = collision.GetComponentInParent<Watcher>();     
        }

        if (watcher == null) return;
        
        triggered = true;
        StartCoroutine(HandleLevelCompletion());
    }

    private IEnumerator HandleLevelCompletion()
    {

        if (LevelUIFeedback.Instance != null)
        {
            LevelUIFeedback.Instance.ShowSuccessAndAdvance(waitBeforeNextLevel);
        }
        else if (LevelUIFeedback.Instance != null)
        {
            LevelUIFeedback.Instance.ShowSuccessAndAdvance(waitBeforeNextLevel);
        }
        else
        {
            LevelSequenceManager.Instance?.LoadNextLevel();
        }

        float t = waitBeforeNextLevel;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
