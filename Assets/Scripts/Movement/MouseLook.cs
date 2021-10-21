using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class MouseLook : MonoBehaviour
{



    float Speed = 500f;



    public Transform playerBody;



    float xRotation = 0f;



    // Use this for initialization

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

    }



    // Update is called once per frame

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * Speed * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * Speed * Time.deltaTime;



        playerBody.Rotate(Vector3.up * mouseX);



        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    }

}