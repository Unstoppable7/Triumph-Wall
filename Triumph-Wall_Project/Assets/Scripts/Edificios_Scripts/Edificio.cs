using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Edificio : MonoBehaviour
{

	public enum B_Actions { UPGRADE, BUY, FIRE, REPAIR} //used by buttons

	//setted from Factory and from Manager
	protected int managerID = -1;
	//Upgrading
	protected bool canBeUpgraded = false;
	protected int maxOfUpgrades = -1;
	protected int currentUpgrade = -1;
	//Durability
	protected float maxDurability = 100;
	protected float currentDurability = 50;
	//Employees Flags
	protected bool canBuyEmployee = false;
	protected bool canFireEmployee = false;
	//Employees
	protected float pricePerEmployee = -1;
	protected int maxEmployeeNum = -1;
	protected int currentEmployeeNum = -1;
	//processing
	protected bool canProcess = false;
	protected float currentProgress = -1;
	protected float processSpeed = -1;
	//inmigrants
	protected int maxInmigrantNum = -1;
	protected int currentInmigrantNum = -1;

	public abstract void SetUP ( ); 
	public abstract void Tick ( );
	protected abstract void StartProcessInmigrant ( );

	//actions
	public abstract void Upgrade ( );
	//repairing
	public abstract void Repair ( );

	//employee its common for every Building
	public virtual float GetEmployeePrice ( )
	{
		return pricePerEmployee;
	}
	public virtual void BuyEmployee ( )
	{
		if(currentEmployeeNum < maxEmployeeNum)
		{
			currentEmployeeNum++;
		}
	}
	public virtual void FireEmployee ( )
	{
		if(currentEmployeeNum > 0)
		{
			currentEmployeeNum--;
		}
	}

	//Uses instace of UIController
	public abstract void UpdateUIData ( );
	public abstract void ShowUI ( );

	#region GETTERS
	public int GetMaxUpgrades ( ) => maxOfUpgrades;
	public int GetCurrentUpgrade ( ) => currentUpgrade;

	public float GetDurability ( ) => currentDurability / maxDurability;

	public float GetPriceEmployee ( ) => pricePerEmployee;
	public float GetMaxEmployee ( ) => maxEmployeeNum;
	public float GetCurrentEmployee ( ) => currentEmployeeNum;

	public float GetProcesSpeed ( ) => processSpeed;
	public float GetProgress ( ) => currentProgress;

	public int GetMaxInmigrants ( ) => maxInmigrantNum;
	public int GetCurrentInmigrants ( ) => currentInmigrantNum;
#endregion
}
