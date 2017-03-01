using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Castle");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
