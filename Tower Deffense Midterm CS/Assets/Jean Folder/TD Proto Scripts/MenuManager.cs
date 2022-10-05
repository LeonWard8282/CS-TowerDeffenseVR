using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public string levelToLoad = "Next Level";
    public string MainMenue_Lobby = "MainMenus";
    public SceneFader sceneFader;
    public Wrist_UI_Manager wristUIManager;
    

    public void Quit()
    {
        Debug.Log("Exciting the game");
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueToNextLevel()
    {
        sceneFader.FadeTo(levelToLoad);

    }

    public void MainMenu_Lobby()
    {
        sceneFader.FadeTo(MainMenue_Lobby);

    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        wristUIManager.SeHandUIStateTo_GamePlay();

    }

    public void Continue()
    {
        wristUIManager.SeHandUIStateTo_GamePlay();
    }

}
