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
        if (!isServer)
        {
            return;
        }

        currentHealth -= 20;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }


    }

    void OnChangeHealth(int currentHealth)
    {
            currBars--;
        if (currBars > 0)
        {
            tm.text = new string('-', currBars);
        }
        else if (gameObject.tag != "Player")
        {
            gameObject.GetComponent<Monster>().Castle.GetComponent<PlayerScript>().AddResources(10);
        }

            
    }
}
