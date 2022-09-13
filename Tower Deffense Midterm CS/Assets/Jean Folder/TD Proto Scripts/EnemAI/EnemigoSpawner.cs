using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoSpawner : MonoBehaviour
{
    public Transform player; // assigning this to the enemy that is spaned. 
    public int NumberOfEnemiesToSpawn = 4;
    public float SpawnDelay = 1f;
    public List<EnemyScriptableObject> Enemies = new List<EnemyScriptableObject>();
    public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;

    public GameObject[] spawnlocations;
    public string spawnPointTagName = "spawnpoint";

    private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    private NavMeshTriangulation triangulation;

    private void Awake()
    {
        //spawnlocations = GameObject.FindGameObjectsWithTag(spawnPointTagName);

        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(Enemies[i].Prefab, NumberOfEnemiesToSpawn));
        }
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        spawnlocations = GameObject.FindGameObjectsWithTag(spawnPointTagName);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(SpawnDelay);

        int SpawnedEnemies = 0;

        while (SpawnedEnemies < NumberOfEnemiesToSpawn)
        {
            if(EnemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(SpawnedEnemies);
            }
            else if(EnemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }
            else if(EnemySpawnMethod == SpawnMethod.SetSpawnLocation)
            {
                int SpawnIndex = SpawnedEnemies % Enemies.Count;
                SpawnEnemyAtSetLocation(SpawnIndex);
            }

            SpawnedEnemies++;
            yield return wait;
        }

    }

    private void SpawnEnemyAtSetLocation(int spawnIndex)
    {

       
        PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();

        //int spawn = UnityEngine.Random.Range(0, spawnlocations.Length - 1);

        if (poolableObject != null)
        {
            Enemigo enemy = poolableObject.GetComponent<Enemigo>();
            Enemies[spawnIndex].Setupenemy(enemy);

          

            enemy.agent.Warp(spawnlocations[UnityEngine.Random.Range(0, spawnlocations.Length)].transform.position);
            enemy.movement.player = player;
            enemy.movement.triangulation = triangulation;
            enemy.agent.enabled = true;
            enemy.movement.Spawn();
        }
        else
        {
            Debug.LogError($"unable to fetch enemy of type {spawnIndex} from object pool. ou tof objects;");

        }




    }

    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int SpawnIndex = spawnedEnemies % Enemies.Count; // if zero spawned enemies and two enemy prefag you get a zero result. 0 % 2 = 0 
        DoSpawnEnemy(SpawnIndex);                                              //
    }

    private void SpawnRandomEnemy( )
    {
        DoSpawnEnemy(UnityEngine.Random.Range(0, Enemies.Count));
    }


    private void DoSpawnEnemy(int spawnIndex)
    {
        PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();
        

        if(poolableObject != null)
        {
            Enemigo enemy = poolableObject.GetComponent<Enemigo>();
            Enemies[spawnIndex].Setupenemy(enemy);


            int VertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);

            NavMeshHit Hit;

            if(NavMesh.SamplePosition(triangulation.vertices[VertexIndex], out Hit, 2f, -1))
            {
                enemy.agent.Warp(Hit.position);
                //Enemy needs to get enabled and start chasing now. 
                enemy.movement.player = player;
                enemy.movement.triangulation = triangulation;
                enemy.agent.enabled = true;
                enemy.movement.Spawn();

            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgeent on Navement. Tried to use {triangulation.vertices[VertexIndex]} ");
            }
        }
        else
        {
            Debug.LogError($"unable to fetch enemy of type {spawnIndex} from object pool. ou tof objects;" );

        }


    }

    public enum SpawnMethod
    {
        RoundRobin,
        Random,
        SetSpawnLocation

    }


}
