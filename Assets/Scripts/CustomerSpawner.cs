using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public float minspawnRate = 3f;
    public float maxspawnRate = 7f;
    public List<GameObject> customers = new List<GameObject>();
    public float customerSpacing = 1.5f;
    public float maxCustomers;

    public float orderingPositionX = -7.43f; // Position where customers stop to order

    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers() // <-- Remove <T> here
    {
        while (true)
        {
            float randomDelay = Random.Range(minspawnRate, maxspawnRate);
            yield return new WaitForSeconds(randomDelay);

            if (!ShouldPauseSpawning()) 
            {
                SpawnCustomer();
            }
        }
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
        if (customers.Count > maxCustomers)
        {
            return true;
        }
        return false;
    }
}
