using UnityEngine;

public class UserInput : MonoBehaviour
{
    public GameObject textWarning;
    public CustomerSpawner spawner; 
    private bool canClick = false;

    private

    void Start()
    {
    }

    void Update()
    {
        CheckIfCustomerIsOrdering();
        if (canClick && Input.GetMouseButtonDown(0))
        {
            Debug.Log("User clicked!");
        }
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
