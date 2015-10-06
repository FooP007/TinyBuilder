using UnityEngine;
using System.Collections;

public class Carpool : Project 
{
	
	private int[] extraPointsList = new int[5] {1, 2, 4, 7, 10};
	public Street street;

	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Carpool");

	}

	void Start()
	{
		projectName = "Carpool";
		clockText.text = "0";
		capacities = new int[5] {3, 9, 19, 34, 55};
		costs = new int[5] {10, 20, 35, 55, 70};
		buildingRounds = new int[5] {1, 2, 3, 4, 5};
		requiredWhitehouse = new int[5] {1, 2, 3, 4, 5};
	}

	protected override void Upgrade ()
	{
		base.Upgrade ();
		Game.overseer.capacity += Capacity();
		Game.overseer.environmentPoints += extraPointsList[projectLevel];
	}

	public override bool MetRequirements()
	{
		Debug.Log ("street level: " + street.projectLevel + " : " + projectLevel + " carpool level");
		if(street.projectLevel > projectLevel)
		{
			return true;
		}
		else
		{
			Debug.Log ("Upgrade street first! Street level:  " + street.projectLevel +". Carpool level: " + projectLevel);
			return false;
		}
	}

	public int EnviromentPoints()
	{
		return ArrayValue(extraPointsList);
	}
}
