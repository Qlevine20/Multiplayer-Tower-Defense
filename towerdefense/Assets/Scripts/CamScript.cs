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
        Vector3 change = transform.position;
        float c = Time.deltaTime * 10;

        if (cam.transform.position.z > 0)
        {
            c = -c;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            change = new Vector3(transform.position.x, transform.position.y, transform.position.z + c);
            
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            change = new Vector3(transform.position.x - c, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            change = new Vector3(transform.position.x + c, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            change = new Vector3(transform.position.x, transform.position.y, transform.position.z - c);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            cam.fieldOfView = f;
        }
        transform.position = change;

        float fov = cam.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        cam.fieldOfView = fov;

        // set camera position
    }

}