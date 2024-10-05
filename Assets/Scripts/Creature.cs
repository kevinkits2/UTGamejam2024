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
    [SerializeField] private int hunger = 100;
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
    [SerializeField] private float fedSpeed = 1;
    [SerializeField] private float hungerSpeed = 2;
    [SerializeField] private float rageSpeed = 4;
    private float wanderTimer = 0f;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private int pointsGenerated;
    [SerializeField] private float pointGenerationTime = 3f;
    private float pointGenerationTimer;


    private bool readyToWander;
    private float hungerDepleteTime = 1f;
    private Coroutine hungerCoroutine;


    private void Awake() {
        currentState = CreatureState.Fed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        readyToWander = true;
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

        CreatureEvents.CreatureDeath(transform.position);
    }

    public void ChangeState(CreatureState state, Transform transform) {
        currentState = state;

        switch (currentState) {
            case CreatureState.Fed:
                agent.speed = fedSpeed;
                multiplyTimer = 0f;
                pointGenerationTimer = 0f;
                break;

            case CreatureState.Hungry:
                agent.speed = hungerSpeed;
                break;

            case CreatureState.Rage:
                agent.speed = rageSpeed;
                readyToSearch = true;
                break;
        }
    }

    private void Update() {
        transform.forward = Camera.main.transform.forward; // Make sprite look at camera

        if (currentState == CreatureState.Fed) {
            multiplyTimer += Time.deltaTime;
            pointGenerationTimer += Time.deltaTime;
            if (multiplyTimer >= multiplyTime) {
                Multiply();
                multiplyTimer = 0f;
            }
            if (pointGenerationTimer >= pointGenerationTime) {
                CreatureEvents.GeneratePoints(pointsGenerated);
                pointGenerationTimer = 0f;
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
            Vector3 randomPoint = GetRandomPointOnNavmesh();

            if (transform.position.x - randomPoint.x < 0) {
                spriteRenderer.flipX = true;
            }
            else {
                spriteRenderer.flipX = false;
            }

            agent.destination = randomPoint;

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
        else {
            readyToSearch = true;
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
    