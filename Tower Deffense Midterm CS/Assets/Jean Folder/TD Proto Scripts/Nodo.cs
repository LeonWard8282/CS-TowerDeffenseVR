using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Nodo : MonoBehaviour
{
    [Header("Hover Color")]
    public Color hoverColor;
    public Material startMaterial;
    public Material emptyMaterial;
    public Material highLightedMaterial;

    public Vector3 positionOffset;
    

    private GameObject turret;


    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;



    void Start()
    {
        buildManager = BuildManager.instance;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        startMaterial = rend.material;
    }

    public void OnButtonPressActivation()
    {
        if(buildManager.GetTurretToBuild() == null)
        {
            return;
        }


        if(turret !=null)
        {
            Debug.Log("Cant Build There - TODO: Display/Error Audio que");
            return;
        }

        GameObject turretToBuild = buildManager.GetTurretToBuild();
        turret = (GameObject) Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation); // casting to gameobject

    }


    public void OnXRRayEnter()
    {
        //only highlight where to build a turret when we have a turret selected
        if(buildManager.GetTurretToBuild() == null)
        {
            return;
        }
        GetComponent<Renderer>().material = highLightedMaterial;
        GetComponent<Renderer>().material.color = hoverColor;
    }

    public void OnXRRayExit()
    {
        GetComponent<Renderer>().material = startMaterial;
        rend.material.color = startColor;
    }


}
