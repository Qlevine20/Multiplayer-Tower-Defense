using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMeshBuild : MonoBehaviour {

    public NavMeshSurface nav;
    public MapGenerator mapGen;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
        if (mapGen.generated)
        {
            mapGen.generated = false;
            nav.Bake();
        }
	}
}
