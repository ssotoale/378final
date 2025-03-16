using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    // testing
    void OnEnable()
    {
        SceneManager.LoadScene("LevelMenu", LoadSceneMode.Single);
    }

}
