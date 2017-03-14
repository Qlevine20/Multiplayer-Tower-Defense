using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamScript : MonoBehaviour
{
    private Vector3 startPos;
    private float f;
    private Camera cam;
    private GameObject mapGen;
    private MapGenerator mg;

    float minFov  = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;
    Vector3 change;
    Vector3 oldChange;
    private int direction = 1;

    public void Start()
    {
        startPos = transform.position;
        cam = GetComponent<Camera>();
        f = cam.fieldOfView;
        mapGen = GameObject.Find("MapGen");
        change = oldChange = transform.position;
        

        //transform.parent = null;
    }
    public void Update()
    {
        if (!mapGen)
        {
            mapGen = GameObject.Find("MapGen");
        }
        else
        {
            if (!mg)
            {
                mg = mapGen.GetComponent<MapGenerator>();
            }
        }


        float c = Time.deltaTime * 10;

        if (cam.transform.position.z > 0)
        {
            direction = -direction;
            c = -c;
        }
        if (Input.GetKey(KeyCode.UpArrow) && Mathf.Abs(transform.position.z) > 2)
        {
            change = new Vector3(transform.position.x, transform.position.y, transform.position.z + c);
            
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.localPosition.x + 30 > -mg.width / 2)
        {
            change = new Vector3(transform.position.x - c, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.RightArrow) && transform.localPosition.x - 30 < (mg.width / 2))
        {
            change = new Vector3(transform.position.x + c, transform.position.y, transform.position.z);
        }


        if (Input.GetKey(KeyCode.DownArrow) && Mathf.Abs(transform.position.z) < Mathf.Abs(startPos.z) + 10)
        {
            change = new Vector3(transform.position.x, transform.position.y, transform.position.z - c);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            cam.fieldOfView = f;
            cam.transform.position = startPos;
            change = transform.position;
            oldChange = change;
        }
        if(change != oldChange)
        {
            transform.position = change;
            oldChange = change;
        }


        float fov = cam.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        cam.fieldOfView = fov;

        // set camera position
    }

}