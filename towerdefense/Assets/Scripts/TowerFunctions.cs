using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFunctions : MonoBehaviour {

	public GameObject tower;

	public string GetUpgrade() {
		return tower.GetComponent<Tower> ().GetUpgrade ();
	}
}
