using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static Overseer overseer;
	public int startDay = 0;
    private int maxDays = 30;

    public GameObject UnitTest;
    private UnitTest unitScript;

    // Village
    public static List<Project> projects = new List<Project>();

	private bool spaceKeyDown = false;
    private bool gameOver = false;

    private bool startQueue = false;
    private float queueTime = 0;
    private int queueInterval = 2;

    // Use this for initialization
    void Awake () 
	{
        unitScript = UnitTest.GetComponent<UnitTest>();
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

        buildTown();
    }

    public void StartJobqueue()
    {
        startQueue = true;
    }

	public void NextDay()
	{
        if(!gameOver)
        {
            ShowBuilder();
            overseer.day++;
            Debug.Log("Day:"+ overseer.day);
            overseer.Income();
            overseer.builder = overseer.maxBuilder;

            foreach (Project p in projects)
            {
                p.Construct();
            }
        }

        if (overseer.day == maxDays)
        {
            Debug.Log("GameOver!");
            GameOver();
        }
    }

    private Project GetProjectByString(string targetProject)
    {
        foreach(Project p in projects)
        {
            if(p.projectName == targetProject)
            {
                return p;
            }
        }
        return null;
    } 

    void UseAllBuilder()
    {
        foreach(Project p in projects)
        {
            if (overseer.builder > 0)
            {
                if (p.allBuilder.Count > 0)
                {
                    for (int i = 0; i < p.allBuilder.Count; i++)
                    {
                        p.UseBuilder(); 
                    }
                }
            }
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
        if(startQueue)
        {
            if (queueTime <= 0 && unitScript.jobqueue.Count > 0)
            {
                queueTime = queueInterval;
               
                string targetString = unitScript.jobqueue.Dequeue();
                
                Project targetProject = GetProjectByString(targetString);
                if(targetProject != null)
                {
                    targetProject.BuyProject();
                }

                GameObject[] allBuilder = GameObject.FindGameObjectsWithTag("Builder");

                UseAllBuilder();

                if (allBuilder.Length == 0)
                {
                    NextDay();
                }
                else
                {
                    Debug.Log("allBuilder: "+allBuilder.Length);
                }
            }
            else if(unitScript.jobqueue.Count <= 0)
            {
                Debug.Log("startQueue");
                startQueue = false;
            }

            queueTime -= 1 * Time.deltaTime;
        }

        if (Input.GetKeyDown("space"))
		{
			spaceKeyDown = true;
            //Debug.Log("pressed");
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
