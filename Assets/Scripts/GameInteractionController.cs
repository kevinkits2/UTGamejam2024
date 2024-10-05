using System;
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
    [SerializeField] GameObject cursor;
    
    private List<FoodItem> foodItems = new List<FoodItem>();

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

        // is dragging only if dragging started above a food item
        if (state == State.dragging)
        {
            selectedFoodItem.transform.position = mouseWorldPosition();
        }

        if (Input.GetMouseButton(0))
        {
            cursor.transform.position = mouseWorldPosition();
            print(cursor.transform.position);
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    isOverACreature();
        //    isOverAFoodSlot();
        //    state = State.notdragging;
        //}
    }

    private Vector3 mouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var result = Physics.Raycast(ray, out var hit);
        return hit.point;
    }

    private Rigidbody objectAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var result = Physics.Raycast(ray, out var hit);
        return hit.rigidbody;
    }

    private void isOverAFoodSlot()
    {
        throw new NotImplementedException();
    }

    private FoodItem getFoodItemUnderMouse()
    {
        var mouseWorldPos = mouseWorldPosition();
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

    void createNewFoodItem()
    {
        foodItems.Add(new FoodItem());
    }
    
    bool isOverACreature()
    {
        throw new NotImplementedException();
    }
}   