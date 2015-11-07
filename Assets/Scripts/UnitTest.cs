using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using Facet.Combinatorics;

public class UnitTest : MonoBehaviour
{
    public GameObject tinyBuilder;
    private Game mainGame;

    private List<Project[]> buildPaths = new List<Project[]>();
    private Project[] buildPath;

	// Use this for initialization
	void Awake ()
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

        // string[] result = Kombinatorik.GetPermutation(3, projects);

        ArrayList output = new ArrayList();
       
        foreach (string Variation in output)
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
        List<Project> buildpath;
        List<Project> possibilities;
        List<List<Project>> buildpaths = new List<List<Project>> ();
        List<List<Project>> allPossibilities = new List<List<Project>>();

        int allSolutions = 4;
        for(int i = 0; i < allSolutions; i++)
        {
            int changeAtDay = -1;
            // get last possibilities
            //Debug.Log("allPossibilities: " + allPossibilities.Count);
            for (int a = allPossibilities.Count - 1; a > 0; a--)
            {
                //Debug.Log("proejct: " + allPossibilities[a]);
                if (allPossibilities[a] != null)
                {
                    if (allPossibilities[a].Count > 0)
                    {
                        // buildpath[a].projectName  compare buildpathes with possibilities
                        Debug.Log("changeAtDay: " + a);
                        changeAtDay = a;
                        break;
                    }
                }
            }

            buildpath = new List<Project>();
            bool newPossibilitie = false;
            for (int day = 0; day < mainGame.maxDays; day++)
            {
                possibilities = GetBuyAbleProjects();
                newPossibilitie = false;
                //Debug.Log("day: " + day);
                if (possibilities.Count > 0)
                {
                    // Debug.Log("project: " + possibilities[0].projectName);

                    int index = 0;
                    if (changeAtDay == day)
                    {
                        index = 1;
                        newPossibilitie = true;
                        // dont add new possibilities
                    }
                    //Debug.Log("index: "+ index);

                    string result = "";
                    foreach (Project p in possibilities)
                    {
                        result += p.projectName + " ";
                    }
                    Debug.Log(result);

                    possibilities[index].TryConstructing();
                    buildpath.Add(possibilities[index]);
                    //Debug.Log("index: " + (index + 1) + " possibilities: " + (possibilities.Count ));
                    if(index + 1 != possibilities.Count && !newPossibilitie)
                    {
                        if(allPossibilities.Count > day)
                        {
                            allPossibilities.RemoveAt(day);
                        }
                        allPossibilities.Insert(day, possibilities.GetRange(index + 1, (possibilities.Count - 1)));
                    }
                    else
                    {
                        allPossibilities.RemoveAt(day);
                        allPossibilities.Insert(day, null);
                    }
                }
                else
                {
                    if (allPossibilities.Count > day)
                    {
                        allPossibilities.RemoveAt(day);
                    }
                    allPossibilities.Insert(day, null);
                    buildpath.Add(null);
                }

                // check if builder are available
                if (Game.overseer.builder > 0)
                {
                    UseAvialableBuilder();
                }
                
                mainGame.NextDay();
            }
            string re = "project/s: ";
            for (int k = 0; k < buildpath.Count; k++)
            {
                if (buildpath[k] == null)
                {
                    re += "empty day: " + k + " ";
                }
                else
                {
                    re += buildpath[k].projectName + " day: " + k + " ";
                }


            }

            Debug.Log(re);
            buildpaths.Add(buildpath);
            mainGame.GameReset();
        }


        /*for (int j = 0; j < allPossibilities.Count; j++)
        {
            if(allPossibilities[j] != null && allPossibilities[j].Count > 0)
            {
                string re = "project/s: ";
                for (int k = 0; k < allPossibilities[j].Count; k++)
                {
                    re +=  allPossibilities[j][k].projectName + " ";
                }
                re += " day: " + j;
                Debug.Log(re);
                //allPossibilities[j][0].TryConstructing();
            }
        }

        for (int j = 0; j < buildpaths.Count; j++)
        {
             
            if (buildpaths[j] != null && buildpaths[j].Count > 0)
            {
                string re = "project/s: ";
                for (int k = 0; k < buildpaths[j].Count; k++)
                {
                    if(buildpaths[j][k] == null)
                    {
                        re +=  "empty day: " + k + " ";
                    }
                    else
                    {
                        re += buildpaths[j][k].projectName + " day: " + k + " ";
                    }

                    
                }
                
                Debug.Log(re);
                //allPossibilities[j][0].TryConstructing();
            }
        }*/

        // Debug.Log(result);
        // day: 0 possibilities: { Street, Houses, CycleTrack, Industry } Chose: Street
        // day: 1 possibilities: { }
        // day: 2 possibilities: { Carpool, Houses } Chose: Carpool
        // day: 3 possibilities: { }
        // day: 4 possibilities: { Whitehouse, Houses } Chose: Whitehouse
    }

    public void StartUnitTest5()
    {
        List<Project> buildpath;
        List<Project> possibilities;
        List<List<Project>> buildpaths = new List<List<Project>>();
        List<List<Project>> allPossibilities = new List<List<Project>>();

        for (int day = 0; day < mainGame.maxDays; day++)
        {
            possibilities = GetBuyAbleProjects();
            buildpath = new List<Project>();

            // add possibilities to allpossibilities, only add new possibilities
            //allPossibilities.Insert(day, possibilities);

            // try first possibilitie and add them to the buildpath
            if (possibilities.Count > 0)
            {
                possibilities[0].TryConstructing();
                buildpath.Add(possibilities[0]);
                if(possibilities.Count > 1)
                {
                    Debug.Log("add possibilities");
                    allPossibilities.Add(possibilities.GetRange(1, (possibilities.Count - 1)));
                }
                else
                {
                    allPossibilities.Add(null);
                }
            }
            else
            {
                buildpath.Add(null);
            }
            

            // buy everything exactly the same but change the last project which has a different possibilities

            // check if builder are available
            if (Game.overseer.builder > 0)
            {
                UseAvialableBuilder();
            }

            mainGame.NextDay();
        }


        for (int j = 0; j < allPossibilities.Count; j++)
        {
            if (allPossibilities[j] != null && allPossibilities[j].Count > 0)
            {
                string re = "project/s: ";
                for (int k = 0; k < allPossibilities[j].Count; k++)
                {
                    re += allPossibilities[j][k].projectName + " ";
                }
                
                Debug.Log(re);
                //allPossibilities[j][0].TryConstructing();
            }
        }

        ArrayList output = new ArrayList();
        GoThroughList(allPossibilities, ref output);
       
        foreach (List<Project> proList in output )
        {
            foreach (Project p in proList)
            {
                Debug.Log("Project: " + p.projectName);
            }
        }
    }

    private void GoThroughList(List<List<Project>> poss, ref ArrayList output, List<Project> outputPart = null)
    {
        Debug.Log("Count1: " + poss.Count);
        if (poss.Count == 0)
        {
            Debug.Log("Fertig!");
        }
        else
        {
            outputPart = new List<Project>();
            for (int j = poss.Count-1; j >= 0; j--)
            {
                if (poss[j] != null && poss[j].Count > 0)
                {
                    for (int k = poss[j].Count-1; k >= 0; k--)
                    {
                        outputPart.Add(poss[j][k]);
                        poss[j].Remove(poss[j][k]);
                        break;
                    }
                }
            }
            outputPart.Reverse();
            output.Add(outputPart);
            Debug.Log("Count2: " + poss.Count);
            //GoThroughList(poss, ref output);
        }
    }

    public void StartUnitTest6()
    {
        List<Project> l = GetBuyAbleProjects();
        Debug.Log("BuyAbleProjects: " + l.Count);
        Variations <Project> variations = new Variations<Project>(l, 3, GenerateOption.WithRepetition);
        int count = (int)variations.Count;
        Debug.Log("count: " + count);
        //Variations<int> variations = new Variations<int>(ints, cofs.Count);
        /*string[] output = GetPermutationIterative(mainGame.maxDays, GetBuyAbleProjects());

        foreach (string Variation in output)
        {
            Debug.Log(Variation);
        }*/


    }

    private string[] GetPermutationIterative(int places, List<Project> projects)
    {
        ArrayList output = new ArrayList();
        int count = 0;
       
        while (places != 0)
        {
            string outputPart = "";
            if (projects.Count > 0)
            {
                foreach (Project p in projects)
                {
                    p.TryConstructing();
                   
                    mainGame.NextDay();
                    outputPart += p.projectName + " ";
                    projects = GetBuyAbleProjects();
                }
            }
            else
            {
                //outputPart.Add(null);
                outputPart += "empty ";
                mainGame.NextDay();
                projects = GetBuyAbleProjects();
            }
            output.Add(outputPart);
            places--;
        }

        //GetPermutationPerRefIterative(places, projects, ref output, ref count);

        return output.ToArray(typeof(string)) as string[];
    }

    private void GetPermutationPerRefIterative(int places, List<Project> projects, ref ArrayList output, ref int count, string outputPart = "")
    {

    }

    public string[] GetPermutationRecursive(int places, List<Project> projects)
    {
        // Eine neue, leere ArrayList generieren, an die alle Möglichkeiten angehängt werden
        ArrayList output = new ArrayList();
        
        int count = 0;

        GetPermutationPerRef(places, projects, ref output, ref count);
        Debug.Log("possibilities: " + count);
        // Das Ergebnis in einen string[] umwandeln und zurückgeben
        return output.ToArray(typeof(string)) as string[];
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
    /// 
    private void GetPermutationPerRef(int places, List<Project> projects, ref ArrayList output, ref int count, string outputPart = "")
    {
        //Debug.Log("places: " + places);
        //outputPart = new List<Project>();
        if (places == 0)
        {
            // Wenn die Anzahl der Stellen durchgerechnet wurde,
            // wird der sich ergebende string (Element) an die Ausgabe angehängt.
            outputPart += " points: " + Game.overseer.points + " coins: " + Game.overseer.coins; 
            output.Add(outputPart);
            count++;
            //outputPart = new List<Project>();
            //Debug.Log("points: " +Game.overseer.points); 
            mainGame.GameReset();
            //Debug.Log("out: "+outputPart);
        }
        else
        {
            if(projects.Count > 0)
            {
                // Für die Stelle rechts im Element, werden alle Zeichenmöglichkeiten durchlaufen
                foreach (Project p in projects)
                {
                    //Debug.Log("project: " + p.projectName);
                    //outputPart.Add(p);
                    p.TryConstructing();
                    //Debug.Log("coins vor: " + Game.overseer.coins);
                    mainGame.NextDay();
                    //Debug.Log("coins danach: " + Game.overseer.coins);
                    projects = GetBuyAbleProjects();
                    
                    string result = "";
                    foreach (Project p2 in projects)
                    {
                        if (p2 != null)
                        {
                            result += p2.projectName + " ";
                        }
                        else
                        {
                            result += "result ";
                        }
                    }
                    //Debug.Log(result);
                   

                    // Danach wird für jedes dieser Zeichen, basierend auf der Anzahl der Stellen, wieder ein neuer
                    // foreach-Vorgang begonnen, der alle Zeichen der nächsten Stelle hinzufügt

                    GetPermutationPerRef(places - 1,    // Die Stellen Anzahl wird verwindert, bis 0
                        projects,                       // Benötigte Variablen werden
                        ref output,                     // mitübergeben
                        ref count,                  
                        outputPart + p.projectName+" ");                 // An diesen letzen string werden alle anderen Stellen angehängt
                }
            }
            else
            {
                mainGame.NextDay();
                projects = GetBuyAbleProjects();

                // Danach wird für jedes dieser Zeichen, basierend auf der Anzahl der Stellen, wieder ein neuer
                // foreach-Vorgang begonnen, der alle Zeichen der nächsten Stelle hinzufügt
                GetPermutationPerRef(places - 1,    // Die Stellen Anzahl wird verwindert, bis 0
                    projects,                       // Benötigte Variablen werden
                    ref output,
                    ref count,                      // mitübergeben
                    outputPart + "empty ");          // An diesen letzen string werden alle anderen Stellen angehängt
            }
        }
    }
}
