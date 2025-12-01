using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonInteractable : MonoBehaviour
{
    [Header("Efecto de Circulo de Carga")]
    public Image progressCircle;      
    public float interactionTime = 3f;      

    [Header("Accion de Abrir la Puerta")]
    public DoorController doorController;

    private bool _isInteracting = false;

    public void StartInteraction()
    {
        if (!_isInteracting && doorController != null && !doorController.IsOpened)   
        {
            _isInteracting = true;
            progressCircle.fillAmount = 0f;
            progressCircle.gameObject.SetActive(true);

            StartCoroutine(FillProgress());
            Debug.Log("Inicia la Coroutine");
        }
    }

    private IEnumerator FillProgress()
    {
        float timer = 0f;

        while (timer < interactionTime)
        {
            timer += Time.deltaTime;
            progressCircle.fillAmount = Mathf.Clamp01(timer / interactionTime);
            yield return null;
        }

        doorController.OpenDoor();
        Debug.Log("Si abri Puerta");

        progressCircle.fillAmount = 0f;
        progressCircle.gameObject.SetActive(false);
        _isInteracting = false;
        Debug.Log("Circulo Reseteado");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<ThrowableBall>())
            StartInteraction();
    }

}
