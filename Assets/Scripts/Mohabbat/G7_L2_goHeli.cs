


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_L2_goHeli : MonoBehaviour
{
    public AudioSource soundObject;
    public AudioClip Helicopter;
    // Start is called before the first frame update
    void Start()
    {
        soundObject.clip = Helicopter;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3((100) * Time.deltaTime, 0f, 0f);
        soundObject.Play();
    }
}
