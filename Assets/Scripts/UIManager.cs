using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private InputManager inputManager;

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
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameoverscore;

    private bool isPaused = false;

    private void Awake()
    {
        //Start game from menu button
        startButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            Time.timeScale = 1.0f;
            menuUIHolder.SetActive(false);
            //Some start game call here
            gameUIHolder.SetActive(true);
            GameManagerEvents.StartGame();
            //GameManagerEvents.ResetScene();
        });

        //Exit game from menu button
        exitButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        });

        //Resume from pause button
        resumePauseButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            HandlePause();
            GameManagerEvents.StartGame();
        });

        //To menu from pause button
        menuPauseButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            HandlePause();
            gameUIHolder.SetActive(false);
            menuUIHolder.SetActive(true);
            overUIHolder.SetActive(false);
            GameManagerEvents.PauseGame();
        });

        //Restart from game end button
        restartButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            overUIHolder.SetActive(false);
            //Some (Re)Start game call here
            gameUIHolder.SetActive(true);
            //GameManagerEvents.ResetScene();
        });

        //To Menu from game end button
        menuEndButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            gameUIHolder.SetActive(false);
            overUIHolder.SetActive(false);
            menuUIHolder.SetActive(true);
        });
    }

    private void Start()
    {
        inputManager.OnPause += HandlePause;
        CreatureEvents.OnGeneratePoints += OnGeneratePoints;
        GameManagerEvents.OnGameOver += HandleGameOver;
    }

    private void HandleGameOver() {
        overUIHolder.SetActive(true);
        gameoverscore.text = GameManagerEvents.GetScore().ToString();
    }

    private void OnGeneratePoints(int points) {
        scoreText.text = (GameManagerEvents.GetScore() + points).ToString();
    }

    private void OnDestroy()
    {
        inputManager.OnPause -= HandlePause;
    }

    private void HandlePause()
    {
        if (isPaused) Time.timeScale = 1.0f;
        else Time.timeScale = 0.0f;
        isPaused = !isPaused;
        pauseUIHolder.SetActive(isPaused);
    }
}
