using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cockroach : MonoBehaviour {

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float wanderTime = 3f;
    [SerializeField] private float maxWanderDistance = 3f;


    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        Vector3 randomPoint = GetRandomPointOnNavmesh();
        transform.forward = Camera.main.transform.forward;

        if (transform.position.x - randomPoint.x < 0) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }

        agent.destination = randomPoint;
    }

    private Vector3 GetRandomPointOnNavmesh() {
        Vector3 randomPos = Random.insideUnitSphere * maxWanderDistance + transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, maxWanderDistance, NavMesh.AllAreas);

        return hit.position;
    }
}
