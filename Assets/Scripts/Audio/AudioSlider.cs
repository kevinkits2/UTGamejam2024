using UnityEngine.UI;
using UnityEngine;

public class VolumeSlider : MonoBehaviour {
    [SerializeField] VolumeManager volumeManager;

    void Start()
    {
        volumeManager.OnVolumeChange += HandleVolumeChange;
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("AudioVolume");
    }

    void HandleVolumeChange()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("AudioVolume");
    }

    private void OnDestroy()
    {
        volumeManager.OnVolumeChange -= HandleVolumeChange;
    }
}
