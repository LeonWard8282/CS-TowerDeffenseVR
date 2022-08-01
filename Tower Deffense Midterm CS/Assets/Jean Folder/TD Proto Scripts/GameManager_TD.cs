using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TD : MonoBehaviour
{
    public static bool GameIsOver;
    public GameObject gameOverUI;

    public GameObject playerHealth_UI;
    public GameObject waveCountDown_UI;
    public GameObject livesCountDown_UI;
    public GameObject money_UI;
    public GameObject builderCanvas_UI;

    public string nextLevel = "Level02";
    public int levelToUnlock = 2;

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

        gameOverUI.SetActive(true);
        waveCountDown_UI.SetActive(false);
        livesCountDown_UI.SetActive(false);
        money_UI.SetActive(false);
        builderCanvas_UI.SetActive(false);


    }

    public void WinLevel()
    {
        Debug.Log("Level Won!!!");
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        sceneFader.FadeTo(nextLevel);
    }

}



   
