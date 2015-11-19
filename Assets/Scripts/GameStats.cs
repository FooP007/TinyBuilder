using UnityEngine;
using System.Collections.Generic;

public class GameStats
{
    private int _coins;
    private int _points;
    private int _citizen;
    private int _capacity;
    private int _environmentPoints;
    private int _day;
    private int _discount;
    private int _builder;

    public int maxBuilder;

    private List<BaseProject> baseProjects;
    
    public GameStats()
    {
        baseProjects = new List<BaseProject>();
        coins = 15;
        points = 0;
        citizen = 5;
        capacity = 5;
        environmentPoints = 0;
        maxBuilder = 0;
       
        FillBaseProjectList();
        //day = startDay;
    }

    private void FillBaseProjectList()
    {
        BaseProject whitehouse = new BaseProject(Whitehouse.costs, Whitehouse.capacities, Whitehouse.buildingRounds, Whitehouse.requiredPoints, Whitehouse.requiredWhitehouse, "Whitehouse", this, BaseProject.citizienProject, null, null, null, 1);
        baseProjects.Add(whitehouse);

        BaseProject houses = new BaseProject(Houses.costs, Houses.capacities, Houses.buildingRounds, Houses.requiredPoints, Houses.requiredWhitehouse, "House", this, BaseProject.citizienProject,  whitehouse);
        baseProjects.Add(houses);

        BaseProject street = new BaseProject(Street.costs, Street.capacities, Street.buildingRounds, Street.requiredPoints, Street.requiredWhitehouse, "Street", this, BaseProject.capacityProject, whitehouse);
        baseProjects.Add(street);
    }

    private bool Solvent(BaseProject baseProject)
    {
        if (coins >= (baseProject.Cost() - discount))
        {
            return true;
        }
        else
        {
            //Debug.Log("Not enough coins! Your coins: " + coins + ". Coins needed: " + project.Cost());
            return false;
        }
    }

    public bool CanBuyBaseProject(BaseProject baseProject)
    {
        if (baseProject.Rounds() != Project.outOfRange)
        {
            if (!baseProject.constructing)
            {
                if (baseProject.MetRequirements())
                {
                    if (Solvent(baseProject))
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
                //Debug.Log ("You cant upgrade this project at the moment because it is still under construction!");
                return false;
            }
        }
        else
        {
            //Debug.Log("Project is at maximum upgrade!");
            return false;
        }
    }

    public void BuyProject(BaseProject p)
    {
        coins -= (p.Cost() - discount);
        
        p.BuyProject();
    }

    public void Income()
    {
        if (capacity >= citizen)
        {
            points += citizen;
        }

        points += environmentPoints;
        coins += citizen;
    }

    public void nextDay()
    {
        // foreach project in constructing list -1 day
        /*
        overseer.day++;
        overseer.Income();
        overseer.builder = overseer.maxBuilder;
        */
        foreach (BaseProject bp in baseProjects)
        {
            bp.Construct();
        }
    }

    public List<BaseProject> GetBuyAbleBaseProjects()
    {
        List<BaseProject> reList = new List<BaseProject>();

        foreach (BaseProject bp in baseProjects)
        {
            if(CanBuyBaseProject(bp))
            {
                reList.Add(bp);
            }
        }
        return reList;
    }

    public int environmentPoints
    {
        get { return _environmentPoints; }
        set { _environmentPoints = value; }
    }

    public int coins
    {
        get { return _coins; }
        set { _coins = value; }
    }

    public int discount
    {
        get { return _discount; }
        set { _discount = value; }
    }

    public int points
    {
        get { return _points; }
        set { _points = value; }
    }

    public int citizen
    {
        get { return _citizen; }
        set { _citizen = value; }
    }

    public int capacity
    {
        get { return _capacity; }
        set { _capacity = value; }
    }
    
    public int  builder
    {
        get { return _builder; }
        set { _builder = value; }
    }
}
