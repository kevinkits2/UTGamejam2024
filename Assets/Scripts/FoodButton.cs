using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodButton : MonoBehaviour {

    [SerializeField] Sprite foodSprite;
    public bool hasFood;
    private Button button;
    private Image image;


    private void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        if (!hasFood) {
            button.interactable = false;
        }

        Color color = image.color;
        color.a = 0;
        image.color = color;
    }

    public void PointerDown() {
        GameManagerEvents.FoodButtonPress(this);
    }

    public void AddFood() {
        hasFood = true;
        image.sprite = foodSprite;
        Color color = image.color;
        color.a = 255;
        image.color = color;

        button.interactable = true;
    }

    public void RemoveFood() {
        hasFood = false;

        image.sprite = foodSprite;
        Color color = image.color;
        color.a = 0;
        image.color = color;

        button.interactable = false;
    }
}
