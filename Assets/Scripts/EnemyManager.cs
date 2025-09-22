using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject Prefab;
    public int EnemyCount;
    public List<GameObject> spawners;
    private void Start()
    {
        SpawnEnemies();
    }
    void SpawnEnemies()
    { 
        for(int i = 0; i < EnemyCount; i++) 
        { 
            // GameObject ins = Instantiate(Prefab, spawners[Random.Range (0,spawners.Count)].transform.position, Quaternion.identity);
            // ins.GetComponent<NavMeshAgent>().SetDestination(GameObject.FindWithTag("Player").transform.position);

            Instantiate(Prefab, spawners[Random.Range(0, spawners.Count)].transform.position, Quaternion.identity);
        }
    }  
}
