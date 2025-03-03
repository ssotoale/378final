using UnityEngine;
using System.Collections.Generic;

public class UserInput : MonoBehaviour
{
    public GameObject textWarning;
    public CustomerSpawner spawner; 
    private bool canClick = false;

    public GameObject[] cupBases;
    public GameObject[] toppings;
    public GameObject[] frostings;
    public GameObject check;
    public GameObject reset;


    public Transform spawnParent;
    public string chosenCupBase = "";
    public string chosenFrosting = "";
    public List<string> chosenToppings = new List<string>();

    void Start()
    {
    }

    void Update()
    {
        CheckIfCustomerIsOrdering();
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
            if (clickedObject == "CherryTop" || clickedObject == "Sprinkles")
            {
                if (!chosenToppings.Contains(clickedObject))
                {
                    chosenToppings.Add(clickedObject);
                    SpawnCupcakeItem(clickedObject, toppings);
                }
            }
            if (clickedObject == "Pink")
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
        foreach (GameObject prefab in category)
        {
            if (prefab.name == clicked) // Match prefab name to clicked object name
            {
                GameObject newObject = Instantiate(prefab, spawnParent.position, Quaternion.identity);
                newObject.transform.SetParent(spawnParent); // Set as a child
                newObject.transform.localPosition = Vector3.zero; // Reset position relative to parent

                Debug.Log("Spawned: " + newObject.name);
                return;
            }
        }
            Debug.LogWarning("No matching prefab found for: " + clicked);
    }

    void CheckOrder()
    {

    }

    void ResetOrder()
    {
        chosenCupBase = "";
        chosenFrosting = "";
        chosenToppings.Clear();
        foreach (Transform child in spawnParent)
        {
            Destroy(child.gameObject); 
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

    void CheckIfCustomerIsOrdering()
    {
        if (spawner != null && spawner.customers.Count > 0)
        {
            Customer firstCustomer = spawner.customers[0].GetComponent<Customer>();
            if (firstCustomer != null && firstCustomer.isOrdering == true) 
            {
                canClick = true;
                if (textWarning != null)
                    textWarning.SetActive(false); 
                return;
            }
        }
        canClick = false;
        if (textWarning != null)
            textWarning.SetActive(true);
    }
}
