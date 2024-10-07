using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Cockroach : MonoBehaviour {

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private bool readyToWander;
    private float wanderTimer = 3f;

    [SerializeField] private float wanderTime = 3f;
    [SerializeField] private float maxWanderDistance = 3f;


    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.forward = Camera.main.transform.forward;

        if (!readyToWander) {
            wanderTimer += Time.deltaTime;
            if (wanderTimer > wanderTime) {
                readyToWander = true;
                wanderTimer = 0f;
            }
        }

        Wander();
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

            agent.destination = new Vector3 (randomPoint.x, transform.position.y, randomPoint.z);

            readyToWander = false;
        }
    }

    private Vector3 GetRandomPointOnNavmesh() {
        Vector3 randomPos = Random.insideUnitSphere * maxWanderDistance + transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, maxWanderDistance, NavMesh.AllAreas);

        return hit.position;
    }
}
