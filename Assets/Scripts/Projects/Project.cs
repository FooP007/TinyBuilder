using UnityEngine;
using System.Collections;

public class Project : MonoBehaviour 
{
	public TextMesh clockText;
	public int projectLevel = 0;
	public Sprite[] projectSprites;
	public Sprite[] placeholderSprites;

	protected int[] costs = new int[1];
	protected int[] capacities = new int[1];
	protected int[] buildingRounds = new int[1];
	protected int[] requiredPoints = new int[1];
	protected int[] requiredWhitehouse = new int[1];

	
	protected GameObject project;
	protected string _projectName;
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
				Upgrade();
				projectLevel++;
				FillSpriteRenderer();
				constructinDays = 0;
				constructing = false;
			}
		}
	}

	public void Initiate()
	{

		int range = projectLevel;

		for(int i = 0; i < range; i++)
		{
			projectLevel = i;
			Debug.Log ("project: " + this.name);
			Upgrade();

		}
		projectLevel = range;
	}

	protected virtual void Upgrade()
	{	}

	public virtual bool MetRequirements()
	{
		if(Game.overseer.points >= Points())
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
		GameObject upgradeWindow = (GameObject)Instantiate(Resources.Load("UpgradeWIndow"));
		upgradeWindow.transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
		UpgradeWindow script = upgradeWindow.GetComponent<UpgradeWindow>();
		script.FillProejct(this);

		/*
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
		*/
	}

	public string projectName
	{
		get{ return _projectName; }
		set{ _projectName = value; }
	}

	public int Whitehouse()
	{
		return ArrayValue(requiredWhitehouse);
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

		if(projectLevel >= 0 && projectLevel < placeholderSprites.Length)
		{
			GetComponent<SpriteRenderer>().sprite = placeholderSprites[projectLevel];
		}
		else
		{
			Debug.Log ("The given level was not in the expected range. Range :0 - " + placeholderSprites.Length +" but projectLevel was " + projectLevel);
		}
		
		if(projectLevel > 0 && projectLevel <= projectSprites.Length)
		{
			appearence.sprite = projectSprites[projectLevel - 1];
		}
		else
		{
			Debug.Log ("The given level was not in the expected range. Range :1 - " + projectSprites.Length +" but projectLevel was " + projectLevel);
		}
		
	}
}
