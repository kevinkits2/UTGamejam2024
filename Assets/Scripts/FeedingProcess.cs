using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedingProcess : MonoBehaviour {

    Ray ray;
    RaycastHit hit;
    private bool draggingFood;


    private void Start() {
        GameManagerEvents.OnFoodDraggedStopped += HandleFoodDragStopped;
        GameManagerEvents.OnFoodDragged += HandleFoodDragged;
    }

    private void HandleFoodDragged() {
        draggingFood = true;
    }

    private void HandleFoodDragStopped() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (!hit.transform.TryGetComponent<Creature>(out Creature creature)) return;
            creature.Feed();
            creature.Move();
            GameManagerEvents.CreatureFeed();
        }

        draggingFood = false;
    }

    private void FixedUpdate() {
        if (!draggingFood) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (!hit.transform.TryGetComponent<Creature>(out Creature creature)) {
                GameManagerEvents.MouseNotOverCreature();
                return;
            };

            creature.StandStill();
        }
    }
}
