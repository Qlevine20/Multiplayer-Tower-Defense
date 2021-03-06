﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Monster : NetworkBehaviour {
    public GameObject Castle;
    public Vector3 locPoint;


    public PlayerScript.CPoint control;

    [SyncVar]
    public bool active = false;

    public GameObject cp;
    public Color mColor;
    public int Damage = 5;
	public AudioClip deathS;
	public AudioClip sockemS;


    void Start()
    {

        if (Castle == null)
        {
            GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
            float farthestCast = (playerList[0].transform.position - transform.position).sqrMagnitude;
            Castle = playerList[0];

            if ((playerList[1].transform.position - transform.position).sqrMagnitude > farthestCast)
                Castle = playerList[1];
        }
        cp.SetActive(active);
        transform.GetChild(2).GetComponent<MeshRenderer>().material.color = mColor;

        //Assigns proper castle to monster


        //Sets the monster's destination to assigned location point
        if (locPoint != null)
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = locPoint;

        if(control != PlayerScript.CPoint.none)
        {
            GameObject[] g = GameObject.FindGameObjectsWithTag("ControlPoint");
            if (control == PlayerScript.CPoint.left)
            {

                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = g[1].transform.position;
            }

            if(control == PlayerScript.CPoint.right)
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = g[0].transform.position;
            }
        }

        GetComponent<UnityEngine.AI.NavMeshAgent>().avoidancePriority = 0;
    }


    void OnTriggerEnter(Collider co) {
        //Monsters do damage to castle and disappear
        if (co.gameObject == Castle) {
			AudioSource.PlayClipAtPoint (sockemS, transform.position, 7.0f);
            Health h = co.GetComponentInChildren<Health>();
            h.TakeDamage(h.currBars - 1);
            Destroy(gameObject);
        }
        if (co.gameObject.tag == "Monster")
		{
            if (co.GetComponent<Monster>().Castle != Castle)
			{
				AudioSource.PlayClipAtPoint (deathS, transform.position, 5.0f);
                Destroy(gameObject);
            }
        }

        //When location point is reached, monsters new destination is set to castle
        if (co.gameObject.tag == "locPoint")
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = Castle.transform.position;
    }

    void OnTriggerStay(Collider co){
        //Attacks monsters that belong to opposite team
        if (co.gameObject.tag == "Monster")
        {
            Monster m = co.gameObject.GetComponent<Monster>();
            if (m.Castle != Castle)
            {
                co.GetComponent<Health>().TakeDamage(1);
            }
        }

        if(co.gameObject.tag == "ControlPoint")
        {
            if(co.gameObject.GetComponent<ControlPoint>().pointOwner == mColor)
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = Castle.transform.position;
                cp.SetActive(false);
            }
                

        }
    }
}
