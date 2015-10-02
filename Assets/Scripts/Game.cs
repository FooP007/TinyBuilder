using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static Overseer overseer;

	// Village
	private GameObject carpool;
	private GameObject street;

	private Carpool carpoolScript;
	private Street streetScript;

	private List<Project> projects = new List<Project>();

	private bool spaceKeyDown = false;

	// 
	private int day = 0;


	// Use this for initialization
	void Start () 
	{
		overseer = Overseer.Instance;

		overseer.coins = 1800;

		// find all projects
		street = GameObject.FindGameObjectWithTag("StreetPlaceholder");
		streetScript = street.GetComponent<Street>();

		carpool = GameObject.FindGameObjectWithTag("CarpoolPlaceholder");
		carpoolScript = carpool.GetComponent<Carpool>();
		carpoolScript.getAppendend(streetScript);


		// fill list
		projects.Add(carpoolScript);
		projects.Add(streetScript);

		buildTown();
	}

	void nextDay()
	{
		day++;
		foreach (Project p in projects)
		{
			p.Construct();
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("space"))
		{

			spaceKeyDown = true;

			nextDay();

		}

		if(Input.GetKeyUp("space"))
		{
			spaceKeyDown = false;
		}
	}

	void buildTown()
	{
		foreach (Project p in projects)
		{
			p.FillSpriteRenderer();
		}

	}
}
