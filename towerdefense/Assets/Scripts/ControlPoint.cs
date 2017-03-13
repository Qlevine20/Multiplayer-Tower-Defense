using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControlPoint : NetworkBehaviour
{
    public GameObject bluePoint;
    public GameObject redPoint;
    public Color red;
    public Color blue;
    public Color pointOwner = Color.black;

    private float controlTime = 5f;
    private float blueCounter;
    private float redCounter;

    private GameObject[] players;
    //public string owner = "neutral";

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (players == null)
            players = GameObject.FindGameObjectsWithTag("Player");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Monster")
        {
            if (other.gameObject.GetComponent<Monster>().mColor == blue)
            {
                blueCounter += Time.deltaTime;
                redCounter = 0;
                if (pointOwner == Color.black || pointOwner == red)
                {
                    if (blueCounter >= controlTime && blueCounter >= redCounter)
                    {
                        GetComponentInChildren<MeshRenderer>().sharedMaterial = bluePoint.GetComponentInChildren<MeshRenderer>().sharedMaterial;
                        foreach (GameObject player in players)
                        {
                            if (player.GetComponent<PlayerScript>().playerColor == Color.blue)
                                player.GetComponent<PlayerScript>().capturedPoints += 1;
                            else if (player.GetComponent<PlayerScript>().playerColor == Color.red && pointOwner == red)
                                player.GetComponent<PlayerScript>().capturedPoints -= 1;
                        }

                        pointOwner = blue;
                    }
                }
            }

            else if (other.gameObject.GetComponent<Monster>().mColor == red)
            {
                redCounter += Time.deltaTime;
                blueCounter = 0;
                if (pointOwner == Color.black || pointOwner == blue)
                {
                    if (redCounter >= controlTime && redCounter >= blueCounter)
                    {
                        GetComponentInChildren<MeshRenderer>().sharedMaterial = redPoint.GetComponentInChildren<MeshRenderer>().sharedMaterial;
                        foreach (GameObject player in players)
                        {
                            if (player.GetComponent<PlayerScript>().playerColor == Color.blue)
                                player.GetComponent<PlayerScript>().capturedPoints -= 1;
                            else if (player.GetComponent<PlayerScript>().playerColor == Color.red && pointOwner == blue)
                                player.GetComponent<PlayerScript>().capturedPoints += 1;
                        }

                        pointOwner = red;
                    }

                }


            }
        }
    }
}
