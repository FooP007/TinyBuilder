using UnityEngine;
using System.Collections;

public class Street : Project 
{
	public Sprite street1;
	public Sprite street2;
	public Sprite street3;
	public Sprite street4;
	public Sprite street5;

	void Awake()
	{
		project = GameObject.FindGameObjectWithTag("Street");
		sprites = new Sprite[5] { street1, street2, street3, street4, street5 };
	}

	void Start()
	{
		clockText.text = "0";
		capacities = new int[5] {5,10,10,15,15};
		costs = new int[5] {15,30,50,75,100};
		buildingRounds = new int[5] {2,4,6,8,10};
		requiredWhitehouse = new int[5] {1,2,3,4,5};
	}

}
