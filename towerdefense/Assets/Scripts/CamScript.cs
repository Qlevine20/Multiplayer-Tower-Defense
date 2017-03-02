using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CamScript : NetworkBehaviour
{
    private bool camsDeactive = false;
    public GameObject SpawnManager;
    public void Update()
    {
        if (!camsDeactive)
        {
            Debug.Log("Cams");
            if (SpawnManager != null)
            {
                Debug.Log("spawnManager not null");
                GameObject[] sl = SpawnManager.GetComponent<Spawn>().playerList;
                if (sl.Length == 2)
                {
                    Debug.Log("sl = 2");
                    foreach (GameObject g in sl)
                    {
                        if (g.transform != transform.parent)
                        {
                            Debug.Log("deactive");
                            g.transform.GetChild(1).gameObject.SetActive(false);
                            camsDeactive = true;
                        }
                    }
                }
            }

        }
    }
}