using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitTest : MonoBehaviour
{
    public GameObject tinyBuilder;
    private Game mainGame;

    private List<Project[]> buildPaths = new List<Project[]>();
    private Project[] buildPath;

	// Use this for initialization
	void Start ()
    {
       
        mainGame = tinyBuilder.GetComponent<Game>();
        buildPath = new Project[mainGame.maxDays];
        buildPaths.Add(buildPath);

    }

    private bool HasFocus(List<Project> projects, Project focus)
    {
        foreach (Project p in projects)
        {
            if (p.projectName == focus.projectName)
            {
                return true;
            }
        }
        return false;
    }

    private Project GetFocusProject(List<Project> projects, Project focus)
    {
        foreach (Project p in projects)
        {
            if (p.projectName == focus.projectName)
            {
                return p;
            }
        }
        return null;
    }

    private Project GetRandomProject(List<Project> projects)
    {
        if(projects.Count >= 1)
        {
            int randomeIndex = Random.Range(0, projects.Count);
            Project p;
            //Debug.Log("randomeIndex: " + randomeIndex);
            p = projects[randomeIndex];

            return p;
        }
        return null;
        
    }

    private Project ChoseProject(Project focus)
    {
        Project curP = null;
        List < Project > pros = GetBuyAbleProjects();

        if (HasFocus(pros, focus))
        {
            curP = GetFocusProject(pros, focus);
        }
        else
        {
            curP = GetRandomProject(pros);
        }

        return curP;
    }

    private List<Project> GetBuyAbleProjects()
    {
        List<Project> bAprojects = new List<Project>(); // buyAblePorjects
    
        foreach (Project p in Game.projects)
        {
            // check if the project is buyable
            if (Game.overseer.CanBuyProject(p, p.constructing))
            {
                bAprojects.Add(p);
            }
        }
        return bAprojects;
    }

    private Project GetBuyAbleProject(int day)
    {
        foreach (Project p in Game.projects)
        {
            // check if the project was already bought at that day
            if (Game.overseer.CanBuyProject(p, p.constructing))
            {
                buildPath[day] = p;
                return p;
            }
        }
        return null;
    }

    private Project CompareLists(int day, int month)
    {
        bool inList = false;
        Project p = null;
        List<Project> buyableProjects = GetBuyAbleProjects();
        string result = "";
        for (int j = 0; j < buyableProjects.Count; j++)
        {
            
            if (buyableProjects[j] != null)
            {
                result += buyableProjects[j].projectName + " ";
            }
            else
            {
                result += "empty ";
            }
        }
        result += " coins: " + Game.overseer.coins;
        Debug.Log(result);


        // go through all project tthe player could buy this day
        for (int j = 0; j < buyableProjects.Count; j++)
        {

            // go through all the buildingpathes which are already used
            for (int i = 0; i < buildPaths.Count; i++)
            {
                // check if the buyable project is already in one of the buildingpathes 
                if (buyableProjects[j] == buildPaths[i][day])
                {
                    inList = true;
                    break;
                }
            }
            if (!inList)
            {
                AddProjectToBuildpath(month, day, buyableProjects[j]);
                return buyableProjects[j];
            }
            inList = false;
        }
       
        

        //Debug.Log("buyProjects: " + buyProjects);
        if (buyableProjects.Count >= 1)
        {
            //Debug.Log("buildpath full: ");
            p = buyableProjects[0];
        }
       
        // add project into the list if at that day i wasnt bought before
        AddProjectToBuildpath(month, day, p);
        return p;
        // Debug.Log("buildPaths.Count: " + buildPaths.Count);
    }

    private void AddProjectToBuildpath(int count, int day, Project p)
    {
        // Debug.Log("buildpath: " + buildPaths[count][day] + " count: "+ count + " day: " + day);
        // already built in a prevoiuse path
       
        buildPaths[count][day] = p;
    }

    private void UseAvialableBuilder()
    {
        foreach (Project p in Game.projects)
        {
            if (p.allBuilder.Count > 0)
            {
                if (Game.overseer.builder > 0)
                {
                    p.UseBuilder();
                }
            }
        }
    }

    public void StartUnitTestKombinatorik()
    {
        string[] projects = new string[9] { "Houses", "Street", "Industry", "Whitehouse", "Carpool", "Station", "Bus", "Train", "CycleTrack" };
        string[] startProjects = new string[5] { " Houses", " Street", " Industry", " CycleTrack", " empty" };

        string[] result = Kombinatorik.GetPermutation(5, startProjects);

        foreach (string Variation in result)
        {
            Debug.Log(Variation);
        }
    }


    public void StartUnitTest()
    {
        for (int day = 0; day < mainGame.maxDays; day++)
        {
            // look for project he can buy
            if (GetBuyAbleProject(day) != null)
            {
                Debug.Log("day: " + day + " bougth project: " + GetBuyAbleProject(day).projectName);
                GetBuyAbleProject(day).TryConstructing();
            }

            // check if builder are available
            if(Game.overseer.builder > 0)
            {
                UseAvialableBuilder();
            }
            // buy a project if not next day
            mainGame.NextDay();
        }

        buildPaths[0] = buildPath;
    }

    public void StartUnitTest2()
    {
        string[] focuse = new string[4] { "placeholderCycleTrack", "PlaceholderIndustry", "placeholderHouses", "placeholderStreet" };
        for (int tries = 0; tries < 4; tries++)
        {
            for (int day = 0; day < mainGame.maxDays; day++)
            {
                if (GetBuyAbleProject(day) != null)
                {
                    ChoseProject(GameObject.Find(focuse[tries]).GetComponent<Project>()).TryConstructing();
                }

                // check if builder are available
                if (Game.overseer.builder > 0)
                {
                    UseAvialableBuilder();
                }
                // Debug.Log("day: " + day);
                // buy a project if not next day
                mainGame.NextDay();
            }

            Debug.Log("GameOver!Points: " + Game.overseer.points + " Focus: " + focuse[tries]);
            mainGame.GameReset();
        }
    }

    public void StartUnitTest3()
    {
        int month = 5;
        for (int i = 0; i < month; i++)
        {
            for (int day = 0; day < mainGame.maxDays; day++)
            {
                // Debug.Log("day: " + day);
                Project p = CompareLists(day, i);
                if (p != null)
                {
                    p.TryConstructing();
                    Debug.Log("Bought: " + p.projectName + " for: " + p.Cost());
                }

                // check if builder are available
                if (Game.overseer.builder > 0)
                {
                    UseAvialableBuilder();
                }

                mainGame.NextDay();
            }

            buildPaths.Add(new Project[mainGame.maxDays]);

            Debug.Log("GameOver! Points: " + Game.overseer.points);
            string result = "";
            for (int j = 0; j < buildPaths.Count; j++)
            {
                foreach (Project p in buildPaths[j])
                {
                    if (p != null)
                    {
                        result += p.projectName + " ";
                    }
                    else
                    {
                        result += "empty ";
                    }
                }
                result += "\n new :";
            }
            Debug.Log(result);

            mainGame.GameReset();
        }
    }

    public void StartUnitTest4()
    {
        List<Project> buildpath = new List<Project>();
        List<List<Project>> allPossibilities = new List<List<Project>>();

        for (int day = 0; day < mainGame.maxDays; day++)
        {
            
            List<Project> possibilities = GetBuyAbleProjects();
            /*
            string result = "";
            foreach (Project p in possibilities)
            {
                result += p.projectName + " ";
                
            }
            Debug.Log(result);
            */

            if (possibilities.Count > 0)
            {
                possibilities[0].TryConstructing();
                buildpath.Add(possibilities[0]);
               
                allPossibilities.Add(possibilities.GetRange(1, (possibilities.Count-1)));
            }
            else
            {
                allPossibilities.Add(null);
            }

            // check if builder are available
            if (Game.overseer.builder > 0)
            {
                UseAvialableBuilder();
            }

            mainGame.NextDay();
        }
        mainGame.GameReset();
       

        for(int j = 0; j < allPossibilities.Count; j++)
        {
            if(allPossibilities[j] != null && allPossibilities[j].Count > 0)
            {
                allPossibilities[j][0].TryConstructing();
            }
        }

        // Debug.Log(result);
        // day: 0 possibilities: { Street, Houses, CycleTrack, Industry } Chose: Street
        // day: 1 possibilities: { }
        // day: 2 possibilities: { Carpool, Houses } Chose: Carpool
        // day: 3 possibilities: { }
        // day: 4 possibilities: { Whitehouse, Houses } Chose: Whitehouse
    }
}
