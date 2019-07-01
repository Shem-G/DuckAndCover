using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    private Vector3 FinalPos;
    private Vector3 SmoothedPos;
    public float Damping = 10f;

    private Vector3 offset;
    private Vector3 lookDir;

    float distance;
    public float cameraDistance = 5.0f;
    public float yPos = 2f;

    Vector3 playerPrevPos, playerMoveDir, finalPos;


    void FixedUpdate()
    {
        finalPos = (Target.transform.position - Target.transform.forward * cameraDistance);
        SmoothedPos = Vector3.Lerp(transform.position, finalPos, Damping * Time.deltaTime);
        SmoothedPos.y = yPos;

        lookDir = Target.transform.position;
        lookDir.y = 0;
        transform.LookAt(lookDir);
        Quaternion rot = transform.rotation;
        rot.x = 0;
        rot.z = 0;
        //rot.w = 0;
        transform.rotation = rot;
        transform.position = SmoothedPos;

    }
}