using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManagerEvents {

    public static event Action OnGamePaused;
    public static void PauseGame() => OnGamePaused?.Invoke();

    public static event Action OnGameStarted;
    public static void StartGame() => OnGameStarted?.Invoke();

    public static Func<int> OnScoreRequested;
    public static int GetScore() => OnScoreRequested?.Invoke() ?? 0;

}
