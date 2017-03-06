using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
    public float speed = 10;
    public Transform target;    
    
    void FixedUpdate() {    
        //Flies toward target; if no target then bullet is destroyed
        if (target) {
            Vector3 dir = target.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * speed;
        }
        else 
            NetworkServer.Destroy(gameObject);
       
    }
    
    void OnTriggerEnter(Collider co) {
        //Does damage to enemies
        Health health = co.GetComponentInChildren<Health>();
        if (health)
        {
            health.TakeDamage();
            NetworkServer.Destroy(gameObject);
        }
    }
}
