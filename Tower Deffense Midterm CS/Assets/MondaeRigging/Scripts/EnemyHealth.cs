using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHealth : MonoBehaviour
{
    public float Health;
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
            Invoke(nameof(DestroyEnemy), 1f);
        }
    }

    private void DestroyEnemy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
