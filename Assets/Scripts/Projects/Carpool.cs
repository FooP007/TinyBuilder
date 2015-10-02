using UnityEngine;
using System.Collections;

public class Carpool : Project 
{
	public Sprite carpool1;
	public Sprite carpool2;
	public Sprite carpool3;
	public Sprite carpool4;
	public Sprite carpool5;

	private Street _street;

	/*private Carpool(Street street)
	{
		_street = street;
	}*/

	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Carpool");
		sprites = new Sprite[5] { carpool1, carpool2, carpool3, carpool4, carpool5 };
	}

	void Start()
	{
		clockText.text = "0";
		capacities = new int[5] {3,9,19,34,55};
		costs = new int[5] {10,20,35,55,70};
		buildingRounds = new int[5] {1,2,3,4,5};
		requiredWhitehouse = new int[5] {1,2,3,4,5};
	}

	public void getAppendend(Street street)
	{
		_street = street;
	}

	public override bool MetRequirements()
	{
		Debug.Log ("street level: " + _street.projectLevel + " : " + projectLevel + " carpool level");
		if(_street.projectLevel >= projectLevel)
		{
			return true;
		}
		else
		{
			Debug.Log ("Upgrade street first! Street level:  " + _street.projectLevel +". Carpool level: " + projectLevel);
			return false;
		}
	}
	
}
