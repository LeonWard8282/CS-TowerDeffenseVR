using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject Human1;

    public Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate("Human1", spawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
