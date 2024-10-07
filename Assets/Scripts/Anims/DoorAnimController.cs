using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimController : MonoBehaviour {
    [SerializeField] private UIManager UIManager;
    [SerializeField] private AudioSource menuMusic;

    void StartGame() {
        UIManager.StartGame();
        menuMusic.Stop();
        GameManagerEvents.StartGame();
        AudioPlayer.Instance.PlayGameMusic();
    }
}
