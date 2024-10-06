using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerFoodButton : MonoBehaviour {

    [SerializeField] Sprite[] sprites;
    private int currentSpriteIndex = 0;
    private Button button;
    private Image image;
    private bool hasFood;

    [SerializeField] private float foodRespawnTime = 5f;
    private float foodTimer;


    private void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        button.onClick.AddListener(() => {
            currentSpriteIndex++;

            if (currentSpriteIndex > sprites.Length - 1) {
                GameManagerEvents.FoodReady(this);
                return;
            }

            image.sprite = sprites[currentSpriteIndex];
        });
    }

    private void Update() {
        if (hasFood) return;

        foodTimer += Time.deltaTime;
        if (foodTimer > foodRespawnTime) {
            ShowFood();
            foodTimer = 0f;
        }
    }

    public void HideFood() {
        Color color = image.color;
        color.a = 0;
        image.color = color;
        button.interactable = false;
        hasFood = false;
    }

    public void ShowFood() {
        Color color = image.color;
        color.a = 255;
        image.color = color;
        button.interactable = true;
        hasFood = true;
        currentSpriteIndex = 0;
        image.sprite = sprites[0];
    }
}
