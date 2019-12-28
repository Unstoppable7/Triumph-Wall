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

	[SerializeField]
	private BuildingDataTypes.SO_ODIData myData = null;

	[SerializeField]
    private UIDataTypes.Buildings.SO_UIODI_Data myUIData = null;

	public override void SetUP()
    {
		processSpeed = 10.2f;
		SetDataFromObject();
		//empieza siendo 10 segundos, restando 0'2 segundos por funcionario,
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		currentProgress = processSpeed; 
    }

    public override void Tick()
	{
		if (myData.debug)
			SetDataFromObject();

		currentInmigrantNum = immigrantsToDeport.Count;
		RemoveImmigrant();

		UpdateDataObject();
        UpdateUIData();
    }

    public override void UpdateUIData()
    {
        myUIData.processSpeed = processSpeed;
        myUIData.maxEmployeeNum = maxEmployeeNum;
        myUIData.maxInmigrantNum = maxEmployeeNum;
        myUIData.currentProgress = currentProgress / processSpeed;
        myUIData.currentEmployeeNum = currentEmployeeNum;
        myUIData.currentInmigrantNum = currentInmigrantNum;
        myUIData.updatedValuesEvent.Invoke();
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

	public override void IncrementInmigrants (GameObject immigrant)
	{
		base.IncrementInmigrants();
		immigrantsToDeport.Enqueue( immigrant );
	}

    protected override void StartProcessInmigrant()
	{
		throw new System.NotImplementedException();
	}

    public override void Repair()
	{
		throw new System.NotImplementedException();
	}

    public override void Upgrade()
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

	private void RemoveImmigrant ( )
	{
		if (currentProgress <= 0.0f && immigrantsToDeport.Count > 0)
		{
			//TODO Consultar inmigrante a la hora de deportarlo para saber si esta:
			//- Normal
			//- Herido
			//- Gravemente Herido
			base.DecrementInmigrants();
			totalDeported++;
			immigrantsToDeport.Dequeue();
			currentProgress = processSpeed;
		}
		else if(immigrantsToDeport.Count > 0)
			currentProgress -= Time.deltaTime;
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
