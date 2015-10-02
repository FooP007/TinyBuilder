using UnityEngine;
using System.Collections;

public class Overseer : MonoBehaviour 
{
	private static Overseer instance = null;

	private int _coins = 0;
	private int _points = 0;

	// Use this for initialization
	private Overseer()
	{

	}
	
	public static Overseer Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Overseer();
			}
			return instance;
		}
	}

	public bool CanBuyProject(Project project, bool constructing)
	{
		if(!constructing)
		{
			if(project.MetRequirements())
			{
				if(coins >= project.Cost())
				{
					Debug.Log ("Project buy succesfully! Costs: " + project.Cost() +" coins: "+ coins );

					coins -= project.Cost();
					Debug.Log ("new income: " + coins);
					return true;
				}
				else
				{
					Debug.Log ("Not enough coins! Your coins: " + coins +". Coins needed: " + project.Cost());
					return false;
				}
			}
			else
			{
				return false;
			}

		}
		else
		{
			Debug.Log ("You cant upgrade this project at the moment because it is still under construction!");
			return false;
		}
	}

	public int coins
	{
		get { return _coins; }
		set { _coins = value; }
	}

	public int points
	{
		get { return _points; }
		set { _points = value; }
	}
}
