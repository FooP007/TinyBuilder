using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using Facet.Combinatorics;

public class UnitTest : MonoBehaviour
{
    public GameObject tinyBuilder;
    private Game mainGame;
    private Variations<Project> variations;

    private List<Project[]> buildPaths = new List<Project[]>();
    private Project[] buildPath;
    public Timer timer;
    private int maxDays = 20;

    private int pointsBarrierer0 = 30;
    private int pointsBarrierer1 = 80;
    private int pointsBarrierer2 = 300;
    private int pointsBarrierer3 = 500;

    private int barrierer0 = 10;
    private int barrierer1 = 15;
    private int barrierer2 = 20;
    private int barrierer3 = 25;

    private string[] buildpathes;
    private int index = 0;
    private int maxIndex = -1;
    private List<int> _allPoints;

    // Use this for initialization
    void Awake ()
    {
        mainGame = tinyBuilder.GetComponent<Game>();
        //buildPath = new Project[mainGame.maxDays];
        buildPaths.Add(buildPath);
    }

    void Start()
    {
        /*
        float temp = Time.realtimeSinceStartup;
        string[] t = StartUnitTest();
        
        Debug.Log("Time: " + (Time.realtimeSinceStartup - temp) + " sec");
        Debug.Log("possibilities: " + t.Length);
        */
        float temp2 = Time.realtimeSinceStartup;
        buildpathes = StartUnitTest();
        Debug.Log("Fix Time: " + (Time.realtimeSinceStartup - temp2) + " sec");
        Debug.Log("possibilities: " + buildpathes.Length);
        Debug.Log("count of points: " + _allPoints.Count);
        //maxIndex = buildpathes.Length - 1;
        
        int t = highestPoints();
        Debug.Log("index: "+t);
        Debug.Log(buildpathes[t]);
        //Debug.Log("Time: " + (Time.realtimeSinceStartup - temp2) + " sec");
        //FillQueue();
    }

    private int highestPoints()
    {
        int index = 0;
        int maxPoints = 0;
        for (int i = 0; i < _allPoints.Count; i++)
        {
            if(_allPoints[i] > maxPoints)
            {
                maxPoints = _allPoints[i];
                index = i;
            }
        }
        Debug.Log("highscore: " + maxPoints);
        return index;
    }

    private void FillQueue()
    {
        Queue<string> q = new Queue<string>();
        foreach (string s in buildpathes)
        {
            q.Enqueue(s);
        }
    }

    private void Update()
    {
        if(index <= maxIndex)
        {
            Debug.Log(buildpathes[index]);
            
            index++;
        }
    }

    public string[] StartUnitTest()
    {
        GameStats game = new GameStats();

        List<BaseProject> bplist = game.GetBuyAbleBaseProjects();

        return GetPermutation(maxDays, bplist, game);
    }

    private LinkedListNode<Day> FindDay(int day, LinkedList<Day> linkedDays)
    {
        for (LinkedListNode<Day> d = linkedDays.First; d != null; d = d.Next)
        {
            if(d.Value.day == day)
            {
                return d;
            }
        }
        return null;
    }

    private void AddDaytoList(int day, GameStats game, LinkedList<Day> linkedDays)
    {
        Day d = new Day();
       
        d.coins = game.coins;
        d.points = game.points;
        d.citizen = game.citizen;
        d.capacity = game.capacity;
        d.environmentPoints = game.environmentPoints;

        d.WhitehouseLvl = game.whitehouse.projectLevel;
        d.CycletrackLvl = game.cycletrack.projectLevel;
        d.HouseLvl = game.houses.projectLevel;
        d.StreetLvl = game.street.projectLevel;
        d.CarpooltLvl = game.carpool.projectLevel;
        d.StationLvl = game.station.projectLevel;
        d.BusLvl = game.bus.projectLevel;
        d.TrainLvl = game.train.projectLevel;
        d.IndustryLvl = game.industry.projectLevel;

        Dictionary<string, int> newBpList = new Dictionary<string, int>();

        foreach (BaseProject bp in game.baseProjects)
        {
            if(bp.constructing)
            {
                newBpList.Add(bp.projectName, bp.constructionDays);
            }
        }
        d.constructProjects = newBpList;
       
        d.discount = game.discount;
        d.maxBuilder = game.maxBuilder;

        //d.dayStats = game;
        d.day = day;
        linkedDays.AddLast(d);
    }

    private void BrandNewDay(int day, LinkedListNode<Day> currentDay, GameStats game, LinkedList<Day> linkedDays)
    {
        if (currentDay != null)
        {
            // clears all constructions which whod have been build after the prevouise buildpath ended
            game.ClearConstructing();
            // sets the projectlevels of that day
            game.whitehouse.projectLevel = currentDay.Value.WhitehouseLvl;
            game.cycletrack.projectLevel = currentDay.Value.CycletrackLvl;
            game.houses.projectLevel = currentDay.Value.HouseLvl;
            game.street.projectLevel = currentDay.Value.StreetLvl;
            game.carpool.projectLevel = currentDay.Value.CarpooltLvl;
            game.station.projectLevel = currentDay.Value.StationLvl;
            game.bus.projectLevel = currentDay.Value.BusLvl;
            game.train.projectLevel = currentDay.Value.TrainLvl;
            game.industry.projectLevel = currentDay.Value.IndustryLvl;

            // sets the income of that day
            game.coins = currentDay.Value.coins;
            game.points = currentDay.Value.points;
            game.citizen = currentDay.Value.citizen;
            game.capacity = currentDay.Value.capacity;
            game.environmentPoints = currentDay.Value.environmentPoints;

            // sets the industry values of that day
            game.maxBuilder = currentDay.Value.maxBuilder;
            game.builder = game.maxBuilder;
            game.discount = currentDay.Value.discount;

            // sets the constructiondays of the constructing projects of that day       
            for (int i = 0; i < currentDay.Value.constructProjects.Count; i++)
            {
                foreach (BaseProject gbp in game.baseProjects)
                {
                    if ( currentDay.Value.constructProjects.ContainsKey(gbp.projectName) )
                    {
                        gbp.constructing = true;
                        gbp.constructionDays = currentDay.Value.constructProjects[gbp.projectName];
                    }
                }
            }
            
            // clear all the new days ahead
            while (currentDay != null)
            {
                var next = currentDay.Next;

                linkedDays.Remove(currentDay);
                currentDay = next;
            }

            // save the events of that day
            AddDaytoList(day, game, linkedDays);
        }
        else
        {
            // save the events of that day
            AddDaytoList(day, game, linkedDays);
        }
    }


    public string[] GetPermutation(int days, List<BaseProject> projects, GameStats game)
    {
        // Eine neue, leere ArrayList generieren, an die alle Möglichkeiten angehängt werden
        List<string> output = new List<string>();
        List<int> allPoints = new List<int>();

        LinkedList<Day> linkedDays = new LinkedList<Day>();

        GetPermutationPerRef(days, projects, output, allPoints, game, linkedDays);

        _allPoints = allPoints;
        // Das Ergebnis in einen string[] umwandeln und zurückgeben
        return output.ToArray();
    }

    /// <summary>
    /// Generiert ein Array, aus Elementen die jeweils aus 'chars' unterschiedlichen Zeichen bestehen, mit jeweils 'places' Stellen.
    /// Das Array beinhaltet alle möglichen Verknüpfungsmöglichkeiten, die durch Permutation ermittelt werden.
    /// Das Ergebnis wird in der als Referenz übergebenen ArrayList 'output' gespeichert.
    /// </summary>
    /// <param name="places">Anzahl der Stellen jedes Elements</param>
    /// <param name="chars">Array von Zeichen die benutzt werden dürfen</param>
    /// <param name="output">ArrayList in die alle Möglichkeiten hinzugefügt werden</param>
    /// <param name="outputPart">Optionaler interner Parameter, zur Weitergabe der Informationen während des rekursiven Vorgangs</param>
    private void GetPermutationPerRef(int days, List<BaseProject> projects, List<string> output, List<int> allPoints, GameStats game, LinkedList<Day> linkedDays, string outputPart = "")
    {
        //Debug.Log("coins:" + game.overseer .coins + " points: " + stats.points + " builder: "+ stats.builder);
        if (days == 0)
        {
            // Wenn die Anzahl der Stellen durchgerechnet wurde,
            // wird der sich ergebende string (Element) an die Ausgabe angehängt.
            //Debug.Log("_--------------------------_");
            allPoints.Add(game.points);
            output.Add(outputPart);
        }
        else
        {
            if (projects.Count > 0)
            {
                // Für die Stelle rechts im Element, werden alle Zeichenmöglichkeiten durchlaufen
                foreach (BaseProject p in projects)
                {
                    if(days == maxDays)
                    {
                        game.Reset();
                        linkedDays.Clear();
                    }
                    //Debug.Log("before coins: " + game.coins);
                    // check if the day already exists
                    LinkedListNode<Day> currentDay = FindDay(days, linkedDays);
                    BrandNewDay(days, currentDay, game, linkedDays);

                    if (days == (maxDays - barrierer0) && game.points <= pointsBarrierer0)
                    {
                        // ende
                    }
                    else if(days == (maxDays - barrierer1) && game.points <= pointsBarrierer1)
                    {
                        // ende
                        //Debug.Log("points: "+ game.points+" : "+barrierer);
                    }
                    else if (days == (maxDays - barrierer2) && game.points <= pointsBarrierer2)
                    {
                        // ende
                    }
                    else if (days == (maxDays - barrierer3) && game.points <= pointsBarrierer3)
                    {
                        // ende
                    }
                    else
                    {
                        // construct the current project
                        game.BuyProject(p);
                        
                        // move on to the next day
                        game.nextDay();
                       
                        // find all the projects the player could buy the next day
                        projects = game.GetBuyAbleBaseProjects();

                        GetPermutationPerRef(days - 1,
                            projects,
                            output,
                            allPoints,
                            game,
                            linkedDays,
                            outputPart + p.projectName + " ");
                    }
                }
            }
            else
            {
                if (days == maxDays)
                {
                    game.Reset();
                    linkedDays.Clear();
                }

                if (days == (maxDays - 10) && game.points <= barrierer0)
                {
                    // ende
                }
                else if (days == (maxDays - 15) && game.points <= barrierer1)
                {
                    // ende
                    //Debug.Log("points: "+ game.points+" : "+barrierer);
                }
                else if (days == (maxDays - 20) && game.points <= barrierer2)
                {
                    // ende
                }
                else if (days == (maxDays - 25) && game.points <= barrierer3)
                {
                    // ende
                }
                else
                {
                    // check if the day already exists
                    LinkedListNode<Day> currentDay = FindDay(days, linkedDays);
                    BrandNewDay(days, currentDay, game, linkedDays);

                    // dont buy or upgrade any project
                    
                    game.nextDay();
                   
                    projects = game.GetBuyAbleBaseProjects();

                    GetPermutationPerRef(days - 1,
                        projects,
                        output,
                        allPoints,
                        game,
                        linkedDays,
                        outputPart + "empty ");
                }

               
            }
        }
    }
    
}
