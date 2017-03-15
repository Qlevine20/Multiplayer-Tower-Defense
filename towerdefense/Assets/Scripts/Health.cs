using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class Health : NetworkBehaviour {
    public const int maxBars = 10;
    
	[SyncVar(hook = "TakeDamage")]
    public int currBars = maxBars;


    private bool GameOver = false;

    private GameObject[] players;
    private GameObject otherPlayer;
    private Health otherPlayerHealth;


    public TextMesh tm;

    void Start()
    {
        tm.text = new string('-', currBars);
        if(tag == "Player")
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject g in players)
            {
                if (g != this.gameObject)
                {
                    otherPlayer = g;
                    otherPlayerHealth = otherPlayer.GetComponent<Health>();
                }
            }
        }


    }



    public void Update()
    {
        if(players != null && players.Length != 2 && !GameOver)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length == 2)
            {
                foreach(GameObject g in players)
                {
                    if(g != this.gameObject)
                    {
                        otherPlayer = g;
                        otherPlayerHealth = otherPlayer.GetComponent<Health>();
                    }
                }
            }
        }
        if (tag == "Player" && otherPlayer != null)
        {
            if (currBars <= 0)
            {
                GameOver = true;
                gameObject.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                transform.GetComponent<PlayerScript>().lost = true;
                StartCoroutine(GameEnd());
            }
            else if (otherPlayerHealth.GetHealth() <= 0)
            {
                GameOver = true;
                gameObject.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
            }

        }

    }

    public void ApplyDamage(int amount)
    {
        CmdOnChangeBars(amount);
    }


    public void TakeDamage(int newBars)
    {


        if (currBars <= 0)
        {
            currBars = 0;
            if(tag != "Player")
            {
                Destroy(gameObject);
            }
            return;
            
            //You lose code;
        }

        currBars = newBars;
        if(currBars >= 0)
        {
            tm.text = new string('-', currBars);
        }
        
    }

    [Command]
    void CmdOnChangeBars(int change)
    {
        currBars -= change;
        if(currBars >= 0)
        {
            tm.text = new string('-', currBars);
        }
    }


    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(3);
        LobbyManager lm = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        if (isServer)
        {
            lm.StopHost();
            lm.ChangeTo(lm.mainMenuPanel);
        }
        else
        {
            lm.StopClient();
            lm.ChangeTo(lm.mainMenuPanel);
        }
        
    }


	public int GetHealth() {
		return currBars;
	}

}
