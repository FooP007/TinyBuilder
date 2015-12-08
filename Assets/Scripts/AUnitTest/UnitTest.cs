using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using Facet.Combinatorics;

public class UnitTest : MonoBehaviour
{
    public GameObject tinyBuilder;
    private Game mainGame;
    private Variations<Project> variations;

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

    public Queue<string> jobqueue;
    private string[] buildpathes;
    private int index = 0;
    private int maxIndex = -1;
    private List<int> _allPoints;

    // Use this for initialization
    void Awake ()
    {
        mainGame = tinyBuilder.GetComponent<Game>();
        
        
        float temp = Time.realtimeSinceStartup;
        
        buildpathes = StartUnitTest();
        Debug.Log("Fix Time: " + (Time.realtimeSinceStartup - temp) + " sec");
        Debug.Log("possibilities: " + buildpathes.Length);
        Debug.Log("count of points: " + _allPoints.Count);
        maxIndex = buildpathes.Length - 1;

        int t = highestPoints();
        Debug.Log("index: " + t);
        Debug.Log("materpath: "+buildpathes[t]);
        /*
        jobqueue = FillQueue(buildpathes[t]);

        mainGame.StartJobqueue();
        */
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

    private Queue<string> FillQueue(string bestBuildPath)
    {
        Queue<string> q = new Queue<string>();
        int index = 0;
        string parts = bestBuildPath;

        for (int i = 0; i < maxDays; i++)
        {
            string part = parts.Substring (index, parts.IndexOf(" "));
            parts = parts.Remove(index, parts.IndexOf(" ") + 1);
           
            q.Enqueue(part);
        }
       
        return q;
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
        List<string> output = new List<string>();
        List<int> allPoints = new List<int>();

        LinkedList<Day> linkedDays = new LinkedList<Day>();

        GetPermutationPerRef(days, projects, output, allPoints, game, linkedDays);

        _allPoints = allPoints;
        
        return output.ToArray();
    }

    /// <summary>
    /// Generiert ein Array, aus Elementen die jeweils aus 'chars' unterschiedlichen Zeichen bestehen, mit jeweils 'places' Stellen.
    /// Das Array beinhaltet alle möglichen Verknüpfungsmöglichkeiten, die durch Permutation ermittelt werden.
    /// Das Ergebnis wird in der als Referenz übergebenen ArrayList 'output' gespeichert.
    /// </summary>
    /// <param name="days">Count of how many days the player would have time to build</param>
    /// <param name="projects">List of the available projects</param>
    /// <param name="output">List in which all the buildpathes will be stored</param>
    /// <param name="allPoints">List in which all the points of each buildpath will be stored</param>
    /// <param name="game">Game holds all the values to provide a environment where project can be build</param>
    /// <param name="outputPart" >Internally parameter to pass on the information during the recursiv progress</param>
    private void GetPermutationPerRef(int days, List<BaseProject> projects, List<string> output, List<int> allPoints, GameStats game, LinkedList<Day> linkedDays, string outputPart = "")
    {
        //Debug.Log("coins:" + game.overseer .coins + " points: " + stats.points + " builder: "+ stats.builder);
        if (days == 0)
        {
            // If all the days are through,
            // the buildpath is added to a List of buildpathes
            // and the score of that buildpath is added to the scorelist
            allPoints.Add(game.points);
            output.Add(outputPart);
        }
        else
        {
            if (projects.Count > 0) // there are projects which can be build
            {
                // for each new baseproject there is a complete new buildpath
                foreach (BaseProject p in projects)
                {
                    // because the days are getting counted down every new buildpath starts with the maximum count of days
                    // the game environment gets reset as do the linkedlist of days
                    if (days == maxDays)
                    {
                        game.Reset();
                        linkedDays.Clear();
                    }
                    
                    // check if the day already exists
                    LinkedListNode<Day> currentDay = FindDay(days, linkedDays);

                    BrandNewDay(days, currentDay, game, linkedDays);

                    // the barriers prevent not promising buildpathes from being furhter pursued
                    // so that the numbers of possible buildpathes dont get out of hand
                    if (days == (maxDays - barrierer0) && game.points <= pointsBarrierer0)
                    {
                        // end of tail recursion
                    }
                    else if(days == (maxDays - barrierer1) && game.points <= pointsBarrierer1)
                    {
                        // end of tail recursion
                    }
                    else if (days == (maxDays - barrierer2) && game.points <= pointsBarrierer2)
                    {
                        // end of tail recursion
                    }
                    else if (days == (maxDays - barrierer3) && game.points <= pointsBarrierer3)
                    {
                        // end of tail recursion
                    }
                    else
                    {
                        // construct the current project
                        game.BuyProject(p);
                        
                        // move on to the next day
                        game.nextDay();
                       
                        // find all the projects the player could buy the next day
                        projects = game.GetBuyAbleBaseProjects();

                        // tail recursion
                        GetPermutationPerRef(days - 1, projects, output, allPoints, game, linkedDays, outputPart + p.projectName + " ");
                    }
                }
            }
            else // there are no projects which can be build
            {
                if (days == maxDays)
                {
                    game.Reset();
                    linkedDays.Clear();
                }

                if (days == (maxDays - barrierer0) && game.points <= pointsBarrierer0)
                {
                    // ende
                }
                else if (days == (maxDays - barrierer1) && game.points <= pointsBarrierer1)
                {
                    // ende
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
