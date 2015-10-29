using UnityEngine;
using System.Collections;

public class Street : Project 
{

	public Whitehouse whitehouse;

	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Street");

	}

	void Start()
	{
		upgradeWindowResource = "UpgradeWindowMirror";
		offset = -1.2f;
		projectName = "Street";
		clockText.text = "0";
		capacities = new int[5] {5,10,10,15,15};
		costs = new int[5] {15,30,50,75,100};
		buildingRounds = new int[5] {2,4,6,8,10};
		requiredWhitehouse = new int[5] {1,2,3,4,5};

        UpdateText(projectLevel, new string[1] { "capacity" }, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }

	protected override void Upgrade()
	{
		Game.overseer.capacity += Capacity();
        UpdateText(projectLevel + 1, new string[1] { "capacity" }, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }

    public override bool MetRequirements()
	{
		Debug.Log ("Whitehouse level: " + whitehouse.projectLevel + " : " + projectLevel + " Street level");
		if(whitehouse.projectLevel > projectLevel)
		{
			return true;
		}
		else
		{
			Debug.Log ("Upgrade Whitehouse first! Whitehouse level:  " + whitehouse.projectLevel +". Street level: " + projectLevel);
			return false;
		}
	}

}
