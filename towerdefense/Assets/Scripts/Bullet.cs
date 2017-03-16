using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
    public float speed = 10;
    public Transform target;

    public bool slowUpgrade = false;
    public float slowChange = 0.4f;
    private float slowedSpeed = 0;

    void FixedUpdate() {
        //Flies toward target; if no target then bullet is destroyed

        if (!isServer)
        {
            return;
        }
        if (target) {
            Vector3 dir = target.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * speed;
        }
        else 
            NetworkServer.Destroy(gameObject);
       
    }
    
    void OnTriggerEnter(Collider co) {
        //Does damage to enemies
        if(co.tag == "Monster")
        {
            if (isServer)
            {
                Health health = co.GetComponentInChildren<Health>();
                if (health)
                {
                    health.ApplyDamage(3);
                    if (slowUpgrade)
                    {
                        if (slowedSpeed == 0)
                            slowedSpeed = co.GetComponent<UnityEngine.AI.NavMeshAgent>().speed * slowChange;
                        co.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = slowedSpeed;
                    }

                }
            }

            Destroy(gameObject);

        }

    }
}
