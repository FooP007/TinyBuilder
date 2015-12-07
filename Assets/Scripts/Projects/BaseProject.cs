using UnityEngine;
using System.Collections.Generic;

public class BaseProject
{
    private BaseProject[] _dependence;
    private GameStats gameStats;
    public int projectLevel = 0;

    private int[] _costs;
    private int[] _capacities;
    private int[] _buildingRounds;
    private int[] _requiredPoints;
    private int[] _requiredWhitehouse;
    private int[] _extraPointsList;

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
        string projectName, GameStats stats, string pType, BaseProject[] dependence = null, int[] extraPointsList = null, int[] discounts = null, int[] builder = null, int newProjectLevel = 0)
    {
         _costs                = costs;
         _capacities           = capacities;
         _buildingRounds       = buildingRounds;
         _requiredPoints       = requiredPoints;
         _requiredWhitehouse   = requiredWhitehouse;

        if(discounts != null)
        {
            _discounts = discounts;
        }

        if (builder != null)
        {
            _builder = builder;
        }

        if (extraPointsList != null)
        {
            _extraPointsList = extraPointsList;
        }


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
            //Debug.Log("project: " + projectName);
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

    private void Upgrade()
    {
        
        if (projectType == BaseProject.capacityProject)
        {
            gameStats.capacity += Capacity();
        }
        else if(projectType == BaseProject.citizienProject)
        {
            gameStats.citizen += Capacity();
            
        }

        if(_extraPointsList != null)
        {
            //Debug.Log("EnvironmentPoints: "+EnvironmentPoints());
            gameStats.environmentPoints += EnvironmentPoints();
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
            if (gameStats.points >= Points())
            {
                return true;
            }
            else
            {
                //Debug.Log ("Not enough points! Your points: " + gameStats.points +". Points needed: " + Points() +" Project:"+ projectName);
                return false;
            }
        }
        else
        {
            if(_dependence.Length > 1)
            {
                if (_dependence[0].projectLevel >= Whitehouse() && _dependence[1].projectLevel >= _extraPointsList[0])
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
                if (_dependence[0].projectLevel >= Whitehouse())
                {
                    return true;
                }
                else
                {
                    /*if (projectName == "Carpool")
                    {
                        Debug.Log("Upgrade "+ _dependence.projectName + " first!"+ _dependence.projectName +" level:  " + _dependence.projectLevel + ". " + projectName + " level: " + Whitehouse());
                    }*/
                    return false;
                }
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
            // Debug.Log("CanBuyProject = false! " + projectName + " costs: " + Cost()+ " coins: "+ gameStats.coins);
        }
    }

    public void BuyProject()
    {
        gameStats.coins -= (Cost() - gameStats.discount);
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

    public int EnvironmentPoints()
    {
        return ArrayValue(_extraPointsList);
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
