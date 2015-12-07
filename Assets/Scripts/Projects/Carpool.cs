﻿using UnityEngine;
using System.Collections;

public class Carpool : Project 
{

    public static int[] extraPointsList = new int[5] {1, 2, 4, 7, 10};
	public Street street;

    public static int[] Costs = new int[5] { 10, 20, 35, 55, 70 };
    public static int[] Capacities = new int[5] { 3, 9, 19, 34, 55 };
    public static int[] BuildingRounds = new int[5] { 1, 2, 3, 4, 5 };
    public static int[] RequiredPoints = new int[1];
    public static int[] RequiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };

    void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Carpool");
        projectName = "Carpool";

        upgradeWindowResource = "UpgradeWindowMirror";
        offset = -1.2f;

        clockText.text = "0";

        costs = new int[5] { 10, 20, 35, 55, 70 };
        capacities = new int[5] { 3, 9, 19, 34, 55 };
        buildingRounds = new int[5] { 1, 2, 3, 4, 5 };
        requiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };
    }

	void Start()
	{
        UpdateText(projectLevel, new string[2] { " capacity", " envir pts" }, new int[2][] { capacities, extraPointsList }, "Street level: ", requiredWhitehouse);
        
    }

	protected override void Upgrade ()
	{
		base.Upgrade ();
		Game.overseer.capacity += Capacity();
		Game.overseer.environmentPoints += extraPointsList[projectLevel];

        UpdateText(projectLevel + 1, new string[2] { " capacity", " envir pts" }, new int[2][] { capacities, extraPointsList }, "Street level: ", requiredWhitehouse);
    }

	public override bool MetRequirements()
	{
		if(street.projectLevel > projectLevel)
		{
			return true;
		}
		else
		{
			//Debug.Log ("Upgrade street first! Street level:  " + street.projectLevel +". Carpool level: " + projectLevel);
			return false;
		}
	}

	public int EnviromentPoints()
	{
		return ArrayValue(extraPointsList);
	}
}
