using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public const int maxBars = 10;

	[SyncVar(hook = "TakeDamage")]
    private int currBars = maxBars;

    public TextMesh tm;

    void Start()
    {
        tm.text = new string('-', currBars);
    }

    public void ApplyDamage(int amount)
    {
        CmdOnChangeBars(amount);
    }

    public void TakeDamage(int newBars)
    {
        currBars = newBars;
        tm.text = new string('-', currBars);

        if (currBars <= 0)
        {
            currBars = 0;
            Destroy(gameObject);
            //You lose code;
        }
    }

    [Command]
    void CmdOnChangeBars(int change)
    {
        currBars -= change;
        tm.text = new string('-', currBars);
    }


	//[Command]
	//void CmdOnChangeBars()
	//{
	//	RpcOnChangeBars ();
	//}

	//[ClientRpc]
	//void RpcOnChangeBars()
	//{
 //       if (!isServer)
 //       {
 //           return;
 //       }
	//	currBars--;
	//	if (currBars > 0)
	//	{
	//		tm.text = new string('-', currBars);
	//	}

	//	else
	//	{
 //           //If monster was killed, player gets resources for monster
	//		if (gameObject.tag != "Player")
	//		{
	//			gameObject.GetComponent<Monster> ().Castle.GetComponent<PlayerScript> ().AddResources (2);
	//			NetworkServer.Destroy(gameObject);
	//		}

 //           //If player was killed, lose text set to active
	//		else
	//		{
	//			gameObject.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
	//			GetComponent<PlayerScript>().lost = true;

	//		}
	//	}
	//}

	public int GetHealth() {
		return currBars;
	}

}
