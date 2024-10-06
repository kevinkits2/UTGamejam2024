using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFollowMouse : MonoBehaviour {

    [SerializeField] private RectTransform rectTransform;


    private void Start() {
        GameManagerEvents.OnFoodDragged += HandleFoodDragged;
        GameManagerEvents.OnFoodDraggedStopped += HandleFoodDragStopped;
    }

    private void HandleFoodDragStopped() {
        rectTransform.gameObject.SetActive(false);
    }

    private void HandleFoodDragged() {
        rectTransform.gameObject.SetActive(true);
    }

    private void Update() {
        rectTransform.position = Input.mousePosition;
    }
}
