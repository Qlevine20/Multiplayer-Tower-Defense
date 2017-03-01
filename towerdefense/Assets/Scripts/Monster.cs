using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Monster : MonoBehaviour {

    // Use this for initialization
    public GameObject Castle;
    void Start () {
        // Navigate to Castle
        if (Castle)
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = Castle.transform.position;
    }
    
    void OnTriggerEnter(Collider co) {
        // If castle then deal Damage, destroy self
        if (co.gameObject == Castle) {
            co.GetComponentInChildren<Health>().decrease();
            Destroy(gameObject);
        }
    }
}
