using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text roundsText;

     void OnEnable()
    {
        roundsText.text = PlayerStats.Rounds.ToString();
    }

    public void Retry()
    {
        //TODO Link this to Prototype scene. 
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManger.LoadScene("Prototype Scene")

    }

    public void Menu()
    {

        Debug.Log("Going to Menu");
    }


}
