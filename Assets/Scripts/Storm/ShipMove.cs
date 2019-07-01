using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour
{
    bool movingX;
    bool movingY;

    public float MaxSpeed = 20;
    public float MinSpeed = 0.1f;
    public float AccelSpeed = 2f;
    public float CurSpeed;
    public float TurnSpeed = 15;
    public float YSpeed = 10;
    public float PitchSpeed = 10;

    public float maxDegrees = 70;

    Vector3 input;
    Vector3 finalMove;

    Vector3 rotation;

    CharacterController cc;
    
    public GameObject shot;
    public Transform cam;
    public Transform barrel;

    Vector3 initialRot;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
        initialRot = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal2");
        input.y = Input.GetAxisRaw("Vertical2");

        if (Input.GetButton("Fire1"))
        {
            CurSpeed += AccelSpeed;
        }
        if (Input.GetButton("Fire2"))
        {
            CurSpeed -= AccelSpeed;
        }

        CurSpeed = Mathf.Clamp(CurSpeed, MinSpeed, MaxSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject p = Instantiate(shot);
            p.transform.position = transform.position;
            p.transform.LookAt(cam.transform.position);
            p.GetComponent<ShotMovement>().targetDirection = barrel.transform.forward;
            p.GetComponent<ShotMovement>().speed = 100;
            p.GetComponent<ShotMovement>().timeOut = 2;
        }
    }

    void Move()
    {
        cc.Move(transform.forward * CurSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        if(input != Vector3.zero)
        {
            rotation.y = input.x * TurnSpeed;
            rotation.x = input.y * PitchSpeed;
            transform.Rotate(rotation * Time.deltaTime);
        }
        else
        {
           /// Auto rotate code here ///
        }
        
    }

    void RotateTopHalf()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Rotate();
        rotation.x = Mathf.Clamp(rotation.x, maxDegrees, -maxDegrees);
    }
}
