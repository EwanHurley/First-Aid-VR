using System;
using UnityEngine;



public class Camera : MonoBehaviour
{
    public float sensitivity;
    float xrotation;
    float yrotation;
    //public Transform orientation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yrotation += mouseX;
        xrotation -= mouseY;

        xrotation = Mathf.Clamp(xrotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xrotation,yrotation,0);
        //orientation.rotation = Quaternion.Euler(0,yrotation,0);
    }
}
