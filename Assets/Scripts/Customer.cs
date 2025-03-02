using UnityEngine;

public class Customer : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public float stopDistance = 1.5f; // Distance to keep from the previous customer
    private float bobFrequency = 2f;
    private float bobAmplitude = 0.5f;
    public CustomerSpawner spawner;

    private float startY;
    private bool isOrdering = false;
    private bool hasStopped = false;

    public float orderingPositionX = -7.43f; // The position where customers stop to order

    void Start()
    {
        startY = transform.position.y;
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
        isOrdering = true;
        Debug.Log(gameObject.name + " is ordering!");
        Invoke(nameof(FinishOrder), 15f); // Simulate taking order
    }

    void FinishOrder()
    {
        spawner.RemoveCustomer(gameObject);
    }
}
