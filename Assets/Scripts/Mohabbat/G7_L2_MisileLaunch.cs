using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_L2_MisileLaunch : MonoBehaviour
{
    public AudioSource L2_Object;
    public AudioClip Missile;
    AudioClip[] L2_Array;
    public float rand;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 parent = this.transform.parent.transform.position;
        this.transform.position = new Vector3(parent.x, parent.y, parent.z);
        rand = Random.Range(1200f, 100f);
        //L2_Object.clip = Missile;
        L2_Array = new AudioClip[] { Missile };
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.position.x <= rand)
        {
            this.transform.position += new Vector3(0, (-100) * Time.deltaTime, 0);
            L2_Object.Play();
        }
       
        if (!L2_Object.isPlaying)
        {
            int index = Random.Range(0, L2_Array.Length);
            AudioClip clip = L2_Array[index];
            L2_Object.clip = clip;
            L2_Object.Play();
        }
    }
}
