using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform barrelEnd;
    public int bulletspeed;
    public float despawntime;
    public bool shootable = true;
    public float waitbeforenextshoot = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shootable)
            {
                shootable = false;
                Shooting();
                StartCoroutine(ShootingYield());
            }
        }
    }
    IEnumerator ShootingYield()
    {
        yield return new WaitForSeconds(waitbeforenextshoot);
        shootable = true;

    }
    void Shooting()
    {
        var Bullet = Instantiate(bullet, barrelEnd.position, barrelEnd.rotation);
        Bullet.GetComponent<Rigidbody>().velocity = Bullet.transform.forward * bulletspeed;
        Destroy(Bullet, despawntime);

    }
   
}
