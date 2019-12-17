using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
     public GameObject obj;
     public GameObject heli;
   
    void Start()
    {
        InvokeRepeating("InstantiateKnow", 0f, 6f);
        InvokeRepeating("InstantiateKnowheli", 0f, 6f);
       
    }

    // Update is called once per frame
    void Update()
    {


    }
    void InstantiateKnow()
    {
       GameObject _obj = Instantiate(obj, new Vector3(926, 263, 144), Quaternion.Euler(0f, 90f, 0f));
        Destroy(_obj, 20f);
    }
    void InstantiateKnowheli()
    {
        GameObject _heli = Instantiate(heli);
        Destroy(_heli, 10f);
    }

}