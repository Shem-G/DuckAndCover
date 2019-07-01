using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMovement : MonoBehaviour
{

    public Vector3 targetDirection;
    public float speed;

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
        transform.position += targetDirection * speed * Time.deltaTime;

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
        Destroy(gameObject);
    }
}
