using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{
    public float Health = 100;
    public GameObject xpDrop;
    // Start is called before the first frame update
    void Start()
    {
        
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
            var randNum = Random.Range(0, 6); // this will return a number between 0 and 9 
            for (var i = 0; i < randNum; i++)
            {
                PhotonNetwork.Instantiate("Loot1b", transform.position, Quaternion.identity);
            }
            Invoke(nameof(DestroyEnemy), 1f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
