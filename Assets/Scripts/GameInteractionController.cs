using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum MouseState
{
    dragging,
    notdragging
}

public class GameInteractionController : MonoBehaviour
{
    [SerializeField] GameObject foodItemPrefab;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject foodPanel;
    [SerializeField] List<GameObject> eightpads;
    [SerializeField] List<GameObject> sixpads;
    
    private List<FoodItem> foodItems = new List<FoodItem>();
    private bool foodPanelUp = false;
    private MouseState state = MouseState.notdragging;
    private GameObject selectedFoodItem;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var pad in eightpads)
        {
            Instantiate(foodItemPrefab, pad.transform.position, pad.transform.rotation).transform.parent = foodPanel.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var raycast = mouseRaycast();
            var hitObject = raycast.collider.gameObject;
            clickMouse(hitObject);
            if (hitObject.GetComponent<FoodItem>())
            {
                selectedFoodItem = hitObject;
                print("dragging a food item");
                state = MouseState.dragging;

                //TODO: exclude dragged item from ray collisions
            }
        }

        // is dragging only if dragging started above a food item
        if (state == MouseState.dragging)
        {
            selectedFoodItem.transform.position = mouseRaycast().point;
        }

    }


    RaycastHit mouseRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var result = Physics.Raycast(ray, out var hit);
        return hit;
    }



    void createNewFoodItem()
    {
        foodItems.Add(new FoodItem());
    }

    public void foodPanelButtonClicked()
    {
        foodPanelUp = !foodPanelUp;
        print(foodPanelUp ? "foodpanel is up" : "foodpanel is down");
        if(foodPanelUp)
        {
            movePanelUp();
        } else {
            movePanelDown();
        }
    }

    void clickMouse(GameObject clickedObject)
    {
        var gameobject = mouseRaycast().collider.gameObject;
        
        if (gameobject.GetComponent<panelToggle>())
        {
            foodPanelButtonClicked();
        } else if (gameobject.GetComponent<FoodItem>()) {
            foodClicked();
        }

    }

    void foodClicked()
    {
        //throw new NotImplementedException();
    }

    void movePanelDown()
    {
        var pos = foodPanel.transform.position;
        pos.y = 0.474f;
        foodPanel.transform.position = pos;

    }

    void movePanelUp()
    {
        var pos = foodPanel.transform.position;
        pos.y = 1.0954f;
        foodPanel.transform.position = pos;
    }
}   