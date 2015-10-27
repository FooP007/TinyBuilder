using UnityEngine;
using System.Collections.Generic;

public class Industry : Project
{
    private int[] discounts = new int[5]    { 5, 0, 10, 0, 15 };
    private int[] builders = new int[5]      { 0, 1,  0, 2,  3 };

    void Awake()
    {
        project = GameObject.FindGameObjectWithTag("Industry");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();
    }

    void Start()
    {
        projectName = "Industry";
        clockText.text = "0";

        capacities = new int[5] { 1, 3, 7, 14, 25 };
        costs = new int[5] { 15, 25, 35, 45, 55 };
        buildingRounds = new int[5] { 2, 4, 6, 8, 10 };
        requiredWhitehouse = new int[5] { 1, 2, 3, 4, 5 };

        UpdateText(projectLevel);
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
        UpdateText(projectLevel + 1);
       
    }

    protected override void UpdateText(int inputLevel)
    {
        if (inputLevel >= costs.Length - 1)
        {
            effectText = "Maximum Upgrade";
            requireText = "Maximum Upgrade";
        }
        else
        {
            effectText = "";

            if (discounts[inputLevel] != 0)
            {
                effectText = "Discount: " + discounts[inputLevel];
            }

            if (builders[inputLevel] != 0)
            {
                if (discounts[inputLevel] != 0)
                {
                    effectText += "\n";
                }
                effectText += "Builder: " + builders[inputLevel];
            }

            requireText = "Whitehouse level: " + requiredWhitehouse[inputLevel];
        }
    }

    public override bool MetRequirements()
    {
        Debug.Log("whitehouse level: " + dependence.projectLevel + " : " + Whitehouse() + " houses level");
        if (dependence.projectLevel >= Whitehouse())
        {
            return true;
        }
        else
        {
            Debug.Log("Upgrade whitehouse first! whitehouse level:  " + dependence.projectLevel + ". houses level: " + Whitehouse());
            return false;
        }
    }

}
