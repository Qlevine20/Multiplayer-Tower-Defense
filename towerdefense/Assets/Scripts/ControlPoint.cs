using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControlPoint : NetworkBehaviour {
    public GameObject bluePoint;
    public GameObject redPoint;
    public Color red;
    public Color blue;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            
            if (other.gameObject.GetComponent<Monster>().mColor == blue)
                GetComponentInChildren<MeshRenderer>().sharedMaterial = bluePoint.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            else if (other.gameObject.GetComponent<Monster>().mColor == red)
                GetComponentInChildren<MeshRenderer>().sharedMaterial = redPoint.GetComponentInChildren<MeshRenderer>().sharedMaterial;

        }
    }
}
