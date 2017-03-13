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
    private GameObject selectedMonster;

    public GameObject BlueMonster;
    public GameObject RedMonster;
    public GameObject RedTower;
    public GameObject BlueTower;
    public GameObject SpawnManager;
    public MapGenerator mapGen;
    public GameObject[] paths;
    public Text resourcesText;
	public Text tooltip;
    public bool lost = false;
    public int resources = 15;
    private GameObject currMonster;

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

        if (!isLocalPlayer)
        {
            if (gameObject.GetComponent<Health>().GetHealth() == 0)
                winner = true;
            return;
        }

        if (lost)
            return;


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
                CmdSpawnMonster(playerColor);

            }
        }

        //If player left clicks & has enough resources, builds a tower and substracts resource cost
        if (Input.GetMouseButtonDown(0))
        {
                Ray ray;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Monster")
                    {
                        if (selectedMonster != null)
                        {
                            selectedMonster.GetComponent<Health>().tm.fontSize /= 4;
                        }   
                        selectedMonster = hit.collider.gameObject;
                        selectedMonster.GetComponent<Health>().tm.fontSize *= 4;
                    selectedMonster.GetComponent<Health>().tm.color = Color.cyan;
                    }
            

                    if (hit.collider.gameObject.tag == "ControlPoint")
                        selectedMonster.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.collider.gameObject.transform.position;
 
                    if (hit.collider.gameObject.tag == "buildPlace")
                    {
                        if(resources >= towerCost)
                        {
                            AddResources(-towerCost);
                            CmdSpawnTower(hit.transform.position + Vector3.up, playerColor);
                            hit.collider.gameObject.tag = "block";
                        }

                    }

                    if (hit.collider.gameObject.tag == "toolTip")
                    {
                        Tower t = hit.collider.gameObject.transform.parent.GetComponent<Tower>();
                        if (t.castle == this.gameObject)
                        {
                            if (!t.slowUpgrade && resources >= t.slowUpgradeCost)
                            {
                                if (isLocalPlayer)
                                {
                                     t.UpgradeTowerSlow(this);
                                    CmdChangeScale(t.gameObject, 1.5f);
                                }
                               
                                
                            }

                            else if (!t.rangeUpgrade && resources >= t.rangeUpgradeCost)
                            {

                                t.UpgradeTowerRange(this);
                                CmdChangeScale(t.gameObject,1.5f);
                                //t.CmdChangeLocalScale(1.5f);
                                

                            }
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
    public void CmdChangeScale(GameObject t, float s)
    {
        NetworkIdentity netId =  t.GetComponent<NetworkIdentity>();
        netId.AssignClientAuthority(connectionToClient);
        RpcChangeScale(t, s);
        netId.RemoveClientAuthority(connectionToClient);
    }

    [ClientRpc]
    public void RpcChangeScale(GameObject t, float s)
    {
        t.transform.localScale *= s;
    }

    [Command]
    public void CmdSpawnMonster(Color pColor)
    {
        
		RpcSpawnMonster (pColor);
    }

	[ClientRpc]
	public void RpcSpawnMonster(Color pColor) {
        if (isServer)
        {
            //Instantiates monster & sets castle
            
            GameObject monster = (pColor == Color.blue?(GameObject)Instantiate(BlueMonster, point.transform.position, Quaternion.identity) : (GameObject)Instantiate(RedMonster, point.transform.position, Quaternion.identity));
            Monster mon = monster.GetComponent<Monster>();
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
            mon.locPoint = paths[Random.Range(0, mapGen.pathsGenerated - 1)];
            NetworkServer.Spawn(monster);
        }

	}

    [Command]
    public void CmdSpawnTower(Vector3 pos, Color pColor)
    {
		RpcSpawnTower (pos, pColor, this.gameObject);
    }

	[ClientRpc]
	public void RpcSpawnTower(Vector3 pos, Color pColor, GameObject player) {
        if (!isServer)
        {
            return;
        }

        GameObject g = (pColor == Color.blue ? (GameObject)Instantiate(BlueTower, pos, Quaternion.identity) : (GameObject)Instantiate(RedTower, pos, Quaternion.identity));
        Tower t = g.GetComponent<Tower>();
        t.castle = player;

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
        if (other.tag == "Monster")
        {
            tooltip.transform.position = tpos;
            tooltip.text = "Monster\n" + other.GetComponent<Health>().GetHealth() + "/10\n";
        }
        else if (other.tag == "toolTip")
        {
            Debug.Log("HOVERIN OVER TOOLTIP");
            tooltip.transform.position = tpos;
            string upgrade = other.GetComponent<TowerFunctions>().GetUpgrade();
            tooltip.text = upgrade + " Tower\n";
            if (upgrade == "Basic")
                tooltip.text += "Upgrade to Slack (5)";
            if (upgrade == "Slack")
                tooltip.text += "Upgrade to Longshot (5)";
        }
        else if (other.tag == "Player")
        {
            tooltip.transform.position = tpos;
            tooltip.text = other.GetComponent<PlayerScript>().playerName + "\n" + other.GetComponent<Health>().GetHealth() + "/10";
        }
        else if (other.tag == "buildPlace")
        {
            tooltip.transform.position = tpos;
            tooltip.text = "Click to build a tower (" + towerCost + ")";
        }
        else if (other.tag == "ControlPoint")
        {
            tooltip.text = " CLICK TO SEND SELECTED MONSTER";
        }


        else
            tooltip.text = "";
	}

}
