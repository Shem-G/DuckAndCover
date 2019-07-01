using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAim : MonoBehaviour
{

    public Transform Cam;
    public Transform Bod;
    NewCoverSystem ncs;
    PlayerManager pm;
    public Transform crosshair;

    public float maxXAngle = 70;
    public float maxYAngle = 10;

    int x = Screen.width / 2;
    int y = Screen.height / 2;
    float z;

    public LayerMask layer;

    public float Rotation_Speed;
    public float Rotation_Friction; //The smaller the value, the more Friction there is. [Keep this at 1 unless you know what you are doing].
    public float Rotation_Smoothness; //Believe it or not, adjusting this before anything else is the best way to go.
    private float Resulting_Value_from_Input;
    private float Resulting_Value_from_Input_Y;
    private Quaternion Quaternion_Rotate_From;
    private Quaternion Quaternion_Rotate_From_Y;
    private Quaternion Quaternion_Rotate_To;
    private Quaternion Quaternion_Rotate_To_Y;

    //Bod
    Quaternion initialBod = new Quaternion(0, 0, 0, 0);

    Vector3 target;

    LineRenderer lr;
    //public RaycastHit hit;
    public float sightLength = 100;
    public float sphereRadius = 2;
    public GameObject currentHitObject;
    private float currentHitDistance;
    Ray ray;
    Ray newRay;
    RaycastHit hit;
    public GameObject blastObject;
    public GameObject barrelBlast;
    public GameObject newPellet;
    public Transform circle;
    public Transform barrel;

    public float damageLevel = 20;

    public float shotVarianceValue = 0.015f;

    float nextFire;
    public float firerate;
    public float machineFireRate;

   


    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        ncs = GetComponent<NewCoverSystem>();
        pm = GetComponent<PlayerManager>();

    }

    public void AimCamera()
    {
        /// CAM MOVEMENT ///
        Resulting_Value_from_Input += Input.GetAxis("Horizontal2") * Rotation_Speed * Rotation_Friction;
        Resulting_Value_from_Input = Mathf.Clamp(Resulting_Value_from_Input, -maxXAngle, maxXAngle);

        Resulting_Value_from_Input_Y += Input.GetAxis("Vertical2") * Rotation_Speed * Rotation_Friction;
        Resulting_Value_from_Input_Y = Mathf.Clamp(Resulting_Value_from_Input_Y, -maxYAngle, maxYAngle);
        //You can also use "Mouse X"
        Quaternion_Rotate_From = Cam.transform.rotation;
        Quaternion_Rotate_To = Quaternion.Euler(-Resulting_Value_from_Input_Y, Resulting_Value_from_Input, 0);

        Cam.transform.rotation = Quaternion.Lerp(Quaternion_Rotate_From, Quaternion_Rotate_To, Time.deltaTime * Rotation_Smoothness);
        //Bod.transform.rotation = Quaternion.Lerp(Quaternion_Rotate_From, Quaternion_Rotate_To, Time.deltaTime * Rotation_Smoothness);
        
    }

    private void Update()
    {
        x = Screen.width / 2;
        y = Screen.height / 2;
        z = Cam.position.z;

        if (pm.GunType == 0)
        {
            damageLevel = 35;
            sightLength = 25;
        }
        else
        {
            damageLevel = 20;
            sightLength = 50;
        }


    }

    public void RotateBod()
    {

        target = new Vector3(x, y, z);
        ray = Cam.GetComponentInChildren<Camera>().ScreenPointToRay(new Vector2(x, y));
        Bod.LookAt(Bod.transform.position + ray.direction * sightLength);
        lr.SetPosition(0, Bod.transform.position);
        //lr.SetPosition(1, Bod.transform.position + ray.direction * sightLength);

        if (ncs.aiming)
        {
            switch (pm.GunType)
            {
                case 0:
                    newShoot();
                    break;
                case 1:
                    machineGunShoot();
                    break;
                case 2:
                    rocketShoot();
                    break;
                default:
                    newShoot();
                    break;
            }


        }
        
    }

    public void newShoot()
    {
        lr.SetPosition(1, Bod.transform.position);
            if (Physics.SphereCast(barrel.transform.position, sphereRadius, ray.direction, out hit, sightLength, layer, QueryTriggerInteraction.UseGlobal))
            {
                
                currentHitObject = hit.transform.gameObject;
                currentHitDistance = hit.distance;
                crosshair.transform.position = hit.point;
            }
            else
            {
                currentHitObject = null;
                currentHitDistance = sightLength;
            }
        if (!hit.transform)
        {
            crosshair.transform.position = new Vector3(9999, 9999, 9999);
            
        }

        if (Input.GetButton("Fire1"))
        {
            if (Time.time > nextFire)
            {
                if (currentHitObject != null)
                {   
                    GameObject bar = Instantiate(barrelBlast, Bod);
                    bar.transform.position = new Vector3(Bod.transform.position.x, Bod.transform.position.y, Bod.transform.position.z + 1);
                    /*GameObject p = Instantiate(newPellet);
                    p.transform.position = Bod.transform.position;
                    p.transform.rotation = barrel.transform.rotation;
                    p.GetComponent<newPelletShot>().target = hit.transform.position;
                    p.GetComponent<newPelletShot>().travelTime = 7f;
                    */
                    ncs.anim.SetTrigger("shoot");
                    GameObject b = Instantiate(blastObject);
                    b.transform.position = hit.point;
                    nextFire = Time.time + firerate;
                    currentHitObject.SendMessage("TakeDamage", damageLevel, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    GameObject b = Instantiate(blastObject);
                    b.transform.position = barrel.transform.position + ray.direction * sightLength;
                    ncs.anim.SetTrigger("shoot");
                    /*GameObject p = Instantiate(newPellet);
                    p.transform.position = Bod.transform.position;
                    p.transform.rotation = barrel.transform.rotation;
                    p.GetComponent<newPelletShot>().target = hit.transform.position;
                    p.GetComponent<newPelletShot>().travelTime = 7f;
                    */
                    nextFire = Time.time + firerate;
                }
            }
        }

        circle.transform.position = Vector3.Lerp(circle.transform.position, barrel.transform.position + ray.direction * currentHitDistance, 0.2f);

    }

    public void machineGunShoot()
    {
        crosshair.transform.position = new Vector3(9999, 9999, 9999);
        if (Input.GetButton("Fire1"))
        {
            if (Time.time > nextFire)
            {
                RaycastHit newHit = new RaycastHit();
                Vector3 newVariance = new Vector3(ShotVariance(), ShotVariance(), 0);
                newRay = new Ray(barrel.transform.position, ray.direction + newVariance);

                if (Physics.Raycast(newRay, out newHit, sightLength, layer, QueryTriggerInteraction.UseGlobal))
                {
                    if (newHit.collider)
                    {
                        GameObject b = Instantiate(blastObject, newHit.transform);
                        b.transform.position = newHit.point;
                        newHit.transform.gameObject.SendMessage("TakeDamage", damageLevel, SendMessageOptions.DontRequireReceiver);
                        nextFire = Time.time + machineFireRate;
                        ncs.anim.SetTrigger("shoot");
                        lr.SetPosition(1, newHit.point);
                    }
                }
                else
                {
                    GameObject b = Instantiate(blastObject);
                    b.transform.position = Bod.transform.position + newRay.direction * sightLength;
                    
                    nextFire = Time.time + machineFireRate;
                    ncs.anim.SetTrigger("shoot");
                    lr.SetPosition(1, Bod.transform.position + newRay.direction * sightLength);
                    Debug.DrawLine(barrel.transform.position, Bod.transform.position + newRay.direction * sightLength, Color.green);
                }
            }
        }

        circle.transform.position = Vector3.Lerp(circle.transform.position, barrel.transform.position + ray.direction * sightLength, 0.2f);
    }

    public void rocketShoot()
    {
        lr.SetPosition(1, Bod.transform.position + ray.direction * sightLength);
        circle.transform.position = Vector3.Lerp(circle.transform.position, barrel.transform.position + ray.direction * sightLength * 2, 0.2f);
        crosshair.transform.position = Bod.transform.position + ray.direction * sightLength;
        if (Input.GetButton("Fire1"))
        {
            if (Time.time > nextFire)
            {
                ncs.anim.SetTrigger("shoot");
                GameObject p = Instantiate(newPellet);
                p.transform.position = Bod.transform.position;
                p.transform.LookAt(crosshair.transform.position);
                p.GetComponent<newPelletShot>().targetDirection = crosshair.transform.position;
                p.GetComponent<newPelletShot>().speed = 50;
                p.GetComponent<newPelletShot>().timeOut = 2;
                nextFire = Time.time + firerate;
            }
        }
    }


    public float ShotVariance()
    {
        float r = Random.Range(-shotVarianceValue, shotVarianceValue);
        return r;
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Debug.DrawLine(barrel.transform.position, Bod.transform.position + ray.direction * currentHitDistance);
        //Gizmos.DrawWireSphere(barrel.transform.position + ray.direction * currentHitDistance, sphereRadius);
        
    }
}
