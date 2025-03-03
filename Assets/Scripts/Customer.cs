using UnityEngine;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public float stopDistance = 1.5f; // Distance to keep from the previous customer
    private float bobFrequency = 2f;
    private float bobAmplitude = 0.5f;
    public CustomerSpawner spawner;

    public GameObject orderReceipt;

    private float startY;
    public bool isOrdering = false;
    private bool hasStopped = false;

    private string[] cupBases = { "Vanilla", "Chocolate", "Strawberry" };
    private string[] frostings = { "Pink" }; //TODO: Add more frosting colors?
    private string[] toppings = { "CherryTop", "Sprinkles" };

    public string custCupBase;
    public string custFrosting;
    public List<string> custToppings = new List<string>();
    public List<string> allItems = new List<string>();

    public float orderingPositionX = -7.43f; // The position where customers stop to order

    void Start()
    {
        isOrdering = false;
        startY = transform.position.y;
        ChooseRandomOrder();
        orderReceipt = GameObject.Find("OrderReceipt");
    }
    
    void ChooseRandomOrder()
    {
        custCupBase = cupBases[Random.Range(0, cupBases.Length)];
        custFrosting = frostings[Random.Range(0, frostings.Length)];
        custToppings.Clear(); 

        int numberOfToppings = Random.Range(0, toppings.Length + 1);
        Debug.Log("Number of Toppings: " + numberOfToppings);
        List<string> shuffledToppings = new List<string>(toppings);
        for (int i = 0; i < numberOfToppings; i++)
        {
            int index = Random.Range(0, shuffledToppings.Count);
            custToppings.Add(shuffledToppings[index]);
            shuffledToppings.RemoveAt(index); 
        }
    }

    //ChatGPT code to just make SpriteRenderers comply
    void DisableSpriteRenderers(GameObject parent)
    {
        SpriteRenderer parentRenderer = parent.GetComponent<SpriteRenderer>();
        if (parentRenderer != null)
        {
            parentRenderer.enabled = false;
        }
        foreach (SpriteRenderer childRenderer in parent.GetComponentsInChildren<SpriteRenderer>())
        {
            childRenderer.enabled = false;
        }
    }

    void EnableSpriteRenderers(GameObject parent, List<string> allowedNames)
    {
        // Check and enable parent if its name is in the list
        SpriteRenderer parentRenderer = parent.GetComponent<SpriteRenderer>();
        if (parentRenderer != null)
        {
            parentRenderer.enabled = allowedNames.Contains(parent.name);
        }

        // Loop through all child SpriteRenderers
        foreach (SpriteRenderer childRenderer in parent.GetComponentsInChildren<SpriteRenderer>())
        {
            // Enable only if the child name is in the allowed list, otherwise disable it
            childRenderer.enabled = allowedNames.Contains(childRenderer.gameObject.name);

        }
    }


    void Update()
    {
        if (!isOrdering)
        {
            if (ShouldMove())
            {
                MoveLeft();
            }
            else
            {
                hasStopped = true; // Customer is now waiting
            }

            // Bobbing effect
            float bobbing = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.position = new Vector3(transform.position.x, startY + bobbing, transform.position.z);
        }
    }

    bool ShouldMove()
    {
        // If customer reaches the ordering position, they should stop and order
        if (transform.position.x <= orderingPositionX)
        {
            if (spawner.customers.Count > 0 && spawner.customers[0] == gameObject)
            {
                StartOrdering();
            }
            return false; // Stop moving
        }

        // If this is the first customer, keep moving until ordering position
        if (spawner.customers.Count == 0 || spawner.customers[0] == gameObject)
        {
            return true;
        }

        // If there is a customer in front, stop if too close
        int index = spawner.customers.IndexOf(gameObject);
        if (index > 0)
        {
            GameObject customerInFront = spawner.customers[index - 1];

            if (Vector3.Distance(transform.position, customerInFront.transform.position) < stopDistance)
            {
                return false;
            }
        }

        return true;
    }

    void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void StartOrdering()
    {
        //TODO: FIX LATER
        allItems.Clear();
        allItems.Add("OrderReceipt");
        allItems.Add("cupcake liner_0");
        allItems.Add("cupcake batter_0");
        allItems.Add(custCupBase);
        allItems.Add(custFrosting);
        allItems.AddRange(custToppings);
        EnableSpriteRenderers(orderReceipt, allItems);
        isOrdering = true;
        Debug.Log(gameObject.name + " is ordering!");
        Invoke(nameof(FinishOrder), 15f); // Simulate taking order
    }

    void FinishOrder()
    {
        isOrdering = false;
        DisableSpriteRenderers(orderReceipt);
        spawner.RemoveCustomer(gameObject);
    }
}
