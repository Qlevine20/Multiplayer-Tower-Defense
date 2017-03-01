using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CamScript : NetworkBehaviour {


    private Camera cam;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        cam.gameObject.SetActive(false);
    }

    public override void OnStartLocalPlayer()
    {
        cam.gameObject.SetActive(true);
    }
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
