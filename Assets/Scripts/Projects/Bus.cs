﻿using UnityEngine;
using System.Collections;

public class Bus : Project
{
    private Station station;

    public static int[] Costs = new int[3] { 20, 20, 20 };
    public static int[] Capacities = new int[3] { 10, 10, 10 };
    public static int[] BuildingRounds = new int[3] { 0, 0, 0 };
    public static int[] RequiredPoints = new int[1];
    public static int[] RequiredWhitehouse = new int[3] { 2, 3, 4 };

    void Awake()
    {
        project = GameObject.FindGameObjectWithTag("Bus");
        dependence = GameObject.Find("PlaceholderWhitehouse").GetComponent<Whitehouse>();
        station = GameObject.Find("PlaceholderStation").GetComponent<Station>();

        upgradeWindowResource = "UpgradeWindowMirror";
        offset = -1.2f;

        projectName = "Bus";
        clockText.text = "0";

        costs = new int[3] { 20, 20, 20 };
        capacities = new int[3] { 10, 10, 10 };
        buildingRounds = new int[3] { 0, 0, 0 };
        requiredWhitehouse = new int[3] { 2, 3, 4 };

        UpdateText(projectLevel, new string[1] { "capacity" }, new int[1][] { capacities }, "Station level: ", new int[1] { 1 });
    }

    protected override void Upgrade()
    {
        Game.overseer.capacity += Capacity();
        UpdateText(projectLevel + 1, new string[1] { "capacity" }, new int[1][] { capacities }, "Whitehouse level: ", requiredWhitehouse);
    }

    public override bool MetRequirements()
    {
        
        if (dependence.projectLevel >= Whitehouse() && station.projectLevel >= 1)
        {
            return true;
        }
        else
        {
            //Debug.Log("Upgrade white or station first! station level:  " + dependence.projectLevel + ". Nedd Station level: " + Whitehouse());
            return false;
        }
    }
}
