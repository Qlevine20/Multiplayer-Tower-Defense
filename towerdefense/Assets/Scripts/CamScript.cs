using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CamScript : NetworkBehaviour {


    public Camera cam;
    // Use this for initialization
    void Start() {

        if (!isLocalPlayer)
        {
            cam.enabled = false;
            return;
        }
    }



    void Update()
    {

    }
}
