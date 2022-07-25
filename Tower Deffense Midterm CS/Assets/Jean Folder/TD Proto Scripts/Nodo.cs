using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Nodo : MonoBehaviour
{
    [Header("Hover Color")]
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Material startMaterial;
    public Material emptyMaterial;
    public Material highLightedMaterial;

    public Vector3 positionOffset;
    
    [Header("Optional")]
    public GameObject turret;


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

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    public void OnButtonPressActivation()
    {
        


        if(turret !=null)
        {

            buildManager.SelectedNode(this);
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }
        buildManager.BuildTurretOn(this);

    }

    //*
    //      WHen the XR Ray Hovers onto the Node
    //*

    public void OnXRRayEnter()
    {
        //only highlight where to build a turret when we have a turret selected
      

        if(!buildManager.CanBuild)
        {
            return;
        }

        if(buildManager.HasMoney)
        {
            GetComponent<Renderer>().material = highLightedMaterial;
            GetComponent<Renderer>().material.color = hoverColor;
        }
        else
        {
            GetComponent<Renderer>().material.color = notEnoughMoneyColor;
        }

        
    }

    //*
    //          WHen the XR Ray Hovers OUT of the Node
    //*
    public void OnXRRayExit()
    {
        GetComponent<Renderer>().material = startMaterial;
        rend.material.color = startColor;
    }


}
