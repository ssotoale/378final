using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levelGameObjects; // Assign Level1, Level2, Level3 GameObjects in Unity
    public GameObject[] levelTexts;

    void Start()
    {
        int levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);
        Debug.Log(levelUnlocked);
        for (int i = 0; i < levelUnlocked; i++)
        {
            levelGameObjects[i].SetActive(true);
            string levelname = "Level" + (i+1) + "HS";
            levelTexts[i].SetActive(true);
            TextMeshProUGUI newText = levelTexts[i].GetComponent<TextMeshProUGUI>();
            if (levelUnlocked == 4 && i == 3)
            {
                newText.text = "Endless HS: " + PlayerPrefs.GetInt("Level4HS", 0); 
            }
            else
            {
                newText.text = "Level " + (i+1) + " HS: " + PlayerPrefs.GetInt(levelname, 0);
            }
        }
    }

}
