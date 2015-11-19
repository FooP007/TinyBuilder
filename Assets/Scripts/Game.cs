using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static Overseer overseer;
    public Overseer _overseer;
	public int startDay = 0;
    public int maxDays = 10;

    public GameObject UnitTest;
    private UnitTest unitScript;

    // Village
    public static List<Project> projects = new List<Project>();

	private bool spaceKeyDown = false;
    private bool gameOver = false;

	// Use this for initialization
	void Awake () 
	{
        unitScript = UnitTest.GetComponent<UnitTest>();
        overseer = Overseer.Instance;
        _overseer = overseer;

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

        buildTown();
       
    }

	public void NextDay()
	{
        if(!gameOver)
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

        if (overseer.day == maxDays)
        {
            GameOver();
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

    public void GameOver()
    {
        gameOver = true;
        overseer.day++;
        //Debug.Log("Game over was called!!");
    }

    public void GameReset()
    {
        overseer.coins = 15;
        overseer.points = 0;
        overseer.citizen = 5;
        overseer.capacity = 5;
        overseer.discount = 0;
        overseer.environmentPoints = 0;
        overseer.day = startDay;

        foreach (Project p in projects)
        {
            if(p is Whitehouse)
            {
                p.projectLevel = 1;
            }
            else
            {
                p.projectLevel = 0;
            }
            
            p.constructionDays = 0;
            p.clockText.text = "0";
            p.constructing = false;
        }

        buildTown();
        gameOver = false;
    }

    public static void UpdateUpgradeWindows()
    {
        GameObject[] upgradeWindows = GameObject.FindGameObjectsWithTag("UpgradeWindow");
        
        foreach (GameObject uw in upgradeWindows)
        {
            UpgradeWindow uwScript = uw.GetComponent<UpgradeWindow>();
            uwScript.UpdateText();
        }
    }
}
