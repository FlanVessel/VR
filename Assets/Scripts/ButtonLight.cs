using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour
{
    [Header("Luces a controlar")]
    [Tooltip("Luces que se prenderán / apagarán")]
    public Light[] luces;

    [Header("Configuración")]
    [Tooltip("Tiempo de espera antes de cambiar el estado de la luz")]
    public float tiempoEspera = 1.5f;

    [Header("UI de Carga")]
    [Tooltip("Imagen que se muestra mientras se 'carga' el prender/apagar")]
    public Image imagenCarga; // imagen encima del botón (World Space o en Canvas)

    private bool lucesEncendidas = true;
    private bool estaOcupado = false;

    /// <summary>
    /// Llamar a esta función desde el TaskManager cuando el rayo toque el botón.
    /// </summary>
    public void Interactuar()
    {
        if (estaOcupado) return; // evita spamear el botón

        StartCoroutine(CambiarLuzConEspera());
    }

    private IEnumerator CambiarLuzConEspera()
    {
        estaOcupado = true;

        // Mostrar imagen de carga (si existe)
        if (imagenCarga != null)
        {
            imagenCarga.gameObject.SetActive(true);
        }

        // Esperar el tiempo configurado
        yield return new WaitForSeconds(tiempoEspera);

        // Alternar estado de las luces
        lucesEncendidas = !lucesEncendidas;
        AplicarEstadoLuces(lucesEncendidas);

        // Ocultar imagen de carga
        if (imagenCarga != null)
        {
            imagenCarga.gameObject.SetActive(false);
        }

        estaOcupado = false;
    }

    private void AplicarEstadoLuces(bool encender)
    {
        if (luces == null) return;

        foreach (var l in luces)
        {
            if (l == null) continue;
            l.enabled = encender;
        }
    }
}
