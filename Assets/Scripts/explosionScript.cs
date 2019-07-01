using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionScript : MonoBehaviour
{

    ParticleSystem particle;
    float damageLevel = 50;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particle.isStopped)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("TakeDamage", damageLevel, SendMessageOptions.DontRequireReceiver);
    }
}
