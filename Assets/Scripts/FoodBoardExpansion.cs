using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBoardExpansion : MonoBehaviour {

    [SerializeField] private GameObject foodBoard;
    [SerializeField] private GameObject otherBoard;
    private Button button;
    [SerializeField] private Button otherButton;
     

    private void Awake() {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => {
            foodBoard.SetActive(false);
            otherBoard.SetActive(true);
            otherButton.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
