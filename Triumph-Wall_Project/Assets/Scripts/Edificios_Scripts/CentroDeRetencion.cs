using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UIDataTypes.Buildings;
using UnityEngine;

public class CentroDeRetencion : Edificio
{

	private CR_Data myData;

	public override void Repair ( )
	{
		throw new System.NotImplementedException();
	}

	public override void SetUP ( )
	{
		myData = ScriptableObject.CreateInstance<CR_Data>();
	}

	//after tick
	public override void UpdateUIData ( )
	{
		myData.durability = currentDurability / maxDurability;
		myData.progress = currentProgress;
		myData.updatedValuesEvent.Invoke();
	}

	public override void ShowUI ( )
	{
		UIController.Instance.ShowEdificioUI(myData );
	}

	public override void Tick ( )
	{
		UpdateUIData();
	}

	public override void Upgrade ( )
	{
		throw new System.NotImplementedException();
	}

	protected override void StartProcessInmigrant ( )
	{
		throw new System.NotImplementedException();
	}
}
