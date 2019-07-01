using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerMove pm;
    public Image ammoBar;

    

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMove>();
        //AmmoPercentage = (pm.curAmmo / pm.maxAmmo) * 100;
    }

    // Update is called once per frame
    void Update()
    {
        float percent = pm.curAmmo / 100;
        //Debug.Log(percent);
        ammoBar.fillAmount = percent;
        
    }
}
