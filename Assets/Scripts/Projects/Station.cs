using UnityEngine;
using System.Collections;

public class Station : Project
{
    public static int[] Costs = new int[2] { 20, 50 };
    public static int[] Capacities = new int[1];
    public static int[] BuildingRounds = new int[2] { 2, 8 };
    public static int[] RequiredPoints = new int[1];
    public static int[] RequiredWhitehouse = new int[2] { 2, 3 };

    void Awake()
    {
        project = GameObject.FindGameObjectWithTag("Station");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();

        upgradeWindowResource = "UpgradeWindowMirror";
        offset = -1.2f;

        projectName = "Station";
        clockText.text = "0";

        costs = new int[2] { 20, 50 };
        buildingRounds = new int[2] { 2, 8 };
        requiredWhitehouse = new int[2] { 2, 3 };

        UpdateText(projectLevel, new string[1] { "Station level " }, new int[1][] { new int[1] { projectLevel + 1 } }, "Whitehouse level: ", requiredWhitehouse, false);

    }

    protected override void Upgrade()
    {
        UpdateText(projectLevel + 1, new string[1] { "Station level " }, new int[1][] { new int[1] { (projectLevel + 1) } }, "Whitehouse level: ", requiredWhitehouse, false);
       
    }

    public override bool MetRequirements()
    {
        //Debug.Log("dependence:" + dependence.projectLevel + " : Whitehouse " + Whitehouse());
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
