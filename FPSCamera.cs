using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{

    //Declaring Variables
    public Transform player;
    public Transform gun;
    float pitchChange;
    float yawChange;
    float pitch;
    float yaw;
    float verticalCameraTransform = 0f;
    float sensitivity= 1f;

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Input.GetAxis("Mouse Y") * sensitivity;
        //Input.GetAxis("Mouse X") * sensitivity;
/*
        //Mouse Input
        pitchChange = Input.GetAxis("Mouse Y") * sensitivity;
        yawChange = Input.GetAxis("Mouse X") * sensitivity;

        verticalCameraTransform -= pitchChange;
        verticalCameraTransform = Mathf.Clamp(verticalCameraTransform, -90f, 90f);
        transform.localEulerAngles = Vector3.right * verticalCameraTransform;    

        player.Rotate(Vector3.up * yawChange);
        //gun.Rotate(Vector3.up * -pitchChange);  
*/
    }
}
