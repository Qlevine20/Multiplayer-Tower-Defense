using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBuildPlaces : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "buildPlace" || other.tag == "block")
        {
            Destroy(other.gameObject);
        }
    }
}
