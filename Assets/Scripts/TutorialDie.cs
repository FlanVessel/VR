using UnityEngine;
using System.Collections;

public class TutorialDie : MonoBehaviour
{
    [Header("Carteles del tutorial (en orden)")]
    public GameObject[] tutorialPanels;

    [Header("Tiempo por cartel")]
    public float panelDuration = 3f;

    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;

        // ¿El que entra es el Watcher?
        Watcher watcher = other.GetComponent<Watcher>();
        if (watcher == null) return;

        _triggered = true;
        StartCoroutine(TutorialSequence(watcher));
    }

    private IEnumerator TutorialSequence(Watcher watcher)
    {
        // Congelar tiempo
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // Mostrar carteles uno por uno
        foreach (var panel in tutorialPanels)
        {
            panel.SetActive(true);
            yield return new WaitForSecondsRealtime(panelDuration);
            panel.SetActive(false);
        }

        // Restaurar tiempo antes de matar al watcher
        Time.timeScale = originalTimeScale;

        // Matar al watcher (usa tu sistema actual)
        watcher.TakeDamage(9999);
    }
}
