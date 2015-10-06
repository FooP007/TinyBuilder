using UnityEngine;
using System.Collections;

public class Overseer : MonoBehaviour 
{
	private static Overseer instance = null;

	private TextMesh coinText;
	private TextMesh pointText;
	private TextMesh environmentText;
	private TextMesh capacityText;
	private TextMesh citizenText;
	private TextMesh dayText;

	private int _coins;
	private int _points;
	private int _citizen;
	private int _capacity;
	private int _environmentPoints;
	private int _day;
	
	private Overseer()
	{
		coinText = GameObject.FindGameObjectWithTag("Coin").GetComponent<TextMesh>();
		pointText = GameObject.FindGameObjectWithTag("Point").GetComponent<TextMesh>();
		environmentText = GameObject.FindGameObjectWithTag("Environment").GetComponent<TextMesh>();
		capacityText = GameObject.FindGameObjectWithTag("Capacity").GetComponent<TextMesh>();
		citizenText = GameObject.FindGameObjectWithTag("Citizen").GetComponent<TextMesh>();
		dayText = GameObject.FindGameObjectWithTag("Day").GetComponent<TextMesh>();
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

	public void Income()
	{
		if(capacity >= citizen)
		{
			points += citizen;
		}

		points += environmentPoints;
		coins += citizen;
	}

	public int day
	{
		get { return _day; }
		set { 
			_day = value; 
			dayText.text = "Day: " + value.ToString();
		}
	}
	
	public int coins
	{
		get { return _coins; }
		set { 
				_coins = value; 
				coinText.text = value.ToString();
			}
	}

	public int environmentPoints
	{
		get { return _environmentPoints; }
		set { 
				_environmentPoints = value;
				environmentText.text = value.ToString() + " Enviroment points";
			}
	}

	public int points
	{
		get { return _points; }
		set { 
				_points = value;
				pointText.text = value.ToString();
			}
	}

	public int citizen
	{
		get { return _citizen; }
		set { 
				_citizen = value;
			citizenText.text = value.ToString()+ " Citizen";
			}
	}

	public int capacity
	{
		get { return _capacity; }
		set { 
				_capacity = value;
			capacityText.text = value.ToString()+ " Capacity";
			}
	}


}
