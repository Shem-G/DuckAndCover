using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    CharacterController cc;

    public Vector2 input;
    public bool flying;
    public float jetFuel;
    public float moveSpeed = 10;
    public float flyPower = 10;
    public float gravityMultiplier = 20;

    Vector2 totalMove;
    public Transform model;
    public ParticleSystem particle;
    public float turnTime = 10;

    public float jumpPower = 10;

    Quaternion lookRot;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        ApplyGravity();
        Move();

        if(Input.GetButton("Fire2"))
        {
            Fly();
        }
        if (Input.GetButton("Fire1") && cc.velocity.y == 0)
        {
            totalMove.y = jumpPower;
        }

        if(input.x == 1)
        {
            lookRot.y = 0;
        }
        if (input.x == -1)
        {
            lookRot.y = -180;
        }

        model.transform.rotation = lookRot;
        if (cc.isGrounded)
        {
            particle.Stop();
        }

    }

    void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        totalMove.x = input.x * moveSpeed;
        cc.Move(totalMove * Time.deltaTime);
        
        
    }

    void ApplyGravity()
    {
        if (!cc.isGrounded)
        {
            totalMove.y -= gravityMultiplier * Time.deltaTime;
        }
        if (cc.isGrounded)
        {
            totalMove.y = 0;
        }
    }

    void Fly()
    {
        particle.Play(); 
        totalMove.y += flyPower;
    }
}
