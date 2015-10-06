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
		costs = new int[5] {0,10,20,30,40};
		buildingRounds = new int[5] {0,2,4,6,8};
		requiredPoints = new int[5]{0,20,80,200,400};
	}
}
