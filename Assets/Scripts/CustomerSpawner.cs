using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public float minspawnRate = 3f;
    public float maxspawnRate = 7f;
    public List<GameObject> customers = new List<GameObject>();
    public float customerSpacing = 1.5f;
    public float maxCustomers;
    int currentLevel = 1;
    public int maxCustomersInLine = 0;
    public int customerCount = 0;
    public int finishedCustomers = 0;
    
    public GameObject nextLevelScene;
    public GameObject lostLevelScene;
    public TextMeshProUGUI  timerText; 
    public float timeRemaining;
    private bool timerRunning = true;
    public GameObject audioTimer;
    public AudioClip dingSound;

    public GameObject popcornTop;
    public GameObject lollipopTop;
    public GameObject cookieTop;
    public GameObject marshmallowsTop;
    public GameObject blueberryBat;
    public GameObject strawBat;
    public GameObject blueFrost;
    public GameObject strawFrost;
    public GameObject chocDrizzTop;
    public GameObject timerObj;
    public PointSystem pointsText;

    public GameObject pauseMenuUI;
    private bool isPaused = false;

    public float orderingPositionX = -7.43f; // Position where customers stop to order

    void Start()
    {
        Time.timeScale = isPaused ? 0 : 1;
        finishedCustomers = 0;
        currentLevel = PlayerPrefs.GetInt("LevelPlaying", 1);
        Debug.Log("Current Level:" + currentLevel);
        if (currentLevel == 1)
        {
            timeRemaining = 110;
            maxCustomersInLine = 3;
            customerCount = 0;
            maxCustomers = 5;
            minspawnRate = 3f;
            maxspawnRate = 7f;
            popcornTop.SetActive(false);
            lollipopTop.SetActive(false);
            cookieTop.SetActive(false);
            marshmallowsTop.SetActive(false);
            blueberryBat.SetActive(false);
            strawBat.SetActive(false);
            blueFrost.SetActive(false);
            strawFrost.SetActive(false);
            chocDrizzTop.SetActive(false);
        }
        else if (currentLevel == 2)
        {
            timeRemaining = 220;
            maxCustomersInLine = 5;
            customerCount = 0;
            maxCustomers = 10;
            minspawnRate = 3f;
            maxspawnRate = 7f;
            popcornTop.SetActive(true);
            lollipopTop.SetActive(false);
            cookieTop.SetActive(true);
            marshmallowsTop.SetActive(true);
            blueberryBat.SetActive(false);
            strawBat.SetActive(true);
            blueFrost.SetActive(false);
            strawFrost.SetActive(true);
            chocDrizzTop.SetActive(false);
        }
        else if (currentLevel == 3)
        {
            timeRemaining = 400;
            maxCustomersInLine = 7;
            customerCount = 0;
            maxCustomers = 17;
            minspawnRate = 3f;
            maxspawnRate = 7f;
            popcornTop.SetActive(true);
            lollipopTop.SetActive(true);
            cookieTop.SetActive(true);
            marshmallowsTop.SetActive(true);
            blueberryBat.SetActive(true);
            strawBat.SetActive(true);
            blueFrost.SetActive(true);
            strawFrost.SetActive(true);
            chocDrizzTop.SetActive(true);
        }
        else
        {
            timeRemaining = int.MaxValue;
            timerObj.SetActive(false);
            maxCustomersInLine = 7;
            customerCount = 0;
            maxCustomers = int.MaxValue;
            minspawnRate = 3f;
            maxspawnRate = 7f;
            popcornTop.SetActive(true);
            lollipopTop.SetActive(true);
            cookieTop.SetActive(true);
            marshmallowsTop.SetActive(true);
            blueberryBat.SetActive(true);
            strawBat.SetActive(true);
            blueFrost.SetActive(true);
            strawFrost.SetActive(true);
            chocDrizzTop.SetActive(true);
        }
        SpawnCustomer();
        StartCoroutine(SpawnCustomers());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        if (timerRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining); // Round up to show whole seconds
        }
        else if (timerRunning)
        {
            timerText.text = "Time's Up!";
            timerRunning = false;
            OnTimerEnd();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenuUI.SetActive(isPaused);
    }

    void OnTimerEnd()
    {
        Debug.Log("Timer has ended!");
        AudioSource audioTime = audioTimer.GetComponent<AudioSource>();
        audioTime.PlayOneShot(dingSound);
        audioTimer.SetActive(false);
        if (finishedCustomers < maxCustomersInLine)
        {
            lostLevelScene.SetActive(true);
        }
        // Handle what happens when the timer reaches 0
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
        customerCount += 1;
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
            finishedCustomers += 1;

            if (finishedCustomers >= customerCount)
            {
                UnlockNextLevel();

            }
        }
    }

    public void UnlockNextLevel()
    {
        timerRunning = false;
        AudioSource audioTime = audioTimer.GetComponent<AudioSource>();
        audioTime.PlayOneShot(dingSound);
        audioTimer.SetActive(false);
        nextLevelScene.SetActive(true);
        string levelname = "Level" + currentLevel + "HS";
        Debug.Log(levelname);
        if (PlayerPrefs.GetInt(levelname, 0) < pointsText.points)
        {
            PlayerPrefs.SetInt(levelname, pointsText.points);
        }
        int nextLevel = PlayerPrefs.GetInt("LevelUnlocked", 1);
        Debug.Log("LevelUnlocked" + nextLevel);
        if (nextLevel <= currentLevel + 1 && currentLevel + 1 != 5)
        {
            PlayerPrefs.SetInt("LevelUnlocked", currentLevel + 1);
        }
    }

    bool ShouldPauseSpawning()
    {
        if (customers.Count >= maxCustomersInLine || customerCount >= maxCustomers)
        {
            return true;
        }
        print("SPAWNING CUSTOMER NOW" + customers.Count);
        return false;
    }
}
