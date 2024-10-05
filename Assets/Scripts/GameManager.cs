using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int score;


    private void Awake() {
        PauseGame();
    }
    private void Start() {
        GameManagerEvents.OnGamePaused += PauseGame;
        GameManagerEvents.OnGameStarted += StartGame;
        CreatureEvents.OnGeneratePoints += OnGeneratePoints;
        GameManagerEvents.OnScoreRequested += HandleScoreRequested;
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
