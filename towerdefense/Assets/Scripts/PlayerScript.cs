using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour{

    public GameObject Monster;
    public GameObject SpawnManager;
    private GameObject opponentCastle;
    private GameObject point;
    
	// Use this for initialization
	void Start () {
        point = transform.GetChild(2).gameObject;

        if(!isLocalPlayer)
        {
            return;
        }
        SpawnManager = (GameObject)Instantiate(SpawnManager, transform.position, Quaternion.identity);
        transform.GetChild(1).gameObject.GetComponent<CamScript>().SpawnManager = SpawnManager;
    }

    public void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (SpawnManager == null)
            {

                SpawnManager = (GameObject)Instantiate(SpawnManager, transform.position, Quaternion.identity);
            }
            if (SpawnManager != null)
            {
                if (opponentCastle == null)
                {
                    foreach (GameObject g in SpawnManager.GetComponent<Spawn>().playerList)
                    {
                        if (g != gameObject)
                        {
                            opponentCastle = g;
                        }
                    }
                }
                Debug.Log(opponentCastle);
                CmdSpawnMonster();
            }

        }
    }

    [Command]
    public void CmdSpawnMonster()
    {
        GameObject monster = (GameObject)Instantiate(Monster, point.transform.position, Quaternion.identity);
        monster.GetComponent<Monster>().Castle = opponentCastle;
        NetworkServer.Spawn(monster);
    }


    

    // Update is called once per frame

}
