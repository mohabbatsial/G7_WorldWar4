using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_L2_goPlanMohabbat : MonoBehaviour
{
    public AudioSource L2SoundObject;
    public AudioClip Jet;
    // Start is called before the first frame update
    void Start()
    {
        L2SoundObject.clip = Jet;
    }

    // Update is called once per frame
    void Update()
    {
       // this.transform.position += new Vector3((-100) * Time.deltaTime, 0f, 0f);
        this.transform.position += new Vector3((-300) * Time.deltaTime, 0f, 0f);
        L2SoundObject.Play();


    }
}
