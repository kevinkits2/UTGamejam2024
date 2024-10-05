using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance;
    public AudioSource AudioSourcePrefab;
    private List<AudioSource> audioSources;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(Instance);
        audioSources = new List<AudioSource>();
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying && !source.loop)
            {
                return source;
            }
        }
        AudioSource new_source = GameObject.Instantiate(AudioSourcePrefab);
        audioSources.Add(new_source);
        return new_source;
    }

    public AudioSource GetLooped()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying && source.loop)
            {
                return source;
            }
        }
        AudioSource new_source = GameObject.Instantiate(AudioSourcePrefab);
        audioSources.Add(new_source);
        return new_source;
    }

    public void ResetAudioSources()
    {
        audioSources.Clear();
        audioSources = new List<AudioSource>();
    }
}
