using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{
    public AudioClipGroup menuMusic;
    public AudioClipGroup gameMusic;
    public AudioClipGroup buttonClick;
    public AudioClipGroup creatureEaten;
    public AudioClipGroup bugSplat;
    public AudioClipGroup bugCrush;

    public static AudioPlayer Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(Instance);
    }

    void Start() {
        PlayMenuMusic();
    }

    private void OnEnable()
    {
        AudioSourcePool.Instance.ResetAudioSources();
        AudioSourcePool.Instance.GetSource();
    }

    public void ButtonClicked()
    {
        buttonClick.Play();
    }

    public void PlayMenuMusic()
    {
        menuMusic.Play();
    }

    public void PlayGameMusic()
    {
        gameMusic.Play();
    }

    public void PlayCreatureEaten()
    {
        creatureEaten.Play();
    }

    public void PlayBugSplat()
    {
        bugSplat.Play();
    }

    public void PlayBugCrush()
    {
        bugCrush.Play();
    }
}
