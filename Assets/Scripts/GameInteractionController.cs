using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum State
{
    dragging,
    notdragging
}
public class GameInteractionController : MonoBehaviour
{
    [SerializeField] GameObject foodItemTemplate;
    
    private List<FoodItem> foodItems;

    private State state = State.notdragging;
    private FoodItem selectedFoodItem;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedFoodItem = getFoodItemUnderMouse();
            if (selectedFoodItem is not null)
            {
                state = State.dragging;
            }
        }

        // is dragging only, if dragging started above a food item
        if (state == State.dragging)
        {
                selectedFoodItem.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isOverACreature();
            isOverA
            state = State.notdragging;
        }
    }

    private FoodItem getFoodItemUnderMouse()
    {
        var mousePos = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        FoodItem foodItem = null;
        foreach (var item in foodItems)
        {
            if (Vector3.Distance(item.transform.position, mouseWorldPos) <= item.radius)
            {
                foodItem = item;
            }
        }

        return foodItem;
    }

    createNewFoodItem()
    {
        foodItems.Add(new FoodItem());
    }
    
    isOverACreature()
}   