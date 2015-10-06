using UnityEngine;
using System.Collections;

public class UpgradeWindow : MonoBehaviour 
{
	
	public TextMesh costText;
	public TextMesh ProjectNameText;

	private Project _project;

	// Use this for initialization
	void Start()
	{

	}

	private void UpdateText()
	{
		costText.text = _project.projectName;
	}
	
	public void FillProejct(Project project)
	{
		_project = project;
		UpdateText();
	}
}
