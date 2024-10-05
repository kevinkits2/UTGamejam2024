using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int score;
    private int aliveCreatures = 5;


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

    private void Update() {
        if (aliveCreatures <= 0) {
            PauseGame();
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
