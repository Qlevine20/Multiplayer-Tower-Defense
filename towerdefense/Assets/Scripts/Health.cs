using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public const int maxHealth = 100;

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
        if (!isLocalPlayer)
        {
            return;
        }
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
            //If monster was killed, player gets resources for monster
			if (gameObject.tag != "Player")
			{
				gameObject.GetComponent<Monster> ().Castle.GetComponent<PlayerScript> ().AddResources (2);
				NetworkServer.Destroy(gameObject);
			}

            //If player was killed, lose text set to active
			else
			{
				gameObject.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
				GetComponent<PlayerScript>().lost = true;

			}
		}
	}

	public int GetHealth() {
		return currBars;
	}

}
