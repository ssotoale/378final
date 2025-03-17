using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBackButton : MonoBehaviour
{
     // load in levels
    private void OnMouseDown()
    {
        SceneManager.LoadScene("MainScreen"); 
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