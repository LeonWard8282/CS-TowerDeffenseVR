using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length == 0)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(this.gameObject);
    }
}
