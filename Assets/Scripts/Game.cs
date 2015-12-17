using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public static Overseer overseer;
	public int startDay = 0;
    private int maxDays = 30;

    public GameObject UnitTest;
    private DynamicVariation variationscript;
    private bool debugging;

    // Village
    public static List<Project> projects = new List<Project>();

	private bool spaceKeyDown = false;
    private bool gameOver = false;

    private bool startQueue = false;
    private float queueTime = 0;
    private int queueIndex;
    private int queueInterval = 1;

    // Use this for initialization
    void Awake () 
	{
        variationscript = UnitTest.GetComponent<DynamicVariation>();
        debugging = false;
        
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
       
    }

    private void Start()
    {
        buildTown();

        if (debugging)
        {
            variationscript.SearchForBestBuildpath();
        }
    }

    public void StartJobqueue(int index)
    {
        startQueue = true;
        queueIndex = index;
    }

	public void NextDay()
	{
        if(!gameOver)
        {
            ShowBuilder();
            UseAllBuilder();
        
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
            Debug.Log("Game Over!");
            GameOver();
        }
    }

    private Project GetProjectByString(string targetProject)
    {
        foreach(Project p in projects)
        {
            if (p.projectName == targetProject)
            {
                return p;
            }
        }
        return null;
    }

    private void UseAllBuilder()
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

    private void ProcessQueue()
    {
        if(startQueue)
        {
            if(queueTime <= 0)
            {
                if (variationscript.jobqueue.Count > 0)
                {
                    queueTime = queueInterval;

                    string targetString = variationscript.jobqueue.Dequeue();

                    Project targetProject = GetProjectByString(targetString);
                    if (targetProject != null)
                    {
                        targetProject.BuyProject();
                    }

                    GameObject[] allBuilder = GameObject.FindGameObjectsWithTag("Builder");


                    if (allBuilder.Length == 0)
                    {
                        NextDay();
                    }
                    else
                    {
                        Debug.Log("Error! something went wrong.");
                    }
                    /*
                    Debug.Log("anzahl info: "+ variationscript.dayInfo.Count+ " index: "+ queueIndex);

                    LinkedListNode<Day> dayNode = variationscript.FindDay((variationscript.maxDays - overseer.day), variationscript.dayInfo[queueIndex]);

                    Debug.Log("Day: " + overseer.day + " points: " + overseer.points+" || " + dayNode.Value.points);
                    */
                }
                else
                {
                    if (overseer.points == variationscript._allPoints[queueIndex])
                    {
                        Debug.Log("Test successful");
                    }
                    startQueue = false;
                }
            }
            queueTime -= 1 * Time.deltaTime;
        }
    }

    private void BuildRandomeBuildpath()
    {
        GameReset();
        queueIndex = variationscript.GenerateNewBuildqueue();
        queueTime = 0;
        startQueue = true;
    }

    // Update is called once per frame
    void Update () 
	{
        ProcessQueue();

        if (Input.GetKeyDown("space"))
		{
			spaceKeyDown = true;
            if(debugging)
            {
                if (!startQueue)
                {
                    BuildRandomeBuildpath();
                }
            }
            else
            {
                GameObject[] allBuilder = GameObject.FindGameObjectsWithTag("Builder");
                if (allBuilder.Length == 0)
                {
                    NextDay();
                }
            }
        }

		if(Input.GetKeyUp("space"))
		{
			spaceKeyDown = false;
		}
    }

	private void buildTown()
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
        // Debug.Log("Game over was called!!");
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
