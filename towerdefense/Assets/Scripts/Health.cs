using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    // The TextMesh Component
    public const int maxHealth = 100;
    public int bars = 5;
    private int currBars = 5;
    
    

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public TextMesh tm;


    public void TakeDamage()
    {
        currentHealth -= 20;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (gameObject.tag != "Player")
            {
                NetworkServer.Destroy(gameObject);
            }
            else
            {
                gameObject.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                GetComponent<PlayerScript>().lost = true;

            }



        }


    }

    void OnChangeHealth(int currentHealth)
    {
        Debug.Log("Change health");
        currBars--;
        if (currBars > 0)
        {
            tm.text = new string('-', currBars);
        }
        else
        {
            if (gameObject.tag != "Player")
            {

                Debug.Log("add");
                gameObject.GetComponent<Monster>().Castle.GetComponent<PlayerScript>().addResource = true;
            }
        }

    }

}
