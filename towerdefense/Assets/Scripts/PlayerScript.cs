using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour{

    public GameObject Monster;
    public GameObject SpawnManager;
    private GameObject opponentCastle;
    private GameObject point;
    public GameObject tower;
    public Text resourcesText;
    public int monsterCost = 1;
    public int towerCost = 5;
    public bool addResource;
    private bool winner = false;
    public bool lost = false;


    public int resources = 10;
    
	// Use this for initialization
	void Start () {
        point = transform.GetChild(2).gameObject;
        UpdateResourcesText();
        if(!isLocalPlayer)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            return;
        }
        SpawnManager = (GameObject)Instantiate(SpawnManager, transform.position, Quaternion.identity);
    }

    public void Update()
    {
        if (lost)
        {
            return;
        }
        if (!isLocalPlayer)
        {
            if (gameObject.GetComponent<Health>().currentHealth == 0)
            {
                winner = true;
            }
            return;
        }
        if (winner)
        {
            gameObject.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
        }

        if (addResource)
        {
            Debug.Log("adding");
            addResource = false;
            AddResources(10);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(resources >= monsterCost)
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
                    AddResources(-monsterCost);
                    CmdSpawnMonster();
                }
            }

        }


        if (Input.GetMouseButtonDown(0))
        {
            if (resources >= towerCost)
            {
                Ray ray;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Renderer r = hit.collider.GetComponent<Renderer>();
                    if (r != null && hit.collider.gameObject.tag == "buildPlace")
                    {
                        AddResources(-towerCost);
                        CmdSpawnTower(hit.transform.position + Vector3.up);
                    }

                    //Debug.Log(hit);
                }

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

    [Command]
    public void CmdSpawnTower(Vector3 pos)
    {

        GameObject g = (GameObject)Instantiate(tower,pos,Quaternion.identity);
        g.GetComponent<Tower>().castle = this.gameObject;
        NetworkServer.Spawn(g);
    }

    public void AddResources(int resource)
    {
            resources += resource;
            UpdateResourcesText();

        
    }

    public void UpdateResourcesText()
    {
        resourcesText.text = "Resources: " + resources.ToString();
    }




    // Update is called once per frame

}
