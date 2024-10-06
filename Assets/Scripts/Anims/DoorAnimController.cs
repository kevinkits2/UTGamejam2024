using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimController : MonoBehaviour {
    [SerializeField] private UIManager UIManager;

    void StartGame() {
        UIManager.StartGame();
        GameManagerEvents.StartGame();
    }
}
