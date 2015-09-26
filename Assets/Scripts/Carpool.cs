using UnityEngine;
using System.Collections;

public class Carpool : MonoBehaviour 
{

	public Sprite carpool1;
	public Sprite carpool2;
	public Sprite carpool3;
	public Sprite carpool4;
	public Sprite carpool5;

	private Sprite[] sprites;

	// Use this for initialization
	void Awake () 
	{
		//this.gameObject.AddComponent<SpriteRenderer>();

	}

	void Start()
	{
		sprites = new Sprite[5] { carpool1, carpool2, carpool3, carpool4, carpool5 };
	}

	public void FillSpriteRenderer(int projectLevel)
	{
		SpriteRenderer appearence = this.GetComponent<SpriteRenderer>();
		appearence.sortingLayerName = "Buildings";


		if(projectLevel > 0 && projectLevel <= sprites.Length)
		{
			appearence.sprite = sprites[projectLevel-1];
			Debug.Log (sprites[projectLevel-1]);
		}
		else
		{
			Debug.Log ("The given level was not in the expected range. Range :1 - " + sprites.Length +" but projectLevel was " + projectLevel);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
