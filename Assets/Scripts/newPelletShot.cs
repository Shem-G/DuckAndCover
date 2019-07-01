using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPelletShot : MonoBehaviour
{
    public Vector3 targetDirection;
    public float speed;
    public GameObject particleBoom;

    float sphereRadius = 4f;

    public float timeOut = 3;
    float nextFire = 3;
    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time + timeOut;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Time.time > nextFire)
        {
            explode();
        }
            

        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            explode();
        }
    }

    void explode()
    {
        GameObject part = Instantiate(particleBoom);
        part.transform.position = transform.position;
        Destroy(gameObject);
    }
}
