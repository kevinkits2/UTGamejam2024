using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

    public Action OnPause;

    private PlayerControls playerControls;


    void Awake() {
        playerControls = new PlayerControls();
        playerControls.Enable();

        playerControls.Player.Pause.performed += HandlePausePerformed;
    }

    private void HandlePausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().name == "Menu") return;
        OnPause?.Invoke();
    }

    public Vector3 GetMouseWorldPosition() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0f;

        return mousePosition;
    }

    public void DisablePlayerControls()
    {
        playerControls.Player.Disable();
    }

    public void EnablePlayerControls()
    {
        playerControls.Player.Enable();
    }
}
