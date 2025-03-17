using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    // testing
    void OnEnable()
    {
        SceneManager.LoadScene("MainScreen", LoadSceneMode.Single);
    }

}
