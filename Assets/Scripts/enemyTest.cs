using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyTest : MonoBehaviour
{
    Animator anim;

    public int hitPoints = 10;
    public float moveSpeed = 3;

    public GameObject Pellet;

    public GameObject target;

    Vector3 moveVector;

    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("PlayerHead");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveVector.z = 1;
        transform.position += -moveVector * moveSpeed * Time.deltaTime;

        if(hitPoints <= 0)
        {
            Destroy(gameObject);

        }
    }

    public void TakeDamage(int damage)
    {
        anim.Play("flash");
        hitPoints -= damage;
    }

    public void ShootAtPlayer(Transform aimDir)
    {
        
    }
}
