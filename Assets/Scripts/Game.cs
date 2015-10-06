using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static Overseer overseer;
	public int startDay = 0;

	// Village
	private GameObject carpool;
	private GameObject street;
	private GameObject whitehouse;

	private Carpool carpoolScript;
	private Street streetScript;
	private Whitehouse whitehouseScript;

	private List<Project> projects = new List<Project>();

	private bool spaceKeyDown = false;

	// 



	// Use this for initialization
	void Start () 
	{
		overseer = Overseer.Instance;

		overseer.coins = 15;
		overseer.points = 0;
		overseer.citizen = 5;
		overseer.capacity = 5;
		overseer.environmentPoints = 0;
		overseer.day = startDay;


		GameObject[] transitionProjects = GameObject.FindGameObjectsWithTag("Project");

		foreach (GameObject p  in transitionProjects)
		{
			Project script = p.GetComponent<Project>();
			projects.Add(script);
		}

		/* find all projects
		street = GameObject.FindGameObjectWithTag("StreetPlaceholder");
		streetScript = street.GetComponent<Street>();

		carpool = GameObject.FindGameObjectWithTag("CarpoolPlaceholder");
		carpoolScript = carpool.GetComponent<Carpool>();
		carpoolScript.getAppendend(streetScript);

		whitehouse = GameObject.FindGameObjectWithTag("WhitehousePlaceholder");
		whitehouseScript = whitehouse.GetComponent<Whitehouse>();

		// fill list
		projects.Add(carpoolScript);
		projects.Add(streetScript);
		projects.Add(whitehouseScript);
		*/
		buildTown();
	}

	void nextDay()
	{
		overseer.day++;
		overseer.Income();
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
			p.Initiate();
		}

	}
}
