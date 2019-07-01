using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour
{

    public RawImage compassImage;
    public Transform Player;
    public Text compassText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        compassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

        Vector3 forward = Player.transform.forward;

        forward.y = 0;

        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;

        headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));

        int displayAngle = Mathf.RoundToInt(headingAngle);
        
    }
}
