using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRotate : MonoBehaviour
{

    public float RotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, RotSpeed, 0) * Time.deltaTime);
    }
}
