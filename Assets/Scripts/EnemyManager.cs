using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject Prefab;
    public int EnemyCount;
    public List<Transform> spawner;
    public Transform[] patrolPoints;  
    private void Start()
    {
        SpawnEnemies();
    }

    // Llamamos la funcion SpawnEnemies
    void SpawnEnemies()
    { 
        for(int i = 0; i < EnemyCount; i++)
        {
            // GameObject ins = Instantiate(Prefab, spawners[Random.Range (0,spawners.Count)].transform.position, Quaternion.identity);
            // ins.GetComponent<NavMeshAgent>().SetDestination(GameObject.FindWithTag("Player").transform.position);

            //Instantiate(Prefab, spawners[Random.Range(0, spawners.Count)].transform.position, Quaternion.identity);

            //Selecciona un spawner al azar de la lista
            Transform spawn = spawner[Random.Range(0, spawner.Count)];

            //Instancia el enemigo en la posicion y rotacion del spawner seleccionado
            GameObject e = Instantiate(Prefab, spawn.position, spawn.rotation);

            //Asigna los puntos de patrulla al enemigo instanciado  
            e.GetComponent<Enemy>().patrolPoints = patrolPoints;
        }
    }  
}
