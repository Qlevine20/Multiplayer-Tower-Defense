using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Buildplace : NetworkBehaviour {
    // The Tower that should be built
    public GameObject towerPrefab;
    
    
    void OnMouseUpAsButton() {
        // Build Tower above Buildplace
        CmdSpawnTower();
    }

    [Command]
    public void CmdSpawnTower()
    {
        GameObject g = (GameObject)Instantiate(towerPrefab);
        g.transform.position = transform.position + Vector3.up;
        NetworkServer.Spawn(g);
    }

    
}
