using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachSpawner : MonoBehaviour {

    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private GameObject cockroachPrefab;
    [SerializeField] private float spawnTime = 40f;
    private float spawnTimer;


    private void Update() {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnTime) {
            Instantiate(cockroachPrefab, GetRandomSpawnPos().position, cockroachPrefab.transform.rotation);
            spawnTimer = 0;
        }
    }

    private Transform GetRandomSpawnPos() {
        int i = spawnPos.Length;

        return spawnPos[Random.Range(0, i - 1)];
    }
}
