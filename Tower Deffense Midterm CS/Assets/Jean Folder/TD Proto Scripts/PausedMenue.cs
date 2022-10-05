using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenue : MonoBehaviour
{
    public Wrist_UI_Manager WristUIManager;
    public string MainMenueScene;


    public void Retry()
    {

        if(Time.timeScale == 0)
        {
            PlayerStats.Rounds = 0;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }

        else
        {
            PlayerStats.Rounds = 0;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }


    }

    public void Menu()
    {
        
        SceneManager.LoadScene(MainMenueScene);
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        // have everything go back to motion, 
        WristUIManager.SetHandUIStateTo_GameWonState();

        //return menue to previous screen



    }


}
