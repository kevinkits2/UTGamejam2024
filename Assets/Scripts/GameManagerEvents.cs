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

    public static event Action<FoodButton> OnFoodButtonPressed;
    public static void FoodButtonPress(FoodButton foodButton) => OnFoodButtonPressed?.Invoke(foodButton);

    public static event Action OnMouseDown;
    public static void MouseDown() => OnMouseDown?.Invoke();

    public static event Action OnMouseUp;
    public static void MouseUp() => OnMouseUp?.Invoke();

    public static event Action OnFoodDragged;
    public static void DragFood() => OnFoodDragged?.Invoke();

    public static event Action OnFoodDraggedStopped;
    public static void StopFoodDrag() => OnFoodDraggedStopped?.Invoke();

    public static event Action<DrawerFoodButton> OnFoodReady;
    public static void FoodReady(DrawerFoodButton button) => OnFoodReady?.Invoke(button);

    public static event Action OnFoodAdded;
    public static void FoodAdded() => OnFoodAdded?.Invoke();

    public static event Action OnMouseNotOverCreature;
    public static void MouseNotOverCreature() => OnMouseNotOverCreature?.Invoke();

    public static event Action OnCreatureFeed;
    public static void CreatureFeed() => OnCreatureFeed?.Invoke();

    public static event Action OnResetScene;
    public static void ResetScene() => OnResetScene?.Invoke();

    public static event Action OnGameOver;
    public static void GameOver() => OnGameOver?.Invoke();

    public static event Action OnCreatureMultiply;
    public static void Multiply() => OnCreatureMultiply?.Invoke();

    public static event Action<Creature> OnMouseOverCreature;
    public static void MouseOverCreature(Creature creature) => OnMouseOverCreature?.Invoke(creature);

}
