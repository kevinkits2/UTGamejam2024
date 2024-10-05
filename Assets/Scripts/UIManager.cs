using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Button resumePauseButton;
    [SerializeField] private Button menuPauseButton;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuEndButton;

    [SerializeField] private GameObject pauseUIHolder;
    [SerializeField] private GameObject menuUIHolder;
    [SerializeField] private GameObject gameUIHolder;
    [SerializeField] private GameObject overUIHolder;

    private bool isPaused = false;

    private void Awake()
    {
        //Start game from menu button
        startButton.onClick.AddListener(() => {
            //AudioPlayer.Instance.ButtonClicked();
            Time.timeScale = 1.0f;
            menuUIHolder.SetActive(false);
            //Some start game call here
            gameUIHolder.SetActive(true);
        });

        //Exit game from menu button
        exitButton.onClick.AddListener(() => {
            //AudioPlayer.Instance.ButtonClicked();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        });

        //Resume from pause button
        resumePauseButton.onClick.AddListener(() => {
            //AudioPlayer.Instance.ButtonClicked();
            HandlePause();
        });

        //To menu from pause button
        menuPauseButton.onClick.AddListener(() => {
            //AudioPlayer.Instance.ButtonClicked();
            HandlePause();
            gameUIHolder.SetActive(false);
            menuUIHolder.SetActive(true);
        });

        //Restart from game end button
        restartButton.onClick.AddListener(() => {
            //AudioPlayer.Instance.ButtonClicked();
            overUIHolder.SetActive(false);
            //Some (Re)Start game call here
            gameUIHolder.SetActive(true);
        });

        //To Menu from game end button
        menuEndButton.onClick.AddListener(() => {
            //AudioPlayer.Instance.ButtonClicked();
            gameUIHolder.SetActive(false);
            overUIHolder.SetActive(false);
            menuUIHolder.SetActive(true);
        });
    }

    private void Start()
    {
        //InputManager.Instance.OnPause += HandlePause;
    }

    private void OnDestroy()
    {
        //InputManager.Instance.OnPause -= HandlePause;
    }

    private void HandlePause()
    {
        if (isPaused) Time.timeScale = 1.0f;
        else Time.timeScale = 0.0f;
        isPaused = !isPaused;
        pauseUIHolder.SetActive(isPaused);
    }
}
