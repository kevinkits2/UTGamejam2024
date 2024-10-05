using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour {

    [SerializeField] private GameObject creaturePrefab;

    private CreatureState currentState;
    [SerializeField] private LayerMask creatureLayerMask;
    [SerializeField] private float multiplyTime;
    private float multiplyTimer;
    [SerializeField] private int hunger;
    [SerializeField] private int hungerStart = 70;
    [SerializeField] private int rageStart = 0;
    [SerializeField] private float rageEnemySearchDistance = 5f;
    [SerializeField] private float rageEnemySearchTime = 0.5f;
    private float rageEnemySearchTimer = 0;
    private bool readyToSearch;
    [SerializeField] private float rageAttackCooldown = 4f;
    private bool rageAttackOnCooldown;
    private Transform rageTarget;
    private Coroutine rageAttackCooldownRoutine;
    [SerializeField] float killDistance = 0.2f;

    private NavMeshAgent agent;
    [SerializeField] private float wanderTime = 3f;
    [SerializeField] private float maxWanderDistance = 3f;
    private float fedWanderTime = 3f;
    private float fedMaxWanderDistance = 3f;
    private float hungryWanderTime = 1.5f;
    private float hungryMaxWanderDistance = 1.5f;
    private float wanderTimer = 0f;


    private bool readyToWander;
    private float hungerDepleteTime = 1f;
    private Coroutine hungerCoroutine;


    private void Awake() {
        currentState = CreatureState.Fed;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        //CreatureEvents.OnCreatureStateChange += ChangeState;

        hungerCoroutine = StartCoroutine(HungerRoutine());
    }

    private void OnDestroy() {
        if (hungerCoroutine != null) {
            StopCoroutine(hungerCoroutine);
        }
        
        if (rageAttackCooldownRoutine != null) {
            StopCoroutine(rageAttackCooldownRoutine);
        }
    }

    public void ChangeState(CreatureState state, Transform transform) {
        currentState = state;

        switch (currentState) {
            case CreatureState.Fed:
                wanderTime = fedWanderTime;
                maxWanderDistance = fedMaxWanderDistance;
                multiplyTimer = 0f;
                break;

            case CreatureState.Hungry:
                wanderTime = hungryWanderTime;
                maxWanderDistance = hungryMaxWanderDistance;
                break;

            case CreatureState.Rage:
                agent.speed = agent.speed * 2;
                readyToSearch = true;
                break;
        }
    }

    private void Update() {
        transform.forward = Camera.main.transform.forward; // Make sprite look at camera

        if (currentState == CreatureState.Fed) {
            multiplyTimer += Time.deltaTime;
            if (multiplyTimer >= multiplyTime) {
                Multiply();
                multiplyTimer = 0f;
            }
        }

        if (!readyToWander) {
            wanderTimer += Time.deltaTime;
            if (wanderTimer > wanderTime) {
                readyToWander = true;
                wanderTimer = 0f;
            }
        }

        if (currentState == CreatureState.Rage) {
            if (!readyToSearch && !rageAttackOnCooldown) {
                rageEnemySearchTimer += Time.deltaTime;
                if (rageEnemySearchTimer > rageEnemySearchTime) {
                    readyToSearch = true;
                    rageEnemySearchTimer = 0f;
                }
            }
        }

        HungerCheck();
        StateBehaviour();
    }

    private void StateBehaviour() {
        switch (currentState) {
            case CreatureState.Fed:
                Wander();
                break;

            case CreatureState.Hungry:
                Wander();
                break;

            case CreatureState.Rage:
                Rage();
                TargetScan();
                
                if (rageTarget == null && !rageAttackOnCooldown && !readyToSearch) {
                    Wander();
                }

                break;
        }
    }

    private void HungerCheck() {
        if (currentState == CreatureState.Fed && hunger <= hungerStart) {
            //CreatureEvents.ChangeCreatureState(CreatureState.Hungry, transform);
            ChangeState(CreatureState.Hungry, transform);
        }
        else if (currentState == CreatureState.Hungry) {
            if (hunger > hungerStart) {
                //CreatureEvents.ChangeCreatureState(CreatureState.Fed, transform);
                ChangeState(CreatureState.Fed, transform);
            }
            else if (hunger <= rageStart) {
                //CreatureEvents.ChangeCreatureState(CreatureState.Rage, transform);
                ChangeState(CreatureState.Rage, transform);
            }
        }
    }

    private void Multiply() {
        Instantiate(creaturePrefab, transform.position, Quaternion.identity);
    }

    private void Wander() {
        if (readyToWander) {
            agent.destination = GetRandomPointOnNavmesh();

            readyToWander = false;
        }
    }

    private void Rage() {
        if (rageTarget == null) return;

        agent.destination = rageTarget.position;

        if (Vector3.Distance(transform.position, rageTarget.transform.position) < killDistance) {
            Destroy(rageTarget.gameObject);
            rageTarget = null;
        }
    }

    private void TargetScan() {
        if (!readyToSearch) return;
        readyToSearch = false;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, rageEnemySearchDistance / 2, transform.forward, 1f, creatureLayerMask);
        float closestTargetDistance = Mathf.Infinity;
        Creature closestTarget = null;

        foreach (RaycastHit hit in hits) {
            Debug.Log(hit);
            if (!hit.transform.TryGetComponent<Creature>(out Creature creature)) return;
            if (creature == this) continue;
            if (Vector3.Distance(creature.transform.position, transform.position) < closestTargetDistance) {
                closestTarget = creature;
            } 
        }

        if (closestTarget != null) {
            rageTarget = closestTarget.transform;
            rageAttackCooldownRoutine = StartCoroutine(RageAttackCooldownRoutine());
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

    private IEnumerator RageAttackCooldownRoutine() {
        rageAttackOnCooldown = true;

        yield return new WaitForSeconds(rageAttackCooldown);

        rageAttackOnCooldown = false;
    }
}

public enum CreatureState {
    Fed,
    Hungry,
    Rage
}
    