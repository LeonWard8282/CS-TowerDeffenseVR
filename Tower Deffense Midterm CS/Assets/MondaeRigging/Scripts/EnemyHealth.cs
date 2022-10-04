using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHealth : MonoBehaviour
{
    public float Health;
    public GameObject xpDrop;
    public SpawnManager1 spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            var randNum = Random.Range(0, 4); // this will return a number between 0 and 9 
            for (var i = 0; i < randNum; i++)
            {
                PhotonNetwork.Instantiate("Loot1b", transform.position, Quaternion.identity);
            }
            Invoke(nameof(DestroyEnemy), 1f);
        }
    }

    private void DestroyEnemy()
    {
        if(this.gameObject.CompareTag("Enemy"))
        {
            spawnManager.enemyCount--;
        }
        if (this.gameObject.CompareTag("Security"))
        {
            spawnManager.securityCount--;
        }
        PhotonNetwork.Destroy(gameObject);
    }
}
