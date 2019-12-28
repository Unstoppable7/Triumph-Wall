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

	public void ShowEdificioUI (SO_UICR_Data toShow )
	{
		//overlapping UI showing = false;
		edificiosUi.StartShowUI(toShow );
		edificiosUi.canvas.SetActive( true );
	}

    public void ShowEdificioUI(SO_UIODI_Data toShow)
    {
        //overlapping UI showing = false;
        edificiosUi.StartShowUI(toShow);
        edificiosUi.canvas.SetActive(true);
    }

    public void ShowEdificioUI(UIDORM_Data toShow)
    {
        //overlapping UI showing = false;
        edificiosUi.StartShowUI(toShow);
        edificiosUi.canvas.SetActive(true);
    }

    public void HideUI ( )
	{
		edificiosUi.Hide();
	}
}
