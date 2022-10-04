using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel_TD : MonoBehaviour
{
    public SceneFader sceneFader;
    public string menuSceneName;
    public string nextLevel;


    public void Continue()
    {

        sceneFader.FadeTo(nextLevel);

    }

 

   
    public void Menu()
    {
        sceneFader.FadeTo(menuSceneName);

    }

}
