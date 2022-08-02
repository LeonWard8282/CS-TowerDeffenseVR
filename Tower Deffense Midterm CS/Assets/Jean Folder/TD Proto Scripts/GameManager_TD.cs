using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TD : MonoBehaviour
{
    public UI_Toggler_TD toggle;
    public static bool GameIsOver;
    public GameObject gameOverUI;
    public GameObject Winning_UI;
 

    public string nextLevel = "Level02";
    public int levelToUnlock = 1;

    public SceneFader sceneFader;


     void Start()
    {
        GameIsOver = false;
    }



    void Update()
    {
        if(GameIsOver)
        {
            return;
        }

        if(PlayerStats.Lives <= 0)
        {
            EndGame();
        }

    }

    void EndGame()
    {
        GameIsOver = true;
        Debug.Log("Game Over!");

    
        toggle.GameWonOrLose();


    }

    public void WinLevel()
    {
        Debug.Log("Level Won!!!");
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        //sceneFader.FadeTo(nextLevel);
        Winning_UI.SetActive(true);
        toggle.GameWonOrLose();
    }

}



   
