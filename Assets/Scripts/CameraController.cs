using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform followTarget;

    [SerializeField] float distance = 3;
    [SerializeField] float maxVertAngle = 45;
    [SerializeField] float minVertAngle = -45;
    [SerializeField] Vector2 framingOffset;
    [SerializeField] float rotationSpeed = 3f;


    // serialized field so its exposed in the inspector
    float rotationY;
    float rotationX;

    private void Start()
    {

    }
    private void Update()

    {


        rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVertAngle, maxVertAngle);

        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;
        //control rotation speed from the inspector
        //NOTE, for horiz. rotation we rotate around Y, for vertical, we rotate around X
        var playerRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
        //quaternion achieves rotation in the y and X (for horizontal and vertical rotation) when mouse is moved
        transform.position = focusPosition - playerRotation * new Vector3(0, -1, distance);
        transform.rotation = playerRotation;
        //set the rotation as the rotation of the camera



    }




    public Quaternion HorizontalRotation => Quaternion.Euler(0, rotationY, 0);
    // property to prevent vertical movement
}
