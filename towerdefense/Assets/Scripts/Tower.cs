using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Tower : NetworkBehaviour {
    // The Bullet
    public GameObject bulletPrefab;
    public GameObject castle;
    public Transform targ;
    private float counter;
    public float reloadTime = .2f;
    public bool canShoot = false;
    
    // Rotation Speed
    public float rotationSpeed = 35;
    
    void Update() {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
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
        // Was it a Monster? Then Shoot it
        if (co.gameObject.tag == "Monster") {
            Monster m = co.GetComponent<Monster>();
            if(m.Castle == castle && canShoot)
            {
                canShoot = false;
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
