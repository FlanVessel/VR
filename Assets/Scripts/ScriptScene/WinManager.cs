using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance { get; private set; }

    public GameObject winPanelPrefab;
    //public Camera cam;
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

    public void ShowWinPanel()
    {
        if (instance != null) return;

        Time.timeScale = 0f; // Congelar el tiempo

            var playerCamera = Camera.main.transform;
            instance = Instantiate(
            winPanelPrefab,
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

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
