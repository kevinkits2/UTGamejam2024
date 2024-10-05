using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    [SerializeField] private GameObject deathEffect;


    private void Awake() {
        CreatureEvents.OnCreatureDeath += OnCreatureDeath;
    }

    private void OnCreatureDeath(Vector3 pos) {
        Instantiate(deathEffect, pos, deathEffect.transform.rotation);
    }
}
