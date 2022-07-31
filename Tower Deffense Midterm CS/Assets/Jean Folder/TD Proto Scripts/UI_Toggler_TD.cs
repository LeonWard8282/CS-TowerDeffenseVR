using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Toggler_TD : MonoBehaviour
{
    public GameObject  wristUI;
    public bool toggle = true;
    //public Canvas lives_CanvasWrist;
    //public Canvas builder_CanvasShopWrist;
    //public Canvas waveCountDownTimerWrist;
    //public Canvas playerHealthUI;
    
    // Start is called before the first frame update
    void Start()
    {
        //lives_CanvasWrist = GetComponent<Canvas>();
        //builder_CanvasShopWrist = GetComponent<Canvas>();
        //waveCountDownTimerWrist = GetComponent<Canvas>();
        //playerHealthUI = GetComponent<Canvas>();
        wristUI = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        TogglingWristUI();
    }

    public void TogglingWristUI()
    {


        //lives_CanvasWrist.enabled = !lives_CanvasWrist;
        //builder_CanvasShopWrist.enabled = !builder_CanvasShopWrist;
        //waveCountDownTimerWrist.enabled = !waveCountDownTimerWrist;
        //playerHealthUI.enabled = !playerHealthUI;

        if(toggle =!toggle)
        {
            wristUI.SetActive(toggle);

        }

    }


}
