using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
    // Speed
    public float speed = 10;
    
    // Target (set by Tower)
    public Transform target;    
    
    void FixedUpdate() {    
        // Still has a Target?
        if (target) {
            // Fly towards the target        
            Vector3 dir = target.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * speed;
        } else {
            // Otherwise destroy self
            NetworkServer.Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter(Collider co) {
        Health health = co.GetComponentInChildren<Health>();
        if (health) {
            health.TakeDamage();
            NetworkServer.Destroy(gameObject);
        }
    }
}
