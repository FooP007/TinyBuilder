using UnityEngine;
using System.Collections;

public class Houses : Project 
{

	//public Whitehouse whitehouse;

	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Houses");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();
	}
	
	void Start()
	{
		projectName = "Houses";
        offset = -1.2f;
        upgradeWindowResource = "UpgradeWindowMirror";
        clockText.text = "0";
		capacities = new int[10] {5,6,7,8,9,10,11,12,13,14};
		costs = new int[10] {10,15,20,25,30,35,40,45,50,55};
		buildingRounds = new int[10] {2,2,3,3,4,4,5,5,6,6};
		requiredWhitehouse = new int[10] {1,1,2,2,3,3,4,4,5,5};
       
        effectText = "+" + capacities[projectLevel] + " citizen";
        requireText = "Whitehouse level: " + requiredWhitehouse[projectLevel];
        
    }

	protected override void Upgrade()
	{
		Game.overseer.citizen += Capacity();

        if(projectLevel >= costs.Length-1)
        {
            effectText = "Maximum Upgrade";
            requireText = "Maximum Upgrade";
        }
        else
        {
            effectText = "+" + capacities[projectLevel + 1] + " capacity";
            requireText = "Whitehouse level: " + requiredWhitehouse[projectLevel + 1];
        }
        
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
