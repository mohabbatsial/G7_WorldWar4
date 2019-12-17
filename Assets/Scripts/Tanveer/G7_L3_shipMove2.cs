using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_L3_shipMove2 : MonoBehaviour
{
    public AudioSource L3_Object;
    public AudioClip ship2;
    // Start is called before the first frame update
    void Start()
    {
        L3_Object.clip = ship2;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3((-100) * Time.deltaTime, 0f, 0f);
        L3_Object.Play();
    }
}
