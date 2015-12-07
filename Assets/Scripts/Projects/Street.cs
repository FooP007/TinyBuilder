using UnityEngine;
using System.Collections;

public class Street : Project 
{
    public Whitehouse whitehouse;

    public static int[] Costs = new int[5] { 15, 30, 50, 75, 100 };
    public static int[] Capacities = new int[5] { 5, 10, 10, 15, 15 };
    public static int[] BuildingRounds = new int[5] { 2, 4, 6, 8, 10 };
    public static int[] RequiredPoints = new int[1];
    public static int[] RequiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };

    void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Street");

        upgradeWindowResource = "UpgradeWindowMirror";
        offset = -1.2f;

        projectName = "Street";
        clockText.text = "0";

        costs = new int[5] { 15, 30, 50, 75, 100 };
        capacities = new int[5] { 5, 10, 10, 15, 15 };
        buildingRounds = new int[5] { 2, 4, 6, 8, 10 };
        requiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };
    }

	void Start()
	{
        UpdateText(projectLevel, new string[1] { " capacity" }, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }

	protected override void Upgrade()
	{
		Game.overseer.capacity += Capacity();
        UpdateText(projectLevel + 1, new string[1] { " capacity" }, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }

    public override bool MetRequirements()
	{
		if(whitehouse.projectLevel > projectLevel)
		{
			return true;
		}
		else
		{
			//Debug.Log ("Upgrade Whitehouse first! Whitehouse level:  " + whitehouse.projectLevel +". Street level: " + projectLevel);
			return false;
		}
	}

}
