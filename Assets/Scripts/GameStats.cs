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

    public List<BaseProject> baseProjects;

    public BaseProject whitehouse;
    public BaseProject cycletrack;
    public BaseProject houses;
    public BaseProject street;
    public BaseProject carpool;
    public BaseProject station;
    public BaseProject bus;
    public BaseProject train;
    public BaseProject industry;

    public GameStats()
    {
        baseProjects = new List<BaseProject>();
        coins = 15;
        points = 0;
        citizen = 5;
        capacity = 5;
        environmentPoints = 0;
        maxBuilder = 0;
        day = 0;

        FillBaseProjectList();
        //day = startDay;
    }

    public void Reset()
    {
        coins = 15;
        points = 0;
        citizen = 5;
        capacity = 5;
        environmentPoints = 0;
        maxBuilder = 0;
        day = 0;
        discount = 0;

        foreach (BaseProject bp in baseProjects)
        {
            if(bp.projectName == "Whitehouse")
            {
                bp.projectLevel = 1;
            }
            else
            {
                bp.projectLevel = 0;
            }
            bp.constructionDays = 0;
            bp.constructing = false;
        }
    }

    public void ClearConstructing()
    {
        foreach (BaseProject bp in baseProjects)
        {
            bp.constructionDays = 0;
            bp.constructing = false;
        }
    }

    private void FillBaseProjectList()
    {
        whitehouse = new BaseProject(Whitehouse.Costs, Whitehouse.Capacities, Whitehouse.BuildingRounds, Whitehouse.RequiredPoints, Whitehouse.RequiredWhitehouse, "Whitehouse", this, BaseProject.citizienProject, null, null, null, null, 1);
        baseProjects.Add(whitehouse);

        houses = new BaseProject(Houses.Costs, Houses.Capacities, Houses.BuildingRounds, Houses.RequiredPoints, Houses.RequiredWhitehouse, "House", this, BaseProject.citizienProject, new BaseProject[] { whitehouse });
        baseProjects.Add(houses);

        street = new BaseProject(Street.Costs, Street.Capacities, Street.BuildingRounds, Street.RequiredPoints, Street.RequiredWhitehouse, "Street", this, BaseProject.capacityProject, new BaseProject[] { whitehouse });
        baseProjects.Add(street);

        carpool = new BaseProject(Carpool.Costs, Carpool.Capacities, Carpool.BuildingRounds, Carpool.RequiredPoints, Carpool.RequiredWhitehouse, "Carpool", this, BaseProject.capacityProject, new BaseProject[] { street }, Carpool.extraPointsList);
        baseProjects.Add(carpool);

        cycletrack = new BaseProject(CycleTrack.Costs, CycleTrack.Capacities, CycleTrack.BuildingRounds, CycleTrack.RequiredPoints, CycleTrack.RequiredWhitehouse, "Cycletrack", this, BaseProject.capacityProject, new BaseProject[] { whitehouse }, CycleTrack.extraPointsList);
        baseProjects.Add(cycletrack);

        industry = new BaseProject(Industry.Costs, Industry.Capacities, Industry.BuildingRounds, Industry.RequiredPoints, Industry.RequiredWhitehouse, "Industry", this, BaseProject.capacityProject, new BaseProject[] { whitehouse }, null, Industry.discounts, Industry.builders);
        baseProjects.Add(industry);

        station = new BaseProject(Station.Costs, Station.Capacities, Station.BuildingRounds, Station.RequiredPoints, Station.RequiredWhitehouse, "Station", this, BaseProject.capacityProject, new BaseProject[] { whitehouse });
        baseProjects.Add(station);

        bus = new BaseProject(Bus.Costs, Bus.Capacities, Bus.BuildingRounds, Bus.RequiredPoints, Bus.RequiredWhitehouse, "Bus", this, BaseProject.capacityProject, new BaseProject[] { whitehouse, station }, new int[] { 1});
        baseProjects.Add(bus);

        train = new BaseProject(Train.Costs, Train.Capacities, Train.BuildingRounds, Train.RequiredPoints, Train.RequiredWhitehouse, "Train", this, BaseProject.capacityProject, new BaseProject[] { whitehouse, station }, new int[] {2 });
        baseProjects.Add(train);

    }

    private bool Solvent(BaseProject baseProject)
    {
        if (coins >= (baseProject.Cost() - discount))
        {
            return true;
        }
        else
        {
            //Debug.Log("Not enough coins! Your coins: " + coins + ". Coins needed: " + baseProject.Cost());
            return false;
        }
    }

    private void UseBuilder()
    {
        
        if (builder > 0)
        {
            foreach (BaseProject bp in baseProjects)
            {
                if(builder <= 0)
                {
                    break;
                }
                else
                {
                    if (bp.constructing)
                    {
                        int length;

                        if (bp.constructionDays < builder)
                        {
                            length = (bp.constructionDays - 1);
                        }
                        else
                        {
                            length = builder;
                        }

                        for (int i = 0; i < length; i++)
                        {
                            bp.UseBuilder();
                        }
                    }
                }
            }
        }
    }

    public bool CanBuyBaseProject(BaseProject baseProject)
    {
        bool debug = false;
        if (baseProject.Rounds() != BaseProject.outOfRange)
        {
            //Debug.Log("project: "+ baseProject.projectName+ " constructing: "+ baseProject.constructing);
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
                    if(baseProject.projectName == "Carpssool")
                    {
                        Debug.Log("dont meet requirements");
                    }
                    return false;
                }
            }
            else
            {
                if (debug)
                {
                    Debug.Log("You cant upgrade this project at the moment because it is still under construction!");
                }
                return false;
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Project is at maximum upgrade!");
            } 
            return false;
        }
    }

    public void BuyProject(BaseProject p)
    {
        p.BuyProject();
    }

    public void Income()
    {
        if (capacity >= citizen)
        {
            points += citizen;
        }
        //Debug.Log("environmentPoints: "+environmentPoints);
        points += environmentPoints;
        coins += citizen;
    }

    public void nextDay()
    {
        // use all available builder
        UseBuilder();

        day++;
        Income();
        builder = maxBuilder;
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
            if (CanBuyBaseProject(bp))
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

    public int day
    {
        get { return _day; }
        set { _day = value; }
    }
}
