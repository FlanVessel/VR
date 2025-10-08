using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public GameObject gameOverPanelPrefab; 
    private GameObject instance;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Llamar esto cuando el jugador pierda
    public void ShowGameOver()
    {
        if (instance != null) return;

        Time.timeScale = 0f;

        var playerCamera = Camera.main.transform;
        instance = Instantiate(
            gameOverPanelPrefab,
            playerCamera.position + playerCamera.forward * 2f,
            Quaternion.LookRotation(playerCamera.forward)
        );

        instance.SetActive(true);

        Button retryButton = instance.GetComponentInChildren<Button>();
        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RetryLevel);
        }
    }

    // Botón Reintentar 
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
