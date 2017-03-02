using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawn : NetworkBehaviour
{
    // The Monster that should be spawned
    public GameObject Monster;
    public GameObject[] playerList;
    private bool allPlayers = false;


    void Start()
    {

    }

    void Update()
    {

        if (!allPlayers)
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");
            if (playerList != null && playerList.Length == 2)
            {
                allPlayers = true;
                foreach (GameObject p in playerList)
                {
                    p.GetComponent<PlayerScript>().SpawnManager = gameObject;
                }
            }

        }
    }



}

