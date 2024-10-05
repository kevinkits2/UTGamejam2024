using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    [SerializeField] private float timeForDeath = 2f;

    private void Awake() {
        Destroy(gameObject, timeForDeath);
    }

}
