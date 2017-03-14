using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour {

    // Use this for initialization
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "buildPlace")
        {
            other.tag = "block";
            other.GetComponent<Renderer>().material.color = Color.black;
        }
    }
}
