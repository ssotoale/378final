using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public SpriteRenderer spriteRend;
    public List<Sprite> custOptions;

    public int currentLevel = 0;
    public int cupBasesChosen = 0;
    public int frostingsChosen = 0;
    public int toppingsChosen = 0;

    private string[] cupBases = { "Vanilla", "Chocolate", "Strawberry", "Blueberry" };
    private string[] frostings = { "VanillaFrost", "ChocFrost", "StrawFrost", "BlueFrost" };
    private string[] toppings = { "CherryTop", "Sprinkles", "MarshmallowsTop", "PopcornTop", "CookiesTop", "ChocDrizzTop", "LollipopTop" };

    public string custCupBase;
    public string custFrosting;
    public List<string> custToppings = new List<string>();
    public List<string> allItems = new List<string>();

    public float orderingPositionX = -7.43f; // Position where customers stop to order
    public float exitThresholdX = -12f; // Off-screen position to destroy the customer

    private bool isLeaving = false;

    private PointSystem pointSystem;

    private float waitTime = 0f; // Timer to track how long the customer has been waiting
    public float maxWaitTime = 30f; // Maximum wait time before deducting points

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("LevelPlaying", 1);
        if (currentLevel == 1)
        {
            cupBasesChosen = 2;
            frostingsChosen = 2;
            toppingsChosen = 2;
            maxWaitTime = 30f;
        }
        else if (currentLevel == 2)
        {
            cupBasesChosen = 3;
            frostingsChosen = 3;
            toppingsChosen = 5;
            maxWaitTime = 25f;
        }
        else if (currentLevel == 3 || currentLevel == 4)
        {
            cupBasesChosen = 4;
            frostingsChosen = 4;
            toppingsChosen = 7;
            maxWaitTime = 20f;
        }
        spriteRend.sprite = custOptions[Random.Range(0, custOptions.Count)];
        isOrdering = false;
        startY = transform.position.y;
        ChooseRandomOrder();
        orderReceipt = GameObject.Find("OrderReceipt");
        pointSystem = FindObjectOfType<PointSystem>();
    }

    void ChooseRandomOrder()
    {
        custCupBase = cupBases[Random.Range(0, cupBasesChosen)];
        custFrosting = frostings[Random.Range(0, frostingsChosen)];
        custToppings.Clear();

        int numberOfToppings = Random.Range(0, toppingsChosen + 1);
        Debug.Log("Number of Toppings: " + numberOfToppings);
        List<string> shuffledToppings = new List<string>(toppings.Take(numberOfToppings));
        for (int i = 0; i < numberOfToppings; i++)
        {
            int index = Random.Range(0, shuffledToppings.Count);
            custToppings.Add(shuffledToppings[index]);
            shuffledToppings.RemoveAt(index);
        }
    }

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
        SpriteRenderer parentRenderer = parent.GetComponent<SpriteRenderer>();
        if (parentRenderer != null)
        {
            parentRenderer.enabled = allowedNames.Contains(parent.name);
        }

        foreach (SpriteRenderer childRenderer in parent.GetComponentsInChildren<SpriteRenderer>())
        {
            childRenderer.enabled = allowedNames.Contains(childRenderer.gameObject.name);
        }
    }

    void Update()
    {
        if (isLeaving)
        {
            MoveLeft(); // Continue moving left to exit
        }
        else if (!isOrdering)
        {
            if (ShouldMove())
            {
                MoveLeft();
            }
            else
            {
                hasStopped = true; // Customer is now waiting
                waitTime += Time.deltaTime; // Increment the wait time

                if (waitTime >= maxWaitTime)
                {
                    DeductPoints(5); // Deduct points for waiting too long
                    waitTime = 0f; // Reset the wait time
                }
            }
        }

        // Bobbing effect (active even when leaving)
        float bobbing = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, startY + bobbing, transform.position.z);
    }

    bool ShouldMove()
    {
        if (transform.position.x <= orderingPositionX)
        {
            if (spawner.customers.Count > 0 && spawner.customers[0] == gameObject)
            {
                StartOrdering();
            }
            return false; // Stop moving
        }

        if (spawner.customers.Count == 0 || spawner.customers[0] == gameObject)
        {
            return true;
        }

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
        allItems.Clear();
        allItems.Add("OrderReceipt");
        allItems.Add(custCupBase);
        allItems.Add(custFrosting);
        allItems.AddRange(custToppings);
        EnableSpriteRenderers(orderReceipt, allItems);
        isOrdering = true;
        Debug.Log(gameObject.name + " is ordering!");
    }

    public void FinishOrder(string cupBase, string frosting, List<string> toppings)
    {
        isOrdering = false;
        DisableSpriteRenderers(orderReceipt);

        int points = CalculatePoints(cupBase, frosting, toppings);
        pointSystem.AddPoints(points);

        StartCoroutine(ExitAndDestroy());
    }

    public void DeductPoints(int amount)
    {
        pointSystem.AddPoints(-amount);
    }

    private int CalculatePoints(string cupBase, string frosting, List<string> toppings)
    {
        int points = 0;

        if (cupBase == custCupBase) 
            points += 10;
        else 
            points -= 5;

        if (frosting == custFrosting) 
            points += 10;
        else 
            points -= 5;

        foreach (string topping in toppings)
        {
            if (custToppings.Contains(topping))
            {
                points += 5;
            }
            else
            {
                points -= 2;
            }
        }

        return points;
    }

    private IEnumerator ExitAndDestroy()
    {
        isLeaving = true;

        while (transform.position.x > exitThresholdX)
        {
            MoveLeft();
            yield return null; // Wait for next frame
        }

        spawner.RemoveCustomer(gameObject);
        Destroy(gameObject);
    }
}
