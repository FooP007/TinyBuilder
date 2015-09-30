using UnityEngine;
using System.Collections.Generic;

abstract class Overseer : MonoBehaviour 
{
	private List<Project> _projects = new List<Project>();

	public void AddProject(Project project)
	{
		_projects.Add (project);
	}

	public void RemoveProject(Project project)
	{
		_projects.Remove(project);
	}

	public void OverseeProjects()
	{
		foreach (Project p in _projects)
		{
			p.Construct();
		}
	}
}
