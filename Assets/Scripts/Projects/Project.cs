using UnityEngine;
using System.Collections.Generic;

public class Project : MonoBehaviour 
{
	public TextMesh clockText;
	protected Project dependence;
	public int projectLevel = 0;
	public Sprite[] projectSprites;
	public Sprite[] placeholderSprites;

	protected int[] costs = new int[1];
	protected int[] capacities = new int[1];
	protected int[] buildingRounds = new int[1];
	protected int[] requiredPoints = new int[1];
	protected int[] requiredWhitehouse = new int[1];

	protected GameObject project;
	protected string _projectName;
    protected string _effectText;
    protected string _requireText;
    protected string _upgradeWindowResource = "UpgradeWindow";
	
	protected bool _constructing = false;
	protected int _constructionDays = 0;
	protected int outOfRange = -1;
	protected float _offset = 1.1f;

    private GameObject upgradeWindow;
    private UpgradeWindow uwScript;
    private GameObject builder;
    public List<GameObject> allBuilder = new List<GameObject>();

    protected void StartConstructing()
	{
        Debug.Log("Porject: " + name);
        _constructionDays = Rounds();
		if(_constructionDays >= 0)
		{
            _constructing = true;
			clockText.text = _constructionDays.ToString();
            if(_constructionDays == 0)
            {
                Construct();
            }
		}
	}

    private void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                
                if (uwScript != null)
                {
                    if (hit.collider.gameObject == uwScript.upgradeButton)
                    {
                        TryConstructing();
                    }

                    if (hit.collider.gameObject == uwScript.closeButton)
                    {
                        CloseUpgradeWindow();
                    }
                }

                if(builder != null)
                {
                    if (hit.collider.gameObject == builder)
                    {

                        UseBuilder();
                    }
                }
                else
                {
                    // Debug.Log("builder ist null");
                }
                
            }
        }
    }

    public void UseBuilder()
    {
        allBuilder.Remove(builder);
        DestroyObject(builder);
        Construct();
        Game.overseer.BuilderUsed();
        if (allBuilder.Count >= 1)
        {
            builder = allBuilder[allBuilder.Count - 1];

        }
    }

    public void Construct()
	{
		if(_constructing)
		{
            constructionDays--;
			clockText.text = _constructionDays.ToString();
			if(_constructionDays == 0)
			{
				Upgrade();
				projectLevel++;
                Game.UpdateUpgradeWindows();
                // remove all builder
                foreach (GameObject b in allBuilder)
                {
                    Destroy(b);
                }

                for (var i = 0; i < allBuilder.Count; i++)
                {
                    allBuilder.RemoveAt(i);
                }

                FillSpriteRenderer();
                _constructionDays = 0;
                _constructing = false;
			}
		}
	}

	public void Initiate()
	{
		int range = projectLevel;

		for(int i = 0; i < range; i++)
		{
			projectLevel = i;
			Upgrade();

		}
		projectLevel = range;
	}

	protected virtual void Upgrade()
	{   }

    protected void UpdateText(int inputLevel, string[] effects, int[][] effectNumbers, string required, int[]requiredNumber, bool isValue = true)
    {
        // costs.length == maximum level
        if (inputLevel >= costs.Length)
        {
            effectText = "Maximum Upgrade";
            requireText = "Maximum Upgrade";
        }
        else
        {
            if(isValue)
            {
                effectText = null;
               
                if (effects.Length <= effectNumbers[0].Length)
                {
                    for (int i = 0; i < effects.Length; i++)
                    {
                        effectText += "+ " + effectNumbers[i][inputLevel] + effects[i] + "\n";
                    }
                }
                else
                {
                    Debug.Log("the array effectNumbers dont have enough members! effects: " + effects.Length + " members - effectNumbers: " + effectNumbers[0].Length + " members");
                }
                
            }
            else
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    // the effect is the upgrade od this project
                    if (effectNumbers[0].Length < 2)
                    {
                        effectText = effects[i] + effectNumbers[0][0];
                    }
                    else
                    {
                        effectText = effects[i] + effectNumbers[i][inputLevel];
                    }
                }
            }

            if (requiredNumber.Length < 2)
            {
                requireText = required + requiredNumber[0];
            }
            else
            {
                requireText = required + requiredNumber[inputLevel];
            }
        }
    }

    public virtual bool MetRequirements()
	{
		if(Game.overseer.points >= Points())
		{
			return true;
		}
		else
		{
			Debug.Log ("Not enough points! Your points: " + Game.overseer.points +". Points needed: " + Points());
			return false;
		}
	}

	public void TryConstructing()
	{
		if(Rounds() != outOfRange)
		{
			if(Game.overseer.CanBuyProject(this, constructing))
			{
                Game.overseer.coins -= (Cost() - Game.overseer.discount);
                StartConstructing();
				CloseUpgradeWindow();
			}
		}
		else
		{
			Debug.Log ("Project is at maximum upgrade!");
		}
	}

	protected void OnMouseDown()
	{
		if(!_constructing)
		{
			if(upgradeWindow == null)
            {
                upgradeWindow = (GameObject)Instantiate(Resources.Load(upgradeWindowResource), (transform.position + new Vector3(0, offset, 0)), Quaternion.identity);
                uwScript = upgradeWindow.GetComponent<UpgradeWindow>();
                uwScript.Show(this);
            }
            else
            {
                CloseUpgradeWindow();
            }
		}
	}

    public void CloseUpgradeWindow()
    {
        if(upgradeWindow != null)
        {
            Destroy(upgradeWindow);
        }
    }

    public void AddBuilder(int count)
    {
        builder = (GameObject)Instantiate(Resources.Load("Builder"), transform.position + new Vector3(0 + (0.6f * count), 0.5f, 0), Quaternion.identity);
        allBuilder.Add(builder);
    }

    public int constructionDays
    {
        get { return _constructionDays; }
        set { _constructionDays = value;
              if (_constructionDays < 0)
              {
                _constructionDays = 0;
              }
            }
    }

    public bool constructing
    {
        get { return _constructing; }
    }

    public float offset
	{
		get{ return _offset; }
		set{ _offset = value; }
	}

	public string upgradeWindowResource
	{
		get{ return _upgradeWindowResource; }
		set{ _upgradeWindowResource = value; }
	}

    public string requireText
    {
        get { return _requireText; }
        set { _requireText = value; }
    }

    public string effectText
	{
		get{ return _effectText; }
		set{ _effectText = value; }
	}

	public string projectName
	{
		get{ return _projectName; }
		set{ _projectName = value; }
	}

	public int Whitehouse()
	{
		return ArrayValue(requiredWhitehouse);
	}

	/**
	 * returns the capcity the project is currently providing
	 */
	public int Capacity()
	{
		return ArrayValue(capacities);
	}

	/**
	 * returns the required Points the player needs to build 
	 * this project or the upgrade of this project
	 */
	public int Points()
	{
		return ArrayValue(requiredPoints);
	}

	/**
	 * returns the count of rounds which it takes to build or upgrade
	 * the project
	 */
	public int Rounds()
	{
		return ArrayValue(buildingRounds);
	}

	/**
	 * returns the cost to build or upgrade the project
	 */
	public int Cost()
	{
		return ArrayValue(costs);
	}

	protected int ArrayValue(int[] array)
	{
		if(projectLevel >= 0 && projectLevel <= array.Length-1)
		{
			return array[projectLevel];
		}
		else
		{
			Debug.Log ("Error! Index is out of range. Array: " + array.ToString());
			return outOfRange;
		}
	}

	/**
	 * Changes the sprite of the SpriteRenderer based on the upgfrade level of the project
	 * change to private later on
	 */
	public void FillSpriteRenderer()
	{
		SpriteRenderer appearence = project.GetComponent<SpriteRenderer>();
		appearence.sortingLayerName = "Buildings";

		if(projectLevel >= 0 && projectLevel < placeholderSprites.Length)
		{
			GetComponent<SpriteRenderer>().sprite = placeholderSprites[projectLevel];
		}
		else
		{
			Debug.Log ("The given level was not in the expected range. Range :0 - " + placeholderSprites.Length +" but projectLevel was " + projectLevel);
		}
		
		if(projectLevel > 0 && projectLevel <= projectSprites.Length)
		{
			appearence.sprite = projectSprites[projectLevel - 1];
		}
		else
		{
			//Debug.Log ("The given level was not in the expected range. Range :1 - " + projectSprites.Length +" but projectLevel was " + projectLevel + " project: " + project.name);
		}
		
	}
}
