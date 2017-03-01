using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawn : NetworkBehaviour
{
    // The Monster that should be spawned
    public GameObject Monster;
    private GameObject opponentCastle;


    void Start()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.transform != transform.parent)
            {
                opponentCastle = g;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject monster = Instantiate(Monster, transform.position, Quaternion.identity) as GameObject;
            monster.GetComponent<Monster>().Castle = opponentCastle;
            NetworkServer.Spawn(monster);
        }
    }
}

