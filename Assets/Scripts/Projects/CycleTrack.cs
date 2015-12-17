 using UnityEngine;
using System.Collections;

public class CycleTrack : Project 
{
    public static string staticProjectName = "Cycletrack";

    public static int[] extraPointsList = new int[5] {2,4,8,14,22};
    //public Whitehouse whitehouse;

    public static int[] Costs = new int[5] { 15, 20, 30, 45, 65 };
    public static int[] Capacities = new int[5] { 1, 3, 7, 14, 25 };
    public static int[] BuildingRounds = new int[5] { 1, 2, 3, 4, 5 };
    public static int[] RequiredPoints = new int[1];
    public static int[] RequiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };

    void Awake()
	{
		project = GameObject.FindGameObjectWithTag("CycleTrack");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();
        projectName = staticProjectName;
        
        clockText.text = "0";

        costs = new int[5] { 15, 20, 30, 45, 65 };
        capacities = new int[5] { 1, 3, 7, 14, 25 };
        buildingRounds = new int[5] { 1, 2, 3, 4, 5 };
        requiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };
    }
	
	void Start()
	{
        UpdateText(projectLevel, new string[2] { " capacity", " envir pts" }, new int[2][] { capacities, extraPointsList }, "Whitehouse level: ", requiredWhitehouse);
    }
	
	protected override void Upgrade()
	{
		Game.overseer.capacity += Capacity();
		Game.overseer.environmentPoints += extraPointsList[projectLevel];

        UpdateText(projectLevel + 1, new string[2] { " capacity", " envir pts" }, new int[2][] { capacities, extraPointsList }, "Whitehouse level: ", requiredWhitehouse);
    }
	
	public override bool MetRequirements()
	{
		if(dependence.projectLevel >= Whitehouse())
		{
			return true;
		}
		else
		{
			//Debug.Log ("Upgrade whitehouse first! whitehouse level:  " + dependence.projectLevel +". houses level: " + Whitehouse());
			return false;
		}
	}
}
