using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour {

    [SerializeField] private AudioMixer audioMixer;

    private const string VolumeKey = "AudioVolume";

    public Action OnVolumeChange;


    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat(VolumeKey, -10));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(PlayerPrefs.GetFloat(VolumeKey));
        audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat(VolumeKey));
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save(); // Make sure to call Save to persist the changes immediately
        OnVolumeChange?.Invoke();
    }
}
