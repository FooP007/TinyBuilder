using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static Overseer overseer;
	public int startDay = 0;

	// Village
	public static List<Project> projects = new List<Project>();

	private bool spaceKeyDown = false;

	// Use this for initialization
	void Start () 
	{
		overseer = Overseer.Instance;

		overseer.coins = 15 + 1000;
		overseer.points = 0+ 20;
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

		buildTown();
	}

	void NextDay()
	{

        ShowBuilder();
        overseer.day++;
        overseer.Income();
        overseer.builder = overseer.maxBuilder;

        foreach (Project p in projects)
        {
            p.Construct();
        }
    }

    void ShowBuilder()
    {
        // check if the player can use builder
        if (overseer.builder > 0)
        {

            foreach (Project p in projects)
            {
                if (p.constructing)
                {
                    int length;

                    if (p.constructionDays < overseer.builder)
                    {
                        length = (p.constructionDays - 1);
                    }
                    else
                    {
                        length = overseer.builder;
                    }

                    for (int i = 0; i < length; i++)
                    {
                        p.AddBuilder(i);
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("space"))
		{
			spaceKeyDown = true;
            GameObject[] allBuilder = GameObject.FindGameObjectsWithTag("Builder");
            if (allBuilder.Length == 0)
            {
                NextDay();
            }
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

    public static void UpdateUpgradeWindows()
    {
        GameObject[] upgradeWindows = GameObject.FindGameObjectsWithTag("UpgradeWindow");
        Debug.Log("updated upgrade windows!!!!");
        foreach (GameObject uw in upgradeWindows)
        {
            UpgradeWindow uwScript = uw.GetComponent<UpgradeWindow>();
            uwScript.UpdateText();
        }
    }
}
