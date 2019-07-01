using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFollow : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position =  player.transform.position + offset;
        transform.rotation = player.rotation;
    }
}
