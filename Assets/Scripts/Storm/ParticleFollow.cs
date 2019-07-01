using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;

    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = target.transform.position + offset;

        var main = ps.main;
        //main.startRotationY = target.transform.position.y;

    }
}
