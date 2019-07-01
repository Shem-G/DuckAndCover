using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.SceneManagement;

public class NewCoverSystem : MonoBehaviour
{
    PlayerAim playerAim;
    RapidGun rapidGun;
    public Animator anim;

    //Camera
    public Transform Cam;
    public Transform Bod;
    public Vector3 DuckPosL;
    public Vector3 DuckPosR;
    public Vector3 SwitchPos;
    public Vector3 AimPos;
    public Vector3 RunPos;
    public float camSwitchTime = 10f;

    Collider boundingBox;
    Ray lookRay;
    Vector3 lookDir;
    public float switchDir;
    RaycastHit switchHit;
    bool doSwitch = false;
    Ray coverRay;
    RaycastHit coverHit;
    public LayerMask cover;

    public float sidleSpeed = 0.5f;
    Vector3 coverPos;

    //Bod
    Quaternion initialBod = new Quaternion(0, 0, 0, 0);

    public List<Transform> Covers;
    public float CoverZOffset = -.5f;
    
    bool canShoot;

    public float moveSpeed = 1f;
    public bool aiming = false;

    //Player States
    public StateMachine<States> fsm;
    public enum States
    {
        Init,
        InCover,
        Aiming,
        Ducked,
        Moving,
        EnteringCover,
        ExitingCover,
        SwitchingCover
    }
        
    void Awake()
    {
        fsm = StateMachine<States>.Initialize(this, States.Init);
        rapidGun = GetComponent<RapidGun>();
        playerAim = GetComponent<PlayerAim>();
        //anim = GetComponent<Animator>();
        DuckPosL = new Vector3(-0.5F, 1.5f, -2f);
        DuckPosR = new Vector3(0.5F, 1.5f, -2f);
        SwitchPos = new Vector3(0f, 2f, -4f);
        AimPos = new Vector3(0.75f, 1.5f, -1.5f);
        RunPos = new Vector3(0f, 1.5f, -1f);
        //switchDir = 1;
    }

    // STATE FUNCTIONS START //

    //Init
    void Init_Enter()
    {

    }

    void Init_Update()
    {
        MoveCamera(DuckPosL);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            fsm.ChangeState(States.Moving);
        }
    }
    
    void Init_FixedUpdate()
    {

    }

    void Init_Exit()
    {

    }

    //InCover
    void InCover_Enter()
    {
        
    }

    void InCover_Update()
    {

    }

    void InCover_FixedUpdate()
    {
        
        
    }

    void InCover_Exit()
    {

    }

    //Aiming
    void Aiming_Enter()
    {
        canShoot = true;
        anim.SetBool("standing", true);
        
    }

    void Aiming_Update()
    {
        //aiming = true;
        //anim.SetBool("standing", true);
        if (!Input.GetButton("Fire2"))
        {
            fsm.ChangeState(States.Ducked);
        }
        else
        {
            //anim.SetBool("standing", true);
        }
    }

    void Aiming_FixedUpdate()
    {
        AimPos.x = 0.75f * switchDir;
        playerAim.AimCamera();
        playerAim.RotateBod();
        MoveCamera(AimPos);
    }

    void Aiming_Exit()
    {

    }

    //Ducked
    void Ducked_Enter()
    {
        anim.SetBool("standing", false);
        canShoot = false;
        //Bod.transform.rotation = initialBod;
    }

    void Ducked_Update()
    {
        
        if (Input.GetButton("Fire2"))
        {
            fsm.ChangeState(States.Aiming); 
        }
        //
    }

    void Ducked_FixedUpdate()
    {
        DuckPosL.x = 0.75f * switchDir;
        DuckPosR.x = 0.75f * switchDir;
        playerAim.AimCamera();
        if(switchDir < 0)
        {
            MoveCamera(DuckPosL);
        }
        if (switchDir > 0)
        {
            MoveCamera(DuckPosR);
        }

        Sidle();
        
    }

    void Ducked_Exit()
    {

    }

    //Moving
    void Moving_Enter()
    {

    }

    void Moving_Update()
    {
        if (Input.GetButton("Fire2"))
        {
            fsm.ChangeState(States.Aiming);
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            switchDir = Input.GetAxisRaw("Horizontal");
            anim.SetFloat("facingDir", switchDir);
        }
    }

    void Moving_FixedUpdate()
    {
        playerAim.AimCamera();
        if (switchDir < 0)
        {
            MoveCamera(DuckPosL);
        }
        if (switchDir > 0)
        {
            MoveCamera(DuckPosR);
        }


        anim.SetBool("standing", false);
        anim.SetBool("running", true);

        float step = moveSpeed * Time.deltaTime;

        Vector3 newPos = new Vector3(Covers[0].transform.position.x, transform.position.y, Covers[0].transform.position.z + CoverZOffset);
        transform.position = Vector3.MoveTowards(transform.position, newPos, step);
        
        if (Vector3.Distance(transform.position, newPos) < 0.001f)
        {
            fsm.ChangeState(States.EnteringCover);
        }
        
        //Strafe();
    }

    void Moving_Exit()
    {

    }

    //Entering Cover
    void EnteringCover_Enter()
    {
        anim.SetBool("running", false);
        
        Vector3 newPos = new Vector3(Covers[0].position.x, transform.position.y, Covers[0].position.z + CoverZOffset);

        Debug.Log("Entering Cover");
        float step = moveSpeed * Time.deltaTime;


        transform.position = Vector3.MoveTowards(transform.position, newPos, step);
        if (Vector3.Distance(transform.position, newPos) < 0.001f)
        {
            fsm.ChangeState(States.Ducked);
        }



    }

    void EnteringCover_Update()
    {

    }


    void EnteringCover_FixedUpdate()
    {
        


    }

    void EnteringCover_Exit()
    {
        Covers.RemoveAt(0);
        Debug.Log(fsm.State.ToString());
    }

    //ExitingCover
    void ExitingCover_Enter()
    {

    }

    void ExitingCover_Update()
    {

    }

    void ExitingCover_FixedUpdate()
    {

    }

    void ExitingCover_Exit()
    {

    }

    void SwitchingCover_Enter()
    {
        if (Physics.Raycast(transform.position, transform.right * switchDir, out switchHit, Mathf.Infinity, cover, QueryTriggerInteraction.UseGlobal))
        {
            coverPos = new Vector3(switchHit.transform.position.x, transform.position.y, transform.position.z);
            doSwitch = true;
        }
        else
        {
            doSwitch = false;
            fsm.ChangeState(States.Ducked);
            coverPos = transform.position;
        }
    }

    void SwitchingCover_Update()
    {

    }

    void SwitchingCover_FixedUpdate()
    {
        if (doSwitch)
        {
            MoveCamera(SwitchPos);
            float step = 15 * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, coverPos, step);
            if (Vector3.Distance(transform.position, coverPos) < 0.001f)
            {
                doSwitch = false;
            }
        }
        else
        {
            fsm.ChangeState(States.Ducked);
        }
    }


    void SwitchingCover_Exit()
    {
        switchDir = -switchDir;
        doSwitch = false;
    }

    // STATE FUNCTIONS END //

    private void Start()
    {
        boundingBox = GetComponent<Collider>();
        lookDir = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            fsm.ChangeState(States.Moving);
        }
        if (Input.GetButton("Fire2"))
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        

        Debug.DrawRay(transform.position, transform.right * switchDir * 5, Color.red);

        Debug.DrawRay(lookDir, transform.forward * 5, Color.blue);
        Debug.DrawRay(transform.position, transform.right * Input.GetAxisRaw("Horizontal") * Mathf.Infinity);
        //Debug.DrawRay(right, transform.forward * 5, Color.blue);
    }

    private void Sidle()
    {
        lookDir.x = transform.position.x + Input.GetAxisRaw("Horizontal") * 0.5f;
        ///right.x = transform.position.x + 0.5f;

        lookRay = new Ray(lookDir, transform.forward * 5);
        //leftRay = new Ray(right, transform.forward * 5);

        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            switchDir = Input.GetAxisRaw("Horizontal");
            anim.SetFloat("facingDir", switchDir);
        }
        
        
        if (Physics.Raycast(lookRay, out coverHit, 5, cover, QueryTriggerInteraction.UseGlobal))
        {
            transform.Translate(transform.right * Input.GetAxisRaw("Horizontal") * sidleSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.L) && switchDir != 0 && Physics.Raycast(transform.position, transform.right * switchDir, out switchHit, Mathf.Infinity, cover, QueryTriggerInteraction.UseGlobal))
            {
                fsm.ChangeState(States.SwitchingCover);
            }
        }
    }

    public void Strafe()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.Translate(moveDir.normalized * sidleSpeed * Time.deltaTime);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (fsm.State == States.Moving)
        {
            if (other.tag == "coverPoint")
            {
                coverPos = other.gameObject.transform.position;
                fsm.ChangeState(States.EnteringCover);
            }
        }
        
    }*/

    public void ChangeState()
    {
        fsm.ChangeState(States.Moving);
    }

    public void MoveCamera(Vector3 statePos)
    {
        Cam.transform.position = Vector3.Lerp(Cam.transform.position, transform.position + statePos, camSwitchTime * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(lookDir, transform.forward * 5, Color.blue);
        //Debug.DrawRay(right, transform.forward * 5, Color.blue);
    }


}
