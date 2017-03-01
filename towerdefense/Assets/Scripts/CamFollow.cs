using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player.transform.position.z > 0)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 20, player.transform.position.z + 50);
        else
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 20, player.transform.position.z - 50);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
