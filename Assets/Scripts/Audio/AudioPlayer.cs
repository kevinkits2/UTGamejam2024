using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{
    public AudioClipGroup menuMusic;
    public AudioClipGroup gameMusic;
    public AudioClipGroup buttonClick;

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

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            menuMusic.Play();
        }
        //else gameMusic.Play();
    }

    private void OnEnable()
    {
        AudioSourcePool.Instance.ResetAudioSources();
        AudioSourcePool.Instance.GetSource();

        if (SceneManager.GetActiveScene().buildIndex >= 2)
        {
            StartCoroutine(AudioCoroutine(gameMusic));
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(AudioCoroutine(menuMusic));
        }
    }

    IEnumerator AudioCoroutine(AudioClipGroup audio)
    {
        yield return new WaitForSeconds(0.1f);
        audio.Play();
    }

    public void ButtonClicked()
    {
        buttonClick.Play();
    }

    public void PlayGameMusic()
    {
        gameMusic.Play();
    }
}
