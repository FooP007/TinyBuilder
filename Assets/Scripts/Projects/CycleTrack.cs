using UnityEngine;
using System.Collections;

public class CycleTrack : Project 
{
	private int[] extraPointsList = new int[5] {2,4,8,14,22};
	public Whitehouse whitehouse;
	
	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("CycleTrack");
		
	}
	
	void Start()
	{

		clockText.text = "0";
		capacities = new int[5] {1,3,7,14,25};
		costs = new int[5] {15,20,30,45,65};
		buildingRounds = new int[5] {1,2,3,4,5};
		requiredWhitehouse = new int[5] {1,2,3,4,5};
	}
	
	protected override void Upgrade ()
	{
		base.Upgrade ();
		Game.overseer.capacity += Capacity();
		Game.overseer.environmentPoints += extraPointsList[projectLevel];
	}
	
	public override bool MetRequirements()
	{
		Debug.Log ("whitehouse level: " + whitehouse.projectLevel + " : " + Whitehouse() + " houses level");
		if(whitehouse.projectLevel >= Whitehouse())
		{
			return true;
		}
		else
		{
			Debug.Log ("Upgrade whitehouse first! whitehouse level:  " + whitehouse.projectLevel +". houses level: " + Whitehouse());
			return false;
		}
	}
}
