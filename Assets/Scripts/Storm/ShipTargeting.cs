using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTargeting : MonoBehaviour
{
    RaycastHit hit;
    public LayerMask targetLayer;
    UITest ui;
    Ray ray;
    public Camera Cam;
    public float sphereRadius = 50f;
    public float currentHitDistance;
    public GameObject currentHitObject;
    public GameObject teleport;
    public GameObject[] TargetArray;
    public Vector3[] TargetLocationArray;

    public List<GameObject> blips = new List<GameObject>();
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindWithTag("UI").GetComponent<UITest>();
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;        

        if (Physics.SphereCast(transform.position, sphereRadius, Cam.transform.forward, out hit, Mathf.Infinity, targetLayer, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
            ui.SetTargetText(hit.transform.name);
            ui.distanceText.text = Mathf.Floor(currentHitDistance).ToString() + "M";

            if(hit.distance <= 10)
            {
                //TeleportTo(teleport.transform);
            }
        }
        else
        {
            currentHitObject = null;
            currentHitDistance = Mathf.Infinity;
            ui.SetTargetText("");
            ui.distanceText.text = "";
        }

        GetTargetLocations();

    }

    void GetTargets()
    {
        TargetArray = GameObject.FindGameObjectsWithTag("target");
    }

    void GetTargetLocations()
    {
        for(int i = 0; i < TargetArray.Length; i++)
        {
            Transform t = TargetArray[i].transform;
            blips[i].transform.LookAt(t.transform.position);



        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + Cam.transform.forward * currentHitDistance);
        Gizmos.DrawWireSphere(transform.position + Cam.transform.forward * currentHitDistance, sphereRadius);

    }

    public void TeleportTo(Transform location)
    {
        transform.position = location.transform.position;
        transform.rotation = Quaternion.identity;
    }

}
