using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletScript : MonoBehaviour
{

    public float moveSpeed = 10;
    public float damageLevel = 1;
    public Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LifeTime");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            Debug.Log("Hit " + other);
            other.SendMessage("TakeDamage", damageLevel, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
        
    }

    public void SetDamage(float damage)
    {
        damageLevel = damage;
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);             
    }

    public void Move()
    {
        
    }
}
