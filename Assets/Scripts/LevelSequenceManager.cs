using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum LevelStatus { Unknown = 0, Passed = 1, Failed = 2 }

public class LevelSequenceManager : MonoBehaviour
{
    public static LevelSequenceManager Instance { get; private set; }

    [Header("Orden de los niveles (por nombre de escena)")]
    public List<string> sceneOrder = new List<string>();

    private Dictionary<string, LevelStatus> levelStatuses = new Dictionary<string, LevelStatus>();

    [Header("Pantalla final")]
    public GameObject finalPanel;
    public string restartSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLevelStatus(string sceneName, LevelStatus status)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        levelStatuses[sceneName] = status;
    }

    public LevelStatus GetLevelStatus(string sceneName)
    {
        return levelStatuses.ContainsKey(sceneName) ? levelStatuses[sceneName] : LevelStatus.Unknown;
    }

    public void LoadNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int index = sceneOrder.IndexOf(currentScene);
        if (index == -1) return;

        int nextIndex = index + 1;
        if (nextIndex < sceneOrder.Count)
        {
            string nextScene = sceneOrder[nextIndex];
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("No tengo mas niveles que cargar");

            if (finalPanel != null)
            {
                finalPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.LogWarning("No hay 'finalPanel' asignado en el LevelSequenceManager");
            }
        }
    }

    public void RestartGame()
    {
        if (string.IsNullOrEmpty(restartSceneName))
        {
            Debug.LogWarning("restartSceneName no está asignado.");
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(restartSceneName);
    }
}
