using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{

    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("LevelUnlocked", 1);
        Debug.Log("PlayerPrefs cleared!");
    }


    // testing
    void OnEnable()
    {
        SceneManager.LoadScene("MainScreen", LoadSceneMode.Single);
    }

}
