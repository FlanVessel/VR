using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonInteractable : MonoBehaviour
{
    [Header("Efecto de Circulo de Carga")]
    public Image progressCircle;      //Una imagen de Canvas
    public float interactionTime = 3f;      //Un float con nombre interactionTime de valor 3

    [Header("Accion de Abrir la Puerta")]
    public DoorController doorController;   //LLamamos a la clase DoorController y lo llamamos doorController

    private bool _isInteracting = false;    //Una bool privada

    public void StartInteraction()   //Un metodo llamado StartInteraction
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

    private IEnumerator FillProgress()   //Una Corotina
    {
        float timer = 0f;

        while (timer < interactionTime)
        {
            timer += Time.deltaTime;
            progressCircle.fillAmount = Mathf.Clamp01(timer / interactionTime);
            yield return null;
        }

        // Interacción completa
        doorController.OpenDoor();
        Debug.Log("Si abre Puerta");

        // Reset
        progressCircle.fillAmount = 0f;
        progressCircle.gameObject.SetActive(false);
        _isInteracting = false;
        Debug.Log("Circulo Reseteado");
    }
}
