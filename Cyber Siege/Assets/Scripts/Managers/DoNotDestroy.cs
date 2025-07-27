using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    public static DoNotDestroy main;
    public float masterVolume = 0f; // Radius
    public float soundFXVolume = 0f; // Radius
    public float musicVolume = 0f; // Radius

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Music");
        // Important as it prevents multiple SoundManagers from being created.
        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        main = this;
    }


}
