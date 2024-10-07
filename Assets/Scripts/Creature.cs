using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour {

    [SerializeField] private GameObject creaturePrefab;

    private CreatureState currentState;
    [SerializeField] private LayerMask creatureLayerMask;
    [SerializeField] private LayerMask cockroachLayerMask;
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
    private Transform cockroachTarget;
    private Coroutine rageAttackCooldownRoutine;
    [SerializeField] float killDistance = 0.2f;
    [SerializeField] float cockroachKillDistance = 0.4f;
    [SerializeField] int hungerDepleteAmount = 4;

    [SerializeField] private int foodValue = 20;

    private NavMeshAgent agent;
    [SerializeField] private float wanderTime = 3f;
    [SerializeField] private float maxWanderDistance = 3f;
    [SerializeField] private float fedSpeed = 1;
    [SerializeField] private float hungerSpeed = 2;
    [SerializeField] private float rageSpeed = 4;
    private float wanderTimer = 0f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] private int pointsGenerated;
    [SerializeField] private float pointGenerationTime = 3f;
    private float pointGenerationTimer;


    private bool readyToWander;
    private float hungerDepleteTime = 1f;
    private Coroutine hungerCoroutine;

    private bool standingStill = false;

    [SerializeField] private bool explode;


    private void Awake() {
        currentState = CreatureState.Fed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        readyToWander = true;
    }

    private void Start() {
        hungerCoroutine = StartCoroutine(HungerRoutine());
        GameManagerEvents.OnMouseNotOverCreature += HandleMouseNotOverCreature;
    }

    private void HandleMouseNotOverCreature() {
        standingStill = false;
    }

    private void OnDestroy() {
        if (hungerCoroutine != null) {
            StopCoroutine(hungerCoroutine);
        }
        
        if (rageAttackCooldownRoutine != null) {
            StopCoroutine(rageAttackCooldownRoutine);
        }
        AudioPlayer.Instance.PlayCreatureEaten();
        CreatureEvents.CreatureDeath(transform.position, currentState);
    }

    public void ChangeState(CreatureState state, Transform transform) {
        currentState = state;

        switch (currentState) {
            case CreatureState.Fed:
                agent.speed = fedSpeed;
                multiplyTimer = 0f;
                pointGenerationTimer = 0f;
                readyToSearch = true;
                break;

            case CreatureState.Hungry:
                agent.speed = hungerSpeed;
                readyToSearch = true;
                break;

            case CreatureState.Rage:
                AudioPlayer.Instance.PlayCrazyTransform();
                agent.speed = rageSpeed;
                readyToSearch = true;
                break;
        }
    }

    private void Update() {
        if (explode) {
            Destroy(gameObject);
        }

        transform.forward = Camera.main.transform.forward; // Make sprite look at camera
        animator.SetFloat("Hunger", hunger);

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

        if (!readyToSearch && !rageAttackOnCooldown) {
            rageEnemySearchTimer += Time.deltaTime;
            if (rageEnemySearchTimer > rageEnemySearchTime) {
                readyToSearch = true;
                rageEnemySearchTimer = 0f;
            }
        }

        HungerCheck();
        StateBehaviour();
    }

    private void StateBehaviour() {
        switch (currentState) {
            case CreatureState.Fed:
                Wander();
                CockroachScan();
                CockroachTargetCheck();
                break;

            case CreatureState.Hungry:
                Wander();
                CockroachScan();
                CockroachTargetCheck();
                break;

            case CreatureState.Rage:
                Rage();
                TargetScan();
                CockroachScan();
                CockroachTargetCheck();

                if (rageTarget == null && !rageAttackOnCooldown && !readyToSearch) {
                    Wander();
                }

                break;
        }
    }

    private void HungerCheck() {
        if (currentState == CreatureState.Fed && hunger <= hungerStart) {
            CreatureEvents.ChangeCreatureState(CreatureState.Hungry, transform);
            ChangeState(CreatureState.Hungry, transform);
        }
        else if (currentState == CreatureState.Hungry) {
            if (hunger > hungerStart) {
                CreatureEvents.ChangeCreatureState(CreatureState.Fed, transform);
                ChangeState(CreatureState.Fed, transform);
            }
            else if (hunger <= rageStart) {
                CreatureEvents.ChangeCreatureState(CreatureState.Rage, transform);
                ChangeState(CreatureState.Rage, transform);
            }
        }
    }

    private void Multiply() {
        GameObject newCreature = Instantiate(creaturePrefab, transform.position, Quaternion.identity);
        newCreature.GetComponent<Creature>().SetHunger(100);
        GameManagerEvents.Multiply();
    }

    private void Wander() {
        if (standingStill) return;
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
            animator.SetTrigger("Eat");
            Destroy(rageTarget.gameObject);
            hunger += 50;

            if (hunger > 100) {
                Destroy(gameObject);
            }

            rageTarget = null;
        }
    }

    private void CockroachTargetCheck() {
        if (cockroachTarget == null) return;

        agent.destination = cockroachTarget.position;

        if (Vector3.Distance(transform.position, cockroachTarget.transform.position) < cockroachKillDistance) {
            animator.SetTrigger("Eat");
            Destroy(cockroachTarget.gameObject);
            hunger += 50;

            if (hunger > 100) {
                Destroy(gameObject);
            }

            cockroachTarget = null;
        }
    }

    public void Feed() {
        if (currentState == CreatureState.Rage) return;

        hunger += foodValue;

        AudioPlayer.Instance.PlayCreatureFed();
        if (hunger > 100) {
            Destroy(gameObject);
        }
    }

    public void StandStill() {
        if (currentState == CreatureState.Rage) return;
        agent.destination = transform.position;
        standingStill = true;
    }

    public void Move() {
        if (currentState == CreatureState.Rage) return;
        standingStill = false;
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

    private void CockroachScan() {
        if (!readyToSearch) return;
        readyToSearch = false;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, rageEnemySearchDistance / 2, transform.forward, 1f, cockroachLayerMask);
        float closestTargetDistance = Mathf.Infinity;
        Cockroach closestTarget = null;

        foreach (RaycastHit hit in hits) {
            if (!hit.transform.TryGetComponent<Cockroach>(out Cockroach cockroach)) return;
            if (Vector3.Distance(cockroach.transform.position, transform.position) < closestTargetDistance) {
                closestTarget = cockroach;
            }
        }

        if (currentState == CreatureState.Rage && rageTarget != null) {
            return;
        }

        if (closestTarget != null) {
            cockroachTarget = closestTarget.transform;
        }
        else {
            readyToSearch = true;
        }
    }

    public void SetHunger(int amount) {
        hunger = amount;
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

            hunger -= hungerDepleteAmount;
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
    