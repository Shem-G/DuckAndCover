using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public Text targetText;
    public Text distanceText;
    public ShipMove sm;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTargetText(string text)
    {
        if(text != null)
        {
            targetText.text = text;
        }
        else
        {
            targetText.text = "";
        }
    }

}
