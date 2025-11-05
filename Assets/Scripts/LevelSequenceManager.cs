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
            Debug.Log("No hay mÃ¡s niveles en la lista.");
        }
    }
}
