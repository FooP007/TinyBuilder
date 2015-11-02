using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitTest : MonoBehaviour
{
    public GameObject tinyBuilder;
    private Game mainGame;

    private List<Project[]> buildPaths;
    private Project[] buildPath = new Project[30];

    int maxDays = 30;
	// Use this for initialization
	void Start ()
    {
        mainGame = tinyBuilder.GetComponent<Game>();
        
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
        int randomeIndex = Random.Range(0, projects.Count);
        Project p;

        p = projects[randomeIndex];

        return p;
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
            // check if the project was already bought at that day
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

    public void StartUnitTest2()
    {
        string[] focuse = new string[4] { "placeholderCycleTrack", "PlaceholderIndustry", "placeholderHouses", "placeholderStreet" };
        for (int tries = 0; tries < 4; tries++)
        {
            for (int day = 0; day < maxDays; day++)
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
                //Debug.Log("day: " + day);
                // buy a project if not next day
                mainGame.NextDay();
                
            }

            Debug.Log("GameOver!Points: " + Game.overseer.points + " Focus: " + focuse[tries]);
            mainGame.GameReset();
        }
        
    }

    public void StartUnitTestKombinatorik()
    {
        string[] projects = new string[9] { "Houses", "Street", "Industry", "Whitehouse", "Carpool", "Station", "Bus", "Train", "CycleTrack" };
        string[] startProjects = new string[4] { " Houses", " Street", " Industry", " CycleTrack" };

        string[] result = Kombinatorik.GetPermutation(5, startProjects);

        foreach (string Variation in result)
        {
            Debug.Log(Variation);
        }
    }
            

        /*public void StartUnitTest()
        {
            for (int day = 0; day < maxDays; day++)
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
        }*/
    }
