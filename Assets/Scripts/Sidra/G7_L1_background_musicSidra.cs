using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_L1_background_musicSidra : MonoBehaviour
{
    public AudioSource BackgroundMusicObject;
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    public AudioClip music4;
    public AudioClip music5;
    

    AudioClip[] BackgroundAudioArray;
    void Start()
    {
        BackgroundAudioArray = new AudioClip[] { music1, music2, music3, music4, music5};   
    }

    // Update is called once per frame
    void Update()
    {
        if (!BackgroundMusicObject.isPlaying)
        {
            int index = Random.Range(0, BackgroundAudioArray.Length);
            AudioClip clip = BackgroundAudioArray[index];
            BackgroundMusicObject.clip = clip;
            BackgroundMusicObject.Play();
        }
    }
}
