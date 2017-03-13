using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControlPoint : NetworkBehaviour {
    public GameObject bluePoint;
    public GameObject redPoint;
    public Color red;
    public Color blue;
    public Color pointOwner;
    private float controlTime = 5f;
    private float blueCounter;
    private float redCounter;
    //public string owner = "neutral";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Monster")
        {
            
            if (other.gameObject.GetComponent<Monster>().mColor == blue)
            {
                blueCounter += Time.deltaTime;
                if (redCounter != 0)
                    redCounter -= Time.deltaTime;
                if(blueCounter >= controlTime && blueCounter >= redCounter)
                {
                    GetComponentInChildren<MeshRenderer>().sharedMaterial = bluePoint.GetComponentInChildren<MeshRenderer>().sharedMaterial;
                    pointOwner = blue;
                }
            }

            else if (other.gameObject.GetComponent<Monster>().mColor == red)
            {
                redCounter += Time.deltaTime;
                if (blueCounter != 0)
                    blueCounter -= Time.deltaTime;
                if(redCounter >= controlTime && redCounter >= blueCounter)
                {
                    GetComponentInChildren<MeshRenderer>().sharedMaterial = redPoint.GetComponentInChildren<MeshRenderer>().sharedMaterial;
                    pointOwner = red;
                }
            }


        }
    }
}
