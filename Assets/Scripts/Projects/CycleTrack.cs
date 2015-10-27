﻿using UnityEngine;
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
		projectName = "Cylce track";
		clockText.text = "0";
		capacities = new int[5] {1,3,7,14,25};
		costs = new int[5] {15,20,30,45,65};
		buildingRounds = new int[5] {1,2,3,4,5};
		requiredWhitehouse = new int[5] {1,2,3,4,5};

        effectText = "+" + capacities[projectLevel + 1] + " capacity" + "\n + " + extraPointsList[projectLevel + 1] + " envir pts";
        requireText = "Whitehouse level: " + requiredWhitehouse[projectLevel];
    }
	
	protected override void Upgrade()
	{
		base.Upgrade();
		Game.overseer.capacity += Capacity();
		Game.overseer.environmentPoints += extraPointsList[projectLevel];

        effectText = "+" + capacities[projectLevel+1] + " capacity" + "\n + " + extraPointsList[projectLevel+1] + " environment pts";
        requireText = "Whitehouse level: " + requiredWhitehouse[projectLevel+1];
    }
	
	public override bool MetRequirements()
	{
		Debug.Log ("whitehouse level: " + dependence.projectLevel + " : " + Whitehouse() + " houses level");
		if(dependence.projectLevel >= Whitehouse())
		{
			return true;
		}
		else
		{
			Debug.Log ("Upgrade whitehouse first! whitehouse level:  " + dependence.projectLevel +". houses level: " + Whitehouse());
			return false;
		}
	}
}
