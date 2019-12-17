using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJeepAndHeli : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3((-100) * Time.deltaTime, 0f, 0f);
    }
}
