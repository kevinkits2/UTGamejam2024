using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int score;
    private int aliveCreatures = 5;
    private bool mouseDown = false;
    private bool foodButtonPressed = false;

    private FoodButton foodButton;


    private void Awake() {
        //PauseGame();
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

    private void Update() {
        if (aliveCreatures <= 0) {
            //PauseGame();
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
