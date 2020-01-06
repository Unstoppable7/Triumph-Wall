using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitorios : Edificio
{

    private Queue<Agent_Inmigrant> sleepingPlaces = new Queue<Agent_Inmigrant>();

    public int structureCost, maintenanceCost;

    [SerializeField]
    private UIDataTypes.Buildings.SO_UIDorm_Data myUIData = null;

	public override void SetUP ( )
	{
		myUIData.name = "House";
		myUIData.managerID = managerID;
		maxInmigrantNum = 10;
		currentInmigrantNum = 0;
	}

	public override void Tick()
	{
		UpdateUIData();
    }

    public override void IncrementInmigrants(Agent_Inmigrant immigrant)
    {
        sleepingPlaces.Enqueue(immigrant);
        currentInmigrantNum = sleepingPlaces.Count;
    }

	public override void DecrementInmigrants(Agent_Inmigrant immigrant)
    {
        sleepingPlaces.Dequeue();
        currentInmigrantNum = sleepingPlaces.Count;
    }

    protected override void UpdateUIData()
    {
        myUIData.currentInmigrantNum = currentInmigrantNum;
        myUIData.maxInmigrantNum = maxInmigrantNum;
        myUIData.updatedValuesEvent.Invoke();
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

    public override void Repair()
    {
        currentDurability = maxDurability;
    }

    protected override void ProcessInmigrant()
	{
		throw new System.NotImplementedException();
	}
       
    public override void Upgrade()
    {
        maxInmigrantNum += 2;
    }

    public override void ResetDay()
    {
    }

    public override void ResetMonth()
    {
        throw new System.NotImplementedException();
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

	public Agent_Inmigrant GetInmigrantToDeport ( )
	{
		return sleepingPlaces.Dequeue();
	}
}
