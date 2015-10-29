using UnityEngine;
using System.Collections;

public class UpgradeWindow : MonoBehaviour 
{
	public GameObject upgradeButton;
	public GameObject closeButton;

	public TextMesh costText;
	public TextMesh projectNameText;
	public TextMesh effectText;
	public TextMesh requirementText;
	public TextMesh buildingroundsText;


	private Project _project;

	/**
	 * Updates the text of the upgradewindow based on the given project
	 * The hardcoded text should be variables so that they could be translated
	 */
	public void UpdateText()
	{
        if(_project != null)
        {
            // (_project.projectLevel != _project.projectSprites.Length -1) == the project has his maximum upgrade
            Debug.Log("projectLevel: " + _project.projectLevel + " -  projectSprites.Length: " + _project.projectSprites.Length);
            bool maximumUpgrade = (_project.projectLevel >= _project.projectSprites.Length);

            projectNameText.text = _project.projectName;
            
            if (!maximumUpgrade)
            {
                costText.text = "Costs: " + (_project.Cost() - Game.overseer.discount) + " (" + _project.Cost().ToString() + " - " + Game.overseer.discount + ")";
                buildingroundsText.text = "Buildingrounds: " + _project.Rounds();
            }
            else
            {
                costText.text = "Costs: 0";
                buildingroundsText.text = "Buildingrounds: 0";
            }

            effectText.text = "Effect: \n " + _project.effectText;
            requirementText.text = "Requires: \n" + _project.requireText;

            if (!Game.overseer.Solvent(_project) && !maximumUpgrade)
            {
                costText.color = Color.red;
                SpriteRenderer curRemd = upgradeButton.GetComponent<SpriteRenderer>();
                curRemd.color = Color.red;
            }
            else
            {
                costText.color = Color.black;
                SpriteRenderer curRemd = upgradeButton.GetComponent<SpriteRenderer>();
                curRemd.color = Color.white;
            }

            if (!_project.MetRequirements() && !maximumUpgrade)
            {
                SpriteRenderer curRemd = upgradeButton.GetComponent<SpriteRenderer>();
                curRemd.color = Color.red;
                requirementText.color = Color.red;
            }
            else
            {
                SpriteRenderer curRemd = upgradeButton.GetComponent<SpriteRenderer>();
                curRemd.color = Color.white;
                requirementText.color = Color.black;
            }
        }
       
    }

	public void Show(Project project)
	{
		_project = project;

		UpdateText();
	}
}
