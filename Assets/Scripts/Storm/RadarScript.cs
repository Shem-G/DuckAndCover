using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScript : MonoBehaviour
{

    public Camera ViewCam;

    Vector3 RadarSquare;

    public List<GameObject> TargetArray = new List<GameObject>();
    public List<GameObject> TargetsInView = new List<GameObject>();
    public List<Vector3> TargetLocationArray = new List<Vector3>();
    public List<GameObject> BlipArray = new List<GameObject>();

    public GameObject blip;
    public GameObject canvas;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetTargets();
    }

    // Update is called once per frame
    void Update()
    {
        //RadarSquare = ViewCam.WorldToViewportPoint();
        //GetTargetLocations();
        transform.position = player.transform.position;
        transform.rotation = new Quaternion(0,player.transform.rotation.y, 0,0);
        for (int i = 0; i < TargetArray.Count; i++)
        {
            Transform t = TargetArray[i].transform;
            Vector3 pos = new Vector3(t.transform.position.x, t.transform.position.y, t.transform.position.z);
            Vector3 direction = pos - transform.position;
            //direction.Normalize();
            Debug.DrawRay(transform.position, direction, Color.blue);
            BlipArray[i].transform.position = new Vector3(transform.position.x + t.transform.position.x, transform.position.y + t.transform.position.y, transform.position.z);
        }
        {
            
        }
    }

    void GetTargets()
    {
         TargetArray.AddRange(GameObject.FindGameObjectsWithTag("target"));

        foreach(GameObject target in TargetArray)
        {
            GameObject b = Instantiate(blip, transform);
            b.transform.position = transform.position;
            //b.GetComponent<RadarBlipScript>().target = target.transform;
            //b.GetComponent<RadarBlipScript>().player = transform;
            BlipArray.Add(b);
        }
          
    }

    void GetTargetLocations()
    {

        for(int i = 0; i < TargetArray.Count; i++)
        {

        }
        foreach(GameObject g in TargetArray)
        {
            TargetLocationArray.Add(g.transform.position);
            if (g.GetComponentInChildren<Renderer>().IsVisibleFrom(ViewCam))
            {
                Debug.Log(g.name);
            }
        }
        
    }

}
