using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitorios : Edificio
{

    public Queue<GameObject> sleepingPlaces = new Queue<GameObject>();

    public int structureCost, maintenanceCost;

    [SerializeField]
    private UIDataTypes.Buildings.SO_UIDorm_Data myUIData = null;

	public override void SetUP ( )
	{
		myUIData.name = "House";
		myUIData.managerID = managerID;
		maxInmigrantNum = 10;
		currentInmigrantNum = 5;
	}

	public override void Tick()
	{
		UpdateUIData();
    }

    public override void IncrementInmigrants(GameObject immigrant)
    {
        sleepingPlaces.Enqueue(immigrant);
        currentInmigrantNum = sleepingPlaces.Count;
    }

	public override void DecrementInmigrants(GameObject immigrant)
    {
        sleepingPlaces.Dequeue();
        currentInmigrantNum = sleepingPlaces.Count;
    }

    public override void UpdateUIData()
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
        throw new System.NotImplementedException();
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
}
