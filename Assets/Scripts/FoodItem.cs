using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour {

    private Button button;


    private void Awake() {
        button = GetComponent<Button>();
    }
}
