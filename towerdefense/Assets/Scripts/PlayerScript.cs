using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour{

	[SyncVar]
	public string playerName;

	[SyncVar]
	public Color playerColor;

    private GameObject opponentCastle;
    private GameObject point;
    private int monsterCost = 3;
    private int towerCost = 5;
    private bool winner = false;
    private bool pathsFound = false;

    public GameObject Monster;
    public GameObject SpawnManager;
    public GameObject tower;
    public MapGenerator mapGen;
    public GameObject[] paths;
    public Text resourcesText;
	public Text tooltip;
    public bool lost = false;
    public int resources = 15;

	public Material redPalette;
	public GameObject model;

	void Start () {

        //Sets spawn point
		point = transform.GetChild(2).gameObject;

		if (playerColor == Color.red)
			model.GetComponent<MeshRenderer> ().material = redPalette;

        //Deactivates other player's camera/resource text
        if (!isLocalPlayer)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            return;
        }
        SpawnManager = (GameObject)Instantiate(SpawnManager, transform.position, Quaternion.identity);

        //Displays # resources and begins resource regeneration
        UpdateResourcesText();
        StartCoroutine(ResourceRegen());
    }

    public void Update()
    {
        //Checks for win/loss state
        if (lost)
            return;

        if (!isLocalPlayer)
        {
            if (gameObject.GetComponent<Health>().currentHealth == 0)
                winner = true;
            return;
        }
        if (winner)
            gameObject.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);


        //If player presses "S" & has enough resources, spawns monster and subtracts resource cost
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(resources >= monsterCost)
            {
                if (SpawnManager == null)
                    SpawnManager = (GameObject)Instantiate(SpawnManager, transform.position, Quaternion.identity);

                else if (opponentCastle == null)
                {
                    foreach (GameObject g in SpawnManager.GetComponent<Spawn>().playerList)
                    {
                        if (g != gameObject)
                            opponentCastle = g;
                    }
                }

                AddResources(-monsterCost);
                CmdSpawnMonster();

            }
        }

        //If player left clicks & has enough resources, builds a tower and substracts resource cost
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
                }
            }
        }

		// Check for Tooltip display
		Ray tooltipPointer = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit point;
		if (Physics.Raycast (tooltipPointer, out point)) {
			Renderer r = point.collider.GetComponent<Renderer> ();
			if (r != null) {
				TooltipController (point.collider.gameObject);
			} else
				tooltip.text = "";
		}

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "block" || other.tag == "buildPlace")
            Destroy(other.gameObject);
    }

    [Command]
    public void CmdSpawnMonster()
    {
		RpcSpawnMonster ();
    }

	[ClientRpc]
	public void RpcSpawnMonster() {
		//Instantiates monster & sets castle
		GameObject monster = (GameObject)Instantiate(Monster, point.transform.position, Quaternion.identity);
		Monster mon = monster.GetComponent<Monster>();
		mon.GetComponent<MeshRenderer> ().material.color = playerColor;
		mon.Castle = opponentCastle;

		//Finds mapGen & paths 
		if (!mapGen)
		{
			mapGen = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MapGenerator>();
		}
		if (!pathsFound)
		{
			pathsFound = true;
			paths = GameObject.FindGameObjectsWithTag("locPoint");
		}

		//Sets monsters travel location to locPoint
		mon.locPoint = paths[Random.Range(0, mapGen.pathsGenerated-1)];
		NetworkServer.Spawn(monster);
	}

    [Command]
    public void CmdSpawnTower(Vector3 pos)
    {
		RpcSpawnTower (pos);
    }

	[ClientRpc]
	public void RpcSpawnTower(Vector3 pos) {
		GameObject g = (GameObject)Instantiate(tower,pos,Quaternion.identity);
		g.GetComponent<MeshRenderer> ().material.color = playerColor;
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

	public void TooltipController(GameObject other) {
		Vector3 tpos = Input.mousePosition + new Vector3(0, 20, 0);
		if (other.tag == "Monster") {
			tooltip.transform.position = tpos;
			tooltip.text = "Monster\n" + other.GetComponent<Health> ().GetHealth () + "/10";
		} else if (other.tag == "Tower") {
			tooltip.transform.position = tpos;
			string upgrade = other.GetComponent<TowerFunctions> ().GetUpgrade();
			tooltip.text = upgrade + " Tower\n";
			if (upgrade == "Basic")
				tooltip.text += "Upgrade to Slack (5)";
			if (upgrade == "Slack")
				tooltip.text += "Upgrade to Longshot (5)";
        } else if (other.tag == "Player") {
			tooltip.transform.position = tpos;
			tooltip.text = other.GetComponent<PlayerScript>().playerName + "\n" + other.GetComponent<Health> ().GetHealth () + "/10";
		} else
			tooltip.text = "";
	}

}
