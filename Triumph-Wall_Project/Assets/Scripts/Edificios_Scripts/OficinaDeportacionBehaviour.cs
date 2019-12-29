using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OficinaDeportacionBehaviour : Edificio
{
	
	private int totalDeported = 0;
	private int normalDeported = 0;
	private int woundedDeported = 0;
	private int greavousDeported = 0;

	//TODO change from GameObject to inmigrant class
    public Queue<GameObject> immigrantsToDeport = new Queue<GameObject>();
	private int processSpeedEmployeeCap = 10;

	[SerializeField]
	private BuildingDataTypes.SO_ODIData myData = null;

	[SerializeField]
    private UIDataTypes.Buildings.SO_UIODI_Data myUIData = null;

	public override void SetUP()
    {
		myUIData.name = "Office";
		myUIData.managerID = managerID;
		SetDataFromObject();
		//empieza siendo 10.2 segundos, restando 0'2 segundos por funcionario,
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		currentProgress = processSpeed; 
    }

    public override void Tick()
	{
		if (myData.debug)
			SetDataFromObject();

		ProcessInmigrant();

		//if(!myData.debug)
		//	UpdateDataObject();

        UpdateUIData();
    }

    public override void UpdateUIData()
    {
		myUIData.maxOfUpgrades = maxOfUpgrades;
		myUIData.currentUpgrade = currentUpgrade;
        myUIData.processSpeed = processSpeed;
        myUIData.maxEmployeeNum = maxEmployeeNum;

        myUIData.maxInmigrantNum = maxInmigrantNum;
        myUIData.currentProgress = 1 - (currentProgress / processSpeed);
        myUIData.currentEmployeeNum = currentEmployeeNum;
        myUIData.currentInmigrantNum = currentInmigrantNum;
        myUIData.updatedValuesEvent.Invoke();
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

	public override void BuyEmployee ( )
	{
		base.BuyEmployee();
		//empieza siendo 10 segundos, restando 0'2 segundos por funcionario,
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		if (currentEmployeeNum + 1 <= processSpeedEmployeeCap)
			processSpeed = 10.0f - (currentEmployeeNum * 0.2f);

		maxInmigrantNum = currentEmployeeNum;
	}
	public override void FireEmployee ( )
	{
		base.FireEmployee();
		//empieza siendo 10 segundos, restando 0'2 segundos por funcionario,
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		if (currentEmployeeNum - 1 <= processSpeedEmployeeCap)
			processSpeed = 10.0f - (currentEmployeeNum * 0.2f);

		maxInmigrantNum = currentEmployeeNum;
	}


	//used by the manager of buildings
	public override void IncrementInmigrants (GameObject inmigrant = null)
	{
		if (currentInmigrantNum + 1 <= maxInmigrantNum)
		{
			base.IncrementInmigrants();
			immigrantsToDeport.Enqueue( inmigrant );
		}
	}
	public override void DecrementInmigrants (GameObject inmigrant = null)
	{
		for (int i = 0; i < currentEmployeeNum; i++)
		{
			if (currentInmigrantNum - 1 < 0) break;
			//TODO Consultar inmigrante a la hora de deportarlo para saber si esta:
			//- Normal
			//- Herido
			//- Gravemente Herido
			totalDeported++;
			base.DecrementInmigrants();
			if (immigrantsToDeport.Count > 0)
				immigrantsToDeport.Dequeue();
		}

	}

	protected override void ProcessInmigrant()
	{
		if ((currentProgress <= 0.0f && currentInmigrantNum > 0))
		{
			DecrementInmigrants();
			currentProgress = processSpeed;
		}
		else if (currentInmigrantNum > 0)
			currentProgress -= Time.deltaTime;
	}

    public override void Upgrade()
    {
		if(currentUpgrade +1 <= maxOfUpgrades)
		{
			maxEmployeeNum++;
			currentUpgrade++;
		}
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
		totalDeported = 0;
		normalDeported = 0;
		woundedDeported = 0;
		greavousDeported = 0;
	}

	public int GetTotalDeported ( )
	{
		return totalDeported;
	}
	public int GetNormalDeported ( )
	{
		return normalDeported;
	}
	public int GetWoundedDeported ( )
	{
		return woundedDeported;
	}
	public int GetGrevousDeported ( )
	{
		return greavousDeported;
	}

	protected override void SetDataFromObject ( )
	{
		totalDeported = myData.totalDeported;
		normalDeported = myData.normalDeported;
		woundedDeported = myData.woundedDeported;
		greavousDeported = myData.greavousDeported;

		maxOfUpgrades = myData.maxOfUpgrades;
		currentUpgrade = myData.currentUpgrade;
		processSpeed = myData.processSpeed;
		maxEmployeeNum = myData.maxEmployeeNum;
		maxInmigrantNum = myData.maxEmployeeNum;
		currentProgress = myData.currentProgress;
		currentEmployeeNum = myData.currentEmployeeNum;
		currentInmigrantNum = myData.currentInmigrantNum;
	}

	protected override void UpdateDataObject ( )
	{
		myData.totalDeported = totalDeported;
		myData.normalDeported = normalDeported;
		myData.woundedDeported = woundedDeported;
		myData.greavousDeported = greavousDeported;

		myData.processSpeed = processSpeed;
		myData.maxEmployeeNum = maxEmployeeNum;
		myData.maxInmigrantNum = maxEmployeeNum;
		myData.currentProgress = currentProgress;
		myData.currentEmployeeNum = currentEmployeeNum;
		myData.currentInmigrantNum = currentInmigrantNum;
	}
}
