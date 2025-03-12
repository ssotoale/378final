using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject alive across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances when reloading
        }
    }
}
