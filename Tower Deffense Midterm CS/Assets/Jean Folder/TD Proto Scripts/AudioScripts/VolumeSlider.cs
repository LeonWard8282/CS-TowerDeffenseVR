using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    [SerializeField] private string _soundName;
    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(_soundName,val));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
