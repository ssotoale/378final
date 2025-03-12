using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsButton : MonoBehaviour
{

    // load in levels
    private void OnMouseDown()
    {
        SceneManager.LoadScene("Controls"); 
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
