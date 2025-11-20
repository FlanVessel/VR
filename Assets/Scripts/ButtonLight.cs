using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour
{
    [Header("Luces a controlar")]
    public Light[] luces;

    [Header("Configuración")]
    public float tiempoEspera = 1.5f;

    [Header("UI de Carga")]
    public Image imagenCarga; // imagen encima del botón

    private bool lucesEncendidas = true;
    private bool estaOcupado = false;

    public void Interactuar()
    {
        if (estaOcupado) return; // evita spamear el botón

        StartCoroutine(CambiarLuzConEspera());
    }

    private IEnumerator CambiarLuzConEspera()
    {
        estaOcupado = true;

        // Mostrar imagen de carga
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
