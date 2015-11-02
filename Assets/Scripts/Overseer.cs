﻿using UnityEngine;
using System.Collections.Generic;

public class Overseer : MonoBehaviour
{
   
    private GameObject upgradeWindow;

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
    private int _discount;
    private int _builder;

    private int _maxBuilder; 

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
    
    public bool Solvent(Project project)
    {
        if (coins >= (project.Cost() - discount))
        {
            return true;
        }
        else
        {
            Debug.Log("Not enough coins! Your coins: " + coins + ". Coins needed: " + project.Cost());
            return false;
        }
    }

    public bool CanBuyProject(Project project, bool constructing)
	{
		if(!constructing)
		{
			if(project.MetRequirements())
			{
				if(Solvent(project))
				{
					return true;
				}
				else
				{
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

    public void BuilderUsed()
    {
        builder--;
        if (builder == 0)
        {
            Debug.Log("remove all builder");

            foreach (Project p in Game.projects)
            {
                foreach (GameObject b in p.allBuilder)
                {
                   
                    DestroyObject(b);
                }

                for (var i = 0; i < p.allBuilder.Count; i++)
                {
                    p.allBuilder.RemoveAt(i);
                }
            }
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

    public int discount
    {
        get { return _discount; }
        set { _discount = value; }
    }
    /**
     * The amount of builder indicates how many time 
     * the player can reduce the building rounds of one of the buildings
    */
    public int builder
    {
        get { return _builder; }
        set { _builder = value; }
    }

    public int maxBuilder
    {
        get { return _maxBuilder; }
        set { _maxBuilder = value; }
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
                Game.UpdateUpgradeWindows();
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
