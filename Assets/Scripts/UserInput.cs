using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UserInput : MonoBehaviour
{
    public GameObject textWarning;
    public CustomerSpawner spawner; 
    private bool canClick = true;

    public GameObject[] cupBases;
    public GameObject[] toppings;
    public GameObject[] frostings;
    public GameObject check;
    public GameObject reset;


    public Transform spawnParent;
    public string chosenCupBase = "";
    public string chosenFrosting = "";
    public List<string> chosenToppings = new List<string>();
    public GameObject x;

    void Start()
    {
        foreach (Transform child in spawnParent)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            string clickedObject = DetectClick();
            if (clickedObject == "Check")
            {
                CheckOrder();
            }
            if (clickedObject == "Reset")
            {
                ResetOrder();
            }
            //Cup Bases
            if (clickedObject == "Vanilla" || clickedObject == "Chocolate" || clickedObject == "Strawberry")
            {
                if (chosenCupBase == "")
                {
                    chosenCupBase = clickedObject;
                    SpawnCupcakeItem(clickedObject, cupBases);
                }
            }
            Debug.Log("Clicked Object is: " + clickedObject);
            // Toppings
            if (clickedObject == "CherryTop" || clickedObject == "Sprinkles" || clickedObject == "CookiesTop" || clickedObject == "MarshmallowsTop" || clickedObject == "PopcornTop")
            {
                if (!chosenToppings.Contains(clickedObject))
                {
                    chosenToppings.Add(clickedObject);
                    SpawnCupcakeItem(clickedObject, toppings);
                }
            }
            if (clickedObject == "VanillaFrost" || clickedObject == "ChocFrost")
            {
                if (chosenFrosting == "")
                {
                    chosenFrosting = clickedObject;
                    SpawnCupcakeItem(clickedObject, frostings);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("You can't click yet!");
        }
    }

    void SpawnCupcakeItem(string clicked, GameObject[] category)
    {
        foreach (GameObject obj in category)
        {
            if (obj.name == clicked) // Match prefab name to clicked object name
            {
                obj.SetActive(true);
                Debug.Log("Spawned: " + obj.name);
                return;
            }
        }
            Debug.LogWarning("No matching prefab found for: " + clicked);
    }

    void CheckOrder()
    {
        // Check order of first customer
        Customer customer = spawner.customers[0].GetComponent<Customer>();
        bool areEqual = customer.custToppings.OrderBy(x => x).SequenceEqual(chosenToppings.OrderBy(x => x));
        if (customer.custCupBase == chosenCupBase && customer.custFrosting == chosenFrosting && areEqual)
        {
            customer.FinishOrder(chosenCupBase, chosenFrosting, chosenToppings);
        }
        else
        {
            if (customer.isOrdering)
            {
                x.SetActive(true);
                Invoke("TurnOff", 2f);
                Debug.Log("You messed up!");
            }
        }
        ResetOrder();
    }

    void TurnOff()
    {
        x.SetActive(false);
    }

    public void ResetOrder()
    {
        chosenCupBase = "";
        chosenFrosting = "";
        chosenToppings.Clear();
        foreach (Transform child in spawnParent)
        {
            child.gameObject.SetActive(false);
        }
        Debug.Log("Order has been reset. All objects removed.");
    }

    //ChatGPT code to detect click on which gameobject on screen
    string DetectClick()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Clicked on: " + hit.collider.gameObject.name);
            return hit.collider.gameObject.name;
        }
        return "";
    }
}
