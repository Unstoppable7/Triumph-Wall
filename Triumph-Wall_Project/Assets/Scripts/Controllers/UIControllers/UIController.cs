using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UIDataTypes.Buildings;

public class UIController : SingletonComponent<UIController>
{

	EdificioUIController edificiosUi = null;
	public void SetUP ( )
	{
		edificiosUi = GetComponentInChildren<EdificioUIController>();
	}

	public void ShowEdificioUI (UICR_Data toShow )
	{
		//overlapping UI showing = false;
		edificiosUi.StartShowUI(toShow );
		edificiosUi.canvas.SetActive( true );
	}
}
