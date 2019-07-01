using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camScript : MonoBehaviour
{
    public Transform camObject;
    public Transform target;
    public Vector3 camOffset = new Vector3(0, 1, -16);
    playerMove pm;
    public float smoothTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<playerMove>();
    }

    // Update is called once per frame
    void LateUpdate()
        
    {
        if(pm.input.x != 0)
        {
            camOffset.x =  pm.input.x *5;
        }
        
        camObject.transform.position = Vector3.Lerp(camObject.transform.position, target.transform.position + camOffset, smoothTime * Time.deltaTime);
    }
}
