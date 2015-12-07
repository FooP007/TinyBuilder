using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

public class Day
{

    public int day { get; set; }
    
    public int coins { get; set; }
    public int points { get; set; }
    public int citizen { get; set; }
    public int capacity { get; set; }
    public int environmentPoints { get; set; }

    public int WhitehouseLvl { get; set; }
    public int CycletrackLvl { get; set; }
    public int HouseLvl { get; set; }
    public int StreetLvl { get; set; }
    public int CarpooltLvl { get; set; }
    public int BusLvl { get; set; }
    public int StationLvl { get; set; }
    public int TrainLvl { get; set; }
    public int IndustryLvl { get; set; }

    public int discount { get; set; }
    public int maxBuilder { get; set; }

    public Dictionary<string, int> constructProjects { get; set; }

    public GameStats dayStats { get; set; }
}
