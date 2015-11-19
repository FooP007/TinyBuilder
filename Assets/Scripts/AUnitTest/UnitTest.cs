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
    private int possibilities = 0;

    // Use this for initialization
    void Awake ()
    {
        mainGame = tinyBuilder.GetComponent<Game>();
        //buildPath = new Project[mainGame.maxDays];
        buildPaths.Add(buildPath);
    }

    void Start()
    {
        //Debug.Log("dac: " + factorial(6, 1));
        //Debug.Log("tail: " + tailRecursive(2));
        // /*
       float temp = Time.realtimeSinceStartup;
       StartUnitTest();
       Debug.Log("Time: " + (Time.realtimeSinceStartup - temp) + " sec");
       // */
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

    private void UseAvialableBuilder()
    {
        if (Game.overseer.builder > 0)
        {
            foreach (Project p in Game.projects)
            {
                if (p.allBuilder.Count > 0)
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

    // day: 0 possibilities: { Street, Houses, CycleTrack, Industry } Chose: Street
    // day: 1 possibilities: { }
    // day: 2 possibilities: { Carpool, Houses } Chose: Carpool
    // day: 3 possibilities: { }
    // day: 4 possibilities: { Whitehouse, Houses } Chose: Whitehouse

    private int factorial(int x, int fac)
    {
        if (x == 1)
        {
            Debug.Log("fac: " + fac);
            return fac;
        }
        else
        {
            Debug.Log(x*fac);
            return factorial(x - 1, x * fac);
        } 
    }

    public int tailRecursive(int days)
    {
        
        if (days <= 2)
        {
            
            return 1;
        }
       
        return tailRecursiveAux(days, 1, 1);
    }

    private int tailRecursiveAux(int days, int iter, int acc)
    {
        if (iter == days)
        {
           
            return acc;
        }
       
        return tailRecursiveAux(days, ++iter, acc + iter);
    }


    public void StartUnitTest()
    {
        GameStats game = new GameStats();

        Debug.Log(game.coins);
        List<BaseProject> bplist = game.GetBuyAbleBaseProjects();
        string[] output = GetPermutation(10, bplist, ref possibilities, game);
        Debug.Log("possibilities: " + possibilities);

        /*foreach (string Variation in output)
        {
            Debug.Log(Variation);
        }
        */
    }

    private string printList(List<Project> list)
    {
        string result = "";
        foreach (Project p in list)
        {
            result += p.projectName + " ";
        }
        return result;
    }

    public string[] GetPermutation(int days, List<BaseProject> projects, ref int possibilities, GameStats game)
    {
        // Eine neue, leere ArrayList generieren, an die alle Möglichkeiten angehängt werden
        ArrayList output = new ArrayList();
        
        GetPermutationPerRef(days, projects, ref output, ref possibilities, game);
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
    private void GetPermutationPerRef(int days, List<BaseProject> projects, ref ArrayList output, ref int possibilities, GameStats game, string outputPart = "")
    {
        //Debug.Log("places: " + places);
        //outputPart = new List<Project>();
        Debug.Log("points: " + game.points); 
        Debug.Log(outputPart);
        //Debug.Log("coins:" + game.overseer .coins + " points: " + stats.points + " builder: "+ stats.builder);
        if (days == 0)
        {
            // Wenn die Anzahl der Stellen durchgerechnet wurde,
            // wird der sich ergebende string (Element) an die Ausgabe angehängt.
            outputPart += " points: " ; 
            output.Add(outputPart);
            // add 1 to the amount of possible buildpathes
            possibilities++;
            // reset all values for the next buildpath
            //mainGame.GameReset();
            Debug.Log("next buildpath");
        }
        else
        {
            if(projects.Count > 0)
            {
                // Für die Stelle rechts im Element, werden alle Zeichenmöglichkeiten durchlaufen
                foreach (BaseProject p in projects)
                {
                    // construct the current project
                    game.BuyProject(p);
                    //p.TryConstructing();
                    // use all available builder
                    //UseAvialableBuilder();
                    // move on to the next day
                    //mainGame.NextDay();
                    // find all the projects the player could buy the next day
                    projects = game.GetBuyAbleBaseProjects();

                    // Danach wird für jedes dieser Zeichen, basierend auf der Anzahl der Stellen, wieder ein neuer
                    // foreach-Vorgang begonnen, der alle Zeichen der nächsten Stelle hinzufügt
                    GetPermutationPerRef(days - 1,          // Die Stellen Anzahl wird verwindert, bis 0
                        projects,                            // Benötigte Variablen werden
                        ref output,                          // mitübergeben
                        ref possibilities,
                        game,                  
                        outputPart + p.projectName +" "/* " day "+days+" "" coins: " + money + " : "+ p.Cost()+ " paid "*/);    // An diesen letzen string werden alle anderen Stellen angehängt
                }
            }
            else
            {
                // dont buy or upgrade any project
                //Debug.Log("empty ");
                //UseAvialableBuilder();
                //mainGame.NextDay();
                projects = game.GetBuyAbleBaseProjects();

                GetPermutationPerRef(days - 1,
                    projects,
                    ref output,
                    ref possibilities,
                    game,
                    outputPart + "empty "/*day " + days + " "*/);
            }
        }
    }
}
