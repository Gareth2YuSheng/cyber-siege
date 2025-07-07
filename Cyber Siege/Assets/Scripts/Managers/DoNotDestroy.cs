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
        // GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Music");
        DontDestroyOnLoad(this.gameObject);
        main = this;
    }


}
