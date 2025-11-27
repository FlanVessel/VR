using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ButtonLight : MonoBehaviour
{
    [Header("Luces a controlar (GameObjects con Light o lo que sea)")]
    public GameObject[] luces;

    [Header("Configuración")]
    public float tiempoEspera = 1.0f;

    [Header("UI de carga (opcional)")]
    public Image imagenCarga;

    [Header("Pared / barrera a desactivar")]
    public GameObject pared;

    private bool estaOcupado = false;

    private void Start()
    {
        if (imagenCarga != null)
            imagenCarga.gameObject.SetActive(false);
    }

    public void Interactuar()
    {
        if (estaOcupado) return;

        StartCoroutine(ToggleLuzRoutine());
    }

    private IEnumerator ToggleLuzRoutine()
    {
        estaOcupado = true;

        // Mostrar carga
        if (imagenCarga != null)
            imagenCarga.gameObject.SetActive(true);

        // Cambiar luces (toggle de todos los elementos del array)
        if (luces != null)
        {
            foreach (var go in luces) // go puede ser null si no se asignó nada en el inspector
            {
                if (go == null) continue; // Saltar si es null
                go.SetActive(!go.activeSelf); // Toggle del estado activo
            }
        }

        // Desactivar la pared (o podrías hacer toggle si quieres que se vuelva a activar)
        if (pared != null)
            pared.SetActive(false);

        // Esperar el tiempo de carga solo como efecto visual
        yield return new WaitForSeconds(tiempoEspera);

        if (imagenCarga != null)
            imagenCarga.gameObject.SetActive(false);

        estaOcupado = false;
    }

}
