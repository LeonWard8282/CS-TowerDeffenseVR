using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TD : MonoBehaviour
{
    public static bool GameIsOver;

     void Start()
    {
        GameIsOver = false;
    }


    public GameObject gameOverUI;

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

    }



}



   
