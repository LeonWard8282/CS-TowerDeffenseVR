using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenue : MonoBehaviour
{
    public GameObject userInterFacePauseMenue;


    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {

            Toggle();

        }


    }

    public void Toggle()
    {
        userInterFacePauseMenue.SetActive(!userInterFacePauseMenue.activeSelf);

        if(userInterFacePauseMenue.activeSelf)
        {
            Time.timeScale = 0f;
            


        }
        else
        {
            Time.timeScale = 1f;

        }

    }


    public void Retry()
    {
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void Menu()
    {

        Debug.Log("Go to Menue");
    }


}
