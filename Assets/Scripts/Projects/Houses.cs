using UnityEngine;
using System.Collections;

public class Houses : Project 
{
    public static string staticProjectName = "Houses";

    public static int[] Costs = new int[10] { 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
    public static int[] Capacities = new int[10] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    public static int[] BuildingRounds = new int[10] { 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
    public static int[] RequiredPoints = new int[1];
    public static int[] RequiredWhitehouse = new int[10] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
    

    void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Houses");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();

        projectName = staticProjectName;
        offset = -1.2f;
        upgradeWindowResource = "UpgradeWindowMirror";
        clockText.text = "0";

        costs = Costs;
        capacities = Capacities;
        buildingRounds = BuildingRounds;
        requiredWhitehouse = RequiredWhitehouse;

        UpdateText(projectLevel, new string[1] { "citizen"}, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }
	

	protected override void Upgrade()
	{
		Game.overseer.citizen += Capacity();
        UpdateText(projectLevel + 1, new string[1] { "citizen" }, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }

	public override bool MetRequirements()
	{
		//Debug.Log ("whitehouse level: " + dependence.projectLevel + " : " + Whitehouse() + " houses level");
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
