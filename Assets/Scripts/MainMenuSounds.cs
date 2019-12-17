using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour
{
    public AudioSource MainMenuObject;
    public AudioClip Tank;
    public AudioClip Jet;
    public AudioClip Jeep;
    public AudioClip Helicopter;
    AudioClip[] MainMenuArray;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuArray = new AudioClip[] { Tank, Jet, Jeep, Helicopter};
    }

    // Update is called once per frame
    void Update()
    {
        if (!MainMenuObject.isPlaying)
        {
            int index = Random.Range(0, MainMenuArray.Length);
            AudioClip clip = MainMenuArray[index];
            MainMenuObject.clip = clip;
            MainMenuObject.Play();
        }
    }
}

