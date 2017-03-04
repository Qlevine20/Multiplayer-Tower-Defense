using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamScript : MonoBehaviour
{
    private Vector3 startPos;
    private float f;
    private Camera cam;

    float minFov  = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;


    public void Start()
    {
        startPos = transform.position;
        cam = GetComponent<Camera>();
        f = cam.fieldOfView;

        //transform.parent = null;
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 change = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * 10);
            transform.position = change;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 change = new Vector3(transform.position.x - Time.deltaTime * 10, transform.position.y, transform.position.z);
            transform.position = change;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 change = new Vector3(transform.position.x + Time.deltaTime * 10, transform.position.y, transform.position.z);
            transform.position = change;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 change = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * 10);
            transform.position = change;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = startPos;
            cam.fieldOfView = f;
        }


        float fov = cam.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        cam.fieldOfView = fov;

        // set camera position
    }

}