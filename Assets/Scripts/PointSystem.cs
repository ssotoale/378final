using UnityEngine;
using TMPro; 

public class PointSystem : MonoBehaviour
{
    public int points = 0;
    public TMP_Text pointsText; 

    void Start()
    {
        UpdatePointsText();
    }

    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsText();
    }

    void UpdatePointsText()
    {
        if (pointsText != null)
            pointsText.text = "Points: " + points.ToString();
        else
            Debug.LogError("PointsText is not assigned in the Inspector!");
    }
}
