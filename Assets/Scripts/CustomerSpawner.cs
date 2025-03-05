using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public float spawnRate = 3f;
    public List<GameObject> customers = new List<GameObject>();
    public float customerSpacing = 1.5f;

    public float orderingPositionX = -7.43f; // Position where customers stop to order

    void Start()
    {
        InvokeRepeating(nameof(SpawnCustomer), 0f, spawnRate);
    }

    void SpawnCustomer()
    {
        // Check if a customer is at the ordering position (don't spawn if true)
        if (ShouldPauseSpawning() || customerPrefab == null)
            return;

        GameObject newCustomer = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        customers.Add(newCustomer);

        Customer customerScript = newCustomer.GetComponent<Customer>();
        if (customerScript != null)
        {
            customerScript.spawner = this;
        }
    }

    public void RemoveCustomer(GameObject customer)
    {
        if (customers.Contains(customer))
        {
            customers.Remove(customer);
            Destroy(customer);
        }
    }

    bool ShouldPauseSpawning()
    {
        if (customers.Count > 7)
        {
            return true;
        }
        return false;
    }
}
