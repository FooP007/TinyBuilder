using UnityEngine;
using System.Collections;

/**
 * The projectlevel of the whitehouse must be at least 1
 */

public class Whitehouse : Project 
{

	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Whitehouse");

	}

	void Start()
	{
		projectName = "Whitehouse";
       
        clockText.text = "0";
		costs           = new int[5] {0, 10, 20,  30,  40};
		buildingRounds  = new int[5] {0,  2,  4,   6,   8};
		requiredPoints  = new int[5] {0, 20, 80, 200, 400};
       
        UpdateText(projectLevel);

    }

    protected override void Upgrade()
    {
        UpdateText(projectLevel + 1);

    }

    protected override void UpdateText(int inputLevel)
    {
        if (inputLevel >= costs.Length )
        {
            effectText = "Maximum Upgrade";
            requireText = "Maximum Upgrade";
        }
        else
        {
            // has to be +2 because thew whitehouse starts at level 1 and not 0 like all the other projects
            effectText = "Whitehouse level" + (projectLevel + 1);
            requireText = "Points: " + requiredPoints[inputLevel];
        }
    }
}
