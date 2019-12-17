using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L3_WaterSound : MonoBehaviour
{
    public AudioSource WaterSoundObject;
    public AudioClip Water1;
    public AudioClip Water2;
    public AudioClip Water3;
    AudioClip[] WaterSoundArray;
    // Start is called before the first frame update
    void Start()
    {
        WaterSoundArray = new AudioClip[] { Water1, Water2, Water3 };
    }

    // Update is called once per frame
    void Update()
    {
        if (!WaterSoundObject.isPlaying)
        {
            int index = Random.Range(0, WaterSoundArray.Length);
            AudioClip clip = WaterSoundArray[index];
            WaterSoundObject.clip = clip;
            WaterSoundObject.Play();
        }
    }
}
