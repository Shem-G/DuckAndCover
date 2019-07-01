using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public float MaxPlayerHP = 9999;
    public float CurPlayerHP = 9999;
    public int PlayerAmmo;

    public int GunType = 0;

    public int damageAmount = 100;

    // UI //

    public Image healthBar;
    public Text healthText;
    float fillAmount;
    public int sign;
    public float barTime = 0.5f;

    // END UI //

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = CurPlayerHP + " / " + MaxPlayerHP;
    }

    // Update is called once per frame
    void Update()
    {
        fillAmount = Mathf.Lerp(fillAmount, CurPlayerHP * 0.0001f, barTime);
        healthText.text = CurPlayerHP + " / " + MaxPlayerHP;
        healthBar.fillAmount = fillAmount;

        if (GunType >= 3)
        {
            GunType = 0;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (GunType < 3)
            {
                GunType++;
            }
            else
            {
                GunType = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(damageAmount);
        }

    }

    public void TakeDamage(int Damage)
    {
        CurPlayerHP -= Damage;
    }
}
