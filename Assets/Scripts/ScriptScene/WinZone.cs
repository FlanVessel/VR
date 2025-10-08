using UnityEngine;

public class WinZone : MonoBehaviour
{
    private bool _activated = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica que sea el Watcher
        if (_activated) return;

        if (other.CompareTag("Watcher"))
        {
            _activated = true;
            Debug.Log("¡Victoria detectada!");
            
            // Llamar al WinManager
            if (WinManager.Instance != null)
            {
                WinManager.Instance.ShowWinPanel();
            }
            else
            {
                Debug.LogWarning("No se encontró WinManager en la escena.");
            }
        }
    }
}
