using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    private CreatureState currentState;
    [SerializeField] private int hunger;

    private float hungerDepleteTime = 1f;
    private Coroutine hungerCoroutine;


    private void Awake() {
        currentState = CreatureState.Fed;
    }

    private void Start() {
        hungerCoroutine = StartCoroutine(HungerRoutine());
    }

    private void OnDestroy() {
        StopCoroutine(hungerCoroutine);
    }

    public void CangeState(CreatureState state) {
        currentState = state;
    }

    private void Update() {
        StateBehaviour();
    }

    private void StateBehaviour() {
        switch (currentState) {
            case CreatureState.Fed:
                break;

            case CreatureState.Hungry:
                break;

            case CreatureState.Rage: 
                break;
        }
    }

    private void Wander() {

    }

    private IEnumerator HungerRoutine() {
        while (true) {
            yield return new WaitForSeconds(hungerDepleteTime);

            hunger--;
        }
    }
}

public enum CreatureState {
    Fed,
    Hungry,
    Rage
}
    