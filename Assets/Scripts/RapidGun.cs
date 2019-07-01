using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidGun : MonoBehaviour
{
    NewCoverSystem ncs;

    public int maxAmmo = 30;
    public int curAmmo = 30;
    float nextFire;
    public float firerate;
    
    public Transform Barrel;
    public float shotVarianceValue = 0.015f;
    public GameObject pellet;
    public float damageLevel;

    PlayerAim pa;

    private void Awake()
    {
        pa = GetComponent<PlayerAim>();
        ncs = GetComponent<NewCoverSystem>();
    }

    public void Shoot()
    {
        if (curAmmo > 0 && Time.time > nextFire)
        {
            GameObject p = Instantiate(pellet, pa.Bod.transform.position, pa.Bod.transform.rotation);
            PelletScript ps = p.GetComponent<PelletScript>();
            ps.moveDirection = pa.Bod.transform.forward;
            ps.SetDamage(damageLevel);
            curAmmo--;
            nextFire = Time.time + firerate;
            ncs.anim.SetTrigger("shoot");
        }
        else
        {
            //Reload();
        }
    }

    public float ShotVariance()
    {
        float r = Random.Range(-shotVarianceValue, shotVarianceValue);
        return r;
    }
}
