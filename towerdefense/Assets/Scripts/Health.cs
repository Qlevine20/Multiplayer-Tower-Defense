using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    // The TextMesh Component
    public const int maxHealth = 100;
    private int bars = 10;

	[SyncVar]
    private int currBars = 10;
    
    

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public TextMesh tm;

	public void Start()
	{
		tm.text = new string ('-', currBars);
	}

    public void TakeDamage()
    {
		if (!isServer)
			return;
		
        currentHealth -= 20;
    }

    void OnChangeHealth(int currentHealth)
	{   
		CmdOnChangeBars ();
    }

	[Command]
	void CmdOnChangeBars()
	{
		RpcOnChangeBars ();
	}

	[ClientRpc]
	void RpcOnChangeBars()
	{
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
				gameObject.GetComponent<Monster> ().Castle.GetComponent<PlayerScript> ().AddResources (2);
				NetworkServer.Destroy(gameObject);
			}
			else
			{
				gameObject.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
				GetComponent<PlayerScript>().lost = true;

			}
		}
	}

}
