using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4 : MonoBehaviour
{
     // load in levels
    private void OnMouseDown()
    {
        PlayerPrefs.SetInt("LevelPlaying", 4);
        SceneManager.LoadScene("Gameplay"); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
