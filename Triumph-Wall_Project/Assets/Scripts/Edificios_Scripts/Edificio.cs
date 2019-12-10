using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Edificio : MonoBehaviour
{
	public enum B_Actions { UPGRADE, BUY, FIRE, REPAIR} //used by buttons
	
	//setted from Factory and from Manager
	protected int managerID; 
	//Upgrading
	protected bool canBeUpgraded;
	protected int maxOfUpgrades;
	protected int currentUpgrade;//variation of max Variable

	//durability
	protected float maxDurability;
	protected float durability;
	
	//employee flags
	protected bool canBuyEmployee;
	protected bool canFireEmployee;
	//employees
	protected float pricePerEmployee;
	protected int maxEmployeeNum;
	protected int currentEmployeeNum;
	//processing
	protected bool canProcess;
	protected float speedPerEmployee;
	protected float processSpeed;
	protected float currentProgress;

	protected int maxInmigrantNum;
	protected int currentInmigrantNum;

	//used by ObjectFacotry to set the Values
	public abstract void SetUP ( ); 
	public abstract void Tick ( );
	protected abstract void StartProcessInmigrant ( );

	//actions
	public abstract void Upgrade ( );
	//repairing
	public abstract void Repair ( );

	//employee its common for every Building
	public float GetEmployeePrice ( )
	{
		return pricePerEmployee;
	}
	public void BuyEmployee ( )
	{
		if(currentEmployeeNum < maxEmployeeNum)
		{
			currentEmployeeNum++;
		}
	}
	public void FireEmployee ( )
	{
		if(currentEmployeeNum > 0)
		{
			currentEmployeeNum--;
		}
	}

	//Uses instace of UIController
	public abstract void ShowUI ( );

	#region GETTERS
	public int GetMaxUpgrades ( ) => maxOfUpgrades;
	public int GetCurrentUpgrade ( ) => currentUpgrade;

	public float GetCurrentDurability ( ) => durability/maxDurability;

	public float GetPriceEmployee ( ) => pricePerEmployee;
	public float GetMaxEmployee ( ) => maxEmployeeNum;
	public float GetCurrentEmployee ( ) => currentEmployeeNum;

	public float GetProcesSpeed ( ) => processSpeed;
	public float GetProgress ( ) => currentProgress;

	public int GetMaxInmigrants ( ) => maxInmigrantNum;
	public int GetCurrentInmigrants ( ) => currentInmigrantNum;
#endregion
}
