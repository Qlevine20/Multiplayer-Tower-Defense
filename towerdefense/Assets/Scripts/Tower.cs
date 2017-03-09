using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Tower : NetworkBehaviour {
    private float counter;

    public GameObject bulletPrefab;
    public GameObject castle;
    public Transform targ;
	public AudioClip pewPew;

    public Color tColor;
    public float rotationSpeed = 35;
    public float reloadTime = .2f;
    public bool canShoot = false;
    public bool slowUpgrade = false;
    public bool rangeUpgrade = false;
    public int slowUpgradeCost = 5;
    public int rangeUpgradeCost = 5;
    public float rangeMultiplier = 2f;

    void Start()
    {
            GetComponent<MeshRenderer>().material.color = tColor;

    }

    void Update() {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);

        //Updates timer until tower can shoot
        if (!canShoot)
        {
            counter += Time.deltaTime;
            if (counter >= reloadTime)
            {
                canShoot = true;
                counter = 0;
            }
        }

    }
    
    void OnTriggerStay(Collider co) {
        //If enemy monster enters tower's range, tower shoots 
        if (co.gameObject.tag == "Monster")
        {
			Monster m = co.GetComponent<Monster>();
            if(m.Castle == castle && canShoot)
            {
				canShoot = false;
				AudioSource.PlayClipAtPoint (pewPew, transform.position, 2.0f);
				targ = co.transform;
				CmdShootMonster();
            }

        }
    }

    void OnMouseDown()
    {
        if (castle)
        {
            if (!slowUpgrade)
            {
                if (castle.GetComponent<PlayerScript>().resources >= slowUpgradeCost)
                {
                    slowUpgrade = true;
                    castle.GetComponent<PlayerScript>().AddResources(-slowUpgradeCost);
                }
            }

            else if (!rangeUpgrade)
            {
                if (castle.GetComponent<PlayerScript>().resources >= rangeUpgradeCost)
                {
                    rangeUpgrade = true;
                    castle.GetComponent<PlayerScript>().AddResources(-rangeUpgradeCost);
                    gameObject.GetComponent<SphereCollider>().radius *= rangeMultiplier;
                }
            }
        }

    }

    [Command]
    public void CmdShootMonster()
    {
        GameObject g = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        g.GetComponent<Bullet>().target = targ;
        if (slowUpgrade)
            g.GetComponent<Bullet>().slowUpgrade = true;
        NetworkServer.Spawn(g);
    }

	public string GetUpgrade() {
		if (!slowUpgrade)
			return "Basic";
		else if (!rangeUpgrade)
			return "Slack";
		else
			return "Longshot";
	}
}
