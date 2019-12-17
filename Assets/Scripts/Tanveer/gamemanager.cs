using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ship;
    public GameObject Ship1;
    void Start()
    {
        InvokeRepeating("InstantiateKnowShip", 0f, 6f);
        InvokeRepeating("InstantiateKnowShip1", 0f, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InstantiateKnowShip()
    {
        GameObject _Ship = Instantiate(Ship);
        Destroy(_Ship, 20f);
    }
    void InstantiateKnowShip1()
    {
        GameObject _Ship1 = Instantiate(Ship1);
        Destroy(_Ship1, 20f);
    }
}
