using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerUpFoodImage : MonoBehaviour {

    [SerializeField] FoodButton correspondingButton;
    [SerializeField] GameObject drawerUp;
    private Image image;


    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Update() {
        if (drawerUp.activeSelf && correspondingButton.hasFood) {
            image.enabled = true;
        } 
        else {
            image.enabled = false;
        }

        if (!drawerUp.activeSelf) {
            image.enabled = false;
        }
    }
}
