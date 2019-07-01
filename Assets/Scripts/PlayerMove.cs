using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{

    /// <summary>
    /// 1. Duck by default
    /// 2. Stand on button press
    /// 3. Sidle while ducked?
    /// </summary>

    public Animator anim;
    bool Standing = false;
    public PlayerAim playerAim;

    float nextFire;
    public float firerate;

    public Transform Cam;
    public Vector3 DuckPos;
    public Vector3 AimPos;
    public float camSwitchTime = 0.3f;

    public int maxAmmo = 30;
    public int curAmmo = 30;
    public bool reloading;
    public bool canShoot;

    public bool runningForward = false;
    public float runTime = 10;
    
    public Vector3 zTarget = new Vector3(0,0,10);
    public LayerMask coverLayer;
    public Transform curCover;
    public Transform nextCover;
    public Vector3 footPos;

    public bool movingToCover;

    public NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        //anim.speed = 2.5f;

        playerAim = GetComponent<PlayerAim>();
        nextFire = 0;

        canShoot = false;
        movingToCover = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if (anim.GetBool("standing") || anim.GetBool("running"))
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        Vector3 curPos = transform.position;

        if (Input.GetButton("Fire2") && !anim.GetBool("running"))
        {
                anim.SetBool("standing", true);
        }
        if(!Input.GetButton("Fire2") && !anim.GetBool("running"))
        {
            anim.SetBool("standing", false);
        }

        if (canShoot && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            //Shoot();
        }

        if (runningForward)
        {
            anim.SetBool("running", true);
            transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, 1) * runTime * Time.deltaTime);
        }

        if (!runningForward)
        {
            anim.SetBool("running", false);
        }

        if (movingToCover)
        {
            Vector3 newCover = new Vector3(nextCover.transform.position.x, transform.position.y, nextCover.transform.position.y);
            transform.position = Vector3.Lerp(transform.position, newCover, 1f);
        }
        //footPos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        //Debug.DrawRay(footPos, transform.forward * 1000, Color.red);

        if (Input.GetKeyDown(KeyCode.C))
        {
            //runningForward = !runningForward;
            movingToCover = !movingToCover;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "cover")
        {
            Debug.Log(other);
            nextCover = other.transform;
            runningForward = false;
            movingToCover = true;
        }

    }

    void Shoot()
    {
        if (curAmmo > 0)
        {
            curAmmo--;
            nextFire = Time.time + firerate;
        }
        else
        {
            Reload();
        }
    }

    void Reload()
    {
        StartCoroutine("ReloadTime", 1f);
    }

    IEnumerator ReloadTime(float time)
    {
        reloading = true;
        anim.SetBool("reloading", true);
        anim.SetBool("standing", false);
        yield return new WaitForSeconds(time);
        curAmmo = maxAmmo;
        anim.SetBool("reloading", false);
        reloading = false;
    }

    void IntoCover(Transform cover)
    {
        //nextCover = cover;
        runningForward = false;
        movingToCover = true;
        //transform.position = Vector3.Lerp(transform.position, cover.position, 1f);
    }

    void SwitchCover()
    {

    }

    void StandUp()
    {
        
        //Debug.Log(anim.GetBool("standing"));
    }

    void crouchDown()
    {
        
        //Debug.Log(anim.GetBool("standing"));
    }

}

