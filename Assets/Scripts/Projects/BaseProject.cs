using UnityEngine;
using System.Collections.Generic;

public class BaseProject
{
    private BaseProject _dependence;
    private GameStats gameStats;
    public int projectLevel = 0;

    private int[] _costs = new int[1];
    private int[] _capacities = new int[1];
    private int[] _buildingRounds = new int[1];
    private int[] _requiredPoints = new int[1];
    private int[] _requiredWhitehouse = new int[1];

    private int[] _discounts = new int[1];
    private int[] _builder = new int[1];

    private string _projectName;

    private bool _constructing = false;
    private int _constructionDays = 0;
    public static int outOfRange = 0;

    public static string citizienProject = "citizienProject";
    public static string capacityProject = "capacityProject";
    private string projectType = "";

    public int allBuilder = 0;

    public BaseProject(int[] costs, int[] capacities, int[] buildingRounds, int[] requiredPoints, int[] requiredWhitehouse,
        string projectName, GameStats stats, string pType, BaseProject dependence = null, int[] discounts = null, int[] builder = null, int newProjectLevel = 0)
    {
        int[] _costs                = costs;
        int[] _capacities           = capacities;
        int[] _buildingRounds       = buildingRounds;
        int[] _requiredPoints       = requiredPoints;
        int[] _requiredWhitehouse   = requiredWhitehouse;
        int[] _builder              = builder;
        int[] _discounts            = discounts;

        projectType = pType;
        gameStats = stats;
        _projectName = projectName;
        _dependence = dependence;
        projectLevel = newProjectLevel;
    }

    private void StartConstructing()
    {
        _constructionDays = Rounds();
        if (_constructionDays >= 0)
        {
            _constructing = true;
            if (_constructionDays == 0)
            {
                Construct();
            }
        }
    }

    public void UseBuilder()
    {
        allBuilder--;
        Construct();
    }

    public void Construct()
    {
        if (_constructing)
        {
            constructionDays--;
            if (_constructionDays == 0)
            {
                Upgrade();
                projectLevel++;
                // remove all builder
                allBuilder = 0;
               
                _constructionDays = 0;
                _constructing = false;
            }
        }
    }

    public void Initiate()
    {
        int range = projectLevel;

        for (int i = 0; i < range; i++)
        {
            projectLevel = i;
            Upgrade();

        }
        projectLevel = range;
    }

    private  void Upgrade()
    {
        if (projectType == BaseProject.capacityProject)
        {
            gameStats.capacity += Capacity();
        }
        else if(projectType == BaseProject.citizienProject)
        {
            gameStats.citizen += Capacity();
        }

        if (Discount() != 0)
        {
            gameStats.discount = Discount();
        }

        if (Builder() != 0)
        {
            gameStats.maxBuilder = Builder();
            gameStats.builder = Builder();
        }
    }

   

    public virtual bool MetRequirements()
    {
        if(_dependence == null)
        {
            if (Game.overseer.points >= Points())
            {
                return true;
            }
            else
            {
                //Debug.Log ("Not enough points! Your points: " + Game.overseer.points +". Points needed: " + Points());
                return false;
            }
        }
        else
        {
            if (_dependence.projectLevel >= Whitehouse())
            {
                return true;
            }
            else
            {
                //Debug.Log ("Upgrade whitehouse first! whitehouse level:  " + dependence.projectLevel +". houses level: " + Whitehouse());
                return false;
            }
        }
    }

    public void TryConstructing()
    {
        if (gameStats.CanBuyBaseProject(this))
        {
            BuyProject();
        }
        else
        {
            Debug.Log("CanBuyProject = false! " + projectName + " costs: " + Cost() + " coins: " + Game.overseer.coins);
        }
    }

    public void BuyProject()
    {
        Game.overseer.coins -= (Cost() - Game.overseer.discount);
        StartConstructing();
        //Debug.Log("bought: " + projectName);
    }

    public void AddBuilder(int count)
    {
        allBuilder += count;
    }

    public int constructionDays
    {
        get { return _constructionDays; }
        set
        {
            _constructionDays = value;
            if (_constructionDays < 0)
            {
                _constructionDays = 0;
            }
        }
    }

    public bool constructing
    {
        get { return _constructing; }
        set { _constructing = value; }
    }

    public string projectName
    {
        get { return _projectName; }
        set { _projectName = value; }
    }

    public int Whitehouse()
    {
        return ArrayValue(_requiredWhitehouse);
    }

    /**
	 * returns the capcity the project is currently providing
	 */
    public int Capacity()
    {
        return ArrayValue(_capacities);
    }

    /**
	 * returns the required Points the player needs to build 
	 * this project or the upgrade of this project
	 */
    public int Points()
    {
        return ArrayValue(_requiredPoints);
    }

    /**
	 * returns the count of rounds which it takes to build or upgrade
	 * the project
	 */
    public int Rounds()
    {
        return ArrayValue(_buildingRounds);
    }

    /**
	 * returns the cost to build or upgrade the project
	 */
    public int Cost()
    {
        return ArrayValue(_costs);
    }

    public int Discount()
    {
        return ArrayValue(_discounts);
    }

    public int Builder()
    {
        return ArrayValue(_builder);
    }

    protected int ArrayValue(int[] array)
    {
        if (projectLevel >= 0 && projectLevel <= array.Length - 1 && array != null)
        {
            return array[projectLevel];
        }
        else
        {
            //Debug.Log ("Error! Index is out of range. Array: " + array.ToString());
            return outOfRange;
        }
    }
}
