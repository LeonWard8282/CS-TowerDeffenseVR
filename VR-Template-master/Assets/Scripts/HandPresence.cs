using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    //variable to display controller or the hands
    public bool showController = false;
    public InputDeviceCharacteristics controllerChracteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandMovel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();

    }

    void TryInitialize()
    {
        List<InputDevice> listOfInputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerChracteristics, listOfInputDevices);

        foreach (var item in listOfInputDevices)
        {
            Debug.Log(item.name + item.characteristics);

        }

        if (listOfInputDevices.Count > 0)
        {
            targetDevice = listOfInputDevices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandMovel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandMovel.GetComponent<Animator>();
        }
    }


    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (!targetDevice.isValid)
        {
            TryInitialize();

        }
        else
        {
            if (showController)
            {
                spawnedHandMovel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandMovel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
        // This is how you can access the buttons triggers or 2daxis 
        //if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        //{ Debug.Log("pressing Primary Button");
        //}

        //if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        //{ Debug.Log("Trigger pressed" + triggerValue);
        //}

        //if( targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero) 
        //{ Debug.Log("Primary TouchPad" + primary2DAxisValue);
        //}

    }
}
