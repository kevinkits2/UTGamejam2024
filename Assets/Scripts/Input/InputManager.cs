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
        playerControls.Player.MouseDown.performed += HandleMouseDownPerformed;
        playerControls.Player.MouseDown.canceled += HandleMouseDownCanceled;
    }

    private void HandleMouseDownCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        GameManagerEvents.MouseUp();
    }

    private void HandleMouseDownPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        GameManagerEvents.MouseDown();
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
