using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Edificio : MonoBehaviour
{

	public enum B_Actions { UPGRADE, BUY_EMPLOYEE, FIRE_EMPLOYEE, REPAIR} //used by buttons

	//setted from Manager
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
	public virtual void BuyEmployee ( )
	{
		if(currentEmployeeNum < maxEmployeeNum)
		{
			currentEmployeeNum++;
		}
	}
	public virtual void FireEmployee ( )
	{
		if (currentEmployeeNum > 0)
		{
			currentEmployeeNum--;
		}
	}

	public virtual void IncrementInmigrants ( ) => currentInmigrantNum++;
	public virtual void DecrementInmigrants ( ) => currentInmigrantNum--;

	//Uses instace of UIController
	public abstract void UpdateUIData ( );
	public abstract void ShowUI ( );

	public abstract void ResetDay ( );
	public abstract void ResetMonth ( );


	#region GETTERS
	public virtual int GetID ( ) => managerID;
	public virtual int GetMaxUpgrades ( ) => maxOfUpgrades;
	public virtual int GetCurrentUpgrade ( ) => currentUpgrade;

	public virtual float GetDurability ( ) => currentDurability / maxDurability;

	public virtual float GetPriceEmployee ( )
	{
		return pricePerEmployee;
	}
	public virtual float GetTotalEmployeeCost ( ) => currentEmployeeNum * pricePerEmployee;
	public virtual float GetMaxEmployee ( ) => maxEmployeeNum;
	public virtual float GetCurrentEmployeeNum ( ) => currentEmployeeNum;

	public virtual float GetProcesSpeed ( ) => processSpeed;
	public virtual float GetProgress ( ) => currentProgress;

	public virtual int GetMaxInmigrants ( ) => maxInmigrantNum;
	public virtual int GetCurrentInmigrants ( ) => currentInmigrantNum;
	#endregion

	#region SETTERS
	public void SetID (int id) => managerID = id;
#endregion
}
