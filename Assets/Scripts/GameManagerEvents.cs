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

    public static event Action OnFoodButtonPressed;
    public static void FoodButtonPress() => OnFoodButtonPressed?.Invoke();

    public static event Action OnMouseDown;
    public static void MouseDown() => OnMouseDown?.Invoke();

    public static event Action OnMouseUp;
    public static void MouseUp() => OnMouseUp?.Invoke();

    public static event Action OnFoodDragged;
    public static void DragFood() => OnFoodDragged?.Invoke();

    public static event Action OnFoodDraggedStopped;
    public static void StopFoodDrag() => OnFoodDraggedStopped?.Invoke();

}
