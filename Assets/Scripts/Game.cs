using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	public GameObject carpool;

	private Carpool carpoolScript;
	private int carpoolLevel = 0;

	private bool spaceKeyDown = false;

	// Use this for initialization
	void Start () 
	{
		carpoolScript = carpool.GetComponent<Carpool>();

		buildTown();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("space"))
		{

			spaceKeyDown = true;
			carpoolLevel++;
			carpoolScript.FillSpriteRenderer(carpoolLevel);
		}

		if(Input.GetKeyUp("space")  )
		{
			spaceKeyDown = false;
		}
	}

	void buildTown()
	{

		carpoolScript.FillSpriteRenderer(carpoolLevel);

	}
}
