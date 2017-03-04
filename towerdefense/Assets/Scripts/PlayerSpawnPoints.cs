using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpawnPoints : MonoBehaviour {

    private int height;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject mapGen;
    public GameObject BuildPiece;

	// Use this for initialization
	void Awake () {
        height = mapGen.GetComponent<MapGenerator>().height;
        spawnPoint1.transform.position = new Vector3(0, spawnPoint1.transform.position.y, -((height / 2) * BuildPiece.transform.localScale.x) + BuildPiece.transform.localScale.x * 3);
        spawnPoint2.transform.position = new Vector3(0, spawnPoint2.transform.position.y, ((height / 2) * BuildPiece.transform.localScale.x) - BuildPiece.transform.localScale.x * 3);
    }


    void Start()
    {
        
    }
	// Update is called once per frame
	void Update () {
		
	}
}
