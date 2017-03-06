using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Tower : NetworkBehaviour {
    private float counter;

    public GameObject bulletPrefab;
    public GameObject castle;
    public Transform targ;
	public AudioClip pewPew;

    public float rotationSpeed = 35;
    public float reloadTime = .2f;
    public bool canShoot = false;

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

    [Command]
    public void CmdShootMonster()
    {
        GameObject g = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        g.GetComponent<Bullet>().target = targ;
        NetworkServer.Spawn(g);
    }
}
