using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelUIFeedback : MonoBehaviour
{
    public static LevelUIFeedback Instance { get; private set; }

    [Header("Configuración de Paneles UI")]

    public GameObject panelPerdiste;
    public GameObject panelGanaste;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Los dejamos apagados al inicio por seguridad
        if (panelPerdiste) panelPerdiste.SetActive(false);
        if (panelGanaste) panelGanaste.SetActive(false);

        // Cuando cargue una nueva escena, apagamos por si quedaron encendidos
        SceneManager.sceneLoaded += (s, m) =>
        {
            if (panelPerdiste) panelPerdiste.SetActive(false);
            if (panelGanaste) panelGanaste.SetActive(false);
        };
    }

    public void ShowFailAndAdvance(float seconds = 5f)
    {
        if (panelPerdiste) StartCoroutine(ShowPanelThenAdvance(panelPerdiste, seconds, LevelStatus.Failed));
        else StartCoroutine(AdvanceAfterSeconds(seconds, LevelStatus.Failed));
    }

    public void ShowSuccessAndAdvance(float seconds = 5f)
    {
        if (panelGanaste) StartCoroutine(ShowPanelThenAdvance(panelGanaste, seconds, LevelStatus.Passed));
        else StartCoroutine(AdvanceAfterSeconds(seconds, LevelStatus.Passed));
    }

    private IEnumerator ShowPanelThenAdvance(GameObject panel, float seconds, LevelStatus status)
    {
        panel.SetActive(true);
        float t = 0f;
        while (t < seconds) { t += Time.unscaledDeltaTime; yield return null; }
        panel.SetActive(false);
        Advance(status);
    }

    private IEnumerator AdvanceAfterSeconds(float seconds, LevelStatus status)
    {
        float t = 0f;
        while (t < seconds) { t += Time.unscaledDeltaTime; yield return null; }
        Advance(status);
        yield break;
    }

    private void Advance(LevelStatus status)
    {
        if (LevelSequenceManager.Instance != null)
        {
            string scene = SceneManager.GetActiveScene().name;
            LevelSequenceManager.Instance.SetLevelStatus(scene, status);
            LevelSequenceManager.Instance.LoadNextLevel();
        }
    }
}
