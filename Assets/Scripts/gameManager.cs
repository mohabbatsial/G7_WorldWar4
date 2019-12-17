using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject objmoveRight;
    public GameObject objmoveLeft;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InstantiateKnowRight", 0f, 10f);
        InvokeRepeating("InstantiateKnowLeft", 0f, 10f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InstantiateKnowRight()
    {
        GameObject _objmoveRight = Instantiate(objmoveRight);
        Destroy(_objmoveRight, 10f);
    }
    void InstantiateKnowLeft()
    {
        GameObject _objmoveLeft = Instantiate(objmoveLeft);
        Destroy(_objmoveLeft, 10f);
    }
}
