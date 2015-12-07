using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Industry : Project
{
    public static int[] discounts = new int[5]    { 5, 0, 10, 0, 15 };
    public static int[] builders  = new int[5]    { 0, 1,  0, 2,  3 };

    public static int[] Costs = new int[5] { 15, 25, 35, 45, 55 };
    public static int[] Capacities = new int[1];
    public static int[] BuildingRounds = new int[5] { 2, 4, 6, 8, 10 };
    public static int[] RequiredPoints = new int[1];
    public static int[] RequiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };

    void Awake()
    {
        project = GameObject.FindGameObjectWithTag("Industry");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();

        projectName = "Industry";
        clockText.text = "0";

        costs = new int[5] { 15, 25, 35, 45, 55 };
        capacities = new int[5] { 1, 3, 7, 14, 25 };
        buildingRounds = new int[5] { 2, 4, 6, 8, 10 };
        requiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };
    }

    void Start()
    {
        UpdateText(projectLevel, new string[1] { "Discount " }, new int[1][] { discounts }, "Whitehouse level: ", requiredWhitehouse, false);
    }

    protected override void Upgrade()
    {
        base.Upgrade();
        // increase discount 5, 10 ,15
        // or, or both
        // increase building time 1, 2, 3 round/s faster
        
        if (discounts[projectLevel] != 0)
        {
            Game.overseer.discount = discounts[projectLevel];
        }

        if (builders[projectLevel] != 0)
        {
            Game.overseer.maxBuilder = builders[projectLevel];
            Game.overseer.builder = builders[projectLevel];
        }
        
        
        switch (ComposedText(projectLevel + 1))
        {
            case 1:
                UpdateText(projectLevel + 1, new string[1] { "Builder" }, new int[1][] { builders }, "Whitehouse level: ", requiredWhitehouse, false);
            break;

            case 2:
                UpdateText(projectLevel + 1, new string[1] { "Discount" }, new int[1][] { discounts }, "Whitehouse level: ", requiredWhitehouse, false);
            break;

            case 3:
                UpdateText(projectLevel + 1, new string[2] { "Discount", " Builder" }, new int[2][] { discounts, builders }, "Whitehouse level: ", requiredWhitehouse, false);
            break;

            default:
                Debug.Log("wrong result");
            break;

        }
    }

    private int ComposedText(int inputLevel)
    {
        int result = 0; ;

        if(inputLevel >= costs.Length)
        {
            result = 2;
        }
        else
        {
            if (discounts[inputLevel] != 0)
            {
                result = 2;
            }

            if (builders[inputLevel] != 0)
            {
                if (discounts[inputLevel] != 0)
                {
                    result = 3;
                }
                result = 1; ;
            }
        }
        
        return result;
    }

    public override bool MetRequirements()
    {
        if (dependence.projectLevel >= Whitehouse())
        {
            return true;
        }
        else
        {
            //Debug.Log("Upgrade whitehouse first! whitehouse level:  " + dependence.projectLevel + ". houses level: " + Whitehouse());
            return false;
        }
    }

}
