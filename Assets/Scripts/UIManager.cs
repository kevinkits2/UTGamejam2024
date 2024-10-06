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

    [SerializeField] private Button openTutorialButton;
    [SerializeField] private Button closeTutorialButton;

    [SerializeField] private GameObject pauseUIHolder;
    [SerializeField] private GameObject menuUIHolder;
    [SerializeField] private GameObject gameUIHolder;
    [SerializeField] private GameObject overUIHolder;
    [SerializeField] private GameObject tutorialUIHolder;
    [SerializeField] private GameObject tutorialButtonHolder;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameoverscore;

    [SerializeField] private Animator door1Animator;
    [SerializeField] private Animator door2Animator;


    private bool isPaused = false;

    private void Awake()
    {
        //Start game from menu button
        startButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            Time.timeScale = 1.0f;
            door1Animator.SetTrigger("Open");
            door2Animator.SetTrigger("Open");
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
            SceneManager.LoadScene("Main");
            //HandlePause();
            //gameUIHolder.SetActive(false);
            //menuUIHolder.SetActive(true);
            //overUIHolder.SetActive(false);
            //tutorialButtonHolder.SetActive(true);
            //GameManagerEvents.PauseGame();
        });

        //Restart from game end button
        restartButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            SceneManager.LoadScene("Main");
            //overUIHolder.SetActive(false);
            //Some (Re)Start game call here
            //gameUIHolder.SetActive(false);
            //menuUIHolder.SetActive(true);
            //Destroy(FindObjectOfType<Canvas>());
            //SceneManager.LoadScene("Main");
            //GameManagerEvents.ResetScene();
        });

        //To Menu from game end button
        menuEndButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            SceneManager.LoadScene("Main");
            //gameUIHolder.SetActive(false);
            //overUIHolder.SetActive(false);
            //menuUIHolder.SetActive(true);
            //tutorialButtonHolder.SetActive(true);
            //Destroy(FindObjectOfType<Canvas>());
        });

        openTutorialButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            tutorialUIHolder.SetActive(true);
            tutorialButtonHolder.SetActive(false);
        });

        closeTutorialButton.onClick.AddListener(() => {
            AudioPlayer.Instance.ButtonClicked();
            tutorialUIHolder.SetActive(false);
            tutorialButtonHolder.SetActive(true);
        });
    }

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
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
        CreatureEvents.OnGeneratePoints -= OnGeneratePoints;
        GameManagerEvents.OnGameOver -= HandleGameOver;
    }

    private void HandlePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            tutorialButtonHolder.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            tutorialButtonHolder.SetActive(true);
        }
        isPaused = !isPaused;
        pauseUIHolder.SetActive(isPaused);
    }

    public void StartGame() {
        menuUIHolder.SetActive(false);
        gameUIHolder.SetActive(true);
        tutorialButtonHolder.SetActive(false);
        door1Animator.SetTrigger("Started");
        door2Animator.SetTrigger("Started");
    }
}
