using UnityEngine;
using System.Collections;

public class UnitTest : MonoBehaviour
{
    public GameObject tinyBuilder;
    private Game mainGame;

    int maxDays = 30;
	// Use this for initialization
	void Start ()
    {
        mainGame = tinyBuilder.GetComponent<Game>();
        
    }

    private Project GetBuyAbleProject()
    {
       
        foreach (Project p in Game.projects)
        {
            Debug.Log("can buy "+ p.name+" "+Game.overseer.CanBuyProject(p, p.constructing));
            if (Game.overseer.CanBuyProject(p, p.constructing))
            {
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

    public void StartUnitTest()
    {
        for (int day = 0; day < maxDays; day++)
        {
            Debug.Log("day: " + day);
            // look for project he can buy
            if (GetBuyAbleProject() != null)
            {
                Debug.Log("buy project: " + GetBuyAbleProject().name);
                GetBuyAbleProject().TryConstructing();
            }
            // check if builder are available

            if(Game.overseer.builder > 0)
            {
                UseAvialableBuilder();
            }
            // buy a project if not next day
            mainGame.NextDay();
        }
    }
}
