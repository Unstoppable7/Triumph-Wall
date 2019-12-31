using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina_Behaviour : Edificio
{
    [SerializeField]
    private UIDataTypes.Buildings.SO_UICocina_Data myUIData = null;

	//public enum Portions { NOTHING, SMALL, NORMAL, BIG }
	private int currentPortion = 0;
	private readonly float[] portionsVals = { 0.0f, 0.2f, 0.33f, 0.5f };
	private float foodStorage = 0.0f;

	private float foodPerEmployee = 50.0f;

    public override void SetUP()
	{
		myUIData.name = "Dinninng Hall";
		myUIData.managerID = managerID;
		myUIData.notifyDorpdownChange.AddListener( ChangePortion );

		foodStorage = 0;
		currentPortion = 0;

		currentInmigrantNum = 0;
		currentUpgrade = 0;
		maxOfUpgrades = 10;

		pricePerEmployee = 10;
        currentEmployeeNum = 0;
        maxEmployeeNum = 10;
    }

    public override void Tick()
    {
        UpdateUIData();
    }

	protected override void UpdateUIData()
    {
		myUIData.foodStorage = foodStorage;
		myUIData.alertFood = CalculateEnoughFood();
		myUIData.currentPortion = currentPortion;

		myUIData.maxOfUpgrades = maxOfUpgrades;
		myUIData.currentUpgrade = currentUpgrade;
		myUIData.currentInmigrantNum = currentInmigrantNum;
        myUIData.currentEmployeeNum = currentEmployeeNum;
        myUIData.maxEmployeeNum = maxEmployeeNum;
        myUIData.updatedValuesEvent.Invoke();
    }

	public override void ShowUI ( )
	{
		UIController.Instance.ShowEdificioUI( myUIData );
	}

	public override void BuyEmployee ( )
	{
		base.BuyEmployee();
	}

	public override void FireEmployee ( )
	{
		base.FireEmployee();

	}
	public override void Upgrade()
    {
        currentEmployeeNum++;
    }
	public override void Repair ( )
	{
		throw new System.NotImplementedException();
	}

	public override void ResetDay ( )
	{
		GenetareFood();
	}

	public override void ResetMonth ( )
	{
		throw new System.NotImplementedException();
	}

    protected override void ProcessInmigrant()
    {

    }

	protected override void SetDataFromObject()
    {
        throw new System.NotImplementedException();
    }
    protected override void UpdateDataObject()
    {
        throw new System.NotImplementedException();
    }

	public override float GetUpgradePrice ( )
	{
		//TODO from blanacefile SO
		return 100;
	}

	public override float GetRepairPrice ( )
	{
		//TODO from blanacefile SO
		return 100;
	}

	public bool FeedInmigrants ( )
	{
		if((foodStorage - currentInmigrantNum * GetPortion()) >=0.00f)
		{
			foodStorage -= currentInmigrantNum * GetPortion();
			return true;
		}
		else
		{
			return false;
		}
	}

	public float GetPortion ( )
	{
		return portionsVals[currentPortion];
	}

	private void ChangePortion(int dropIndx)
	{
		currentPortion = dropIndx;
	}
	private bool CalculateEnoughFood ( )
	{
		return (foodStorage - currentInmigrantNum * portionsVals[currentPortion]) < 0;
	}

	private void GenetareFood ( )
	{
		foodStorage += currentEmployeeNum * foodPerEmployee;
	}

	public void SetInmigrantsToFeed(int num)
	{
		currentInmigrantNum = num;
	}
}
