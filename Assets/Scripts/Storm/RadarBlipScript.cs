using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBlipScript : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public Transform player;

    public Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(0, 0, 0);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {



        if (target)
        {

            //position.y = player.transform.position.y - target.position.y;
            /*
            if (position.x > 1 || position.x < 1)
            {
                position.x = Mathf.Sign(player.transform.position.x - target.position.x);
            }

            if (position.y > 1 || position.y < 1)
            {
                position.y = Mathf.Sign(player.transform.position.y - target.position.y);
            }
            */
        }

        position = player.transform.forward - target.position;

        //position.x = Mathf.Clamp(position.x, 0, 1);
        //position.y = Mathf.Clamp(position.y, 0, 1);

    }

    private void FixedUpdate()
    {
        if (target.GetComponentInChildren<Renderer>().IsVisibleFrom(cam))
        {
            //transform.position = transform.position - position.normalized;
            //transform.position = new Vector3(transform.parent.transform.position.x - target.position.x, transform.parent.transform.position.y - target.position.y, transform.parent.transform.position.z);

        }
        else
        {
           // transform.position = transform.position - position.normalized;
        }
    }
}
