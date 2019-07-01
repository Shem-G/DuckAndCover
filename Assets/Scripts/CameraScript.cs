using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Cam;
    public Vector3 DuckPos;
    public Vector3 AimPos;

    public Transform Player;

    PlayerMove pm;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMove>();
        Cam = Camera.main.transform;
        DuckPos = new Vector3(-4, 1.4f, -3.7f);
        AimPos = new Vector3(-4.7f, 2.3f, -2.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
