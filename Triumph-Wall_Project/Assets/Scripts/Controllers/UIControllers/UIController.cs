using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : SingletonComponent<UIController>
{
	bool showingBuilding = false;
	EdificioUIController edificiosUi = null;
	public void SetUP ( )
	{
		edificiosUi = GetComponentInChildren<EdificioUIController>();

	}

	public void Tick ( )
	{
		if (showingBuilding)
		{
			edificiosUi.ShowUI();
		}
	}

	public void ShowEdificioUI (ref Edificio toShow )
	{
		//overlapping UI showing = false;
		edificiosUi.StartShowing( ref toShow );
		showingBuilding = true;
	}
}
