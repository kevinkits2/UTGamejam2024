using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int score;
    private int aliveCreatures = 3;
    private bool mouseDown = false;
    private bool foodButtonPressed = false;

    private FoodButton foodButton;
    private bool resettingScene = false;

    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject canvas;


    private void Awake() {
        PauseGame();
        Instantiate(canvas);
    }

    private void Start() {
        GameManagerEvents.OnGamePaused += PauseGame;
        GameManagerEvents.OnGameStarted += StartGame;
        CreatureEvents.OnGeneratePoints += OnGeneratePoints;
        GameManagerEvents.OnScoreRequested += HandleScoreRequested;
        CreatureEvents.OnCreatureStateChange += HandleCreatureStateChanged;
        CreatureEvents.OnCreatureDeath += HandleCreatureDeath;
        GameManagerEvents.OnMouseDown += HandleMouseDown;
        GameManagerEvents.OnMouseUp += HandleMouseUp;
        GameManagerEvents.OnFoodButtonPressed += HandleFoodButtonPressed;
        GameManagerEvents.OnCreatureFeed += HandleCreatureFed;
        GameManagerEvents.OnResetScene += ResetScene;
        GameManagerEvents.OnCreatureMultiply += HandleCreatureMultiply;
    }

    private void OnDestroy() {
        GameManagerEvents.OnGamePaused -= PauseGame;
        GameManagerEvents.OnGameStarted -= StartGame;
        CreatureEvents.OnGeneratePoints -= OnGeneratePoints;
        GameManagerEvents.OnScoreRequested -= HandleScoreRequested;
        CreatureEvents.OnCreatureStateChange -= HandleCreatureStateChanged;
        CreatureEvents.OnCreatureDeath -= HandleCreatureDeath;
        GameManagerEvents.OnMouseDown -= HandleMouseDown;
        GameManagerEvents.OnMouseUp -= HandleMouseUp;
        GameManagerEvents.OnFoodButtonPressed -= HandleFoodButtonPressed;
        GameManagerEvents.OnCreatureFeed -= HandleCreatureFed;
        GameManagerEvents.OnResetScene -= ResetScene;
        GameManagerEvents.OnCreatureMultiply -= HandleCreatureMultiply;
    }

    private void HandleCreatureMultiply() {
        aliveCreatures++;
    }

    private void HandleCreatureFed() {
        if (foodButton == null) return;

        foodButton.RemoveFood();
        foodButton = null;
    }

    private void HandleFoodButtonPressed(FoodButton foodButton) {
        this.foodButton = foodButton;
        foodButtonPressed = true;
    }

    private void HandleMouseUp() {
        mouseDown = false;
    }

    private void HandleMouseDown() {
        mouseDown = true;
    }

    private void ResetScene() {
        resettingScene = true;
        CreatureEvents.GeneratePoints(0);
        score = 0;

        foreach (Creature creature in FindObjectsOfType<Creature>()) {
            Destroy(creature);
        }

        StartCoroutine(ResetSceneRoutine());

        aliveCreatures = 3;
        resettingScene = false;
    }

    private IEnumerator ResetSceneRoutine() {

        yield return new WaitForSeconds(0.1f);

        SpawnCreatures();
    }

    private void SpawnCreatures() {
        if (FindObjectsOfType<Creature>().Length == 0) {
            for (int i = 0; i < 3; i++) {
                Instantiate(creaturePrefab, spawnPoints[i].position, creaturePrefab.transform.rotation);
            }
        }
    }

    private void Update() {
        if (aliveCreatures <= 0 && !resettingScene) {
            PauseGame();
            Debug.Log(aliveCreatures);
            GameManagerEvents.GameOver();
        }

        if (mouseDown && foodButtonPressed) {
            GameManagerEvents.DragFood();
        }
        else if (foodButtonPressed && !mouseDown) {
            GameManagerEvents.StopFoodDrag();
            foodButtonPressed = false;
        }
    }

    private void HandleCreatureDeath(Vector3 pos, CreatureState state) {
        if (state == CreatureState.Hungry || state == CreatureState.Fed) {
            aliveCreatures--;
        }
    }

    private void HandleCreatureStateChanged(CreatureState state, Transform transform) {
        if (state == CreatureState.Rage) {
            aliveCreatures--;
        }
    }

    private int HandleScoreRequested() {
        return score;
    }

    private void OnGeneratePoints(int points) {
        score += points;
    }

    public void PauseGame() {
        Time.timeScale = 0f;
    }

    public void StartGame() {
        Time.timeScale = 1f;
    }
}
