using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ButtonLight : MonoBehaviour
{
    [Header("Luces que quiero controlar")]
    public Light[] lightsToToggle;

    [Header("Obstáculo que bloquea el paso")]
    public GameObject obstacle;

    [Header("UI - Círculo de progreso encima del botón")]
    public Image progressCircle;
    public float activationTime = 3f;

    private bool _isOn = false;
    private bool _isProcessing = false;

    private void Start()
    {
        if (progressCircle != null)
            progressCircle.fillAmount = 0f;

        ApplyState();
    }

    public void Activate()
    {
        if (_isProcessing) return;

        StartCoroutine(ActivationRoutine());
    }

    private IEnumerator ActivationRoutine()
    {
        _isProcessing = true;

        float timer = 0f;

        if (progressCircle != null)
            progressCircle.gameObject.SetActive(true);

        while (timer < activationTime)
        {
            timer += Time.deltaTime;

            if (progressCircle != null)
                progressCircle.fillAmount = timer / activationTime;

            yield return null;
        }

        // Toggle ON/OFF
        _isOn = !_isOn;
        ApplyState();

        if (progressCircle != null)
        {
            progressCircle.fillAmount = 0f;
            progressCircle.gameObject.SetActive(false);
        }

        _isProcessing = false;
    }

    private void ApplyState()
    {
        foreach (var l in lightsToToggle)
            if (l != null)
                l.enabled = _isOn;

        if (obstacle != null)
            obstacle.SetActive(!_isOn); // ON = luz prendida = obstáculo apagado
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Activación por pelota
        if (collision.collider.GetComponent<ThrowableBall>())
            Activate();
    }

}
