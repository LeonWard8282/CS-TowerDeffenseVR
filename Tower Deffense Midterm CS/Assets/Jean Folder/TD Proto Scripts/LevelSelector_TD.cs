using UnityEngine;

public class LevelSelector_TD : MonoBehaviour
{
    public SceneFader fader;

    public void Select(string levelName)
        {

        fader.FadeTo(levelName);
        }


}
