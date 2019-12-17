using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject Tank1;
    public GameObject obj;

    void Start()
    {
        InvokeRepeating("InstantiateKnow", 0f, 10f);
        InvokeRepeating("InstantiateKnow1", 0f, 10f);

    }

    // Update is called once per frame
    void Update()
    {


    }
    void InstantiateKnow()
    {
        GameObject _tank1 = Instantiate(Tank1);
        Destroy(_tank1, 20f);
    }
    void InstantiateKnow1()
    {
        GameObject _obj = Instantiate(obj);
        Destroy(_obj, 20f);
    }
}
