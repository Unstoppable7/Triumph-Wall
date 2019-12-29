using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina_Behaviour : Edificio
{

    [SerializeField]
    private UIDataTypes.Buildings.SO_UICocina_Data myUIData = null;

    public override void SetUP()
	{
		myUIData.name = "Dinninng Hall";
		myUIData.managerID = managerID;
        currentEmployeeNum = 0;
        maxEmployeeNum = 10;
        Tick();
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

    public override void Tick()
    {
        UpdateUIData();
    }

	public override void BuyEmployee ( )
	{
		base.BuyEmployee();
	}

	public override void FireEmployee ( )
	{
		base.FireEmployee();

	}

	public override void UpdateUIData()
    {
        myUIData.currentEmployeeNum = currentEmployeeNum;
        myUIData.maxEmployeeNum = maxEmployeeNum;
        myUIData.updatedValuesEvent.Invoke();
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
		throw new System.NotImplementedException();
	}

	public override void ResetMonth ( )
	{
		throw new System.NotImplementedException();
	}

    protected override void StartProcessInmigrant()
    {
        //usare esta funcion para saber cuantos immigrantes hay actualmente
        currentInmigrantNum++; //si current immigrant num es mas grande que max immigrant num se deberan contratar a mas cocineros
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
