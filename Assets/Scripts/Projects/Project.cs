using UnityEngine;
using System.Collections;

public class Project : MonoBehaviour 
{
	public TextMesh clockText;

	protected Sprite[] sprites;
	protected int[] costs;
	protected int[] capacities;
	protected int[] buildingRounds;
	protected int _rounds;
	protected int[] requiredPoints;
	protected int[] requiredWhitehouse;

	public int projectLevel = 0;
	protected GameObject project;

	protected bool constructing = false;
	protected int constructinDays = 0;


	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	protected void StartConstructing()
	{
		constructing = true;

		constructinDays = Rounds();
		clockText.text = constructinDays.ToString();
	}

	public void Construct()
	{
		if(constructing)
		{
			constructinDays--;
			clockText.text = constructinDays.ToString();
			if(constructinDays == 0)
			{
				projectLevel++;
				constructinDays = 0;
				constructing = false;
			}
		}
	}

	/**
	 * returns the capcity the project is currently providing
	 */
	public int Capacity()
	{
		return ArrayValue(capacities);
	}

	/**
	 * returns the count of rounds which it takes to build or upgrade
	 * the project
	 */
	public int Rounds()
	{
		return ArrayValue(buildingRounds);
	}

	/**
	 * returns the cost to build or upgrade the project
	 */
	public int Cost()
	{
		return ArrayValue(costs);
	}

	protected int ArrayValue(int[] array)
	{
		if(projectLevel >= 0 && projectLevel <= array.Length-1)
		{
			return array[projectLevel];
		}
		else
		{
			Debug.Log ("error "+ this.name + " index is out of range");
			return 0;
		}

	}

	/**
	 * Changes the sprite of the SpriteRenderer based on the upgfrade level of the project
	 * change to private later on
	 */
	public void FillSpriteRenderer()
	{
		SpriteRenderer appearence = project.GetComponent<SpriteRenderer>();
		appearence.sortingLayerName = "Buildings";

		if(projectLevel > 0 && projectLevel <= sprites.Length)
		{
			appearence.sprite = sprites[projectLevel-1];
			Debug.Log (sprites[projectLevel-1]);
		}
		else
		{
			Debug.Log ("The given level was not in the expected range. Range :1 - " + sprites.Length +" but projectLevel was " + projectLevel);
		}
		
	}
}
