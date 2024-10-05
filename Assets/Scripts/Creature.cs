using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour {

    private CreatureState currentState;
    [SerializeField] private int hunger;

    private NavMeshAgent agent;
    [SerializeField] private float wanderTime = 3f;
    [SerializeField] private float maxWanderDistance = 3f;
    private float wanderTimer = 0f;


    private bool readyToWander;
    private float hungerDepleteTime = 1f;
    private Coroutine hungerCoroutine;


    private void Awake() {
        currentState = CreatureState.Fed;
        agent = GetComponent<NavMeshAgent>();
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
        transform.forward = Camera.main.transform.forward; // Make sprite look at camera

        wanderTimer += Time.deltaTime;
        if (wanderTimer > wanderTime) {
            readyToWander = true;
            wanderTimer = 0f;
        }

        StateBehaviour();
    }

    private void StateBehaviour() {
        Wander();

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
        if (readyToWander) {
            agent.destination = GetRandomPointOnNavmesh();

            readyToWander = false;
        }
    }

    private Vector3 GetRandomPointOnNavmesh() {
        Vector3 randomPos = Random.insideUnitSphere * maxWanderDistance + transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, maxWanderDistance, NavMesh.AllAreas);

        return hit.position;
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
    