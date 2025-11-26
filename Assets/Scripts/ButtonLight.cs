using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ButtonLight : MonoBehaviour
{
    [Header("Luces a controlar")]
    public Light[] luces;

    [Header("Configuración")]
    public float tiempoEspera = 1.0f;

    [Header("UI de carga (opcional)")]
    public Image imagenCarga;

    [Header("Barreras para el watcher")]
    public NavMeshObstacle[] barreras;

    public GameObject pared;

    private bool estaOcupado = false;

    private void Start()
    {
        if (imagenCarga != null)
        {
            imagenCarga.gameObject.SetActive(false);
        }

        ActualizarBarreras(EstanLucesEncendidas());
    }

    public void Interactuar()
    {
        if (estaOcupado) return;

        StartCoroutine(ToggleLuzRoutine());
    }

    private IEnumerator ToggleLuzRoutine()
    {
        estaOcupado = true;
        pared.SetActive(false);

        if (imagenCarga != null)
            imagenCarga.gameObject.SetActive(true);

        // Esperar el tiempo de "carga"
        yield return new WaitForSeconds(tiempoEspera);

        // Invertir el estado de cada luz individualmente
        if (luces != null)
        {
            foreach (var l in luces)
            {
                if (l == null) continue;
                l.enabled = !l.enabled;
            }
        }

        ActualizarBarreras(EstanLucesEncendidas());

        if (imagenCarga != null)
            imagenCarga.gameObject.SetActive(false);

        estaOcupado = false;
    }

    private bool EstanLucesEncendidas()
    {
        if (luces == null || luces.Length == 0 || luces[0] == null)
            return false;

        return luces[0].enabled;
    }

    private void ActualizarBarreras(bool activar)
    {
        if (barreras == null) return;

        bool activarBarreras = !activar;

        foreach (var obs in barreras)
        {
            if (obs == null) continue;

            obs.enabled = activarBarreras;
        }
    }
}
