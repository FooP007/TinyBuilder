using UnityEngine;
using System.Collections;

public class Project : MonoBehaviour 
{
	public TextMesh clockText;

	protected Sprite[] sprites;
	protected int[] costs;
	protected int[] capacities;
	protected int[] buildingRounds;
	protected int[] requiredPoints = new int[1];
	protected int[] requiredWhitehouse;

	public int projectLevel = 0;
	protected GameObject project;

	protected bool constructing = false;
	protected int constructinDays = 0;
	protected int outOfRange = -1;

	protected void StartConstructing()
	{
		constructinDays = Rounds();

		if(constructinDays >= 0)
		{
			constructing = true;
			clockText.text = constructinDays.ToString();
		}

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
				FillSpriteRenderer();
				constructinDays = 0;
				constructing = false;
			}
		}
	}

	public virtual bool MetRequirements()
	{
		if(Points() >= Game.overseer.points)
		{
			return true;
		}
		else
		{
			Debug.Log ("Not enough points! Your points: " + Game.overseer.points +". Points needed: " + Points());
			return false;
		}
	}

	protected void OnMouseDown()
	{
		if(Rounds() != outOfRange)
		{
			if(Game.overseer.CanBuyProject(this, constructing))
			{
				StartConstructing();
			}
		}
		else
		{
			Debug.Log ("Project is at maximum upgrade!");
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
	 * returns the required Points the player needs to build 
	 * this project or the upgrade of this project
	 */
	public int Points()
	{
		return ArrayValue(requiredPoints);
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
			Debug.Log ("Error! Index is out of range. Array: " + array.ToString());
			return outOfRange;
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
			appearence.sprite = sprites[projectLevel - 1];
		}
		else
		{
			Debug.Log ("The given level was not in the expected range. Range :1 - " + sprites.Length +" but projectLevel was " + projectLevel);
		}
		
	}
}
