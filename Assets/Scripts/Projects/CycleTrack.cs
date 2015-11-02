using UnityEngine;
using System.Collections;

public class CycleTrack : Project 
{
	private int[] extraPointsList = new int[5] {2,4,8,14,22};
	//public Whitehouse whitehouse;
	
	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("CycleTrack");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();

    }
	
	void Start()
	{
		projectName = "Cylcetrack";
		clockText.text = "0";
		capacities = new int[5] {1,3,7,14,25};
		costs = new int[5] {15,20,30,45,65};
		buildingRounds = new int[5] {1,2,3,4,5};
		requiredWhitehouse = new int[5] {1,2,3,4,5};

        UpdateText(projectLevel, new string[2] { " capacity", " envir pts" }, new int[2][] { capacities, extraPointsList }, "Street level: ", requiredWhitehouse);
    }
	
	protected override void Upgrade()
	{
		Game.overseer.capacity += Capacity();
		Game.overseer.environmentPoints += extraPointsList[projectLevel];

        UpdateText(projectLevel + 1, new string[2] { " capacity", " envir pts" }, new int[2][] { capacities, extraPointsList }, "Street level: ", requiredWhitehouse);
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
