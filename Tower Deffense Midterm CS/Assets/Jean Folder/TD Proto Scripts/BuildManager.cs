using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //singleton pattern to ensure one instance on the scene. Easy to access this instance
    public static BuildManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More than one build manager in scene");
            return;
        }
        instance = this;    
    }


    [Header("Standard Turret Prefab")]
    public GameObject standardTurretPrefab;
    [Header("Missle Turret Prefab")]
    public GameObject MissledTurretPrefab;
    [Header("Lazer Turret Prefab")]
    public GameObject LazerTurretPrefab;

  

    private GameObject turretToBuild;

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
    }


}
