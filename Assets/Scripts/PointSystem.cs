using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this to use SceneManager

public class PointSystem : MonoBehaviour
{
    public int points = 0;
    public TMP_Text pointsText;
    public int failThreshold = -50; // Set your fail threshold here
    public GameObject failMessage; // UI element to display fail message

    void Start()
    {
        UpdatePointsText();
        if (failMessage != null)
        {
            failMessage.SetActive(false); // Ensure the fail message is hidden at start
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsText();
        CheckFailCondition();
    }

    void UpdatePointsText()
    {
        if (pointsText != null)
            pointsText.text = "Dollars: " + points.ToString();
        else
            Debug.LogError("PointsText is not assigned in the Inspector!");
    }

    void CheckFailCondition()
    {
        if (points < failThreshold)
        {
            TriggerLevelFail();
        }
    }

    void TriggerLevelFail()
    {
        Debug.Log("Level Failed!");
        if (failMessage != null)
        {
            failMessage.SetActive(true); // Show the fail message
        }
        Invoke("ResetLevel", 2f); // Wait for 2 seconds before resetting the level
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
