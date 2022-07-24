using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }



    public void PurchaseStandardTurret()
    {
        Debug.Log("Standard Turret Purchased");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }

    public void PurchasedMissleTurret()
    {
        Debug.Log("Missel Turret Purchased");
        buildManager.SetTurretToBuild(buildManager.MissledTurretPrefab);
    }

    public void PurchasedLazerTurret()
    {
        Debug.Log("Lazere Turret Purchased");
        buildManager.SetTurretToBuild(buildManager.LazerTurretPrefab);
    }

}
