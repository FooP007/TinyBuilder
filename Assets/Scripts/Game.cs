using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	// Village
	private GameObject carpool;

	private Carpool carpoolScript;
	private int carpoolLevel = 0;

	private bool spaceKeyDown = false;

	// 
	private int day = 0;

	// Use this for initialization
	void Start () 
	{
		carpool = GameObject.FindGameObjectWithTag("CarpoolPlaceholder");
		carpoolScript = carpool.GetComponent<Carpool>();

		buildTown();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("space"))
		{

			spaceKeyDown = true;

			carpoolLevel++;
			Debug.Log ("carpool capacity: " + carpoolScript.Capacity());
			carpoolScript.FillSpriteRenderer();
		}

		if(Input.GetKeyUp("space")  )
		{
			spaceKeyDown = false;
		}
	}

	void buildTown()
	{

		carpoolScript.FillSpriteRenderer();

	}
}
