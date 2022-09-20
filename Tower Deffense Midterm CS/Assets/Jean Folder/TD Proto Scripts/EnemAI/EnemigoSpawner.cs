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
    public List<WeightedSpawnScriptableObject> weightedEnemies = new List<WeightedSpawnScriptableObject>();
    public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;
    [Space]
    [Tooltip("If true, it will spawn Enemies Continuosly, wave after wave with out the need to clear the wave. If False" +
        "you will need to clear the wave first before any more enemies spawn")]
    public bool continuousSpawning;
    public ScalingScriptableObject scaling;
    public GameObject[] spawnlocations;
    [Space]
    public string spawnPointTagName = "spawnpoint";

    private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    private NavMeshTriangulation triangulation;

    [SerializeField]
    private int level = 0;

    [SerializeField]
    private List<EnemyScriptableObject> scaledEnemies = new List<EnemyScriptableObject>();

    [SerializeField]
    private float[] weights;

    [SerializeField]
    private int enemiesAlive = 0;
    private int spawnedEnemies = 0;
    private int intialEnemiesToSpawn;
    private float initialSpawnDelay;


    private void Awake()
    {
        //spawnlocations = GameObject.FindGameObjectsWithTag(spawnPointTagName);

        for (int i = 0; i < weightedEnemies.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(weightedEnemies[i].enemy.Prefab, NumberOfEnemiesToSpawn));
        }


        weights = new float[weightedEnemies.Count];

        intialEnemiesToSpawn = NumberOfEnemiesToSpawn;
        initialSpawnDelay = SpawnDelay;

    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        spawnlocations = GameObject.FindGameObjectsWithTag(spawnPointTagName);

        for(int i = 0; i < weightedEnemies.Count; i++)
        {


            scaledEnemies.Add(weightedEnemies[i].enemy.ScaleUpForLevel(scaling, 0));

        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        level++;
        spawnedEnemies = 0;
        enemiesAlive = 0;

        for(int i = 0; i < weightedEnemies.Count; i++)
        {
            scaledEnemies[i] = weightedEnemies[i].enemy.ScaleUpForLevel(scaling, level);

        }

        ResetSpawnWheights();

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
                int SpawnIndex = SpawnedEnemies % weightedEnemies.Count;
                SpawnEnemyAtSetLocation(SpawnIndex);
            }
            else if(EnemySpawnMethod == SpawnMethod.WeightedRandom)
            {
                SpawnWeightedRandomEnemy();

            }
            else if (EnemySpawnMethod == SpawnMethod.SetSpawnLocationWeightedRandom)
            {
                SpawnWeightedEnemy_AtSetLocation();
            }

            SpawnedEnemies++;
            yield return wait;
        }

        if(continuousSpawning)
        {
            ScaledUpSpawns();
            StartCoroutine(SpawnEnemies());
        }


    }

    private void SpawnWeightedEnemy_AtSetLocation()
    {
        float value = UnityEngine.Random.value;

        for (int i = 0; i < weights.Length; i++)
        {
            if (value < weights[i])
            {
                //DoSpawnEnemy(i, ChooseRandomPositionOnNavMesh());
                SpawnEnemyAtSetLocation(i);
                return;
            }

            value -= weights[i];

        }
        Debug.Log("Invalid configuratio! could not spawn a weighted random enemy. Did you forget to call resetSpawnWeights()?");

    }

    private void SpawnWeightedRandomEnemy()
    {
        float value = UnityEngine.Random.value;

        for (int i = 0; i < weights.Length; i++)
        {
            if(value < weights[i])
            {
                DoSpawnEnemy(i, ChooseRandomPositionOnNavMesh());
                return;
            }

            value -= weights[i];

        }
        Debug.Log("Invalid configuratio! could not spawn a weighted random enemy. Did you forget to call resetSpawnWeights()?");

    }

    private void ResetSpawnWheights()
    {
        float totalWeight = 0;
        for (int i = 0; i < weightedEnemies.Count; i++)
        {
            weights[i] = weightedEnemies[i].GetWeight();
            totalWeight += weights[i];


        }

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = weights[i] / totalWeight;
        }  


    }



    private void SpawnEnemyAtSetLocation(int spawnIndex)
    {

       
        PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();

        //int spawn = UnityEngine.Random.Range(0, spawnlocations.Length - 1);

        if (poolableObject != null)
        {
            Enemigo enemy = poolableObject.GetComponent<Enemigo>();
            weightedEnemies[spawnIndex].enemy.Setupenemy(enemy);

          

            enemy.agent.Warp(spawnlocations[UnityEngine.Random.Range(0, spawnlocations.Length)].transform.position);
            enemy.movement.player = player;
            enemy.movement.triangulation = triangulation;
            enemy.agent.enabled = true;
            enemy.movement.Spawn();
            enemy.OnDie += HandleEnemyDeath;


            enemiesAlive++;

        }
        else
        {
            Debug.LogError($"unable to fetch enemy of type {spawnIndex} from object pool. ou tof objects;");

        }




    }

    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int SpawnIndex = spawnedEnemies % weightedEnemies.Count; // if zero spawned enemies and two enemy prefag you get a zero result. 0 % 2 = 0 
        DoSpawnEnemy(SpawnIndex , ChooseRandomPositionOnNavMesh());                                              //
    }

    private void SpawnRandomEnemy( )
    {
        DoSpawnEnemy(UnityEngine.Random.Range(0, weightedEnemies.Count), ChooseRandomPositionOnNavMesh());
    }

    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        int VertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);
        return triangulation.vertices[VertexIndex];

    }

    public void DoSpawnEnemy(int spawnIndex, Vector3 spawnPosition)
    {
        PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();
        

        if(poolableObject != null)
        {
            Enemigo enemy = poolableObject.GetComponent<Enemigo>();
            scaledEnemies[spawnIndex].Setupenemy(enemy);



            NavMeshHit Hit;

            if(NavMesh.SamplePosition(spawnPosition, out Hit, 2f, -1))
            {
                enemy.agent.Warp(Hit.position);
                //Enemy needs to get enabled and start chasing now. 
                enemy.movement.player = player;
                enemy.movement.triangulation = triangulation;
                enemy.agent.enabled = true;
                enemy.movement.Spawn();
                enemy.OnDie += HandleEnemyDeath;

                enemiesAlive++;

            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgeent on Navement. Tried to use {spawnPosition} ");
            }
        }
        else
        {
            Debug.LogError($"unable to fetch enemy of type {spawnIndex} from object pool. ou tof objects;" );

        }


    }


    private void ScaledUpSpawns()
    {
        NumberOfEnemiesToSpawn = Mathf.FloorToInt(intialEnemiesToSpawn * scaling.spawnCountCurve.Evaluate(level + 1));
        SpawnDelay = initialSpawnDelay * scaling.SpawnRateCurve.Evaluate(level + 1);

    }



    private void HandleEnemyDeath(Enemigo enemy)
    {
        enemiesAlive--;
        Debug.Log("HandleEnemyDeath is being passed trhough");
        if(enemiesAlive <= 0 && spawnedEnemies == NumberOfEnemiesToSpawn)
        {
            ScaledUpSpawns();
            StartCoroutine( SpawnEnemies() );
        }

    }



    public enum SpawnMethod
    {
        RoundRobin,
        Random,
        SetSpawnLocation,
        WeightedRandom,
        SetSpawnLocationWeightedRandom

    }


}
