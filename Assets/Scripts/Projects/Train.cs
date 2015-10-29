using UnityEngine;
using System.Collections;

public class Train : Project
{

    private Station station;

    void Awake()
    {
        project = GameObject.FindGameObjectWithTag("Train");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();
        station = GameObject.Find("PlaceholderStation").GetComponent<Station>();

        upgradeWindowResource = "UpgradeWindowMirror";
        offset = -1.2f;

        projectName = "Train";
        clockText.text = "0";

        costs = new int[3] { 50, 50, 50 };
        buildingRounds = new int[3] { 0, 0, 0 };
        capacities = new int[3] { 20, 20, 20 };
        requiredWhitehouse = new int[3] { 3, 4, 5 };

        UpdateText(projectLevel, new string[1] { "capacity" }, new int[1][] { capacities }, "Station level: ", new int[1] { 2 });
    }

    protected override void Upgrade()
    {
        UpdateText(projectLevel + 1, new string[1] { "capacity" }, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }

    public override bool MetRequirements()
    {
        Debug.Log("Station level: " + dependence.projectLevel + " : need" + Whitehouse() + " Station level");
        if (dependence.projectLevel >= Whitehouse() && station.projectLevel >= 2)
        {
            return true;
        }
        else
        {
            Debug.Log("Upgrade white or station first! station level:  " + dependence.projectLevel + ". Nedd Station level: " + Whitehouse());
            return false;
        }
    }
}
