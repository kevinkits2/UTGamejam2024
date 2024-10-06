using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodButtonContainer : MonoBehaviour {

    [SerializeField] Button[] buttons;


    private void Awake() {
        GameManagerEvents.OnFoodReady += HandleFoodReady;
    }

    private void OnDestroy() {
        GameManagerEvents.OnFoodReady -= HandleFoodReady;
    }

    private void HandleFoodReady(DrawerFoodButton button) {
        FoodButton availableSlot = CheckAvailability();

        if (availableSlot != null) {
            button.HideFood();
            availableSlot.AddFood();
        }
    }

    private FoodButton CheckAvailability() {
        Debug.Log(buttons);
        foreach (Button button in buttons) {
            Debug.Log(button);
            if (!button.TryGetComponent<FoodButton>(out FoodButton foodButton)) continue;
            if (!foodButton.hasFood) return foodButton;
        }

        return null;
    }
}
