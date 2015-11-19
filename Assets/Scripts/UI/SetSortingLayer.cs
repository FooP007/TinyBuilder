using UnityEngine;
using System.Collections;

public class SetSortingLayer : MonoBehaviour 
{
	public string sortingLayer;
	public int sortingOrder;

	// Use this for initialization
	void Start () 
	{
		GetComponent<Renderer>().sortingLayerName = sortingLayer;
		GetComponent<Renderer>().sortingOrder = sortingOrder;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
