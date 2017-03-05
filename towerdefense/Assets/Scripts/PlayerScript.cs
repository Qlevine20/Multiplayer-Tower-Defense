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
    public MapGenerator mapGen;
    public GameObject[] paths;
    public Text resourcesText;
    private int monsterCost = 3;
    private int towerCost = 15;
    //public bool addResource;
    private bool winner = false;
    public bool lost = false;
    private bool pathsFound = false;


    public int resources = 15;
    
	// Use this for initialization
	void Start () {
        point = transform.GetChild(2).gameObject;
        UpdateResourcesText();
        if (!isLocalPlayer)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            return;
        }
        SpawnManager = (GameObject)Instantiate(SpawnManager, transform.position, Quaternion.identity);
		StartCoroutine (ResourceRegen());
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

        /*if (addResource)
        {
            Debug.Log("adding");
            addResource = false;
            AddResources(3);
        }*/

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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "block" || other.tag == "buildPlace")
        {
            Destroy(other.gameObject);
        }
    }

    [Command]
    public void CmdSpawnMonster()
    {

        GameObject monster = (GameObject)Instantiate(Monster, point.transform.position, Quaternion.identity);
        Monster mon = monster.GetComponent<Monster>();
        mon.Castle = opponentCastle;
        if (!mapGen)
        {
            mapGen = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MapGenerator>();
        }
        if (!pathsFound)
        {
            pathsFound = true;
            paths = GameObject.FindGameObjectsWithTag("locPoint");
        }
        Debug.Log(paths.Length);
        mon.locPoint = paths[Random.Range(0, mapGen.pathsGenerated-1)];
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
        resourcesText.text = "RESOURCES: " + resources.ToString();
    }

	public IEnumerator ResourceRegen() {
		yield return new WaitForSeconds (1);
		AddResources (1);
		StartCoroutine (ResourceRegen());
	}



    // Update is called once per frame

}
