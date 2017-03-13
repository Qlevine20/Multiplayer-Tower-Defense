using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Tower : NetworkBehaviour {
    private float counter;

    public GameObject bulletPrefab;
    public GameObject castle;
    public Transform targ;
	public AudioClip pewPew;
    public bool red = false;


    public float rotationSpeed = 35;
    public float reloadTime = .2f;
    public bool canShoot = false;
    public bool slowUpgrade = false;
    public bool rangeUpgrade = false;
    public int slowUpgradeCost = 5;
    public int rangeUpgradeCost = 5;
    public float rangeMultiplier = 2f;



    void Update() {

        if (!castle)
        {
            foreach (GameObject c in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (c.GetComponent<PlayerScript>().playerColor == Color.red && red)
                {
                    castle = c;
                    break;
                }
                else if (c.GetComponent<PlayerScript>().playerColor == Color.blue && !red)
                {
                    castle = c;
                    break;
                }
            }
        }
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
				AudioSource.PlayClipAtPoint (pewPew, transform.position, 5.0f);
				targ = co.transform;
				CmdShootMonster();
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


    public void UpgradeTowerSlow(PlayerScript p)
    {
        slowUpgrade = true;
        p.AddResources(-slowUpgradeCost);
    }

    public void UpgradeTowerRange(PlayerScript p)
    {
        rangeUpgrade = true;
        p.AddResources(-rangeUpgradeCost);
        gameObject.GetComponent<SphereCollider>().radius *= rangeMultiplier;
    }
}
