using UnityEngine;
using System.Collections;

public class Carpool : Project 
{

	public Sprite carpool1;
	public Sprite carpool2;
	public Sprite carpool3;
	public Sprite carpool4;
	public Sprite carpool5;



	void Start()
	{
		clockText.text = "0";

		project = GameObject.FindGameObjectWithTag("Carpool");
		sprites = new Sprite[5] { carpool1, carpool2, carpool3, carpool4, carpool5 };
		capacities = new int[5] {3,9,19,34,55};
		costs = new int[5] {10,20,35,55,7};
		buildingRounds = new int[5] {1,2,3,4,5};
		requiredWhitehouse = new int[5] {1,2,3,4,5};

		_rounds = ArrayValue(buildingRounds);
		Debug.Log ("_rounds: " + _rounds);
	}

	void OnMouseDown()
	{
		StartConstructing();
	}


	
	// Update is called once per frame
	void Update () 
	{

	}
}
