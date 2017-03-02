using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    private bool camsDeactive = false;
    public GameObject SpawnManager;
    public void Update()
    {
        if (!camsDeactive)
        {
            if (SpawnManager != null)
            {
                GameObject[] sl = SpawnManager.GetComponent<Spawn>().playerList;
                if (sl.Length == 2)
                {
                    foreach (GameObject g in sl)
                    {
                        if (g != transform.parent.gameObject)
                        {
                            g.transform.GetChild(1).gameObject.SetActive(false);
                            g.transform.GetChild(3).gameObject.SetActive(false);
                            camsDeactive = true;
                            Debug.Log("deactivate");
                        }
                    }
                }
            }

        }
    }

}